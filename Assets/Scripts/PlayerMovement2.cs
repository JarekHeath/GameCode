using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement2 : MonoBehaviour
{

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction hitAction;
    private InputAction placeAction;
    private InputAction Menu;

    public float moveSpeed = 8.0f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 8.0f;
    public bool isGrounded;

    private CharacterController player;
    private PlayerInput input;
    private Vector3 position;
    private Transform moveCamera;

    Animator animator;

    public float hitTimer = 0.0f;

    public float volume;
    public float posX;
    public float posY;
    public float posZ;
    public float health;
    public float food;
    public float water;
    public float New;
    public float file; 


    // Start is called before the first frame update
    void Awake()
    {
        New = PlayerPrefs.GetInt("New");
        file = PlayerPrefs.GetInt("File");


        player = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        moveCamera = Camera.main.transform;

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        runAction = input.actions["Run"];
        Menu = input.actions["Main Menu"];
        hitAction = input.actions["Hit"];
        placeAction = input.actions["Place"];

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        LoadSettings();

        //Generate Player object
        //Move to position
        //Set stats to loaded ones
        //Set inventory to loaded one

    }


    void Update()
    {
       
        SaveSettings();
        animator.SetBool("Jump", false);
        if (hitTimer >= 0.0f)
        {
            animator.SetBool("Hit", false);
        }
        animator.SetBool("Place", false);
        isGrounded = player.isGrounded;
        if (isGrounded && position.y < 0)
        {
            position.y = -1.0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        move = move.x * moveCamera.right.normalized +
              move.z * moveCamera.forward.normalized;
        move.y = 0f;
        if (moveAction.IsPressed() == true)
        {
            if (runAction.IsPressed() == true)
            {
                moveSpeed = 16.0f;
                animator.SetInteger("Movement", 2);
            }
            else
            {
                moveSpeed = 8.0f;
                animator.SetInteger("Movement", 1);
            }
        }
        else
        {
            animator.SetInteger("Movement", 0);
        }

        player.Move(move * Time.deltaTime * moveSpeed);

        // Changes the height position of the player..

        if (jumpAction.triggered && isGrounded)
        {
            position.y += 6.0f;     //Change to acceptable jump
            animator.SetInteger("Movement", -1);
            animator.SetBool("Jump", true);
        }

        if (hitAction.IsPressed() == true && hitTimer >=1.0f)
        {
            animator.SetInteger("Movement", -1);
            animator.SetBool("Hit", true);
            hitTimer = -0.2f;
        }

        if (placeAction.IsPressed() == true)
        {
            animator.SetInteger("Movement", -1);
            animator.SetBool("Place", true);

        }

        hitTimer += 1 * Time.deltaTime;
        position.y += gravityValue * Time.deltaTime;
        player.Move(position * Time.deltaTime);

        // Rotate towards camera direction.
        if (input.x != 0 || input.y != 0)
        {
            float targetAngle = moveCamera.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        /*time0 += Time.deltaTime;
        time1 += Time.deltaTime;
        if (time0 >= interpolationPeriod0)
        {
            time0 = time0 - interpolationPeriod0;
            Spawn(firstEnemy);
        }*/

    }

    //Update for use in chickens and bears
    public void Spawn(GameObject Prefab)
    {
        Vector3 Spawnpoint = new Vector3(0, 4, 46);
        GameObject enemy = GameObject.Instantiate(Prefab, Spawnpoint, Quaternion.identity);
        NavMeshHit closestSpot;
        if (NavMesh.SamplePosition(Spawnpoint, out closestSpot, 100, 1))
        {
            enemy.transform.position = closestSpot.position;
            enemy.AddComponent<NavMeshAgent>();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bear")
        {
            //Check if bear is in attack animation
            //Take damage function
            //set timer to 0 for whether player can take damage
        }
        if (collision.gameObject.tag == "Item")
        {
            //Destroy object
            //Add object to inventory

        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Empty inventory and place items on ground.
            //Transfer player to spawn
            //Health goes back to full
            //Stats go back to full


        }
    }

    public void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("Volume" + file,100);
        posX = PlayerPrefs.GetFloat("PosX" + file, 0);
        posY = PlayerPrefs.GetFloat("PosY" + file, 2);
        posZ = PlayerPrefs.GetFloat("PosZ" + file, 0);
        health = PlayerPrefs.GetFloat("Health" + file, 100);
        food = PlayerPrefs.GetFloat("Food" + file, 100);
        water = PlayerPrefs.GetFloat("Water" + file, 100);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume" + file,volume);
        PlayerPrefs.SetFloat("PosX" + file,posX);
        PlayerPrefs.SetFloat("PosY" + file,posY);
        PlayerPrefs.SetFloat("PosZ" + file,posZ);
        PlayerPrefs.SetFloat("Health" + file,health);
        PlayerPrefs.SetFloat("Food" + file,food);
        PlayerPrefs.SetFloat("Water" + file,water);
    }
    



}



