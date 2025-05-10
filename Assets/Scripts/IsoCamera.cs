using UnityEngine;

public class IsoCamera : MonoBehaviour {
    public Transform cameraTarget;
    void Start() {
        transform.rotation = Quaternion.Euler(45, 0, 0); // 3/4 view
        //GetComponent<Camera>().orthographic = true;
        //GetComponent<Camera>().orthographicSize = 8; // Adjust zoom
    }
    void LateUpdate() {
        transform.position = cameraTarget.position + new Vector3(0, 15, -10 );
    }
}