using System.Collections;
using System.Collections.Generic;
using ProgressionEnums;
using UnityEngine;
using UnityEngine.Events;

public abstract class Room : MonoBehaviour
{
	public MicroMapLocation microMapLocation;

	public string Name { get; set; }

	public string Id { get; set; }

	public int IndexInLocation { get; set; }

	public IslandEnum Island { get; set; }

	public CombatGrid Grid { get; set; }

	public virtual string BannerTextBegin { get; } = "";


	public virtual string BannerTextEnd { get; } = "";


	public bool RoomWasLoadedFromSaveData { get; private set; }

	public virtual SimpleCameraFollow.CameraMode CameraMode => SimpleCameraFollow.CameraMode.followHero;

	protected Environment Environment { get; set; }

	public List<Pickup> Pickups { get; private set; }

	public virtual void Initialize(string name, string id, bool loadRoomStateFromSaveData)
	{
		RoomWasLoadedFromSaveData = loadRoomStateFromSaveData;
		Grid = ((Component)this).GetComponentInChildren<CombatGrid>();
		Grid.Initialize();
		Environment = ((Component)this).GetComponentInChildren<Environment>();
		Environment.Initialize();
		Name = name;
		Id = id;
		Pickups = new List<Pickup>();
		EventsManager.Instance.PickupCreated.AddListener((UnityAction<Pickup>)OnPickupCreated);
		EventsManager.Instance.PickupPickedUp.AddListener((UnityAction<Pickup>)OnPickupPickedUp);
	}

	public abstract void Begin();

	public virtual void End()
	{
		EventsManager.Instance.PickupCreated.RemoveListener((UnityAction<Pickup>)OnPickupCreated);
		EventsManager.Instance.PickupPickedUp.RemoveListener((UnityAction<Pickup>)OnPickupPickedUp);
		CombatSceneManager.Instance.GoToNextRoom();
	}

	public abstract IEnumerator ProcessTurn();

	public IEnumerator PickUpAllPickUps()
	{
		if (Pickups.Count == 0)
		{
			yield break;
		}
		List<Pickup> list = new List<Pickup>();
		foreach (Pickup pickup in Pickups)
		{
			list.Add(pickup);
		}
		foreach (Pickup item in list)
		{
			item.ForcePickUp();
		}
		SoundEffectsManager.Instance.Play("PickupPickUp");
		yield return (object)new WaitForSeconds(0.3f);
	}

	public void AddEnemy(Enemy enemy, Cell cell)
	{
		((Component)enemy).transform.SetParent(((Component)this).transform, true);
		enemy.Cell = cell;
		enemy.SetPositionToCellPosition();
		CombatManager.Instance.Enemies.Add(enemy);
	}

	public virtual void PopulateSaveData(RunSaveData runSaveData)
	{
		foreach (Pickup pickup in Pickups)
		{
			runSaveData.pickups.Add(pickup.PickupEnum);
			runSaveData.pickupsCellIndex.Add(pickup.Cell.IndexInGrid);
		}
	}

	public virtual void LoadFromSaveData(RunSaveData runSaveData)
	{
		for (int i = 0; i < runSaveData.pickups.Count; i++)
		{
			PickupFactory.Instance.InstantiatePickup(runSaveData.pickups[i], Grid.Cells[runSaveData.pickupsCellIndex[i]], null, null, null, null, playSoundEffect: false);
		}
	}

	private void OnPickupCreated(Pickup pickup)
	{
		Pickups.Add(pickup);
	}

	private void OnPickupPickedUp(Pickup pickup)
	{
		Pickups.Remove(pickup);
	}
}
