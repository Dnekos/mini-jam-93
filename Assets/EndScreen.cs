using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI FunText, JobText;
	
	// Start is called before the first frame update
	void Start()
    {
		EndTransport sock = FindObjectOfType<EndTransport>();
		JobText.text = sock.JobsDone + " / " + sock.NumJobs;
		FunText.text = sock.averageFun + " / 15";
		Destroy(sock.gameObject);
	}
}
