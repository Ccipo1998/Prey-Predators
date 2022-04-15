using System.Collections;
using UnityEngine;

namespace Assets.Resources
{
    public class Food : MonoBehaviour
    {
        // quantity
        [Range(0, 100)]
        public int Quantity;
        // food type
        public FoodType Type;

        public Food()
        {
            Quantity = 50;
        }
    }

    public enum FoodType
    {
        Meat, Plant
    }
}