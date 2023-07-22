using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float speed = 4.0f;
    [SerializeField]
    int points = 10;

    private Player _player;
    private Animator Enemy_anim;

    [SerializeField]
    GameObject lazer_prefab;
    [SerializeField]
    float fireRate = 3.0f;
    float canFire = -1;

    AudioSource _enemy_as;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        Enemy_anim = GetComponent<Animator>();
        _enemy_as= GetComponent<AudioSource>();
    }
    void Update()
    {
        CalculateMovement();
        if (Time.time > canFire)
        {
            fireRate = Random.Range(3f, 7f);
            canFire = Time.time + fireRate;
            GameObject enemyLazer = Instantiate(lazer_prefab, transform.position, Quaternion.identity);
            Lazer[] lazer = enemyLazer.GetComponentsInChildren<Lazer>();
            for (int i =0; i < lazer.Length; i++)
            {
                lazer[i].ApplyEnemyLazer();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float RandomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(RandomX, 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            Destroy(GetComponent<Collider2D>());
            if (player != null)
            {
                player.Damage();
            }
            Enemy_anim.SetTrigger("Death");
            speed = 0;
            _enemy_as.Play();
            Destroy(this.gameObject,2.38f);
        }

        if(other.tag == "Lazer")
        {
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            if (_player != null)
            {
                _player.AddScore(points);
            }
            Enemy_anim.SetTrigger("Death");
            speed = 0;
            _enemy_as.Play();
            Destroy(this.gameObject,2.38f);
        }
    }
}
