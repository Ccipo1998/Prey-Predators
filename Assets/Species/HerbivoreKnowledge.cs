using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class HerbivoreKnowledge : Knowledge
    {
        // herbivores have last predator position
        public Transform LastSeenPredator;
    }
}