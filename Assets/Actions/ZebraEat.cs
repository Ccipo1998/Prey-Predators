using Assets.FOV;
using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraEat : MonoBehaviour
    {
        // aimed food object
        public GameObject FoodToEat;
        // the current spot assegned to the animal basing on current position
        public ResourceSpot AssignedSpot;
        // eating rate
        public float EatVelocity = 1.0f;

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
            FoodToEat = gameObject.GetComponent<ZebraSearchFood>().CurrentNearerFreeFood;
            AssignedSpot = gameObject.GetComponent<ZebraSearchFood>().CurrentNearerFreeSpot;

            // start the coroutine to add food to animal and subtract food to the resource
            coroutine = StartCoroutine(EatAndConsume());
        }

        private IEnumerator EatAndConsume()
        {
            while (true)
            {
                FoodToEat.GetComponent<Food>().Consume(1);
                gameObject.GetComponent<Zebra>().Eat(1);
                yield return new WaitForSeconds(1 / EatVelocity);
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