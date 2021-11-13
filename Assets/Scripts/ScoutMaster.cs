using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;


public class ScoutMaster : PathingBrain
{
	int ClickableMasks;

	ScoutBrain selectedScout;

	[SerializeField] Transform SelectedIndicator;

	// move to world manager later
	public static int Health = 0;

    // Start is called before the first frame update
    override protected void Start()
    {
		base.Start();
		ClickableMasks = LayerMask.GetMask("Ground", "Clickable","Scout","UI");
    }

	// Update is called once per frame
	override protected void Update()
    {
		base.Update();
		SelectedIndicator.LookAt(Camera.main.transform);


        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
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

							SelectedIndicator.SetParent(null);
							SelectedIndicator.position = Vector3.zero;
						}
						else
							SetDestination(info.point);
						break;
					case "Scout":
						ScoutBrain newScout = info.collider.GetComponent<ScoutBrain>();
						selectedScout = (selectedScout == newScout) ? null : info.collider.GetComponent<ScoutBrain>();
						
						SelectedIndicator.SetParent(selectedScout.transform);
						SelectedIndicator.localPosition = Vector3.up * 0.7f;

						break;
					case "Job":
						if (selectedScout)
						{
							selectedScout.AssignJob(info.collider.GetComponent<BasicJob>());
							selectedScout = null;

							SelectedIndicator.SetParent(null);
							SelectedIndicator.position = Vector3.zero;
						}
						break;
				}			
			}
		}
    }
}
