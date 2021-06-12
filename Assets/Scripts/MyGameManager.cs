using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    int minNumOfCards = 4;

    [SerializeField] Pack p1;
    [SerializeField] Pack p2;

    [SerializeField] float pressCooldown=1.5f;
    bool isPressable = true; // a few seconds of cooldown required before next press
    bool atWar = false;

    public delegate void UpdateScoreAction(int p1NumOfCards, int p2NumOfCards);
    public static event UpdateScoreAction UpdateScore;

    public delegate void UpdateTextAction(Pack winner, Pack loser);
    public static event UpdateTextAction UpdateText;

    public delegate void UpdateWarTextAction();
    public static event UpdateWarTextAction UpdateWarText;

    public delegate void UpdateWarWinnerTextAction(Pack winner, int numOfCards);
    public static event UpdateWarWinnerTextAction UpdateWarWinnerText;

    public delegate void UpdateGameWinnerTextAction(Pack winner);
    public static event UpdateGameWinnerTextAction UpdateGameWinnerText;

    private void OnEnable()
    {
        Pack.OnClicked += PlayRound;
    }

    private void OnDisable()
    {
        Pack.OnClicked -= PlayRound;
    }

    void PlayRound()
    {
        if(isPressable)
            StartCoroutine(PlayOneRound());
    }

    IEnumerator PlayOneRound()
    {
        isPressable = false; // disable pressing

        if (UpdateScore != null)
            UpdateScore(p1.GetNumOfCards(), p2.GetNumOfCards()); // update the score GUI using an event

        Card[] currentCards = InstantiateAndReturnTwoCards(); // get two cards from top of each pack

        int result = CompareCards(currentCards[0],currentCards[1]); // compare cards
        RoundWinner(result, currentCards); // act according to the results
        
        yield return new WaitForSecondsRealtime(pressCooldown); // cooldown before next routine
        if(!atWar)
            isPressable = true;
        
        CheckWinner(); // check if a player has won.
        
    }

    int CompareCards(Card c1, Card c2)
    {
        return c1.CompareTo(c2);
    }

    void RoundWinner(int result,Card[] cards) {
        List<Card> cardsForWinner = new List<Card>(); // a list of cards that will be given to the winner. can include more than two cards if there is a war
        AddTwoCardsToList(cardsForWinner, cards);
        switch (result)
        {
            case 1:
                AddCardsToWinner(p1, cardsForWinner);
                UpdateText(p1, p2);
                break;
            case -1:
                AddCardsToWinner(p2, cardsForWinner);
                UpdateText(p2,p1);
                break;
            case 0:
                StartCoroutine(War(cardsForWinner)); // go to war with the current two cards. more cards to be added during the war
                break;
            default:
                break;
        }
    }

    IEnumerator War(List<Card> warCardsList)
    {
        // in this function we keep a list of cards that will eventually be given to the
        // winner of the war. if the war won't end in one take, the same list will be passed
        // on to the next iteration of war, so that the cards in it will be passed to the
        // winner when the war ends.
        
        if (UpdateWarText != null)
            UpdateWarText();

        atWar = true;
        isPressable = false;
        yield return new WaitForSecondsRealtime(pressCooldown);
        Card[] bonusCards = InstantiateAndReturnTwoCards(); // pull two cards that will be the bonus cards
        AddTwoCardsToList(warCardsList, bonusCards);

        yield return new WaitForSecondsRealtime(pressCooldown);
        Card[] warCards = InstantiateAndReturnTwoCards(); // pull two cards that will be compared, to declare the war winner
        AddTwoCardsToList(warCardsList, warCards);

        int result = CompareCards(warCards[0], warCards[1]);
        if (result == 0) // equal cards
            StartCoroutine(War(warCardsList)); // go to war again, with the same list of cards. more will be added in the next war.
        else if (result == 1)
        {
            AddCardsToWinner(p1, warCardsList);
            UpdateWarWinnerText(p1, warCardsList.Count);
        }
        else
        {
            AddCardsToWinner(p2, warCardsList);
            UpdateWarWinnerText(p2, warCardsList.Count);
        }
        if (UpdateScore != null)
            UpdateScore(p1.GetNumOfCards(), p2.GetNumOfCards()); // update the score GUI using an event

        atWar = false;
        isPressable = true;
    }

    Card[] InstantiateAndReturnTwoCards()
    {
        //getting two first cards
        Card c1 = p1.RemoveFromTop();
        Card c2 = p2.RemoveFromTop();
        
        //instantiating them for animation
        GameObject g1= Instantiate(c1, p1.transform.position, p1.transform.rotation).gameObject;
        GameObject g2= Instantiate(c2, p2.transform.position, p2.transform.rotation).gameObject;
       
        Card[] cards = new Card[2]; // taking the two cards

        cards[0] = c1;
        cards[1] = c2;

        //destoy game object but keep the cards
        Destroy(g1,pressCooldown);
        Destroy(g2,pressCooldown);
        return cards; // returning the first two cards that were pulled from each pack
    }

    void AddCardsToWinner(Pack winner, List<Card> cards) // adds the cards to the winner's pack. used in war.
    {
        foreach (Card c in cards)
            winner.AddToBack(c);
    }

    void AddTwoCardsToList(List<Card> list, Card[] cards) // adds two cards to list.
    {
        list.Add(cards[0]);
        list.Add(cards[1]);
    }

    void CheckWinner()
    {
        int p1NumOfCards = p1.GetNumOfCards();
        int p2NumOfCards = p2.GetNumOfCards();
        int winner;

        if (p1NumOfCards > minNumOfCards && p2NumOfCards > minNumOfCards) // no one wins
            return;
        // if one player drops below 4 cards, the other player wins
        // if they are both below four cards, the player with more cards wins
        if (p1NumOfCards < minNumOfCards || p2NumOfCards < minNumOfCards)
        {
            winner = Mathf.Max(p1NumOfCards, p2NumOfCards);
            if (winner == p1NumOfCards)
                UpdateGameWinnerText(p1);
            else
                UpdateGameWinnerText(p2);
            isPressable = false;
        }
    }
}