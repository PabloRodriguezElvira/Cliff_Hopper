using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


enum TerrainType { Grass, Road };


public class CreateLevel : MonoBehaviour
{
    public Vector3[] Placas;
    public GameObject bloke;
    public GameObject placa;

    void Start()
    {
        GameObject obj;
        Placas = new Vector3[20];
        int k = 0;
        float sizeBlock = 4.0f;
        float sizePlaca = 3.0f;
        int blocks = 4;

        Vector3 position;

        //Tiles en línea recta:
        for (int i = 0; i < 4; ++i)
        {
            position = new Vector3(0.0f, -4.0f, i * sizeBlock);
            obj = (GameObject)Instantiate(bloke);
            obj.transform.Translate(position);
            obj.transform.parent = transform;
        }
        //Giradas
        for (int i = 0; i < 4; ++i)
        {
            position = new Vector3(i * sizeBlock, -4.0f, 4.0f * sizeBlock);
            obj = (GameObject)Instantiate(bloke);
            obj.transform.Translate(position);
            obj.transform.parent = transform;
            
            //Placa:
            if (i == 0)
            {
                position = new Vector3(i * sizeBlock, 0.0f, 4.0f * sizePlaca);
                Placas[k] = position; ++k;
                Debug.Log(position);

                obj = (GameObject)Instantiate(placa);
                obj.transform.Translate(position);
                obj.transform.parent = transform;
            }
        }
        //Tiles en línea recta:
        for (int i = 4; i < 8; ++i)
        {
            position = new Vector3(blocks * sizeBlock, -4.0f, i * sizeBlock);
            obj = (GameObject)Instantiate(bloke);
            obj.transform.Translate(position);
            obj.transform.parent = transform;

            //Placa
            if (i == 4)
            {
                position = new Vector3(blocks * sizeBlock, 0.0f, i * sizePlaca);
                Debug.Log(position);
                Placas[k] = position; ++k;

                obj = (GameObject)Instantiate(placa);
                obj.transform.Translate(position);
                obj.transform.parent = transform;
            }
        }
    }
}
