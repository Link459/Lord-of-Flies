namespace Lord_of_Flies 
{
	public class LordOfFlies : Mod
    {
        public static LordOfFlies instance;
		new public string GetName() => "Lord of Flies";
        public override string GetVersion() => "1.0.0.0";

        public override void Initialize()
        {
            ModHooks.AfterSavegameLoadHook += this.SaveGame;
            ModHooks.NewGameHook += AddComponent;
            ModHooks.AfterTakeDamageHook += AddHealth;
            ModHooks.LanguageGetHook += OnLangGet;
            instance = this;
        }

        public Dictionary<string, Dictionary<string, GameObject>> assetsByScene = new()
         {
            ["GG_Hollow_Knight"] = new()
            {
                ["Shot HK Shadow"] = null,
            },
         };

         private Func<IEnumerator> SaveAssetsFromScene(string scenename)
         {
             IEnumerator SaveAssets()
             {
                 Dictionary<string, GameObject> assets = assetsByScene[scenename];
                 foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
                 {
                     if (assets.ContainsKey(go.name))
                     {
                         Lord lord = new Lord();
                         lord.AncientSword = go;
                         GameObject.DontDestroyOnLoad(lord.AncientSword);
                         lord.AncientSword.SetActive(false);
                         lord.AncientSword.name = go.name;
                         assets[lord.AncientSword.name] = lord.AncientSword;
                     }
                 }
                 yield break;
             }
             return SaveAssets;
         }
        private void SaveGame(SaveGameData data){AddComponent();}
        private void AddComponent(){GameManager.instance.gameObject.AddComponent<LordFinder>();}
        private int AddHealth(int hazardType, int damageAmount)
        {
            if(GameManager.instance.sceneName == "GG_Sly")
            {
                 GameObject.Find("Sly Boss").GetComponent<HealthManager>().hp += 100;
                 return damageAmount*2;}
            else
            {return damageAmount;} 
        }
        private string OnLangGet(string key, string sheettitle, string orig)
        {
            return key switch
            {
                "NAME_SLY" => "Lord of Flies",
                "GG_S_SLY" => "Prepare to die",
                "SLY_BOSS_MAIN" => "Flies",
                "SLY_BOSS_SUPER" => "Lord of",
                _ => orig
            };
        }
	}
}