using System.Collections;
using System.Collections.Generic;
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
            // create the list of positions where animals can go to eat
            Transform currentTransform = gameObject.GetComponent<Rigidbody>().transform;
            SpotList = new List<ResourceSpot>();
            // spots are around the food object
            for (int i = 0; i < SpotNumber; i++)
            {
                currentTransform.Rotate(currentTransform.up, (float)(360 / SpotNumber));
                SpotList.Add( new ResourceSpot(currentTransform.position + gameObject.transform.forward * SpotDistance));
            }

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