using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;

    public skinInformation ball_Information;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public skin get_information(int id) 
    {
        foreach(skin obj in ball_Information.skins)
        {
            if (obj.ID == id)
            {
                return obj;
            }
        }
        return null; 
    }

}
