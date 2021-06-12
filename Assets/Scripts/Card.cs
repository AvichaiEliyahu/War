using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Card : MonoBehaviour, IComparable<Card>
{
    int numOfAnimations = 4;
    [SerializeField] [Range(2, 14)] int level;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PlayRandomAnimation();
    }

    public int GetLevel()
    {
        return level;
    }

    void PlayRandomAnimation()
    {
        int randomIndex = UnityEngine.Random.Range(1, numOfAnimations+1);
        animator.SetTrigger("anim"+randomIndex);
    }

    public int CompareTo(Card other)
    {
        if (level > other.GetLevel())
            return 1;
        else if (level < other.GetLevel())
            return -1;
        else return 0;
    }
}
