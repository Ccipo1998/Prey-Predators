using System.Collections;
using UnityEngine;
using Assets.HierarchicalStateMachine;
using Assets.Species;
using Assets.Actions;
using Assets.GoalOrientedBehavior;
using Assets.FOV;

namespace Assets.Behaviors
{
    public enum StateName
    {
        Search, SatisfyNeed, Happiness, Reproduction, Comfort, RunAway, Death,
        Survive, Live, Welfare,
        ZebraHSM
    }

    public class ZebraBehavior : MonoBehaviour
    {
        private HSM ZebraHSM;
        public float ReactionTime = 1f;
        public StateName CurrentState;

        // Use this for initialization
        void Start()
        {
            // basic states
            HSMstate RunAway = new HSMstate(StateName.RunAway, 1);
            HSMstate Death = new HSMstate(StateName.Death, 1);
            HSMstate Happiness = new HSMstate(StateName.Happiness, 3);
            HSMstate Reproduction = new HSMstate(StateName.Reproduction, 3);
            //HSMstate Comfort = new HSMstate(StateName.Comfort, 3);
            HSMstate Search = new HSMstate(StateName.Search, 3);
            HSMstate SatisfyNeed = new HSMstate(StateName.SatisfyNeed, 3);

            // HSM states
            HSM Survive = new HSM(StateName.Survive, Search, 2);
            HSM Live = new HSM(StateName.Live, Survive, 1);
            HSM Welfare = new HSM(StateName.Welfare, Happiness, 2);

            // state actions -> IMPORTANT: upper-states exit functions MUST have all the sub-states exit functions
            Live.EnterActions.Add(gameObject.GetComponent<Zebra>().SocialityBehaviorEnter);
            Live.ExitActions.Add(gameObject.GetComponent<Zebra>().SocialityBehaviorExit);
            Search.StayActions.Add(gameObject.GetComponent <Zebra>().Search);
            Search.ExitActions.Add(gameObject.GetComponent<Zebra>().SearchExit);
            Search.EnterActions.Add(gameObject.GetComponent<Zebra>().SearchEnter);
            SatisfyNeed.EnterActions.Add(gameObject.GetComponent<Zebra>().SatisfyNeedEnter);
            SatisfyNeed.ExitActions.Add(gameObject.GetComponent<Zebra>().SatisfyNeedExit);
            Survive.ExitActions.Add(gameObject.GetComponent<Zebra>().SurviveExit);
            Welfare.EnterActions.Add(gameObject.GetComponent<Zebra>().WelfareEnter);
            Welfare.ExitActions.Add(gameObject.GetComponent<Zebra>().WelfareExit);
            Happiness.EnterActions.Add(gameObject.GetComponent<Zebra>().HappinessEnter);
            Happiness.ExitActions.Add(gameObject.GetComponent<Zebra>().HappinessExit);
            Reproduction.EnterActions.Add(gameObject.GetComponent<Zebra>().ReproductionEnter);
            Reproduction.ExitActions.Add(gameObject.GetComponent<Zebra>().ReproductionExit);

            // transitions
            HSMtransition CanSatisfyTran = new HSMtransition("Can satisfy", CanSatisfy);
            HSMtransition DoneTran = new HSMtransition("Done", Done);
            HSMtransition HighPrimaryNeedsLevelsTran = new HSMtransition("High primary needs levels", HighPrimaryNeedLevels);
            HSMtransition LowPrimaryNeedsLevelsTran = new HSMtransition("Low primary needs levels", LowPrimaryNeedLevels);
            HSMtransition CanReproduceTran = new HSMtransition("Can reproduce", CanReproduce);
            HSMtransition CannotReproduceTran = new HSMtransition("CannotReproduce", CannotReproduce);

            // transitions links
            Search.AddTransition(CanSatisfyTran, SatisfyNeed);
            SatisfyNeed.AddTransition(DoneTran, Search);
            Survive.AddTransition(HighPrimaryNeedsLevelsTran, Welfare);
            Welfare.AddTransition(LowPrimaryNeedsLevelsTran, Survive);
            Happiness.AddTransition(CanReproduceTran, Reproduction);
            Reproduction.AddTransition(CannotReproduceTran, Happiness);

            // parents
            Welfare.AddParent(Live);
            Survive.AddParent(Live);
            Happiness.AddParent(Welfare);
            Reproduction.AddParent(Welfare);
            //Comfort.AddParent(Welfare);
            Search.AddParent(Survive);
            SatisfyNeed.AddParent(Survive);

            // create the HSM for the Zebra
            ZebraHSM = new HSM(StateName.ZebraHSM, Live, 0);

            // launch HSM
            StartCoroutine(Think());
        }

        public IEnumerator Think()
        {
            while (true)
            {
                ZebraHSM.Update();

                // current state name
                HSMstate state = ZebraHSM.CurrentState;
                while (state is HSM)
                {
                    state = (state as HSM).CurrentState;
                }
                if (state != null)
                    CurrentState = (StateName)state.Name;

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
                    if (gameObject.GetComponent<ZebraSearchFood>().Status == SearchingResourceStatus.Arrived)
                        return true;
                    break;

                // water case
                case GoalName.Water:
                    if (gameObject.GetComponent<ZebraSearchWater>().Status == SearchingResourceStatus.Arrived)
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

            /* QUESTO NON FUNZIONA PERCHE ZebraConsume.Status QUI RIMANE Consuming, CAPIRE PERCHE...
            if (gameObject.GetComponent<ZebraConsume>().Status == ConsumingStatus.Done)
                isDone = true;
            */

            switch (currentGoal.Name)
            {
                case GoalName.Food:
                    if (gameObject.GetComponent<ZebraEat>().Status == ConsumingStatus.Done)
                        isDone = true;
                    break;

                case GoalName.Water:
                    if (gameObject.GetComponent<ZebraDrink>().Status == ConsumingStatus.Done)
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

        // check if current primary needs have low values
        private bool LowPrimaryNeedLevels()
        {
            // TODO: add energy
            int food = gameObject.GetComponent<Zebra>().Food;
            int water = gameObject.GetComponent<Zebra>().Water;

            if (food < 40 || water < 40)
                return true;

            return false;
        }

        // check if the zebra has: high sociality and high energy and other zebra for reproduction in FOV
        private bool CanReproduce()
        {
            int sociality = gameObject.GetComponent<Zebra>().Sociality;
            int energy = gameObject.GetComponent<Zebra>().Energy;
            bool zebraInFOV = gameObject.GetComponent<ZebraFOV>().ZebraForReproductionInFOV();

            if (sociality >= 80 && energy > 60 && zebraInFOV)
                return true;

            return false;
        }

        // check if the zebra has: low energy or no other zebra for reproduction in FOV
        private bool CannotReproduce()
        {
            int sociality = gameObject.GetComponent<Zebra>().Sociality;
            int energy = gameObject.GetComponent<Zebra>().Energy;
            bool zebraInFOV = gameObject.GetComponent<ZebraFOV>().ZebraForReproductionInFOV();

            if (energy <= 60 || !zebraInFOV)
                return true;

            return false;
        }

        #endregion CONDITIONS
    }
}