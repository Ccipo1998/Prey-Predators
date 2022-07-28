using Assets.FOV;
using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public enum SearchType
    {
        Food, Water
    }

    public enum SearchingResourceStatus
    {
        SearchingResource, SearchingSpot, Arrived
    }

    public class ZebraSearchResource : MonoBehaviour
    {
        // static components
        private NavMeshAgent CurrentNavMeshAgent;
        private Knowledge CurrentKnowledge;
        private ZebraFOV CurrentZebraFOV;
        private Vector3 CurrentPosition;

        private bool InQueue = false;
        private float LastWanderingTime;

        // searching info
        public GameObject AssignedResource;
        public ResourceSpot AssignedSpot;
        public SearchingResourceStatus Status;
        public float WanderingRate = 2.0f;
        public float MaxRotation = 90.0f;

        private GameObject CurrentNearerFreeResource;
        private ResourceSpot CurrentNearerFreeSpot;

        // Use this for initialization
        public void InitComponents()
        {
            // get static components
            CurrentNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            CurrentKnowledge = gameObject.GetComponent<Zebra>().Knowledge;
            CurrentZebraFOV = gameObject.GetComponent<ZebraFOV>();
            Status = SearchingResourceStatus.SearchingResource;
            LastWanderingTime = -WanderingRate;
        }

        public void ClearParameters()
        {
            // clear status and parameters
            Status = SearchingResourceStatus.SearchingResource;
            InQueue = false;

            // disable current nav mesh
            if (CurrentNavMeshAgent.hasPath)
                CurrentNavMeshAgent.ResetPath();

            // clear init parameters
            AssignedResource = null;
            AssignedSpot = null;
            CurrentNearerFreeResource = null;
            CurrentNearerFreeSpot = null;
        }

        private bool IsNearFreeSpot()
        {
            return CurrentNearerFreeSpot != null && Vector3.Distance(CurrentPosition, CurrentNearerFreeSpot.Position) < (CurrentNavMeshAgent.stoppingDistance) && CurrentNearerFreeSpot.IsFree;
        }

        public void Search(SearchType searchType)
        {
            CurrentPosition = transform.position;

            switch (Status)
            {
                case SearchingResourceStatus.SearchingResource:

                    // check resource in FOV
                    GameObject nearerFreeResource;
                    if (searchType == SearchType.Food)
                        nearerFreeResource = CurrentZebraFOV.GetNearerFreeFood();
                    else
                        nearerFreeResource = CurrentZebraFOV.GetNearerFreeWater();

                    if (nearerFreeResource != null)
                    {
                        // resource with at least one free spot found -> search nearer free spot
                        CurrentNearerFreeResource = nearerFreeResource;
                        Status = SearchingResourceStatus.SearchingSpot;
                    }
                    // else search basing on knowledge
                    else if (searchType == SearchType.Food && CurrentKnowledge.LastFoundedFood != null)
                    {
                        // clear current resource/spot
                        //CurrentNearerFreeResource = null;
                        //CurrentNearerFreeSpot = null;

                        CurrentNavMeshAgent.destination = (Vector3)CurrentKnowledge.LastFoundedFood;
                    }
                    else if (searchType == SearchType.Water && CurrentKnowledge.LastFoundedWater != null)
                    {
                        // clear current resource/spot
                        //CurrentNearerFreeResource = null;
                        //CurrentNearerFreeSpot = null;

                        CurrentNavMeshAgent.destination = (Vector3)CurrentKnowledge.LastFoundedWater;
                    }
                    else
                    {
                        // else wandering
                        Wander();

                        //CurrentNavMeshAgent.destination = new Vector3(10.0f, 0.0f, 0.0f);
                    }

                    break;

                case SearchingResourceStatus.SearchingSpot:

                    // get nearer free spot
                    ResourceSpot nearerFreeSpot = CurrentNearerFreeResource.GetComponent<Resource>().GetNearerFreeSpot(CurrentPosition);

                    if (nearerFreeSpot != null)
                    {
                        // there is still a free spot -> go to it
                        CurrentNearerFreeSpot = nearerFreeSpot;
                        CurrentNavMeshAgent.destination = CurrentNearerFreeSpot.Position;

                        // if near to free spot -> request the spot
                        //if (CurrentNavMeshAgent.velocity == Vector3.zero)
                        if (IsNearFreeSpot())
                        {
                            if (!InQueue)
                            {
                                // if not already in queue -> add to queue
                                CurrentNearerFreeSpot.ToQueue(gameObject);
                                InQueue = true;
                            }
                            else
                            {
                                // if already in queue -> check who is the spot for
                                GameObject assigned = CurrentNearerFreeSpot.AssignSpot();

                                if (assigned == gameObject)
                                {
                                    // spot assigned to current animal -> clear queue and occupy the spot
                                    CurrentNearerFreeSpot.ClearQueue();
                                    CurrentNearerFreeSpot.Occupy();
                                    Status = SearchingResourceStatus.Arrived;

                                    CurrentNavMeshAgent.ResetPath();

                                    AssignedResource = CurrentNearerFreeResource;
                                    AssignedSpot = CurrentNearerFreeSpot;
                                }
                                else
                                {
                                    // spot not assigned to current animal
                                    InQueue = false;
                                }
                            }
                        }
                        else
                        {
                            // far to free spot -> not in queue
                            InQueue = false;
                        }
                    }
                    else
                    {
                        // there is no more a free spot -> search another resource
                        Status = SearchingResourceStatus.SearchingResource;
                    }

                    break;

                case SearchingResourceStatus.Arrived:

                    // set position and look at
                    transform.position = CurrentNearerFreeSpot.Position;
                    gameObject.transform.LookAt(CurrentNearerFreeResource.transform.position, gameObject.transform.up);

                    break;
            }
        }

        public void Wander()
        {
            if (Time.time - LastWanderingTime > WanderingRate)
            {
                LastWanderingTime = Time.time;

                Vector3 randomDirection = Quaternion.AngleAxis(Random.Range(-MaxRotation, MaxRotation), gameObject.transform.up) * gameObject.transform.forward;

                CurrentNavMeshAgent.destination = gameObject.transform.position + randomDirection * (CurrentNavMeshAgent.acceleration * WanderingRate);
            }
        }

        private void OnDisable()
        {
            // if the spot is assigned but the goal change suddenly
            if (AssignedSpot != null)
                AssignedSpot.Free();
        }
    }
}