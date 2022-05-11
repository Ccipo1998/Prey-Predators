using System.Collections;
using UnityEngine;
using Assets.HierarchicalStateMachine;
using Assets.Species;
using Assets.Actions;

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
            Survive.StayActions.Add(gameObject.GetComponent<Zebra>().Survive);
            Search.StayActions.Add(gameObject.GetComponent <Zebra>().Search);
            Search.ExitActions.Add(gameObject.GetComponent<Zebra>().SearchExit);
            SatisfyNeed.EnterActions.Add(gameObject.GetComponent<Zebra>().SatisfyNeedEnter);
            SatisfyNeed.ExitActions.Add(gameObject.GetComponent<Zebra>().SatisfyNeedExit);

            // transitions
            HSMtransition CanSatisfyTran = new HSMtransition("Can satisfy", CanSatisfy);
            HSMtransition NewNeedTran = new HSMtransition("New need", NewNeed);

            // transitions links
            Search.AddTransition(CanSatisfyTran, SatisfyNeed);
            SatisfyNeed.AddTransition(NewNeedTran, Search);

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
            // get current goal
            switch (gameObject.GetComponent<Zebra>().CurrentGoal.Name)
            {
                // food case
                case GoalOrientedBehavior.GoalName.Food:
                    if (gameObject.GetComponent<ZebraSearchFood>().Status == SearchingStatus.Arrived)
                        return true;
                    break;

                // water case
                case GoalOrientedBehavior.GoalName.Water:
                    if (gameObject.GetComponent<ZebraSearchWater>().Status == SearchingStatus.Arrived)
                        return true;
                    break;
            }

            return false;
        }

        // check if there is a new more urgent need to satisfy
        private bool NewNeed()
        {
            return gameObject.GetComponent<Zebra>().GoalChanged;
        }

        #endregion CONDITIONS
    }
}