using System;
using System.Collections;
using System.Collections.Generic;
using HelperClasses;
using Models;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

//Fireball Games * * * PetrZavodny.com

public class BuildStoreController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Sprite[] itemsSprites;
    [SerializeField] private BuildElement[] itemsPrefabs;
    [SerializeField] private Image shownItemImage;
    [SerializeField] private int currentItemIndex;
    private readonly List<BuildStoreItem> itemsStore = new List<BuildStoreItem>();
    [SerializeField] private bool inputEnabled = true;
    [SerializeField] private GameMode gameMode;
    [SerializeField] public CustomUnityEvents.EventBuildElement OnStoreItemChanged;
#pragma warning restore 649

    void Start()
    {
        initialize();
    }

    private void Update()
    {
        if (inputEnabled)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        var delta = Input.GetAxis(Strings.MouseScrollWheel);

        if (gameMode == GameMode.Build && Math.Abs(delta) > float.Epsilon)
        {
            if (delta > 0f)
            {
                currentItemIndex--;
            }
            else if (delta < 0f)
            {
                currentItemIndex++;
            }
        
            ChangeStoreItem(currentItemIndex);
        }
    }

    private void ChangeStoreItem(int itemIndex)
    {
        if (itemsStore.Count < 1) return;
        
        currentItemIndex = Mathf.Clamp(itemIndex, 0, itemsStore.Count - 1);

        shownItemImage.sprite = itemsStore[currentItemIndex].storeSprite;
        OnStoreItemChanged?.Invoke(itemsStore[currentItemIndex].prefab);
    }
    
    private void initialize()
    {
        if (itemsPrefabs.Length != itemsSprites.Length)
        {
            Debug.LogError("Items lists in a build store must have same number of sprites as prefabs");
            return;
        }

        for (int i = 0; i < itemsSprites.Length; i++)
        {
            itemsStore.Add(new BuildStoreItem()
            {
                storeSprite = itemsSprites[i],
                prefab = itemsPrefabs[i]
            });
        }

        currentItemIndex = 0;
        gameMode = GameMode.FreeFlight;
    }

    /// <summary>
    /// Run from GameController OnGameModeChangeEvent
    /// </summary>
    /// <param name="newMode"></param>
    public void OnGameModeChange(GameMode newMode)
    {
        gameMode = newMode;
        ChangeStoreItem(currentItemIndex);
    }

    /// <summary>
    /// Run from GameController OnInputEnabledChanged
    /// </summary>
    /// <param name="newState"></param>
    public void OnInputEnabledChanged(bool newState)
    {
        inputEnabled = newState;
    }
}
