using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Attributes")]
    public Vector2 offset;

    [Header("Reference")]
    public Transform player;

    private void Update()
    {
        Vector3 camPosition = transform.position;
        camPosition.x = player.position.x + offset.x;
        transform.position = camPosition;
    }
}
