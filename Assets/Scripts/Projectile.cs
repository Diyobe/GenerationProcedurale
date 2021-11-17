using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    public float speed = 5;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;

        if (duration <= 0.0f)
            return;

        duration -= Time.deltaTime;
        if (duration <= 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & destroyOnHit) != 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
