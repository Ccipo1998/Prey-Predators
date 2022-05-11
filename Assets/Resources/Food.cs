using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources
{
    public class Food : MonoBehaviour
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

        public Food()
        {
            Quantity = 50;
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