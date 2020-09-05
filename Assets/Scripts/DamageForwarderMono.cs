using UnityEngine;

public abstract class DamageForwarderMono : MonoBehaviour, IDamageForwarderReceiver
{
    public virtual void OnCollisionReceived(Collision other)
    {
        
    }
}