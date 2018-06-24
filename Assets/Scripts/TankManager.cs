using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SortType{normal, vertical, horizontal};

public class TankManager : MonoBehaviour {

	//public bool setted = false;

	public SortType sortType = SortType.normal;

	public GameObject selector;

	// public float horizontalRadius;

	// public float verticalRadius;

	// public float verticalDistance;

	// public float maxDegree;

	List<GameObject> childList;

	Vector3 targetPos;

	// Use this for initialization
	void Start () {
		// targetPos  = new Vector3();
		// childList = new List<GameObject>();
		// foreach(Transform child in gameObject.transform){
		// 	childList.Add(child.gameObject);
		// }
	}

	public void initTankManager(){
		targetPos  = new Vector3();
		childList = new List<GameObject>();
		foreach(Transform child in gameObject.transform){
			child.GetComponent<TankBehavior>().initTank();
			childList.Add(child.gameObject);
		}
	}

	public void SortTank(SortType st){
		switch(st){
			case SortType.normal:
				break;
			// case SortType.vertical:
			// 	for(int i = 0; i < childList.Count; i++){
			// 		targetPos = 
			// 		new Vector3(Mathf.Cos(Mathf.Deg2Rad * maxDegree / childList.Count * i + (180 - maxDegree) / 2) * horizontalRadius, 
			// 					0, 
			// 					-Mathf.Sin(Mathf.Deg2Rad * maxDegree / childList.Count * i + (180 - maxDegree) / 2) * horizontalRadius);
			// 		//Debug.Log(targetPos);
			// 		childList[i].transform.localPosition = targetPos;
			// 	}
			// 	transform.forward = cam.transform.forward;
			// 	break;
			// case SortType.horizontal:
			// 	//Debug.Log(childList.Count);
			// 	for(int i = 0; i < childList.Count; i++){
			// 		targetPos = 
			// 		new Vector3(Mathf.Cos(Mathf.Deg2Rad * 360 / childList.Count * i ) * verticalRadius, 
			// 					0, 
			// 					-Mathf.Sin(Mathf.Deg2Rad * 360 / childList.Count * i ) * verticalRadius);
			// 		//Debug.Log(targetPos);
			// 		childList[i].transform.localPosition = targetPos;
			// 	}
			// 	transform.up = -cam.transform.forward;
			// 	transform.position -= transform.up * verticalDistance;
			// 	break;
			default:
				break;
		}
	}


}
