using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Movement : MonoBehaviour
{
    private Vector2 trackpad;
    private float Direction;
    private Vector3 moveDirection;
 

    public SteamVR_Input_Sources Hand;//Set Hand To Get Input From

    public SteamVR_Action_Vector2 TrackpadAction;// action for getting trackpad input
    public float speed;
    public GameObject Head;
    public CapsuleCollider Collider;
    public GameObject AxisHand;//Hand Controller GameObject
    public float Deadzone;//the Deadzone of the trackpad. used to prevent unwanted walking.
    
    // Start is called before the first frame update
    private void Start()
    {
        Collider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        updateInput();
        updateCollider();

        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward; //get the angle of the touch and correct it for the rotation of the controller
        Rigidbody rBody = GetComponent<Rigidbody>();
        Vector3 velocity = new Vector3(0,0,0);
        if (trackpad.magnitude > Deadzone)
        {
            velocity = moveDirection;
            rBody.AddForce(velocity.x*speed - rBody.velocity.x, 0, velocity.z*speed - rBody.velocity.z, ForceMode.VelocityChange);

            Debug.Log("Velocity" + velocity);
            Debug.Log("Movement Direction:" + moveDirection);
        }
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    private void updateCollider()
    {
        //Set size and position of the capsule collider so it maches our head.
        Collider.height = Head.transform.localPosition.y;
        Collider.center = new Vector3(Head.transform.localPosition.x, Head.transform.localPosition.y / 2, Head.transform.localPosition.z);
    }

    private void updateInput()
    {
        trackpad = TrackpadAction.GetAxis(Hand);
    }
}
