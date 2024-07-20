using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;

    public ball_Information ball_Information;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public ball get_information(int id) 
    {
        foreach(ball obj in ball_Information.balls)
        {
            if (obj.id == id)
            {
                return obj;
            }
        }
        return null; 
    }

}
