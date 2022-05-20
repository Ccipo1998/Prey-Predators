using Assets.FOV;
using Assets.Species;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.BackgroundBehaviours
{
    public class ZebraSociality : MonoBehaviour
    {
        // static components
        private ZebraFOV CurrentFOV;
        private Zebra CurrentZebra;

        // Use this for initialization
        void Start()
        {
            CurrentFOV = gameObject.GetComponent<ZebraFOV>();
            CurrentZebra = gameObject.GetComponent<Zebra>();

            // increase or decrease sociality basing on number of near Zebras (i.e. in FOV)
            InvokeRepeating("UpdateSociality", 1, 4);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateSociality()
        {
            List<GameObject> zebrasInFOV = CurrentFOV.GetZebrasInFOV();
            CurrentZebra.Sociality += 1 * (zebrasInFOV.Count - 1);
        }
    }
}