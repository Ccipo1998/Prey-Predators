using Assets.FOV;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraSearchWater : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            this.enabled = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Knowledge currentKnowledge = gameObject.GetComponent<Zebra>().Knowledge;

            // check if there is a water source in current FOV
            GameObject nearWater = gameObject.GetComponent<ZebraFOV>().NearestWaterInFOV;
            if (nearWater != null)
            {
                //GoToWater();
            }

            // check if the animal knows already a position of a food source
            else if (currentKnowledge.LastFoundedWater != null)
            {
                gameObject.GetComponent<NavMeshAgent>().destination = currentKnowledge.LastFoundedWater.position;
            }
        }

        // Called at disabling
        private void OnDisable()
        {
            // disable current nav mesh
            NavMeshAgent currentAgent = gameObject.GetComponent<NavMeshAgent>();
            if (currentAgent.hasPath)
                currentAgent.ResetPath();
        }
    }
}