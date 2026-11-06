using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vShooter
{
	// Token: 0x0200064B RID: 1611
	public class vAimCanvas : MonoBehaviour
	{
		// Token: 0x060024F0 RID: 9456 RVA: 0x000CF6D4 File Offset: 0x000CD8D4
		protected virtual void Start()
		{
			this.onDisableScopeCamera.Invoke();
			this.onDisableScopeUI.Invoke();
			this.onDisableAim.Invoke();
			if (this.aimCenter)
			{
				this.sizeDeltaCenter = this.aimCenter.sizeDelta;
			}
			if (this.aimTarget)
			{
				this.sizeDeltaTarget = this.aimTarget.sizeDelta;
			}
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000CF73E File Offset: 0x000CD93E
		public void DisableAll()
		{
			this.onDisableScopeCamera.Invoke();
			this.onDisableScopeUI.Invoke();
			this.onDisableAim.Invoke();
			this.isValid = false;
			this.isAimActive = false;
			this.isScopeCameraActive = false;
			this.isScopeUIActive = false;
		}

		// Token: 0x040024B0 RID: 9392
		public RectTransform aimTarget;

		// Token: 0x040024B1 RID: 9393
		public RectTransform aimCenter;

		// Token: 0x040024B2 RID: 9394
		public bool useScopeTransition = true;

		// Token: 0x040024B3 RID: 9395
		public bool scaleAimWithMovement = true;

		// Token: 0x040024B4 RID: 9396
		[Tooltip("Set aim center to same position of aim target ")]
		public bool aimCenterToAimTarget;

		// Token: 0x040024B5 RID: 9397
		public float scaleWithMovement = 2f;

		// Token: 0x040024B6 RID: 9398
		public float smoothChangeScale = 2f;

		// Token: 0x040024B7 RID: 9399
		public float smoothTransitionIn = 10f;

		// Token: 0x040024B8 RID: 9400
		public float smoothTransitionOut = 5f;

		// Token: 0x040024B9 RID: 9401
		[Range(0f, 1f)]
		public float movementSensibility = 0.1f;

		// Token: 0x040024BA RID: 9402
		public UnityEvent onEnableAim;

		// Token: 0x040024BB RID: 9403
		public UnityEvent onDisableAim;

		// Token: 0x040024BC RID: 9404
		public UnityEvent onCheckvalidAim;

		// Token: 0x040024BD RID: 9405
		public UnityEvent onCheckInvalidAim;

		// Token: 0x040024BE RID: 9406
		public UnityEvent onEnableScopeCamera;

		// Token: 0x040024BF RID: 9407
		public UnityEvent onDisableScopeCamera;

		// Token: 0x040024C0 RID: 9408
		public UnityEvent onEnableScopeUI;

		// Token: 0x040024C1 RID: 9409
		public UnityEvent onDisableScopeUI;

		// Token: 0x040024C2 RID: 9410
		[HideInInspector]
		public bool isValid;

		// Token: 0x040024C3 RID: 9411
		[HideInInspector]
		public bool isAimActive;

		// Token: 0x040024C4 RID: 9412
		[HideInInspector]
		public bool isScopeCameraActive;

		// Token: 0x040024C5 RID: 9413
		[HideInInspector]
		public bool isScopeUIActive;

		// Token: 0x040024C6 RID: 9414
		[HideInInspector]
		public Vector2 sizeDeltaTarget;

		// Token: 0x040024C7 RID: 9415
		[HideInInspector]
		public Vector2 sizeDeltaCenter;
	}
}
