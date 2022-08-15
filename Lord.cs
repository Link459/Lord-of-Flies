namespace Lord_of_Flies
{
    public class Lord : MonoBehaviour
    {
        private HealthManager _hm;
        private tk2dSpriteAnimator _anim;
        private PlayMakerFSM _control;
        private PlayMakerFSM _stuns;
        private Rigidbody2D _rb;
        public GameObject AncientSword;
        private ParticleSystem _trail;

        public void Awake()
        {
            _hm = gameObject.GetComponent<HealthManager>();
            _control = gameObject.LocateMyFSM("Control");
            _stuns = gameObject.LocateMyFSM("Stun Control");
            _anim = gameObject.GetComponent<tk2dSpriteAnimator>();
            _rb = GetComponent<Rigidbody2D>();

        }
        public IEnumerator Start()  
        {
            _trail = Trail.AddTrail(gameObject, 3, 0.8f, 1.5f, 1, 1.8f, Color.red);
            _hm.hp = 3000;

            Action trajector(Func<GameObject> proj,float speed,float height,float time)
            {
                return () =>
                {
                    (float x, float y, float z) = transform.position;
                    float vx = speed * Math.Sign(transform.localScale.x);
                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(proj?.Invoke(), new Vector3(x,y,z),Quaternion.Euler(Vector3.zero))
                            .GetComponent<Rigidbody2D>().velocity = new Vector2(vx,y);
                        proj.Invoke().transform.position = new Vector3(x,Mathf.Lerp(0f, height + i, time + i),z);
                    }
                };
            }
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

            _control.GetAction<FloatMultiply>("Slash Recover").multiplyBy = 5 / 6f;
            
            _stuns.FsmVariables.GetFsmInt("Stun Combo").Value = 22;
            _stuns.FsmVariables.GetFsmInt("Stun Hit Max").Value = 28;
            
            _control.GetState("Attack1 S1").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            _control.GetState("Attack1 S2").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            _control.GetState("Attack1 S3").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            _control.GetState("Attack2 S1").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            _control.GetState("Attack2 S2").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            _control.GetState("Attack2 S3").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            _control.GetState("Attack2 S4").InsertMethod(0, ProjectileSpawner(() => AncientSword, 30f));
            
            _control.GetState("Attack1 S1").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            _control.GetState("Attack1 S2").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            _control.GetState("Attack1 S3").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            _control.GetState("Attack2 S1").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            _control.GetState("Attack2 S2").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            _control.GetState("Attack2 S3").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            _control.GetState("Attack2 S4").InsertMethod(0, trajector(() => AncientSword, 30f,5f,2f));
            
            _control.GetState("Spin Slash").InsertCoroutine(4,SpinSlashLaunch);
            _control.GetState("").InsertCoroutine(0,SwordSlash);
            
            _control.CreateState("Sword Spawn");
            FsmState swordSpawn = _control.GetState("Sword Spawn");
            swordSpawn.AddTransition("Jump","Sword Spawn");
            swordSpawn.AddCoroutine(SwordSpawn);
            yield break;
        }
        private IEnumerator SpinSlashLaunch()
        {
            for(int i = 0; i < 20;i++)
            {
                var direction = new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time),0);
                AncientSword.transform.position = direction;
                yield return new WaitForSeconds(0.05f);
            }
        }
        private IEnumerator SwordSpawn()
        {
            float angle = 0;
            for (int i = 0;i < 1;i++)
            {
                angle += Mathf.PI / 5;
                var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle),0);
                var go = Instantiate(AncientSword,transform.position,Quaternion.identity);
                go.transform.position += 5 * direction;
            }
            yield return new WaitForSeconds(0.5f);
            AncientSword.transform.position = HeroController.instance.transform.position;
        }
        private IEnumerator SwordSlash()
        {
            AncientSword.transform.position = new Vector2(transform.position.x, 2 * Time.deltaTime * 2);
            yield return new WaitForSeconds(0.5f);
            AncientSword.transform.position = HeroController.instance.transform.position * Time.deltaTime * 2;
        } 
    }
}