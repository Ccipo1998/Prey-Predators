using System.Collections;
using UnityEngine;

namespace Assets.Resources
{
    public class Water : MonoBehaviour
    {

        // quantity
        [Range(0, 100)]
        public int Quantity;

        public Water()
        {
            Quantity = 50;
        }
    }
}