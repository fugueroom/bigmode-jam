using UnityEngine;

public class Flammable : MonoBehaviour
{
    public float HP;

    private float damageCounter = 0.0f;
    private Renderer _renderer;
    private Color currentColor;

    private void Start()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
        currentColor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            damageCounter += 1f;
            currentColor.r += 1f;
            _renderer.material.color = currentColor;

            if (damageCounter > HP)
            {
                Destroy(gameObject);
            }
        }
    }
}
