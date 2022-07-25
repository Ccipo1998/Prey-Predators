using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources
{
    public class Resource : MonoBehaviour
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
        // automatic spot creation flag
        public bool SpotsGeneration = false;

        public void CreateSpots()
        {
            // empty object creation
            //GameObject empty = new GameObject();

            // create the list of positions where animals can go to eat/drink
            Transform currentTransform = gameObject.transform;
            SpotList = new List<ResourceSpot>();
            // spots are around the food object
            for (int i = 0; i < SpotNumber; i++)
            {
                currentTransform.Rotate(currentTransform.up, (float)(360 / SpotNumber));
                // SpotList.Add(new ResourceSpot(currentTransform.position + gameObject.transform.forward * SpotDistance));
                var spot = Instantiate(GameObject.FindGameObjectWithTag("Spot"));
                spot.transform.position = currentTransform.position + gameObject.transform.forward * SpotDistance;
                SpotList.Add(new ResourceSpot(spot));
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

        public bool IsOver()
        {
            return Quantity < 1;
        }
    }
}