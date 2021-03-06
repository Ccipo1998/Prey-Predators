using Assets.FOV;
using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraEat : ZebraConsume
    {
        // eat rate
        public float ConsumeVelocity = 1.0f;
        // energy loss rate
        public float EffortPerUnit = .7f;

        // Use this for initialization
        void Start()
        {

        }

        // Called on enabling
        private void OnEnable()
        {
            InitComponents();

            // start the coroutine to add water to animal and subtract water to the resource
            Coroutine = StartCoroutine(EatAndConsume());
        }

        private IEnumerator EatAndConsume()
        {
            float effort = 0.0f;

            while (CurrentZebra.Food < ResourceTarget && !ResourceToConsume.GetComponent<Resource>().IsOver())
            {
                ResourceToConsume.GetComponent<Resource>().Consume(1);
                gameObject.GetComponent<Zebra>().Eat(1);
                effort += EffortPerUnit;
                gameObject.GetComponent<Zebra>().Effort((int)effort);
                effort -= Mathf.Floor(effort);

                yield return new WaitForSeconds(1 / ConsumeVelocity);
            }

            Status = ConsumingStatus.Done;
        }

        // Called on disabling
        private void OnDisable()
        {
            FreeSpot();
            ClearComponents();
        }

        public void SetFoodTarget(int foodTarget)
        {
            ResourceTarget = foodTarget;
        }

        private void InitComponents()
        {
            Status = ConsumingStatus.Consuming;
            // static components init
            CurrentZebra = gameObject.GetComponent<Zebra>();

            // get food info when enabled
            ResourceToConsume = gameObject.GetComponent<ZebraSearchFood>().AssignedResource;
            AssignedSpot = gameObject.GetComponent<ZebraSearchFood>().AssignedSpot;
        }
    }
}