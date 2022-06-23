


namespace Lord_of_Flies
{
    internal class LordFinder : MonoBehaviour
    {
        private static readonly FastReflectionDelegate _updateDelgate =
            typeof(BossStatue)
                .GetMethod("UpdateDetails", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetFastDelegate();

        private void Start()
        {
            USceneManager.activeSceneChanged += SceneChanged;
        }

        private void SceneChanged(Scene arg0, Scene arg1) => StartCoroutine(SceneChangeRoutine(arg0.name, arg1.name));

        private IEnumerator SceneChangeRoutine(string prev, string next)
        {
            if (next == "GG_Workshop") yield return SetStatue();
            if (next != "GG_Sly") yield break;
            if (prev != "GG_Workshop") yield break;

            StartCoroutine(AddComponent());
        }

        private static IEnumerator SetStatue()
        {
            yield return null;

            GameObject statue = GameObject.Find("GG_Statue_Sly");

            var scene = ScriptableObject.CreateInstance<BossScene>();
            scene.sceneName = "GG_Sly";

            var bs = statue.GetComponent<BossStatue>();
            bs.dreamBossScene = scene;
            bs.dreamStatueStatePD = "statueStateSly";

            bs.SetPlaquesVisible(bs.StatueState.isUnlocked && bs.StatueState.hasBeenSeen);

            var details = new BossStatue.BossUIDetails();
            details.nameKey = details.nameSheet = "Sly_Name";
            details.descriptionKey = details.descriptionSheet = "Sly_Desc";
            bs.dreamBossDetails = details;

            GameObject @switch = statue.Child("dream_version_switch");
            @switch.SetActive(true);
            @switch.transform.position = new Vector3(173.3f, 36.5f, 0.4f);

            GameObject burst = @switch.Child("lit_pieces/Burst Pt");
            burst.transform.position = new Vector3(167.7f, 36.3f, 0.4f);

            GameObject glow = @switch.Child("lit_pieces/Base Glow");
            glow.transform.position = new Vector3(173.7f, 36.2f, 0.4f);

            glow.GetComponent<tk2dSprite>().color = Color.white;

            var fader = glow.GetComponent<ColorFader>();
            fader.upColour = Color.white;
            fader.downColour = Color.white;

            var toggle = statue.GetComponentInChildren<BossStatueDreamToggle>(true);

            toggle.SetState(true);

            Modding.ReflectionHelper.SetField
            (
                toggle,
                "colorFaders",
                toggle.litPieces.GetComponentsInChildren<ColorFader>(true)
            );

            toggle.SetOwner(bs);

            yield return new WaitWhile(() => bs.bossUIControlFSM == null);

            _updateDelgate(bs);
        }

        
        private static IEnumerator AddComponent()
        {
            yield return null;

            GameObject go = GameObject.Find("Sly Boss");
            go.AddComponent<Lord>();
            go.GetComponent<tk2dSprite>().color = Color.cyan;
            go.AddComponent<Healer>();

        }

        static void Log(object o)
        {
            Log($"[{Assembly.GetExecutingAssembly().GetName().Name}]: " + o);
        }
    }
} 