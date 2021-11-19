using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationThingy))]
public class AnimationThingyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationThingy at = (AnimationThingy) target;

        if (GUILayout.Button("Generate texture"))
        {
            at.GenerateTexture();
        }
    }
}
