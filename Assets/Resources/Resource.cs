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
        public float SpotDistance = 1.1f;
        // the list of spots for eating
        public List<ResourceSpot> SpotList;
        // automatic spot creation flag
        public bool SpotsGeneration = false;
        // spot prefab
        public GameObject SpotPrefab;

        public void CreateSpots()
        {
            // empty object creation
            //GameObject empty = new GameObject();

            // create the list of positions where animals can go to eat/drink
            SpotList = new List<ResourceSpot>();
            Vector3 pos = gameObject.transform.position + gameObject.transform.forward * SpotDistance;
            // spots are around the food object
            for (int i = 0; i < SpotNumber; i++)
            {
                var spot = Instantiate(SpotPrefab);
                spot.transform.position = pos;
                spot.transform.parent = gameObject.transform;
                gameObject.transform.Rotate(gameObject.transform.up, (float)(360 / SpotNumber));
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