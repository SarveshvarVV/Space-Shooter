using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;

    //ID for PowerUp
    //0 = tripleshot
    //1 = speed
    //2 = shield
    [SerializeField]
    int PowerUpID;

    [SerializeField]
    AudioClip _powerupClip;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
            Player player = collision.transform.GetComponent<Player>();
            if(player != null)
            {
                switch(PowerUpID)
                {
                    case 0:
                        player.TripleShotActivation();
                        break;
                    case 1:
                        player.SpeedActivation(); 
                        break;
                    case 2:
                        player.ShieldActivation();
                        break;
                    default:
                        Debug.Log("Nill Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
