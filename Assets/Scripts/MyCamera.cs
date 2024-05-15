using UnityEngine;
using System.Collections;

//Code by StallWorth

public class MyCamera : MonoBehaviour {
	//Camera settings
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject target;
	public Vector3 offset;
	Vector3 targetPos;

	void Awake () 
	{
	//positions the camera to character
		Vector3 camPos = transform.position;
		camPos.x = 0;
		camPos.y = 0.4f;
		offset = new Vector3 (camPos.x, camPos.y, 0);
		//set camera position
		targetPos = transform.position;
	}
	

	void FixedUpdate () 
	{
		//moves camera in relationship to character
		if (target) {
			Vector3 posNoZ = transform.position;
			posNoZ.z = target.transform.position.z;
			Vector3 targetDirection = (target.transform.position - posNoZ);
			interpVelocity = targetDirection.magnitude * 100f;
			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
			transform.position = Vector3.Lerp (transform.position, targetPos + offset, 0.35f);
		}
	}
}
