using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicJob : MonoBehaviour
{
	public Transform JobPoint;

	public enum JobID
	{
		Tent = 0,
		Fishing = 1,
		Wood = 2,
		Fire = 3,
		Cooking = 4
	}
	public JobID ID;
	
	public bool Completed = false;
	[SerializeField] float TotalProgress;
	[SerializeField] float Progress;

	enum StageMethod
	{
		Replace,
		Add
	}
	[Header("Scout Stat Increments"), SerializeField]
	float FunInc;
	[SerializeField] float FocusInc;

	[Header("Visual Indicators"), SerializeField]
	StageMethod VisualMethod = StageMethod.Add;
	[SerializeField] GameObject[] stages;
	int activestage = 0;
	
	[Header("UI")]
	public bool ActivelyWorkedOn = false;
	[SerializeField] GameObject SceneCanvas;
	[SerializeField] Image dial;

	private void Start()
	{
		foreach (GameObject stage in stages)
			stage.SetActive(false);
		stages[activestage].SetActive(true);
	}

	private void Update()
	{
		float fillratio = Progress / TotalProgress;
		SceneCanvas.SetActive(ActivelyWorkedOn);
		SceneCanvas.transform.LookAt(Camera.main.transform);

		dial.fillAmount = Mathf.Lerp(dial.fillAmount, fillratio, Time.deltaTime * 6);

	}

	public void IncrementProgress()
	{
		if (Progress >= TotalProgress)
			return;

		Progress++;

		UpdateVisual();
		if (Progress >= TotalProgress)
		{
			Completed = true;
		}
	}
	public void UpdateScout(ScoutBrain scout)
	{
		scout.Fun += FunInc;
		scout.Focus += FocusInc;
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
