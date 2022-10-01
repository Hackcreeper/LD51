public class Trashcan : Interactable
{
    public override void Interact(Player.Player player)
    {
        if (player.GetItemHolder().GetCurrentPlate() != null)
        {
            player.GetItemHolder().GetCurrentPlate().Clear();
            return;
        }
        
        player.GetItemHolder().RemoveCurrent();
    }
}