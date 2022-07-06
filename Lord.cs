
namespace Lord_of_Flies
{
    internal class Lord : MonoBehaviour
    {
        private const int HP = 3000;
        private HealthManager _hm;
        public ParticleSystem _trail;

        public IEnumerator Start()  
        {
            _trail = Trail.AddTrail(gameObject, 4, 0.8f, 1.5f, 2, 1.8f, Color.green);

            yield break;
        }
    }
}
