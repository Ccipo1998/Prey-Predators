using Assets.FOV;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;
using Assets.Species;

namespace Assets.Actions
{
    public enum ReproductionStatus
    {
        SearchingPartner, ApproachingPartner, Reproducing
    }

    public class ZebraReproduction : MonoBehaviour
    {
        // reproduction parameters
        public GameObject Partner;
        //private bool Reproduced;

        // static parameters
        private ReproductionStatus Status;
        private ZebraFOV CurrentFOV;
        private NavMeshAgent CurrentNavMeshAgent;

        /*
        public bool IsReproduced()
        {
            return Reproduced;
        }
        */

        private void OnEnable()
        {
            CurrentFOV = GetComponent<ZebraFOV>();
            CurrentNavMeshAgent = GetComponent<NavMeshAgent>();
            Status = ReproductionStatus.SearchingPartner;
            //Reproduced = false;
        }

        // Update is called once per frame
        // TODO: FIX REPRODUCTION
        void Update()
        {
            Vector3 CurrentPosition = gameObject.transform.position;

            switch (Status)
            {
                case ReproductionStatus.SearchingPartner:

                    GameObject potentialPartner = CurrentFOV.GetNearerZebraForReproduction();
                    if (potentialPartner != null)
                    {
                        Partner = potentialPartner;
                        Status = ReproductionStatus.ApproachingPartner;
                    }

                    break;

                case ReproductionStatus.ApproachingPartner:

                    // first check if partner is still available
                    if (Partner.GetComponent<ZebraReproduction>().Partner == gameObject)
                    {
                        //float distance = Vector3.Distance(CurrentPosition, Partner.transform.position);
                        if (CurrentNavMeshAgent.velocity == Vector3.zero && Partner.GetComponent<NavMeshAgent>().velocity == Vector3.zero)
                        {
                            CurrentNavMeshAgent.ResetPath();
                            Status = ReproductionStatus.Reproducing;
                        }
                        else if (!CurrentNavMeshAgent.hasPath)
                        {
                            Vector3 dest = (Partner.transform.position + gameObject.transform.position) / 2;
                            //dest.Set(dest.x - CurrentNavMeshAgent.stoppingDistance, dest.y - CurrentNavMeshAgent.stoppingDistance, dest.z - CurrentNavMeshAgent.stoppingDistance);
                            CurrentNavMeshAgent.destination = dest;
                        }
                    }
                    else
                    {
                        Status = ReproductionStatus.SearchingPartner;
                        CurrentNavMeshAgent.ResetPath();
                        Partner = null;
                    }
                    /*
                    else
                    {
                        Status = ReproductionStatus.SearchingPartner;
                    }
                    */

                    break;

                case ReproductionStatus.Reproducing:

                    if (gameObject.GetComponent<Zebra>().Energy > 60 && Partner.GetComponent<Zebra>().Energy > 60)
                    {
                        // this zebra reproduce
                        GameObject zebraClone = Instantiate(Spawner.GetZebraPrefab());
                        zebraClone.transform.position = gameObject.transform.position + gameObject.transform.right * 1.5f;
                        //Spawner.ResetZebra(zebraClone);

                        gameObject.GetComponent<Zebra>().Energy -= 20;
                        Partner.GetComponent<Zebra>().Energy -= 20;
                    }

                    /*
                    bool partnerReproduced = Partner.GetComponent<ZebraReproduction>().IsReproduced();

                    if (!partnerReproduced && !Reproduced)
                    {
                        // this zebra reproduce
                        GameObject zebraClone = Instantiate(gameObject);
                        zebraClone.GetComponent<Zebra>().Sociality = 50;
                        zebraClone.transform.position = gameObject.transform.position + gameObject.transform.right * 1.5f;
                    }

                    // TODO: aggiungere possibilità secondo figlio

                    Reproduced = true;
                    gameObject.GetComponent<Zebra>().Energy -= 20;
                    //gameObject.GetComponent<Zebra>().Sociality /= 2;
                    */

                    break;
            }
        }

        private void OnDisable()
        {
            Status = ReproductionStatus.SearchingPartner;
            Partner = null;
        }
    }
}