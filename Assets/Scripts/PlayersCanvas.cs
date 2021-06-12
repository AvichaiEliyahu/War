using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayersCanvas : MonoBehaviour
{
    TextMeshProUGUI player1CardsText;
    TextMeshProUGUI player2CardsText;

    private void OnEnable()
    {
        MyGameManager.UpdateScore += UpdateScore;
    }

    private void OnDisable()
    {
        MyGameManager.UpdateScore -= UpdateScore;
    }

    void Start()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        player1CardsText = texts[0];
        player2CardsText = texts[1];
    }

    void UpdateScore(int p1NumOfCards, int p2NumOfCards)
    {
        player1CardsText.SetText("Player1: "+p1NumOfCards);
        player2CardsText.SetText("Player2: "+p2NumOfCards);
    }
}
