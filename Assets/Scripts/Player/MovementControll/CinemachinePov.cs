using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachinePov : CinemachineExtension
{
   [SerializeField] private PlayerMain playerMain;
   [SerializeField] private float clampAngle = 80f;
   [SerializeField] private float horizontalSpeed = 10f;
   private Vector3 startingRotation;
   
   
    protected override void Awake()
    {
        if(playerMain == null)
            Debug.LogError($"Setup PlayerMain in {this.gameObject.transform.name} CinemachinePov script!!!");
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)  //for override CM input provider for mouse interaction for camera look movement
    {
        if (!Application.isPlaying || playerMain == null) return;
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null)
                    startingRotation.y = 0;

                Vector2 deltaInput = playerMain.GetMouseDelta();

                if (deltaInput.x > 300) return;  //prevent for start wierd mouse delta input value
                
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, playerMain.transform.rotation.eulerAngles.y, 0f);
            }
        }
    }
}
