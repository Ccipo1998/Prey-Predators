using Assets.FOV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Actions
{
    public class ZebraSearchFlock : MonoBehaviour
    {
        // static components
        private ZebraFOV CurrentFOV;

        // Use this for initialization
        void Start()
        {
            CurrentFOV = gameObject.GetComponent<ZebraFOV>();
        }

        // Update is called once per frame
        void Update()
        {
            // check FOV for near Zebras
            List<GameObject> zebrasInFOV = CurrentFOV.GetZebrasInFOV();
            if (zebrasInFOV.Count != 0)
            {
                // TODO: implement flocking
            }
        }
    }
}