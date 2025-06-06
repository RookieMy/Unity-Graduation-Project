using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("Instances", order = 0)]
    public MazeGenerator mazeParent;
    public GameObject[] treePrefabs; 

    [Header("Settings",order = 1)]
    public int treeCount = 100;      
    public float ringOffset = 10f;

    [Header("Randomization Settings",order = 2)]
    public float scaleMin = 0.95f;
    public float scaleMax = 1.2f;
    public float rotationVariance = 30f;


    public void SpawnTreesAroundMaze()
    {
        if (mazeParent == null || treePrefabs.Length == 0) return;

        Vector3 mazeCenter = new Vector3((((mazeParent.columns-1)*8)/2)+300,0, (((mazeParent.columns - 1) * 8) / 2) + 300);
        float mazeWidth = ((mazeParent.columns - 1) * 8) / 2;
        float radius = mazeWidth / 2f + ringOffset;

        for (int i = 0; i < treeCount; i++)
        {
            float angle = i * Mathf.PI * 2f / treeCount;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 position = new Vector3(x, 0, z) + mazeCenter;

            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            GameObject tree = Instantiate(treePrefab, position, Quaternion.identity, transform);

            float randomScale = Random.Range(scaleMin, scaleMax);
            tree.transform.localScale *= randomScale;

            float randomYRot = Random.Range(-rotationVariance, rotationVariance);
            tree.transform.Rotate(0, randomYRot, 0);

        }
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            SpawnTreesAroundMaze();

        }
    }
}
