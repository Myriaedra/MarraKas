using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TPPlayerManager : MonoBehaviour {

    [Header("========TPStuff========")]
    public Transform TPUpTransform;
    public Transform TPDownTransform;
    public TimelineAsset timelineGoingUp;
    public TimelineAsset timelineGoingDown;
    public PlayableDirector pD;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "TPDown" && Vector3.Distance(transform.position, TPDownTransform.position) < 2.5f)
        {
            transform.position = TPUpTransform.position + TPUpTransform.forward * 5;
            transform.rotation = Quaternion.LookRotation(TPUpTransform.forward);
        }
        else if (other.name == "TPUp" && Vector3.Distance(transform.position, TPUpTransform.position) < 2.5f)
        {
            transform.position = TPDownTransform.position + TPDownTransform.forward * 5;
            transform.rotation = Quaternion.LookRotation(TPDownTransform.forward);
        }
    }

    IEnumerator hey(bool goingUp)
    {
        PlayerController.controlsAble = false;
        if (goingUp)
        {
            pD.Play(timelineGoingUp);
        }
        else
        {
            pD.Play(timelineGoingDown);
        }

        while (pD.state == PlayState.Playing)
        {
            yield return null;
        }
        PlayerController.controlsAble = true;
    }
}
