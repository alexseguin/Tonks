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

    // Update is called once per frame
    void Update()
    {
        if (CheckForCollision(this.transform.position))
        {
            ParticleSystem fireEffect = Instantiate(fire, transform.position, transform.rotation);
            ParticleSystem smokeEffect = Instantiate(smoke, transform.position, transform.rotation);
            Destroy(smokeEffect.gameObject, smokeEffect.main.duration);
            Destroy(fireEffect.gameObject, fireEffect.main.duration);
            Destroy(this.gameObject);
        }
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
