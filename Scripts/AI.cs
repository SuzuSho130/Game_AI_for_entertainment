using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	public GameObject Player1;
	PlayerController _player_controller;
	private EnemyController _enemy_controller;
	private float dis=0.0f;
	private int dir=1;
	private float threshold = 0.1f; //技判定

	public int distance_threshold; //距離差
	public int hp_threshold;
	public int[] short_dis_judge = new int[6]; //各技判定
	public int[] long_dis_judge = new int[6]; //各技判定
	public int[] short_dis_state = new int[6]; //敵ステート
	public int[] long_dis_state = new int[6]; //敵ステート

	public int miss_distance; //距離誤差
	public int[] short_dis_restriction_judge = new int[6]; //行動制限
	public int[] long_dis_restriction_judge = new int[6]; //行動制限
	public int operation_mistake; //操作ミス

	// Use this for initialization
	void Start () {
		_player_controller = Player1.GetComponent<PlayerController> ();
		_enemy_controller = GetComponent<EnemyController> ();
		dis = Mathf.Abs (Player1.transform.position.x - transform.position.x);
//		SetInitialValue ();
	}

	private int SetInitialValue() {
		for(int i=0;i<6;i++) {
			short_dis_judge [i] = Random.Range (0, 100);
			long_dis_judge [i] = Random.Range (0, 100);
			//			Debug.Log ("Judge" + i + ": " + judge [i]);
		}
		for(int i=0;i<6;i++) {
			short_dis_state [i] = Random.Range (0, 100);
			long_dis_state [i] = Random.Range (0, 100);
			//			Debug.Log ("State" + i + ": " + state [i]);
		}
		return 0;
	}

	public void SetStatusValue(int[] para) {
		distance_threshold = para [0];
		hp_threshold = para [1];
		for (int i = 0; i < 6; i++) {
			short_dis_judge [i] = para [i + 2];
		}
		for (int i = 0; i < 6; i++) {
			long_dis_judge [i] = para [i + 8];
		}
		for (int i = 0; i < 6; i++) {
			short_dis_judge [i] = para [i + 14];
		}
		for (int i = 0; i < 6; i++) {
			short_dis_judge [i] = para [i + 20];
		}
		miss_distance = para [26];
		for (int i = 0; i < 6; i++) {
			short_dis_judge [i] = para [i + 27];
		}
		for (int i = 0; i < 6; i++) {
			short_dis_judge [i] = para [i +33];
		}
		operation_mistake = para [39];
	}

	public int Select_Action() {
		dis = Mathf.Abs (Player1.transform.position.x - transform.position.x);
		if (Fork_Distance()) { //近距離
			return Short_Distance_Action();
		} else { //遠距離
			return Long_Distance_Action();
		}
	}

	private int Move() {
		int difference_hp = _enemy_controller.GetHP() - _player_controller.GetHP ();
		if (difference_hp >= hp_threshold) {	//Unfavorable
			if (Player1.transform.position.x - transform.position.x > 0) {
				return 4;
			} else {
				return 5;
			}
		} else {
			if (Player1.transform.position.x - transform.position.x > 0) {
				return 5;
			} else {
				return 4;
			}
		}
	}
		

	private int Short_Distance_Action() {
		float check;
		for(int i=0;i<6;i++) {
			check = Mathf.Abs (dis * dir + short_dis_state [GetState()] / 100) / 2.0f - short_dis_judge [i] / 100.0f;
			//			Debug.Log(Mathf.Abs (dis * dir + state [GetState()] / 100) / 2.0f - judge [i] / 100.0f);
			if (threshold >= Mathf.Abs (check)) {
				if (Player1.transform.position.x - transform.position.x > 0 && !_enemy_controller.GetDirect()) {
					return 4;
				} else if(Player1.transform.position.x - transform.position.x < 0 && _enemy_controller.GetDirect())  {
					return 5;
				}
				return i;
			}
		}
		return Move ();
	}

	private int Long_Distance_Action() {
		float check;
		for(int i=0;i<6;i++) {
			check = Mathf.Abs (dis * dir + long_dis_state [GetState()] / 100) / 2.0f - long_dis_judge [i] / 100.0f;
			//			Debug.Log(Mathf.Abs (dis * dir + state [GetState()] / 100) / 2.0f - judge [i] / 100.0f);
			if (threshold >= Mathf.Abs (check)) {
				if (Player1.transform.position.x - transform.position.x > 0 && !_enemy_controller.GetDirect()) {
					return 4;
				} else if(Player1.transform.position.x - transform.position.x < 0 && _enemy_controller.GetDirect())  {
					return 5;
				}
				return i;
			}
		}
		return Move ();
	}

	int GetState() {
		return _player_controller.current_state;
	}

	private bool Fork_Distance() {
		bool fork;
		fork = dis < distance_threshold / 50.0f;
		return fork;
	}

	private int Fork_Action() {
		float check;
		for(int i=0;i<6;i++) {
			check = Mathf.Abs (dis * dir + short_dis_state [GetState()] / 100) / 2.0f - short_dis_judge [i] / 100.0f;
//			Debug.Log(Mathf.Abs (dis * dir + state [GetState()] / 100) / 2.0f - judge [i] / 100.0f);
			if (threshold >= Mathf.Abs (check)) {
				if (Player1.transform.position.x - transform.position.x > 0 && !_enemy_controller.GetDirect()) {
					return 4;
				} else if(Player1.transform.position.x - transform.position.x < 0 && _enemy_controller.GetDirect())  {
					return 5;
				}
				return i;
			}
		}
		return Move ();
	}
}
