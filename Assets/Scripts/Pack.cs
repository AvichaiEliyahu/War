using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pack : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    [SerializeField] List<Card> pack;
    // Start is called before the first frame update
    void Start()
    {
       // Shuffle();
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

    private void OnMouseDown()
    {
        if (OnClicked != null)
            OnClicked();
    }
}
