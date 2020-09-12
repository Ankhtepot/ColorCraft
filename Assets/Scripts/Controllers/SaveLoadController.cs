using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components;
using Extensions;
using Models;
using UI;
using UnityEngine;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class SaveLoadController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameController gameController;
        [SerializeField] private ScreenShotService screenShotService;
        [SerializeField] private string saveFolder = "Save";
        [SerializeField] private string saveFilePrefix = "NewSave";
        [SerializeField] private string quickSaveFilePrefix = "QuickSave";
        [SerializeField] private string saveFileExtension = ".json";
        [SerializeField] public CustomUnityEvents.EventBool OnFileSave;
        [SerializeField] public CustomUnityEvents.EventSavedPosition OnFileLoad;
        private StatusMessageController statusReporter;
#pragma warning restore 649

        private void Update()
        {
            ManageInput();
        }

        private void ManageInput()
        {
            if (gameController.InputEnabled && Input.GetKeyDown(KeyCode.F5))
            {
                QuickSave();
            }
            else
            if (Input.GetKeyDown(KeyCode.F9))
            {
                QuickLoad();
            }
        }

        private void QuickSave()
        {
            Save(SavedPositionType.QuickSave);
        }

        private void Save(SavedPositionType positionType)
        {
            var environment = FindObjectOfType<EnvironmentController>();
            var terrain = FindObjectOfType<TerrainSpawnerController>();
            var characterTransform = FindObjectOfType<CharacterController>().transform;
            var newSaveFilePath = GetSavePath(positionType);
        
            var newSave = new SavedPosition()
            {
                GridSize = environment.GridSize,
                WorldTileSide = environment.WorldTileSideSize,
                positionType = positionType,
                OriginalOffset = terrain.InitialWorldOffset,
                TraversedOffset = terrain.TraversedOffset,
                CharacterPosition = characterTransform.position,
                CharacterRotation = characterTransform.rotation,
                ScreenShotFileName = screenShotService.TakeScreenShot(newSaveFilePath),
                DateTimeTicks = DateTime.Now.Ticks,
                BuiltElements = GetBuildDescriptions(BuiltElementsStoreController.GetBuiltElements())
            };
        
            var saveResult = FileServices.SavePosition(newSave, newSaveFilePath);
        
            ReportMessage(saveResult ? Strings.SaveSuccessful : Strings.SaveFailed, saveResult);
        
            OnFileSave?.Invoke(saveResult);
        }

        private string GetSavePath(SavedPositionType positionType)
        {
            return Path.Combine(GetSaveFolderPath(), GetUniqueSaveName(positionType));
        }

        private string GetSaveFolderPath()
        {
            return Path.Combine(Application.persistentDataPath, saveFolder);
        }

        private List<BuiltElementDescription> GetBuildDescriptions(List<GameObject> list)
        {
            return list.Select(GameObjectToBuiltElementDescription)
                .Where(item => item != null)
                .ToList();
        }
    
        private static BuiltElementDescription GameObjectToBuiltElementDescription(GameObject item)
        {
            if (!item.GetComponent<BuildElement>()) return null;

            var buildElement = item.GetComponent<BuildElement>();
            var health = item.GetComponent<BuildElementLifeCycle>();

            return new BuiltElementDescription()
            {
                Position = item.transform.position.ToVector3Int(),
                Name = buildElement.Description,
                Health = (health ? health.Hitpoints : 0)
            };
        }

        private void QuickLoad()
        {
            ReportMessage(Strings.LoadingPosition, true);
            
            var loadedPosition = GetLatestQuickSave();
            var isLoadSuccess = loadedPosition != null;
        
            ReportMessage(isLoadSuccess ? Strings.LoadSuccessful : Strings.LoadFailed, isLoadSuccess);
        
            if (isLoadSuccess)
            {
                OnFileLoad?.Invoke(loadedPosition);
            }
        }

        private SavedPosition GetLatestQuickSave()
        {
            CheckSaveFolderPath();
            var quickSaves = Directory.GetFiles(GetSaveFolderPath())
                .Where(file => file.Contains(quickSaveFilePrefix) && file.Contains(saveFileExtension))
                .Select(FileServices.LoadPosition)
                .ToList();

            return quickSaves.OrderByDescending(position => position.DateTimeTicks).ToList()[0];
        }

        private void ReportMessage(string text, bool isSuccessful)
        {
            if (!statusReporter)
            {
                statusReporter = FindObjectOfType<StatusMessageController>();
            }
        
            statusReporter.RegisterMessage(text, isSuccessful);
        }

        public string GetUniqueSaveName(SavedPositionType positionType)
        {
            CheckSaveFolderPath();

            var nameBase = positionType == SavedPositionType.QuickSave ? quickSaveFilePrefix : saveFilePrefix;
            
            var existingSaves = Directory.GetFiles(GetSaveFolderPath()).Where(file => file.Contains(nameBase) && file.EndsWith(saveFileExtension)).ToList();
            var counterSuffix = existingSaves.Count <= 0 ? "" : (existingSaves.Count).ToString(); 
            
            var newFileName = $"{nameBase}{counterSuffix}{saveFileExtension}";

            while (existingSaves.Where(file => file.Contains(newFileName)).ToList().Count > 0)
            {
                if(int.TryParse(counterSuffix, out var innerCounter))
                {
                    innerCounter += 1;
                }
                newFileName = $"{nameBase}{innerCounter}{saveFileExtension}";
            }

            return newFileName;
        }

        private void CheckSaveFolderPath()
        {
            if (!Directory.Exists(GetSaveFolderPath()))
            {
                Directory.CreateDirectory(GetSaveFolderPath());
            }
        }
    }
}
