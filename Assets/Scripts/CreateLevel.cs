using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


enum TerrainType { Grass, Road };


public class CreateLevel : MonoBehaviour
{
	public List<Vector3> Placas;
	public GameObject bloke;
	public GameObject blokeGiro;
	float sizeBlock;


	Vector3 GeneratePath(Vector3 start, Vector3 dir, int pathLength)
	{
		Vector3 pos = new Vector3();
		for (int i = 1; i < pathLength + 1; ++i)
		{
			pos = start + i * sizeBlock * dir;
			GameObject newBlock;

			//Pintamos bloque de giro:
			if (i == pathLength)
			{
				newBlock = Instantiate(blokeGiro, pos, Quaternion.identity);
				Vector3 centroBloque = new Vector3(pos.x, 0.0f, pos.z - sizeBlock/2.0f);
				Placas.Add(centroBloque);
			}
			//Bloques normales:
			else newBlock = Instantiate(bloke, pos, Quaternion.identity);
			newBlock.transform.parent = transform;
		}
		return pos;
	}

	void Start()
	{
		Placas = new List<Vector3>();
		sizeBlock = 4.0f;
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
}
