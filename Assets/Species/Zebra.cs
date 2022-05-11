using System.Collections;
using UnityEngine;
using Assets.Behaviors;
using Assets.GoalOrientedBehavior;
using UnityEngine.AI;
using Assets.FOV;
using Assets.Actions;
using System.Collections.Generic;

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
                new Goal(GoalName.Food, Food),
                new Goal(GoalName.Water, Water),
                new Goal(GoalName.Energy, Energy)
            };
        }

        // Stay() function of the Survive state
        public void Survive()
        {
            // compute next basic goal to survive
            Goal nextGoal = ChooseBasicGoal();

            // check if the current goal is changing and set the relative flag
            if (nextGoal.Name != CurrentGoal.Name)
                GoalChanged = true;

            // assign
            CurrentGoal = nextGoal;
        }

        // Stay() function of the Search state
        public void Search()
        {
            // check if the current goal changed
            if (GoalChanged)
            {
                // disable all the previous actions (all because there is not memory of what the Zebra was doing before)
                gameObject.GetComponent<ZebraSearchFood>().enabled = false;
                gameObject.GetComponent<ZebraSearchWater>().enabled = false;
                //gameObject.GetComponent<ZebraSearchPlaceToSleep>().enabled = false;

                // enable searching actions basing on current goal
                switch (CurrentGoal.Name)
                {
                    case GoalName.Food:
                        gameObject.GetComponent<ZebraSearchFood>().enabled = true;
                        break;

                    case GoalName.Water:
                        gameObject.GetComponent<ZebraSearchWater>().enabled = true;
                        break;

                    case GoalName.Energy:
                        //gameObject.GetComponent<ZebraSearchPlaceToSleep>().enabled = true;
                        break;
                }

                // flag resetted
                GoalChanged = false;
            }

            // else there is nothing to do
        }

        // Enter() function of SatisfyNeed state
        public void SatisfyNeedEnter()
        {
            // get current goal to satisfy
            switch (CurrentGoal.Name)
            {
                // food case
                case GoalName.Food:
                    gameObject.GetComponent<ZebraEat>().enabled = true;
                    break;

                // water case
                case GoalName.Water:
                    gameObject.GetComponent<ZebraDrink>().enabled = true;
                    break;
            }
        }

        // Exit() function of SatisfyNeed state
        public void SatisfyNeedExit()
        {
            // disable all the satisfy actions
            gameObject.GetComponent<ZebraEat>().enabled = false;
            gameObject.GetComponent<ZebraDrink>().enabled = false;
        }

        // Exit() function of Search state
        public void SearchExit()
        {
            // disabling searching behaviors
            gameObject.GetComponent<ZebraSearchFood>().enabled = false;
            gameObject.GetComponent<ZebraSearchWater>().enabled = false;
        }
    }
}