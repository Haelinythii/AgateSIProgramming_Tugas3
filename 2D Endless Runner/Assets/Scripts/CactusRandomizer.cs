using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusRandomizer : MonoBehaviour
{
    [Range(0, 100)][SerializeField] private int cactusSpawnChance = 10;
    public GameObject cactus;

    private void OnEnable()
    {
        cactus = transform.GetChild(0).gameObject;
        DetermineCactusSpawn();
    }

    private void DetermineCactusSpawn()
    {
        int randNum = Random.Range(0, 100);
        if(randNum < cactusSpawnChance)
        {
            cactus.SetActive(true);
        }
        else
        {
            cactus.SetActive(false);
        }
    }
}
