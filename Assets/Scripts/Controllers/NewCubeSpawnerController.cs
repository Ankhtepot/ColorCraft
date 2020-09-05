using System.Collections;
using Components;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class NewCubeSpawnerController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private bool inputEnabled = true;
        [SerializeField] private bool canBuild = true;
        [SerializeField] private float canBuildCooldown = 0.2f;
        [SerializeField] private BuildPositionProvider buildPositionProvider;
        [SerializeField] private BuiltElementsStoreController storeController;
        [SerializeField] private GameObject previewPresenter;
        [SerializeField] private GameObject previewElementPivot;
        [SerializeField] private Transform builtElementParent;
        [SerializeField] private Vector3 previewElementScaleFactor = new Vector3(0.9f, 0.9f, 0.9f);
        [SerializeField] private float previewElementTransparencyFactor = 0.7f;
        [SerializeField] private GameObject elementToBuild;
        private GameObject previewItem;
#pragma warning restore 649

        void Start()
        {
            buildPositionProvider.OnPreviewPositionChanged.AddListener(ShowPreviewElement);
            buildPositionProvider.OnNoValidPreviewPosition.AddListener(HidePreviewElement);
        }

        private void Update()
        {
            if (inputEnabled && canBuild)
            {
                InstantiateBuildElement();
            }
        }

        private void HidePreviewElement()
        {
            previewPresenter.SetActive(false);
        }

        private void ShowPreviewElement(Vector3Int previewPosition, Vector3Int hitNormal)
        {
            if (previewItem.CompareTag(Strings.BuildTopOnly) && hitNormal != Vector3Int.up)
            {
                return;
            }
        
            previewPresenter.transform.position = previewPosition;
            previewPresenter.SetActive(true);
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

        private void InstantiateBuildElement()
        {
            if (Input.GetMouseButton(0) && elementToBuild != null && previewPresenter.activeSelf)
            {
                var instantiatedElement =
                    Instantiate(elementToBuild, previewPresenter.transform.position, Quaternion.identity);
            
                instantiatedElement.tag = Strings.BuildAllSides;
                instantiatedElement.transform.parent = builtElementParent;
            
                storeController.AddElement(instantiatedElement);

                canBuild = false;
            
                StartCoroutine(CooldownCanBuild());
            }
        }

        private IEnumerator CooldownCanBuild()
        {
            yield return new WaitForSeconds(canBuildCooldown);
            canBuild = true;
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

        private void SetPreviewItem(GameObject element)
        {
            element.transform.localScale = previewElementScaleFactor;

            var elementMaterial = element.GetComponentInChildren<Renderer>().material;
            var newColor = elementMaterial.color;
            newColor.a = previewElementTransparencyFactor;
            var newMaterial = new Material(elementMaterial);
            newMaterial.SetColor(Strings.SetMaterialColorKeyword, newColor);

            element.GetComponentInChildren<Renderer>().material = newMaterial;
            element.tag = "Untagged";

            element.transform.parent = previewElementPivot.transform;
        }

        /// <summary>
        /// Run from GameController OnInputEnabledChanged
        /// </summary>
        /// <param name="isEnabled"></param>
        public void OnInputEnabledChanged(bool isEnabled)
        {
            inputEnabled = isEnabled;
        }
    }
}
