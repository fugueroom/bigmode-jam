using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Flammable : MonoBehaviour
{
    public float HP;
    public ParticleSystem FireVFX;
    public Renderer[] RendereredComponents;
    private MobSpawner mobSpawner;

    public float InitialFireScale;
    public float FinalFireScale;

    private float damageCounter = 0.0f;
    private float fireScaleRange;

    private void Start()
    {
        mobSpawner = FindFirstObjectByType<MobSpawner>();
        FireVFX.gameObject.SetActive(false);
        fireScaleRange = FinalFireScale - InitialFireScale;
    }

    bool burned;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            if (FireVFX.gameObject.activeSelf == false)
            {
                FireVFX.gameObject.SetActive(true);
                FireVFX.Play();
            }

            float damage = 1.0f;

            // Give an advantage to those last few brave mobbers
            if (mobSpawner != null && mobSpawner.CurrentMobCount < 4)
            {
                damage = 3.0f;
            }

            damageCounter += damage;

            // scale the fire based off of the percentage of damage to HP
            // scale = targetScale - initialScale (range 1)
            float dmgPercentage = damageCounter / HP;
            Vector3 targetFireScale = new Vector3(InitialFireScale + fireScaleRange * dmgPercentage,
                InitialFireScale + fireScaleRange * dmgPercentage,
                InitialFireScale + fireScaleRange * dmgPercentage);

            FireVFX.gameObject.transform.DOScale(targetFireScale, 0.1f);

            if (damageCounter > HP && ! burned)
            {
                burned = true;

                for (int i = 0; i < RendereredComponents.Length; i++)
                {
                    RendereredComponents[i].enabled = false;
                }

                if (GetComponent<Tank>() != null)
                {
                    GetComponent<Tank>().enabled = false;
                }

                if (GetComponentInChildren<NavMeshObstacle>() != null)
                {
                    GetComponentInChildren<NavMeshObstacle>().enabled = false;
                }

                FireVFX.gameObject.transform.DOScale(0f, 2f).OnComplete(() => FireVFX.Stop());
            }
        }
    }
}
