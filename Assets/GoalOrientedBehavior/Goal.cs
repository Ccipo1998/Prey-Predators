using System.Collections;
using UnityEngine;

namespace Assets.GoalOrientedBehavior
{
    public class Goal
    {
        public GoalName Name;
        public float Value;

        public Goal(GoalName name, float value)
        {
            Name = name;
            Value = value;
        }
    }

    public enum GoalName
    {
        Food, Water, Sociality, Space, Energy
    }
}