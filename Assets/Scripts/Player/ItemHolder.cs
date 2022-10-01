using Cooking;
using Cooking.Data;
using UnityEngine;

namespace Player
{
    public class ItemHolder : MonoBehaviour
    {
        private IPickable _currentItem;

        public void PickIngredient(Ingredient ingredient)
        {
            RemoveCurrent();
            SpawnAndPosition(ingredient.pickupPrefab);
        }

        public void PickPlate(Plate plate)
        {
            RemoveCurrent();
            
            plate.transform.SetParent(transform);
            plate.transform.localPosition = new Vector3(0, 2f, 0);
            _currentItem = plate.GetComponent<IPickable>();
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

        public Ingredient GetCurrentIngredient() =>_currentItem != null && _currentItem.GetType() == typeof(PickableIngredient) ? ((PickableIngredient)_currentItem).ingredient : null;

        public IPickable MoveIngredient(Transform target, Vector3 offset)
        {
            var item = _currentItem;
            
            if (_currentItem != null)
            {
                ((PickableIngredient)_currentItem).PlaceOnPlate(target, offset);
            }

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
    }
}