using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using System.Collections;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class QuestControllerSwap : MonoBehaviour
{
    [Header("原本控制器模型")]
    public GameObject originalControllerModel;

    [Header("新模型")]
    public GameObject newModel;

    [Header("切換特效")]
    public GameObject spawnEffectPrefab;

    [Header("射擊系統")]
    public Shoot shooter;

    [Header("切換延遲")]
    public float spawnDelay = 1f;

    [Header("控制器")]
    public XRNode controllerNode = XRNode.RightHand;

    [Header("震動強度")]
    [Range(0, 1)]
    public float hapticAmplitude = 0.7f;

    [Header("震動時間")]
    public float hapticDuration = 0.2f;

    private XRInputDevice device;

    private bool lastSecondaryState;
    private bool lastTriggerState;

    private bool hasSwapped;
    private bool isPlaying;

    void Start()
    {
        InitializeDevice();

        if (newModel != null)
            newModel.SetActive(false);

        if (originalControllerModel != null)
            originalControllerModel.SetActive(true);
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

        if (device.TryGetFeatureValue(XRCommonUsages.secondaryButton, out secondaryButton))
        {
            if (secondaryButton && !lastSecondaryState)
            {
                StartSwap();
            }

            lastSecondaryState = secondaryButton;
        }
    }

    void CheckTriggerInput()
    {
        bool triggerButton;

        if (device.TryGetFeatureValue(XRCommonUsages.triggerButton, out triggerButton))
        {
            if (triggerButton && !lastTriggerState)
            {
                if (newModel != null && newModel.activeSelf)
                {
                    PlayTriggerEffect();
                }
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
            StartSwap();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (newModel != null && newModel.activeSelf)
            {
                PlayTriggerEffect();
            }
            
        }
        
    }

    void StartSwap()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayEffectAndSwap());
        }
    }

    IEnumerator PlayEffectAndSwap()
    {
        isPlaying = true;

        hasSwapped = !hasSwapped;

        SendHaptic();

        if (spawnEffectPrefab != null)
        {
            Instantiate(
                spawnEffectPrefab,
                transform.position,
                transform.rotation);
        }

        yield return new WaitForSeconds(spawnDelay);

        if (originalControllerModel != null)
            originalControllerModel.SetActive(!hasSwapped);

        if (newModel != null)
            newModel.SetActive(hasSwapped);

        isPlaying = false;
    }

    void PlayTriggerEffect()
    {
        if (shooter != null)
        {
            shooter.Fire();
        }
        SendHaptic();
    }

    void SendHaptic()
    {
        if (device.isValid)
        {
            device.SendHapticImpulse(
                0,
                hapticAmplitude,
                hapticDuration);
        }
    }
}