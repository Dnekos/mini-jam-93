using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
	[SerializeField] Transform target;

	[Header("Movement"), SerializeField, Tooltip("How fast the Camera moves")]
	float smoothTime = .5f;
	[SerializeField] Vector3 offset;
	Vector3 velocity;

	// Start is called before the first frame update
	void Start()
	{
		if (target == null)
			target = GameObject.FindGameObjectWithTag("Player").transform;
		transform.position = target.position + offset;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (target != null)
			transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);
	}
}
