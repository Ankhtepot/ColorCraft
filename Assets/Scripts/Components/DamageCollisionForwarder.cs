using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class DamageCollisionForwarder : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private DamageCollisionCollisionForwarderReceiverMono target;
#pragma warning restore 649

        private void Start()
        {
            Initialize();
        }

        private void OnCollisionEnter(Collision other)
        {
            target.OnCollisionReceived(other);
        }
        
        private void Initialize()
        {
            if (target) return;
            
            target = GetComponentInParent<DamageCollisionCollisionForwarderReceiverMono>();
                
            if (!target)
            {
                Debug.LogWarning("DamageCollisionForwarder found no receiver in a parent.");
            }
        }
    }
}
