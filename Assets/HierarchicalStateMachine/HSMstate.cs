using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HierarchicalStateMachine
{
    public class HSMstate
    {
        // state name -> for sanity
        public string Name;

        // list of actions to perform based on transitions fire or not
        public List<HSMaction> EnterActions = new List<HSMaction>();
        public List<HSMaction> StayActions = new List<HSMaction>();
        public List<HSMaction> ExitActions = new List<HSMaction>();

        // links between each transition with its target state
        private Dictionary<HSMtransition, HSMstate> Links;

        // current level in the hierarchy
        // higher hierarchy -> lower level
        public int Level;

        // parent state of the current one
        public HSMstate ParentState;

        public HSMstate(string name, int hierarchyLevel)
        {
            Name = name;
            Links = new Dictionary<HSMtransition, HSMstate>();
            Level = hierarchyLevel;
        }

        public void AddParent(HSMstate parentState)
        {
            ParentState = parentState;
        }

        // the update for a standard state simply do nothing
        public virtual UpdateResult Update() { return null; }

        // the update down for a standard state
        public virtual void UpdateDown(HSMstate nextState, int level) { }

        public void AddTransition(HSMtransition transition, HSMstate state)
        {
            Links[transition] = state;
        }

        // return the difference in levels of the hierarchy between source and target states of a given transition
        // if 0 -> target state at same level than source state
        // if > 0 -> target state at higher level than source state
        // if < 0 -> target state at lower level than source state
        public int GetLevel(HSMtransition transition)
        {
            return Level - Links[transition].Level;
        }

        public HSMtransition VerifyTransitions()
        {
            foreach (HSMtransition tran in Links.Keys)
            {
                if (tran.Condition()) return tran;
            }

            return null;
        }

        public HSMstate NextState(HSMtransition transition)
        {
            return Links[transition];
        }

        public void Enter()
        {
            if (EnterActions != null) foreach (HSMaction action in EnterActions) action();
        }

        public void Stay()
        {
            if (StayActions != null) foreach (HSMaction action in StayActions) action();
        }

        public void Exit()
        {
            if (ExitActions != null) foreach (HSMaction action in ExitActions) action();
        }
    }
}