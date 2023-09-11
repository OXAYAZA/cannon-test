using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator
{
    private float minSize;

    private float maxSize;

    private Mesh mesh;

    private List<Vector3> vertices;

    private List<int> triangles;

    public CubeGenerator(float minSize, float maxSize)
    {
        this.minSize = minSize;
        this.maxSize = maxSize;
    }

    public Mesh Generate()
    {
        this.mesh = new Mesh();
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();

        this.GenerateVertices();
        this.GenerateSide(0, 1, 2, 3); // top
        this.GenerateSide(4, 5, 6, 7); // bottom
        this.GenerateSide(1, 0, 5, 4); // front
        this.GenerateSide(3, 2, 7, 6); // back
        this.GenerateSide(0, 3, 6, 5); // left
        this.GenerateSide(2, 1, 4, 7); // right

        this.mesh.RecalculateNormals();
        this.mesh.RecalculateBounds();

        this.mesh.vertices = this.vertices.ToArray();
        this.mesh.triangles = this.triangles.ToArray();

        return this.mesh;
    }

    private void GenerateVertices()
    {
        var a = this.RandomValue();
        this.vertices.Add(new Vector3(a, a, a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(a, a, -a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(-a, a, -a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(-a, a, a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(a, -a, -a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(a, -a, a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(-a, -a, a));

        a = this.RandomValue();
        this.vertices.Add(new Vector3(-a, -a, -a));
    }

    private void GenerateSide(int a, int b, int c, int d)
    {
        this.triangles.Add(b);
        this.triangles.Add(c);
        this.triangles.Add(a);

        this.triangles.Add(d);
        this.triangles.Add(a);
        this.triangles.Add(c);
    }

    private float RandomValue()
    {
        return this.minSize + (this.maxSize - this.minSize) * Random.value;
    }
}
