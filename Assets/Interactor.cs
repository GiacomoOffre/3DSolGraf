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

    public GameObject pointer;
    public GameObject pointerRed;

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

        
    }
}
