using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources
{
    public class Water : Resource
    {
        public Water()
        {
            Quantity = 50;
        }

        private void Start()
        {
            CreateSpots();
        }
    }
}