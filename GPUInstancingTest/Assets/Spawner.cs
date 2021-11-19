using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjData
{
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }

    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
    }
}

public class Spawner : MonoBehaviour
{
    public int instances;
    public Vector3 maxPos;
    public Mesh objectMesh;
    public Material objectMaterial;

    private List<List<ObjData>> batches = new List<List<ObjData>>();

    // Start is called before the first frame update
    void Start()
    {
        int batchIndex = 0;
        List<ObjData> currBatch = new List<ObjData>();

        for (int i = 0; i < instances; i++)
        {
            AddObject(currBatch, i);
            batchIndex++;
            if (batchIndex >= 1000)
            {
                batches.Add(currBatch);
                currBatch = BuildNewBatch();
                batchIndex = 0;
            }
        }
    }

    private void AddObject(List<ObjData> currBatch, int i)
    {
        //Vector3 pos = new Vector3(0f + 0.4f * i, 0, 0f /*+ 0.4f * 100 / i*/);
        Vector3 pos = new Vector3(Random.Range(-maxPos.x, maxPos.x), Random.Range(-maxPos.y, maxPos.y), Random.Range(-maxPos.y, maxPos.y));
        currBatch.Add(new ObjData(pos, new Vector3(1,1,1), Quaternion.identity));
    }

    private List<ObjData> BuildNewBatch()
    {
        return new List<ObjData>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderBatches();
    }

    private void RenderBatches()
    {
        Debug.Log("Rendering batches: " + batches.Count);

        foreach (var batch in batches)
        {
            Debug.Log("Rendering batch with:" + batch.Count + " objects");

            Graphics.DrawMeshInstanced(objectMesh, 0, objectMaterial, batch.Select((a) => a.matrix).ToList());
        }
    }
}
