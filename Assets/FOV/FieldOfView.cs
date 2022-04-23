using Assets.Species;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FOV
{
    public class FieldOfView : MonoBehaviour
    {
        public float Radius = 5f;
        public LayerMask TargetsMask;

        public List<Transform> InFOV = new List<Transform>();

        // Use this for initialization
        void Start()
        {
            TargetsMask = LayerMask.GetMask(new string[] { "Targets" });
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            FindVisible();
            AddKnowledge();
        }

        // find visible objects in current FOV
        public void FindVisible()
        {
            // clear previous
            InFOV.Clear();

            Vector3 currentPosition = gameObject.GetComponent<Rigidbody>().position;
            Collider currentCollider = gameObject.GetComponent<Collider>();

            // find objects in current FOV
            Collider[] colliders = Physics.OverlapSphere(currentPosition, Radius, TargetsMask);

            // find visible ones
            for (int i = 0; i < colliders.Length; i++)
            {
                // track a ray from current position to the seen collider and check if there is not intersection with obstacles (~TargetsMask is the negation)
                if (!Physics.Raycast(currentPosition, colliders[i].transform.position, Vector3.Distance(currentPosition, colliders[i].transform.position), ~TargetsMask) && currentCollider != colliders[i])
                {
                    InFOV.Add(colliders[i].transform);
                    Debug.DrawLine(currentPosition, colliders[i].transform.position, Color.red);
                }
            }
        }

        // add positions to internal knowledge
        public void AddKnowledge()
        {
            Animal currentAnimal = gameObject.GetComponent<Animal>();

            for (int i = 0; i < InFOV.Count; i++)
            {

            }
        }
    }
}