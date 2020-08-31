using System;
using System.Collections;
using System.Collections.Generic;
using HelperClasses;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class NewCubeSpawner : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private BuildPositionProvider buildPositionProvider;
    [SerializeField] private GameObject previewPresenter;
    [SerializeField] private GameObject previewElementPivot;
    [SerializeField] private Vector3 previewElementScaleFactor = new Vector3(0.9f, 0.9f, 0.9f);
    [SerializeField] private float previewElementTransparencyFactor = 0.7f;
#pragma warning restore 649

    void Start()
    {
        buildPositionProvider.OnPreviewPositionChanged.AddListener(ShowPreviewElement);
        buildPositionProvider.OnNoValidPreviewPosition.AddListener(HidePreviewElement);
    }

    private void HidePreviewElement()
    {
        previewPresenter.SetActive(false);
    }

    private void ShowPreviewElement(Vector3Int previewPosition)
    {
        // print($"New previewCube position: {previewPosition}");
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

    /// <summary>
    /// Run from BuildStoreController OnStoreItemChange
    /// </summary>
    /// <param name="newElement"></param>
    public void SetPreviewElement(BuildElement newElement)
    {
        if (newElement == null) return;

        foreach (Transform child in previewElementPivot.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        var previewItem = newElement.gameObject;
        previewItem = Instantiate(previewItem, previewElementPivot.transform.position, Quaternion.identity);

        SetPreviewItem(previewItem);
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

        element.transform.parent = previewElementPivot.transform;
    }
}
