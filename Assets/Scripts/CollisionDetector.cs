using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using OculusSampleFramework;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;

public class CollisionDetector : MonoBehaviour
{
    //private OVRGrabber grabbable;
    //private OVRHandGrabInteractable handGrabInteractable;
    //public GameObject hand;
    public bool detected = false;

    // Start is called before the first frame update
    void Start()
    {
        /*//grabbable = gameObject.AddComponent<OVRGrabbable>();
        grabbable = GetComponent<OVRGrabber>();

        if (grabbable != null)
        {
            grabbable.enabled = true;
        }*/
    }

    void OnTriggerEnter(Collider other)
    {
        detected = true;        
    }

    void OnTriggerExit(Collider other)
    {
        detected = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (this.transform.parent == null && added == 0)
        {
            //grabbable = gameObject.AddComponent<OVRGrabber>();
            gameObject.AddComponent<Rigidbody>();
            //gameObject.GetComponent<HandGrabInteractable>().SetGrabbable(grabbable);
            //handGrabInteractable = gameObject.AddComponent<HandGrabInteractable>();
            //handGrabInteractable.SetGrabbable(grabbable);
            added = 1;
        }*/
    }
}
