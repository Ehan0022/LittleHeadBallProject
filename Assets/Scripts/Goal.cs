using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    public event EventHandler OnGoalScored;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            if(gameManager.scoringEnabled)
            {
                if (gameObject.tag.Equals("RightGoal"))
                {
                    gameManager.IncementGoal("Right");
                    StartCoroutine(ExampleCoroutine());
                }
                else
                {
                    gameManager.IncementGoal("Left");
                    StartCoroutine(ExampleCoroutine());
                }
            }           
        }
     }

        IEnumerator ExampleCoroutine()
        {
            // �lk k�s�m
            //Debug.Log("Korutini ba�lad�: " + Time.time);

            // 2 saniye bekleyin
            yield return new WaitForSeconds(2);
            gameManager.ResetPossesion();


        // �kinci k�s�m
        //Debug.Log("2 saniye ge�ti: " + Time.time);
        }
}

