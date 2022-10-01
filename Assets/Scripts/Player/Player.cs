using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(ItemHolder))]
    public class Player : MonoBehaviour
    {
        private ItemHolder _itemHolder;

        private void Awake()
        {
            _itemHolder = GetComponent<ItemHolder>();
        }

        public ItemHolder GetItemHolder() => _itemHolder;
    }
}