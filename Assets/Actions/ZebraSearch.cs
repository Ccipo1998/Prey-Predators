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

    public enum SearchingStatus
    {
        SearchingResource, SearchingSpot, Arrived
    }

    public class ZebraSearch : MonoBehaviour
    {
        // static components
        private NavMeshAgent CurrentNavMeshAgent;
        private Knowledge CurrentKnowledge;
        private ZebraFOV CurrentZebraFOV;
        private Vector3 CurrentPosition;

        private bool InQueue = false;

        // searching info
        public GameObject AssignedResource;
        public ResourceSpot AssignedSpot;
        public SearchingStatus Status;

        private GameObject CurrentNearerFreeResource;
        private ResourceSpot CurrentNearerFreeSpot;

        // Use this for initialization
        public void InitComponents()
        {
            // get static components
            CurrentNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            CurrentKnowledge = gameObject.GetComponent<Zebra>().Knowledge;
            CurrentZebraFOV = gameObject.GetComponent<ZebraFOV>();
            Status = SearchingStatus.SearchingResource;
        }

        public void ClearParameters()
        {
            // clear status and parameters
            Status = SearchingStatus.SearchingResource;
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
                case SearchingStatus.SearchingResource:

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
                        Status = SearchingStatus.SearchingSpot;
                    }
                    // else search basing on knowledge
                    else if (searchType == SearchType.Food && CurrentKnowledge.LastFoundedFood != null)
                    {
                        // clear current resource/spot
                        //CurrentNearerFreeResource = null;
                        //CurrentNearerFreeSpot = null;

                        CurrentNavMeshAgent.destination = CurrentKnowledge.LastFoundedFood.position;
                    }
                    else if (searchType == SearchType.Water && CurrentKnowledge.LastFoundedWater != null)
                    {
                        // clear current resource/spot
                        //CurrentNearerFreeResource = null;
                        //CurrentNearerFreeSpot = null;

                        CurrentNavMeshAgent.destination = CurrentKnowledge.LastFoundedWater.position;
                    }
                    else
                    {
                        // else wandering
                        CurrentNavMeshAgent.destination = new Vector3(10.0f, 0.0f, 0.0f);
                    }

                    break;

                case SearchingStatus.SearchingSpot:

                    // get nearer free spot
                    ResourceSpot nearerFreeSpot = CurrentNearerFreeResource.GetComponent<Resource>().GetNearerFreeSpot(CurrentPosition);

                    if (nearerFreeSpot != null)
                    {
                        // there is still a free spot -> go to it
                        CurrentNearerFreeSpot = nearerFreeSpot;
                        CurrentNavMeshAgent.destination = CurrentNearerFreeSpot.Position;

                        // if near to free spot -> request the spot
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
                                    Status = SearchingStatus.Arrived;

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
                        Status = SearchingStatus.SearchingResource;
                    }

                    break;

                case SearchingStatus.Arrived:

                    // set position and look at
                    transform.position = CurrentNearerFreeSpot.Position;
                    gameObject.transform.LookAt(CurrentNearerFreeResource.transform.position, gameObject.transform.up);

                    break;
            }
            /*
            // check if the animal can request a spot
            if (IsNearFreeSpot())
            {
                // if the animal is not in queue for a free spot -> add to queue
                if (!InQueue)
                {
                    CurrentNearerFreeSpot.ToQueue(gameObject);
                    InQueue = true;
                }
                // if the animal is already in queue for a free spot -> request spot assignment
                else if (Status == SearchingStatus.Searching)
                {
                    GameObject assigned = CurrentNearerFreeSpot.AssignSpot();
                    if (assigned == gameObject)
                    {
                        // clear queue and occupy the spot
                        CurrentNearerFreeSpot.ClearQueue();
                        CurrentNearerFreeSpot.Occupy();
                        Status = SearchingStatus.Arrived;

                        // set position and look at
                        transform.position = CurrentNearerFreeSpot.Position;
                        gameObject.transform.LookAt(CurrentNearerFreeResource.transform.position, gameObject.transform.up);
                    }
                }
            }
            else
            {
                // reset status
                Status = SearchingStatus.Searching;

                // check resource and spot in FOV
                GameObject nearerFreeResource;
                if (searchType == SearchType.Food)
                    nearerFreeResource = CurrentZebraFOV.GetNearerFreeFood();
                else
                    nearerFreeResource = CurrentZebraFOV.GetNearerFreeWater();

                // free spot found
                if (nearerFreeResource != null)
                {
                    ResourceSpot nearerFreeSpot = nearerFreeResource.GetComponent<Resource>().GetNearerFreeSpot(CurrentPosition);
                    // check if the spot changed
                    if (nearerFreeSpot != CurrentNearerFreeSpot)
                    {
                        CurrentNearerFreeResource = nearerFreeResource;
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
                else if (searchType == SearchType.Food && CurrentKnowledge.LastFoundedFood != null)
                {
                    // clear current resource/spot
                    CurrentNearerFreeResource = null;
                    CurrentNearerFreeSpot = null;

                    CurrentNavMeshAgent.destination = CurrentKnowledge.LastFoundedFood.position;
                }
                else if (searchType == SearchType.Water && CurrentKnowledge.LastFoundedWater != null)
                {
                    // clear current resource/spot
                    CurrentNearerFreeResource = null;
                    CurrentNearerFreeSpot = null;

                    CurrentNavMeshAgent.destination = CurrentKnowledge.LastFoundedWater.position;
                }
                // else wandering
                else
                {
                    CurrentNavMeshAgent.destination = new Vector3(10.0f, 0.0f, 0.0f);
                }
            }
            */
        }

        private void OnDisable()
        {
            // if the spot is assigned but the goal change suddenly
            if (AssignedSpot != null)
                AssignedSpot.Free();
        }
    }
}