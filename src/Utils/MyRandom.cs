using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils;

internal static class MyRandom
{
	private static Random _R = new Random();

	public static void Shuffle<T>(this List<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = _R.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static T NextEnum<T>()
	{
		Array values = Enum.GetValues(typeof(T));
		return (T)values.GetValue(_R.Next(values.Length));
	}

	public static T[] NextNFromArrayNoRepetition<T>(T[] input, int n, float[] probabilities = null)
	{
		int num = n * 100;
		List<T> list = new List<T>();
		for (int i = 0; i <= num; i++)
		{
			if (list.Count == n)
			{
				break;
			}
			T item = NextFromArray(input, probabilities);
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public static T NextRandomUniform<T>(T[] options)
	{
		return options[Random.Range(0, options.Length)];
	}

	public static T NextRandomUniform<T>(List<T> options)
	{
		return options[Random.Range(0, options.Count)];
	}

	public static T NextFromArray<T>(T[] input, float[] probabilities = null)
	{
		int num = input.Length;
		if (probabilities == null)
		{
			probabilities = new float[num];
			for (int i = 0; i < num; i++)
			{
				probabilities[i] = 1f;
			}
		}
		else
		{
			float[] array = probabilities;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] < 0f)
				{
					Debug.LogError((object)"A negative probability was specified in MyRandom.NextFromArray");
				}
			}
		}
		float num2 = probabilities.Sum();
		if (num2 == 0f)
		{
			Debug.LogError((object)"MyRandom.NextFromArray: the sum of all probabilities is zero...");
		}
		float num3 = Random.Range(0f, num2);
		float num4 = 0f;
		for (int k = 0; k < num; k++)
		{
			num4 += probabilities[k];
			if (num3 < num4)
			{
				return input[k];
			}
		}
		return input.Last();
	}

	public static T NextFromArray<T>(Dictionary<T, float> inputAndProbabilities)
	{
		List<T> list = new List<T>();
		List<float> list2 = new List<float>();
		foreach (KeyValuePair<T, float> inputAndProbability in inputAndProbabilities)
		{
			list.Add(inputAndProbability.Key);
			list2.Add(inputAndProbability.Value);
		}
		return NextFromArray(list.ToArray(), list2.ToArray());
	}

	public static bool Bool(float trueProbability = 0.5f)
	{
		if (trueProbability < 0f || trueProbability > 1f)
		{
			Debug.Log((object)"MyRandom.Bool: trueProbability should be between 0 and 1");
		}
		return Random.Range(0f, 1f) < trueProbability;
	}

	public static List<T> ShuffleList<T>(List<T> list)
	{
		List<T> list2 = new List<T>();
		foreach (T item in list)
		{
			list2.Add(item);
		}
		for (int i = 0; i < list2.Count; i++)
		{
			int index = Random.Range(0, list2.Count);
			T value = list2[i];
			list2[i] = list2[index];
			list2[index] = value;
		}
		return list2;
	}
}
