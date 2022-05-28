using Assets.Resources;
using Assets.Species;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // TODO: l'ultima posizione di una risorsa conosciuta diventa null quando la si visita e non c'è più la risorsa disponibile
        // TODO: si aggiunge la posizione di una risorsa alla conoscenza solo quando si riesce a consumare quella risorsa, non solo quando la si vede
        // add positions to internal knowledge and objects of interest in current FOV
        public void AddKnowledge()
        {
            /*
            // params to clear near objects of interest
            bool isFoodNear = false;
            bool isWaterNear = false;
            */

            Zebra currentAnimal = gameObject.GetComponent<Zebra>();
            Vector3 currentPosition = transform.position;

            for (int i = 0; i < ObjectsInFOV.Count; i++)
            {
                GameObject seenObject = ObjectsInFOV[i];
                float objectDistance = Vector3.Distance(currentPosition, seenObject.transform.position);

                // seen object is food
                if (seenObject.GetComponent<Food>() != null)
                {
                    // there is food in current FOV
                    //isFoodNear = true;

                    float lastFoodDistance = currentAnimal.Knowledge.LastFoundedFood != null ? Vector3.Distance(currentPosition, currentAnimal.Knowledge.LastFoundedFood.position) : float.PositiveInfinity;
                    if (objectDistance <= lastFoodDistance)
                    {
                        // position info in internal knowledge (for searching)
                        currentAnimal.Knowledge.LastFoundedFood = seenObject.transform.transform;

                        // gameobject of actual food in current FOV, for actions on that
                        //NearestFoodInFOV = seenObject;
                    }
                }
                // seen object is water
                else if (seenObject.GetComponent<Water>() != null)
                {
                    // there is water in current FOV
                    //isWaterNear = true;

                    float lastWaterDistance = currentAnimal.Knowledge.LastFoundedWater != null ? Vector3.Distance(currentPosition, currentAnimal.Knowledge.LastFoundedWater.position) : float.PositiveInfinity;
                    if (objectDistance <= lastWaterDistance)
                    {
                        // position info in internal knowledge (for searching)
                        currentAnimal.Knowledge.LastFoundedWater = seenObject.transform.transform;

                        // gameobject of actual food in current FOV, for actions on that
                        //NearestWaterInFOV = seenObject;
                    }
                }
                // else if (seenObject.GetComponent<Lion>() != null) // seen predator position TODO
                // else if (seenObject.GetComponent<Zebra>() != null) // seen similar position TODO
            }

            /*
            // clear near objects
            if (!isFoodNear)
                NearestFoodInFOV = null;
            if (!isWaterNear)
                NearestWaterInFOV = null;
            */
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
                if (seenObject.GetComponent<Food>() != null && seenObjectDistance <= nearerDistance && seenObject.GetComponent<Food>().HasFreeSpot())// && !seenObject.GetComponent<Food>().IsOver())
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
                if (seenObject.GetComponent<Water>() != null && seenObjectDistance <= nearerDistance && seenObject.GetComponent<Water>().HasFreeSpot())// && !seenObject.GetComponent<Water>().IsOver())
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