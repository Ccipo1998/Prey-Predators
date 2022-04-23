﻿using Assets.Resources;
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

        // objects of interest
        public GameObject NearestFoodInFOV;
        public GameObject NearestWaterInFOV;

        // Use this for initialization
        void Start()
        {
            TargetsMask = LayerMask.GetMask(new string[] { "Targets" });
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            FindVisible();
            AddKnowledge();
        }

        // find visible objects in current FOV
        public void FindVisible()
        {
            // clear previous
            ObjectsInFOV.Clear();

            Vector3 currentPosition = gameObject.GetComponent<Rigidbody>().position;
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

        // add positions to internal knowledge and objects of interest in current FOV
        public void AddKnowledge()
        {
            // params to clear near objects of interest
            bool isFoodNear = false;

            Zebra currentAnimal = gameObject.GetComponent<Zebra>();
            Vector3 currentPosition = gameObject.GetComponent<Rigidbody>().position;

            for (int i = 0; i < ObjectsInFOV.Count; i++)
            {
                GameObject seenObject = ObjectsInFOV[i];
                float objectDistance = Vector3.Distance(currentPosition, seenObject.GetComponent<Rigidbody>().position);

                if (seenObject.GetComponent<Food>() != null)
                {
                    // there is food in current FOV
                    isFoodNear = true;

                    float lastFoodDistance = currentAnimal.Knowledge.LastFoundedFood != null ? Vector3.Distance(currentPosition, currentAnimal.Knowledge.LastFoundedFood.position) : float.PositiveInfinity;
                    if (objectDistance <= lastFoodDistance)
                    {
                        // position info in internal knowledge (for searching)
                        currentAnimal.Knowledge.LastFoundedFood = seenObject.GetComponent<Rigidbody>().transform;

                        // gameobject info of actual food in current FOV, for actions on that
                        NearestFoodInFOV = seenObject;
                    }
                }
                //else if (seenObject.GetComponent<Water>() != null) // seen water position TODO
                // else if (seenObject.GetComponent<Lion>() != null) // seen predator position TODO
                // else if (seenObject.GetComponent<Zebra>() != null) // seen similar position TODO
            }

            if (!isFoodNear)
                NearestFoodInFOV = null;
        }
    }
}