// SkinDetector for C# by Pietro Piazzolla 2020
// Adaptation from c++ code found here:
// https://bytefish.de/blog/opencv/skin_color_thresholding/


using UnityEngine;
using OpenCvSharp;

public class SkinDetector
{

    public bool R1(int R, int G, int B)
    {
        bool e1 = (R > 95) && (G > 40) && (B > 20) && ((Mathf.Max(R, Mathf.Max(G, B)) - Mathf.Min(R, Mathf.Min(G, B))) > 15) && (Mathf.Abs(R - G) > 15) && (R > G) && (R > B);
        bool e2 = (R > 220) && (G > 210) && (B > 170) && (Mathf.Abs(R - G) <= 15) && (R > B) && (G > B);
        return (e1 || e2);
    }

    public bool R2(float Y, float Cr, float Cb)
    {
        bool e3 = Cr <= 1.5862 * Cb + 20;
        bool e4 = Cr >= 0.3448 * Cb + 76.2069;
        bool e5 = Cr >= -4.5652 * Cb + 234.5652;
        bool e6 = Cr <= -1.15 * Cb + 301.75;
        bool e7 = Cr <= -2.2857 * Cb + 432.85;
        return e3 && e4 && e5 && e6 && e7;
    }

    public bool R3(float H, float S, float V)
    {
        return (H < 25) || (H > 230);
    }

    unsafe public Mat Init(Mat src) {

        Mat dst = src.Clone();
        var dstIndexer = dst.GetGenericIndexer<Vec3b>();
        var srcIndexer = src.GetGenericIndexer<Vec3b>();

        Vec3b cwhite = new Vec3b(255, 255, 255);
        Vec3b cblack = new Vec3b(0, 0, 0);

        Mat src_ycrcb = new Mat();
        Mat src_hsv = new Mat();

        // OpenCV scales the YCrCb components, so that they
        // cover the whole value range of [0,255], so there's
        // no need to scale the values:
        Cv2.CvtColor(src, src_ycrcb, ColorConversionCodes.BGR2YCrCb);
        var dstYCrCb = src_ycrcb.GetGenericIndexer<Vec3b>();

        // OpenCV scales the Hue Channel to [0,180] for
        // 8bit images, so make sure we are operating on
        // the full spectrum from [0,360] by using floating
        // point precision:
        src.ConvertTo(src_hsv, MatType.CV_32FC3);
        Cv2.CvtColor(src_hsv, src_hsv, ColorConversionCodes.BGR2HSV); //BGR2HSV
        // Now scale the values between [0,255]:
        Cv2.Normalize(src_hsv, src_hsv, 0.0, 255.0, NormTypes.MinMax, MatType.CV_32FC3); //NORM_MINMAX
        var dstHSV = src_hsv.GetGenericIndexer<Vec3f>();


        for (int i = 0; i < src.Rows; i++)
        {
            for (int j = 0; j < src.Cols; j++)
            {

                //Vec3b* pix_bgr = (Vec3b*)src.Ptr(i, j);   
                Vec3b pix_bgr = srcIndexer[i, j];
                int B = pix_bgr.Item0;
                int G = pix_bgr.Item1;
                int R = pix_bgr.Item2;
                // apply rgb rule
                bool a = R1(R, G, B);

                //Vec3b* pix_ycrcb = (Vec3b*)src_ycrcb.Ptr(i, j);
                Vec3b pix_ycrcb = dstYCrCb[i, j];
                int Y =  pix_ycrcb.Item0;
                int Cr = pix_ycrcb.Item1;
                int Cb = pix_ycrcb.Item2;
                // apply ycrcb rule
                bool b = R2(Y, Cr, Cb);

                //Vec3f* pix_hsv = (Vec3f*)src_hsv.Ptr(i, j); 
                Vec3f pix_hsv = dstHSV[i, j];
                float H = pix_hsv.Item0;
                float S = pix_hsv.Item1;
                float V = pix_hsv.Item2;
                // apply hsv rule
                bool c = R3(H, S, V);

                if (!(a && b && c)) {
                    dstIndexer[i, j] = cblack;
                }
                     
            }
        }

        return dst;
    }
}
