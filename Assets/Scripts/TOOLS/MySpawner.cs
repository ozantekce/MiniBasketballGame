using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FOR 2D
public class MySpawner : MonoBehaviour
{

    public float delay = 1f;

    public GameObject prefab;

    public bool staticXPosition;
    public bool staticYPosition;

    public bool staticXScale;
    public bool staticYScale;



    private float positionXMax,positionXMin,positionX;
    private float positionYMax,positionMin,positionY;
    

    private Vector3 tempVectorPosition;
    private Vector3 tempVectorScale;
    [SerializeField]
    private float scaleXMax, scaleXMin, scaleX ,scaleYMax, scaleYMin, scaleY;

    private float temp;
    void Start()
    {
        //X

        positionX = transform.position.x;
        positionXMax = transform.position.x;
        positionXMin = positionXMax;

        temp = transform.lossyScale.y / 2;

        positionXMax += temp;
        positionXMin -= temp;

        //Y
        positionY = transform.position.y;
        positionYMax = transform.position.y;
        positionMin = positionYMax;

        temp = transform.lossyScale.y / 2;

        positionYMax += temp;
        positionMin -= temp;

        tempVectorPosition.z = 2;

    }


    
    private float timeSinceLastSpawn;
    private GameObject tempGO;

    void Update()
    {

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > delay)
        {
            tempGO = Instantiate(prefab);

            if (!staticXPosition)
                tempVectorPosition.x = Random.Range(positionXMin, positionXMax);
            else
                tempVectorPosition.x = positionX;

            if (!staticYPosition)
                tempVectorPosition.y = Random.Range(positionMin, positionYMax);
            else
                tempVectorPosition.y = positionY;

            
            tempGO.transform.position = tempVectorPosition;



            if (!staticXScale)
                tempVectorScale.x = Random.Range(scaleXMin, scaleXMax);
            else
                tempVectorScale.x = scaleX;

            if (!staticYScale)
                tempVectorScale.y = Random.Range(scaleYMin, scaleYMax);
            else
                tempVectorScale.y = scaleY;


            tempGO.transform.localScale = tempVectorScale;
            
            
            timeSinceLastSpawn = 0f;
        }



    }




}
