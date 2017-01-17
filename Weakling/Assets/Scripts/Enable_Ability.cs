using UnityEngine;
using System.Collections;

public class Enable_Ability : MonoBehaviour {
	public GameObject script;
	public void enabltScript(){
		script.SetActive (true);
	}
	public void disableScript(){
		script.SetActive (false);
	}
}
