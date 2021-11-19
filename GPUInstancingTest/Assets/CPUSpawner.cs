using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUSpawner : MonoBehaviour
{
    public int instances;

    public GameObject go;

    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < instances; i++)
        {
            Instantiate(go,
                new Vector3(Random.Range(-pos.x, pos.x), Random.Range(-pos.y, pos.y), Random.Range(-pos.z, pos.z)),
                Quaternion.identity);
        }   
    }
}
