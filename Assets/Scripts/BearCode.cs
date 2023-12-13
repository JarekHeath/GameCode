using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearCode : MonoBehaviour
{
    [SerializeField]
    private AudioSource enemySound;
    [SerializeField]
    private AudioClip Hurt;
    [SerializeField]
    private NavMeshAgent enemy;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject Meat;
    [SerializeField]
    private Vector3 Spawnpoint;

    public int maxHealth = 5;
    public int health;
    public float distance;
    public float attackTimer = 0.0f;
    public float damageTimer = 0.0f;
    Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        health = maxHealth;
        animator = GetComponent<Animator>();
        NavMeshHit closestSpot;
        if (NavMesh.SamplePosition(position, out closestSpot, 100, 1))
        {
            enemy.transform.position = closestSpot.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= 0.0f)
        {
            animator.SetBool("Hit", false);
        }
        attackTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;
        GameObject player = GameObject.FindWithTag("Player");
        NavMeshAgent enemy = GetComponent<NavMeshAgent>();
        distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance <= 10)
        {
            enemy.destination = player.transform.position;
            animator.SetBool("Move", true);
        }
        if (distance <= 1 && attackTimer >= 1.0f)
        {
            attackTimer = -1.0f;
            animator.SetBool("Hit", true);
        }
        if (distance > 10)
        {
            animator.SetBool("Move", false);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Animator playerAnim = player.GetComponent<Animator>();
        if (collision.gameObject.tag == "Axe" && damageTimer >= 1 && playerAnim.GetBool("Hit") == true)
        {
            TakeDamage(1);
            damageTimer = 0.0f;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        enemySound.PlayOneShot(Hurt);
        if (health <= 0)
        {            
            Vector3 position = transform.position;
            Spawn(Meat, position);
            Spawn(Meat, position);
            Spawn(Meat, position);
            Destroy(gameObject);

        }
    }

    public void Spawn(GameObject Prefab, Vector3 Position)
    {
        Vector3 Spawnpoint = Position;
        GameObject enemy = GameObject.Instantiate(Prefab, Spawnpoint, Quaternion.identity);
    }
}
