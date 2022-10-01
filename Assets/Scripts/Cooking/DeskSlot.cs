using UnityEngine;

namespace Cooking
{
    public class DeskSlot : Interactable
    {
        public Transform itemOnSlot;
        
        private void Start()
        {
            IsInteractable = itemOnSlot == null;
        }

        public override void Interact(Player.Player player)
        {
            if (itemOnSlot != null)
            {
                if (itemOnSlot.GetComponent<PickableIngredient>())
                {
                    player.GetItemHolder().PickIngredient(itemOnSlot.GetComponent<PickableIngredient>().ingredient);
                    Destroy(itemOnSlot.gameObject);
                    itemOnSlot = null;
                    return;
                }
                
                if (itemOnSlot.GetComponent<Plate>())
                {
                    player.GetItemHolder().PickPlate(itemOnSlot.GetComponent<Plate>());
                    itemOnSlot = null;
                    return;
                }
            }
            
            var pickable = player.GetItemHolder().MoveIngredient(transform, Vector3.zero);
            itemOnSlot = ((MonoBehaviour)pickable).transform;
        }

        public void FreeSlot()
        {
            IsInteractable = true;
            itemOnSlot = null;
        }
    }
}