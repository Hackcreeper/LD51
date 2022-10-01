using Cooking;
using Cooking.Data;
using UnityEngine;

namespace Player
{
    public class ItemHolder : MonoBehaviour
    {
        private IPickable _currentItem;

        public bool PickIngredient(Ingredient ingredient, bool replace = false)
        {
            if (replace)
            {
                RemoveCurrent();
            }

            if (_currentItem != null)
            {
                return false;
            }
            
            SpawnAndPosition(ingredient.pickupPrefab);
            return true;
        }

        public bool PickPlate(Plate plate, bool replace = false)
        {
            if (replace)
            {
                RemoveCurrent();
            }

            if (_currentItem != null)
            {
                return false;
            }

            plate.transform.SetParent(transform);
            plate.transform.localPosition = new Vector3(0, 2f, 0);
            _currentItem = plate.GetComponent<IPickable>();
            return true;
        }

        private void SpawnAndPosition(GameObject prefab)
        {
            var instance = Instantiate(
                prefab,
                transform
            );

            instance.transform.localPosition = new Vector3(0, 2f, 0);
            _currentItem = instance.GetComponent<IPickable>();
        }

        public Ingredient GetCurrentIngredient() =>
            _currentItem != null && _currentItem.GetType() == typeof(PickableIngredient)
                ? ((PickableIngredient)_currentItem).ingredient
                : null;

        public Plate GetCurrentPlate() => _currentItem != null && _currentItem.GetType() == typeof(Plate)
            ? (Plate)_currentItem
            : null;

        public IPickable MoveItem(Transform target, Vector3 offset)
        {
            var item = _currentItem;

            if (_currentItem == null)
            {
                return item;
            }
            
            if (_currentItem.GetType() == typeof(PickableIngredient))
            {
                ((PickableIngredient)_currentItem).PlaceOnPlate(target, offset);
                
                _currentItem = null;
                return item;
            }

            if (_currentItem.GetType() == typeof(Plate))
            {
                ((Plate)_currentItem).Placed(target);
            }

            var mono = ((MonoBehaviour)_currentItem).transform;
                
            mono.SetParent(target);
            mono.localPosition = offset;

            _currentItem = null;
            
            return item;
        }

        public void RemoveCurrent()
        {
            if (_currentItem == null)
            {
                return;
            }

            Destroy(((MonoBehaviour)_currentItem).gameObject);
            _currentItem = null;
        }

        public bool IsEmpty() => _currentItem == null;
    }
}