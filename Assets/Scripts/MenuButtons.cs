using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButtons : MonoBehaviour
{
	[SerializeField]
	GameObject Blackout;

	[Header("LevelSelect"), SerializeField]
	int FirstLevel = 2, NumLevels = 3;
	int SelectedIndex = 0;
	[SerializeField] TextMeshProUGUI LevelNameTxt;
	[SerializeField] string[] LevelNames;

	public void ExitGame()
	{
		//AudioManager.instance.PlaySound("Click");
		Debug.Log("exit");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
	public void ChangeScene(int index)
	{
		Instantiate(Blackout).GetComponentInChildren<BlackoutScript>().index = index;
	}
	public void ChangeLevelIndex(int mod)
	{
		Debug.Log(SelectedIndex);
		SelectedIndex = ((SelectedIndex + mod) % NumLevels + NumLevels) % NumLevels; // python mod
		Debug.Log("after" + SelectedIndex);
		if (LevelNameTxt != null)
			LevelNameTxt.text = LevelNames[SelectedIndex];
	}
	public void LoadLevel()
	{
		ChangeScene(SelectedIndex + FirstLevel);
	}

}
