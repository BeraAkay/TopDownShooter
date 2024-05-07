using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public static Dropper Instance;
    
    [SerializeField]
    GameObject expDrop;

    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    DropScript GetDroppable(Drop drop)
    {
        return Instantiate(expDrop).GetComponent<DropScript>();
    }

    public void Drop(Drop drop, Vector3 position, int value)
    {
        DropScript dropScript = GetDroppable(drop);
        dropScript.Initialize(drop, position);
    }


}
