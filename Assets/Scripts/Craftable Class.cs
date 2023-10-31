using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craftable", menuName = "Craftable")]

public class Craftable : ScriptableObject
{
    public new string item;
    public Sprite image;
    public Vector3 currentPosition;
    

    private void Print ()
    {
        Debug.Log(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
