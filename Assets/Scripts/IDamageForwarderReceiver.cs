using UnityEngine;

public interface IDamageForwarderReceiver
{
    void OnCollisionReceived(Collision other);
}