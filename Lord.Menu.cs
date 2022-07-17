namespace Lord_of_Flies
{
 public class LordofFlies: Mod, IMenuMod
{
	bool IMenuMod.ToggleButtonInsideMenu => true;
    public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
    {
		
        return new List<IMenuMod.MenuEntry>
        {
			
            new IMenuMod.MenuEntry {
                Name = "Modify Pantheons",
                // Nothing will be displayed
                Description = null,
                Values = new string[] {
                    "Off",
                    "On"
                },
                Saver = opt => GlobalSettings.modifyPantheons = opt switch {
                    0 => false,
                    1 => true,
                    // This should never be called
                    _ => throw new InvalidOperationException()
                },
                Loader = () => GlobalSettings.modifyPantheons switch {
                    false => 0,
                    true => 1,
                }
            }
        };
    }
}
} 