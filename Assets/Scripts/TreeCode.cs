using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class TreeCode : MonoBehaviour
{
    public int health = 4;
    public float damageTimer = 0.0f;

    [SerializeField]
    private GameObject Wood;
    [SerializeField]
    private GameObject Leaf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer += Time.deltaTime;
    }

    //Function where tree loses a health if interacted with an axe and damage timer is over 1.0f
    //Damage timer goes back to zero
    //Check if health is at or below zero.
    //If so, then create two wood gameobjects and one leaf.
    //Destroy tree gameobject.

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Axe" && damageTimer >= 1)
        {
            health -= 1;
            if (health <= 0)
            {
                Vector3 position = transform.position;
                Spawn(Wood, position);
                Spawn(Wood, position);
                Spawn(Leaf, position);
                Destroy(gameObject);
            }
                damageTimer = 0.0f;
        }
    }

    public void Spawn(GameObject Prefab, Vector3 Position)
    {
        Vector3 Spawnpoint = Position;
        GameObject enemy = GameObject.Instantiate(Prefab, Spawnpoint, Quaternion.identity);
    }


}
