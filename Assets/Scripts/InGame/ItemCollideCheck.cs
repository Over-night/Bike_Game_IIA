using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollideCheck : MonoBehaviour
{
    [SerializeField] GameObject ItemBox;
    private Collider collideObject = null;
    private bool isDestroy = false;
    private bool isPlayer = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            collideObject = collision;
            isDestroy = true;
            isPlayer = true;
        }
        if (collision.tag == "Destroyer")
        {
            isDestroy = true;
        }
    }

    public bool GetIsDestroy()
    {
        return isDestroy;
    }
    public bool GetIsPlayer()
    {
        return isPlayer;
    }
    public Collider GetCollideObject()
    {
        return collideObject;
    }
}
