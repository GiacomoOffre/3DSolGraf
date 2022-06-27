using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eating : MonoBehaviour
{
    public GameObject donuteater;
    private int nCiambelle = 0;

    public GameObject ciambella00;
    public GameObject ciambella01;
    public GameObject ciambella02;
    public GameObject ciambella03;
    public GameObject ciambella04;
    public GameObject ciambella05;
    public GameObject ciambella06;

    private GameObject[] ciambelle;

    public GameObject schermataFinale;

    // Start is called before the first frame update
    void Start()
    {
        ciambelle = new GameObject[7];
        ciambelle[0] = ciambella06;
        ciambelle[1] = ciambella05;
        ciambelle[2] = ciambella04;
        ciambelle[3] = ciambella03;
        ciambelle[4] = ciambella02;
        ciambelle[5] = ciambella01;
        ciambelle[6] = ciambella00;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump") && nCiambelle<7)
        {
            donuteater.transform.position = ciambelle[nCiambelle].transform.position;
            ciambelle[nCiambelle].active = false;
            nCiambelle = nCiambelle +1;            
        }

        if (nCiambelle >= 7)
        {
            schermataFinale.SetActive(true);
        }
        
    }
}
