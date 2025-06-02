using System.Collections;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    private bool alreadyActived = false;
    
    [SerializeField] private CinematicController controller;
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (alreadyActived) yield break; 
        if (other.tag == "Player")
        {
            alreadyActived = true;
            other.gameObject.SetActive(false);
            yield return DoCinematic();
            other.gameObject.SetActive(true);
        }
    }

    private IEnumerator DoCinematic()
    {
        controller.gameObject.SetActive(true);
        yield return controller.PlayCinematic();
        controller.gameObject.SetActive(false);
    }
}
