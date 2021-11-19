using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshDebug : MonoBehaviour
{
    private List<Mesh> meshes;
    private SkinnedMeshCalculator _calculator;
    void Start()
    {
        meshes = new List<Mesh>();
        _calculator = GetComponent<SkinnedMeshCalculator>();
        _calculator.OnCalculationComplete += DrawVertices;
    }
    void DrawVertices(Mesh mesh)
    {
        //_calculator.OnCalculationComplete -= DrawVertices;
        meshes.Add(mesh);
    }

    void OnDrawGizmos()
    {
        if (meshes != null)
        {
            foreach (var mesh in meshes)
            {
                for (int i = 0; i < mesh.vertexCount; i++)
                {
                    Vector3 pos = new Vector3(2 + 2 * meshes.IndexOf(mesh), 0, 0);
                    Vector3 position = mesh.vertices[i] + pos;
                    Vector3 normal = mesh.normals[i];

                    Color color = Color.green;
                    Gizmos.color = color;
                    Gizmos.DrawWireCube(position, new Vector3(0.01f, 0.01f, 0.01f));
                    //Debug.DrawRay(position, normal, color);
                }
            }
        }
    }
}