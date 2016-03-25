using UnityEngine;
using System.Collections;

public class ChangeVertices : MonoBehaviour
{

    public MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] oriVertices;
 
    void Start()
    {
        mesh = meshFilter.mesh;
        SaveOriVertices();
    }

    void SaveOriVertices()
    {
        Vector3[] vertices = mesh.vertices;
        oriVertices = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            oriVertices[i] = vertices[i];
        }
    }

    Vector3 GetMinPoint()
    {
        Vector3[] vertices = oriVertices;
        Vector3 min = Vector3.zero;
        min.y = float.MaxValue;
        Vector3 minRelust = vertices[20];
        for (int i = 20; i <= 39; i++)
        {
            Vector3 worldPosition =  meshFilter.gameObject.transform.TransformPoint(vertices[i]);
            if (worldPosition.y < min.y)
            {
                min = worldPosition;
                minRelust = vertices[i];
            }

        }
        return minRelust;
    }
    void Change(float tan, Vector3 min)
    {
        Vector3[] vertices = mesh.vertices;

        vertices[41].y = CalcY(41, tan ,min);
        vertices[43].y = CalcY(43, tan, min);
        vertices[45].y = CalcY(45, tan, min);
        vertices[46].y = CalcY(46, tan, min);

        for (int i = 20; i <= 39; i++)
        {
            vertices[i].y = CalcY(i, tan, min);
        }

        for (int i = 68; i < vertices.Length; i++)
        {
            vertices[i].y = CalcY(i, tan, min);
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    float CalcY(int index ,float tan, Vector3 min)
    {
        float deltaY = 1 - Distance(oriVertices[index], min) * tan;
        return deltaY < -1 ? -1 : deltaY;
    }
    float Distance( Vector3 point2, Vector3 min)
    {
        //到切线的距离
        return Mathf.Abs((min.x * point2.x + min.z * point2.z)*2 - 0.5f);
    }

    void FixedUpdate()
    {
        Vector3 min = GetMinPoint();
        float tan = CalcAngle();
        Change(tan, min);
    }
    float CalcAngle()
    {
        Transform goTf = meshFilter.gameObject.transform;
        float angle = 0f;
        Vector2 distenceV2 = new Vector2(goTf.up.x, goTf.up.z);
        angle =   distenceV2.magnitude / goTf.up.y;
        return angle;
    }
}
