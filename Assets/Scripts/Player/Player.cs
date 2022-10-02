using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(ItemHolder), typeof(PlayerMovement))]
    public class Player : MonoBehaviour
    {
        private ItemHolder _itemHolder;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _itemHolder = GetComponent<ItemHolder>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public ItemHolder GetItemHolder() => _itemHolder;
        public PlayerMovement GetPlayerMovement() => _playerMovement;
    }
}