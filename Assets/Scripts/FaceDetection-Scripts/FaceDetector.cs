//Script di gestione della cinepresa per il progetto 3 di openCV


using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using OpenCvSharp;

public class FaceDetector : MyWebCamera
{
	public TextAsset faces;
	public TextAsset eyes;
	public TextAsset shapes;

    public GameObject sceneController; //è l'oggetto che ospiterà i valori degli elementi identificati

	private FaceProcessorLive<WebCamTexture> processor;
	private Controller controller; 


	/// <summary>
	/// Default initializer for MonoBehavior sub-classes
	/// </summary>
	protected override void Awake()
	{
		if (webCamTexture != null)
		{
			if (webCamTexture.isPlaying)
			{
				webCamTexture.Stop();
			}
			webCamTexture = null;
		}

		if (webCamDevice != null)
		{
			webCamDevice = null;
		}

		base.Awake();
		base.forceFrontalCamera = true; // we work with frontal cams here, let's force it for macOS s MacBook doesn't state frontal cam correctly

		byte[] shapeDat = shapes.bytes;
		if (shapeDat.Length == 0)
		{
			string errorMessage =
				"In order to have Face Landmarks working you must download special pre-trained shape predictor " +
				"available for free via DLib library website and replace a placeholder file located at " +
				"\"OpenCV+Unity/Assets/Resources/shape_predictor_68_face_landmarks.bytes\"\n\n" +
				"Without shape predictor demo will only detect face rects.";

#if UNITY_EDITOR
			// query user to download the proper shape predictor
			if (UnityEditor.EditorUtility.DisplayDialog("Shape predictor data missing", errorMessage, "Download", "OK, process with face rects only"))
				Application.OpenURL("http://dlib.net/files/shape_predictor_68_face_landmarks.dat.bz2");
#else
            UnityEngine.Debug.Log(errorMessage);
#endif
 


        }

		processor = new FaceProcessorLive<WebCamTexture>();
		processor.Initialize(faces.text, eyes.text, shapes.bytes);

		// data stabilizer - affects face rects, face landmarks etc.
		processor.DataStabilizer.Enabled = true;        // enable stabilizer
		processor.DataStabilizer.Threshold = 2.0;       // threshold value in pixels
		processor.DataStabilizer.SamplesCount = 2;      // how many samples do we need to compute stable data

		// performance data - some tricks to make it work faster
		processor.Performance.Downscale = 256;          // processed image is pre-scaled down to N px by long side
		processor.Performance.SkipRate = 0;             // we actually process only each Nth frame (and every frame for skipRate = 0)
	}

	/// <summary>
	/// Per-frame video capture processor
	/// </summary>
	protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
	{
		// detect everything we're interested in
		processor.ProcessTexture(input, TextureParameters);

		// mark detected objects
		processor.MarkDetected();

        if(processor.Faces.Count > 0)
        {

			/*
			Inserire qui il codice che gestisce cosa fare delle informazioni circa i volti
			identificati..
			*/
			controller = sceneController.GetComponent<Controller>();	//ogni volta che trova delle facce, detected face avrà le info relative a quei volti. Detected face viene impiegato nello scipt Controller
			controller.detectedFaces = processor.Faces; 
        }


		// processor.Image now holds data we'd like to visualize
		output = OpenCvSharp.Unity.MatToTexture(processor.Image, output);   // if output is valid texture it's buffer will be re-used, otherwise it will be re-created

		return true;
	}
}
