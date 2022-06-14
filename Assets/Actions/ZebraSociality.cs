using Assets.FOV;
using Assets.Species;
using System.Collections;
using UnityEngine;

namespace Assets.Actions
{
    public class ZebraSociality : MonoBehaviour
    {
        // static components
        private ZebraFOV CurrentFOV;
        private Zebra CurrentZebra;
        private Coroutine Coroutine;

        // Use this for initialization
        private void OnEnable()
        {
            CurrentFOV = gameObject.GetComponent<ZebraFOV>();
            CurrentZebra = gameObject.GetComponent<Zebra>();

            // increase or decrease sociality basing on number of near Zebras (i.e. in FOV)
            Coroutine = StartCoroutine(UpdateSociality());
        }

        private IEnumerator UpdateSociality()
        {
            while (true)
            {
                if (CurrentFOV.CurrentZebrasNumber == 0)
                {
                    CurrentZebra.Sociality--;
                    yield return new WaitForSeconds(5.0f);
                }
                else if (CurrentFOV.CurrentZebrasNumber == 1)
                {
                    yield return new WaitForSeconds(5.0f);
                }
                else if (CurrentFOV.CurrentZebrasNumber > 1 && CurrentFOV.CurrentZebrasNumber < 9)
                {
                    CurrentZebra.Sociality++;
                    yield return new WaitForSeconds(10.0f - CurrentFOV.CurrentZebrasNumber);
                }
                else
                {
                    CurrentZebra.Sociality++;
                    yield return new WaitForSeconds(1.0f);
                }
            }
        }

        private void OnDisable()
        {
            StopCoroutine(Coroutine);
        }
    }
}