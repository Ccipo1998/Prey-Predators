using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FiniteStateMachine
{
    // defer function to trigger activation condition
    // returns true when the transition can fire
    public delegate bool FSMcondition();

    // defer function to perform action
    public delegate void FSMaction();

    public class FSMtransition
    {
        // the method to evaluate if the transition is ready to fire
        public FSMcondition Condition;

        // a list of actions to perform when this transition fires
        private List<FSMaction> Actions;

        public FSMtransition(FSMcondition condition, List<FSMaction> actions = null)
        {
            Condition = condition;
            if (actions != null) Actions = actions;
        }

        // call all actions
        public void Fire()
        {
            if (Actions == null) return;

            foreach (FSMaction action in Actions)
            {
                action();
            }
        }
    }
}