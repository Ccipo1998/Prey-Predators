using Assets.Resources;
using Assets.Species;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.GoalOrientedBehavior;

namespace Assets.FOV
{
    public class ZebraFOV : MonoBehaviour
    {

        public float Radius = 5f;
        public LayerMask TargetsMask;

        // all the objects that the animal can see currently
        public List<GameObject> ObjectsInFOV = new List<GameObject>();

        /* -> moved to zebraeat behavior
        // objects of interest
        public GameObject NearestFoodInFOV;
        public GameObject NearestWaterInFOV;
        */

        // Use this for initialization
        void Start()
        {
            TargetsMask = LayerMask.GetMask(new string[] { "Targets" });
        }

        void Update()
        {
            FindVisible();
            AddKnowledge();
        }

        // find visible objects in current FOV
        public void FindVisible()
        {
            // clear previous
            ObjectsInFOV.Clear();

            Vector3 currentPosition = transform.position;
            Collider currentCollider = gameObject.GetComponent<Collider>();

            // find objects in current FOV
            Collider[] colliders = Physics.OverlapSphere(currentPosition, Radius, TargetsMask);

            // find visible ones
            for (int i = 0; i < colliders.Length; i++)
            {
                // track a ray from current position to the seen collider and check if there is not intersection with obstacles (~TargetsMask is the negation)
                float currentDistance = Vector3.Distance(currentPosition, colliders[i].transform.position);
                if (!Physics.Raycast(currentPosition, colliders[i].transform.position, currentDistance, ~TargetsMask) && currentCollider != colliders[i])
                {
                    ObjectsInFOV.Add(colliders[i].gameObject);
                    //Debug.DrawLine(currentPosition, colliders[i].transform.position, Color.red);
                }
            }
        }

        // add positions to internal knowledge
        public void AddKnowledge()
        {
            Zebra currentAnimal = gameObject.GetComponent<Zebra>();
            Vector3 currentPosition = transform.position;
            float knownFoodDistance = currentAnimal.Knowledge.LastFoundedFood != null ? Vector3.Distance(currentPosition, currentAnimal.Knowledge.LastFoundedFood.position) : float.PositiveInfinity;
            float knownWaterDistance = currentAnimal.Knowledge.LastFoundedWater != null ? Vector3.Distance(currentPosition, currentAnimal.Knowledge.LastFoundedWater.position) : float.PositiveInfinity;

            bool foodChecked = false;
            bool waterChecked = false;

            int index = 0;
            while (!foodChecked && !waterChecked && index < ObjectsInFOV.Count)
            {
                GameObject seenObject = ObjectsInFOV[index];

                if (seenObject.GetComponent<Food>() != null && !seenObject.GetComponent<Food>().IsOver() && seenObject.GetComponent<Food>().HasFreeSpot() && currentAnimal.CurrentGoal != null && currentAnimal.CurrentGoal.Name == GoalName.Food)
                {
                    // food in FOV available to eat and when the zebra is searching for it -> knowledge deleted because food is in FOV
                    currentAnimal.Knowledge.LastFoundedFood = null;
                }
                else if (seenObject.GetComponent<Food>() != null && !seenObject.GetComponent<Food>().IsOver() && seenObject.GetComponent<Food>().HasFreeSpot() && currentAnimal.CurrentGoal != null && currentAnimal.CurrentGoal.Name != GoalName.Food)
                {
                    // food in FOV available to eat and when the zebra is searching another resource -> knowledge added for future search of food
                    currentAnimal.Knowledge.LastFoundedFood = seenObject.transform;
                    foodChecked = true;
                }
                else if (seenObject.GetComponent<Food>() != null && (seenObject.GetComponent<Food>().IsOver() || !seenObject.GetComponent<Food>().HasFreeSpot()) && knownFoodDistance < Radius)
                {
                    // food in FOV not available to eat and when the zebra arrived to the known place with food -> knowledge deleted because previous knoledge for food led to an unavailable resource
                    currentAnimal.Knowledge.LastFoundedFood = null;
                }

                if (seenObject.GetComponent<Water>() != null && !seenObject.GetComponent<Water>().IsOver() && seenObject.GetComponent<Water>().HasFreeSpot() && currentAnimal.CurrentGoal != null && currentAnimal.CurrentGoal.Name == GoalName.Water)
                {
                    // water in FOV available to drink and when the zebra is searching for it -> knowledge deleted because water is in FOV
                    currentAnimal.Knowledge.LastFoundedWater = null;
                }
                else if (seenObject.GetComponent<Water>() != null && !seenObject.GetComponent<Water>().IsOver() && seenObject.GetComponent<Water>().HasFreeSpot() && currentAnimal.CurrentGoal != null && currentAnimal.CurrentGoal.Name != GoalName.Water)
                {
                    // water in FOV available to drink and when the zebra is searching another resource -> knowledge added for future search of water
                    currentAnimal.Knowledge.LastFoundedWater = seenObject.transform;
                    waterChecked = true;
                }
                else if (seenObject.GetComponent<Water>() != null && (seenObject.GetComponent<Water>().IsOver() || !seenObject.GetComponent<Water>().HasFreeSpot()) && knownWaterDistance < Radius)
                {
                    // water in FOV not available to drink and when the zebra arrived to the known place with water -> knowledge deleted because previous knoledge for water led to an unavailable resource
                    currentAnimal.Knowledge.LastFoundedWater = null;
                }

                index++;
            }
        }

        // get the nearer not over food object in FOV with at least a free spot
        public GameObject GetNearerFreeFood()
        {
            GameObject nearerFreeFood = null;
            float nearerDistance = float.PositiveInfinity;

            // check current FOV for food
            for (int i = 0; i < ObjectsInFOV.Count; i++)
            {
                GameObject seenObject = ObjectsInFOV[i];
                float seenObjectDistance = Vector3.Distance(gameObject.transform.position, seenObject.transform.position);
                if (seenObject.GetComponent<Food>() != null && seenObjectDistance <= nearerDistance && seenObject.GetComponent<Food>().HasFreeSpot() && !seenObject.GetComponent<Food>().IsOver())
                {
                    nearerFreeFood = seenObject;
                    nearerDistance = seenObjectDistance;
                }
            }

            return nearerFreeFood;
        }

        // get the nearer not over water object in FOV with at least a free spot
        public GameObject GetNearerFreeWater()
        {
            GameObject nearerFreeWater = null;
            float nearerDistance = float.PositiveInfinity;

            // check current FOV for water
            for (int i = 0; i < ObjectsInFOV.Count; i++)
            {
                GameObject seenObject = ObjectsInFOV[i];
                float seenObjectDistance = Vector3.Distance(gameObject.transform.position, seenObject.transform.position);
                if (seenObject.GetComponent<Water>() != null && seenObjectDistance <= nearerDistance && seenObject.GetComponent<Water>().HasFreeSpot() && !seenObject.GetComponent<Water>().IsOver())
                {
                    nearerFreeWater = seenObject;
                    nearerDistance = seenObjectDistance;
                }
            }

            return nearerFreeWater;
        }

        // get near Zebras
        public List<GameObject> GetZebrasInFOV()
        {
            List<GameObject> zebras = new List<GameObject>();
            
            for (int i = 0; i < ObjectsInFOV.Count; i++)
            {
                if (ObjectsInFOV[i].GetComponent<Zebra>() != null)
                    zebras.Add(ObjectsInFOV[i]);
            }

            return zebras;
        }
    }
}