using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
	[SerializeField]
	GameObject Blackout;

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
}
