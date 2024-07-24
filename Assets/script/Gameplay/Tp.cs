using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tp : MonoBehaviour
{
    public int tpId;
    private GameObject ball;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("unit");
        Debug.Log(ball.name);
        ////Teleport(() =>
        //{
            
        //});
    }

    public void Teleport(Action callback)
    {
        StartCoroutine(TeleportCoroutine(callback));
    }

    private IEnumerator TeleportCoroutine(Action callback)
    {
        GameObject[] tp = GameObject.FindGameObjectsWithTag("tp");

        foreach (GameObject tp2 in tp)
        {
            
            if (tp2 != this.gameObject && tpId == tp2.GetComponent<Tp>().tpId)
            {

                ball.transform.position = new Vector3(tp2.transform.position.x, tp2.transform.position.y + 1, tp2.transform.position.z);
                Debug.Log("Unit has been teleported.");
                yield return null; 
                callback?.Invoke();
            }
        }
    }

}
