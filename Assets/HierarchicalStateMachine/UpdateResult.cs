using System.Collections;
using UnityEngine;

namespace Assets.HierarchicalStateMachine
{
    public class UpdateResult
    {
        public UpdateResult(HSMstate nextState, HSMtransition nextTransition, int level)
        {
            NextState = nextState;
            NextTransition = nextTransition;
            Level = level;
        }

        public HSMstate NextState;
        public HSMtransition NextTransition;
        public int Level;
    }
}