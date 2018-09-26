using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour {

	public int damage;
	public string colliderName;
	Collider2D coll2D;
	public bool bulletOn = false;
	public GameObject BulletPrefabs;

	void Start() {
		coll2D = GetComponent<Collider2D> ();
	}
	void OnTriggerEnter2D(Collider2D other) {
		OnAttackTermination ();
		other.SendMessage ("Damage", damage);
	}

	public void OnAttack(bool direction) {
		coll2D.enabled = true;
		if (bulletOn) {
			GameObject bullet = Instantiate (BulletPrefabs, transform.position, Quaternion.identity);
			if (direction) {
				bullet.GetComponent<BulletController> ().SetSpeed (3.0f);
			} else {
//				Vector3 theScale = bullet.transform.localScale;
//				theScale.x *= -1;
//				bullet.transform.localScale = theScale;
				bullet.GetComponent<BulletController> ().SetSpeed (-3.0f);
			}
		}
	}

	public void OnAttackTermination() {
		coll2D.enabled = false;
	}
}
