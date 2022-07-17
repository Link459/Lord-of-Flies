namespace Lord_of_Flies 
{
  public sealed partial class LordGlobalSettings: IGlobalSettings<GlobalSettings>
{
	public static GlobalSettings GlobalSettings { get; private set; } = new();
	public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
	public GlobalSettings OnSaveGlobal() => GlobalSettings;
}

public sealed class GlobalSettings {
	public static bool modifyPantheons = false;
}
}