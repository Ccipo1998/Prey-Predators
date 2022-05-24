using Assets.FOV;
using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraSearchWater : MonoBehaviour
    {

        // static components
        private NavMeshAgent CurrentNavMeshAgent;
        private Knowledge CurrentKnowledge;
        private ZebraFOV CurrentZebraFOV;

        private bool InQueue = false;

        // searching info
        public GameObject CurrentNearerFreeWater;
        public ResourceSpot CurrentNearerFreeSpot;
        public SearchingStatus Status;

        // Use this for initialization
        void Start()
        {
            // get static components
            CurrentNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            CurrentKnowledge = gameObject.GetComponent<Zebra>().Knowledge;
            CurrentZebraFOV = gameObject.GetComponent<ZebraFOV>();
            Status = SearchingStatus.Searching;

            // clear previous path
            CurrentNavMeshAgent.ResetPath();
        }

        private void OnEnable()
        {

        }

        void Update()
        {
            Vector3 currentPosition = transform.position;

            // check if the animal arrived to the spot
            if (Status == SearchingStatus.Arrived)
            {
                //CurrentNavMeshAgent.ResetPath();

                //Positioned = true;
                //transform.position = CurrentNearerFreeSpot.Position;
                //transform.forward = Vector3.Normalize(CurrentNearerFreeWater.transform.position - transform.position);

                //CurrentNavMeshAgent.isStopped = true;
                //CurrentNavMeshAgent.velocity = Vector3.zero;
                //CurrentNavMeshAgent.angularSpeed = 0f;
                //CurrentRigidBody.velocity = Vector3.zero;
                //CurrentRigidBody.angularVelocity = Vector3.zero;
                //CurrentRigidBody.transform.position = CurrentNearerFreeSpot.Position;
                //CurrentRigidBody.transform.forward = Vector3.Normalize(CurrentNearerFreeWater.GetComponent<Rigidbody>().position - CurrentRigidBody.transform.position);
                return;
            }

            // check if the animal can request a spot
            if (CurrentNearerFreeSpot != null && Vector3.Distance(currentPosition, CurrentNearerFreeSpot.Position) < (CurrentNavMeshAgent.stoppingDistance) && CurrentNearerFreeSpot.IsFree)
            {
                // in proximity of the free spot -> in the queue for the spot, if not already
                /*
                if (Status == SearchingStatus.Searching)
                {
                    CurrentNearerFreeSpot.ToQueue(gameObject);
                    Status = SearchingStatus.InQueue;
                }
                else if (Status == SearchingStatus.InQueue)
                {
                    GameObject assigned = CurrentNearerFreeSpot.AssignSpot();
                    if (assigned == gameObject)
                    {
                        CurrentNearerFreeSpot.ClearQueue();
                        CurrentNearerFreeSpot.IsFree = false;
                        Status = SearchingStatus.Arrived;
                    }
                }
                */

                if (!InQueue)
                {
                    CurrentNearerFreeSpot.ToQueue(gameObject);
                    InQueue = true;
                }
                else if (Status == SearchingStatus.Searching)
                {
                    GameObject assigned = CurrentNearerFreeSpot.AssignSpot();
                    if (assigned == gameObject)
                    {
                        CurrentNearerFreeSpot.ClearQueue();
                        CurrentNearerFreeSpot.IsFree = false;
                        Status = SearchingStatus.Arrived;

                        transform.position = CurrentNearerFreeSpot.Position;
                        transform.forward = Vector3.Normalize(CurrentNearerFreeWater.transform.position - transform.position);
                    }
                }
            }
            else
            {
                // reset status
                Status = SearchingStatus.Searching;

                // check water and spot in FOV
                GameObject nearerFreeWater = CurrentZebraFOV.GetNearerFreeWater();
                if (nearerFreeWater != null)
                {
                    ResourceSpot nearerFreeSpot = nearerFreeWater.GetComponent<Water>().GetNearerFreeSpot(currentPosition);
                    // check if the spot changed
                    if (nearerFreeSpot != CurrentNearerFreeSpot)
                    {
                        CurrentNearerFreeWater = nearerFreeWater;
                        CurrentNearerFreeSpot = nearerFreeSpot;
                        CurrentNavMeshAgent.destination = CurrentNearerFreeSpot.Position;
                        CurrentNavMeshAgent.isStopped = false;
                    }
                    else
                    {
                        CurrentNavMeshAgent.destination = CurrentNearerFreeSpot.Position;
                        CurrentNavMeshAgent.isStopped = false;
                    }
                }
                // else search basing on knowledge
                else if (CurrentKnowledge.LastFoundedWater != null)
                {
                    // clear current water/spot
                    CurrentNearerFreeWater = null;
                    CurrentNearerFreeSpot = null;

                    CurrentNavMeshAgent.destination = CurrentKnowledge.LastFoundedWater.position;
                }
                // else wandering
                else
                {
                    CurrentNavMeshAgent.destination = new Vector3(10.0f, 0.0f, 0.0f);
                }
            }
        }

        // Called at disabling
        private void OnDisable()
        {
            // clear status and parameters
            Status = SearchingStatus.Searching;
            InQueue = false;
            // disable current nav mesh
            if (CurrentNavMeshAgent.hasPath)
                CurrentNavMeshAgent.ResetPath();
        }
    }
}