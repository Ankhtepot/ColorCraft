using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class NewCubeSpawner : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private BuildPositionProvider buildPositionProvider;
    [SerializeField] private GameObject previewCubePrefab;
    private GameObject previewCube;
#pragma warning restore 649

    void Start()
    {
        buildPositionProvider.OnPreviewPositionChanged.AddListener(ShowPreviewCube);
        buildPositionProvider.OnNoValidPreviewPosition.AddListener(HidePreviewCube);
        previewCube = Instantiate(previewCubePrefab, Vector3.zero, Quaternion.identity);
        previewCube.SetActive(false);
    }

    private void HidePreviewCube()
    {
        previewCube.SetActive(false);
    }

    private void ShowPreviewCube(Vector3Int previewPosition)
    {
        print($"New previewCube position: {previewPosition}");
        previewCube.transform.position = previewPosition;
        previewCube.SetActive(true);
    }
}
