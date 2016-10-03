using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	private bool isAttacking = false;
	private float attackTimer = 0f;
	private float attackCD = 0.3f;

	public Transform attackCheck;
	public LayerMask whatCanBeAttacked;
	public int damage = 10;

	void Update()
	{
		if (!isAttacking && Input.GetButtonDown("Fire1")) {
			isAttacking = true;
			attackTimer = attackCD;

			checkAttack ();
		}
		if (isAttacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				isAttacking = false;
			}
		}
	}

	private void checkAttack()
	{
		if (Physics2D.Linecast (transform.position, attackCheck.position, whatCanBeAttacked)) {
			RaycastHit2D _enemy = Physics2D.Linecast (transform.position, attackCheck.position, whatCanBeAttacked);
			_enemy.collider.SendMessageUpwards ("dealDamage", damage);
		}
	}
}
