﻿using System.Globalization;
using Components;
using TMPro;
using UnityEngine;
using CharacterController = Controllers.CharacterController;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
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
            RefreshPositionText(characterPosition.CurrentGridPosition);
        }

        private void RefreshPositionText(Vector3Int newPosition)
        {
            XText.text = newPosition.x.ToString(CultureInfo.InvariantCulture);
            ZText.text = newPosition.z.ToString(CultureInfo.InvariantCulture);
        }
    }
}
