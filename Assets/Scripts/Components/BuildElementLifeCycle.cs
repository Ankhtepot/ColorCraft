using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Controllers;
using Extensions;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class BuildElementLifeCycle : DamageCollisionCollisionForwarderReceiverMono
    {
#pragma warning disable 649
        [SerializeField] private int hitpoints;
        [SerializeField] private float damageableCooldown = 0.25f;
        [SerializeField] private bool isDamageable = true;
        [SerializeField] private GameObject Body;
        [SerializeField] private ParticleSystem damageVfx;
        [SerializeField] private ParticleSystem pointOfDamage;
        [SerializeField] private ParticleSystem deathVfx;
        private Color mainMaterialColor;
        
        [SerializeField] private bool isDetached;
        public bool IsDetached
        {
            get => isDetached;
            set => ManageDetached(value);
        }
#pragma warning restore 649

        private void Start()
        {
            SetParticleEffects();
        }
        
        public override void OnCollisionReceived(Collision other)
        {
            print($"Collided with: {other.gameObject.name} | is ground: {other.gameObject.GetComponentInParent<TerrainElement>() != null} | is buildElement: {other.gameObject.GetComponentInParent<BuildElement>() != null}");
            if (isDamageable && !isDetached && other.gameObject.CompareTag(Strings.Harmful))
            {
                if (pointOfDamage != null)
                {
                    // print($"{other.GetContact(0).normal}");
                    pointOfDamage.transform.position = other.transform.position;
                    pointOfDamage.Play();
                }

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

            //TODO move collision detection to object itself, not body
            if (isDetached && (other.gameObject.GetComponentInParent<TerrainElement>() ||
                               other.gameObject.GetComponentInParent<BuildElement>()))
            {
                // TODO: rework whole body collision forwarding to main object or destroy both collider and rigidbody from main object  
                Destroy(gameObject.GetComponent<Rigidbody>());
                var currentPosition = transform.position;
                transform.position = currentPosition.ToVector3Int();
                FindObjectOfType<BuiltElementsStoreController>().AddElement(gameObject);
                
                isDetached = false;
            }
        }

        private void SetParticleEffects()
        {
            mainMaterialColor = GetComponentInChildren<Renderer>().material.color;

            if (damageVfx == null) return;
            
            var mainModule = damageVfx.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(mainMaterialColor);
        }
        
        [SuppressMessage("ReSharper", "BitwiseOperatorOnEnumWithoutFlags")]
        private void ManageDetached(bool detached)
        {
            if (isDetached == detached) return;

            if (detached)
            {
                //TODO add rigidbody and collider (0.99x3!) to object, not body
                FindObjectOfType<BuiltElementsStoreController>().RemoveElementWithPosition(transform.position);
            
                var cube = GetComponentInChildren<Collider>().gameObject;
                var rigidBody = cube.AddComponent<Rigidbody>();
                rigidBody.angularDrag = 0;
                rigidBody.mass = 50;
                rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionZ;
            }
            
            isDetached = detached;
        }

        private void PopEffect(ParticleSystem effect)
        {
            if (deathVfx == null) return;
            
            var vfx = Instantiate(effect, transform.position, Quaternion.identity);
            vfx.transform.parent = transform.parent;
        
            var mainModule = vfx.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(mainMaterialColor);
            vfx.Play();
        }

        private void DestroyElement()
        {
            if (deathVfx != null)
            {
                PopEffect(deathVfx);
            }

            var storeController = FindObjectOfType<BuiltElementsStoreController>();
            storeController.RemoveElement(gameObject);
            storeController.CheckForDetachedElements(transform.position.ToVector3Int());
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
