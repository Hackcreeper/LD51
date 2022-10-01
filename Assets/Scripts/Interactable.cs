using Ui;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool IsInteractable = true;

    public abstract void Interact(Player.Player player);

    public bool CanInteract() => IsInteractable;

    public void ShowPopup()
    {
        if (!IsInteractable)
        {
            return;
        }
        
        PopupManager.Instance.ShowAt(GetComponent<Collider>().bounds.center);
    }
}