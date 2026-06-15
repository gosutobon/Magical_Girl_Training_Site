using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        GameObject xrOrigin = GameObject.Find("XR Origin");

        if (xrOrigin != null)
        {
            xrOrigin.transform.SetPositionAndRotation(
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
    }
}