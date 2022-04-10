using System.Collections;
using UnityEngine;
using Assets.Species;

namespace Assets.Spawner
{
    public class Spawner : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            GameObject ToSpawn = GameObject.FindGameObjectWithTag("Zebra");
            ToSpawn.AddComponent<Rigidbody>();
            ToSpawn.GetComponent<Rigidbody>().transform.position = Vector3.zero + new Vector3(0.0f, 1.0f, 0.0f);

            for (int i = 1; i < 5; i++)
            {
                Zebra zebra = new Zebra(Instantiate(ToSpawn));
                zebra.GameObject.AddComponent<Rigidbody>();
                zebra.GameObject.GetComponent<Rigidbody>().transform.position = Vector3.zero + new Vector3((float)i * 2, 1.0f, 0.0f);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}