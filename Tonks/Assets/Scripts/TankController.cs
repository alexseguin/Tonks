using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    // State variables
    private bool isMoving = false;


    public Transform muffler;
    public ParticleSystem smoke;
    private ParticleSystem activeSmoke;


    // Start is called before the first frame update
    void Start()
    {
        activeSmoke = Instantiate(smoke, muffler.position, muffler.rotation);
        activeSmoke.transform.parent = this.muffler;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTank();
        MufflerEmissions();
    }

    private void MoveTank() 
    {
        // GetAxisRaw(horizontal): (a / d) or (left / right) arrow
        float inputX = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        if(inputX < 0 || inputX > 0) {
            isMoving = true;
        } else {
            isMoving = false;
        }
        transform.position = new Vector3(transform.position.x + inputX, transform.position.y, transform.position.z);
    }

    private void MufflerEmissions() {
        if(isMoving && !activeSmoke.isPlaying) {
            activeSmoke.Play();
        } else if(!isMoving && activeSmoke.isPlaying) {
            activeSmoke.Stop();
        }
    }
}
