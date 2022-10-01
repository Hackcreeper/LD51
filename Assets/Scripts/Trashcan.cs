public class Trashcan : Interactable
{
    public override void Interact(Player.Player player)
    {
        player.GetItemHolder().RemoveCurrent();
    }
}