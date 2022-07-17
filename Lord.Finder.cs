
namespace Lord_of_Flies
{
  public class LordFinder : MonoBehaviour
    {
		private void Start(){UnityEngine.SceneManagement.SceneManager.activeSceneChanged += this.SceneChanged;}
		
		private void SceneChanged(Scene lastscene, Scene currentscene)
		{
			if (GlobalSettings.modifyPantheons == false && lastscene.name == "GG_Workshop" && currentscene.name == "GG_Sly"){StartCoroutine(AddComponents());}
			
			if(GlobalSettings.modifyPantheons == true && currentscene.name == "GG_Sly"){StartCoroutine(AddComponents());}
		}
		private static IEnumerator AddComponents()
	    {
			 GameObject.Find("Sly Boss").AddComponent<Lord>();
			 yield break;
		}
	}
}