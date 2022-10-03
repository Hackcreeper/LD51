using Cooking;
using UnityEngine;

public class YeetPlatform : DeskSlot
{
    public Transform yeetTarget;
    public Transform standingTarget;

    public override void Interact(Player.Player player)
    {
        if (itemOnSlot != null)
        {
            Debug.Log("Feel the yeet");
            itemOnSlot.GetComponent<Plate>().Yeet(yeetTarget.position);
            itemOnSlot = null;
            
            return;
        }
        
        var plate = player.GetItemHolder().GetCurrentPlate();
        if (plate == null)
        {
            return;
        }

        if (plate.GetMeal() == null || !plate.GetPickableMeal().IsReadyToDeliver())
        {
            return;
        }
        
        base.Interact(player);
        
        plate.PlacedOnYeetPlatform();
    }
}