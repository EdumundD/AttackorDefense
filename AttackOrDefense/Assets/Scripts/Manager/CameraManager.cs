//
// @brief: 相机管理类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/13
// 
// 
//

using UnityEngine;
using DG.Tweening;

public class CameraManager : BaseManager {

    private GameObject camerGo;
    private Animator cameraAnim;
    private FollowTarget followTarget;
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    public CameraManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        camerGo = Camera.main.gameObject;
        cameraAnim = camerGo.GetComponent<Animator>();
        followTarget = camerGo.GetComponent<FollowTarget>();
        cameraAnim.enabled = true;
        followTarget.enabled = false;
    }

  
    public void FollowRole()
    {
        cameraAnim.enabled = false;
        originalPosition = camerGo.transform.position;
        originalRotation = camerGo.transform.eulerAngles;
        followTarget.setTarget();
        Quaternion targetQuaternion = Quaternion.LookRotation(followTarget.target.position - camerGo.transform.position);
        camerGo.transform.DORotateQuaternion(targetQuaternion, 1.8f).OnComplete(() => {
            followTarget.enabled = true; });

    }

    public void WalkthroughScene()
    {
        followTarget.enabled = false;
        camerGo.transform.DOMove(originalPosition, 3f);
        camerGo.transform.DORotate(originalRotation, 3f).OnComplete(() =>
        {
            cameraAnim.enabled = true;
        });
    }
}
