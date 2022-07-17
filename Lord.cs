namespace Lord_of_Flies
{
    public class Lord : MonoBehaviour
    {

             public static GameObject AncientSword { get; set; }
     public static void Preloaded(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects){
        AncientSword = UnityEngine.Object.Instantiate(preloadedObjects["sharedassets447"]["Shot HK Shadow(Clone)"]);
        AncientSword.GetComponent<tk2dSprite>().color = new Color32(1,1,1,255);
     }
     
        public HealthManager _hm;
        public tk2dSpriteAnimator _anim;
        public PlayMakerFSM _control;
        public PlayMakerFSM _stuns;
        public ParticleSystem _trail;
        public void Awake()
        {
            _hm = gameObject.GetComponent<HealthManager>();
            _control = gameObject.LocateMyFSM("Control");
            _stuns = gameObject.LocateMyFSM("Stun Control");
            _anim = gameObject.GetComponent<tk2dSpriteAnimator>();
        }
        public IEnumerator Start()  
        {
         _trail = Trail.AddTrail(gameObject, 3, 0.8f, 1.5f, 1, 1.8f, Color.green);
         _hm.hp = 3000;

           Action ProjectileSpawner(Func<GameObject> proj, float speed)
            {
                return () =>
                {
                    Quaternion angle = Quaternion.Euler(Vector3.zero);
                    (float x, float y, float z) = transform.position;
                    float vx = speed * Math.Sign(transform.localScale.x);

                    for (float i = 0; i <= 3; i += 1.5f)
                    {
                        Instantiate(proj?.Invoke(), new Vector3(x, y - i, z), angle)
                            .GetComponent<Rigidbody2D>()
                            .velocity = new Vector2(vx, 0);
                    }
                };
            }
            //Modifing animation speeds
            _anim.Library.GetClipByName("Antic").fps = 20;
            _anim.Library.GetClipByName("Attack1 S1").fps = 20;
            _anim.Library.GetClipByName("Attack1 S2").fps = 20;
            _anim.Library.GetClipByName("Attack1 S3").fps = 20;
            _anim.Library.GetClipByName("Attack2 Antic").fps = 20;
            _anim.Library.GetClipByName("Attack2 S1").fps = 20;
            _anim.Library.GetClipByName("Attack2 S2").fps = 20;
            _anim.Library.GetClipByName("Attack2 S3").fps = 20;
            _anim.Library.GetClipByName("Attack2 S4").fps = 20;
            _anim.Library.GetClipByName("Stomp Antic").fps = 20;
            _anim.Library.GetClipByName("Spin Slash").fps = 20;
            _anim.Library.GetClipByName("Spin Slash Recover").fps = 20;
            _anim.Library.GetClipByName("SpinStomp Antic").fps = 20;
            _anim.Library.GetClipByName("Jump Antic").fps = 20;
            _anim.Library.GetClipByName("Jump").fps = 20;
            _anim.Library.GetClipByName("Charge Ground").fps = 10;
            _anim.Library.GetClipByName("Dash").fps = 20;
            _anim.Library.GetClipByName("GSlash End").fps = 20;
            _anim.Library.GetClipByName("Cyclone").fps = 20;
            _anim.Library.GetClipByName("Fall").fps = 20;
            
            //modifing stuns
            _stuns.FsmVariables.GetFsmInt("Stun Combo").Value = 22;
            _stuns.FsmVariables.GetFsmInt("Stun Hit Max").Value = 28;
           //setting the modified attck points
           _control.GetState("").InsertCoroutine(4, (Func<IEnumerator>)SpinSlashLaunch,false);

           _control.GetState("Attack1 S1").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
           _control.GetState("Attack1 S2").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
           _control.GetState("Attack1 S3").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
           _control.GetState("Attack2 S1").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
           _control.GetState("Attack2 S2").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
           _control.GetState("Attack2 S3").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
           _control.GetState("Attack2 S4").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
   
 


            yield break;
        }

        //making the modified attacks;
         IEnumerator SpinSlashLaunch()
        {
            yield break;
        }
    }
}