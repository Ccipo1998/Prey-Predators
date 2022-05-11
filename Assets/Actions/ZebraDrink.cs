using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;

namespace Assets.Actions
{
    public class ZebraDrink : MonoBehaviour
    {
        // aimed water object
        public GameObject WaterToDrink;
        // the current spot assegned to the animal basing on current position
        public ResourceSpot AssignedSpot;
        // drink rate
        public float DrinkVelocity = 1.0f;

        // static components
        private Coroutine coroutine;

        // Use this for initialization
        void Start()
        {

        }

        // Called on enabling
        private void OnEnable()
        {
            // get food info when enabled
            WaterToDrink = gameObject.GetComponent<ZebraSearchWater>().CurrentNearerFreeWater;
            AssignedSpot = gameObject.GetComponent<ZebraSearchWater>().CurrentNearerFreeSpot;

            // start the coroutine to add food to animal and subtract food to the resource
            coroutine = StartCoroutine(DrinkAndConsume());
        }

        private IEnumerator DrinkAndConsume()
        {
            while (true)
            {
                WaterToDrink.GetComponent<Water>().Consume(1);
                gameObject.GetComponent<Zebra>().Drink(1);
                yield return new WaitForSeconds(1 / DrinkVelocity);
            }
        }

        // Called on disabling
        private void OnDisable()
        {
            // end the coroutine
            StopCoroutine(coroutine);
            // free the spot after a while -> so the animal moved away a little from the spot position TODO
            AssignedSpot.Free();
        }
    }
}