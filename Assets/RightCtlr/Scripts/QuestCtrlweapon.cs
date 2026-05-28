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

    [Header("新的模型")]
    public GameObject newModel;

    [Header("生成特效 Prefab")]
    public GameObject spawnEffectPrefab;

    [Header("特效後延遲幾秒生成模型")]
    public float spawnDelay = 1.0f;

    [Header("控制器")]
    public XRNode controllerNode = XRNode.RightHand;

    [Header("震動強度")]
    [Range(0, 1)]
    public float hapticAmplitude = 0.7f;

    [Header("震動時間")]
    public float hapticDuration = 0.2f;

    private XRInputDevice device;

    private bool lastSecondaryState = false;
    private bool lastTriggerState = false;

    private bool hasSwapped = false;
    private bool isPlaying = false;

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
        {
            InitializeDevice();
        }

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
                // 只有新模型啟用時才觸發
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

        // G鍵：切換模型
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            StartSwap();
        }

        // Q鍵：模擬板機
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            // 只有新模型啟用時才觸發
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

        // 切換時特效
        if (spawnEffectPrefab != null)
        {
            Instantiate(
                spawnEffectPrefab,
                transform.position,
                transform.rotation
            );
        }

        // 等待特效
        yield return new WaitForSeconds(spawnDelay);

        if (originalControllerModel != null)
        {
            originalControllerModel.SetActive(!hasSwapped);
        }

        if (newModel != null)
        {
            newModel.SetActive(hasSwapped);
        }

        isPlaying = false;
    }

    void PlayTriggerEffect()
    {
        if (spawnEffectPrefab != null)
        {
            Instantiate(
                spawnEffectPrefab,
                transform.position,
                transform.rotation
            );
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
                hapticDuration
            );
        }
    }
}