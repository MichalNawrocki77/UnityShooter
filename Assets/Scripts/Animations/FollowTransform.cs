using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    //I created this script so that Targets and Hints of Animation Rigging package could follow the position of a gun that the hands are supposed to hold. At first I wanted to set the target and hint objects as children of a gun, but that breaks the IK functionality for some reason.

    [SerializeField] Transform transformToFollow;
    void Update()
    {
        transform.localPosition = transformToFollow.position;
        transform.localRotation = transformToFollow.rotation;
    }
}
