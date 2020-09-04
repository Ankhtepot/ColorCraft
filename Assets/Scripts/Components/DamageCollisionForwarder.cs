using UnityEngine;
using UnityEngine.Events;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class DamageCollisionForwarder : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] Health target;
#pragma warning restore 649

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag(Strings.Harmful))
            {
                target.OnCollisionReceived(other.tag, other.transform.position);
            }
        }
    }
}
