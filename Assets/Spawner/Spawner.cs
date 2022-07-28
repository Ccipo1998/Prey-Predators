using System.Collections;
using UnityEngine;
using Assets.Species;
using Assets.GoalOrientedBehavior;
using Assets.Behaviors;
using Assets.Actions;

public class Spawner : MonoBehaviour
{
    public int SpawnNumber = 2;
    public GameObject ZebraPrefab;

    private static GameObject ZebraPrefab_copy;

    // Use this for initialization
    void Start()
    {
        ZebraPrefab_copy = ZebraPrefab;

        for (int i = 0; i < SpawnNumber; i++)
        {
            GameObject zebraClone = Instantiate(ZebraPrefab);
            zebraClone.transform.position = new Vector3(8f, .5f, 3f) + new Vector3((float)i * 2, 1.0f, 0.0f);
            zebraClone.transform.rotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), zebraClone.transform.up);
        }
            
    }

    public static GameObject GetZebraPrefab()
    {
        return ZebraPrefab_copy;
    }

    // reset zebra parameters: both properties and behaviors/actions
    public static void ResetZebra(GameObject zebra)
    {
        zebra.GetComponent<Zebra>().ResetNeeds();
        zebra.GetComponent<ZebraBehavior>().enabled = false;
        zebra.GetComponent<ZebraBehavior>().enabled = true;
        zebra.GetComponent<ZebraSearchFood>().enabled = false;
        zebra.GetComponent<ZebraSearchWater>().enabled = false;
        zebra.GetComponent<ZebraEat>().enabled = false;
        zebra.GetComponent<ZebraDrink>().enabled = false;
        zebra.GetComponent<ZebraFlocking>().enabled = false;
        zebra.GetComponent<ZebraReproduction>().enabled = false;
        zebra.GetComponent<ZebraSociality>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}