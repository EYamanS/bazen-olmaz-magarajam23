using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kari : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject missionObjectToShow;
    [SerializeField]
    GameObject[] hideObjectsOnInteract;

    Collider2D interactCollider;

    private void Awake()
    {
        interactCollider = GetComponent<Collider2D>();    
    }

    public void interact()
    {
        missionObjectToShow.gameObject.SetActive(true);
        interactCollider.enabled = false;

        for (int i = 0; i < hideObjectsOnInteract.Length; i++)
        {
            hideObjectsOnInteract[i].SetActive(false);
        }
    }

    public void OnTriggerExit()
    {
        
    }
}
