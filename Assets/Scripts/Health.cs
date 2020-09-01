using System;
using System.Collections;
using System.Collections.Generic;
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
                damageVfx.Play();
            }

            StartCoroutine(DamageCooldown());

            if (hitpoints <= 0)
            {
                DestroyElement();
            }
        }
    }

    private void DestroyElement()
    {
        print("Element destroyed");
        if (deathVfx != null)
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity).Play();
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
