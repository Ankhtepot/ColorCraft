using System.Collections;
using Controllers;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class Health : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private int hitpoints;
        [SerializeField] private float damageableCooldown = 0.25f;
        [SerializeField] private bool isDamageable = true;
        [SerializeField] private ParticleSystem damageVfx;
        [SerializeField] private ParticleSystem deathVfx;
        private Color mainMaterialColor;
#pragma warning restore 649

        private void Start()
        {
            SetParticleEffects();
        }

        private void SetParticleEffects()
        {
            mainMaterialColor = GetComponentInChildren<Renderer>().material.color;
        
            if (damageVfx != null)
            {
                var mainModule = damageVfx.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(mainMaterialColor);
            }
        }

        public void OnCollisionReceived(string otherTag, Vector3 position)
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

        private void PopEffect(ParticleSystem effect)
        {
            if (deathVfx != null)
            {
                var vfx = Instantiate(effect, transform.position, Quaternion.identity);
                vfx.transform.parent = transform.parent;
        
                var mainModule = vfx.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(mainMaterialColor);
                vfx.Play();
            }
        }

        private void DestroyElement()
        {
            if (deathVfx != null)
            {
                PopEffect(deathVfx);
            }
        
            FindObjectOfType<BuiltElementsStoreController>().RemoveElement(gameObject);
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
}
