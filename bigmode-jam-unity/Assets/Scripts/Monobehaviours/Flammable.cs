using UnityEngine;
using DG.Tweening;

public class Flammable : MonoBehaviour
{
    public float HP;
    public ParticleSystem FireVFX;

    private float damageCounter = 0.0f;
    private Renderer _renderer;
    private Color currentColor;
    private float fireScaleRange;
    private Vector3 initialFireScale;

    private void Start()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
        currentColor = _renderer.material.color;
        FireVFX.gameObject.SetActive(false);
        initialFireScale = FireVFX.transform.localScale;
        fireScaleRange = 1f - FireVFX.transform.localScale.x;
    }

    bool burned;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            if (FireVFX.gameObject.activeSelf == false)
            {
                FireVFX.gameObject.SetActive(true);
                FireVFX.Play();
            }

            damageCounter += 1f;

            // scale the fire based off of the percentage of damage to HP
            // scale = targetScale - initialScale (range 1)
            float dmgPercentage = damageCounter / HP;
            Vector3 targetFireScale = new Vector3(initialFireScale.x + fireScaleRange * dmgPercentage,
                initialFireScale.y + fireScaleRange * dmgPercentage,
                initialFireScale.z + fireScaleRange * dmgPercentage);

            FireVFX.gameObject.transform.DOScale(targetFireScale, 0.1f);
            //currentColor.r += 5f / HP;
            //_renderer.material.color = currentColor;

            if (damageCounter > HP && ! burned)
            {
                burned = true;
                _renderer.enabled = false;
                // remove later
                GetComponentInChildren<Tank>().gameObject.SetActive(false);
                FireVFX.gameObject.transform.DOScale(0f, 2f);
                //Destroy(gameObject);
            }
        }
    }
}
