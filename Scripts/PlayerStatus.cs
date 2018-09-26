using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

	public GameObject standA;
	public GameObject standB;
	public GameObject standFA;
	public GameObject standFB;
	public GameObject AirA;
	public GameObject AirB;
	public GameObject AirFA;
	public GameObject AirFB;
	public GameObject CrouchA;
	public GameObject CrouchB;
	public GameObject CrouchFA;
	public GameObject CrouchFB;
	Attack scriptStandA;
	Attack scriptStandB;
	Attack scriptStandFA;
	Attack scriptStandFB;
	Attack scriptAirA;
	Attack scriptAirB;
	Attack scriptAirFA;
	Attack scriptAirFB;
	Attack scriptCrouchA;
	Attack scriptCrouchB;
	Attack scriptCrouchFA;
	Attack scriptCrouchFB;
	public int hp;
	public Text textHP;
	int damageCount = 0;
	public int damgeCountMax = 5;
	public float damageInterval = 1.0f;
	Animator anim;

	// Use this for initialization
	void Start () {
		hp = 0;
		scriptStandA = standA.GetComponent<Attack> ();
		scriptStandB = standB.GetComponent<Attack> ();
		scriptStandFA = standFA.GetComponent<Attack> ();
		scriptStandFB = standFB.GetComponent<Attack> ();
		scriptAirA = AirA.GetComponent<Attack> ();
		scriptAirB = AirB.GetComponent<Attack> ();
		scriptAirFA = AirFA.GetComponent<Attack> ();
		scriptAirFB = AirFB.GetComponent<Attack> ();
		scriptCrouchA = CrouchA.GetComponent<Attack> ();
		scriptCrouchB = CrouchB.GetComponent<Attack> ();
		scriptCrouchFA = CrouchFA.GetComponent<Attack> ();
		scriptCrouchFB = CrouchFB.GetComponent<Attack> ();
		anim = GetComponent<Animator> ();
	}

	public void Attacked(int no, bool direcition) {
		if (no == 0) {
			scriptStandA.OnAttack (direcition);
		} else if (no == 1) {
			scriptStandB.OnAttack (direcition);
		} else if (no == 2) {
			scriptStandFA.OnAttack (direcition);
		} else if (no == 3) {
			scriptStandFB.OnAttack (direcition);
		} else if (no == 4) {
			scriptAirA.OnAttack (direcition);
		} else if (no == 5) {
			scriptAirB.OnAttack (direcition);
		} else if (no == 6) {
			scriptAirFA.OnAttack (direcition);
		} else if (no == 7) {
			scriptAirFB.OnAttack (direcition);
		} else if (no == 8) {
			scriptCrouchA.OnAttack (direcition);
		} else if (no == 9) {
			scriptCrouchB.OnAttack (direcition);
		} else if (no == 10) {
			scriptCrouchFA.OnAttack (direcition);
		} else if (no == 11) {
			scriptCrouchFB.OnAttack (direcition);
		}
	}

	public void AttackEnd(int no) {
		if (no == 0) {
			scriptStandA.OnAttackTermination ();
		} else if (no == 1) {
			scriptStandB.OnAttackTermination ();
		} else if (no == 2) {
			scriptStandFA.OnAttackTermination ();
		} else if (no == 3) {
			scriptStandFB.OnAttackTermination ();
		} else if (no == 4) {
			scriptAirA.OnAttackTermination ();
		} else if (no == 5) {
			scriptAirB.OnAttackTermination ();
		} else if (no == 6) {
			scriptAirFA.OnAttackTermination ();
		} else if (no == 7) {
			scriptAirFB.OnAttackTermination ();
		} else if (no == 8) {
			scriptCrouchA.OnAttackTermination ();
		} else if (no == 9) {
			scriptCrouchB.OnAttackTermination ();
		} else if (no == 10) {
			scriptCrouchFA.OnAttackTermination ();
		} else if (no == 11) {
			scriptCrouchFB.OnAttackTermination ();
		}
	}

	public void Damage(int damage) {
//		Debug.Log ("Damage");
		hp -= damage;
		textHP.text = hp.ToString();
		anim.SetTrigger ("DamageS");
		StartCoroutine ("DamageCount", damage);
	}

	IEnumerator DamageCount(int damage) {
		damageCount += damage;
		if (damageCount > damgeCountMax) {
			anim.SetTrigger ("DamageL");
		}
//		Debug.Log (damageCount);
		yield return new WaitForSeconds (damageInterval);
		damageCount -= damage;
		yield return null;
	}

	public int GetHP() {
		return hp;
	}
}
