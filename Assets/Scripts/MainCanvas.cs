using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] float timeOnScreen = 2f;
    TextMeshProUGUI mainText;
    TextMeshProUGUI subtext;
    TextMeshProUGUI warText;
    

    [SerializeField] string[] winningTexts;
    [SerializeField] string[] cheerupTexts;

    [SerializeField] string startWarText;
    [SerializeField] string startWarSubtext;
    [SerializeField] string warWinningText;

    private void OnEnable()
    {
        MyGameManager.OnChooseWinner += UpdateFlowTextCoroutine;
        MyGameManager.OnWarStart += UpdateWarTextCoroutine;
        MyGameManager.OnWarWin += UpdateWarWinnerTextCoroutine;
        MyGameManager.OnGameWinner += UpdateGameWinnerText;
    }

    private void OnDisable()
    {
        MyGameManager.OnChooseWinner -= UpdateFlowTextCoroutine;
        MyGameManager.OnWarStart -= UpdateWarTextCoroutine;
        MyGameManager.OnWarWin -= UpdateWarWinnerTextCoroutine;
        MyGameManager.OnGameWinner -= UpdateGameWinnerText;
    }

    void Start()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        mainText = texts[0];
        subtext = texts[1];
        warText = texts[2];

        mainText.SetText("Press a card pack to start");
        subtext.SetText("You know the rules, right?");
        warText.SetText("");
    }

    void UpdateWarWinnerTextCoroutine(Pack winner, int numOfCardsWon)
    {
        StartCoroutine(UpdateWarWinnerText(winner,numOfCardsWon));
    }

    void UpdateWarTextCoroutine()
    {
        StartCoroutine(UpdateWarText());
    }

    void UpdateFlowTextCoroutine(Pack winner, Pack loser)
    {
        StartCoroutine(UpdateTexts(winner,loser));
    }

    IEnumerator UpdateTexts(Pack winner, Pack loser)
    {

        int randomWinningTextIndex = Random.Range(0,winningTexts.Length);
        mainText.SetText(winner.GetPlayerName()+" " +winningTexts[randomWinningTextIndex]);

        int randomCheerupText = Random.Range(0, cheerupTexts.Length);
        subtext.SetText(loser.GetPlayerName()+" " + cheerupTexts[randomCheerupText]);

        yield return new WaitForSecondsRealtime(timeOnScreen);
        EmptyTextboxes();
    }

    IEnumerator UpdateWarText()
    {
        warText.SetText("WAR\nPRESS DISABLED");
        mainText.SetText(startWarText);
        subtext.SetText(startWarSubtext);
        yield return new WaitForSecondsRealtime(timeOnScreen*3);
        warText.SetText("");
        EmptyTextboxes();
    }

    IEnumerator UpdateWarWinnerText(Pack winner, int numOfCardsWon)
    {
        mainText.SetText(winner.GetPlayerName()+" "+warWinningText);
        subtext.SetText(numOfCardsWon + " cards won!");
        yield return new WaitForSecondsRealtime(timeOnScreen);
        EmptyTextboxes();
    }

    void UpdateGameWinnerText(Pack winner)
    {
        mainText.SetText("Congratulations! "+winner.GetPlayerName()+ " is the winner!");
        subtext.SetText("thanks for playing!");
    }

    void EmptyTextboxes()
    {
        mainText.SetText("");
        subtext.SetText("");
    }
}
