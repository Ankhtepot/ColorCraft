using System.Collections;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class Health : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private int hitpoints;
    [SerializeField] private float damageableCooldown = 0.25f;
    [SerializeField] private bool isDamageable = true;
    [SerializeField] private ParticleSystem damageVfx;
    [SerializeField] private ParticleSystem deathVfx;
#pragma warning restore 649

    private void Start()
    {
        SetParticleEffects();
    }

    private void SetParticleEffects()
    {
        var newColor = GetComponentInChildren<Renderer>().material.color;
        
        if (damageVfx != null)
        {
            var mainModule = damageVfx.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(newColor);
        }
        
        if (deathVfx != null)
        {
            var mainModule = deathVfx.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(newColor);
        }
    }

    public void OnCollisionReceived()
    {
        if (isDamageable)
        {
            isDamageable = false;
            
            hitpoints--;

            if (damageVfx != null)
            {
                PopEffect(damageVfx);
            }

            StartCoroutine(DamageCooldown());

            if (hitpoints <= 0)
            {
                DestroyElement();
            }
        }
    }

    private void PopEffect(ParticleSystem effect)
    {
        var vfx = Instantiate(damageVfx, transform.position, Quaternion.identity);
        vfx.transform.parent = transform;
        vfx.Play();
    }

    private void DestroyElement()
    {
        if (deathVfx != null)
        {
            PopEffect(deathVfx);
        }
        
        FindObjectOfType<BuiltElementsStore>().RemoveElement(gameObject);
        Destroy(gameObject);
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageableCooldown);

        isDamageable = true;
    }

    public int GetCurrentHitpoints()
    {
        return hitpoints;
    }

    public void SetHitpoints(int value)
    {
        hitpoints = value;
    }
}
