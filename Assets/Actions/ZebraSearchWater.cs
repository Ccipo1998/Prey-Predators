using Assets.FOV;
using Assets.Resources;
using Assets.Species;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Actions
{
    public class ZebraSearchWater : ZebraSearch
    {
        private void Start()
        {
            InitComponents();
        }

        private void OnEnable()
        {
            InitComponents();
            ClearParameters();
        }

        void Update()
        {
            Search(SearchType.Water);
        }

        // Called at disabling
        private void OnDisable()
        {
            
        }
    }
}