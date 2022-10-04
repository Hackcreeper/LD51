using System;
using UnityEngine;

namespace Cooking.Data
{
    [Serializable]
    public class Recipe
    {
        public Ingredient input;
        public Ingredient output;
        public GameObject replacementPrefab;
        public AudioClip audioClip;
    }
}