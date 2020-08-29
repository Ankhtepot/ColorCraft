using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class PositionTextControler : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private TextMeshProUGUI XText;
    [SerializeField] private TextMeshProUGUI ZText;
    [SerializeField] private Position characterPosition;
#pragma warning restore 649

    private void Start()
    {
        characterPosition = FindObjectOfType<CharacterController>().GetComponent<Position>();
        characterPosition.OnPositionChanged.AddListener(RefreshPositionText);
        RefreshPositionText(characterPosition.currentGridPosition);
    }

    private void RefreshPositionText(Vector3 newPosition)
    {
        XText.text = newPosition.x.ToString(CultureInfo.InvariantCulture);
        ZText.text = newPosition.z.ToString(CultureInfo.InvariantCulture);
    }
}
