using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCar : MonoBehaviour
{
    public GameObject carPrefab;
    public Material[] colors;
    public float minTimeBetweenCars, maxTimeBetweenCars;
    public float speed;

    float timeToNextCar;

    // Start is called before the first frame update
    void Start()
    {
        timeToNextCar = Random.value * (maxTimeBetweenCars - minTimeBetweenCars);
    }

    void Update()
    {
        timeToNextCar -= Time.deltaTime;
        if (timeToNextCar <= 0.0f)
        {
            GameObject car = (GameObject)Instantiate(carPrefab, transform.position, transform.rotation);
            car.transform.parent = transform;
            car.GetComponent<Move>().speed = new Vector3(speed, 0.0f, 0.0f);
            Material[] mats = car.GetComponent<MeshRenderer>().materials;
            mats[0] = colors[(int)(Random.value * ((float)colors.Length - 0.001f))];
            car.GetComponent<MeshRenderer>().materials = mats;
            timeToNextCar = minTimeBetweenCars + Random.value * (maxTimeBetweenCars - minTimeBetweenCars);
        }
    }
}

