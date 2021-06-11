using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    [SerializeField] Pack p1;
    [SerializeField] Pack p2;

    [SerializeField] float pressCooldown=1.5f;
    bool isPressable = true; // a few seconds of cooldown required before next press

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

        Card[] currentCards = InstantiateAndReturnTwoCards(); // get two cards from top of each pack

        int result = CompareCards(currentCards[0],currentCards[1]); // compare cards and act according to the results
        RoundWinner(result, currentCards);
        
        yield return new WaitForSecondsRealtime(pressCooldown); // cooldown before next rutine
        isPressable = true;
    }

    int CompareCards(Card c1, Card c2)
    {
        return c1.CompareTo(c2);
    }

    void RoundWinner(int result,Card[] cards) {
        List<Card> cardsForWinner = new List<Card>();
        AddTwoCardsToList(cardsForWinner, cards);
        switch (result)
        {
            case 1:
                AddCardsToWinner(p1, cardsForWinner);
                break;
            case -1:
                AddCardsToWinner(p2, cardsForWinner);
                break;
            case 0:
                List<Card> warCards = new List<Card>();
                AddTwoCardsToList(warCards,cards);
                StartCoroutine(War(warCards));
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
        yield return new WaitForSecondsRealtime(pressCooldown);
        Card[] bonusCards = InstantiateAndReturnTwoCards();
        AddTwoCardsToList(warCardsList, bonusCards);

        yield return new WaitForSecondsRealtime(pressCooldown);
        Card[] warCards = InstantiateAndReturnTwoCards();
        AddTwoCardsToList(warCardsList, warCards);

        int result = CompareCards(warCards[0], warCards[1]);
        if (result == 0)
            StartCoroutine(War(warCardsList));
        else if (result == 1)
            AddCardsToWinner(p1, warCardsList);
        else AddCardsToWinner(p2, warCardsList);
        
    }

    Card[] InstantiateAndReturnTwoCards()
    {
        //getting two first cards
        Card c1 = p1.RemoveFromTop();
        Card c2 = p2.RemoveFromTop();
        
        //instantiating them for animation
        GameObject g1= Instantiate(c1, p1.transform.position, p1.transform.rotation).gameObject;
        GameObject g2= Instantiate(c2, p2.transform.position, p2.transform.rotation).gameObject;
       
        Card[] cards = new Card[2];

        cards[0] = c1;
        cards[1] = c2;

        //destoy game object but keep the cards
        Destroy(g1,pressCooldown);
        Destroy(g2,pressCooldown);
        return cards;
    }

    void AddCardsToWinner(Pack winner, List<Card> cards)
    {
        foreach (Card c in cards)
            winner.AddToBack(c);
    }

    void AddTwoCardsToList(List<Card> list, Card[] cards)
    {
        list.Add(cards[0]);
        list.Add(cards[1]);
    }

    void CheckWinner()
    {
        int p1NumOfCards = p1.GetNumOfCards();
        int p2NumOfCards = p2.GetNumOfCards();
    }
}