using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Movement
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float speedMultiplier = 1.5f;

    //Projectile
    [SerializeField]
    GameObject LazerPrefab;
    [SerializeField]
    GameObject TripleShotPrefab;
    [SerializeField]
    float FireRate = 0.15f;
    [SerializeField]
    float CanFire = -1f;

    //PowerUps
    [SerializeField]
    bool isTripleShotActive = false;
    [SerializeField]
    bool isShieldActive = false;
    [SerializeField]
    GameObject Shields;
    [SerializeField]
    GameObject left_engine, right_engine;

    //Player_Info
    [SerializeField]
    int lives = 3;
    Spawn_Manager _spawnManager;
    [SerializeField]
    int score = 0;
    UIManager _UImanager;

    //Audio
    [SerializeField]
    AudioClip LazerSound;
    AudioSource _as;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        _UImanager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _as = GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > CanFire)
        {
            LazerShooting();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void LazerShooting()
    {
        CanFire = Time.time + FireRate;
        if (isTripleShotActive)
        {
            Instantiate(TripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(LazerPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _as.clip = LazerSound;
        _as.Play();
    }

    public void Damage()
    {
        if (isShieldActive)
        {
            isShieldActive= false;
            Shields.SetActive(false);
            return;
        }
        lives--;
        if (lives == 2)
        {
            left_engine.SetActive(true); 
        }
        else if (lives == 1)
        {
            right_engine.SetActive(true);
        }
        _UImanager.UpdateLives(lives);
        if (lives < 1)
        {
            lives= 0;
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActivation()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isTripleShotActive = false;
    }

    public void SpeedActivation()
    {
        speed *= speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        speed /= speedMultiplier;  
    }

    public void ShieldActivation()
    {
        isShieldActive= true;
        Shields.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
        _UImanager.UpdateScore(score);
    }
}
