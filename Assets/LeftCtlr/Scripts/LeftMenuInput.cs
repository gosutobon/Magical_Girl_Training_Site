using UnityEngine;
using UnityEngine.XR;

public class LeftHandMenu : MonoBehaviour
{
    public MenuManager menuManager;

    private InputDevice leftHand;

    private bool lastY;

    void Start()
    {
        leftHand =
            InputDevices.GetDeviceAtXRNode(
                XRNode.LeftHand);
    }

    void Update()
    {
        if (!leftHand.isValid)
        {
            leftHand =
                InputDevices.GetDeviceAtXRNode(
                    XRNode.LeftHand);
        }

        bool yButton;

        if (leftHand.TryGetFeatureValue(
                CommonUsages.primaryButton,
                out yButton))
        {
            if (yButton && !lastY)
            {
                menuManager.ToggleMenu();
            }

            lastY = yButton;
        }
    }
}