using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject flyEffectPrefab;
    public GameObject hitEffectPrefab;
    

    private GameObject flyEffect;

    void Start()
    {
        if (flyEffectPrefab != null)
        {
            flyEffect =
                Instantiate(
                    flyEffectPrefab,
                    transform);

            flyEffect.transform.localPosition =
                Vector3.zero;
        }
    }

    private void OnCollisionEnter(
        Collision collision)
    {
        Vector3 hitPoint =
            collision.contacts[0].point;

        if (hitEffectPrefab != null)
        {
            GameObject effect =
                Instantiate(
                    hitEffectPrefab,
                    hitPoint,
                    Quaternion.identity);

            Destroy(effect, 3f);
        }

        Target target =
            collision.gameObject.GetComponent<Target>();

        if (target != null)
        {
            target.HideTarget();
        }

        Destroy(gameObject);
    }
}