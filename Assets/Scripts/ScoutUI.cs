using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ScoutUI : MonoBehaviour, IDragHandler,IBeginDragHandler
{
	public ScoutBrain scout;
	[Header("UI Components"), SerializeField]
	TextMeshProUGUI Nametxt;
	[SerializeField] Transform EnergyBar,FunBar;
	[SerializeField] TextMeshProUGUI[] Friends, ThoughtPanel;

	Vector2 DragOffset;

	// Start is called before the first frame update
	void Start()
    {
		Nametxt.text = scout.scoutName;
		Friends[0].text = scout.Friends[0].GetComponent<ScoutBrain>().scoutName;
		Friends[1].text = scout.Friends[1].GetComponent<ScoutBrain>().scoutName;

	}

	// Update is called once per frame
	void Update()
    {
		EnergyBar.localScale = new Vector3(scout.Focus / scout.maxStat, 1, 1);
		FunBar.localScale = new Vector3(scout.Fun / scout.maxStat, 1, 1);
		for (int i = 0; i < scout.thoughts.Count; i++)
		{
			ThoughtPanel[i].text = scout.scoutName + scout.thoughts[i];
		}
	}

	public void ExitUI()
	{
		Destroy(gameObject);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//DragOffset = transform.position - eventData.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position += (Vector3)eventData.delta;
	}
}
