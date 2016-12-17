using UnityEngine;
using System.Collections;

public class DemoSpin : MonoBehaviour {
    private float rotateRate = 90f;

	void Update () {
	    transform.Rotate(Vector3.up, rotateRate * Time.deltaTime);
	}
}
