using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using OpenCvSharp;

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
    //public GameObject rawimage;
    //public GameObject roiRawImage;

    public GameObject schermataIniziale;
    public Button gioca;

    public TextMeshProUGUI nGelato;

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


    // Start is called before the first frame update
    void Start()
    {
        n = 0;
        gelati = new GameObject[3];
        gelati[0] = crema;
        gelati[1] = fragola;
        gelati[2] = cioccolato;

        detectedFaces = new List<DetectedFace>();
        
    }


    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timer.text = string.Format("{0:00} sec", seconds);
      if (timeLeft > 0 & n<3)
       {

	  //  if (Input.GetKeyDown("space")){
			//if (n == 0)
   //                     {
   //                         gelati[n].transform.position = cono.transform.position + new Vector3(0, 150, 0);
   //                         n = n + 1;
   //                     }
   //                     else if (n == 1)
   //                     {
   //                         gelati[n].transform.position = cono.transform.position + new Vector3(0, 220, 0);
   //                         n = n + 1;
   //                     }
   //                     else if (n == 2)
   //                     {
   //                         gelati[n].transform.position = cono.transform.position + new Vector3(0, 320, 0);
   //                         n = n + 1;
   //                     }
	  //   }



            foreach (DetectedFace f in detectedFaces)
            {


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

                    soglia = ((d06 - dist06rest) / dist06rest) * 100;

                    if (soglia > 22)
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

                    img = new Mat(img, r);
                    //img = new Mat(img, s);
                    roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img);

                }

            }


        }
        else
        {
            if (n >= 3)
            {
		timer.gameObject.SetActive(false);
                StartCoroutine("wait");
            }
            else
            {
		timer.gameObject.SetActive(false);
                schermataFinale.SetActive(true);
                sconfitta.gameObject.SetActive(true);
                nGelato.text = n.ToString();
                camTexture.gameObject.SetActive(false);
                roiTexture.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator  wait()
    {
        yield return new WaitForSeconds(1);
        schermataFinale.SetActive(true);
        sconfitta.gameObject.SetActive(false);
        vittoria.gameObject.SetActive(true);
        camTexture.gameObject.SetActive(false);
        roiTexture.gameObject.SetActive(false);
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
