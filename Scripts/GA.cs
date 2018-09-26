using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GA : MonoBehaviour {
	private const int Group = 4;
	private const int GeneLength = 30;      // 遺伝子長
	private const int Generation = 1000;
	private int Age;                        // 現在の世代
	private const double MutateRate = 0.05; // 突然変異の確率5%
	private System.Random Rnd = new System.Random();

	void Start() {
		Run ();
	}

	// 実行
	public void Run()
	{
		// 初期設定(適当な4個体を用意)
		Age = 1;
		var currentPopulation = CreateInitialData();
		foreach (Data data in currentPopulation) {
//			Debug.Log ("Ev:" + data.GetEvalution());
//			Debug.Log ("gene:" + data.GetMmbers()[20]);
		}

		while (true)
		{
			DebugState(Age, currentPopulation);

			// 選択
			var selected = Select(currentPopulation).ToList();
//
//			// 交差
			var children = CrossOver(selected[0], selected[1]).ToList();

//			// 突然変異
			var nextPopulation = selected.Concat(children).Select(individual =>
				{
					if (Rnd.NextDouble() < MutateRate)
					{
						return Mutate(individual);
					}
					return individual;
				});
//
//			// 次世代に進む
			currentPopulation = nextPopulation.ToList();

			for (int i = 0; i < Group; i++)
			{
				Debug.Log (i);
				TeatEvalution(currentPopulation[i]);
			}
			// 終了
			if (Age>Generation) break;
			Age++;
		}
		DebugState(Age, currentPopulation);
	}

	// 初期データ
	private List<Data> CreateInitialData()
	{
		return Enumerable.Range(0, Group).Select(_ =>
			{
				var array = new int[GeneLength];
				for (int i = 0; i < GeneLength; i++)
				{
					array[i] = Rnd.Next(0, 101);
				}
				var data = new Data(array, 0);
//				var data = new Data(array, Rnd.Next(0, 101));
				return data;
			}).ToList();
	}

	// 選択・淘汰
	private IEnumerable<Data> Select(List<Data> currentPopulation)
	{
		var c = new Comparison<Data> (Compare);
		currentPopulation.Sort (c);
		// 成績上位2個体を選択
		yield return currentPopulation[0];
		yield return currentPopulation[1];
	}

	// 交差
	private IEnumerable<Data> CrossOver(Data GeneA, Data GeneB)
	{
		// 個体A, Bをランダムな点で分割し A=A1B2, B=B1A2 とする
		// 分割点
		var point = Rnd.Next(1, GeneLength - 1);

		// 親を複製
		var splitedA1 = GeneA.GetMmbers().Take(point);
		var splitedA2 = GeneA.GetMmbers().Skip(point).Take(GeneLength - point);
		var splitedB1 = GeneB.GetMmbers().Take(point);
		var splitedB2 = GeneB.GetMmbers().Skip(point).Take(GeneLength - point);

		// 結合して返す
		var child_data1 = new Data(splitedA1.Concat(splitedB2).ToArray(), 0);
		var child_data2 = new Data(splitedB1.Concat(splitedA2).ToArray(), 0);
//		yield return splitedA1.Concat(splitedB2).ToArray();
//		yield return splitedB1.Concat(splitedA2).ToArray();
		yield return child_data1;
		yield return child_data2;
	}

	// 突然変異
	private Data Mutate(Data data)
	{
		// ランダム位置で1ビットのみ反転
		var point = Rnd.Next(0, GeneLength);
		if (point >= 0) data.GetMmbers()[point] = Rnd.Next(0, 101);
		return data;
	}

	// 収束評価(すべて1になっていれば終了)
	private bool IsConverged(List<bool[]> Population)
	{
		var top = Population.OrderByDescending(d => d.Count(b => b)).First();
		return top.Count(b => b) == top.Count();
	}

	private void TeatEvalution(Data data) {
		int avg = 0;
		for (int i = 0; i < GeneLength; i++)
		{
			avg += data.GetMmbers()[i];
		}
		data.SetEvalution (avg / GeneLength);
	}

	// 状態確認用
	public void DebugState(int age, List<Data> Population)
	{
		Debug.Log("世代数: " + age);
		float avg = 0;
		var c = new Comparison<Data> (Compare);
		Population.Sort (c);
		foreach (Data indivisual in Population)
		{
//			Debug.Log(indivisual.GetEvalution());
			avg += indivisual.GetEvalution();
		}
		avg /= Group;
		Debug.Log ("Avarage" + avg);
	}
	static int Compare(Data a, Data b) {
		return b.GetEvalution () - a.GetEvalution ();
	}
}