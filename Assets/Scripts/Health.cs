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
    [SerializeField] private ParticleSystem deathVfx;
#pragma warning restore 649

    public void OnCollisionReceived()
    {
        if (isDamageable)
        {
            isDamageable = false;
            
            hitpoints--;

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
}
