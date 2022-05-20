﻿using System.Collections;
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

        // simple goal selection
        public Goal ChoosePressingBasicGoal()
        {
            List<Goal> goals = GetBasicGoals();

            Goal choosen = goals[0];
            for (int i = 1; i < goals.Count; i++)
            {
                if (goals[i].Value > choosen.Value)
                    choosen = goals[i];
            }

            return choosen;
        }

        // get distance from LastFoundedFood in knowledge
        public float GetDistanceFromFoodKnowledge()
        {
            // known position
            if (Knowledge.LastFoundedFood != null)
            {
                // calculate path distance
                NavMeshPath navMeshPath = new NavMeshPath();
                gameObject.GetComponent<NavMeshAgent>().CalculatePath(Knowledge.LastFoundedFood.position, navMeshPath);
                float distance = Vector3.Distance(gameObject.GetComponent<Rigidbody>().position, navMeshPath.corners[0]);
                for (int i = 0; i < navMeshPath.corners.Length - 1; ++i)
                {
                    distance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
                }

                return distance;
            }
            // unkown position
            else
            {
                // maximum distance (information unknown)
                return float.PositiveInfinity;
            }
        }

        // get distance from LastFoundedWater in knowledge
        public float GetDistanceFromWaterKnowledge()
        {
            // known position
            if (Knowledge.LastFoundedWater != null)
            {
                // calculate path distance
                NavMeshPath navMeshPath = new NavMeshPath();
                gameObject.GetComponent<NavMeshAgent>().CalculatePath(Knowledge.LastFoundedWater.position, navMeshPath);
                float distance = Vector3.Distance(gameObject.GetComponent<Rigidbody>().position, navMeshPath.corners[0]);
                for (int i = 0; i < navMeshPath.corners.Length - 1; ++i)
                {
                    distance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
                }

                return distance;
            }
            // unkown position
            else
            {
                // maximum distance (information unknown)
                return float.PositiveInfinity;
            }
        }

        // get the decrease rate of the passed goal
        public float GetGoalDecreaseRate(GoalName goalName)
        {
            float rate = 0.0f;
            switch (goalName)
            {
                case GoalName.Food:
                    rate = FoodDecreaseRate;
                    break;

                case GoalName.Water:
                    rate = WaterDecreaseRate;
                    break;
            }

            return rate;
        }

        // current distance from nearer resource, visible (in FOV) or known (knowledge)
        public float GetDistanceFromNearerFreeResource(GoalName goalName)
        {
            float distance = 0.0f;
            switch (goalName)
            {
                case GoalName.Food:
                    // check in FOV for resource with a free spot
                    GameObject nearerFreeFood = gameObject.GetComponent<ZebraFOV>().GetNearerFreeFood();
                    // if in FOV
                    if (nearerFreeFood != null)
                    {
                        // calculate path distance
                        NavMeshPath navMeshPath = new NavMeshPath();
                        gameObject.GetComponent<NavMeshAgent>().CalculatePath(nearerFreeFood.GetComponent<Rigidbody>().position, navMeshPath);
                        distance = Vector3.Distance(gameObject.GetComponent<Rigidbody>().position, navMeshPath.corners[0]);
                        for (int i = 0; i < navMeshPath.corners.Length - 1; ++i)
                        {
                            distance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
                        }
                    }
                    // check in animal knowledge
                    else
                    {
                        distance = GetDistanceFromFoodKnowledge();
                    }
                    break;

                case GoalName.Water:
                    // check in FOV for resource with a free spot
                    GameObject nearerFreeWater = gameObject.GetComponent<ZebraFOV>().GetNearerFreeWater();
                    // if in FOV
                    if (nearerFreeWater != null)
                    {
                        // calculate path distance
                        NavMeshPath navMeshPath = new NavMeshPath();
                        gameObject.GetComponent<NavMeshAgent>().CalculatePath(nearerFreeWater.GetComponent<Rigidbody>().position, navMeshPath);
                        distance = Vector3.Distance(gameObject.GetComponent<Rigidbody>().position, navMeshPath.corners[0]);
                        for (int i = 0; i < navMeshPath.corners.Length - 1; ++i)
                        {
                            distance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
                        }
                    }
                    // check in animal knowledge
                    else
                    {
                        distance = GetDistanceFromWaterKnowledge();
                    }
                    break;
            }

            return distance;
        }

        // goal selection involving timing
        public Goal ChooseConvenientBasicGoal()
        {
            List<Goal> goals = GetBasicGoals();

            // calculated values
            Goal choosen = null;
            int choosenTiming = -1;
            float currentDiscontentment = float.PositiveInfinity;

            // several timing considered: better is choose
            List<int> timings = new List<int> { 5, 10, 15, 20 };

            // loop on possible timings for actions
            foreach (int timing in timings)
            {
                // check discontentment fullfilling each goal for the selected timing
                for (int i = 0; i < goals.Count; i++)
                {
                    float discontentment = 0.0f;
                    // loop through each goal
                    for (int j = 0; j < goals.Count; j++)
                    {
                        // new value after the action
                        float newValue = goals[j].Value + GetGoalChange(goals[i].Name, goals[j].Name, timing);

                        // change due to time alone
                        newValue += timing * (1 / (goals[j].Name == GoalName.Food ? FoodDecreaseRate : WaterDecreaseRate));

                        discontentment += Discontentment(newValue);
                    }

                    if (discontentment < currentDiscontentment)
                    {
                        choosen = goals[i];
                        choosenTiming = timing;
                        currentDiscontentment = discontentment;
                    }
                }
            }

            /*
            // if the animal knows nothing -> simple selection
            if (choosen == null)
                return ChoosePressingBasicGoal();
            */

            switch (choosen.Name)
            {
                case GoalName.Food:
                    gameObject.GetComponent<ZebraEat>().SetFoodTarget((int)(choosenTiming * gameObject.GetComponent<ZebraEat>().EatVelocity + Food));
                    break;

                case GoalName.Water:
                    gameObject.GetComponent<ZebraDrink>().SetWaterTarget((int)(choosenTiming * gameObject.GetComponent<ZebraDrink>().DrinkVelocity + Water));
                    break;
            }

            return choosen;
        }

        public float Discontentment(float newValue)
        {
            return newValue * newValue;
        }

        // get the change in insistence of the goal that the timing would provide, choosing a precise goal to fullfill
        public float GetGoalChange(GoalName selectedGoalName, GoalName targetGoalName, int timing)
        {
            float change = 0.0f;
            float eatVelocity = gameObject.GetComponent<ZebraEat>().EatVelocity;
            float drinkVelocity = gameObject.GetComponent<ZebraDrink>().DrinkVelocity;

            switch (selectedGoalName)
            {
                case GoalName.Food:
                    switch (targetGoalName)
                    {
                        // food insistence decrease
                        case GoalName.Food:
                            change = -eatVelocity * timing;
                            break;

                        // water insistence increase
                        case GoalName.Water:
                            change = drinkVelocity * timing;
                            break;
                    }
                    break;

                case GoalName.Water:
                    switch (targetGoalName)
                    {
                        // food insistence increase
                        case GoalName.Food:
                            change = eatVelocity * timing;
                            break;

                        // water insistence decrease
                        case GoalName.Water:
                            change = -drinkVelocity * timing;
                            break;
                    }
                    break;
            }

            return change;
        }

        /*
        public float Discontentment(GoalName goalName, List<Goal> goals)
        {
            float discontentment = 0.0f;

            for (int i = 0; i < goals.Count; i++)
            {
                float value = 0.0f;
                if (goals[i].Name == goalName)
                    value = goals[i].Value - ((1 / GetGoalDecreaseRate(goals[i].Name)) * GetDistanceFromNearerFreeResource(goals[i].Name));
                else
                    value = goals[i].Value + ((1 / GetGoalDecreaseRate(goals[i].Name)) * GetDistanceFromNearerFreeResource(goals[i].Name));
                value *= value;
                discontentment += value;
            }

            return discontentment;
        }
        */

        // current zebra needs
        private List<Goal> GetBasicGoals()
        {
            return new List<Goal> {
                new Goal(GoalName.Food, 100 - Food),
                new Goal(GoalName.Water, 100 - Water),
                //new Goal(GoalName.Energy, Energy)
            };
        }

        // Stay() function of the Survive state
        public bool SelectNextGoal()
        {
            // compute next basic goal to survive
            Goal nextGoal = ChooseConvenientBasicGoal();

            // check if the current goal is changing and set the relative flag
            if (CurrentGoal != null && nextGoal.Name == CurrentGoal.Name)
                return false;

            // assign
            CurrentGoal = nextGoal;
            return true;
        }

        // Stay() function of the Search state
        public void Search()
        {
            // first check next goal
            bool goalChanged = SelectNextGoal();

            // check if the current goal changed
            if (goalChanged)
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

        // Enter() function of Search state
        public void SearchEnter()
        {
            // reset the current goal when entering the searching state
            CurrentGoal = null;
        }

        public void Happiness()
        {
            // find other Zebras

        }
    }
}