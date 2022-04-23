using System.Collections;
using UnityEngine;
using Assets.Behaviors;
using Assets.GoalOrientedBehavior;
using UnityEngine.AI;
using Assets.FOV;
using Assets.Actions;

namespace Assets.Species
{
    public class Zebra : Animal
    {
        public Zebra()
        {
            // animal type
            Type = AnimalType.Herbivore;

            // zebra rates
            FoodDecreaseRate = 5.0f;
            WaterDecreaseRate = 3.0f;

            // herbivore knowledge
            Knowledge = new HerbivoreKnowledge();
        }

        private void Start()
        {
            InvokeRepeating("DecreaseFood", 1, FoodDecreaseRate);
            InvokeRepeating("DecreaseWater", 5, WaterDecreaseRate);
        }

        // decrease food over time
        private void DecreaseFood()
        {
            if (Food > 0)
                Food--;
        }

        // decrease water over time
        private void DecreaseWater()
        {
            if (Water > 0)
                Water--;
        }

        public void Survive()
        {
            // get the next basic goal to survive
            Goal nextGoal = gameObject.GetComponent<ZebraGOB>().ChooseBasicGoal();

            switch (nextGoal.Name)
            {
                case GoalName.Food:
                    //gameObject.GetComponent<NavMeshAgent>().destination = GameObject.FindGameObjectWithTag("Grass").GetComponent<Rigidbody>().transform.position;
                    //SearchFood();
                    gameObject.GetComponent<ZebraSearchFood>().enabled = true;
                    break;

                case GoalName.Water:
                    break;

                case GoalName.Energy:
                    break;
            }
        }

        /*
        private void SearchFood()
        {
            // check if there is a food source in current FOV
            GameObject nearFood = gameObject.GetComponent<ZebraFOV>().NearestFoodInFOV;
            if (nearFood != null)
            {
                gameObject.GetComponent<NavMeshAgent>().ResetPath();

                //GoToFood();
            }

            // check if the animal knows already a position of a food source
            else if (Knowledge.LastFoundedFood != null)
            {
                gameObject.GetComponent<NavMeshAgent>().destination = Knowledge.LastFoundedFood;
            }
        }
        */
    }
}