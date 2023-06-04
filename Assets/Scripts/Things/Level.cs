using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance;

    [SerializeField] private TurnBlock TurnBlockPrefab;
    [SerializeField] private GameObject BlockPrefab;
    // [SerializeField] private Trap TrapPrefab;
    [SerializeField] private List<GameObject> Traps;
    [SerializeField] private GameObject CoinPrefab;
	[SerializeField] private GameObject FogPrefab;
    [SerializeField] private GameObject BlockWinPrefab;

    float sizeBlock, trampaProb, coinProb;
	float offsetYCoin;

    void Awake()
    {
        instance = this;
    }

	
	public void changeFogColor()
	{
		GameObject[] objetosFog = GameObject.FindGameObjectsWithTag("fog");
		foreach (GameObject obj in objetosFog)
		{
			ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
			if (ps != null)
			{
				var mainModule = ps.main;
				mainModule.startColor = Color.red;
			}
		}
	}

    public void crearNivel()
	{
        //Eliminar el nivel anterior.
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        sizeBlock = 4.0f;
		trampaProb = 0.35f;  coinProb = 0.15f;
		int nPaths = 60;
		int minLen = 2, maxLen = 9;

		Vector3 start = new Vector3(0.0f, -4.0f, -2.0f);
		Vector3 dir = Vector3.forward;

        int pathLen1 = 4;
		int pathLen2 = Random.Range(minLen, maxLen);

        start = GeneratePath(start, dir, ref pathLen1, pathLen2);

        for (int i = 1; i < nPaths; ++i)
		{
			
			if (i % 2 == 0) dir = Vector3.forward;
			else dir = Vector3.right;

			//Se genera el camino de minimo 2 y maximo 8 bloques.
			pathLen2 = Random.Range(minLen, maxLen);

            start = GeneratePath(start, dir, ref pathLen1, pathLen2);
        }

        //Generate last block:
        Vector3 pos = start + sizeBlock * dir;
        GameObject lastBlock = Instantiate(BlockWinPrefab, pos, Quaternion.identity);
        lastBlock.transform.parent = transform;
    }

	Vector3 GeneratePath(Vector3 start, Vector3 dir, ref int pathLength1, int pathLength2)
	{
		//Para controlar las trampas consecutivas.
		int consecTraps = 0;
        Vector3 pos = new Vector3(), offset;
        GameObject newBlock;
        GameObject coin; 

        for (int i = 1; i <= pathLength1; ++i)
		{
			pos = start + i * sizeBlock * dir;

			//Bloque de giro:
			if (i == pathLength1)
			{
				TurnBlock newTurnBlock = Instantiate(TurnBlockPrefab, pos, Quaternion.identity);
                newTurnBlock.transform.parent = transform;

                Vector3 dir2;
				if (dir == Vector3.forward) dir2 = Vector3.right;
				else dir2 = Vector3.forward;
                newTurnBlock.setNextDestination(pos - 2 * Vector3.forward + pathLength2 * sizeBlock * dir2 + 4 * Vector3.up);
            }
			//Bloques normales/trampas:
			else
			{
				if (consecTraps < 2 && Random.value <= trampaProb && pos.x != 0.0f && i != 1)
				{
					++consecTraps;
					//Cada trampa tiene probabilidad 1/5.
					int trap = Random.Range(0, 5);
					newBlock = Instantiate(Traps[trap], pos, Quaternion.identity);
				}
				else
				{
					consecTraps = 0;
					newBlock = Instantiate(BlockPrefab, pos, Quaternion.identity);
				}
                newBlock.transform.parent = transform;

                //Monedas:
                if (i != pathLength1 && Random.value <= coinProb)
				{
					//Calculamos offset para que la pellet aparezca en el centro del bloque.
					//Se randomiza el offset de Y
					offsetYCoin = Random.value * (15.0f - 4.7f) + 4.7f;
					
					if (dir == Vector3.forward) offset = new Vector3(-1.0f, offsetYCoin, 0.0f);
					else offset = new Vector3(0.0f, offsetYCoin, -2.5f);

					coin = Instantiate(CoinPrefab, pos + offset, Quaternion.identity);
					coin.transform.parent = transform;
				}
			}
			//newBlock.transform.parent = transform;
			//Generar niebla:
			if (i % 2 == 0)
			{
				Vector3 posFog = new Vector3(pos.x, -10.0f, pos.z);
				GameObject fogObj = Instantiate(FogPrefab, posFog, Quaternion.identity);
				fogObj.transform.parent = transform;
			}
		}

        pathLength1 = pathLength2;
		return pos;
	}

	
}