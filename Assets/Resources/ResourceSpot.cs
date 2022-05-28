using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources
{
    public class ResourceSpot
    {
        // position of the spot
        public Vector3 Position;
        // currently free or occupied spot
        public bool IsFree;
        // list of object in queue for this spot
        private List<GameObject> Queue = new List<GameObject>();

        public ResourceSpot(Vector3 position)
        {
            Position = position;
            IsFree = true;
        }

        public void Occupy()
        {
            IsFree = false;
        }

        public void Free()
        {
            IsFree = true;
        }

        // add an object in the queue for the spot
        public void ToQueue(GameObject objectToQueue)
        {
            Queue.Add(objectToQueue);
        }

        // assign the spot to the nearest object
        public GameObject AssignSpot()
        {
            // if the spot is occupied -> no assignment
            if (!IsFree)
            {
                Queue.Clear();
                return null;
            }

            GameObject assigned = null;
            float distance = float.PositiveInfinity;
            for (int i = 0; i < Queue.Count; i++)
            {
                float dist = Vector3.Distance(Position, Queue[i].transform.position);
                if (dist < distance)
                {
                    distance = dist;
                    assigned = Queue[i];
                }
            }

            return assigned;
        }

        // clear the queue
        public void ClearQueue()
        {
            Queue.Clear();
        }
    }
}