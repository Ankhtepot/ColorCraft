using System.Collections;
using UnityEngine;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class GunController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private int bulletSpeed = 20;
        [SerializeField] private float canShootDelay;
        [SerializeField] private bool canShoot = true;
        [SerializeField] private GameController gameController;
        [SerializeField] private GameObject bulletPrefab;
        
#pragma warning restore 649

        private void Update()
        {
            HandleShooting();
        }

        private void HandleShooting()
        {
            if (gameController.InputEnabled && canShoot && gameController.GameMode == GameMode.Beam && Input.GetMouseButton(0))
            {
                canShoot = false;
                
                var bullet = Instantiate(bulletPrefab, transform.position, transform.parent.rotation);
                bullet.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

                StartCoroutine(CanShootCooldown());
            }
        }

        private IEnumerator CanShootCooldown()
        {
            yield return new WaitForSeconds(canShootDelay);

            canShoot = true;
        }
    }
}
