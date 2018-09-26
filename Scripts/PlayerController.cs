using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharaController {

	EvalutionAI _ai;
	public float airSpeed = 0.06f;
	public float dashSpeed = 0.1f;
	public float walkSpeed = 0.05f;
	public bool enablAction;
	Animator anim;
	CheckingAttack[] ca;
	CheckingAttackEnemy[] caE;
	PlayerStatus status;
	float horizontal;
	float vertical;
	bool direction;
	int preDirect;
	bool dash;
	float speed = 0.05f;
	bool jump;
	public int jumpMax = 40;
	bool crouch;
	bool a;
	bool b;
	bool c;
	bool d;
	private int airCount;
	private Vector3 scale;
	Collider2D coll;
	public int current_state = 0;
	// 0: stop
	// 1: move
	// 2: ground_attack1
	// 3: ground_attack2
	// 4: ground_attack3
	// 5: ground_attack4
	// 6: air_attack1;
	// 7: air_attack2;
	// 8: air_attack3;
	// 9: air_attack4;
	// 10: crouch_attack1;
	// 11; crouch_attack2;
	// 12: crouch_attack3;
	// 13: crouch_attack4;
	// 14: air
	// 15: crouch

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		ca = anim.GetBehaviours<CheckingAttack>();
		caE = anim.GetBehaviours<CheckingAttackEnemy>();
		status = GetComponent<PlayerStatus> ();
		horizontal = 0;
		vertical = 0;
		direction = true;
		dash = false;
		jump = false;
		crouch = false;
		a = false;
		b = false;
		c = false;
		d = false;
		airCount = 0;
		enablAction = true;
		scale = transform.localScale;
		foreach (CheckingAttack c in ca) {
			c.SetPlayer (true);
		}
		foreach (CheckingAttackEnemy ce in caE) {
			ce.SetPlayer (true);
		}
		coll = GetComponent<BoxCollider2D> ();
		_ai = GetComponent<EvalutionAI> ();
	}
	
	// Update is called once per frames
	void Update () {
//		GetInputPlayer1 ();
		GetSelectAction();
		if (jump) {
			JumpAction ();
		} else if (crouch) {
			CrouchAction ();
		} else {
			GroundAction ();
		}
//		Debug.Log ("current_state" + current_state);
	}


	public void OneAction() {
		//		GetInputPlayer1 ();
		GetSelectAction();
		if (jump) {
			JumpAction ();
		} else if (crouch) {
			CrouchAction ();
		} else {
			GroundAction ();
		}
	}

	//入力
	private void GetInputPlayer1() {
		if (enablAction) {
			a = Input.GetKeyDown (KeyCode.T);
			b = Input.GetKeyDown (KeyCode.Y);
			c = Input.GetKeyDown (KeyCode.U);
			d = Input.GetKeyDown (KeyCode.I);
			horizontal = Input.GetAxis ("Horizontal");
			vertical = Input.GetAxis ("Vertical");
		} else {
			if (jump) {
				horizontal = Input.GetAxis ("Horizontal");
				vertical = Input.GetAxis ("Vertical");
			}
			return;
		}
		if (vertical < 0) {
			crouch = true;
			anim.SetBool ("Crouch", true);
		} else if (vertical > 0) {
			jump = true;
			anim.SetBool ("Jump", true);
			anim.SetBool ("Crouch", false);
		} else {
			anim.SetBool ("Crouch", false);
		}
		if (horizontal < 0) {
			preDirect = -1;
			direction = false;
		} else if (horizontal > 0) {
			preDirect = 1;
			direction = true;
		} else {
			preDirect = 0;
		}
		return;
	}

	void GetSelectAction() {
		int select = _ai.Select_Action ();
		//		Debug.Log ("select" + select);
		if (enablAction) {
			if (select == 0) {
				a = true;
			} else if (select == 1) {
				b = true;
			} else if (select == 2) {
				c = true;
			} else if (select == 3) {
				d = true;
			} else if (select == 4) {
				horizontal = 1;
			} else if (select == 5) {
				horizontal = -1;
			} else {
				horizontal = 0;
			}
		} else {
			return;
		}
		if (horizontal < 0) {
			preDirect = -1;
			direction = false;
		} else if (horizontal > 0) {
			preDirect = 1;
			direction = true;
		} else {
			preDirect = 0;
		}
		return;
	}

	//Jump
	private void JumpAction() {
		speed = airSpeed;
		anim.SetBool ("Jump", true);
		StateAir ();
		transform.localPosition += (new Vector3 (horizontal*speed, ((float)jumpMax/2-(float)airCount)/300.0f, 0.0f));
		airCount++;
		if (airCount == jumpMax+1) {
			airCount = 0;
			jump = false;
			anim.SetBool ("Jump", false);
		}
	}
	//Crouch
	private void CrouchAction() {
		StateCrouch ();
		if (vertical >= 0) {
			crouch = false;
		}
	}
	//地上
	private void GroundAction() {
		if (CheckAttack()) {
			StateStand ();
		} else if(enablAction) {
			if (horizontal != 0) {
				if (direction) {
					scale.x = Mathf.Abs (scale.x);
					if (preDirect == 1) {
						dash = true;
					} else {
						dash = false;
					}
				} else {
					scale.x = -Mathf.Abs (scale.x);
					if (preDirect == -1) {
						dash = true;
					} else {
						dash = false;
					}
				}
				transform.localScale = scale;
				if (dash) {
					anim.SetBool ("Dash", true);
					speed = dashSpeed;
				} else {
					anim.SetBool ("Walk", true);
					speed = walkSpeed;
				}
				current_state = 1;
			} else {
				anim.SetBool ("Dash", false);
				anim.SetBool ("Walk", false);
				current_state = 0;
			}
		}
		transform.localPosition += (new Vector3 (horizontal*speed, 0.0f, 0.0f));
	}
	//空中時の状態
	private void StateAir() {
		if (a) {
			anim.SetTrigger ("ButtonA");
			a = false;
			current_state = 6;
		} else if (b) {
			anim.SetTrigger ("ButtonB");
			b = false;
			current_state = 7;
		} else if (c) {
			anim.SetTrigger ("BUttonFA");
			c = false;
			current_state = 8;
		} else if (d) {
			anim.SetTrigger ("ButtonFB");
			d = false;
			current_state = 9;
		} else if (enablAction) {
			current_state = 14;
		}
		return;
	}
	//地上時の状態
	private void StateStand() {
		if (a) {
			anim.SetTrigger ("ButtonA");
			a = false;
			current_state = 2;
		} else if (b) {
			anim.SetTrigger ("ButtonB");
			b = false;
			current_state = 3;
		} else if (c) {
			anim.SetTrigger ("BUttonFA");
			c = false;
			current_state = 4;
		} else if (d) {
			anim.SetTrigger ("ButtonFB");
			d = false;
			current_state = 5;
		}
		return;
	}
	//しゃがみ時の状態
	private void StateCrouch() {
		if (a) {
			anim.SetTrigger ("ButtonA");
			a = false;
			current_state = 10;
		} else if (b) {
			anim.SetTrigger ("ButtonB");
			b = false;
			current_state = 11;
		} else if (c) {
			anim.SetTrigger ("BUttonFA");
			c = false;
			current_state = 12;
		} else if (d) {
			anim.SetTrigger ("ButtonFB");
			d = false;
			current_state = 13;
		} else if (enablAction) {
			current_state = 15;
		}
		return;
	}
	private bool CheckAttack () {
		if (a || b || c || d) {
			return true;
		} else {
			return false;
		}
	}
	public IEnumerator AttackStart(int no, float startMotionTime, float motionTime) {
		horizontal = 0.0f;
		enablAction = false;
		yield return new WaitForSeconds (startMotionTime);
		status.Attacked (no, direction);
		yield return new WaitForSeconds (motionTime);
		status.AttackEnd (no);
//		enablAction = true;
		yield return null;
	}
	public void AttackEnd(int no) {
		status.AttackEnd (no);
		enablAction = true;
		StopCoroutine ("AttackStart");
	}
	public void Attackable() {
		coll.enabled = true;
	}
	public void UnAttackable() {
		coll.enabled = false;
	}

	public int GetHP() {
		return status.GetHP ();
	}

	public bool GetDirect() {
		return direction;
	}
}