using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using Utilities;
using Utilities.Enumerations;
using Utilities.MonoAbstracts;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    [SelectionBase]
    [RequireComponent(typeof(BuildElementLifeCycle))]
    public class BuildElement : BuildElementMono
    {
#pragma warning disable 649
        public BuildPosition CanBeBuilt = BuildPosition.AllSides;
        public BuildPosition BuildBaseOn = BuildPosition.AllSides;
        public List<BuildElement> ConnectedTo = new List<BuildElement>();
        public bool IsGrounded;
        public string Description;
        public Sprite StoreSprite;
        private BuildElementLifeCycle lifeCycle;
        
#pragma warning restore 649

        private void OnEnable()
        {
            StartCoroutine(DelayedGroundCheck());
        }

        private IEnumerator DelayedGroundCheck()
        {
            yield return new WaitForSeconds(0.3f);
            
            SetIsGrounded();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetDetached()
        {
            if (!lifeCycle)
            {
                lifeCycle = GetComponent<BuildElementLifeCycle>();
            }

            lifeCycle.IsDetached = true;
            
            Disconnect();
        }
        
        public void SetConnections()
        {
            SetIsGrounded();

            SetConnectedTo();
        }

        private void SetIsGrounded()
        {
            IsGrounded = SurroundingElementInfo.IsAnyPositionAroundGround(transform.position.ToVector3Int());
        }
        
        public void Disconnect()
        {
            ConnectedTo.ForEach(element => element.SetConnections());
        }
        
        private void SetConnectedTo()
        {
            ConnectedTo.Clear();
            
            ConnectedTo = SurroundingElementInfo.GetConnectedElements(
                transform.position.ToVector3Int(),
                SurroundingElementInfo.ResolveDirectionsValidForBuildBasePosition(BuildBaseOn)).ToList();
            
            ConnectedTo.ForEach(RegisterToNeighbourElement);
        }

        private void RegisterToNeighbourElement(BuildElement element)
        {
            if(element && !element.ConnectedTo.Contains(this))
            {
                element.ConnectedTo.Add(this);
            };
        }

        private void Start()
        {
            CheckSetup();
            SetConnections();
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
