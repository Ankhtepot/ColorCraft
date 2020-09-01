using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Models;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class SaveLoadController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameController gameController;
    [SerializeField] private string saveFolder = "Save";
    [SerializeField] private string saveFileName = "SavedPosition.json";
    [SerializeField] public CustomUnityEvents.EventBool OnFileSave;
    [SerializeField] public CustomUnityEvents.EventSavedPosition OnFileLoad;
    private StatusMessageController statusReporter;
#pragma warning restore 649

    void Start()
    {
        initialize();
    }

    void Update()
    {
            ManageInput();
    }

    private void ManageInput()
    {
        if (gameController.InputEnabled && Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            Load();
        }
    }

    private void Save()
    {
        var environment = FindObjectOfType<EnvironmentControler>();
        var terrain = FindObjectOfType<TerrainSpawner>();
        var store = FindObjectOfType<BuiltElementsStore>();
        var characterTransform = FindObjectOfType<CharacterController>().transform;
        
        var newSave = new SavedPosition()
        {
            GridSize = environment.GridSize,
            WorldTileSide = environment.WorldTileSideSize,
            OriginalOffset = terrain.InitialWorldOffset,
            TraversedOffset = terrain.TraversedOffset,
            CharacterPosition = characterTransform.position,
            CharacterRotation = characterTransform.rotation,
            BuiltElements = GetBuildDescriptions(store.GetBuiltElements())
        };
        
        var saveResult = FileServices.SavePosition(newSave, GetSavePath());
        
        ReportMessage(saveResult ? "Save Successful" : "Save failed", saveResult);
        
        OnFileSave?.Invoke(saveResult);
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFolder, saveFileName);
    }

    private List<BuiltElementDescription> GetBuildDescriptions(List<GameObject> list)
    {
        return list.Select(GameObjectToBuiltElementDescription)
            .Where(item => item != null)
            .ToList();
    }
    
    private BuiltElementDescription GameObjectToBuiltElementDescription(GameObject item)
    {
        if (!item.GetComponent<BuildElement>()) return null;

        var buildElement = item.GetComponent<BuildElement>();
        var health = item.GetComponent<Health>();

        return new BuiltElementDescription()
        {
            Position = item.transform.position.ToVector3Int(),
            Name = buildElement.Description,
            Health = (health != null ? health.GetCurrentHitpoints() : 0)
        };
    }

    private void Load()
    {
        var loadedPosition = FileServices.LoadPosition(GetSavePath());
        var isLoadSuccess = loadedPosition != null;
        
        ReportMessage(isLoadSuccess ? "Load Successful" : "Load Failed", isLoadSuccess);
        
        if (isLoadSuccess)
        {
            OnFileLoad?.Invoke(loadedPosition);
        }
    }

    private void ReportMessage(string text, bool isSuccessful)
    {
        if (statusReporter == null)
        {
            statusReporter = FindObjectOfType<StatusMessageController>();
        }
        
        statusReporter.DisplayMessage(text, isSuccessful);
    }

    private void initialize()
    {
       
    }
}
