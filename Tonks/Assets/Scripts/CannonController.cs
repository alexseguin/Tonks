using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    LineRenderer lineRenderer;

    public GameObject cannon;
    public GameObject muzzle;
    public float cannonRotationSpeed = 0.5f;

    // Cannon launch speed
    public float launchSpeed = 25f;
    float simulateForDuration = 5f;//simulate for 5 secs in the furture
    float simulationStep = 0.1f;//Will add a point every 0.1 secs.

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateCannon();
        UpdateLaunchSpeed();
        SimulateArc();
    }

    public void RotateCannon()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(cannon.transform.position);

        //Get the Screen position of the mouse
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

        float rotationAngle = AngleBetweenPoints(positionOnScreen, mouseWorldPosition) + 180;

        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAngle));
        if ((cannon.transform.eulerAngles.z) > 80f && cannon.transform.eulerAngles.z < 357f)
        {
            rotation.z = 0f;
        }
        cannon.transform.rotation = Quaternion.Lerp(cannon.transform.rotation, rotation, Time.deltaTime * cannonRotationSpeed);
    }
    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void UpdateLaunchSpeed()
    {
        // when holding down w key, increase launch speed smoothly
        if (Input.GetKey(KeyCode.W) && launchSpeed < 50f)
        {
            launchSpeed = Mathf.Lerp(launchSpeed, launchSpeed+2f, Time.deltaTime * 5f);
        } else if(Input.GetKey(KeyCode.S) && launchSpeed > 10f)
        {
            launchSpeed = Mathf.Lerp(launchSpeed, launchSpeed-2f, Time.deltaTime * 5f);
        }
    }

    private float collisionCheckRadius = 0.1f;
    private void SimulateArc()
    {

        int steps = (int)(simulateForDuration / simulationStep);//50 in this example
        Vector2 directionVector = muzzle.transform.position - cannon.transform.position;
        Vector2 launchPosition = new Vector2(muzzle.transform.position.x, muzzle.transform.position.y);

        Vector2 calculatedPosition;
        List<Vector2> lineRendererPoints = new List<Vector2>();
        for (int i = 0; i < steps; ++i)
        {
            calculatedPosition = launchPosition + (directionVector * (launchSpeed * i * simulationStep));
            //Calculate gravity
            calculatedPosition.y += Physics2D.gravity.y * (i * simulationStep) * (i * simulationStep);
            lineRendererPoints.Add(calculatedPosition);
            if (CheckForCollision(calculatedPosition))//if you hit something
            {
                break;//stop adding positions
            }
        }


        lineRenderer.positionCount = lineRendererPoints.Count;
        lineRendererPoints.ForEach(point =>
        {
            lineRenderer.SetPosition(lineRendererPoints.IndexOf(point), point);
        });

    }
    private bool CheckForCollision(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, collisionCheckRadius);
        if (hits.Length > 0)
        {
            //We hit something 
            //check if its a wall or seomthing
            //if its a valid hit then return true
            return true;
        }
        return false;
    }
}
