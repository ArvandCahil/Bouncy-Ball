using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class generalManager : MonoBehaviour
{
    public static generalManager instance;
    private playerInformation playerInformation;

    private void Start()
    {
        if (instance != null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void newPlayer()
    {
        

    }

    public static class currency
    {
        public static class star
        {
            public static bool spend(int amount)
            {
                if (instance.playerInformation.currency.star >= amount)
                {
                    instance.playerInformation.currency.star -= amount;
                    refresh();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool add(int amount) 
            {
                if (instance.playerInformation.currency.star >= amount)
                {
                    instance.playerInformation.currency.star += amount;
                    refresh() ;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static void refresh()
            {
                TextMeshProUGUI starText = GameObject.FindGameObjectWithTag("Star").GetComponent<TextMeshProUGUI>();
                starText.text = instance.playerInformation.currency.star.ToString();
            }
        }

    }

    public class inventory
    {
        public class skin
        {
            public bool addItem(int id)
            {
                if (!instance.playerInformation.inventory.ownedSkinID.Contains(id))
                {
                    instance.playerInformation.inventory.ownedSkinID.Append(id);
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public bool removeItem(int id)
            {
                for (int i = 0; i < instance.playerInformation.inventory.ownedSkinID.Length; i++)
                {
                    if (instance.playerInformation.inventory.ownedSkinID[i] == id)
                    {
                        instance.playerInformation.inventory.ownedSkinID = RemoveAt(instance.playerInformation.inventory.ownedSkinID, id);
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
                if (instance.playerInformation.inventory.ownedSkinID.Contains(id))
                {
                    instance.playerInformation.inventory.equippedSkinID = id;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static T[] RemoveAt<T>(T[] source, int index)
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
            string json = JsonUtility.ToJson(instance.playerInformation);
            string path = Application.persistentDataPath + "/" + instance.playerInformation.profile.name;
            StreamWriter writer = new StreamWriter(path);
            writer.Write(json);
        }
        public void load() 
        {
            string path = Application.persistentDataPath + "/" + instance.playerInformation.profile.name;
            StreamReader reader = new StreamReader(path);
            instance.playerInformation = JsonUtility.FromJson<playerInformation>(reader.ReadToEnd());
            generalManager.currency.star.refresh();
        }
    }
}
