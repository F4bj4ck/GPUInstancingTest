using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationThingy : MonoBehaviour
{
    public SkinnedMeshRenderer _renderer;

    private Mesh _mesh;

    public Animator _animator;

    public AnimationClip _animationClip;

    public string _clipName;

    public int _clipFrameLength;

    private int _vertexCount;
    private Vector3[] _vertices;
    private Vector3[] _normals;

    private List<Mesh> _completeAnimation;

    public void GenerateTexture()
    {
        _mesh = _renderer.sharedMesh;
        _vertexCount = _mesh.vertexCount;
        _vertices = new Vector3[_vertexCount];
        _normals = new Vector3[_vertexCount];
        _completeAnimation = new List<Mesh>();

        for (int i = 0; i < _clipFrameLength; i++)
        {
            if (i == 0)
            {
                _animator.Play(_clipName);
                _animator.Update(0f);
            }
            else
            {
                _animator.Update(_animationClip.length / _clipFrameLength);
            }

            Debug.Log("Calculating vertex positions for frame number: " + i);

            CalculateVertexPositions();
        }

        SaveVertexPositionsToTexture();
    }

    private void CalculateVertexPositions()
    {
        Matrix4x4[] boneMatrices = new Matrix4x4[_renderer.bones.Length];

        for (int i = 0; i < boneMatrices.Length; i++)
        {
            boneMatrices[i] = _renderer.bones[i].localToWorldMatrix * _mesh.bindposes[i];
        }

        for (int i = 0; i < _mesh.vertexCount; i++)
        {
            BoneWeight boneWeight = _mesh.boneWeights[i];

            Matrix4x4 bm0 = boneMatrices[boneWeight.boneIndex0];
            Matrix4x4 bm1 = boneMatrices[boneWeight.boneIndex1];
            Matrix4x4 bm2 = boneMatrices[boneWeight.boneIndex2];
            Matrix4x4 bm3 = boneMatrices[boneWeight.boneIndex3];

            Matrix4x4 vertexMatrix = new Matrix4x4();

            //do this 16 times since a 4x4 matrix has 16 cells
            for (int j = 0; j < 16; j++)
            {
                vertexMatrix[j] = bm0[j] * boneWeight.weight0 +
                                  bm1[j] * boneWeight.weight1 +
                                  bm2[j] * boneWeight.weight2 +
                                  bm3[j] * boneWeight.weight3;
            }

            _vertices[i] = vertexMatrix.MultiplyPoint3x4(_mesh.vertices[i]);
            _normals[i] = vertexMatrix.MultiplyVector(_mesh.normals[i]);
            //_vertices = testMesh.vertices;
            //_normals = testMesh.normals;

        }

        Mesh meshSnapShot = new Mesh();
        meshSnapShot.vertices = _vertices;
        meshSnapShot.normals = _normals;

        _completeAnimation.Add(meshSnapShot);
    }

    private void SaveVertexPositionsToTexture()
    {
        Texture2D animationTexture = new Texture2D(_clipFrameLength, _completeAnimation[0].vertices.Length, TextureFormat.RGBAFloat, false);
        Texture2D normalTexture = new Texture2D(_clipFrameLength, _completeAnimation[0].vertices.Length, TextureFormat.RGBAFloat, false);


        for (int i = 0; i < _completeAnimation.Count; i++)
        {
            for (int j = 0; j < _completeAnimation[i].vertices.Length; j++)
            {
                Vector3 rgb = new Vector3(
                    ConvertValueToRGB(_completeAnimation[i].vertices[j].x),
                    ConvertValueToRGB(_completeAnimation[i].vertices[j].y),
                    ConvertValueToRGB(_completeAnimation[i].vertices[j].z)
                );

                animationTexture.SetPixel(i, j, new Color(rgb.x, rgb.y, rgb.z));

                Vector3 normalrgb = new Vector3(
                    ConvertValueToRGB(_completeAnimation[i].normals[j].x),
                    ConvertValueToRGB(_completeAnimation[i].normals[j].y),
                    ConvertValueToRGB(_completeAnimation[i].normals[j].z)
                );

                normalTexture.SetPixel(i, j, new Color(normalrgb.x, normalrgb.y, normalrgb.z));
            }
        }

        animationTexture.Apply();

        normalTexture.Apply();

        SaveTextureToFile(animationTexture, "testAnimation");

        SaveTextureToFile(normalTexture, "testNormals");
    }

    private float ConvertValueToRGB(float input)
    {
        float result = input * 0.5f + 0.5f;

        return result;
    }

    void SaveTextureToFile(Texture2D texture, string filename)
    {
        string s = @"D:\Projekte\GPU instancing\Assets";

        s.Replace(@"\", @"/");

        System.IO.File.WriteAllBytes(s + "/" + filename + ".png", texture.EncodeToPNG());
    }
}
