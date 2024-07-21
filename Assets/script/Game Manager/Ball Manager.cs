using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinManager : MonoBehaviour
{
    public static skinManager instance;

    [SerializeField] public skinInformation skinInformation;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public skin get_information(int id) 
    {
        foreach(skin obj in skinInformation.skins)
        {
            if (obj.ID == id)
            {
                return obj;
            }
        }
        return null; 
    }

}
