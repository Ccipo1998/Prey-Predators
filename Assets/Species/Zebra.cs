using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class Zebra : Animal
    {
        public Zebra()
        {
            // animal type
            Type = AnimalType.Herbivore;

            // zebra rates
            FoodDecreaseRate = 5.0f;
            WaterDecreaseRate = 3.0f;
        }

        private void Start()
        {
            InvokeRepeating("DecreaseFood", 1, FoodDecreaseRate);
            InvokeRepeating("DecreaseWater", 5, WaterDecreaseRate);
        }

        // decrease food over time
        private void DecreaseFood()
        {
            if (Food > 0)
                Food--;
        }

        // decrease water over time
        private void DecreaseWater()
        {
            if (Water > 0)
                Water--;
        }
    }
}