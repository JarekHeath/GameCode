using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save", menuName = "Player Save")]

public class Player : ScriptableObject
{
    public new string name;
    public int health;
    public int stamina;
    public int food;
    public int water;
    public Vector3 currentPosition;


    private void Print()
    {
        Debug.Log(name);
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
