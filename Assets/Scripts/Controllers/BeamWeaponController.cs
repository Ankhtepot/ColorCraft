using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class BeamWeaponController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameController gameController;
        [SerializeField] private ParticleSystem beamVFX;
#pragma warning restore 649

        private void Update()
        {
            HandleShooting();
        }

        private void HandleShooting()
        {
            if (gameController.InputEnabled && gameController.GameMode == GameMode.Beam && Input.GetMouseButton(0))
            {
                beamVFX.Play();
            }
            else if (beamVFX.isPlaying)
            {
                beamVFX.Stop();
            }
        }
    }
}
