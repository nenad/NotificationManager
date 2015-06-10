using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    Rigidbody rb;
    public float Speed;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        var hspeed = (Input.GetAxisRaw("Horizontal")) * Time.deltaTime * Speed;
        var vspeed = (Input.GetAxisRaw("Vertical")) * Time.deltaTime * Speed;

        transform.Translate(hspeed, 0, vspeed);

	}
}
