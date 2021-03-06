﻿using System.Collections;
using UnityEngine;
using Utilities.MonoAbstracts;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class BulletController : DamageCollisionCollisionForwarderReceiverMono
    {
#pragma warning disable 649
        [SerializeField] private float bulletLifetime = 0.2f;
#pragma warning restore 649

        private void Start()
        {
            StartCoroutine(DestroyBulletAfterInterval());
        }

        public override void OnCollisionReceived(Collision other)
        {
            DestroyBullet();
        }

        private IEnumerator DestroyBulletAfterInterval()
        {
            yield return new WaitForSeconds(bulletLifetime);
            
            DestroyBullet();
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}
