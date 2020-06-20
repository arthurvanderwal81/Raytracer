using System;
using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Materials;

namespace Raytracer.Objects.GLB
{
    public class GLBMesh
    {
        public string Name { get; set; }
        public List<GLBPrimitive> Primitives { get; set; }

        public GLBMesh()
        {
            Primitives = new List<GLBPrimitive>();
        }

        public List<Vertex> GetVertices(int primitiveIndex)
        {
            List<Vertex> vertices = new List<Vertex>();

            GLBPrimitive primitive = Primitives[primitiveIndex];
            GLBBufferView bufferView = primitive.Attributes.Position.BufferView;

            for (int bytePointer = 0; bytePointer < bufferView.ByteLength; bytePointer += 3 * sizeof(float))
            {
                float x = System.BitConverter.ToSingle(bufferView.Buffer, bufferView.ByteOffset + bytePointer + sizeof(float) * 0);
                float y = System.BitConverter.ToSingle(bufferView.Buffer, bufferView.ByteOffset + bytePointer + sizeof(float) * 1);
                float z = System.BitConverter.ToSingle(bufferView.Buffer, bufferView.ByteOffset + bytePointer + sizeof(float) * 2);

                Vertex vertex = new Vertex(x, y, z);
                vertices.Add(vertex);
            }

            if (vertices.Count != primitive.Attributes.Position.Count)
            {
                throw new Exception("Vertices count does not equal expected count");
            }

            return vertices;
        }

        public List<int> GetIndices(int primitiveIndex)
        {
            List<int> indices = new List<int>();

            GLBPrimitive primitive = Primitives[primitiveIndex];
            GLBBufferView bufferView = primitive.Indices.BufferView;

            for (int bytePointer = 0; bytePointer < bufferView.ByteLength; bytePointer += sizeof(short))
            {
                int index = System.BitConverter.ToInt16(bufferView.Buffer, bufferView.ByteOffset + bytePointer);

                indices.Add(index);
            }

            if (indices.Count != primitive.Indices.Count)
            {
                throw new Exception("Indices count does not equal expected count");
            }

            return indices;
        }

        public List<TriangleObject> GetTriangleObjects(int primitiveIndex)
        {
            List<TriangleObject> triangleObjects = new List<TriangleObject>();

            List<int> indices = GetIndices(primitiveIndex);
            List<Vertex> vertices = GetVertices(primitiveIndex);

            if ((indices.Count % 3) != 0)
            {
                throw new Exception("Indices count not a multiple of 3");
            }

            Material material = new Material();
            material.Color = new Color3f(0d, 0d, 1d);

            int triangleCount = indices.Count / 3;

            for (int triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
            {
                string triangleName = string.Format("{0}_PRIMITIVE_{1}_TRIANGLE_{2}", Name, primitiveIndex, triangleIndex);

                Vertex v0 = vertices[indices[triangleIndex * 3 + 0]];
                Vertex v1 = vertices[indices[triangleIndex * 3 + 1]];
                Vertex v2 = vertices[indices[triangleIndex * 3 + 2]];

                TriangleObject triangleObject = new TriangleObject(triangleName, v0, v1, v2, material);

                triangleObjects.Add(triangleObject);
            }

            return triangleObjects;
        }
    }
}