using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class PickableIngredient : MonoBehaviour, IPickable
    {
        public Ingredient ingredient;
        public Transform normalModel;
        public Transform plateModel;
    
        public void PlaceOnPlate(Transform parent, Vector3 offset)
        {
            transform.SetParent(parent);
            transform.localPosition = offset + new Vector3(0, 0.14f, 0);
            
            normalModel.gameObject.SetActive(false);
            plateModel.gameObject.SetActive(true);
        }
    }
}