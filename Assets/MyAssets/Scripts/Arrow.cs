using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;

    public DragonEffect dragonEffect;
    [SerializeField] private float headshot;
    [SerializeField] private float bodyDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetType()==typeof(CircleCollider2D))
        {
            dragonEffect.TakeDamage(headshot); Destroy(gameObject);
        }
        if (collision.collider.GetType() == typeof(BoxCollider2D))
        {
            dragonEffect.TakeDamage(headshot); Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
