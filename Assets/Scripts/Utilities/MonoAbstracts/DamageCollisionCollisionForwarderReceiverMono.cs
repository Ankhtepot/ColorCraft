using UnityEngine;
using Utilities.Interfaces;

namespace Utilities.MonoAbstracts
{
    public abstract class DamageCollisionCollisionForwarderReceiverMono : MonoBehaviour, IDamageCollisionForwarderReceiver
    {
        public virtual void OnCollisionReceived(Collision other)
        {
        
        }
    }
}