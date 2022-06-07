using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;

public class Controller : MonoBehaviour
{
    public RawImage camTexture;
    public RawImage roiTexture;

    public double dist39 = 0f;
    public double dist06 = 0f;

    public List<DetectedFace> detectedFaces; //si vuole che nello script FaceDetector (nella parte di "Inserire qui"), vada a mandare in controller tutte le info relative ai volti 
    // Start is called before the first frame update
    void Start()
    {
        detectedFaces = new List<DetectedFace>(); 
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PaintROI());
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

                InputArray array0 = new InputArray(2);

                dist06 = Cv2.Norm(array0, NormTypes.L2); 


                img = new Mat(img, r);
                //img = new Mat(img, s);
                roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img);               

            }

            
        }

    }

}
