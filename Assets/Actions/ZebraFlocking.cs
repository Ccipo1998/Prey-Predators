using Assets.FOV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraFlocking : MonoBehaviour
    {
        // static components
        private ZebraSearchFlock SearchFlockBehavior;
        private NavMeshAgent CurrentNavMeshAgent;
        private ZebraFOV CurrentFOV;

        // flocking parameters
        public float SeparationFactor = 2.0f;

        private void InitComponents()
        {
            // get and initialize searching behavior
            SearchFlockBehavior = gameObject.GetComponent<ZebraSearchFlock>();
            SearchFlockBehavior.InitComponents();

            CurrentNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            CurrentFOV = gameObject.GetComponent<ZebraFOV>();
        }

        private void ClearParameters()
        {
            SearchFlockBehavior.ClearParameters();
        }

        // Use this for initialization
        void Start()
        {
            InitComponents();
        }

        private void OnEnable()
        {
            InitComponents();
            ClearParameters();
        }

        // Update is called once per frame
        void Update()
        {
            // search flock
            bool founded = SearchFlockBehavior.Search();
            if (founded)
            {
                // start flocking behavior
                Flocking();
            }
        }

        private void Flocking()
        {
            // TODO: considerare solo le zebre che sono nello stato happiness
            int zebrasNumber = CurrentFOV.CurrentZebrasNumber;
            Vector3 posAvg = Vector3.zero;
            Vector3 dirAvg = Vector3.zero;
            Vector3 distAvg = Vector3.zero;

            List<GameObject> zebras = CurrentFOV.GetZebrasInFOV();
            for (int i = 0; i < zebrasNumber; i++)
            {
                posAvg += zebras[i].transform.position;
                dirAvg += zebras[i].transform.forward;

                float distance = Vector3.Distance(zebras[i].transform.position, gameObject.transform.position);
                if (distance < SeparationFactor)
                    distAvg += (gameObject.transform.position - zebras[i].transform.position).normalized;
            }

            posAvg /= zebrasNumber;
            dirAvg /= zebrasNumber;
            //distAvg /= zebrasNumber;

            Vector3 v = gameObject.transform.forward.normalized;
            Vector3 u = posAvg - gameObject.transform.position;

            //CurrentNavMeshAgent.destination = gameObject.transform.position + (Quaternion.AngleAxis(Vector3.Angle(v, u) * Time.deltaTime * CurrentNavMeshAgent.angularSpeed, gameObject.transform.up) * v);
            CurrentNavMeshAgent.destination = gameObject.transform.position + (u.normalized + dirAvg.normalized + distAvg.normalized).normalized * 3.0f;
        }

        private void OnDisable()
        {
            // disable current nav mesh
            if (CurrentNavMeshAgent.hasPath)
                CurrentNavMeshAgent.ResetPath();
        }
    }
}