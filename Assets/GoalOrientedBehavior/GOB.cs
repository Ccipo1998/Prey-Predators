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
            ChoosenGoal = ChooseGoal().Name;
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
                new Goal("Food", gameObject.GetComponent<Zebra>().Food),
                new Goal("Water", gameObject.GetComponent<Zebra>().Water),
                new Goal("Space", gameObject.GetComponent<Zebra>().Space),
                new Goal("Sociality", gameObject.GetComponent<Zebra>().Sociality),
                new Goal("Energy", gameObject.GetComponent<Zebra>().Energy)
            };
        }
    }

    public struct Goal
    {
        public Goal(string name, float value)
        {
            Name = name;
            Value = value;
        }

        public string Name;
        public float Value;
    }
}