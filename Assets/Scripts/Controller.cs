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
            if (f.Elements[7].Marks != null & f.Elements[8].Marks != null)
                {
                Mat img = OpenCvSharp.Unity.TextureToMat(camTexture.texture as Texture2D);
                // OpenCvSharp.Rect r = Cv2.BoundingRect(f.Elements[5].Marks); //si crea un rettangolo minimo che includa tutti i marker
                OpenCvSharp.Rect r = Cv2.BoundingRect(f.Elements[7].Marks); //si crea un rettangolo minimo che includa tutti i marker
                OpenCvSharp.Rect s = Cv2.BoundingRect(f.Elements[8].Marks); //si crea un rettangolo minimo che includa tutti i marker

                img = new Mat(img, r);
                //img2 = new Mat(img, s);
                roiTexture.texture = OpenCvSharp.Unity.MatToTexture(img); 

            }
        }
    }

}
