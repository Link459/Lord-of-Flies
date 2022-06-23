

namespace Lord_of_Flies
{
    [UsedImplicitly]
    public class LordofFlies : Mod, ITogglableMod, ILocalSettings<SaveSettings>
    {
        [PublicAPI]
        public static LordofFlies Instance { get; private set; }

        private SaveSettings _settings;

        public void OnLoadLocal(SaveSettings s) => _settings = s;
        
        public SaveSettings OnSaveLocal() => _settings;
        

        private string _lastScene;

        public LordofFlies() : base("Lord of Flies") {}

        public override void Initialize()
        {
            Instance = this;
            
            Unload();
            
            ModHooks.BeforeSavegameSaveHook += BeforeSaveGameSave;
            ModHooks.AfterSavegameLoadHook += SaveGame;
            ModHooks.SavegameSaveHook += SaveGameSave;
            ModHooks.NewGameHook += AddComponent;
            ModHooks.LanguageGetHook += OnLangGet;
            ModHooks.SetPlayerVariableHook += SetVariableHook;
            ModHooks.GetPlayerVariableHook += GetVariableHook;
            USceneManager.activeSceneChanged += SceneChanged;
        }


        private object SetVariableHook(Type t, string key, object obj)
        {
            if (key != "statueStateSly") 
                return obj;

            var completion = (BossStatue.Completion) obj;
            
            _settings.Completion = completion;

            // Set alt false so if the mod is uninstalled then it doesn't break the mod.
            completion.usingAltVersion = false;

            return completion;
        }
        
        private object GetVariableHook(Type type, string name, object orig)
        {
            return name == "statueStateSly" ? _settings.Completion : orig;
        }

        private void SceneChanged(Scene arg0, Scene arg1)
        {
            // API Issue.
            if (arg1.name == Constants.MENU_SCENE)
            {
                _settings = new SaveSettings();
            }
            
            _lastScene = arg0.name;
        }

        private string OnLangGet(string key, string sheettitle, string orig)
        {
            switch (key)
            {
                case "Sly_Name":
                case "Sly_MAIN" when _lastScene == "GG_Workshop" && PlayerData.instance.statueStateHollowKnight.usingAltVersion:
                    return "Lord of Flies";
                case "Sly_Desc":
                    return "Prepare to die";
                default:
                    return orig;
            }
        }

        private void BeforeSaveGameSave(SaveGameData data)
        {
            _settings.AltStatue = PlayerData.instance.statueStateHollowKnight.usingAltVersion;
            
            PlayerData.instance.statueStateHollowKnight.usingAltVersion = false;
        }

        private void SaveGame(SaveGameData data)
        {
            SaveGameSave();
            AddComponent();
        }
        
        private void SaveGameSave(int id = 0)
        {
            PlayerData.instance.statueStateHollowKnight.usingAltVersion = _settings.AltStatue;
        }

        private static void AddComponent()
        {
            GameManager.instance.gameObject.AddComponent<LordFinder>();
        }

        public void Unload()
        {
            ModHooks.BeforeSavegameSaveHook -= BeforeSaveGameSave;
            ModHooks.AfterSavegameLoadHook -= SaveGame;
            ModHooks.SavegameSaveHook -= SaveGameSave;
            ModHooks.NewGameHook -= AddComponent;
            ModHooks.LanguageGetHook -= OnLangGet;
            ModHooks.SetPlayerVariableHook -= SetVariableHook;
            USceneManager.activeSceneChanged -= SceneChanged;

            var finder = GameManager.instance.gameObject.GetComponent<LordFinder>();

            if (finder != null)
                UObject.Destroy(finder);
        }
    }
}