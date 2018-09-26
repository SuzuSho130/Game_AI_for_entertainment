using UnityEditor;
using UnityEngine;
using System.Media;

[InitializeOnLoad]
class Initialized1
{
	private static SoundPlayer player;
	private static bool flag = true;
	static Initialized1 ()
	{
		player = new SoundPlayer ("Assets/Editor/Unity.wav");
		//EditorApplication.update += Update;
	}

	static void Update ()
	{
		if (EditorApplication.isCompiling) {
			if (flag) {
				player.PlaySync ();
				flag = false;
			}
		} else {
			flag = true;
		}
	}
}