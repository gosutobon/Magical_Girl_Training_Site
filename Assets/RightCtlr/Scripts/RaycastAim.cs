using UnityEngine;
using UnityEngine.InputSystem; // 使用新版 Input System 獲取滑鼠位置

public class RaycastAim : MonoBehaviour
{
    [Header("VR相機 / 主相機")]
    public Camera xrCamera;

    [Header("最大距離")]
    public float maxDistance = 200f;

    [Header("碰撞圖層 (避免射線打到自己或子彈)")]
    public LayerMask targetLayer = ~0; // 預設為 All Layers

    public Vector3 GetAimPoint()
    {
        if (xrCamera == null)
        {
            Debug.LogWarning("XR Camera未指定");
            return Vector3.zero;
        }

        Ray ray;

        // 💡 核心改動：如果偵測到滑鼠（PC 模式），射線從滑鼠游標位置射出
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            ray = xrCamera.ScreenPointToRay(mousePosition);
        }
        else
        {
            // VR 模式：維持原本從相機中心往前射
            ray = new Ray(xrCamera.transform.position, xrCamera.transform.forward);
        }

        RaycastHit hit;

        // 加入 targetLayer 確保不會射到玩家手把或子彈自己
        if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
        {
            return hit.point;
        }

        // 如果沒指到任何東西，就回傳射線終點
        return ray.origin + ray.direction * maxDistance;
    }

    void Update()
    {
        if (xrCamera == null) return;

        // 讓 Debug 射線在場景視窗中也能動態跟隨滑鼠，方便除錯
        Ray ray;
        if (Mouse.current != null)
        {
            ray = xrCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
        else
        {
            ray = new Ray(xrCamera.transform.position, xrCamera.transform.forward);
        }

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
    }
}