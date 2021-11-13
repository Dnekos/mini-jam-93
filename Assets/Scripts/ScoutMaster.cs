using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class ScoutMaster : PathingBrain
{
	int ClickableMasks;

	ScoutBrain selectedScout;

	// move to world manager later
	public static int Health = 0;

    // Start is called before the first frame update
    override protected void Start()
    {
		base.Start();
		ClickableMasks = LayerMask.GetMask("Ground", "Clickable","Scout");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
			RaycastHit info;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out info, 100.0f, ClickableMasks))
			{
				Debug.Log("clicked on " + info.collider.gameObject);
				switch (info.transform.gameObject.tag)
				{
					case "Ground":
						if (selectedScout)
						{
							selectedScout.MoveTo(info.point);
							selectedScout = null;
						}
						else
							SetDestination(info.point);
						break;
					case "Scout":
						ScoutBrain newScout = info.collider.GetComponent<ScoutBrain>();
						selectedScout = (selectedScout == newScout) ? null : info.collider.GetComponent<ScoutBrain>();
						break;
					case "Job":
						if (selectedScout)
						{
							selectedScout.AssignJob(info.collider.GetComponent<BasicJob>());
							selectedScout = null;
						}
						break;
				}			
			}
		}
    }
}
