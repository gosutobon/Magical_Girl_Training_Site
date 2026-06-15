using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public Target[] targets;

    public void RespawnAllTargets()
    {
        foreach (Target target in targets)
        {
            target.gameObject.SetActive(true);
            print("Terget");
        }
    }
}