using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoalsUI : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;
    [SerializeField] TextMeshProUGUI leftGoals;
    [SerializeField] TextMeshProUGUI rightGoals;

    void Start()
    {
        gameManager.OnGoalScored += GameManager_OnGoalScored;
    }

    private void GameManager_OnGoalScored(object sender, System.EventArgs e)
    {
        leftGoals.text = gameManager.leftGoals.ToString();
        rightGoals.text = gameManager.rightGoals.ToString();
    }
}

   
