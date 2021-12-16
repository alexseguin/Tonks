using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    // Get parent component reference


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxisRaw(horizontal): (a / d) or (left / right) arrow
        float inputX = Input.GetAxisRaw("Horizontal") * Time.deltaTime;

        transform.position = new Vector3(transform.position.x + inputX, transform.position.y, transform.position.z);
    }
}
