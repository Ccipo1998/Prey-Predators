using System.Collections;
using UnityEngine;
using Assets.HierarchicalStateMachine;
using Assets.Species;

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

            // transitions

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
    }
}