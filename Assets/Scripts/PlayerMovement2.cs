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
using NUnit.Framework.Interfaces;
using System;
using static System.Net.Mime.MediaTypeNames;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement2 : MonoBehaviour
{
    public static event HandleMeatCollected OnEat;
    public delegate void HandleMeatCollected(ItemData itemData, int amount);
    public static event HandleItemsCollected OnPlace;
    public delegate void HandleItemsCollected(ItemData itemData, int amount1, ItemData itemData2, int amount2);
    public static event Action OnMeatsCollected;
    public static event Action OnWoodsCollected;
    public static event Action OnLeafsCollected;
    public delegate void Action (ItemData itemData);

    public ItemData meatData;
    public ItemData woodData;
    public ItemData leafData;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction hitAction;
    private InputAction placeAction;
    private InputAction menuAction;
    private InputAction eatAction;
    private InputAction drinkAction;

    public float moveSpeed = 2.0f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 8.0f;
    public bool isGrounded;

    private CharacterController player;
    private PlayerInput input;
    private Vector3 position;
    private Transform moveCamera;

    Animator animator;

    public float hitTimer = 0.0f;
    public float damageTimer = 0.0f;

    public float posX;
    public float posY;
    public float posZ;
    public float health = 100;
    public float food = 100;
    public float water = 100;
    public float stamina = 100;
    public float New;
    public float file;
    public int wood;
    public int leaf;
    public int meat;

    Vector3 spawnPoint;

    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Text staminaText;
    public UnityEngine.UI.Text foodText;
    public UnityEngine.UI.Text waterText;
    private int food2;
    private int water2;
    private int stamina2;
    private int health2;


    // Start is called before the first frame update
    void Start()
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
        menuAction = input.actions["Exit"];
        hitAction = input.actions["Hit"];
        placeAction = input.actions["Place"];
        eatAction = input.actions["Eat"];
        drinkAction = input.actions["Drink"];

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (New == 0)
        {
            ClearSave();
        }

        LoadSettings();
        LoadInventory();
        player.transform.position = new Vector3 (posX,posY, posZ);


    }


    void Update()
    {
        GameObject spawnpoint = GameObject.FindWithTag("Respawn");
        if (health <= 0)
        {

            player.enabled = false;
            player.transform.position = spawnpoint.transform.position;
            health = 100;
            food = 100;
            water = 100;
            stamina = 100;
            player.enabled = true;
            SaveSettings();
        }
        SaveSettings();
        food2 = Mathf.RoundToInt(food);
        water2 = Mathf.RoundToInt(water);
        health2 = Mathf.RoundToInt(health);
        stamina2 = Mathf.RoundToInt(stamina);
        healthText.text = "Health: " + health2.ToString() + "/100";
        staminaText.text = "Stamina: " + stamina2.ToString() + "/100";
        foodText.text = "Food: " + food2.ToString() + "/100";
        waterText.text = "Water: " + water2.ToString() + "/100";
        posX = player.transform.position.x;
        posY= player.transform.position.y;
        posZ = player.transform.position.z;
        food -= (1 * Time.deltaTime/10);
        water -= (1 * Time.deltaTime/10);
        stamina += 1 * Time.deltaTime;
        if ( food < 0)
        {
            health -= 1 * Time.deltaTime;
            food = 0;
        }
        if ( water < 0 )
        {
            health -= 1 * Time.deltaTime;
            water = 0;
        }
        if (food >=50)
        {
            if (health <= 99.7)
            {
                health += 1 * Time.deltaTime;
            }
        }
        if (stamina > 100)
        {
            stamina = 100;
        }
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
            if (runAction.IsPressed() == true && stamina >= 0)
            {
                moveSpeed = 4.0f;
                animator.SetInteger("Movement", 2);
                stamina -= 10 * Time.deltaTime;
            }
            else
            {
                moveSpeed = 2.0f;
                animator.SetInteger("Movement", 1);
            }
        }
        else
        {
            animator.SetInteger("Movement", 0);
        }

        player.Move(move * Time.deltaTime * moveSpeed);

        if(eatAction.IsPressed() == true)
        {
            OnEat?.Invoke(meatData, 1);
        }
        if (menuAction.IsPressed() == true)
        {
            SaveSettings();
            SceneManager.LoadScene("Title");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


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
            hitTimer = -1.0f;
        }

        if (placeAction.IsPressed() == true)
        {
            animator.SetInteger("Movement", -1);
            animator.SetBool("Place", true);

        }

        hitTimer += 1 * Time.deltaTime;
        damageTimer += 1 * Time.deltaTime;
        position.y += gravityValue * Time.deltaTime;
        player.Move(position * Time.deltaTime);

        // Rotate towards camera direction.
        if (input.x != 0 || input.y != 0)
        {
            float targetAngle = moveCamera.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        SaveSettings();

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



    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject enemy = collision.gameObject;
            Animator enemAnim = enemy.GetComponent<Animator>();
            if (damageTimer >= 1 && enemAnim.GetBool("Hit") == true)
            {
                damageTimer = 0;
                TakeDamage(10);
            }
           
        }

        if (collision.gameObject.tag == "Drinkable" && drinkAction.IsPressed() == true)
        {
            water += 20 * Time.deltaTime;
            if (water > 100)
            {
                water = 100;
            }
        }

        if (collision.gameObject.tag == "Sea" && placeAction.IsPressed() == true)
        {
            OnPlace?.Invoke(leafData, 6, woodData, 10);
        }
    }
    private void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey("PosX" + file);
        PlayerPrefs.DeleteKey("PosY" + file);
        PlayerPrefs.DeleteKey("PosZ" + file);
        PlayerPrefs.DeleteKey("Health" + file);
        PlayerPrefs.DeleteKey("Food" + file);
        PlayerPrefs.DeleteKey("Water" + file);
        PlayerPrefs.DeleteKey("Meat" + file);
        PlayerPrefs.DeleteKey("Wood" + file);
        PlayerPrefs.DeleteKey("Leaf" + file);
    }

    public void LoadSettings()
    {
        posX = PlayerPrefs.GetFloat("PosX" + file, 78);
        posY = PlayerPrefs.GetFloat("PosY" + file, 2);
        posZ = PlayerPrefs.GetFloat("PosZ" + file, -80);
        health = PlayerPrefs.GetFloat("Health" + file, 100);
        food = PlayerPrefs.GetFloat("Food" + file, 100);
        water = PlayerPrefs.GetFloat("Water" + file, 100);
        meat = PlayerPrefs.GetInt("Meat" + file, 0);
        wood = PlayerPrefs.GetInt("Wood" + file, 0);
        leaf = PlayerPrefs.GetInt("Leaf" + file, 0);

    }

    public void LoadInventory()
    {
        int i = meat;
        int j = wood;
        int k = leaf;
        for (int a = 0; a < i; a++)
        {
            OnMeatsCollected?.Invoke(meatData);
        }
        for (int a = 0; a < j; a++)
        {
            OnWoodsCollected?.Invoke(woodData);
        }
        for (int a = 0; a < k; a++)
        {
            OnLeafsCollected?.Invoke(leafData);
        }

        meat = i;
        wood = j;
        leaf = k;
    }


    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("PosX" + file,posX);
        PlayerPrefs.SetFloat("PosY" + file,posY);
        PlayerPrefs.SetFloat("PosZ" + file,posZ);
        PlayerPrefs.SetFloat("Health" + file,health);
        PlayerPrefs.SetFloat("Food" + file,food);
        PlayerPrefs.SetFloat("Water" + file,water);
        PlayerPrefs.SetInt("Meat" + file, meat);
        PlayerPrefs.SetInt("Wood" + file, wood);
        PlayerPrefs.SetInt("Leaf" + file, leaf);
    }

    private void OnEnable()
    {
        Inventory.OnUsedMeat += Heal;
        Inventory.OnMakeBoat += Won;
        Inventory.OnAddedItem += Add;
        Inventory.OnRemovedItem += Remove;
    }

    private void OnDisable()
    {
        Inventory.OnUsedMeat -= Heal;
        Inventory.OnMakeBoat -= Won;
        Inventory.OnAddedItem -= Add;
        Inventory.OnRemovedItem -= Remove;
    }

    public void Heal()
    {
        food += 20;
        if (food > 100)
        {
            food = 100;
        }
        Remove(meatData);
    }

    public void Won()
    {
        SceneManager.LoadScene("End");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Add(ItemData item)
    {
        if (item == meatData)
        {
            meat++;
        }
        if (item == woodData)
        {
            wood++;
        }
        if (item == leafData)
        {
            leaf++;
        }
    }

    public void Remove(ItemData item)
    {
        if (item == meatData)
        {
            meat--;
        }
        if (item == woodData)
        {
            wood--;
        }
        if (item == leafData)
        {
            leaf--;
        }
    }
}



