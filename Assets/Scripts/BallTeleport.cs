using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTeleport : MonoBehaviour
{

    [SerializeField] GameObject ball;
    [SerializeField] Transform ballPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ball.transform.position = ballPoint.position;
    }
}
