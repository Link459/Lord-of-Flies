namespace Lord_of_Flies
{
    public class Lord : MonoBehaviour
    {
        private HealthManager _hm;
        private tk2dSpriteAnimator _anim;
        private PlayMakerFSM _control;
        private PlayMakerFSM _stuns;
        private ParticleSystem _trail;

        
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

            _control.GetAction<FloatMultiply>("Slash Recover").multiplyBy = 5 / 6f;
            
            //modifing stuns
            _stuns.FsmVariables.GetFsmInt("Stun Combo").Value = 22;
            _stuns.FsmVariables.GetFsmInt("Stun Hit Max").Value = 28;
           //setting the modified attck points
           _control.GetState("").InsertCoroutine(4, (Func<IEnumerator>)SpinSlashLaunch,false);

           _control.GetState("Attack1 S1").InsertMethod(0, ProjectileSpawner(() => A(index), 30f));
           _control.GetState("Attack1 S2").InsertMethod(0, ProjectileSpawner(() => AncientSword(index), 30f));
           _control.GetState("Attack1 S3").InsertMethod(0, ProjectileSpawner(() => AncientSword(index), 30f));
           _control.GetState("Attack2 S1").InsertMethod(0, ProjectileSpawner(() => AncientSword(index), 30f));
           _control.GetState("Attack2 S2").InsertMethod(0, ProjectileSpawner(() => AncientSword(index), 30f));
           _control.GetState("Attack2 S3").InsertMethod(0, ProjectileSpawner(() => AncientSword(index), 30f));
           _control.GetState("Attack2 S4").InsertMethod(0, ProjectileSpawner(() => , 30f));
   
           //creating and setting the the Sword Spawn attck
           _control.CreateState("Sword Spawn");
           FsmState swordSpawn = _control.GetState("Sword Spawn");
          /* _control.GetState("").AddAction("Fireball Pos", new GetDistance()
           {
           gameObject = new FsmOwnerDefault()
           {
           OwnerOption = OwnerDefaultOption.SpecifyGameObject,
           GameObject = _control.gameObject
           },
           target = new FsmGameObject()
           {
            Value = HeroController.instance.gameObject
           },
           storeResult = ("Distance"),
           everyFrame = true
           });*/
           swordSpawn.AddTransition("Jump","Sword Spawn");
           
           swordSpawn.AddCoroutine(SwordSpawn);

            yield break;
        }


         //creating the new attacks
         IEnumerator SpinSlashLaunch()
        {
            //spawns a random sword and makes it shoot at the player
            for(int i = 0; i < 5;i++)
            {
                Instantiate(AncientSword1);
                AncientSword1.transform.position = HeroController.instance.transform.position;
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }

        IEnumerator SwordSpawn()
        {
            //spawns the swords in a half circle behind sly
            float angle = 0;
            for (int i = 0;i < 1;i++)
            {
            var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle),0);
            var go = Instantiate(AncientSword[index],transform.position,Quaternion.identity);// spawn your gameobject at the center
            go.transform.position += 5 * direction; // spawn them 5 units away from the center
            angle += Mathf.PI / 5; // for 10 objects in the half circle
            }
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.position = HeroController.instance.transform.position;
            yield break;
    
        }
    }
}