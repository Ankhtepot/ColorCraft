using UnityEngine;
using Utilities;
using Utilities.MonoAbstracts;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class BuildElement : BuildElementMono
    {
#pragma warning disable 649
        public BuildPosition CanBeBuilt = BuildPosition.AllSides;
        public BuildPosition BuildBaseFor = BuildPosition.AllSides;
        public string Description;
        public Sprite storeSprite;
#pragma warning restore 649
    }
}
