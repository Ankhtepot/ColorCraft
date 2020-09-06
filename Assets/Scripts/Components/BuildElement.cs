using UnityEngine;
using Utilities.Enumerations;
using Utilities.MonoAbstracts;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class BuildElement : BuildElementMono
    {
#pragma warning disable 649
        public BuildPosition CanBeBuilt = BuildPosition.AllSides;
        public BuildPosition BuildBaseOn = BuildPosition.AllSides;
        public string Description;
        public Sprite StoreSprite;
#pragma warning restore 649
        
        private void Start()
        {
            CheckSetup();
        }

        private void CheckSetup()
        {
            if (CanBeBuilt == BuildPosition.None)
            {
                Debug.LogError($"BuildElement \"{gameObject.name}\" has set {nameof(CanBeBuilt)}:{BuildPosition.None} which renders this element invalid");
            }

            if (StoreSprite == null)
            {
                Debug.LogWarning($"BuildElement \"{gameObject.name}\" has set no {nameof(StoreSprite)}.");
            }
        }
    }
}
