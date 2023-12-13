using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenCode : MonoBehaviour
{
    [SerializeField]
    private AudioSource enemySound;
    [SerializeField]
    private AudioClip Hurt;
    [SerializeField]
    private GameObject Meat;
    [SerializeField]
    private NavMeshAgent chicken;
    public float walkTimer = 0.0f;
    public float damageTimer = 0.0f;
    public int maxHealth = 2;
    public int health;
    public int walkSpeed = 3;
    public int runSpeed = 5;
    Animator animator;
    Vector3 runAway;
    [SerializeField]
    private Vector3 Spawnpoint;



    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        health = maxHealth;
        animator = GetComponent<Animator>();
        NavMeshHit closestSpot;
        if (NavMesh.SamplePosition(position, out closestSpot, 100, 1))
        {
            chicken.transform.position = closestSpot.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        damageTimer += Time.deltaTime;

        if (damageTimer > 3)
        {
            walkTimer += Time.deltaTime;
            if (walkTimer > 3)
            {
                chicken.destination = chicken.transform.position + (new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * walkSpeed);
                walkTimer = 0;
            }

            animator.SetBool("Move", true);

        }
        else
        {
            runAway = chicken.transform.position - player.transform.position;
            chicken.destination = runAway * runSpeed;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Axe" && damageTimer >= 1)  //Add and player is in hit animation.
        {
            TakeDamage(1);
            damageTimer = 0;
            walkTimer = 0;
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
            Destroy(gameObject);

        }
    }

    public void Spawn(GameObject Prefab, Vector3 Position)
    {
        Vector3 Spawnpoint = Position;
        GameObject enemy = GameObject.Instantiate(Prefab, Spawnpoint, Quaternion.identity);
    }

}
