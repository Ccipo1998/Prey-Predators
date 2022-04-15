using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HierarchicalStateMachine
{
    // defer function to trigger activation condition
    // returns true when the transition can fire
    public delegate bool HSMcondition();

    // defer function to perform action
    public delegate void HSMaction();

    public class HSMtransition
    {
        // transition name -> for sanity
        public string Name;

        // the method to evaluate if the transition is ready to fire
        public HSMcondition Condition;

        // a list of actions to perform when this transition fires
        private List<HSMaction> Actions;

        public HSMtransition(string name, HSMcondition condition, List<HSMaction> actions = null)
        {
            Name = name;
            Condition = condition;
            if (actions != null) Actions = actions;
        }

        // call all actions
        public void Fire()
        {
            if (Actions == null) return;

            foreach (HSMaction action in Actions)
            {
                action();
            }
        }
    }
}