using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gelato : MonoBehaviour
{
    public GameObject cono;
    public GameObject crema;
    public GameObject fragola;
    public GameObject cioccolato;

    public GameObject[] gelati;

    int n = 0;

    public GameObject schermataFinale;


    // Start is called before the first frame update
    void Start()
    {
        n = 0;
        gelati = new GameObject[3];
        gelati[0] = crema;
        gelati[1] = fragola;
        gelati[2] = cioccolato;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump") && n <3)
        {
            if (n == 0)
            {
                gelati[n].transform.position = cono.transform.position + new Vector3(0, 210, 0);
                n = n + 1;
            } else if (n == 1)
            {
                gelati[n].transform.position = cono.transform.position + new Vector3(0, 320, 0);
                n = n + 1;
            }else if (n == 2)
            {
                gelati[n].transform.position = cono.transform.position + new Vector3(0, 440, 0);
                n = n + 1;
            }
        }

        if (n >= 3)
        {
            schermataFinale.SetActive(true);
        }

    }
}
