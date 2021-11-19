using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapShotSpawner : MonoBehaviour
{
    public SkinnedMeshCalculator _calculator;

    public MeshFilter _filter;

    public Texture2D texture, normalTexture;

    private int index;

    private int i;

    // Start is called before the first frame update
    void Start()
    {
        _calculator.OnCalculationComplete += UpdateMesh;
    }

    void LateUpdate()
    {
        if (texture)
        {
            //UpdateMesh(new Mesh());
        }
    }

    float ConvertValueFromRGB(float input)
    {
        float result = input * 2 - 1;

        return result;
    }

    private void UpdateMesh(Mesh meshToSpawn)
    {
        if (i % 3 == 0)
        {
            //Debug.LogError("Test");

            Mesh originalMesh = _filter.mesh;

            Mesh clonedMesh = new Mesh();

            clonedMesh.name = "clone";

            //Debug.LogError("Setting new mesh");

            index = index % 26;

            clonedMesh.vertices = originalMesh.vertices;
            clonedMesh.triangles = originalMesh.triangles;
            clonedMesh.normals = originalMesh.normals;
            clonedMesh.uv = originalMesh.uv;

            clonedMesh.vertices = meshToSpawn.vertices;

            Vector3[] array = new Vector3[clonedMesh.vertexCount];

            Vector3[] normalArray = new Vector3[clonedMesh.vertexCount];

            for (int i = 0; i < clonedMesh.vertexCount; i++)
            {
                array[i] = new Vector3(ConvertValueFromRGB(texture.GetPixel(index, i).r), ConvertValueFromRGB(texture.GetPixel(index, i).g), ConvertValueFromRGB(texture.GetPixel(index, i).b));
                normalArray[i] = new Vector3(ConvertValueFromRGB(normalTexture.GetPixel(index, i).r), ConvertValueFromRGB(normalTexture.GetPixel(index, i).g), ConvertValueFromRGB(normalTexture.GetPixel(index, i).b));
            }

            clonedMesh.vertices = array;
            clonedMesh.normals = normalArray;

            //clonedMesh.Optimize();
            //clonedMesh.RecalculateNormals();

            _filter.mesh = clonedMesh;

            index++;
        }

        i++;
    }
}
