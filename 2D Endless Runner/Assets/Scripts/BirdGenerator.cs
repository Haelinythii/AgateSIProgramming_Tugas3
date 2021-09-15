using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGenerator : MonoBehaviour
{
    [SerializeField] private float birdSpawnCooldown;
    [Range(0, 100)] [SerializeField] private int birdSpawnChance = 10;

    public List<GameObject> birds = new List<GameObject>();
    public GameObject birdPrefab;
    [SerializeField] private Transform birdSpawnPoint;

    private float birdSpawnTimer;
    private float yMaxPosBird;

    private void Start()
    {
        birdSpawnTimer = birdSpawnCooldown;
        yMaxPosBird = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
    }

    private void Update()
    {
        birdSpawnTimer -= Time.deltaTime;
        if (birdSpawnTimer < 0)
        {
            SpawnBird();

            birdSpawnTimer = birdSpawnCooldown;
        }
    }

    //cek probabilitas dan spawn burung
    private void SpawnBird()
    {
        int randNum = Random.Range(0, 100);
        if (randNum < birdSpawnChance) //jika chance lagi kena
        {
            GameObject bird = GetOrCreateBird();
            float randomYBirdPosition = Random.Range(yMaxPosBird + 2f, birdSpawnPoint.transform.position.y); //set posisi y secara random
            bird.transform.position = new Vector3(birdSpawnPoint.transform.position.x, randomYBirdPosition); //set posisi x dan y dari spawn point

            //aktifkan burung
            bird.SetActive(true);
        }
    }

    private GameObject GetOrCreateBird()
    {
        //cari gameobject burung yang inactive
        GameObject inactiveBird = birds.Find(b => !b.activeSelf);

        //kalau belum ada yang inactive
        if(inactiveBird == null)
        {
            //buat burung baru
            inactiveBird = Instantiate(birdPrefab, transform);
            birds.Add(inactiveBird);
        }

        //kembalikan burungnya
        return inactiveBird;
    }
}
