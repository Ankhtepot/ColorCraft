using UnityEngine;

namespace Utilities.Interfaces
{
    public interface IDamageCollisionForwarderReceiver
    {
        void OnCollisionReceived(Collision other);
    }
}