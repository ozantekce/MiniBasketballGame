using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ShowFPS : MonoBehaviour
{

    public TMP_Text fpsText;
    public TMP_Text avgFpsText;


    private float avgFPS;
    private int t;

    void Update()
    {

        float fps = (int)(1f / Time.unscaledDeltaTime);

        avgFPS = ((avgFPS * t + fps) / (t + 1));
        int temp = (int)avgFPS;

        t++;


        if (Time.frameCount % 128 == 0)
        {
            fpsText.text = fps.ToString();
            avgFpsText.text = temp.ToString();
        }



    }


}