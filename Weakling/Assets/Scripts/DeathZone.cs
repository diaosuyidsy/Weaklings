﻿using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	public int DamageDealt = 1000;

	public bool destroyNonePlayerObject = true;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<CharacterController2D>().dealDamage (DamageDealt);
		}else if (other.gameObject.tag == "PossessedPlayer"){
			other.SendMessageUpwards ("dealDamage", DamageDealt);
		}else if (destroyNonePlayerObject) {
			DestroyObject (other.gameObject);
		}
	}
}
