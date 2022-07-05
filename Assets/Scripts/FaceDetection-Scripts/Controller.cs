using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using TMPro;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public RawImage camTexture;
    public RawImage roiTexture;
    public GameObject ok;

    private static double dist39rest = 0f;
    private static double dist06rest = 0f;
    public double d06 = 0f;
    public double d39 = 0f;
    public double distRelrest = 0f;

    public string movimento = "nulla";

    float timeLeft = 10.0f;

    public TextMeshProUGUI timer;
    public TextMeshProUGUI textSopra;
    public TextMeshProUGUI textSotto;

    public int miniGioco = 1; //può essere 1 2 o 3

    public List<DetectedFace> detectedFaces; //si vuole che nello script FaceDetector (nella parte di "Inserire qui"), vada a mandare in controller tutte le info relative ai volti 
    // Start is called before the first frame update
    void Start()
    {
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


                    dist06rest = calcolaDist(mark0, mark6);
                    dist39rest = calcolaDist(mark3, mark9);

                    d06 = dist06rest;
                    d39 = dist39rest;

                    distRelrest = ((dist39rest) / dist06rest) * 100;

                    if (distRelrest <= 55 && distRelrest >= 45)
                    {
                        movimento = "soffio";
                    }
                    else if (distRelrest <= 27 && distRelrest >= 23)
                    {
                        movimento = "sorriso";
                    }
                    else if (distRelrest <= 80 && distRelrest >= 70)
                    {
                        movimento = "boccaAperta";
                    }
                    else
                    {
                        movimento = "nulla";
                    }

                    img = new Mat(img, r);
                    //img = new Mat(img, s);
                    roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img);

                }


            }
        }
        else
        {
            camTexture.gameObject.SetActive(false);
            roiTexture.gameObject.SetActive(false);
            timer.gameObject.SetActive(false);
            textSopra.gameObject.SetActive(false);
            textSotto.gameObject.SetActive(false);
            ok.SetActive(true);
        }
    }

    /*Durante l'update, si ha bisogno di recuperare il contenuto della camtexture, ritagliarne un pezzo, e disegnarlo su un'altra texture. Si ha però un problema di sincronizzazione. 
     * Si ha bisogno di coroutine (metodo che può essere eseguito in modo asincrono. */ 
    IEnumerator PaintROI()
    {
        yield return new WaitForEndOfFrame();
        //roiTexture.texture = camTexture.texture; 

        foreach(DetectedFace f in detectedFaces)
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

               
                    dist06rest = calcolaDist(mark0, mark6);
                    dist39rest = calcolaDist(mark3, mark9);

                    distRelrest = ((dist39rest) / dist06rest) * 100;

                    if (distRelrest <= 55 && distRelrest >= 45)
                    {
                        movimento = "soffio";
                    }
                    else if (distRelrest <= 27 && distRelrest >= 23)
                    {
                        movimento = "sorriso";
                    }
                    else if (distRelrest <= 80 && distRelrest >= 70)
                    {
                        movimento = "boccaAperta";
                    }
                    else
                    {
                        movimento = "nulla";
                    }

                    img = new Mat(img, r);
                    //img = new Mat(img, s);
                    roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img);               

            }

            
        }

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

    public void next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + miniGioco);
    }

}
