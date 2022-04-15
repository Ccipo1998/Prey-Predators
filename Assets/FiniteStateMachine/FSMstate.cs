using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FiniteStateMachine
{
    public class FSMstate
    {
        // list of actions to perform based on transitions fire or not
        public List<FSMaction> EnterActions = new List<FSMaction>();
        public List<FSMaction> StayActions = new List<FSMaction>();
        public List<FSMaction> ExitActions = new List<FSMaction>();

        // links between each transition with its target state
        private Dictionary<FSMtransition, FSMstate> Links;

        public FSMstate()
        {
            Links = new Dictionary<FSMtransition, FSMstate>();
        }

        public void AddTransition(FSMtransition transition, FSMstate state)
        {
            Links[transition] = state;
        }

        public FSMtransition VerifyTransitions()
        {
            foreach (FSMtransition tran in Links.Keys)
            {
                if (tran.Condition()) return tran;
            }

            return null;
        }

        public FSMstate NextState(FSMtransition transition)
        {
            return Links[transition];
        }

        public void Enter()
        {
            if (EnterActions != null) foreach (FSMaction action in EnterActions) action();
        }

        public void Stay()
        {
            if (StayActions != null) foreach (FSMaction action in StayActions) action();
        }

        public void Exit()
        {
            if (ExitActions != null) foreach (FSMaction action in ExitActions) action();
        }

    }
}