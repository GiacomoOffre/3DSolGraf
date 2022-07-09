using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class Interactor : MonoBehaviour
{

    public LayerMask interactableLayerMask = 8;
    UnityEvent onInteract;

    Controller controller;
    int nGioco = 0;

    public GameObject interactPointer;
    public GameObject defaultPointer;

    //public Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, interactableLayerMask))
        {
            Debug.Log(hit.collider.name);           

            if (hit.collider.GetComponent<Interactable>() != false)
            {

                //if(interactable==null || interactable.ID != hit.collider.GetComponent<Interactable>().ID)
                //{
                //    interactable = hit.collider.GetComponent<Interactable>();
                //    Debug.Log("new interactable");
                //}

                interactPointer.SetActive(true);
                defaultPointer.SetActive(false);
                
                onInteract = hit.collider.GetComponent<Interactable>().onInteract;
                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(2);
                    if (hit.collider.name == "stagno")
                    {
                        nGioco = 3;
                    } else if(hit.collider.name == "negozioCiambelle")
                    {
                        nGioco = 4;
                    }
                    else if (hit.collider.name == "carrettoGelato")
                    {
                        nGioco = 5;
                    }

                    Controller.miniGioco = nGioco;
                }
            }
            
        }
        else
        {
            interactPointer.SetActive(false);
            defaultPointer.SetActive(true);
        }


    }
}
