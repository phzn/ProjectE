using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006AD RID: 1709
	[vClassHeader("Trigger Change Camera State", true, "icon_v2", false, "", openClose = false)]
	public class vTriggerChangeCameraState : vMonoBehaviour
	{
		// Token: 0x0600298D RID: 10637 RVA: 0x000E9D8C File Offset: 0x000E7F8C
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				if (this.tpInput == null || this.tpInput.gameObject != other.gameObject)
				{
					this.tpInput = other.GetComponent<vThirdPersonInput>();
				}
				if (this.tpInput != null)
				{
					if (this.cameraState != string.Empty)
					{
						this.tpInput.ChangeCameraState(this.cameraState, this.smoothTransition);
					}
					else if (this.cameraState == string.Empty)
					{
						this.tpInput.ResetCameraState();
					}
					if (!string.IsNullOrEmpty(this.customCameraPoint))
					{
						this.tpInput.customlookAtPoint = this.customCameraPoint;
					}
					this.tpInput.cc.keepDirection = this.keepDirection;
				}
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000E9E6A File Offset: 0x000E806A
		private void OnTriggerExit(Collider other)
		{
			if (this.resetCameraStateOnExitTrigger && other.gameObject.CompareTag("Player") && this.tpInput != null)
			{
				this.tpInput.ResetCameraState();
			}
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000E9EA0 File Offset: 0x000E80A0
		private void OnDrawGizmos()
		{
			Gizmos.color = this.gizmoColor;
			this.comp = base.gameObject.GetComponent<BoxCollider>();
			if (this.comp != null)
			{
				base.gameObject.GetComponent<BoxCollider>().isTrigger = true;
				base.gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
				base.gameObject.GetComponent<BoxCollider>().size = Vector3.one;
			}
			Gizmos.matrix = base.transform.localToWorldMatrix;
			if (this.comp == null)
			{
				base.gameObject.AddComponent<BoxCollider>();
			}
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000E9F4C File Offset: 0x000E814C
		private Vector3 getLargerScale(Vector3 value)
		{
			if (value.x > value.y || value.x > value.z)
			{
				return new Vector3(value.x, value.x, value.x);
			}
			if (value.y > value.x || value.y > value.z)
			{
				return new Vector3(value.y, value.y, value.y);
			}
			if (value.z > value.y || value.z > value.x)
			{
				return new Vector3(value.z, value.z, value.z);
			}
			return base.transform.localScale;
		}

		// Token: 0x040029B1 RID: 10673
		[Tooltip("Check if you want to lerp the state transitions, you can change the lerp value on the TPCamera - Smooth Follow variable")]
		public bool smoothTransition = true;

		// Token: 0x040029B2 RID: 10674
		public bool keepDirection = true;

		// Token: 0x040029B3 RID: 10675
		[vHelpBox("Keep it empty to Reset back to Default", vHelpBoxAttribute.MessageType.None)]
		[Tooltip("Check your CameraState List and set the State here, use the same String value.\n*Leave this field empty to return the original state")]
		public string cameraState;

		// Token: 0x040029B4 RID: 10676
		public bool resetCameraStateOnExitTrigger;

		// Token: 0x040029B5 RID: 10677
		[Tooltip("Set a new target for the camera.\n*Leave this field empty to return the original target (Player)")]
		public string customCameraPoint;

		// Token: 0x040029B6 RID: 10678
		public Color gizmoColor = Color.green;

		// Token: 0x040029B7 RID: 10679
		private Component comp;

		// Token: 0x040029B8 RID: 10680
		public vThirdPersonInput tpInput;
	}
}
