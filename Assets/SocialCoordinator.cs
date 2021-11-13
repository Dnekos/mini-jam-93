using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialCoordinator : MonoBehaviour
{
	ScoutBrain[] AllScouts;
	BasicJob[] AllJobs;
	List<string> names;

	[SerializeField] GameObject ScoutUIPrefab;
	int mask;

	private void Awake()
	{
		AllScouts = FindObjectsOfType<ScoutBrain>();
		AllJobs = FindObjectsOfType<BasicJob>();

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
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit info;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out info, 100.0f, mask))
			{
				Debug.Log("right clicked on " + info.collider.gameObject);
				Instantiate(ScoutUIPrefab, transform).GetComponent<ScoutUI>().scout = info.collider.GetComponent<ScoutBrain>();
			}
		}
	}

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
		} while (job.Completed && ++redun < 100);
		return job;
	}

	#region Thought lists
	public string SocialThought()
	{
		return "haha";
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
