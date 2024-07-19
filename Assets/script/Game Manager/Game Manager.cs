using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public ball_Information ball;
    private playerInformation playerInfo;

    private void Start()
    {
        if (current != null)
        {
            current = this;
        }
            
    }

    public class currencyManager
    {
        public bool star(int amount)
        {
            if(current.playerInfo.currency.star >= amount) 
            {
                current.playerInfo.currency.star -= amount;
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

    public class saveManager
    {
        public void save()
        {
            
        }
        public void load() 
        { 
        
        }
    }
}
