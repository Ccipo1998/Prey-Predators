using Assets.FOV;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraSearchFood : MonoBehaviour
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

            // check if there is a food source in current FOV
            GameObject nearFood = gameObject.GetComponent<ZebraFOV>().NearestFoodInFOV;
            if (nearFood != null)
            {
                //GoToFood();
            }

            // check if the animal knows already a position of a food source
            else if (currentKnowledge.LastFoundedFood != null)
            {
                gameObject.GetComponent<NavMeshAgent>().destination = currentKnowledge.LastFoundedFood.position;
            }
        }
    }
}