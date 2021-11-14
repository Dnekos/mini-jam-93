using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;
using TMPro;

public class SocialCoordinator : MonoBehaviour
{
	// move to world manager later
	public static float averageFun = 0;
	public static int JobsDone = 0;
	public static int NumJobs;

	ScoutBrain[] AllScouts;
	BasicJob[] AllJobs;
	List<string> names;

	// Social
	[Header("Social"), SerializeField]
	float QueueWaitTime = 2;
	[SerializeField] int MaxDiscussionSize = 4;
	[SerializeField] float DiscussionCircleRadius = 0.5f;
	public List<ScoutBrain> QueuedScouts;
	Coroutine co;
	bool matchmaking;

	[Header("UI"),SerializeField] GameObject ScoutUIPrefab;
	int mask;
	[SerializeField] GameObject InfoPanel;
	[SerializeField] TextMeshProUGUI[] infotexts;

	[Header("Pause"),SerializeField] GameObject PauseMenu;
	public bool paused = false;

	private void Awake()
	{
		AllScouts = FindObjectsOfType<ScoutBrain>();
		AllJobs = FindObjectsOfType<BasicJob>();
		QueuedScouts = new List<ScoutBrain>();

		mask = LayerMask.GetMask("Scout");

		names = new List<string>{
			"Demetri",
			"Peter",
			"Max",
			"Luke",
			"Michael",
			"Alex",
			"Vincent",
			"Ian",
			"Jacob",
			"Anthony",
			"Liam",
			"Cole",
			"Jake",
			"Matthew",
			"Steven",
			"Robert",
			"Nathan",
			"Nick",
			"Brunin",
			"Patrick",
			"Shane",
			"J.J.",
			"Dani",
			"Chris",
			"Wren",
			"Julian",
			"Aiden",
			"Al",
			"Ronald",
			"Spencer"};
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
		{
			RaycastHit info;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out info, 100.0f, mask))
			{
				Debug.Log("right clicked on " + info.collider.gameObject);
				if (info.collider.tag == "Scout")
					Instantiate(ScoutUIPrefab, transform).GetComponent<ScoutUI>().scout = info.collider.GetComponent<ScoutBrain>();
				else if (info.collider.tag == "Scoutmaster")
					InfoPanel.SetActive(true);
			}
		}
		for (int i = 0;  i < infotexts.Length; i++)
		{
			int total = 0, completed = 0;
			foreach (BasicJob job in AllJobs)
			{
				if (job.ID == (BasicJob.JobID)i)
				{
					total++;
					completed += (job.Completed) ? 1 : 0;
				}
			}
			infotexts[i].text = completed + "/" + total;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			PauseMenu.SetActive(paused);
			foreach (ScoutBrain scoot in AllScouts)
				scoot.GetComponent<AIPath>().canMove = !paused;
		}
	}

	public void PrepTransport()
	{
		Debug.Log("prepping trans");
		GameObject trans = Instantiate(new GameObject());
		trans.AddComponent(typeof(EndTransport));
		EndTransport transport = trans.GetComponent<EndTransport>();

		foreach (BasicJob job in AllJobs)
			if (job.Completed)
				transport.JobsDone++;
		foreach (ScoutBrain scoot in AllScouts)
			transport.averageFun += scoot.Fun;
		transport.averageFun /= AllScouts.Length;
		transport.NumJobs = AllJobs.Length;

		DontDestroyOnLoad(trans);
	}

	#region Social
	public void JoinQueue(ScoutBrain scoot)
	{
		Debug.Log(scoot.scoutName+ " joined matchmaking");

		QueuedScouts.Add(scoot);
		if (!matchmaking)
			co = StartCoroutine(BeginMatchmaking());
		if (QueuedScouts.Count >= MaxDiscussionSize)
		{
			StopCoroutine(co);
			AssignSocial();
		}
	}
	IEnumerator BeginMatchmaking()
	{
		matchmaking = true;
		Debug.Log("started matchmaking");
		yield return new WaitForSeconds(QueueWaitTime);
		Debug.Log("matchmaking timed out");

		AssignSocial();
	}
	void AssignSocial()
	{
		Debug.Log("placing scouts in social now");
		matchmaking = false;


		// ideal destination is midpoint of all pieces
		Bounds bound = new Bounds();
		foreach (ScoutBrain scoot in QueuedScouts)
			bound.Encapsulate(scoot.transform.position);
		Vector3 discussionCenter = bound.center;

		// check if that midpoint is valid, if not then use one of the scout position (i should later put a redundancy if that fails too)
		GraphNode startOfPath = AstarPath.active.GetNearest(QueuedScouts[0].transform.position).node;
		GraphNode endOfPath = AstarPath.active.GetNearest(discussionCenter).node;
		if (PathUtilities.IsPathPossible(startOfPath, endOfPath) == false)
			discussionCenter = QueuedScouts[0].transform.position;

		foreach(ScoutBrain scoot in QueuedScouts)
		{
			Vector3 rando = Random.insideUnitCircle;
			rando = discussionCenter + (new Vector3(rando.x, 0, rando.y)).normalized * DiscussionCircleRadius;
			scoot.startSocial(rando, Random.Range(0, 15));
		}
		QueuedScouts.Clear();
	}
	#endregion

	#region Assigning Scout Functions
	public string AssignName()
	{
		int index = Random.Range(0, names.Count);
		string returner = names[index];
		names.RemoveAt(index);
		return returner;
	}

	public ScoutBrain[] AssignFriends(ScoutBrain scout)
	{
		ScoutBrain[] friends = new ScoutBrain[2];
		for (int i = 0; i < 2; i++)
		{
			int redun = 0;
			do
			{
				friends[i] = AllScouts[Random.Range(0, AllScouts.Length)];
			} while ((friends[1] == friends[0] || friends[i] == scout) && ++redun < 100);
		}
		return friends;
	}
	public BasicJob AssignJob()
	{
		BasicJob job;
		int redun = 0;
		do
		{
			job = AllJobs[Random.Range(0, AllJobs.Length)];
		} while ((job.Completed || job.ActivelyWorkedOn)&& ++redun < 100);
		return job;
	}
	#endregion

	#region Thought lists
	public string SocialThought(ScoutBrain self, int index)
	{
		float maxDist = Mathf.Infinity;
		string talkedTo = "";
		foreach (ScoutBrain scoot in AllScouts)
		{
			float newDist = Vector3.Distance(self.transform.position, scoot.transform.position);
			if (newDist <= maxDist && scoot != self)
			{
				maxDist = newDist;
				talkedTo = scoot.scoutName;
			}
		}
		
		//int index = Random.Range(0, 15);

		switch (index)
		{
			case 0:
				return " told "+talkedTo+" about bugs.";
			case 1:		 
				return " listened to "+talkedTo+" talk about film.";
			case 2:		 
				return " played cards with "+talkedTo+".";
			case 3:		 
				return " told a riddle to "+talkedTo+".";
			case 4:		 
				return " joked around with "+talkedTo+".";
			case 5:		 
				return " argued with "+talkedTo+".";
			case 6:		 
				return " discussed frogs with "+talkedTo+".";
			case 7:		 
				return " lost at tag to "+talkedTo+".";
			case 8:		 
				return " went looking for " + talkedTo + ".";
			case 9:		 
				return " mused on the weather with " + talkedTo + ".";
			case 10:	 
				return " sang with " + talkedTo + ".";
			case 11:	 
				return " had a snack with " + talkedTo + ".";
			case 12:	 
				return " read a poem to " + talkedTo + ".";
			case 13:	 
				return " looked at a map with " + talkedTo + ".";
			case 14:	 
				return " told ghost stories to " + talkedTo + ".";
		}
		return " is idle.";
	}
	public string IdleThought()
	{
		int index = Random.Range(0, 15);

		switch(index)
		{
			case 0:
				return " is feeling bored.";
			case 1:		 
				return " is birdwatching.";
			case 2:		 
				return " is thinking about bugs.";
			case 3:		 
				return " is tying their shoes.";
			case 4:		 
				return " is thinking about the weather.";
			case 5:		 
				return " is feeling cold.";
			case 6:		 
				return " stepped on a flower.";
			case 7:		 
				return " is wandering about.";
			case 8:		 
				return " is looking around.";
			case 9:		 
				return " is lost in thought.";
			case 10:	 
				return " is getting hungry.";
			case 11:	 
				return " is eating a snack.";
			case 12:	 
				return " is idly pacing.";
			case 13:	 
				return " found a stick.";
			case 14:	 
				return " is keeping warm.";

		}
		return " is idle.";
	}
	public string WorkThought(BasicJob.JobID job)
	{
		int index = Random.Range(0, 6) + (6 * (int)job);
		switch (index)
		{
			case 0:
				return " is hammering stakes in.";
			case 1:		 
				return " is reading instructions.";
			case 2:		 
				return " is tying knots.";
			case 3:		 
				return " is unfolding tarps.";
			case 4:		 
				return " is unrolling sleeping bags.";
			case 5:		 
				return " got tangled in the tent.";
			case 6:		 
				return " caught some trash.";
			case 7:		 
				return " caught a fish.";
			case 8:		 
				return " felt a tug on the line.";
			case 9:		 
				return " is watching the water.";
			case 10:	 
				return " caught some fish.";
			case 11:	 
				return " let a fish go.";
			case 12:	 
				return " is chopping wood.";
			case 13:	 
				return " is sawing wood.";
			case 14:	 
				return " got a splinter.";
			case 15:	 
				return " is looking for sticks.";
			case 16:	 
				return " whittled a stick.";
			case 17:	 
				return " is holding a hatchet.";
			case 18:	 
				return " is placing logs.";
			case 19:	 
				return " is burning kindling.";
			case 20:	 
				return " burnt their hand.";
			case 21:	 
				return " is poking the fire.";
			case 22:	 
				return " is drying the wood.";
			case 23:	 
				return " is getting the fire hot.";
		}

		return " is working.";
	}
	#endregion
}
