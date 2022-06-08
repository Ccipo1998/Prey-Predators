using Assets.FOV;
using Assets.Species;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    // TODO: creare azione SearchFlock, utilizzabile sia da Leoni che da Zebre (fanno la stessa cosa) <- CREDO servirà un FOV comune sia per ZebraFOV che per LeoneFOV
    public class ZebraSearchFlock : MonoBehaviour
    {
        // static components
        private ZebraFOV CurrentFOV;
        private Vector3 CurrentPosition;
        private NavMeshAgent CurrentNavMeshAgent;
        private Knowledge CurrentKnowledge;
        private float LastWanderingTime;

        // wandering parameters
        public float WanderingRate = 2.0f;
        public float MaxRotation = 90.0f;

        public void InitComponents()
        {
            CurrentFOV = gameObject.GetComponent<ZebraFOV>();
            CurrentNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            CurrentKnowledge = gameObject.GetComponent<Zebra>().Knowledge;
        }

        public void ClearParameters()
        {
            // disable current nav mesh
            if (CurrentNavMeshAgent.hasPath)
                CurrentNavMeshAgent.ResetPath();
        }

        // return true if other zebras are founded -> i.e. if there are zebras in FOV
        public bool Search()
        {
            CurrentPosition = transform.position;

            // if similar in FOV -> flock founded
            if (CurrentFOV.ZebrasInFOV())
                return true;

            // search basing on knowledge
            if (CurrentKnowledge.LastSeenSimilar != null)
            {
                CurrentNavMeshAgent.destination = (Vector3)CurrentKnowledge.LastSeenSimilar;
            }
            // searching by wandering
            else
            {
                Wander();
            }

            return false;
        }

        private void Wander()
        {
            if (Time.time - LastWanderingTime > WanderingRate)
            {
                LastWanderingTime = Time.time;

                Vector3 randomDirection = Quaternion.AngleAxis(Random.Range(-MaxRotation, MaxRotation), gameObject.transform.up) * gameObject.transform.forward;

                CurrentNavMeshAgent.destination = gameObject.transform.position + randomDirection * (CurrentNavMeshAgent.acceleration * WanderingRate);
            }
        }
    }
}