using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class VisibilityController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private KeyCode ShortcutKey;
    [SerializeField] private bool isUIPartShown;
    [SerializeField] private Animator animator;
    [SerializeField] public CustomUnityEvents.EventBool OnVisibilityChanged;
#pragma warning restore 649

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(ShortcutKey))
        {
            isUIPartShown = !isUIPartShown;
            animator.SetBool(Strings.Show, isUIPartShown);
            
            OnVisibilityChanged?.Invoke(!isUIPartShown);
        }
        
    }
}
