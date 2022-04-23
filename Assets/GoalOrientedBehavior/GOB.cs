using System.Collections;
using UnityEngine;
using Assets.Species;
using System.Collections.Generic;

namespace Assets.GoalOrientedBehavior
{
    public class GOB : MonoBehaviour
    {
        // for visualization
        public string ChoosenGoal;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            ChoosenGoal = ChooseGoal().Name.ToString();
        }

        // simple selection
        public Goal ChooseGoal()
        {
            List<Goal> goals = GetGoals();

            Goal choosen = goals[0];
            for (int i = 1; i < goals.Count; i++)
            {
                if (goals[i].Value < choosen.Value)
                    choosen = goals[i];
            }

            return choosen;
        }

        // return resources values of the current animal
        private List<Goal> GetGoals()
        {
            return new List<Goal> {
                new Goal(GoalName.Food, gameObject.GetComponent<Zebra>().Food),
                new Goal(GoalName.Water, gameObject.GetComponent<Zebra>().Water),
                new Goal(GoalName.Sociality, gameObject.GetComponent<Zebra>().Sociality),
                new Goal(GoalName.Space, gameObject.GetComponent<Zebra>().Space),
                new Goal(GoalName.Energy, gameObject.GetComponent<Zebra>().Energy)
            };
        }
    }
}