using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Motorcycle_Controller : MonoBehaviour
{
    public bool usingAccelerometer = false;

    //used to determine when player is crashed
    public static bool crash = false;
    public static bool crashed = false;

    //used to enable/disable motorcycle controlling
    public static bool isControllable = true;

    //used to count scores
    public static int score = 0;

    //used to change motorcycle characteristics
    public Rigidbody body;
    public Rigidbody frontFork;
    public Rigidbody rearFork;
    public Rigidbody frontWheel;
    public Rigidbody rearWheel;

    public float speed;
    public float accelaration;
    public float groundedWeightFactor = 20.0f;
    public float inAirRotationSpeed = 10.0f;
    public float wheelieStrength = 15.0f;

    //used to make biker detach from bike when crashed
    public HingeJoint leftHand;
    public HingeJoint rightHand;
    public HingeJoint leftFoot;
    public HingeJoint rightFoot;
    public ConfigurableJoint hips;

    //used for lean backward/forward, changes hips' targetposition value
    public Vector3 leanBackwardTargetPosition;
    public Vector3 normalizeTargetPosition;
    public Vector3 leanForwardTargetPosition;
    public float leanSpeed;

    //used to start/stop dirt particles
    public ParticleSystem dirt;

    //used for showing score particles when flips are done
    public ParticleSystem backflipParticle;
    //public ParticleSystem frontflipParticle;

    //used to determine if motorcycle is grounded or in air
    private RaycastHit hit;
    [SerializeField] private bool onGround = false;
    private bool inAir = false;

    //used to manipulate engine sound pitch
    private AudioSource audioSource;
    private float pitch;

    //used to determine when flip is done
    private bool flip = false;

    //used for knowing input	
    [SerializeField] private bool accelerate = false;
    private bool brake = false;
    [SerializeField] private bool left = false;
    private bool right = false;
    private bool leftORright = false;

    private Transform bodyTransform;
    private float bodyRotationZ;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private int FlipCount;
    [SerializeField] private bool IsBoosting;


    void Start()
    {
        ObserverManager.RefreshTexts.AddListener(SetBehaviourValues);
        ObserverManager.RefreshTexts?.Invoke();

        ObserverManager.GamePlay.AddListener(CanControlChange);

        Input.multiTouchEnabled = true;
        //reset static variables
        crash = false;
        crashed = false;
        isControllable = false;

        //adding motorcycle body as a follow target for camera
        Camera.main.GetComponent<SmoothFollow>().target = body.transform;

        //ignoring collision between motorcycle wheels and body
        Physics.IgnoreCollision(frontWheel.GetComponent<Collider>(), body.GetComponent<Collider>());
        Physics.IgnoreCollision(rearWheel.GetComponent<Collider>(), body.GetComponent<Collider>());

        //ignoring collision between motorcycle and ragdoll
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer("Ragdoll"), true);

        //ignoring collision between motorcycle colliders
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer("Motorcycle"), true);

        //used to manipulate engine sound pitch
        audioSource = body.GetComponent<AudioSource>();

        //setting wheels max angular rotation speed
        rearWheel.GetComponent<Rigidbody>().maxAngularVelocity = speed;
        frontWheel.GetComponent<Rigidbody>().maxAngularVelocity = speed;

        bodyTransform = body.GetComponent<Transform>();
    }
    private void SetBehaviourValues()
    {
        speed = _playerData.Speed;
        accelaration = _playerData.Accelaration;
        leanSpeed = _playerData.LeanSpeed;
    }
    private void CanControlChange()
    {
        isControllable = !isControllable;
    }

    private void BoostPlayer()
    {
        speed = speed * 1.5f * FlipCount;
        IsBoosting = true;
        FlipCount = 0;
        StartCoroutine(DeActiveBoost());
    }
    private IEnumerator DeActiveBoost()
    {
        yield return new WaitForSeconds(2);
        IsBoosting = false;
        speed = _playerData.Speed;
    }

    //Update is called once per frame
    void Update()
    {
        if (isControllable)
        {
            if (onGround)
            {
                if (Input.GetMouseButton(0))
                {
                    brake = false;
                    accelerate = true;
                }
                else
                {
                    brake = true;
                    accelerate = false;
                }


                if (FlipCount > 0 && IsBoosting == false)
                {
                    BoostPlayer();
                }
            }
            else
            {
                accelerate = true;
                if (Input.GetMouseButton(0))
                {
                    left = true;
                }
                else
                {
                    left = false;

                    // // normalize bike rotation

                    // bodyRotationZ = bodyTransform.localRotation.eulerAngles.z;

                    // if (bodyRotationZ >= 30 && bodyRotationZ <= 90 || bodyRotationZ >= 30 && bodyRotationZ <= 90)//normalize
                    // {
                    //     right = true;
                    //     left = false;
                    //     Debug.Log("buraya geldi");
                    // }
                    // else
                    // {
                    //     right = false;
                    //     left = true;
                    //     Debug.Log("şuraya geldi");
                    // }
                }
            }

            if (body.rotation.eulerAngles.z > 210 && body.rotation.eulerAngles.z < 220)
                flip = true;

            if (body.rotation.eulerAngles.z > 320 && flip) //backflip is done
            {
                flip = false;
                FlipCount++;
                //backflipParticle.Emit(1);
            }

            // if (body.rotation.eulerAngles.z < 30 && flip) //frontflip is done
            // {
            //     flip = false;
            //     frontflipParticle.Emit(1);
            //     score += 150;
            // }

            // //if any horizontal key (determined in edit -> project settings -> input)  is pressed or if "formobile" is activated, left or right buttons are touched or accelerometer is used
            if (leftORright)
            {
                if (left)//left horizontal key is pressed or left button is touched on mobile or using accelerometer
                    hips.targetPosition = Vector3.Lerp(hips.targetPosition, leanBackwardTargetPosition, leanSpeed * Time.deltaTime); //lean backward

                if (right)//right horizontal key is pressed or if "formobile" is activated, right button is touched or using accelerometer
                    hips.targetPosition = Vector3.Lerp(hips.targetPosition, leanForwardTargetPosition, leanSpeed * Time.deltaTime); //lean forward
            }

            //changing engine sound pitch depending rear wheel rotational speed
            if (accelerate)
            {
                pitch = rearWheel.angularVelocity.sqrMagnitude / speed;
                pitch *= Time.deltaTime * 2;
                pitch = Mathf.Clamp(pitch + 1, 0.5f, 1.8f);
            }
            else
                pitch = Mathf.Clamp(pitch - Time.deltaTime * 2, 0.5f, 1.8f);
        }

        if (crash && !crashed) //if player just crashed
        {
            Camera.main.GetComponent<SmoothFollow>().target = leftHand.transform; //make camera to follow biker's hips

            //disable hinge joints, so biker detaches from motorcycle
            Destroy(leftHand);
            Destroy(rightHand);
            Destroy(leftFoot);
            Destroy(rightFoot);
            Destroy(hips);

            //turn on collision between ragdoll and motorcycle
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer("Ragdoll"), false);

            body.constraints = RigidbodyConstraints.None;
            frontFork.constraints = RigidbodyConstraints.None;
            frontWheel.constraints = RigidbodyConstraints.None;
            rearFork.constraints = RigidbodyConstraints.None;
            rearWheel.constraints = RigidbodyConstraints.None;

            isControllable = false;
            crashed = true;
        }

        //manipulating engine sound pitch
        pitch = Mathf.Clamp(pitch - Time.deltaTime * 2, 0.5f, 1.8f);
        audioSource.pitch = pitch;
    }

    //physics are calculated in FixedUpdate function
    void FixedUpdate()
    {
        if (isControllable)
        {
            if (accelerate)
            {
                rearWheel.freezeRotation = false; //allow rotation to rear wheel
                rearWheel.AddTorque(new Vector3(0, 0, -speed * Time.deltaTime * accelaration), ForceMode.Impulse); //add rotational speed to rear wheel

                if (onGround)//if motorcycle is standing on object tagged as "Ground"
                {
                    if (!dirt.isPlaying)
                        dirt.Play(); //play dirt particle

                    dirt.transform.position = rearWheel.position; //allign dirt to rear wheel

                }
                else dirt.Stop();

            }
            else dirt.Stop();

            if (brake)
                rearWheel.freezeRotation = true; //disable rotation for rear wheel if player is braking								
            else
                rearWheel.freezeRotation = false; //enable rotation for rear wheel if player isn't braking

            if (left)
            { //left horizontal key (determined in edit -> project settings -> input) is pressed or left button is touched on mobile if "formobile" is activated
                if (!inAir)
                { //rotate left the motorcycle body
                    body.AddTorque(new Vector3(0, 0, 1 * groundedWeightFactor * 100 * Time.deltaTime));
                    body.AddForceAtPosition(body.transform.up * Mathf.Abs(Input.acceleration.x) * wheelieStrength * body.velocity.sqrMagnitude / 100,
                        new Vector3(frontWheel.position.x, frontWheel.position.y - 0.5f, body.transform.position.z));//add wheelie effect
                }
                else
                {
                    body.AddTorque(new Vector3(0, 0, 1 * inAirRotationSpeed * 100 * Time.deltaTime));
                }

            }
            else if (right)
            { //right horizontal key is pressed or right button is touched on mobile
                if (!inAir)
                { //rotate right the motorcycle body
                    body.AddTorque(new Vector3(0, 0, 1 * -groundedWeightFactor * 100 * Time.deltaTime));
                }
                else
                {
                    body.AddTorque(new Vector3(0, 0, 1 * -inAirRotationSpeed * 100 * Time.deltaTime));
                }
            }

            if (Physics.Raycast(rearWheel.position, -body.transform.up, out hit, 0.5f) || Physics.Raycast(frontWheel.position, -body.transform.up, out hit, 0.5f)) // cast ray to know if motorcycle is in air or grounded	
            {
                if (hit.collider.tag == "Ground") //if motorcycle is standig on object taged as "Ground"
                {
                    onGround = true;
                    left = false;
                }
                else
                    onGround = false;

                inAir = false;
            }
            else
            {
                onGround = false;
                inAir = true;
            }

        }
        else dirt.Stop();
    }
    public void ThrowRider()
    {

        if (hips != null)
        {
            hips.GetComponent<Rigidbody>().AddForce(0, 50, 0, ForceMode.Impulse);
            //hips.GetComponent<Rigidbody>().AddTorque(0, 10, 0, ForceMode.Impulse);
        }
        if (body != null)
            body.AddForce(20, 0, 0, ForceMode.Impulse);

    }
}