using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class gelato : MonoBehaviour
{
    public GameObject cono;
    public GameObject crema;
    public GameObject fragola;
    public GameObject cioccolato;

    public GameObject[] gelati;

    int n = 0;

    public GameObject schermataFinale;
    public Image vittoria;
    public Image sconfitta;
    public TextMeshProUGUI timer;

    public GameObject schermataIniziale;
    public Button gioca;

    public TextMeshProUGUI nGelato;

    float timeLeft = 35.0f;

    Controller c;

   

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
            timeLeft -= Time.deltaTime;
            float seconds = Mathf.FloorToInt(timeLeft % 60);
            timer.text = string.Format("{0:00} sec", seconds);

            if (Input.GetButtonDown("Jump") && n < 3)
            {
                if (n == 0)
                {
                    gelati[n].transform.position = cono.transform.position + new Vector3(0, 210, 0);
                    n = n + 1;
                }
                else if (n == 1)
                {
                    gelati[n].transform.position = cono.transform.position + new Vector3(0, 320, 0);
                    n = n + 1;
                }
                else if (n == 2)
                {
                    gelati[n].transform.position = cono.transform.position + new Vector3(0, 440, 0);
                    n = n + 1;
                }
            }

            if (timeLeft < 0 || n >= 3)
            {
                if (n >= 3)
                {
                    StartCoroutine("wait");
                }
                else
                {
                    schermataFinale.SetActive(true);
                    sconfitta.gameObject.SetActive(true);
                    nGelato.text = n.ToString();
                    timer.gameObject.SetActive(false);
                }

            }
        

    }

    IEnumerator  wait()
    {
        yield return new WaitForSeconds(1);
        schermataFinale.SetActive(true);
        vittoria.gameObject.SetActive(true);
        timer.gameObject.SetActive(false);
    }

    public void MainGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void playAgain()
    {
        SceneManager.LoadScene("GiocoSorriso");
    }

    public void Gioca()
    {
        schermataIniziale.SetActive(false);
        gioca.gameObject.SetActive(false);
    }

}
