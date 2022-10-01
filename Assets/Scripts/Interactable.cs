using Ui;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string message;
    public Sprite icon;

    public abstract void Interact(Player.Player player);

    public void ShowPopup()
    {
        PopupManager.Instance.ShowAt(
            GetComponent<Collider>().bounds.center,
            message, icon
            );
    }
}