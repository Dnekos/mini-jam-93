using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
	[SerializeField]
	GameObject Blackout;

	[Header("Timing"), SerializeField] 
	float MinutesTillEnd = 1;
	float timeElapsed;
	[SerializeField] SocialCoordinator SC;

	[Header("Visualizers"), SerializeField] Transform Arrow;
	[SerializeField] Vector2 RotBounds;
	[SerializeField] Transform DirLight;
	Vector3 originalDirLightRot;

    // Start is called before the first frame update
    void Start()
    {
		originalDirLightRot = DirLight.eulerAngles;
		Arrow.eulerAngles = Vector3.forward * RotBounds.x; 
    }

    // Update is called once per frame
    void Update()
    {
		timeElapsed += Time.deltaTime;
		float ratio = timeElapsed / (MinutesTillEnd * 60);

		Arrow.eulerAngles = Vector3.forward * Mathf.Lerp(RotBounds.y, RotBounds.x, ratio);
		DirLight.eulerAngles = originalDirLightRot + Vector3.left * Mathf.Lerp(-originalDirLightRot.x, 0, ratio);

		if (timeElapsed >= (MinutesTillEnd * 60))
		{
			SC.PrepTransport();
			Instantiate(Blackout).GetComponentInChildren<BlackoutScript>().index = SceneManager.GetActiveScene().buildIndex+1;
			timeElapsed = Mathf.NegativeInfinity; // prevent too many blackouts existing at once    
		}
	}
}
