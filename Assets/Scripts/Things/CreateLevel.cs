using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;


enum TerrainType { Grass, Road };


public class CreateLevel : MonoBehaviour
{
	public static CreateLevel instance;

	public List<Vector3> Placas;
	public GameObject bloke;
	public GameObject blokeGiro;
	public GameObject moneda;
	public List<GameObject> trampas;
	float sizeBlock, trampaProb, coinProb;
	float offsetYCoin;

	void Awake()
	{
		instance = this;
	}

	public void crearNivel()
	{
		//Eliminar el nivel anterior.
		foreach (Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}
		
		//Crear nivel:
		Placas = new List<Vector3>();
		sizeBlock = 4.0f;
		trampaProb = 0.15f;  coinProb = 0.15f;
		int nPaths = 20;
		int minLen = 4, maxLen = 10;

		Vector3 start = new Vector3(0.0f, -4.0f, 0.0f);

		for (int i = 0; i < nPaths; ++i)
		{
			Vector3 dir;
			if (i % 2 == 0) dir = Vector3.forward;
			else dir = Vector3.right;

			//Se genera el camino de minimo 4 y maximo 10 bloques.
			float p = Random.value * (maxLen - minLen) + minLen;
			int pathLen = (int)p;

			start = GeneratePath(start, dir, (int)pathLen);
		}
	}	
	
	Vector3 GeneratePath(Vector3 start, Vector3 dir, int pathLength)
	{
		//Para controlar las trampas consecutivas.
		int consecTraps = 0;
		Vector3 pos = new Vector3();
		for (int i = 1; i <= pathLength; ++i)
		{
			pos = start + i * sizeBlock * dir;
			GameObject newBlock, coin;

			//Bloque de giro:
			if (i == pathLength)
			{
				newBlock = Instantiate(blokeGiro, pos, Quaternion.identity);
				Vector3 centroBloque = new Vector3(pos.x, 0.0f, pos.z - sizeBlock/2);
				Placas.Add(centroBloque);
			}
			//Bloques normales/trampas:
			else
			{
				if (consecTraps < 2 && Random.value <= trampaProb)
				{
					++consecTraps;
					//Cada trampa tiene probabilidad 1/5.
					float p = Random.value * 4;
					int trap = (int)p;
					Debug.Log(trap);
					newBlock = Instantiate(trampas[trap], pos, Quaternion.identity);
				}
				else
				{
					consecTraps = 0;
					newBlock = Instantiate(bloke, pos, Quaternion.identity);
				}
			}
			newBlock.transform.parent = transform;

			//Monedas:
			if (i != pathLength && Random.value <= coinProb)
			{
				//Calculamos offset para que la pellet aparezca en el centro del bloque.
				//Se randomiza el offset de Y
				offsetYCoin = Random.value * (7 - 4.7f) + 4.7f;
				Vector3 offset;
				if (dir == Vector3.forward) offset = new Vector3(-1.0f, 4.7f, 0.0f);
				else offset = new Vector3(0.0f, 4.7f, -2.5f);

				Vector3 posCoin = pos + offset;
				coin = Instantiate(moneda, posCoin, Quaternion.identity);
				coin.transform.parent = transform;
			}	
		}
		return pos;
	}

}
