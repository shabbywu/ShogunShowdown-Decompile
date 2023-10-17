using System;
using PickupEnums;
using UnityEngine;

[Serializable]
public class PickupDrop
{
	public string name;

	public PickupEnum pickupEnum;

	[Range(0f, 1f)]
	public float probability;
}
