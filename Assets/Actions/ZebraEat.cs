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
        // status
        public EatingStatus Status;

        // static components
        private Coroutine coroutine;
        private Zebra CurrentZebra;

        // food target value for eating duration
        private int FoodTarget = 0;

        // Use this for initialization
        void Start()
        {
            
        }

        // Called on enabling
        private void OnEnable()
        {
            Status = EatingStatus.Eating;
            // static components init
            CurrentZebra = gameObject.GetComponent<Zebra>();

            // get food info when enabled
            FoodToEat = gameObject.GetComponent<ZebraSearchFood>().CurrentNearerFreeFood;
            AssignedSpot = gameObject.GetComponent<ZebraSearchFood>().CurrentNearerFreeSpot;

            // start the coroutine to add food to animal and subtract food to the resource
            coroutine = StartCoroutine(EatAndConsume());
        }

        private IEnumerator EatAndConsume()
        {
            while (CurrentZebra.Food < FoodTarget)
            {
                for (int i = 0; i < EatVelocity; i++)
                {
                    FoodToEat.GetComponent<Food>().Consume(1);
                    CurrentZebra.Eat(1);
                }
                yield return new WaitForSeconds(1);
            }
            
            Status = EatingStatus.Done;
        }

        // Called on disabling
        private void OnDisable()
        {
            // end the coroutine
            StopCoroutine(coroutine);
            // free the spot after a while -> so the animal moved away a little from the spot position TODO
            AssignedSpot.Free();
            // standard exit status
            Status = EatingStatus.Done;
        }

        public void SetFoodTarget(int foodTarget)
        {
            FoodTarget = foodTarget;
        }
    }

    public enum EatingStatus
    {
        Eating, Done
    }
}