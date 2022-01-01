using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LandGeneration : MonoBehaviour
{

    [SerializeField] private SpriteShapeController shape;


    [SerializeField] private int scale = 1000;
    [SerializeField] private float pointCount = 100;
    private float startHeight = 1f;

    private float endHeight = 15f;



    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        shape.spline.SetPosition(2, shape.spline.GetPosition(2) + Vector3.right * scale);
        shape.spline.SetPosition(3, shape.spline.GetPosition(3) + Vector3.right * scale);


        for (int i = 0; i < pointCount; i++)
        {
            float xPos = shape.spline.GetPosition(i+1).x + scale/pointCount;
            float yPos = this.transform.position.y;
            shape.spline.InsertPointAt(i+2, new Vector3(xPos, 10*Mathf.PerlinNoise(i*Random.Range(startHeight, endHeight),0)));
        };

        for (int i = 2; i < pointCount + 2; i++)
        {
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            shape.spline.SetLeftTangent(i, new Vector3(-1,0,0));
            shape.spline.SetRightTangent(i, new Vector3(1,0,0));
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateHeightMap()
    {
        Mathf.PerlinNoise(0, 0);
    }
}
