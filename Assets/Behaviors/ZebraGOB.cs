using Assets.GoalOrientedBehavior;
using Assets.Species;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Behaviors
{
    public class ZebraGOB : MonoBehaviour
    {
        // simple selection
        public Goal ChooseBasicGoal()
        {
            List<Goal> goals = GetBasicGoals();

            Goal choosen = goals[0];
            for (int i = 1; i < goals.Count; i++)
            {
                if (goals[i].Value < choosen.Value)
                    choosen = goals[i];
            }

            return choosen;
        }

        // current zebra needs
        private List<Goal> GetBasicGoals()
        {
            return new List<Goal> {
                new Goal(GoalName.Food, gameObject.GetComponent<Zebra>().Food),
                new Goal(GoalName.Water, gameObject.GetComponent<Zebra>().Water),
                new Goal(GoalName.Energy, gameObject.GetComponent<Zebra>().Energy)
            };
        }
    }
}