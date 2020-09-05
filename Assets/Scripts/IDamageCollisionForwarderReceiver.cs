using UnityEngine;

public interface IDamageCollisionForwarderReceiver
{
    void OnCollisionReceived(Collision other);
}