using UnityEngine;
using System.Collections;
using extOSC;
using System.IO;
using System;

//<summary>
//Ball movement controlls and simple third-person-style camera
//</summary>
public class RollerBall : MonoBehaviour {

	//public GameObject ViewCamera = null;
	public AudioClip JumpSound = null;
	public AudioClip HitSound = null;
	public AudioClip CoinSound = null;

	private Rigidbody mRigidBody = null;
	private AudioSource mAudioSource = null;
	private bool mFloorTouched = false;
    private bool buttonHeld = false;
    private float originalScale = 1.6183f;

    private int collectedCoins;

    void Start () {
		mRigidBody = GetComponent<Rigidbody> ();
		mAudioSource = GetComponent<AudioSource> ();
        collectedCoins = 0;
    }

    public void onMessage(OSCMessage msg)
    {
        string logString = string.Format("Ball script: msg {0}", msg.ToString());
        File.AppendAllText(Application.dataPath + "/Log.txt", logString + Environment.NewLine);

        //if ((msg.Values[0].FloatValue == 0) && mFloorTouched)
        //{
        //    
        //}

        //if (msg.Values[0].FloatValue == 0)
        //{
        //    Vector3 currentScale = transform.localScale;
        //    if (currentScale.y > 1)
        //    {
        //        transform.localScale = new Vector3(currentScale.x, currentScale.y - 0.1f, currentScale.z);

        //    }
        //} else
        //{
        //    Vector3 currentScale = transform.localScale;
        //    transform.localScale = new Vector3(currentScale.x, currentScale.x, currentScale.z);
        //}
        //string scaleYString = string.Format("Transform Y {0}", transform.localScale.y);
        //File.AppendAllText(Application.dataPath + "/Log.txt", scaleYString + Environment.NewLine);

        if (msg.Values[0].FloatValue == 0)
        {
            buttonHeld = false;
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(originalScale, originalScale, originalScale);

            //if (mFloorTouched || (since_left_floor <= 6))
            //{
                Vector3 up = FindObjectOfType<CubeController>().gameObject.transform.up;
                mRigidBody.AddForce(up * 250);
                File.AppendAllText(Application.dataPath + "/Log.txt", "Ball script: added force" + Environment.NewLine);
            //}
        }
        else
        {
            buttonHeld = true;
        }
    }

    void Update()
    {
        if (buttonHeld)
        {
            Vector3 currentScale = transform.localScale;
            if (currentScale.y > 1)
            {
                transform.localScale = new Vector3(currentScale.x - 0.05f, currentScale.y - 0.05f, currentScale.z - 0.05f);
            }
        }

        //string scaleYString = string.Format("Transform Y {0}", transform.localScale.y);
        //File.AppendAllText(Application.dataPath + "/Log.txt", scaleYString + Environment.NewLine);
    }


    void FixedUpdate () {
		//if (mRigidBody != null) {
		//	if (Input.GetButton ("Horizontal")) {
		//		mRigidBody.AddTorque(Vector3.back * Input.GetAxis("Horizontal")*10);
		//	}
		//	if (Input.GetButton ("Vertical")) {
		//		mRigidBody.AddTorque(Vector3.right * Input.GetAxis("Vertical")*10);
		//	}
		//	if (Input.GetButtonDown("Jump")) {
		//		if(mAudioSource != null && JumpSound != null){
		//			mAudioSource.PlayOneShot(JumpSound);
		//		}
		//		mRigidBody.AddForce(Vector3.up*200);
		//	}
		//}
		//if (ViewCamera != null) {
		//	Vector3 direction = (Vector3.up*2+Vector3.back)*2;
		//	RaycastHit hit;
		//	Debug.DrawLine(transform.position,transform.position+direction,Color.red);
		//	if(Physics.Linecast(transform.position,transform.position+direction,out hit)){
		//		ViewCamera.transform.position = hit.point;
		//	}else{
		//		ViewCamera.transform.position = transform.position+direction;
		//	}
		//	ViewCamera.transform.LookAt(transform.position);
		//}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = true;
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		} else {
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		}
	}

	void OnCollisionExit(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Coin")) {
			if(mAudioSource != null && CoinSound != null){
				mAudioSource.PlayOneShot(CoinSound);
			}
			Destroy(other.gameObject);
            collectedCoins++;
            File.AppendAllText(Application.dataPath + "/Log.txt", "Picked up totally " + collectedCoins.ToString() + Environment.NewLine);

        }
    }

    public int getCollectedCoins()
    {
        return collectedCoins;
    }
}
