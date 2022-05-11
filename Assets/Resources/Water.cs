using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources
{
    public class Water : MonoBehaviour
    {

        // quantity
        [Range(0, 100)]
        public int Quantity;
        // the number of spots for this food
        public int SpotNumber = 4;
        // spots distance from the food object
        public float SpotDistance = 2.0f;
        // the list of spots for eating
        public List<ResourceSpot> SpotList;

        public Water()
        {
            Quantity = 50;
        }

        private void Start()
        {
            // create the list of positions where animals can go to eat
            Transform currentTransform = gameObject.GetComponent<Rigidbody>().transform;
            SpotList = new List<ResourceSpot>();
            // spots are around the food object
            for (int i = 0; i < SpotNumber; i++)
            {
                currentTransform.Rotate(currentTransform.up, (float)(360 / SpotNumber));
                SpotList.Add(new ResourceSpot(currentTransform.position + gameObject.transform.forward * SpotDistance));
            }
        }

        // get the nearer and free spot (between the ones of this food) from a starting position
        public ResourceSpot GetNearerFreeSpot(Vector3 startingPosition)
        {
            ResourceSpot spot = null;
            float distance = float.PositiveInfinity;
            for (int i = 0; i < SpotList.Count; i++)
            {
                float dist = Vector3.Distance(startingPosition, SpotList[i].Position);
                if (dist < distance && SpotList[i].IsFree)
                {
                    spot = SpotList[i];
                    distance = dist;
                }
            }

            return spot;
        }

        public bool HasFreeSpot()
        {
            for (int i = 0; i < SpotList.Count; i++)
            {
                if (SpotList[i].IsFree)
                    return true;
            }

            return false;
        }

        public void Consume(int quantity)
        {
            Quantity -= quantity;
        }
    }
}