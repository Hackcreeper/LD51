using UnityEngine;

public class YeetPlatform : Interactable
{
    public Transform yeetTarget;

    public override void Interact(Player.Player player)
    {
        var plate = player.GetItemHolder().GetCurrentPlate();
        if (plate == null)
        {
            return;
        }

        if (plate.GetMeal() == null || !plate.GetPickableMeal().IsReadyToDeliver())
        {
            return;
        }

        var oldPos = plate.transform.position;
        player.GetItemHolder().MoveItem(transform, Vector3.zero);
        plate.transform.position = oldPos;
        
        plate.Yeet(yeetTarget.position);
    }
}