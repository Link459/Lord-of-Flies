using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Lord_of_Flies 
{
	public class LordOfFlies : Mod
	{
		new public string GetName() => "Lord of Flies";
        public override string GetVersion() => "1.0.0.0";
         public override void Initialize()
         {
               ModHooks.AfterSavegameLoadHook += this.SaveGame;
               ModHooks.NewGameHook += AddComponent;
               ModHooks.AfterTakeDamageHook += AddHealth;
               ModHooks.LanguageGetHook += OnLangGet; 
         }

         public Dictionary<string, Dictionary<string, GameObject>> assetsByScene = new()
         {
            ["GG_HK_Prime"] = new()
            {
                ["ancientswordname"] = null,
            },
         };

         private Func<IEnumerator> SaveAssetsFromScene(string scenename)
         {
             IEnumerator SaveAssets()
             {
                 Dictionary<string, GameObject> assets = assetsByScene[scenename];
                 foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
                 {
                     if (assets.ContainsKey(go.name = ""))
                     {
                         Lord lord = new Lord();
                         lord.AncientSword = GameObject.Instantiate(go);
                         GameObject.DontDestroyOnLoad(lord.AncientSword);
                         lord.AncientSword.SetActive(true);
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