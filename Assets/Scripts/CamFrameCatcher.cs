using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp; 


public class CamFrameCatcher : MyWebCamera   //Non si estendono monobehavior ma 
{
    protected override void Awake()
    {
        base.Awake(); //invece a riferirsi all'oggetto, si riferisce a MyWebCamera. Deve quindi eseguire l'awake. 
        base.forceFrontalCamera = true;     
    }
    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)  
    {
        Mat img = OpenCvSharp.Unity.TextureToMat(input, TextureParameters); //prende l'input che arriva dalla webcam, utilizza dei parametri di conversione tra 
        //il fotogramma della texture e un'immagine, e caricali dentro img. 
        output = OpenCvSharp.Unity.MatToTexture(img, output);
        return true; 
    }
    
}
