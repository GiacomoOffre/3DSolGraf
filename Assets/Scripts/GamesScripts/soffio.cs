using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class soffio : MonoBehaviour
{
    public GameObject barca00;
    public GameObject barca01;
    public GameObject barca02;

    public GameObject schermataFinale;
    public GameObject immagineVittoria;
    public GameObject immagineSconfitta;

    bool barcaVeloce = false;
    bool vittoria = true;

    // Start is called before the first frame update
    void Start()
    {
        int n = Random.Range((int)0,(int)9);

        if(n % 2 == 0)
        {
            barcaVeloce = true;
        } else
        {
            barcaVeloce = false;
        }
     }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            barca01.transform.position = barca01.transform.position + new Vector3(100, 0, 0);
        }

        if (barcaVeloce == true)
        {
            barca00.transform.position = barca00.transform.position + new Vector3((float)0.3, 0, 0);
            barca02.transform.position = barca02.transform.position + new Vector3((float)0.1, 0, 0);

            if (barca00.transform.position.x == 790)
            {
                vittoria = false;
            }
        }
        else
        {
            barca00.transform.position = barca00.transform.position + new Vector3((float)0.1, 0, 0);
            barca02.transform.position = barca02.transform.position + new Vector3((float)0.3, 0, 0);

            if (barca02.transform.position.x == 790)
            {
                vittoria = false;
            }
        }

        if (barca01.transform.position.x >= 1800)
        {
            schermataFinale.SetActive(true);

            if (vittoria == true)
            {
                immagineVittoria.SetActive(true);
            }
            else
            {
                immagineSconfitta.SetActive(true);
            }
        }
        
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
