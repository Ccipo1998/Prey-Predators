using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class Knowledge
    {
        // the internal knowledge of an animal

        // herbivore last food = plant
        // carnivorous last food = prey
        public Transform LastFoundedFood;

        // position of last seen source of water
        public Transform LastFoundedWater;

        // last known position of a similar animal
        public Transform LastSeenSimilar;
    }
}