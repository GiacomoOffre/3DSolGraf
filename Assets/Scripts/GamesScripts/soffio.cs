using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using OpenCvSharp;

public class soffio : MonoBehaviour
{
    public GameObject barca00;
    public GameObject barca01;
    public GameObject barca02;

    public GameObject schermataFinale;
    public Image vittoria;
    public Image sconfitta;

    bool barcaVeloce = false;
    bool win = true;

    public GameObject schermataIniziale;
    public Button gioca;

    Controller cr;
    double dist06rest = Controller.dist06rest;
    double dist39rest = Controller.dist39rest;

    public double d06 = 0f;
    public double d39 = 0f;
    public double soglia = 0f;

    public List<DetectedFace> detectedFaces;

    public RawImage camTexture;
    public RawImage roiTexture;

    public TextMeshProUGUI timer;

    float timeLeft = 32.0f;

    // Start is called before the first frame update
    void Start()
    {
        int n = UnityEngine.Random.Range((int)0,(int)9);

        if(n % 2 == 0)
        {
            barcaVeloce = true;
        } else
        {
            barcaVeloce = false;
        }

        detectedFaces = new List<DetectedFace>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timer.text = string.Format("{0:00} sec", seconds);
        if (timeLeft > 0) 
        {
		
	    if (barcaVeloce == true)
                    {
                        barca00.transform.position = barca00.transform.position + new Vector3((float)0.3, 0, 0);
                        barca02.transform.position = barca02.transform.position + new Vector3((float)0.1, 0, 0);

                        if (barca00.transform.position.x == 1800)
                        {
                            win = false;
                        }
                    }
                    else
                    {
                        barca00.transform.position = barca00.transform.position + new Vector3((float)0.1, 0, 0);
                        barca02.transform.position = barca02.transform.position + new Vector3((float)0.3, 0, 0);

                        if (barca02.transform.position.x == 1800)
                        {
                            win = false;
                        }
                    }

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

                    if (soglia<-10)
                    {
                        barca01.transform.position = barca01.transform.position + new Vector3(100, 0, 0);
                    }

                    img = new Mat(img, r);
                    //img = new Mat(img, s);
                    roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img);

                }

            }
        }
        else
        {
            if (win==true)
            {
		timer.gameObject.SetActive(false);
                StartCoroutine("wait");
            }
            else
            {
		timer.gameObject.SetActive(false);
                schermataFinale.SetActive(true);
                sconfitta.gameObject.SetActive(true);                
                camTexture.gameObject.SetActive(false);
                roiTexture.gameObject.SetActive(false);
                //timer.gameObject.SetActive(false);
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
        //timer.gameObject.SetActive(false);
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

    public void QuitGame()
    {
        Debug.Log("quit");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
