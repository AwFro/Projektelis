﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinDetectionScript : MonoBehaviour {

    public int coin;
    private Text coincount;
    void Start()
    {
        coincount = GameObject.Find("CoinCounter").GetComponent<Text>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            if (coincount.text == "---") coin = 0;
            else coin = int.Parse(coincount.text);     
            coin += 1;
            coincount.text =  coin.ToString();
            Destroy(gameObject);
        }

    }
    

        

    
}
