using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    private float collisionCheckRadius = 0.1f;
    private ParticleSystem particles;

    public ParticleSystem smoke;
    public ParticleSystem fire;

    // Start is called before the first frame update
    void Start()
    {

    }

    private float power = 1f;
    private float radius = 3f;

    // Update is called once per frame
    void Update()
    {
        if (CheckForCollision(this.transform.position))
        {
            ShellCollisionParticles();

            Vector3 explosionPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
            foreach (Collider2D hit in colliders)
            {
                var hitRigid = hit.GetComponent<Rigidbody2D>();
                if (hit.gameObject.tag == "Player")
                {
                    hitRigid.AddForce((power * (hit.transform.position - explosionPos).normalized) , ForceMode2D.Impulse);
                }
            }
            Destroy(this.gameObject);
        }
    }

    private void ShellCollisionParticles()
    {
        ParticleSystem fireEffect = Instantiate(fire, transform.position, transform.rotation);
        ParticleSystem smokeEffect = Instantiate(smoke, transform.position, transform.rotation);
        Destroy(smokeEffect.gameObject, smokeEffect.main.duration);
        Destroy(fireEffect.gameObject, fireEffect.main.duration);
    }

    private bool CheckForCollision(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, collisionCheckRadius);
        foreach (var item in hits)
        {
            if (item.gameObject.tag != "Shell")
            {
                return true;
            }
        }
        return false;
    }
}
