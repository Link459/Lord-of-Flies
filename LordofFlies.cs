namespace Lord_of_Flies 
{
	public class Lord_of_Flies : Mod
	{
		new public string GetName() => "Lord of Flies";
        public override string GetVersion() => "1.0.0.0";
         public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) 
        {
               var Shot = preloadedObjects["sharedassets449"]["Shot HK Shadow(Clone)"];
            ModHooks.AfterSavegameLoadHook += this.SaveGame;
            ModHooks.NewGameHook += AddComponent;
            ModHooks.AfterTakeDamageHook += AddHealth;
            ModHooks.LanguageGetHook += OnLangGet;
        }

        /*public override List<(string, string)> GetPreloadNames()
    {
       return new List<(string, string)>
       {
        ("sharedassets447","Shot HK Shadow(Clone)")
       };
    }*/
     public static GameObject AncientSword { get; set; }
     //public static void Preloaded(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects){
       // AncientSword = UnityEngine.Object.Instantiate(preloadedObjects["sharedassets447"]["Shot HK Shadow(Clone)"]);
     //}

        //occurs in case we aren't starting a new game.
        private void SaveGame(SaveGameData data){AddComponent();}
        private void AddComponent(){GameManager.instance.gameObject.AddComponent<LordFinder>();}
        private int AddHealth(int hazardType, int damageAmount){
            if(GameManager.instance.sceneName == "GG_Sly")
            {
                 GameObject.Find("Sly Boss").GetComponent<HealthManager>().hp += 100;
                 return damageAmount*2;}
            else
            {return damageAmount;}
            }
        private string OnLangGet(string key, string sheettitle, string orig)
        {
            switch (key)
            {
                case "NAME_SLY":
                    return "Lord of Flies";
                case "GG_S_SLY":
                    return "Prepare to die";
                case "SLY_BOSS_MAIN":
                    return "Flies";
                case "SLY_BOSS_SUPER":
                    return "Lord of";
                default:
                    return orig;
            }
        }
	}
}