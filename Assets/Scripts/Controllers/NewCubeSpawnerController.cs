using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Components;
using UnityEngine;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class NewCubeSpawnerController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private float canBuildCooldown = 0.2f;
        [SerializeField] private GameObject previewPresenter;
        [SerializeField] private GameObject previewElementPivot;
        [SerializeField] private Transform builtElementParent;
        [SerializeField] private Vector3 previewElementScaleFactor = new Vector3(0.9f, 0.9f, 0.9f);
        [SerializeField] private float previewElementTransparencyFactor = 0.7f;
        
        private bool inputEnabled = true;
        private bool canBuild = true;
        private GameObject elementToBuild;
        private GameObject previewItem;
#pragma warning restore 649
       
        private void Update()
        {
            InstantiateBuildElement();
        }

        private void InstantiateBuildElement()
        {
            if (!canBuild 
                || !inputEnabled 
                || !Input.GetMouseButton(0) 
                || !elementToBuild
                || !previewPresenter.activeSelf 
                || BuiltElementsStoreController.ContainsKey(previewPresenter.transform.position)) return;
            
            var instantiatedElement =
                Instantiate(elementToBuild, previewPresenter.transform.position, Quaternion.identity);
            
            instantiatedElement.tag = elementToBuild.tag;
            instantiatedElement.transform.parent = builtElementParent;
            
            BuiltElementsStoreController.AddElement(instantiatedElement);

            canBuild = false;
            
            StartCoroutine(CooldownCanBuild());
        }

        private IEnumerator CooldownCanBuild()
        {
            yield return new WaitForSeconds(canBuildCooldown);
            canBuild = true;
        }

        private void SetPreviewItem(GameObject element)
        {
            element.transform.localScale = previewElementScaleFactor;
            
            var elementMaterial = element.GetComponentInChildren<Renderer>().material;
            element.GetComponentInChildren<Renderer>().material = CreateTransparentMaterialVariant(elementMaterial);
            element.GetComponent<BuildElement>().BuildBaseOn = BuildPosition.None;

            element.transform.parent = previewElementPivot.transform;
        }

        [SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private Material CreateTransparentMaterialVariant(Material originalMaterial)
        {
            var newMaterial = new Material(Shader.Find("Standard"));
            newMaterial.CopyPropertiesFromMaterial(originalMaterial);
            newMaterial.SetFloat("_Mode", 2);
            newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            newMaterial.SetInt("_ZWrite", 0);
            newMaterial.DisableKeyword("_ALPHATEST_ON");
            newMaterial.EnableKeyword("_ALPHABLEND_ON");
            newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            newMaterial.renderQueue = 3000;
            var newColor = newMaterial.color;
            newColor.a = previewElementTransparencyFactor;
            newMaterial.color = newColor;

            return newMaterial;
        }
        
        /// <summary>
        /// Run from BuildPositionProvider OnNoValidPreviewPosition
        /// </summary>
        public void HidePreviewElement()
        {
            previewPresenter.SetActive(false);
        }
        
        /// <summary>
        /// Run from BuildPositionProvider OnPreviewPositionChanged
        /// </summary>
        public void ShowPreviewElement(Vector3Int previewPosition, Vector3Int hitNormal)
        {
            if (BuildPositionProvider.GetBuildPosition(previewItem.transform) == BuildPosition.Top && hitNormal != Vector3Int.up)
            {
                return;
            }
        
            previewPresenter.transform.position = previewPosition;
            previewPresenter.SetActive(true);
        }

        /// <summary>
        /// Run from GameController OnInputEnabledChanged
        /// </summary>
        /// <param name="isEnabled"></param>
        public void OnInputEnabledChanged(bool isEnabled)
        {
            inputEnabled = isEnabled;
        }
        
        /// <summary>
        /// Run from GameController gameModeChange event
        /// </summary>
        /// <param name="newMode"></param>
        public void ShowHidePreviewElementBasedOnGameMode(GameMode newMode)
        {
            if (newMode != GameMode.Build)
            {
                previewPresenter.SetActive(false);
            }
        }
        
        /// <summary>
        /// Run from BuildStoreController OnStoreItemChange
        /// </summary>
        /// <param name="newElement"></param>
        public void SetPreviewElement(BuildElement newElement)
        {
            if (newElement == null) return;

            foreach (Transform child in previewElementPivot.transform) {
                Destroy(child.gameObject);
            }
        
            previewItem = newElement.gameObject;
            elementToBuild = previewItem;
            var newPreviewItem = Instantiate(previewItem, previewElementPivot.transform.position, Quaternion.identity);

            SetPreviewItem(newPreviewItem);
        }
    }
}
