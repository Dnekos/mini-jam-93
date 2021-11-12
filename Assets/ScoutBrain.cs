using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScoutBrain : PathingBrain
{
	public BasicJob ActiveJob;
	
	[Header("Individual Stats"), Tooltip("The amount of attention they give a task, decrements when doing a job, increments when idle.")]
	public float Focus;
	[Tooltip("How fast they fill a job's Progress bar")]
	public float Skill;
	[Tooltip("How happy the Scout is, determines score at the end.")]
	public float Fun;

	[Header("Jobs"),SerializeField, Tooltip("Speed is calulated by multiplier / Skill")]
	float SpeedMultiplier;
	bool isWorking = false;
	
    // Start is called before the first frame update
    override protected void Start()
	{
		base.Start();
		//AssignJob(ActiveJob);
    }

	IEnumerator DoWork()
	{
		isWorking = true;
		ActiveJob.IncrementProgress();
		Debug.Log("waiting for " + (SpeedMultiplier / Skill));

		yield return new WaitForSeconds(SpeedMultiplier / Skill);


		ScoutStatus();
	}

	public void AssignJob(BasicJob newjob)
	{
		if (newjob == null)
			return;

		ActiveJob = newjob;
		SetDestination(ActiveJob.JobPoint.position);//, ActiveJob.GetComponent<Collider>());
	}
	public void MoveTo(Vector3 point)
	{
		ActiveJob = null;
		isWorking = false;
		SetDestination(point);
	}

	void ScoutStatus()
	{
		if (ActiveJob.Completed)
		{
			ActiveJob = null;
			isWorking = false;
		}
		if (ActiveJob != null && path.reachedDestination)
			StartCoroutine(DoWork());
	}

	// Update is called once per frame
	void Update()
    {
		if (ActiveJob != null && path.reachedDestination && !isWorking)
			StartCoroutine(DoWork());

    }
}
