using UnityEngine;

public class MicroMapLocationUI : MonoBehaviour
{
	public enum State
	{
		todo,
		current,
		cleared
	}

	public SpriteRenderer spriteRenderer;

	public GameObject currentHighlight;

	private MicroMapLocation microMapLocation;

	private State state;

	public MicroMapConnection LeftConnection { get; set; }

	public MicroMapConnection RightConnection { get; set; }

	public int RoomIndex { get; private set; }

	public State CurrentState
	{
		get
		{
			return state;
		}
		set
		{
			if (value == State.todo || value == State.current)
			{
				spriteRenderer.sprite = microMapLocation.activeSprite;
			}
			else
			{
				spriteRenderer.sprite = microMapLocation.inactiveSprite;
			}
			currentHighlight.SetActive(value == State.current);
			state = value;
		}
	}

	public void Initialize(MicroMapLocation microMapLocation, int roomIndex)
	{
		this.microMapLocation = microMapLocation;
		RoomIndex = roomIndex;
		CurrentState = State.todo;
	}
}
