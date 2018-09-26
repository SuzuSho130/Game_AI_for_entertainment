using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public float life = 1.0f;
	public float speedX = 0.1f;
	public float speed = 0.1f;
	Collider2D coll2D;

	// Use this for initialization
	void Start () {
		coll2D = GetComponent<Collider2D> ();
		StartCoroutine ("Limit");
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += (new Vector3 (speedX * speed, 0.0f, 0.0f));
	}

//	void OnCollisionEnter (Collision other) {
//		if (other.gameObject.CompareTag ("Player")) {
//			StopCoroutine ("Limit");
//			Death ();
//		}
//	}

	void OnTriggerEnter2D(Collider2D other) {
		OnAttackTermination ();
		other.SendMessage ("Damage", 1);
	}

	public void SetSpeed(float x) {
		speedX = x;
	}

	IEnumerator Limit() {
		yield return new WaitForSeconds (life);
		Death ();
		yield return null;
	}

	void Death() {
		Destroy (gameObject);
	}

	public void OnAttackTermination() {
		coll2D.enabled = false;
	}
}
