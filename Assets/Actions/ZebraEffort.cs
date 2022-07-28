using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraEffort : MonoBehaviour
    {
        // static components
        private Zebra CurrentZebra;
        private Coroutine Coroutine;

        // effort velocity
        public float EffortVelocity = 1.0f;
        public float EffortValue = 1.0f;

        private void OnEnable()
        {
            CurrentZebra = gameObject.GetComponent<Zebra>();

            // increase or decrease sociality basing on number of near Zebras (i.e. in FOV)
            Coroutine = StartCoroutine(UpdateEnergy());
        }

        private IEnumerator UpdateEnergy()
        {
            float effort = 0f;

            while (true)
            {
                // effort depends only on current velocty:
                // - if velocity is zero -> zebra is performing actions (with others efforts) or is sleeping, so no effort
                // - if velocity > 0 -> effort changes depending on velocity value
                effort += gameObject.GetComponent<NavMeshAgent>().speed * EffortValue / 2;
                CurrentZebra.Energy -= (int)effort;
                effort -= (int)effort;

                yield return new WaitForSeconds(1 / EffortVelocity);
            }
        }

        private void OnDisable()
        {
            StopCoroutine(Coroutine);
        }
    }
}