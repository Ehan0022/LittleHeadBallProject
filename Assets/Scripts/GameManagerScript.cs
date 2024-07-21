using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject ball;
    Rigidbody2D ballRigidBody;
    Vector2 player1Pos;
    Vector2 player2Pos;
    Vector2 ballPos;
    

    public int leftGoals = 0;
    public int rightGoals = 0;
    public bool scoringEnabled = true;
    public event EventHandler OnGoalScored;

    private void Start()
    {
        scoringEnabled = true;
        ballRigidBody = ball.GetComponent<Rigidbody2D>();
        player1Pos = player1.transform.position;
        player2Pos = player2.transform.position;
        ballPos = ball.transform.position;
    }


    public void ResetPossesion()
    {
        player1.transform.position = player1Pos;
        player2.transform.position = player2Pos;
        ballRigidBody.velocity = Vector2.zero;
        ball.transform.position = ballPos;
        scoringEnabled = true;
    }

    public void IncementGoal(string s)
    {
        if(scoringEnabled)
        {
            if (s.Equals("Right"))
            {
                rightGoals++;
                OnGoalScored?.Invoke(this, EventArgs.Empty);
                scoringEnabled = false;

            }
            else if (s.Equals("Left"))
            {
                leftGoals++;
                OnGoalScored?.Invoke(this, EventArgs.Empty);
                scoringEnabled = false;
            }
        }    
    }


}
