public interface ISavable
{
	void PopulateSaveData(SaveData saveData);

	void LoadFromSaveData(SaveData saveData);
}
