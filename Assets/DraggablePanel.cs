using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IDragHandler
{
	public void ExitUI()
	{
		Destroy(gameObject);
	}
	public void HideUI()
	{
		gameObject.SetActive(false);
	}


	public void OnDrag(PointerEventData eventData)
	{
		transform.position += (Vector3)eventData.delta;
	}
}
