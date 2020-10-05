﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPolygonTriangulationExample : MonoBehaviour
{
    public Material material;
    public Transform[] outlinePointTransforms = null;
    Mesh _mesh;
    PolygonExample _polygon;
  


    void Awake()
    {
        _mesh = new Mesh();
        _polygon = new PolygonExample();

    }
    void Update()
    {
        //Add outline to the polygon object
        _polygon.SetPointCount(outlinePointTransforms.Length);
        for (int i = 0; i < outlinePointTransforms.Length; i++)
        {
            Vector3 position = outlinePointTransforms[i].position;
            _polygon.SetPoint(i, position);
        }

        //Get mesh data from polygon and forward it to the mesh. 
        _mesh.SetVertices(_polygon.GetVertices());
        _mesh.SetIndices(_polygon.GetTriangleIndices(), MeshTopology.Triangles, 0);
        _mesh.SetNormals(_polygon.GetNormals());

        //Draw triangulated polygon mesh

        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }

    void OnDrawGizmos()
    {
        if (outlinePointTransforms == null) return;
        for (int i = 0; i < outlinePointTransforms.Length; i++)
        {
            Vector3 position = outlinePointTransforms[i].position;
            Gizmos.DrawSphere(position, 0.03f);

            #if UNITY_EDITOR //only include this code in the editor. UnityEditor class does not exist in builds
            UnityEditor.Handles.Label(position, i.ToString());
            #endif
        }
    }
}
