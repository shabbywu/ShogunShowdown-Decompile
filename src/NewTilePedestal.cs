using System;
using System.Collections;
using InfoBoxUtils;
using Parameters;
using TilesUtils;
using UINavigation;
using UnityEngine;

public class NewTilePedestal : MonoBehaviour, IInfoBoxable, INavigationTarget
{
	private Animator animator;

	[SerializeField]
	private InputActionButtonBinder inputActionButtonBinder;

	public Transform headTransform;

	public InfoBoxActivator infoBoxActivator;

	private MyButton button;

	private SimpleTileContainer cc;

	private Tile tile;

	private NewTileReward newTileRewardThisBelongsTo;

	private readonly float verticalOscillationAmplitude = TechParams.pixelSize;

	private readonly float verticalOscillationOmega = MathF.PI;

	private readonly float headLocalY = 1.9375f;

	public string InfoBoxText { get; private set; }

	public bool InfoBoxEnabled => true;

	public BoxWidth BoxWidth => BoxWidth.medium;

	public bool HasTile => cc.HasTile;

	public Tile Tile => tile;

	public bool Busy { get; private set; }

	public Transform Transform => ((Component)cc).transform;

	private void Update()
	{
		VerticalHeadOscillation();
	}

	public void Initialize(NewTileReward newTileReward)
	{
		newTileRewardThisBelongsTo = newTileReward;
		cc = ((Component)this).GetComponentInChildren<SimpleTileContainer>();
		animator = ((Component)this).GetComponent<Animator>();
		button = ((Component)this).GetComponentInChildren<MyButton>();
		button.Disappear();
	}

	public void StartEvent(Tile tile)
	{
		this.tile = tile;
		Busy = true;
		button.Interactable = false;
		button.Appear();
		SoundEffectsManager.Instance.Play("Spawn");
		animator.SetBool("HasTile", true);
		cc.AddTile(tile);
		cc.TeleportTileInContainer();
		cc.Interactable = false;
		tile.Interactable = false;
		tile.InfoBoxActivator.ColliderEnabled = false;
		tile.Graphics.ShowLevel(value: true);
		InfoBoxText = tile.InfoBoxText;
	}

	public void OnButtonClick()
	{
		button.Disappear();
		((Component)inputActionButtonBinder).gameObject.SetActive(false);
		TakeTile();
	}

	public void Ready()
	{
		button.Interactable = true;
		Busy = false;
	}

	public void OnPointerEnter()
	{
		infoBoxActivator.Open();
		animator.SetBool("Highlight", true);
		Globals.Hero.LookAt(((Component)this).transform);
	}

	public void OnPointerExit()
	{
		infoBoxActivator.Close();
		animator.SetBool("Highlight", false);
	}

	private void TakeTile()
	{
		newTileRewardThisBelongsTo.UpgradePicked(this);
		SoundEffectsManager.Instance.Play("NewTilePicked");
		animator.SetTrigger("Picked");
		tile.InfoBoxActivator.ColliderEnabled = true;
		infoBoxActivator.Close();
	}

	public void Reroll(Tile tile)
	{
		((MonoBehaviour)this).StartCoroutine(RerollCoroutine(tile));
	}

	private IEnumerator RerollCoroutine(Tile tile)
	{
		((Component)tile).gameObject.SetActive(false);
		Busy = true;
		DisappearTile();
		while (cc.HasTile)
		{
			yield return null;
		}
		yield return null;
		((Component)tile).gameObject.SetActive(true);
		StartEvent(tile);
		yield return (object)new WaitForSeconds(0.5f);
	}

	public void DisappearTile()
	{
		animator.SetBool("HasTile", false);
		button.Disappear();
	}

	private void VerticalHeadOscillation()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		headTransform.localPosition = new Vector3(0f, headLocalY + verticalOscillationAmplitude * Mathf.Sin(verticalOscillationOmega * Time.time), 0f);
	}

	public void SendTileToHand()
	{
		animator.SetBool("HasTile", false);
		SoundEffectsManager.Instance.Play("TileSubmit");
		TilesManager.Instance.TakeTile(cc.RemoveTile());
		newTileRewardThisBelongsTo.UpgradeClaimed();
		EffectsManager.Instance.WaitAndCreateInGameEffect("TileShineEffect", ((Component)tile).transform, TilesParameters.translationSmoothTime + 0.3f);
	}

	public void DestroyTile()
	{
		cc.RemoveTile();
		Object.Destroy((Object)(object)((Component)tile).gameObject);
	}

	public void Select()
	{
		OnPointerEnter();
		((Component)inputActionButtonBinder).gameObject.SetActive(true);
	}

	public void Deselect()
	{
		OnPointerExit();
		((Component)inputActionButtonBinder).gameObject.SetActive(false);
	}

	public void Submit()
	{
	}
}
