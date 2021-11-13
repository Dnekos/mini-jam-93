using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScoutBrain : PathingBrain
{
	public enum ScoutState
	{
		Idle,
		Working,
		Socializing
	}
	public ScoutState state;
	bool inAction; // wether they are in the process of doing something

	[Header("Social")]
	[SerializeField] SocialCoordinator SC;

	public string scoutName;
	public ScoutBrain[] Friends;
	[SerializeField] int maxThoughts = 4;
	public List<string> thoughts;

	[Header("Idle"), SerializeField,Tooltip("Range for how long scout waits between idle movements")]
	Vector2 IdleWaitRange;
	[SerializeField, Tooltip("How far a single idle motion goes")]
	float IdleDist = 1;
	[SerializeField] float IdleFocusIncrement = 1;

	[Header("Individual Stats"), Tooltip("The amount of attention they give a task, decrements when doing a job, increments when idle.")]
	public float Focus;
	[Tooltip("How fast they fill a job's Progress bar")]
	public float Skill;
	[Tooltip("How happy the Scout is, determines score at the end.")]
	public float Fun;
	public int maxStat;

	[Header("Jobs"),SerializeField, Tooltip("Speed is calulated by multiplier / Skill")]
	float SpeedMultiplier;
	public BasicJob ActiveJob;
	Coroutine activeCo;
	
    // Start is called before the first frame update
    override protected void Start()
	{
		base.Start();

		// make sure destination isnt null
		path.destination = transform.position;

		if (SC == null)
			SC = FindObjectOfType<SocialCoordinator>();

		scoutName = SC.AssignName();
		
		thoughts = new List<string>();

		// picking friends
		Friends = SC.AssignFriends(this);
	}

	#region IEnumerators
	IEnumerator DoWork()
	{
		inAction = true;
		Debug.Log(scoutName + "waiting for " + (SpeedMultiplier / Skill));
		
		yield return new WaitForSeconds(SpeedMultiplier / Skill);
		ActiveJob.IncrementProgress();
		ActiveJob.UpdateScout(this);
		inAction = false;

		AddThought(SC.WorkThought(ActiveJob.ID));

		StateCheck();
	}

	IEnumerator DoIdle()
	{
		Debug.Log(scoutName + "waiting idly");

		inAction = true;
		yield return new WaitForSeconds(Random.Range(IdleWaitRange.x, IdleWaitRange.y));
		inAction = false;

		Focus += IdleFocusIncrement;
		
		AddThought(SC.IdleThought());

		StateCheck();
	}
	IEnumerator DoSocial()
	{
		inAction = true;
		yield return new WaitForSeconds(Random.Range(IdleWaitRange.x, IdleWaitRange.y));
		Focus++;
		Fun++;
		inAction = false;
		AddThought(SC.SocialThought());


		StateCheck();
	}
	#endregion
	void AddThought(string thought)
	{
		thoughts.Add(thought);
		if (thoughts.Count > maxThoughts)
			thoughts.RemoveAt(0);
	}

	#region scoutmaster actions
	public void AssignJob(BasicJob newjob)
	{
		if (newjob == null)
			return;

		// reset values
		StopCoroutine(activeCo);
		inAction = false;

		// manually set state
		state = ScoutState.Working;

		ActiveJob = newjob;
		SetDestination(ActiveJob.JobPoint.position);//, ActiveJob.GetComponent<Collider>());
	}
	public void MoveTo(Vector3 point)
	{
		// reset values
		StopCoroutine(activeCo);
		inAction = false;
		ActiveJob = null;

		// manually set state
		state = ScoutState.Idle;

		SetDestination(point);
	}
	#endregion
	
	// Update is called once per frame
	void Update()
    {
		// keep stats capped
		Focus = Mathf.Min(Focus, maxStat);
		Fun = Mathf.Min(Fun, maxStat);

		if (inAction) // if they are currently doing something, keep doing it
			return;

		if (!path.reachedDestination)
			return;

		switch (state)
		{
			case ScoutState.Idle:
				activeCo = StartCoroutine(DoIdle());
				break;
			case ScoutState.Working:
				activeCo = StartCoroutine(DoWork());
				break;
			case ScoutState.Socializing:
				break;
		}
	}

	void StateCheck()
	{
		float rando = Random.Range(1, 101);

		Debug.Log(gameObject + " rolled " + rando + " on state change");
		switch (state)
		{
			case ScoutState.Idle:
				if (rando < 60)
					startIdle();
				else if (rando < 60 + Focus)
					startWork();
				else
					startSocial();
				break;
			case ScoutState.Working:
				if (rando < 9 * Focus)
					startWork();
				else if (rando > 90)
					startIdle();
				else
					startSocial();
				break;
			case ScoutState.Socializing:
				if (rando < 3 * Focus)
					startWork();
				else if (rando > 85)
					startIdle();
				else
					startSocial();
				break;
		}
	}

	void startIdle()
	{
		Debug.Log(scoutName + "("+ gameObject+") is now idle");

		GraphNode startOfPath, endOfPath;
		int redun = 0;
		do
		{
			path.destination = transform.position + new Vector3(Random.Range(-IdleDist, IdleDist), 0, Random.Range(-IdleDist, IdleDist));

			// make sure path is legal
			startOfPath = AstarPath.active.GetNearest(transform.position).node;
			endOfPath = AstarPath.active.GetNearest(path.destination).node;
		}
		while (PathUtilities.IsPathPossible(startOfPath, endOfPath) == false && ++redun < 100);
		state = ScoutState.Idle;
	}
	void startSocial()
	{
		Debug.Log(scoutName + "(" + gameObject + ") is now social");

		startIdle();
		//state = ScoutState.Socializing;
	}
	void startWork()
	{
		Debug.Log(scoutName + "(" + gameObject + ") is now working");

		state = ScoutState.Working;
		ActiveJob = SC.AssignJob();
		path.destination = ActiveJob.JobPoint.position;
	}
}