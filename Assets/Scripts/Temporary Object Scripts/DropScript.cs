using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{
    public int value = 5;

    GameObject player;
    public float pickupRange = 0.5f;


    public Drop drop;

    public Drop.DropType type;


    private void Start()
    {
        if(player == null)
        {
            player = LevelManager.instance.playerTransform.gameObject;
        }
    }


    void FixedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance < pickupRange)
        {
            DropFunction();
            BackToPool();
        }
    }
    void DropFunction()
    {
        drop.DropFunction(value);
    }

    void BackToPool()
    {
        LevelManager.instance.UnsubscribeDrop(Collect);
        Destroy(gameObject);//for now
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    public void Initialize(Drop drop_, Vector3 position_, int value_ = 0)
    {
        if(value_ != 0)
        {
            value = value_;
        }
        transform.position = position_;
        type = drop_.type;

        LevelManager.instance.SubscribeDrop(Collect);
    }

    void Collect()
    {
        BackToPool();
    }
}
