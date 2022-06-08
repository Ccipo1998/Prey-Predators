using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class Knowledge
    {
        // the internal knowledge of an animal

        // herbivore last food = plant
        // carnivorous last food = prey
        public Vector3? LastFoundedFood;

        // position of last seen source of water
        public Vector3? LastFoundedWater;

        // last known position of a similar animal
        public Vector3? LastSeenSimilar;
    }
}