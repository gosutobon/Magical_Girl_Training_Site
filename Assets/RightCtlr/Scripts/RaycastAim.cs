using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastAim : MonoBehaviour
{
    [Header("主相機")]
    public Camera xrCamera;

    [Header("Editor滑鼠瞄準")]
    public bool useMouseAim = true;

    [Header("最大距離")]
    public float maxDistance = 200f;

    [Header("碰撞圖層")]
    public LayerMask targetLayer = ~0;

    public Vector3 GetAimPoint()
    {
        if (xrCamera == null)
            return Vector3.zero;

        Ray ray;

        if (useMouseAim && Mouse.current != null)
        {
            ray = xrCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue());
        }
        else
        {
            ray = new Ray(
                xrCamera.transform.position,
                xrCamera.transform.forward);
        }

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                maxDistance,
                targetLayer))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * maxDistance;
    }

    void Update()
    {
        if (xrCamera == null)
            return;

        Ray ray;

        if (useMouseAim && Mouse.current != null)
        {
            ray = xrCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue());
        }
        else
        {
            ray = new Ray(
                xrCamera.transform.position,
                xrCamera.transform.forward);
        }

        Debug.DrawRay(
            ray.origin,
            ray.direction * maxDistance,
            Color.green);
    }
}