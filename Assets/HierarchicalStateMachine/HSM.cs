using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HierarchicalStateMachine
{
    public class HSM : HSMstate
    {
        // standard initial state
        public HSMstate InitialState;

        // current state
        public HSMstate CurrentState;

        public HSM(Enum name, HSMstate initialState, int hierarchyLevel) : base(name, hierarchyLevel)
        {
            InitialState = initialState;
        }

        public HSM(Enum name, HSM initialState, int hierarchyLevel) : base(name, hierarchyLevel)
        {
            InitialState = initialState;
        }

        // recursively update of the machine
        public override UpdateResult Update()
        {
            // entering the state without a current state -> standard initial state
            if (CurrentState == null)
            {
                CurrentState = InitialState;

                // Execute actions associated to enter from the new current state
                CurrentState.Enter();

                // if current state is null -> it is a new entry in the current state -> init all children states
                CurrentState.Update();

                return null;
            }

            // searching a transition in current state
            HSMtransition nextTran = CurrentState.VerifyTransitions();

            UpdateResult result;

            // if transaction founded in current state -> make result
            if (nextTran != null)
            {
                HSMstate nextState = CurrentState.NextState(nextTran);
                int level = CurrentState.GetLevel(nextTran);

                result = new UpdateResult(nextState, nextTran, level);
            }
            // else search in lower levels
            else
            {
                result = CurrentState.Update();
            }

            // need to to something
            if (result != null)
            {
                if (result.Level == 0)
                {
                    // next state is in current level

                    // exit from current state
                    CurrentState.Exit();
                    // fire transition from result (it can be either from current state or from lower ones
                    result.NextTransition.Fire();
                    // set target state (which is at this level) as current because level = 0
                    CurrentState = result.NextState;
                    // enter in new state
                    CurrentState.Enter();

                    // clean result to stop the search in next step
                    result = null;
                }
                else if (result.Level > 0)
                {
                    // next state is at higher level

                    // exit from current state
                    CurrentState.Exit();

                    // the destination is higher level, so current state is lost
                    CurrentState = null;

                    // next step is one step closer to target level
                    result.Level--;
                }
                else // result.Level < 0
                {
                    // next state is at lower level

                    // not recursive step of Update()

                    // fire transition from result (it can be either from current state or from lower ones
                    result.NextTransition.Fire();

                    // get the parent state for update up to current hierarchy level
                    HSMstate parentState = result.NextState.ParentState;
                    parentState.UpdateDown(result.NextState, -result.Level);

                    // clean result to stop the search in next step
                    result = null;
                }
            }
            else
            {
                // basic current state actions
                CurrentState.Stay();
            }

            return result;
        }

        public override void UpdateDown(HSMstate nextState, int level)
        {
            // if not in beginning level -> continue recursing
            if (level > 0)
                this.ParentState.UpdateDown(this, level - 1);

            // exit from current state if exists
            if (CurrentState != null)
                CurrentState.Exit();

            // new state is the parent of next state
            CurrentState = nextState;
            // entering each parent state
            CurrentState.Enter();
        }
    }
}