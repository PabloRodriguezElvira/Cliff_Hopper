using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum TerrainType { Grass, Road };


public class CreateLevel : MonoBehaviour
{
    readonly float[] terrainProb = { 0.85f, 0.85f };

    public GameObject grassPrefab, roadPrefab;
    public GameObject bloke;
    public float minSpeed, maxSpeed;

    //Start is called before the first frame update
    /*void Start()
    {
        GameObject obj;
        for (uint i = 0; i < 15; i++)
        {
            obj = (GameObject)Instantiate(grassPrefab);
            obj.transform.Translate(0.0f, 0.0f, i - 10.0f);
            obj.transform.parent = transform;
        }
        TerrainType type = TerrainType.Road;
        uint size;
        float value, z = 5.0f;
        for (uint i = 5; i < 100; i++)
        {
            value = Random.value;
            if (value < terrainProb[(int)type])
                size = 1;
            else
                size = 5;
            for (uint j = 0; j < size; j++)
            {
                obj = (GameObject)Instantiate((type == TerrainType.Grass) ? grassPrefab : roadPrefab);
                obj.transform.Translate(0.0f, 0.0f, z);
                if (Random.value < 0.5f)
                {
                    obj.transform.Rotate(0.0f, 180.0f, 0.0f);
                }
                obj.transform.parent = transform;
                if (type == TerrainType.Road)
                    obj.transform.GetChild(0).GetComponent<CreateCar>().speed = minSpeed + Random.value * (maxSpeed - minSpeed);
                z += 1.0f;
            }
            type = (TerrainType)(((int)type + 1) % 2);
        }
    }*/
    void Start()
    {
        GameObject obj;
        float sizeBlock = 4.0f;

        //Tiles en línea recta:
        for (int i = 0; i < 4; ++i)
        {
            obj = (GameObject)Instantiate(bloke);
            obj.transform.Translate(0.0f, -4.0f, i * sizeBlock);
            obj.transform.parent = transform;
        }
        //Rotar 90º
        for (int i = 0; i < 4; ++i)
        {
            obj = (GameObject)Instantiate(bloke);
            obj.transform.Translate(i*sizeBlock, -4.0f, 4*sizeBlock);
            obj.transform.parent = transform;
        }
    }
}
