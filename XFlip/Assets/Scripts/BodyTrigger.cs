using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BodyTrigger : MonoBehaviour
{
    public static bool finish = false;

    //used to play sounds
    public AudioClip bonesCrackSound;
    public AudioClip hitSound;
    public AudioClip oohCrowdSound;

    private AudioSource bonesCrackSC;
    private AudioSource hitSC;
    private AudioSource oohCrowdSC;

    //used to show text when entered in finish
    public Text winText;
    public Text crashText;
    public Color winTextColor;
    public Color crashTextColor;

    //used to know if next level exists.
    private bool nextLevel = false;

    private bool IsHitGround;
    void Start()
    {
        finish = false;
        //ignoring collision between biker's bodytrigger and motorcycle body
        Physics.IgnoreCollision(this.GetComponent<Collider>(), transform.parent.GetComponent<Collider>());

        //add new audio sources and add audio clips to them, used to play sounds
        bonesCrackSC = gameObject.AddComponent<AudioSource>();
        hitSC = gameObject.AddComponent<AudioSource>();
        oohCrowdSC = gameObject.AddComponent<AudioSource>();

        bonesCrackSC.playOnAwake = false;
        hitSC.playOnAwake = false;
        oohCrowdSC.playOnAwake = false;

        bonesCrackSC.rolloffMode = AudioRolloffMode.Linear;
        hitSC.rolloffMode = AudioRolloffMode.Linear;
        oohCrowdSC.rolloffMode = AudioRolloffMode.Linear;

        bonesCrackSC.clip = bonesCrackSound;
        hitSC.clip = hitSound;
        oohCrowdSC.clip = oohCrowdSound;
        //--------------------------------------------------
    }

    void OnTriggerEnter(Collider obj)
    {
        // if (obj.gameObject.tag == "Finish" && !Motorcycle_Controller.crash)//if entered in finish trigger
        // {
        //     finish = true;

        //     Motorcycle_Controller.isControllable = false; //disable motorcycle controlling

        //     //disable rear wheel rotation
        //     var m = transform.root.GetComponent<Motorcycle_Controller>();
        //     m.rearWheel.freezeRotation = true;

        //     //winText.enabled = true; //show win text				

        //     if (Application.loadedLevel < Application.levelCount - 1) //if won level isn't last level (levels are set in File -> Build Settings)
        //     {
        //         nextLevel = true;

        //         winText.text = "CONGRATULATIONS, YOU WON! \n YOUR SCORE IS: " + Motorcycle_Controller.score + "\n\n PRESS SPACE FOR NEXT LEVEL";

        //     }
        //     else //won level is last one
        //     {
        //         winText.text = "CONGRATULATIONS, YOU WON! \n YOUR SCORE IS: " + Motorcycle_Controller.score + "\n\n PRESS SPACE TO PLAY FIRST LEVEL";

        //         nextLevel = false;
        //     }
        // }
        if (obj.tag == "Ground")
        {
            Motorcycle_Controller.crash = true;
            ObserverManager.GameLose?.Invoke();
        }
        else if (obj.tag == "DeadZone")
        {
            Motorcycle_Controller.crash = true;
        }
        else if (IsHitGround == false)
        {
            if (obj.tag == "2xTrigger")
            {
                GameManager.coinMultiplier = 2;
                IsHitGround = true;
            }
            else if (obj.tag == "3xTrigger")
            {
                GameManager.coinMultiplier = 3;
                IsHitGround = true;
            }
            else if (obj.tag == "4xTrigger")
            {
                GameManager.coinMultiplier = 4;
                IsHitGround = true;
            }
            else if (obj.tag == "7xTrigger")
            {
                GameManager.coinMultiplier = 7;
                IsHitGround = true;
            }
            else if (obj.tag == "9xTrigger")
            {
                GameManager.coinMultiplier = 9;
                IsHitGround = true;
            }
        }
        else if (IsHitGround == true)
        {
            var m = transform.root.GetComponent<Motorcycle_Controller>();
            m.ThrowRider();
            ObserverManager.GameWin?.Invoke();
            Destroy(this.GetComponent<Collider>());
            Debug.Log("Multiplier: x" + GameManager.coinMultiplier);
        }

        // else if (obj.tag != "Checkpoint") //if entered in any other trigger than "Finish" & "Checkpoint", that means player crashed
        // {
        //     if (!Motorcycle_Controller.crash)
        //     {
        //         Motorcycle_Controller.crash = true;

        //         //play sounds
        //         //bonesCrackSC.Play();
        //         //hitSC.Play();
        //         //oohCrowdSC.Play();

        //         if (!finish) //if we haven't entered in finish make crash text visible
        //         {
        //             //crashText.enabled = true;

        //             var m = transform.root.GetComponent<Motorcycle_Controller>();
        //             // if (Checkpoint.lastPoint)
        //             //     crashText.text = "PRESS 'C' TO GO TO LAST CHECKPOINT \n PRESS 'R' TO RESTART";
        //             // else
        //             //     crashText.text = "PRESS 'R' TO RESTART";
        //         }
        //     }
        // }

    }
}