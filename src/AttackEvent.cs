using System;
using UnityEngine.Events;

[Serializable]
public class AttackEvent : UnityEvent<Agent, Agent, Hit>
{
}
