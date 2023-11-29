using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearCode : MonoBehaviour
{
    [SerializeField]
    private AudioSource enemySound;
    [SerializeField]
    private AudioClip Death;
    [SerializeField]
    private NavMeshAgent enemy;
    [SerializeField]
    private GameObject enemyPrefab;

    public int maxHealth = 5;
    public int health;
    public float distance;
    public float attackTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        GameObject player = GameObject.FindWithTag("Player");
        NavMeshAgent enemy = GetComponent<NavMeshAgent>();
        distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance <= 3)
        {
            enemy.destination = player.transform.position;
        }
        if (distance <= 1 && attackTimer >= 1.0f)
        {
            attackTimer = 0.0f;
            //Start Hit animation
            Debug.Log("The bear wanted to hit you but doesn't have animation");
        }
        //Bear checks an area around it.
        //If player is nearby, move towards player
        //If in certain range of player, start hit animation
        //If bear collides with axe during player hit animation, take damage.

    }


    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            enemySound.PlayOneShot(Death);
            Destroy(gameObject);
            //Drop 3 meat at gameObject's position

        }

    }
}
