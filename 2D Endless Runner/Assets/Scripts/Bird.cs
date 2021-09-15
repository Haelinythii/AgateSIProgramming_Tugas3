using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    float speed = 2f;
    private void Update()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BirdDestroyer")
        {
            gameObject.SetActive(false);
        }
    }
}
