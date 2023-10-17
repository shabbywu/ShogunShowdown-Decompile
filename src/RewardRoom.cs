using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RewardRoom : Room
{
	public RewardRerolling rewardRerolling;

	public MyButton skipButton;

	private RewardSaveData rewardSaveData;

	[SerializeField]
	private MonochromeFrame monochromeFrame;

	public Reward Reward { get; private set; }

	public override SimpleCameraFollow.CameraMode CameraMode => SimpleCameraFollow.CameraMode.fixedPosition;

	public override void Initialize(string name, string id, bool loadRoomStateFromSaveData)
	{
		base.Initialize(name, id, loadRoomStateFromSaveData);
		Reward = ((Component)this).GetComponentInChildren<Reward>();
		Reward.Initialize();
		skipButton.Disappear();
		rewardRerolling.Hide();
	}

	public override void Begin()
	{
		if (rewardSaveData != null && rewardSaveData.rewardExausted)
		{
			End();
			return;
		}
		Globals.TilesInfoMode = true;
		CombatSceneManager.Instance.CurrentMode = CombatSceneManager.Mode.reward;
		Globals.Hero.AllowExternallyImposingFacingDir = true;
		TilesManager.Instance.ShowTilesLevel(value: true);
		skipButton.Appear();
		rewardRerolling.StartEvent(Reward.Rerollable);
		monochromeFrame.Open = true;
		Reward.StartEvent(rewardSaveData);
		((MonoBehaviour)this).StartCoroutine(WaitRewardCompletionAndEndRoom());
		EventsManager.Instance.RewardRoomBegin.Invoke();
	}

	public override void End()
	{
		Globals.Hero.AllowExternallyImposingFacingDir = false;
		if (Globals.FullInfoMode)
		{
			CombatSceneManager.Instance.DisableInfoMode();
		}
		base.End();
	}

	public override IEnumerator ProcessTurn()
	{
		yield return null;
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		EventsManager.Instance.RewardsScreenUpgradeChoosen.AddListener(new UnityAction(RewardsScreenUpgradeChoosen));
		EventsManager.Instance.RewardBusy.AddListener(new UnityAction(RewardBusy));
		EventsManager.Instance.RewardReady.AddListener(new UnityAction(RewardReady));
	}

	public IEnumerator WaitRewardCompletionAndEndRoom()
	{
		while (Reward.InProgress)
		{
			yield return null;
		}
		TilesManager.Instance.CanInteractWithTiles = false;
		yield return (object)new WaitForSeconds(0.2f);
		End();
	}

	public void RerollButtonPressed()
	{
		Reward.Reroll();
	}

	public void SkipButtonPressed()
	{
		if (Reward.InProgress)
		{
			Reward.Skipped();
			Reward.EndEvent();
			SoundEffectsManager.Instance.Play("Reroll");
			RewardsScreenUpgradeChoosen();
		}
	}

	private void RewardsScreenUpgradeChoosen()
	{
		Globals.TilesInfoMode = false;
		skipButton.Disappear();
		rewardRerolling.EndEvent();
		monochromeFrame.Open = false;
		EventsManager.Instance.SaveRunProgress.Invoke();
		EventsManager.Instance.RewardRoomEnd.Invoke();
	}

	private void RewardReady()
	{
		skipButton.Interactable = true;
		rewardRerolling.UpdateState(allowButtonInteraction: true);
		EventsManager.Instance.SaveRunProgress.Invoke();
	}

	private void RewardBusy()
	{
		skipButton.Interactable = false;
		rewardRerolling.UpdateState(allowButtonInteraction: false);
	}

	public override void PopulateSaveData(RunSaveData runSaveData)
	{
		base.PopulateSaveData(runSaveData);
		runSaveData.rewardRoom = new RewardRoomSaveData();
		runSaveData.rewardRoom.rerollPrice = rewardRerolling.RerollPrice;
		Reward.PopulateSaveData(runSaveData.rewardRoom.reward);
	}

	public override void LoadFromSaveData(RunSaveData runSaveData)
	{
		base.LoadFromSaveData(runSaveData);
		rewardRerolling.RerollPrice = runSaveData.rewardRoom.rerollPrice;
		rewardSaveData = runSaveData.rewardRoom.reward;
	}
}
