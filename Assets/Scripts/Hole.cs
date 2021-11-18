using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] Transform respawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponentInParent<Player>().FallInHole(transform, respawnPosition);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponentInParent<Enemy>().FallInHole(transform);
        }
    }
}
