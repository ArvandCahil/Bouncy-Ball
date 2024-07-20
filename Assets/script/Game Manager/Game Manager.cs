using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
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
        public bool Star(int amount)
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

    public class inventoryManager
    {
        public bool addItem(int id)
        {
            if(!current.playerInfo.inventory.ownedBallId.Contains(id))
            {
                current.playerInfo.inventory.ownedBallId.Append(id);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool removeItem(int id)
        {
            for(int i = 0; i < current.playerInfo.inventory.ownedBallId.Length; i++)
            {
                if (current.playerInfo.inventory.ownedBallId[i] == id)
                {
                    current.playerInfo.inventory.ownedBallId = RemoveAt(current.playerInfo.inventory.ownedBallId, id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
            
        }

        public bool equipitem(int id)
        {
            if (current.playerInfo.inventory.ownedBallId.Contains(id))
            {

            }
            else
            {
                return false;
            }
        }

        public static T[] RemoveAt<T>(T[] source, int index)
        {
            if (index < 0 || index >= source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            T[] result = new T[source.Length - 1];
            if (index > 0)
            {
                Array.Copy(source, 0, result, 0, index);
            }
            if (index < source.Length - 1)
            {
                Array.Copy(source, index + 1, result, index, source.Length - index - 1);
            }

            return result;
        }

    }

    public class saveManager
    {
        public void save()
        {
            string json = JsonUtility.ToJson(current.playerInfo);
        }
        public void load() 
        { 
        
        }
    }
}
