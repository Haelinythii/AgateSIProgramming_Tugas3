using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    private const float debugLineHeight = 10.0f;

    [Header("Attributes")]
    public Camera mainCamera;
    public float areaStartOffset;
    public float areaEndOffset;

    private List<GameObject> spawnedTerrain;
    private float lastGeneratedPositionX;
    private float lastRemovedPositionX;

    public List<TerrainTemplateController> earlyTerrainTemplates;

    public Dictionary<string, List<GameObject>> terrainPool;

    [Header("Template Attributes")]
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;


    private void Start()
    {
        spawnedTerrain = new List<GameObject>();
        terrainPool = new Dictionary<string, List<GameObject>>();
        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovedPositionX = GetHorizontalPositionStart() - terrainTemplateWidth;

        foreach (var terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(lastGeneratedPositionX, terrain);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while(lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
    }

    private void Update()
    {
        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while(lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
        {
            lastRemovedPositionX += terrainTemplateWidth;
            RemoveTerrain(lastRemovedPositionX);
        }
    }

    private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        if (terrainPool.ContainsKey(item.name))
        {
            if(terrainPool[item.name].Count > 0)
            {
                GameObject terrainToBeSpawned = terrainPool[item.name][0];
                terrainPool[item.name].Remove(terrainToBeSpawned);
                terrainToBeSpawned.SetActive(true);
                return terrainToBeSpawned;
            }
        }
        else
        {
            terrainPool.Add(item.name, new List<GameObject>());
        }

        GameObject newSpawnedTerrain = Instantiate(item, parent);
        newSpawnedTerrain.name = item.name;
        return newSpawnedTerrain;
    }

    private void ReturnToPool(GameObject item)
    {
        if (!terrainPool.ContainsKey(item.name))
        {
            Debug.LogError("Invalid pool item!");
        }

        terrainPool[item.name].Add(item);
        item.SetActive(false);
    }

    private void GenerateTerrain(float xPosition, TerrainTemplateController forceTerrain = null)
    {
        //GameObject newSpawnedTerrain = Instantiate(forceTerrain == null? terrainTemplates[UnityEngine.Random.Range(0, terrainTemplates.Count)].gameObject : forceTerrain.gameObject, transform);
        GameObject newSpawnedTerrain = GenerateFromPool(forceTerrain == null ? terrainTemplates[UnityEngine.Random.Range(0, terrainTemplates.Count)].gameObject : forceTerrain.gameObject, transform);
        newSpawnedTerrain.transform.position = new Vector2(xPosition, 0f);
        spawnedTerrain.Add(newSpawnedTerrain);
    }

    private void RemoveTerrain(float lastRemovedPositionX)
    {
        GameObject terrainToBeRemoved = null;
        foreach (var terrain in spawnedTerrain)
        {
            if (terrain.transform.position.x == lastRemovedPositionX)
            {
                terrainToBeRemoved = terrain;
                break;
            }
        }

        if (terrainToBeRemoved != null)
        {
            ReturnToPool(terrainToBeRemoved);
            spawnedTerrain.Remove(terrainToBeRemoved);
            //Destroy(terrainToBeRemoved);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight / 2, Color.red);
        Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight / 2, Color.red);
    }

    private float GetHorizontalPositionEnd()
    {
        return mainCamera.ViewportToWorldPoint(Vector2.right).x + areaEndOffset;
    }

    private float GetHorizontalPositionStart()
    {
        return mainCamera.ViewportToWorldPoint(Vector2.zero).x + areaStartOffset;
    }
}
