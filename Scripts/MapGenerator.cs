using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Variables")]
    public int size;
    public int groundDecorationAmount;
    public GameObject[] groundDecorations;
    public int treeAmount;
    public GameObject[] trees;

    private List<Vector2> takenPositions = new List<Vector2>();
    void Start()
    {
        takenPositions.Add(Vector2.zero);
        Generate();
    }

    public void Generate()
    {
        DestroyMap();
        for (int i = 0; i < groundDecorationAmount; i++)
        {
            Vector2 pos = GetRandomPos();
            takenPositions.Add(pos);
            Instantiate(GetRandomObject(groundDecorations),pos,Quaternion.identity);
        }

        for (int i = 0; i < treeAmount; i++)
        {
            Vector2 pos = GetRandomPos();
            takenPositions.Add(pos);
            Instantiate(GetRandomObject(trees), pos, Quaternion.identity);
        }
    }


    private Vector2 GetRandomPos()
    {
        A:
        Vector2 pos = new Vector2(Random.Range(-size / 2, size / 2), Random.Range(-size / 2, size / 2));
        if (isPositionTaken(pos)) { goto A; }
        return pos;
    }

    private GameObject GetRandomObject(GameObject[] objects)
    {
        return objects[Random.Range(0, objects.Length)];
    }

    private bool isPositionTaken(Vector2 pos)
    {
        foreach (Vector2 v in takenPositions)
        {
            if (v.Equals(pos)) { return true; }
        }
        return false;
    }

    public void DestroyMap()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
        takenPositions.Clear();
    }
}
