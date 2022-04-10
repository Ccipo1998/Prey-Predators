using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class Animal
    {
        // Game Object associated with the Animal
        public GameObject GameObject;

        // all animals have the same needs
        public int Food;
        public int Water;
        public int Space;
        public int Sociality;
        public int Energy;
        // animals have a type
        public AnimalType Type;

        public Animal()
        {
            // starting values: 100%
            Food = 100;
            Water = 100;
            Space = 100;
            Sociality = 100;
            Energy = 100;
        }
    }

    public enum AnimalType
    {
        Carnivorous, Herbivore
    }
}