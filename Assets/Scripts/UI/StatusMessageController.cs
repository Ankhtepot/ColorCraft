using TMPro;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class StatusMessageController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Color successfulColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Animator animator;
#pragma warning restore 649

    public void DisplayMessage(string text, bool isSuccessful)
    {
        textMesh.text = text;
        textMesh.color = isSuccessful ? successfulColor : failedColor;
        
        animator.SetTrigger(Strings.Show);
    }
}
