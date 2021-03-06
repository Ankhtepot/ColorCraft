﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Controllers;
using Extensions;
using UnityEngine;
using Utilities;
using Utilities.MonoAbstracts;
using static Utilities.SurroundingElementInfo;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class BuildElementLifeCycle : DamageCollisionCollisionForwarderReceiverMono
    {
#pragma warning disable 649
        [SerializeField] private int hitpoints;
        [SerializeField] private float damageableCooldown = 0.25f;
        [SerializeField] private float healingCooldown = 0.25f;
        [SerializeField] private ParticleSystem damageVfx;
        [SerializeField] private ParticleSystem pointOfDamage;
        [SerializeField] private ParticleSystem deathVfx;
        [Header("Black is default, means, will not be used")]
        public Color AlternativeVfxColor;
        public bool IsDetached
        {
            set => ManageDetached(value);
        }

        public int Hitpoints
        {
            get => hitpoints;
            set => hitpoints = value;
        }

        private int originalHitpoints;
        private bool isDamageable = true;
        private bool isHealable = true;
        [SerializeField] private bool isDetached;
        private Color mainMaterialColor;
        private Rigidbody rigidBody;
        private Collider elementCollider;
        private Collider bodyCollider;
        [HideInInspector] public BuildElement ownBuildElement;
#pragma warning restore 649

        private void Start()
        {
            SetParticleEffects();
            originalHitpoints = Hitpoints;
            bodyCollider = GetComponentInChildren<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionReceived(other);
        }

        public override void OnCollisionReceived(Collision other)
        {
            ManageCollisionWithHarmful(other);
            
            ManageCollisionWithHealing(other);
        }

        private void ManageCollisionWithHealing(Collision other)
        {
            if (!isHealable || !other.gameObject.CompareTag(Strings.Healing) || Hitpoints >= originalHitpoints) return;

            isHealable = false;
            print("healing placeholder");

            StartCoroutine(HealingCooldown());
        }

        private void ManageCollisionWithHarmful(Collision other)
        {
            if (!isDamageable || isDetached || !other.gameObject.CompareTag(Strings.Harmful)) return;
            
            if (pointOfDamage != null)
            {
                pointOfDamage.transform.position = other.transform.position;
                pointOfDamage.Play();
            }

            isDamageable = false;
            
            hitpoints--;

            if (damageVfx != null)
            {
                damageVfx.Play();
            }

            if (hitpoints <= 0)
            {
                DestroyElement();
            }
                
            StartCoroutine(DamageCooldown());
        }

        private void SetParticleEffects()
        {
            mainMaterialColor = AlternativeVfxColor.a.Equals(0f) 
                ? GetComponentInChildren<Renderer>().material.color 
                : AlternativeVfxColor;

            if (damageVfx == null) return;
            
            var mainModule = damageVfx.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(mainMaterialColor);
        }
        
        [SuppressMessage("ReSharper", "BitwiseOperatorOnEnumWithoutFlags")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void ManageDetached(bool detached)
        {
            if (isDetached == detached) return;

            if (detached)
            {
                // print($"REMOVING {transform.position} from elementStore.");
                BuiltElementsStoreController.RemoveElementWithPosition(transform.position);

                rigidBody = gameObject.AddComponent<Rigidbody>();
                rigidBody.angularDrag = 0;
                rigidBody.mass = 50;
                rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionZ;

                if (!elementCollider)
                {
                    elementCollider = gameObject.AddComponent<BoxCollider>();
                    var NinetyNinePercentOfCubicScaleDimension = transform.localScale.x - transform.localScale.x / 100f;
                    ((BoxCollider) elementCollider).size = new Vector3(
                        NinetyNinePercentOfCubicScaleDimension,
                        NinetyNinePercentOfCubicScaleDimension,
                        NinetyNinePercentOfCubicScaleDimension); 
                }

                elementCollider.enabled = true;
                
                //Disable body collider for falling
                bodyCollider.enabled = false;

                StartCoroutine(CheckVelocityTillZero());
            }
            else
            {
                if (!ownBuildElement)
                {
                    ownBuildElement = GetComponent<BuildElement>();
                }
                
                var currentPosition = transform.position;
                transform.position = currentPosition.ToVector3Int();
                // print($"ADDING {transform.position} to elementStore.");
                BuiltElementsStoreController.AddElement(ownBuildElement);
                ownBuildElement.SetConnections();
            }
            
            isDetached = detached;
        }
        
        

        private IEnumerator CheckVelocityTillZero()
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitWhile(() => rigidBody.velocity != Vector3.zero && ElementBellowIsNotDetached(rigidBody.transform.position));
            
            Destroy(rigidBody);
            elementCollider.enabled = false;
            bodyCollider.enabled = true;
                
            IsDetached = false;
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

            if (!ownBuildElement)
            {
                ownBuildElement = GetComponent<BuildElement>();
            }
            
            BuiltElementsStoreController.RemoveElement(ownBuildElement);
            ownBuildElement.Disconnect();

            var buildBaseFor = ownBuildElement.BuildBaseOn;
            
            
            DetachedElementsChecker.CheckForDetachedElements(ownBuildElement);
            Destroy(gameObject);
        }

        private IEnumerator DamageCooldown()
        {
            yield return new WaitForSeconds(damageableCooldown);

            isDamageable = true;
        }

        private IEnumerator HealingCooldown()
        {
            yield return new WaitForSeconds(healingCooldown);

            isHealable = true;
        }
    }
}
