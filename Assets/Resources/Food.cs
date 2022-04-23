using System.Collections;
using UnityEngine;

namespace Assets.Resources
{
    public class Food : MonoBehaviour
    {
        // quantity
        [Range(0, 100)]
        public int Quantity;

        public Food()
        {
            Quantity = 50;
        }
    }
}