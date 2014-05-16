using UnityEngine;
using System.Collections;

public class OnGuiScript : MonoBehaviour {

    public Transform CameraObject;

    void Start() {
        CameraObject = Camera.main.transform;
    }

	// Update is called once per frame
	void Update () {
        this.transform.position = CameraObject.transform.position + new Vector3(0f, -0.2f, 4f);
	}
}
