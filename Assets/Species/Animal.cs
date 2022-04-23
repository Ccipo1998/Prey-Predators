using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class Animal : MonoBehaviour
    {
        // all animals have the same needs
        [Range(0, 100)]
        public int Food;
        [Range(0, 100)]
        public int Water;
        [Range(0, 100)]
        public int Space;
        [Range(0, 100)]
        public int Sociality;
        [Range(0, 100)]
        public int Energy;

        // animals have a type
        public AnimalType Type;

        // animals have a knowledge of the surrounding environment
        public Knowledge Knowledge;

        // resources change rates
        public float FoodDecreaseRate;
        public float WaterDecreaseRate;

        public Animal()
        {
            // starting values: 100%
            Food = 50;
            Water = 50;
            Space = 50;
            Sociality = 50;
            Energy = 100;
        }
    }

    public enum AnimalType
    {
        Carnivorous, Herbivore
    }
}