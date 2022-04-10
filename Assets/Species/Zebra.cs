using System.Collections;
using UnityEngine;

namespace Assets.Species
{
    public class Zebra : Animal
    {
        public Zebra(GameObject gameObject)
        {
            Type = AnimalType.Herbivore;
            GameObject = gameObject;
        }
    }
}