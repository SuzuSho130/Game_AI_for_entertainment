using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Data {
	public int[] members;
	public int evalution;

	public Data(int[] members, int evalution) {
		this.members = members;
		this.evalution = evalution;
	}
	public int GetEvalution() {
		return this.evalution;
	}
	public void SetEvalution(int evalution) {
		this.evalution = evalution;
	}
	public int[] GetMmbers() {
		return this.members;
	}
}