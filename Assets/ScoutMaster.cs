using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class ScoutMaster : MonoBehaviour
{
	AIPath path;
	int ClickableMasks;


    // Start is called before the first frame update
    void Start()
    {
		path = GetComponent<AIPath>();
		ClickableMasks = LayerMask.GetMask("Ground", "Scout");
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
				switch (info.transform.gameObject.tag)
				{
					case "Ground":
						break;
					case "Scout":
						break;
				}			
			}
		}
    }
}
