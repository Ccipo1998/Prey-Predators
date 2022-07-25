using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources
{
    public class Grass : Food
    {
        public Grass()
        {
            Quantity = 50;
        }

        // Use this for initialization
        void Start()
        {
            if (SpotsGeneration)
                CreateSpots();

            // grass growing
            InvokeRepeating("Grow", 1, 2);
        }

        // for debugging of spot positions
        private void Update()
        {
            /*
            for (int i = 0; i < SpotList.Count; i++)
            {
                Debug.DrawLine(gameObject.GetComponent<Rigidbody>().transform.position, SpotList[i].Position, Color.red);
            }
            */
        }

        // increase quantity over time
        private void Grow()
        {
            Quantity++;
        }
    }
}