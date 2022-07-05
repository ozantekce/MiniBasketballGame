using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotScrollBar : MonoBehaviour
{

    public Image red;
    public Image yellow;
    public Image green;


    public float value;

    public TMPro.TMP_Text  text;


    private float yellowWidth;
    private float greenWidth;

    private Scrollbar scrollbar;
    private void Awake()
    {
        yellowWidth = yellow.rectTransform.localScale.x;
        greenWidth = green.rectTransform.localScale.x;
        scrollbar = GetComponent<Scrollbar>();
    }


    private void OnEnable()
    {




    }


    private void OnDisable()
    {
        
        

    }

    public float mul;
    private bool dir;
    private void Update()
    {
        
        if (dir)
        {
            float c = scrollbar.value;
            c = Mathf.Lerp(c, 2, 0.4f*Time.deltaTime);
            scrollbar.value = c;
            if (c > 0.98f)
            {
                dir = false;
            }
        }
        else
        {
            float c = scrollbar.value;
            c = Mathf.Lerp(c, -1, 0.4f*Time.deltaTime);
            scrollbar.value = c;
            if (c < 0.02f)
            {
                dir = true;
            }
        }

        value = scrollbar.value;


        // 100 - 80  :  0.4-0.5  |  0.5-0.6
        // 80  - 50  :  0.3-0.4  |  0.6-0.7
        // 0   - 50  :  0.0-0.3  |  0.7-1.0

        float mul = this.mul;
        if(mul > 1)
        {
            mul = 1;
        }else if(mul < 0)
        {
            mul = 0;
        }

        
        float greenMax = 0.1f;
        float greenMin = 0.02f;
        float yellowMax = 0.1f;
        float yellowMin = 0.05f;

        float deltaGreen_ = greenMin+(greenMax - greenMin) * mul;
        float deltaYellow_ = yellowMin+(yellowMax - yellowMin) * mul;

        float val = scrollbar.value;
        float green_1_1 = 0.5f - deltaGreen_ , green_1_2 = 0.5f;
        float green_2_1 = 0.5f, green_2_2 = 0.5f+deltaGreen_;

        float yellow_1_1 = green_1_1-deltaYellow_ , yellow_1_2 = green_1_1;
        float yellow_2_1 = green_2_2, yellow_2_2 = green_2_2 + deltaYellow_;

        float red_1_1 = 0.0f, red_1_2 = yellow_1_1;
        float red_2_1 = yellow_2_2, red_2_2 = 1.0f;


        float deltaGreen = 20 / (green_1_2 - green_1_1);
        float deltaYellow = 30 / (yellow_1_2 - yellow_1_1);
        float deltaRed = 50 / (red_1_2 - red_1_1);

        green.rectTransform.localScale = new Vector3((green_1_2-green_1_1), 1, 1);
        yellow.rectTransform.localScale = new Vector3((yellow_2_2 - green_1_1), 1, 1);


        float percent = 0;

        if (val >= 0.5f)
        {
            if (val > green_2_1 && val < green_2_2)
            {
                percent = (100 - deltaGreen * (val - green_2_1));
                //Debug.Log(" " + (100-deltaGreen * (val - green_2_1)));
            }
            else if (val > yellow_2_1 && val < yellow_2_2)
            {
                percent = (80 - deltaYellow * (val - yellow_2_1));
                //Debug.Log(" " + (80-deltaYellow * (val - yellow_2_1)));
            }
            else if (val > red_2_1 && val < red_2_2)
            {
                percent = (50 - deltaRed * (val - red_2_1));
                //Debug.Log(" " + (50 - deltaRed * (val - red_2_1)));
            }

        }
        else
        {

            if (val > green_1_1 && val < green_1_2)
            {
                percent = (100 + deltaGreen * (val - green_1_2));
                //Debug.Log(" " + (100 + deltaGreen * (val - green_1_2)));
            }
            else if (val > yellow_1_1 && val < yellow_1_2)
            {
                percent = (80 + deltaYellow * (val - yellow_1_2));
                //Debug.Log(" " + (80 + deltaYellow * (val - yellow_1_2)));
            }
            else if (val > red_1_1 && val < red_1_2)
            {
                percent = (50 + deltaRed * (val - red_1_2));
                //Debug.Log(" " + (50 + deltaRed * (val - red_1_2)));
            }

        }


        if(Mathf.Abs(percent-last) > 5f)
        {
            last = ((int)percent) /5 * 5;
            text.text = "%"+last.ToString();
        }

    }

    

    private float last;


}
