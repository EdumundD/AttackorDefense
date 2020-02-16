using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class FollowTarget : MonoBehaviour {

    [SerializeField] private GameObject target_Blue;
    [SerializeField] private GameObject target_Green;
    [SerializeField] private GameObject target_Red;
    [SerializeField] private GameObject target_Brown;

    [HideInInspector]
    public Transform target;
    private Vector3 offset = new Vector3(-1,15,-13.5f);
    private float smoothing = 2;
    private bool isend = false;

    private Vector3 dirVector3;
    private Vector3 rotaVector3;
    private float paramater = 0.1f;
    //旋转参数
    private float xspeed = -0.05f;
    private float yspeed = 0.1f;

    private float dis;

    public Vector3 targetPosition;
    private void Awake()
    {
        target = target_Blue.transform;
        targetPosition = target.position + offset;
    }
    public void setTarget()
    {
        if (GameFacade.Instance.URoleType == RoleType.Blue)
        {
            target = target_Blue.transform;
        }
        if (GameFacade.Instance.URoleType == RoleType.Red)
        {
            target = target_Red.transform;
        }
        if (GameFacade.Instance.URoleType == RoleType.Green)
        {
            target = target_Green.transform;
        }
        if (GameFacade.Instance.URoleType == RoleType.Brown)
        {
            target = target_Brown.transform;
        }
        targetPosition = target.position + offset;
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(targetPosition, transform.position) > 0.1f && isend == false)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            isend = true;
            ////旋转
            //if (Input.GetMouseButtonDown(1))
            //{
            //    rotaVector3.y += Input.GetAxis("Horizontal") * yspeed;
            //    rotaVector3.x += Input.GetAxis("Vertical") * xspeed;
            //    transform.rotation = Quaternion.Euler(rotaVector3);
            //}

            //移动
            dirVector3 = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.z = 7;
                else dirVector3.z = 3;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.z = -7;
                else dirVector3.z = -3;
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = -7;
                else dirVector3.x = -3;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = 7;
                else dirVector3.x = 3;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = -2;
                else dirVector3.y = -1;
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = 2;
                else dirVector3.y = 1;
            }
            transform.Translate(dirVector3 * paramater, Space.World);
            //限制摄像机范围
            transform.position = Vector3.ClampMagnitude(transform.position, 100);
        }
    }
}
