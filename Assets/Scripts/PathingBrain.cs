using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathingBrain : MonoBehaviour
{
	[Header("Pathing Variables"), SerializeField]
	Transform Reticle;
	protected AIPath path;

	protected virtual void Start()
	{
		path = GetComponent<AIPath>();
		if (Reticle == null)
			Reticle = GameObject.Find("PosReticle").transform;
	}
	protected void SetDestination(Vector3 point,Collider destCol = null)
	{
		path.destination = point;
		Reticle.position = point + Vector3.up * 0.01f;
		if (destCol == null)
			Reticle.localScale = Vector3.one * 0.25f;
		else
			Reticle.localScale = destCol.bounds.size;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
