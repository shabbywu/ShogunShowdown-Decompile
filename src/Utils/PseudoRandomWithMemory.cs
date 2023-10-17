using System.Collections.Generic;
using UnityEngine;

namespace Utils;

public class PseudoRandomWithMemory<T>
{
	private bool _allowSameConsecutiveResults;

	private bool _allowSameMultipleResults;

	private T[] _choices;

	private int[] _previousPicks;

	private float[] _baseProbabilities;

	private float[] _currentProbabilities;

	private float _notPickedIncreaseFactor;

	private bool _debug;

	public PseudoRandomWithMemory((T, float)[] choicesAndBaseProbabilities, float notPickedIncreaseFactor = 2f, bool allowSameConsecutiveResults = true, bool allowSameMultipleResults = false, bool debug = false)
	{
		_debug = debug;
		int num = choicesAndBaseProbabilities.Length;
		_choices = new T[num];
		_baseProbabilities = new float[num];
		for (int i = 0; i < num; i++)
		{
			T[] choices = _choices;
			int num2 = i;
			ref float reference = ref _baseProbabilities[i];
			(T, float) tuple = choicesAndBaseProbabilities[i];
			choices[num2] = tuple.Item1;
			reference = tuple.Item2;
		}
		_currentProbabilities = new float[num];
		for (int j = 0; j < num; j++)
		{
			_currentProbabilities[j] = _baseProbabilities[j];
		}
		_notPickedIncreaseFactor = notPickedIncreaseFactor;
		_allowSameConsecutiveResults = allowSameConsecutiveResults;
		_allowSameMultipleResults = allowSameMultipleResults;
		_previousPicks = new int[0];
	}

	public T GetNext()
	{
		return GetNextN(1)[0];
	}

	public T GetDifferentNext()
	{
		bool allowSameConsecutiveResults = _allowSameConsecutiveResults;
		_allowSameConsecutiveResults = false;
		T[] nextN = GetNextN(1);
		_allowSameConsecutiveResults = allowSameConsecutiveResults;
		return nextN[0];
	}

	public T[] GetNextN(int n)
	{
		if (_debug)
		{
			Debug.Log((object)"Choices, probabilities:");
			for (int i = 0; i < _choices.Length; i++)
			{
				Debug.Log((object)$"{_choices[i]} : {_currentProbabilities[i]}");
			}
		}
		int[] array = new int[_choices.Length];
		for (int j = 0; j < _choices.Length; j++)
		{
			array[j] = j;
		}
		int num = 100;
		List<int> list = new List<int>();
		for (int k = 0; k <= num; k++)
		{
			if (list.Count == n)
			{
				break;
			}
			int num2 = MyRandom.NextFromArray(array, _currentProbabilities);
			bool flag = true;
			if (!_allowSameMultipleResults)
			{
				foreach (int item in list)
				{
					if (item == num2)
					{
						flag = false;
					}
				}
			}
			if (!_allowSameConsecutiveResults)
			{
				int[] previousPicks = _previousPicks;
				for (int l = 0; l < previousPicks.Length; l++)
				{
					if (previousPicks[l] == num2)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				list.Add(num2);
			}
		}
		for (int m = 0; m < _currentProbabilities.Length; m++)
		{
			_currentProbabilities[m] *= _notPickedIncreaseFactor;
		}
		foreach (int item2 in list)
		{
			_currentProbabilities[item2] = _baseProbabilities[item2];
		}
		_previousPicks = list.ToArray();
		T[] array2 = new T[n];
		for (int num3 = 0; num3 < n; num3++)
		{
			array2[num3] = _choices[list[num3]];
		}
		if (_debug)
		{
			Debug.Log((object)"Picked:");
			for (int num4 = 0; num4 < array2.Length; num4++)
			{
				Debug.Log((object)$"{array2[num4]}");
			}
		}
		return array2;
	}
}
