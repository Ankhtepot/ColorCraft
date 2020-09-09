using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class StatusMessageController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Color successfulColor;
        [SerializeField] private Color failedColor;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Animator animator;

        private bool isBusy;
        private readonly Queue<KeyValuePair<string, bool>> messageQueue = new Queue<KeyValuePair<string, bool>>();
#pragma warning restore 649

        public void RegisterMessage(string text, bool isSuccessful)
        {
            messageQueue.Enqueue(new KeyValuePair<string, bool>(text, isSuccessful));

            if (!isBusy)
            {
                DisplayMessage();
            }
        }

        private void DisplayMessage()
        {
            if (messageQueue.Count <= 0)
            {
                isBusy = false;
                return;
            }

            isBusy = true;
            
            var message = messageQueue.Dequeue();
            textMesh.text = message.Key;
            textMesh.color = message.Value ? successfulColor : failedColor;
        
            animator.SetTrigger(Strings.Show);
        }
    }
}
