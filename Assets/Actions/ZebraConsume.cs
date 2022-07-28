using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;

namespace Assets.Actions
{
    public enum ConsumingStatus
    {
        Consuming, Done
    }

    public class ZebraConsume : MonoBehaviour
    {
        // aimed water object
        public GameObject ResourceToConsume;
        // the current spot assegned to the animal basing on current position
        public ResourceSpot AssignedSpot;
        // status
        public ConsumingStatus Status;
        // choosen coroutine
        public Coroutine Coroutine;
        // resource target value for consume duration
        public int ResourceTarget = 0;
        public Zebra CurrentZebra;

        private void Update()
        {
            transform.position = AssignedSpot.Position;
            gameObject.transform.LookAt(ResourceToConsume.transform.position, gameObject.transform.up);
        }

        public void ClearComponents()
        {
            // end the coroutine
            StopCoroutine(Coroutine);
            // standard exit status
            Status = ConsumingStatus.Done;
        }

        public void FreeSpot()
        {
            // free the spot after a while -> so the animal moved away a little from the spot position TODO
            AssignedSpot.Free();
        }
    }
}