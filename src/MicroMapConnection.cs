using UnityEngine;

public class MicroMapConnection : MonoBehaviour
{
	public enum State
	{
		todo,
		current,
		cleared
	}

	public SpriteRenderer spriteRenderer;

	public Sprite todoSprite;

	public Sprite inTransitionSprite;

	public Sprite clearedSprite;

	private State state;

	public State CurrentState
	{
		get
		{
			return state;
		}
		set
		{
			switch (value)
			{
			case State.todo:
				spriteRenderer.sprite = todoSprite;
				break;
			case State.current:
				spriteRenderer.sprite = inTransitionSprite;
				break;
			case State.cleared:
				spriteRenderer.sprite = clearedSprite;
				break;
			}
			state = value;
		}
	}
}
