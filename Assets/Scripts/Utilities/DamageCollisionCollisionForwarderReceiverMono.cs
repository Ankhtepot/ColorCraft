﻿using UnityEngine;

namespace Utilities
{
    public abstract class DamageCollisionCollisionForwarderReceiverMono : MonoBehaviour, IDamageCollisionForwarderReceiver
    {
        public virtual void OnCollisionReceived(Collision other)
        {
        
        }
    }
}