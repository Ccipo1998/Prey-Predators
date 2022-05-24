using System.Collections;
using UnityEngine;
using Assets.HierarchicalStateMachine;
using Assets.Species;
using Assets.Actions;
using Assets.GoalOrientedBehavior;

namespace Assets.Behaviors
{
    public class ZebraBehavior : MonoBehaviour
    {
        private HSM ZebraHSM;
        public float ReactionTime = 3f;

        // Use this for initialization
        void Start()
        {
            // basic states
            HSMstate RunAway = new HSMstate("RunAway", 1);
            HSMstate Death = new HSMstate("Death", 1);
            HSMstate Happiness = new HSMstate("Happiness", 3);
            HSMstate Reproduce = new HSMstate("Reproduce", 3);
            HSMstate Comfort = new HSMstate("Comfort", 3);
            HSMstate Search = new HSMstate("Search", 3);
            HSMstate SatisfyNeed = new HSMstate("Satisfy Need", 3);

            // HSM states
            HSM Survive = new HSM("Survive", Search, 2);
            HSM Live = new HSM("Live", Survive, 1);
            HSM Welfare = new HSM("Welfare", Happiness, 2);

            // state actions
            //Survive.StayActions.Add(gameObject.GetComponent<Zebra>().Survive);
            Search.StayActions.Add(gameObject.GetComponent <Zebra>().Search);
            Search.ExitActions.Add(gameObject.GetComponent<Zebra>().SearchExit);
            Search.EnterActions.Add(gameObject.GetComponent<Zebra>().SearchEnter);
            SatisfyNeed.EnterActions.Add(gameObject.GetComponent<Zebra>().SatisfyNeedEnter);
            SatisfyNeed.ExitActions.Add(gameObject.GetComponent<Zebra>().SatisfyNeedExit);
            //Happiness.StayActions.Add(gameObject.GetComponent<Zebra>().Happiness);

            // transitions
            HSMtransition CanSatisfyTran = new HSMtransition("Can satisfy", CanSatisfy);
            HSMtransition DoneTran = new HSMtransition("Done", Done);
            //HSMtransition HighPrimaryNeedsLevelsTran = new HSMtransition("High primary needs levels", HighPrimaryNeedLevels);

            // transitions links
            Search.AddTransition(CanSatisfyTran, SatisfyNeed);
            SatisfyNeed.AddTransition(DoneTran, Search);
            //Survive.AddTransition(HighPrimaryNeedsLevelsTran, Welfare);

            // parents
            Welfare.AddParent(Live);
            Survive.AddParent(Live);
            Happiness.AddParent(Welfare);
            Reproduce.AddParent(Welfare);
            Comfort.AddParent(Welfare);
            Search.AddParent(Survive);
            SatisfyNeed.AddParent(Survive);

            // create the HSM for the Zebra
            ZebraHSM = new HSM("ZebraHSM", Live, 0);

            // launch HSM
            StartCoroutine(Think());
        }

        public IEnumerator Think()
        {
            while (true)
            {
                ZebraHSM.Update();
                yield return new WaitForSeconds(ReactionTime);
            }
        }

        #region CONDITIONS

        // check if the animal can satisfy current need (if he is in the spot)
        private bool CanSatisfy()
        {
            if (gameObject.GetComponent<Zebra>().CurrentGoal == null)
                return false;

            // get current goal
            switch (gameObject.GetComponent<Zebra>().CurrentGoal.Name)
            {
                // food case
                case GoalName.Food:
                    if (gameObject.GetComponent<ZebraSearchFood>().Status == SearchingStatus.Arrived)
                        return true;
                    break;

                // water case
                case GoalName.Water:
                    if (gameObject.GetComponent<ZebraSearchWater>().Status == SearchingStatus.Arrived)
                        return true;
                    break;
            }

            return false;
        }

        // check if there is a new more urgent need to satisfy
        private bool Done()
        {
            Goal currentGoal = gameObject.GetComponent<Zebra>().CurrentGoal;

            bool isDone = false;

            switch (currentGoal.Name)
            {
                case GoalName.Food:
                    if (gameObject.GetComponent<ZebraEat>().Status == EatingStatus.Done)
                        isDone = true;
                    break;

                case GoalName.Water:
                    if (gameObject.GetComponent<ZebraDrink>().Status == DrinkingStatus.Done)
                        isDone = true;
                    break;
            }

            return isDone;

            //return gameObject.GetComponent<Zebra>().GoalChanged;
        }

        // check if current primary needs have high values
        private bool HighPrimaryNeedLevels()
        {
            // TODO: add energy
            int food = gameObject.GetComponent<Zebra>().Food;
            int water = gameObject.GetComponent<Zebra>().Water;

            if (food > 80 && water > 80)
                return true;

            return false;
        }

        #endregion CONDITIONS
    }
}