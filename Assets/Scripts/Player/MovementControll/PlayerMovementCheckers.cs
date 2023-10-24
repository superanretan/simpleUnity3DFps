using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMovement
{
   public void CheckIsGrounded()
   {
      RaycastHit hit;
      if (Physics.SphereCast(GetIsGroundedCheckRayCastOrigin(), 0.09f, -Vector3.up, out hit,
             isGroundedRaycastCheckDistance,
             layerMask: groundLayer))
      {
         isGrounded = true;
      }
      else
         isGrounded = false;
   }

   private Vector3 GetIsGroundedCheckRayCastOrigin()
   {
      var rayCastOrigin = transform.position;
      rayCastOrigin.y += isGroundedRaycastHeightOffset;
      return rayCastOrigin;
   }
   
}
