using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraBehavior : MonoBehaviour
{

    [SerializeField]
    protected Transform trackingTarget;
    [SerializeField]
    float xOffset;
    [SerializeField]
    float yOffset;
    [SerializeField]
    protected float followSpeed;
    [SerializeField]
    protected bool isXLocked = false;
    [SerializeField]
    protected bool isYLocked = false;
    [SerializeField]
    protected float hitboxRate; //reccommend: 0.15f

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        basicMove();
    }

    void basicMove()
    {
        float xTarget = trackingTarget.position.x + xOffset;
        float yTarget = trackingTarget.position.y + yOffset;

        float xNew = transform.position.x;
        if (!isXLocked)
        {
            xNew = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * followSpeed);
        }

        float yNew = transform.position.y;
        if (!isYLocked)
        {
            yNew = Mathf.Lerp(transform.position.y, yTarget, Time.deltaTime * followSpeed);
        }

        /*float xNew = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * followSpeed);
        float yNew = Mathf.Lerp(transform.position.y, yTarget, Time.deltaTime * followSpeed);*/

        if(!(xNew > 27.17 || xNew < 8.36)) // if in the bounds of the level, scroll normally. otherwise ignore it.
        {
            transform.position = new Vector3(xNew, yNew, transform.position.z);
        }
    }
}