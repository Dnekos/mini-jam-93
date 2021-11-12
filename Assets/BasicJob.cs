using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJob : MonoBehaviour
{
	public Transform JobPoint;

	public bool Completed = false;
	[SerializeField] float TotalProgress;
	[SerializeField] float Progress;
	[SerializeField] int HealthValue;
	enum StageMethod
	{
		Replace,
		Add
	}
	[Header("Visual Indicators"), SerializeField]
	StageMethod VisualMethod = StageMethod.Add;
	[SerializeField] GameObject[] stages;
	int activestage = 0;

	private void Start()
	{
		foreach (GameObject stage in stages)
			stage.SetActive(false);
		stages[activestage].SetActive(true);
	}

	public void IncrementProgress()
	{
		if (Progress >= TotalProgress)
			return;

		Progress++;

		UpdateVisual();
		if (Progress >= TotalProgress)
		{
			ScoutMaster.Health += HealthValue;
			Completed = true;
		}
	}
	void UpdateVisual()
	{
		float progRatio = Progress / TotalProgress;
		if (progRatio >=  activestage / (float)stages.Length && activestage < stages.Length-1)
		{
			if (VisualMethod == StageMethod.Replace)
				stages[activestage].SetActive(true);
			activestage++;
			stages[activestage].SetActive(true);
		}
	}
}
