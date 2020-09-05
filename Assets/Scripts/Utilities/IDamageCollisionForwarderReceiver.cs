using UnityEngine;

namespace Utilities
{
    public interface IDamageCollisionForwarderReceiver
    {
        void OnCollisionReceived(Collision other);
    }
}