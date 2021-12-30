using UnityEngine;

public class MainCameraController : MonoBehaviour
{

    public Transform following;
    float yOffset = 5f;
    float xOffset = 5f;
    float sensitivity = 10f;
    float minSize = 9f;
    float maxSize = 18f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(following.position.x + xOffset, following.position.y + yOffset, this.transform.position.z);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            var sz = Camera.main.orthographicSize;
            
            sz -= scroll * sensitivity;
            sz = Mathf.Clamp(sz, minSize, maxSize);
            Camera.main.orthographicSize = sz;
        }

    }
}
