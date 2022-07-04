using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interactableObject : MonoBehaviour
{
    public Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        ///mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButtonDown(0))
        {
            ray = new Ray(mainCamera.ScreenToWorldPoint(Input.mousePosition), mainCamera.transform.forward);


            
                Debug.Log("Click");
        }
    }
}
