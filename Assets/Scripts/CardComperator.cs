using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardComperator : IComparer<Card>
{
    public int Compare(Card c1, Card c2)
    {
        if (c1.GetLevel() < c2.GetLevel())
            return 1;
        else if (c1.GetLevel() > c2.GetLevel())
            return -1;
        else return 0;
    }
}
