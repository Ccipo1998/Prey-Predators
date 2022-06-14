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
        SearchingPartner, CheckingPartner, ApproachingPartner, Reproducing
    }

    public class ZebraReproduction : MonoBehaviour
    {
        // reproduction parameters
        public GameObject Partner;
        private bool Reproduced;

        // static parameters
        private ReproductionStatus Status;
        private ZebraFOV CurrentFOV;
        private NavMeshAgent CurrentNavMeshAgent;

        public bool IsReproduced()
        {
            return Reproduced;
        }

        private void OnEnable()
        {
            CurrentFOV = GetComponent<ZebraFOV>();
            CurrentNavMeshAgent = GetComponent<NavMeshAgent>();
            Status = ReproductionStatus.SearchingPartner;
            Reproduced = false;
        }

        // Update is called once per frame
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
                        Status = ReproductionStatus.CheckingPartner;
                    }

                    break;

                case ReproductionStatus.CheckingPartner:

                    if (Partner.GetComponent<ZebraReproduction>().Partner == gameObject)
                        Status = ReproductionStatus.ApproachingPartner;

                    break;

                case ReproductionStatus.ApproachingPartner:

                    float distance = Vector3.Distance(CurrentPosition, Partner.transform.position);
                    if (distance < CurrentNavMeshAgent.stoppingDistance)
                    {
                        CurrentNavMeshAgent.ResetPath();
                        Status = ReproductionStatus.Reproducing;
                    }
                    else if (!CurrentNavMeshAgent.hasPath)
                    {
                        Vector3 dest = (Partner.transform.position + gameObject.transform.position) / 2;
                        dest.Set(dest.x - CurrentNavMeshAgent.stoppingDistance, dest.y - CurrentNavMeshAgent.stoppingDistance, dest.z - CurrentNavMeshAgent.stoppingDistance);
                        CurrentNavMeshAgent.destination = dest;
                    }

                    break;

                case ReproductionStatus.Reproducing:

                    bool partnerReproduced = Partner.GetComponent<ZebraReproduction>().IsReproduced();

                    if (!partnerReproduced && !Reproduced)
                    {
                        // questa zebra si riproduce
                        GameObject zebraClone = Instantiate(gameObject);
                        zebraClone.transform.position = gameObject.transform.position + gameObject.transform.right * 1.5f;
                    }

                    Reproduced = true;
                    gameObject.GetComponent<Zebra>().Sociality /= 2;

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