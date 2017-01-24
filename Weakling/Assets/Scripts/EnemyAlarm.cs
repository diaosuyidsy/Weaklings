using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class EnemyAlarm : MonoBehaviour {

	public float max_alarm = 100f;
	public float cur_alarm = 0f;
	float calc_alarm;

	private GameObject player;

	private float distance;

	public GameObject testPoint;
	public GameObject alarmBar;
	public GameObject Bar;

	public bool playerInSight = false;

	public BehaviorTree behaviroTree;

	// Use this for initialization
	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance (player.transform.position, testPoint.transform.position);
		if (distance < 6.6 &&distance>0.6&& cur_alarm < 100) {
			increaseAlarm ();
		} 
		else if(distance<=0.6&&distance>0&&cur_alarm<100){
			cur_alarm = 100;
			playerInSight = true;
			behaviroTree.SetVariableValue ("PIS", true);
			behaviroTree.SetVariableValue ("Target", player);
		}
		else if (cur_alarm > 50) {
			highAlarm ();
		} else if (cur_alarm <= 50 && cur_alarm > 0) {
			decreaseAlarm ();
		}

		if (cur_alarm <= 0.01) {
			playerInSight = false;
			behaviroTree.SetVariableValue ("PIS", false);
			behaviroTree.SetVariableValue ("Target", null);
		} else if (100f - cur_alarm <= 0.01f) {
			playerInSight = true;
			behaviroTree.SetVariableValue ("PIS", true);
			behaviroTree.SetVariableValue ("Target", player);
		}
	}

	void increaseAlarm(){
		cur_alarm += 5f/(Vector3.Distance(player.transform.position,testPoint.transform.position));
		calc_alarm = cur_alarm/ max_alarm;
		setAlarmBar (calc_alarm);
	}


	void highAlarm(){
		cur_alarm -= 0.3f;
		calc_alarm = cur_alarm / max_alarm;
		setAlarmBar (calc_alarm);
	}

	void decreaseAlarm(){
		cur_alarm -= 0.5f;
		calc_alarm = cur_alarm / max_alarm;
		setAlarmBar (calc_alarm);
	}

	public void setAlarmBar(float alarm){
		Bar.transform.localScale = new Vector3 (alarm, alarmBar.transform.localScale.y, alarmBar.transform.localScale.z);
	}
}
