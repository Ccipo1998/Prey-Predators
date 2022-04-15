using System.Collections;
using UnityEngine;

namespace Assets.FiniteStateMachine
{
    public class FSM
    {
        // current state
        public FSMstate CurrentState;

        public FSM(FSMstate state)
        {
            CurrentState = state;
        }

        public void Start()
        {
            CurrentState.Enter();
        }

        public void Update()
        {
            FSMtransition nextTran = CurrentState.VerifyTransitions();

            if (nextTran == null)
            {
                // Execute actions associated to stay from the current state
                CurrentState.Stay();
            }
            else
            {
                // Execute actions associated to exit from the current state
                CurrentState.Exit();
                // Execute actions associated to the firing transition
                nextTran.Fire();
                // Get the next state and set it as the current state
                CurrentState = CurrentState.NextState(nextTran);
                // Execute actions associated to enter from the new current state
                CurrentState.Enter();
            }
        }

    }
}