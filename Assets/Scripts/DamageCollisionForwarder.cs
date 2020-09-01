using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class DamageCollisionForwarder : MonoBehaviour
{
#pragma warning disable 649
    private Health target;
    [SerializeField] public UnityEvent OnCollision;
#pragma warning restore 649

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(Strings.Harmful))
        {
            OnCollision?.Invoke();
        }
    }
}
