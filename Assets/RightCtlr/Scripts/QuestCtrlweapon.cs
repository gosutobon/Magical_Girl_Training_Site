using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class QuestControllerSwap : MonoBehaviour
{
    public GameObject originalControllerModel;
    public GameObject newModel;

    public Shoot shooter;

    public XRNode controllerNode = XRNode.RightHand;

    private XRInputDevice device;

    private bool lastSecondaryState;
    private bool lastTriggerState;

    private bool hasSwapped;

    void Start()
    {
        InitializeDevice();

        if (newModel != null)
            newModel.SetActive(false);
    }

    void Update()
    {
        if (!device.isValid)
            InitializeDevice();

        CheckXRInput();
        CheckTriggerInput();
        CheckKeyboardInput();
    }

    void InitializeDevice()
    {
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    void CheckXRInput()
    {
        bool secondaryButton;

        if (device.TryGetFeatureValue(
                XRCommonUsages.secondaryButton,
                out secondaryButton))
        {
            if (secondaryButton && !lastSecondaryState)
            {
                ToggleWeapon();
            }

            lastSecondaryState = secondaryButton;
        }
    }

    void CheckTriggerInput()
    {
        bool triggerButton;

        if (device.TryGetFeatureValue(
                XRCommonUsages.triggerButton,
                out triggerButton))
        {
            if (triggerButton && !lastTriggerState)
            {
                FireWeapon();
            }

            lastTriggerState = triggerButton;
        }
    }

    void CheckKeyboardInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            ToggleWeapon();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            FireWeapon();
        }
    }

    void ToggleWeapon()
    {
        hasSwapped = !hasSwapped;

        if (originalControllerModel != null)
            originalControllerModel.SetActive(!hasSwapped);

        if (newModel != null)
            newModel.SetActive(hasSwapped);
    }

    void FireWeapon()
    {
        if (newModel != null &&
            newModel.activeSelf &&
            shooter != null)
        {
            shooter.Fire();
        }
    }
}