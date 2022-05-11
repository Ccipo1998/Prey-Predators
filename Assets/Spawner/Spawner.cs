﻿using System.Collections;
using UnityEngine;
using Assets.Species;
using Assets.GoalOrientedBehavior;
using Assets.Behaviors;

namespace Assets.Spawner
{
    public class Spawner : MonoBehaviour
    {
        public int SpawnNumber = 2;

        // Use this for initialization
        void Start()
        {
            GameObject Zebra = GameObject.FindGameObjectWithTag("Zebra");
            //Zebra.GetComponent<Rigidbody>().transform.position = Vector3.zero + new Vector3(0.0f, 1.0f, 0.0f);

            // adding all the components
            //Zebra.AddComponent<Zebra>();
            //Zebra.AddComponent<GOB>();
            //Zebra.AddComponent<ZebraGOB>();
            //Zebra.AddComponent<ZebraBehavior>();

            
            for (int i = 1; i < SpawnNumber; i++)
            {
                GameObject zebraClone = Instantiate(Zebra);
                zebraClone.GetComponent<Rigidbody>().transform.position = Zebra.GetComponent<Rigidbody>().transform.position + new Vector3((float)i * 2, 1.0f, 0.0f);
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}