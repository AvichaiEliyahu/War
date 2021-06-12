using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pack : MonoBehaviour
{
    [SerializeField] string playerName = "PLAYER";
    [SerializeField] List<Card> pack;

    // using deligate to inform the GameManager that the pack of cards is clicked
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    void Start()
    {
        Shuffle();
    }

    private void Shuffle()
    {
        for (int i = 0; i < pack.Count; i++)
        {
            Card temp = pack[i];
            int randomIndex = Random.Range(i, pack.Count);
            pack[i] = pack[randomIndex];
            pack[randomIndex] = temp;
        }
    }

    public Card RemoveFromTop()
    {
        Card firstCard = pack[0];
        pack.RemoveAt(0);
        return firstCard;
    }

    public void AddToBack(Card card)
    {
        pack.Add(card);
    }

    public int GetNumOfCards()
    {
        return pack.Count;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    private void OnMouseDown()
    {
        if (OnClicked != null)
            OnClicked();
    }
}
