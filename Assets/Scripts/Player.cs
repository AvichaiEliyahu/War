using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNumber { one,two}
public class Player : MonoBehaviour
{
    [SerializeField] PlayerNumber number;
    [SerializeField] Pack pack;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card PullCard()
    {
        return pack.RemoveFromTop();
    }
}
