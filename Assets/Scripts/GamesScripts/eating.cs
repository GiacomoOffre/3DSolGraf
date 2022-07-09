using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using OpenCvSharp;

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
    public Image vittoria;
    public Image sconfitta;
    public TextMeshProUGUI timer;

    public GameObject schermataIniziale;
    public Button gioca;

    public TextMeshProUGUI ciambelleMangiate;

    float timeLeft = 32.0f;

    Controller cr;
    double dist06rest = Controller.dist06rest;
    double dist39rest = Controller.dist39rest;

    public double d06 = 0f;
    public double d39 = 0f;
    public double soglia = 0f;

    public List<DetectedFace> detectedFaces;

    public RawImage camTexture;
    public RawImage roiTexture;

    private bool ready = true;

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

        detectedFaces = new List<DetectedFace>();
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timer.text = string.Format("{0:00} sec", seconds);
        if (timeLeft > 0 & nCiambelle < 7)
        {
            
		//if (Input.GetKeyDown("space"))
  //                  {
  //                      donuteater.transform.position = ciambelle[nCiambelle].transform.position;
  //                      ciambelle[nCiambelle].active = false;
  //                      nCiambelle = nCiambelle + 1;
  //                  }


            foreach (DetectedFace f in detectedFaces)
            {
                //0 - Jaw; 1-2 Eyebrow; 3 nosebridge; 4 nose; 5-6 Eyes; 7-8 Lips : questi sono gli elementi che è in grado di risconoscere
                // if (f.Elements[5].Marks !=null)
                if (f.Elements[7].Marks != null & f.Elements[0].Marks != null)
                {
                    Mat img = OpenCvSharp.Unity.TextureToMat(camTexture.texture as Texture2D);
                    // OpenCvSharp.Rect r = Cv2.BoundingRect(f.Elements[5].Marks); //si crea un rettangolo minimo che includa tutti i marker
                    OpenCvSharp.Rect r = Cv2.BoundingRect(f.Elements[0].Marks); //si crea un rettangolo minimo che includa tutti i marker
                    OpenCvSharp.Rect s = Cv2.BoundingRect(f.Elements[7].Marks); //si crea un rettangolo minimo che includa tutti i marker

                    Point mark0 = f.Elements[7].Marks[0];

                    Cv2.Circle(img, mark0, 1, Scalar.FromRgb(255, 0, 0), -1);

                    Point mark3 = f.Elements[7].Marks[3];

                    Cv2.Circle(img, mark3, 1, Scalar.FromRgb(255, 0, 0), -1);

                    Point mark9 = f.Elements[7].Marks[9];

                    Cv2.Circle(img, mark9, 1, Scalar.FromRgb(255, 0, 0), -1);

                    Point mark6 = f.Elements[7].Marks[6];

                    Cv2.Circle(img, mark6, 1, Scalar.FromRgb(255, 0, 0), -1);


                    d06 = calcolaDist(mark0, mark6);
                    d39 = calcolaDist(mark3, mark9);

                    soglia = ((d39 - dist39rest) / dist39rest) * 100;

                    if (soglia > 110 && ready==true)
                    {
                        donuteater.transform.position = ciambelle[nCiambelle].transform.position;
                        ciambelle[nCiambelle].active = false;
                        nCiambelle = nCiambelle + 1;
                        ready = false;
                        StartCoroutine("waitGame");
                    }

                    img = new Mat(img, r);
                    //img = new Mat(img, s);
                    roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img);

                }



            }
        }
        else
        {
            if (nCiambelle >= 7)
            {
		timer.gameObject.SetActive(false);
                StartCoroutine("wait");
            }
            else
            {
		timer.gameObject.SetActive(false);
                schermataFinale.SetActive(true);
                sconfitta.gameObject.SetActive(true);
                ciambelleMangiate.text = nCiambelle.ToString();
                camTexture.gameObject.SetActive(false);
                roiTexture.gameObject.SetActive(false);
            }
        }

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        schermataFinale.SetActive(true);
        vittoria.gameObject.SetActive(true);
        camTexture.gameObject.SetActive(false);
        roiTexture.gameObject.SetActive(false);
    }

    IEnumerator waitGame()
    {
        yield return new WaitForSeconds(1);
        ready = true;
    }

    public void MainGame()
    {
        SceneManager.LoadScene(1);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Gioca()
    {
        schermataIniziale.SetActive(false);
        gioca.gameObject.SetActive(false);
    }

    double calcolaDist(Point p1, Point p2)
    {
        double x1 = p1.X;
        double y1 = p1.Y;
        double x2 = p2.X;
        double y2 = p2.Y;

        double dist = Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));

        return dist;
    }


}
