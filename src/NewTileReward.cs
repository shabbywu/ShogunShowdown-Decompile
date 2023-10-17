using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TileEnums;
using UINavigation;
using UnityEngine;
using UnlocksID;

public class NewTileReward : Reward, INavigationGroup
{
	private NewTilePedestal[] newTilePedestals;

	private bool busy;

	private bool firstOnEntry = true;

	public override bool Rerollable => true;

	private int NPedistals => newTilePedestals.Length;

	public NewTilePedestal[] NewTilePedestals => newTilePedestals;

	private int RandomTileLevel => Random.Range(2, 5);

	private string OptionsDescriptionForMetrics
	{
		get
		{
			string text = "";
			NewTilePedestal[] array = newTilePedestals;
			foreach (NewTilePedestal newTilePedestal in array)
			{
				text = text + newTilePedestal.Tile.Attack.TechName + "/";
			}
			return text.TrimEnd('/');
		}
	}

	public List<INavigationTarget> Targets => ((IEnumerable<INavigationTarget>)NewTilePedestals).ToList();

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	private void Update()
	{
		if (newTilePedestals != null && busy)
		{
			bool flag = false;
			NewTilePedestal[] array = newTilePedestals;
			foreach (NewTilePedestal newTilePedestal in array)
			{
				flag = flag || newTilePedestal.Busy;
			}
			if (!flag)
			{
				busy = false;
				EventsManager.Instance.RewardReady.Invoke();
			}
		}
	}

	public override void Initialize()
	{
		((Component)this).gameObject.SetActive(true);
		newTilePedestals = ((Component)this).GetComponentsInChildren<NewTilePedestal>();
		for (int i = 0; i < NPedistals; i++)
		{
			newTilePedestals[i].Initialize(this);
		}
	}

	public override void Skipped()
	{
		base.Skipped();
		EventsManager.Instance.PlayerChoiceInNewTileReward.Invoke($"{OptionsDescriptionForMetrics} Payed:{base.IsPayedReward} Skipped");
	}

	public override void StartEvent(RewardSaveData rewardSaveData)
	{
		base.InProgress = true;
		busy = true;
		EventsManager.Instance.RewardBusy.Invoke();
		TilesManager.Instance.CanInteractWithTiles = false;
		if (rewardSaveData != null && rewardSaveData.inProgress)
		{
			for (int i = 0; i < NPedistals; i++)
			{
				newTilePedestals[i].StartEvent(TilesFactory.Instance.Create(rewardSaveData.tilesRewards[i]));
			}
		}
		else
		{
			AttackEnum[] pseudoRandomAttackEnums = GetPseudoRandomAttackEnums();
			for (int j = 0; j < NPedistals; j++)
			{
				newTilePedestals[j].StartEvent(TilesFactory.Instance.Create(pseudoRandomAttackEnums[j], RandomTileLevel));
			}
		}
		EffectsManager.Instance.ScreenShake();
	}

	public override void Reroll()
	{
		busy = true;
		EventsManager.Instance.PlayerChoiceInNewTileReward.Invoke($"{OptionsDescriptionForMetrics} Payed:{base.IsPayedReward} Rerolled");
		EventsManager.Instance.RewardBusy.Invoke();
		AttackEnum[] pseudoRandomAttackEnums = GetPseudoRandomAttackEnums();
		for (int i = 0; i < NPedistals; i++)
		{
			newTilePedestals[i].Reroll(TilesFactory.Instance.Create(pseudoRandomAttackEnums[i], RandomTileLevel));
		}
		EffectsManager.Instance.ScreenShake();
	}

	private AttackEnum[] GetPseudoRandomAttackEnums()
	{
		int num = NPedistals;
		List<AttackEnum> list = new List<AttackEnum>();
		if (UnlocksManager.Instance.RecentlyUnlockedTilesID.Count > 0 && num > 0)
		{
			UnlockID unlockID = UnlocksManager.Instance.RecentlyUnlockedTilesID[0];
			UnlocksManager.Instance.RecentlyUnlockedTilesID.Remove(unlockID);
			UnlocksManager.Instance.PopulateSaveData(SaveDataManager.Instance.saveData);
			SaveDataManager.Instance.StoreSaveData();
			if (ID.tilesID.ContainsKey(unlockID))
			{
				list.Add(ID.tilesID[unlockID]);
				num--;
			}
			else
			{
				Debug.LogWarning((object)$"RecentlyUnlockedTilesID contains an ID that is not available in this version of the game: {unlockID}.");
			}
		}
		AttackEnum[] nextN;
		do
		{
			nextN = TilesFactory.Instance.PseudoRandomAttackEnumsGenerator.GetNextN(num);
		}
		while (list.Intersect(nextN).Count() > 0 || TilesManager.Instance.DeckAttackEnums.Intersect(nextN).Count() == NPedistals);
		list.AddRange(nextN);
		return list.ToArray();
	}

	public override void EndEvent()
	{
		NewTilePedestal[] array = newTilePedestals;
		foreach (NewTilePedestal newTilePedestal in array)
		{
			if (newTilePedestal.HasTile)
			{
				newTilePedestal.DisappearTile();
			}
		}
		newTilePedestals = null;
		base.InProgress = false;
		busy = false;
	}

	public void UpgradePicked(NewTilePedestal picked)
	{
		EventsManager.Instance.NewTilePicked.Invoke(picked.Tile);
		EventsManager.Instance.PlayerChoiceInNewTileReward.Invoke($"{OptionsDescriptionForMetrics} Payed:{base.IsPayedReward} Picked:{picked.Tile.Attack.TechName}");
		NewTilePedestal[] array = newTilePedestals;
		foreach (NewTilePedestal newTilePedestal in array)
		{
			if ((Object)(object)newTilePedestal != (Object)(object)picked)
			{
				newTilePedestal.DisappearTile();
			}
		}
		EventsManager.Instance.RewardsScreenUpgradeChoosen.Invoke();
		((MonoBehaviour)this).StartCoroutine(WaitAndEndEvent(0.9f));
	}

	public void UpgradeClaimed()
	{
		base.Exausted = true;
	}

	private IEnumerator WaitAndEndEvent(float t)
	{
		yield return (object)new WaitForSeconds(t);
		EndEvent();
	}

	public override void PopulateSaveData(RewardSaveData rewardSaveData)
	{
		rewardSaveData.inProgress = base.InProgress;
		rewardSaveData.rewardExausted = base.Exausted;
		if (base.InProgress)
		{
			rewardSaveData.tilesRewards = new List<TileSaveData>();
			NewTilePedestal[] array = newTilePedestals;
			foreach (NewTilePedestal newTilePedestal in array)
			{
				rewardSaveData.tilesRewards.Add(newTilePedestal.Tile.GetTileSaveData());
			}
		}
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (navigationDirection == NavigationDirection.up && SelectedTarget == null)
		{
			UINavigationHelper.SelectNewTarget(this, NewTilePedestals[0]);
		}
		else
		{
			switch (navigationDirection)
			{
			case NavigationDirection.left:
				UINavigationHelper.SelectNewTarget(this, NewTilePedestals[0]);
				break;
			case NavigationDirection.right:
				UINavigationHelper.SelectNewTarget(this, NewTilePedestals[1]);
				break;
			default:
				return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
			}
		}
		return this;
	}

	public void OnEntry(NavigationDirection navigationDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		if (firstOnEntry || (Object)((previousTarget is Object) ? previousTarget : null) == (Object)null)
		{
			firstOnEntry = false;
			UINavigationHelper.SelectNewTarget(this, NewTilePedestals[0]);
		}
		else
		{
			INavigationTarget newTarget = UINavigationHelper.FindClosetsTarget(previousTarget, Targets);
			UINavigationHelper.SelectNewTarget(this, newTarget);
		}
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}

	public INavigationGroup NavigatePrev()
	{
		return Navigate(NavigationDirection.left);
	}

	public INavigationGroup NavigateNext()
	{
		return Navigate(NavigationDirection.right);
	}
}
