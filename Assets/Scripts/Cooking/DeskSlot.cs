using UnityEngine;

namespace Cooking
{
    public class DeskSlot : Interactable
    {
        public Transform itemOnSlot;
        public Transform itemTarget;
        
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
                    var worked = player.GetItemHolder().PickIngredient(itemOnSlot.GetComponent<PickableIngredient>().ingredient);
                    if (worked)
                    {
                        Destroy(itemOnSlot.gameObject);
                        itemOnSlot = null;
                        return;
                    }

                    var spawnedPickable = player.GetItemHolder().MoveItem(transform, itemTarget.localPosition);
                    player.GetItemHolder().PickIngredient(itemOnSlot.GetComponent<PickableIngredient>().ingredient);
                    
                    Destroy(itemOnSlot.gameObject);
                    itemOnSlot = ((MonoBehaviour)spawnedPickable).transform;
                    
                    return;
                }
                
                if (itemOnSlot.GetComponent<Plate>())
                {
                    var worked = player.GetItemHolder().PickPlate(itemOnSlot.GetComponent<Plate>());

                    if (worked)
                    {
                        itemOnSlot = null;
                        return;
                    }
                    
                    var spawnedPickable = player.GetItemHolder().MoveItem(transform, itemTarget.localPosition);
                    player.GetItemHolder().PickPlate(itemOnSlot.GetComponent<Plate>());
                    
                    itemOnSlot = ((MonoBehaviour)spawnedPickable).transform;

                    return;
                }
            }
            
            var pickable = player.GetItemHolder().MoveItem(transform, itemTarget.localPosition);
            itemOnSlot = ((MonoBehaviour)pickable)?.transform;
        }

        public void FreeSlot()
        {
            IsInteractable = true;
            itemOnSlot = null;
        }

        public void UnfreeSlot()
        {
            IsInteractable = false;
        }
    }
}