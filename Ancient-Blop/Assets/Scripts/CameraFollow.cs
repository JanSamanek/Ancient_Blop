using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 CameraPos;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (!player)
        {
            return;
        }
        else
        {
            CameraPos = transform.position;
            CameraPos.x = player.position.x;
            CameraPos.y = player.position.y + 3.5f;

            transform.position = CameraPos;

            if (CameraPos.x < minX)
            {
                CameraPos.x = minX;
            }
            else if (CameraPos.x > maxX)
            {
                CameraPos.x = maxX;
            }

            transform.position = CameraPos;

        }
    }
}
