using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    float speed = 2f;
    private void Update()
    {
        //gerak ke kiri terus menerus
        transform.position -= new Vector3(speed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //kalau collide sama bird destroyer
        if(collision.tag == "BirdDestroyer")
        {
            //matikan gameobject
            gameObject.SetActive(false);
        }
    }
}
