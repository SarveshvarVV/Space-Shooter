using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    float rotation_speed = 3.0f;

    [SerializeField]
    GameObject explosion;

    private Spawn_Manager _sm;
    // Start is called before the first frame update
    void Start()
    {
        _sm = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotation_speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Lazer")
        {
            Instantiate(explosion,transform.position,Quaternion.identity);
            Destroy(collision.gameObject);
            _sm.StartSpawning();
            Destroy(this.gameObject,0.3f);
        }
    }
}
