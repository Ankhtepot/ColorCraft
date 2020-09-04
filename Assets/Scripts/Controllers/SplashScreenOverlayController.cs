using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class SplashScreenOverlayController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject content;
        [SerializeField] private Animator animator;
#pragma warning restore 649

        private void Awake()
        {
            content.SetActive(true);
        }

        /// <summary>
        /// Run from TerrainSpawner OnSpawningFinished
        /// </summary>
        public void OnSpawningTerrainFinished()
        {
            animator.SetTrigger(Strings.Swap);
        }
    
        /// <summary>
        /// Run from GameController OnGameLoopStarted
        /// </summary>
        public void OnGameLoopStarted()
        {
            animator.SetTrigger(Strings.Hide);
        } 
    }
}
