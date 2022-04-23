using System.Collections;
using UnityEngine;

namespace Assets.Resources
{
    public class Grass : Food
    {
        public Grass()
        {
            
        }

        // Use this for initialization
        void Start()
        {
            InvokeRepeating("Grow", 1, 2);
        }

        // increase quantity over time
        private void Grow()
        {
            Quantity++;
        }
    }
}