using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vShooter
{
	// Token: 0x0200064C RID: 1612
	public class vControlAimCanvas : MonoBehaviour
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000CF7D8 File Offset: 0x000CD9D8
		// (set) Token: 0x060024F4 RID: 9460 RVA: 0x000CF7F9 File Offset: 0x000CD9F9
		public bool isScopeCameraActive
		{
			get
			{
				return this.scopeBackgroundCamera && this.scopeBackgroundCamera.gameObject.activeInHierarchy;
			}
			set
			{
				if (this.scopeBackgroundCamera)
				{
					this.scopeBackgroundCamera.gameObject.SetActive(value);
				}
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x000CF819 File Offset: 0x000CDA19
		// (set) Token: 0x060024F6 RID: 9462 RVA: 0x000CF835 File Offset: 0x000CDA35
		public bool isValid
		{
			get
			{
				return this.currentAimCanvas && this.currentAimCanvas.isValid;
			}
			set
			{
				this.currentAimCanvas.isValid = value;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000CF843 File Offset: 0x000CDA43
		// (set) Token: 0x060024F8 RID: 9464 RVA: 0x000CF85F File Offset: 0x000CDA5F
		public bool isAimActive
		{
			get
			{
				return this.currentAimCanvas && this.currentAimCanvas.isAimActive;
			}
			set
			{
				this.currentAimCanvas.isAimActive = value;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x000CF86D File Offset: 0x000CDA6D
		// (set) Token: 0x060024FA RID: 9466 RVA: 0x000CF889 File Offset: 0x000CDA89
		public bool isScopeUIActive
		{
			get
			{
				return this.currentAimCanvas && this.currentAimCanvas.isScopeUIActive;
			}
			set
			{
				this.currentAimCanvas.isScopeUIActive = value;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x000CF897 File Offset: 0x000CDA97
		// (set) Token: 0x060024FC RID: 9468 RVA: 0x000CF8B3 File Offset: 0x000CDAB3
		public bool useScopeTransition
		{
			get
			{
				return this.currentAimCanvas && this.currentAimCanvas.useScopeTransition;
			}
			set
			{
				this.currentAimCanvas.useScopeTransition = value;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x000CF8C1 File Offset: 0x000CDAC1
		protected bool scaleAimWithMovement
		{
			get
			{
				return this.currentAimCanvas && this.currentAimCanvas.scaleAimWithMovement;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x000CF8DD File Offset: 0x000CDADD
		protected float movementSensibility
		{
			get
			{
				return this.currentAimCanvas.movementSensibility;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x000CF8EA File Offset: 0x000CDAEA
		protected float scaleWithMovement
		{
			get
			{
				return this.currentAimCanvas.scaleWithMovement;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x000CF8F7 File Offset: 0x000CDAF7
		protected float smoothChangeScale
		{
			get
			{
				return this.currentAimCanvas.smoothChangeScale;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x000CF904 File Offset: 0x000CDB04
		protected float smoothTransitionIn
		{
			get
			{
				return this.currentAimCanvas.smoothTransitionIn;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x000CF911 File Offset: 0x000CDB11
		protected float smoothTransitionOut
		{
			get
			{
				return this.currentAimCanvas.smoothTransitionOut;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002503 RID: 9475 RVA: 0x000CF91E File Offset: 0x000CDB1E
		protected RectTransform aimTarget
		{
			get
			{
				return this.currentAimCanvas.aimTarget;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x000CF92B File Offset: 0x000CDB2B
		protected RectTransform aimCenter
		{
			get
			{
				return this.currentAimCanvas.aimCenter;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x000CF938 File Offset: 0x000CDB38
		protected Vector2 sizeDeltaTarget
		{
			get
			{
				return this.currentAimCanvas.sizeDeltaTarget;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000CF945 File Offset: 0x000CDB45
		protected Vector2 sizeDeltaCenter
		{
			get
			{
				return this.currentAimCanvas.sizeDeltaCenter;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002507 RID: 9479 RVA: 0x000CF952 File Offset: 0x000CDB52
		protected UnityEvent onEnableAim
		{
			get
			{
				return this.currentAimCanvas.onEnableAim;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000CF95F File Offset: 0x000CDB5F
		protected UnityEvent onDisableAim
		{
			get
			{
				return this.currentAimCanvas.onDisableAim;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002509 RID: 9481 RVA: 0x000CF96C File Offset: 0x000CDB6C
		protected UnityEvent onCheckvalidAim
		{
			get
			{
				return this.currentAimCanvas.onCheckvalidAim;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x0600250A RID: 9482 RVA: 0x000CF979 File Offset: 0x000CDB79
		protected UnityEvent onCheckInvalidAim
		{
			get
			{
				return this.currentAimCanvas.onCheckInvalidAim;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x000CF986 File Offset: 0x000CDB86
		public UnityEvent onEnableScopeCamera
		{
			get
			{
				return this.currentAimCanvas.onEnableScopeCamera;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x000CF993 File Offset: 0x000CDB93
		public UnityEvent onDisableScopeCamera
		{
			get
			{
				return this.currentAimCanvas.onDisableScopeCamera;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x000CF9A0 File Offset: 0x000CDBA0
		protected UnityEvent onEnableScopeUI
		{
			get
			{
				return this.currentAimCanvas.onEnableScopeUI;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x000CF9AD File Offset: 0x000CDBAD
		protected UnityEvent onDisableScopeUI
		{
			get
			{
				return this.currentAimCanvas.onDisableScopeUI;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x000CF9BA File Offset: 0x000CDBBA
		// (set) Token: 0x06002510 RID: 9488 RVA: 0x000CF9C2 File Offset: 0x000CDBC2
		protected virtual float scopeCameraTransformWeight { get; set; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000CF9CB File Offset: 0x000CDBCB
		// (set) Token: 0x06002512 RID: 9490 RVA: 0x000CF9D3 File Offset: 0x000CDBD3
		protected virtual bool scopeActive { get; set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x000CF9DC File Offset: 0x000CDBDC
		private float scopeCameraOriginZoom
		{
			get
			{
				return this.mainCamera.fieldOfView;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002514 RID: 9492 RVA: 0x000CF9E9 File Offset: 0x000CDBE9
		private Quaternion scopeCameraOriginRot
		{
			get
			{
				return this.mainCamera.transform.rotation;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000CF9FB File Offset: 0x000CDBFB
		private Vector3 scopeCameraOriginPos
		{
			get
			{
				return this.mainCamera.transform.position;
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000CFA10 File Offset: 0x000CDC10
		public virtual void Init(vThirdPersonController cc)
		{
			if (this.scopeBackgroundCamera == null)
			{
				this.scopeBackgroundCamera = base.GetComponentInChildren<Camera>(true);
			}
			if (this.scopeBackgroundCamera == null)
			{
				Debug.LogWarning("Could not find Scope Background Camera. Please assign ScopeBackgroundCamera of Control aim canvas", base.gameObject);
			}
			this.mainCamera = Camera.main;
			vControlAimCanvas.instance = this;
			this.cc = cc;
			this.currentAimCanvas = this.aimCanvasCollection[this.currentCanvasID];
			this.isValid = true;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000CFA8C File Offset: 0x000CDC8C
		public void UpdateScopeCameraTransition()
		{
			this.scopeBackgroundCamera.transform.position = Vector3.Lerp(this.scopeCameraOriginPos, this.scopeCameraTargetPos, this.scopeCameraTransformWeight);
			if (this.scopeCameraTargetDir.magnitude > 0.01f)
			{
				this.scopeBackgroundCamera.transform.rotation = Quaternion.Lerp(this.scopeCameraOriginRot, Quaternion.LookRotation(this.scopeCameraTargetDir, this.scopeCameraUpDir), this.scopeCameraTransformWeight);
			}
			this.scopeBackgroundCamera.fieldOfView = Mathf.Lerp(this.scopeCameraOriginZoom, this.scopeCameraTargetZoom, this.scopeCameraTransformWeight);
			if (this.useScopeTransition)
			{
				if (this.scopeActive)
				{
					this.scopeCameraTransformWeight = Mathf.Clamp01(this.scopeCameraTransformWeight += this.smoothTransitionIn * Time.fixedDeltaTime);
				}
				else
				{
					this.scopeCameraTransformWeight = Mathf.Clamp01(this.scopeCameraTransformWeight -= this.smoothTransitionOut * Time.fixedDeltaTime);
				}
			}
			else
			{
				this.scopeCameraTransformWeight = (this.scopeActive ? 1f : 0f);
			}
			if (this.scopeActive && this.scopeCameraTransformWeight > 0f && !this.isScopeCameraActive)
			{
				this.isScopeCameraActive = true;
				this.mainCamera.enabled = false;
				this.onEnableScopeCamera.Invoke();
				return;
			}
			if (!this.scopeActive && this.scopeCameraTransformWeight == 0f && this.isScopeCameraActive)
			{
				this.mainCamera.enabled = true;
				this.isScopeCameraActive = false;
				this.onDisableScopeCamera.Invoke();
			}
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000CFC1C File Offset: 0x000CDE1C
		public void SetAimToCenter(bool validPoint = true)
		{
			if (this.currentAimCanvas == null)
			{
				return;
			}
			if (validPoint != this.isValid)
			{
				this.isValid = validPoint;
				if (this.isValid)
				{
					this.onCheckvalidAim.Invoke();
				}
				else
				{
					this.onCheckInvalidAim.Invoke();
				}
			}
			if (this.aimTarget)
			{
				this.aimTarget.anchoredPosition = Vector2.Lerp(this.aimTarget.anchoredPosition, Vector2.zero, this.useAimCorrectionSmooth ? (this.aimCorrectionSmooth * Time.fixedDeltaTime) : 1f);
			}
			if (this.currentAimCanvas.aimCenterToAimTarget && this.aimCenter)
			{
				this.aimCenter.anchoredPosition = Vector2.Lerp(this.aimCenter.anchoredPosition, Vector2.zero, this.useAimCorrectionSmooth ? (this.aimCorrectionSmooth * Time.fixedDeltaTime) : 1f);
			}
			this.UpdateAimSize();
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000CFD0C File Offset: 0x000CDF0C
		public void SetWordPosition(Vector3 wordPosition, bool validPoint = true)
		{
			if (this.currentAimCanvas == null)
			{
				return;
			}
			if (validPoint != this.isValid)
			{
				this.isValid = validPoint;
				if (this.isValid)
				{
					this.onCheckvalidAim.Invoke();
				}
				else
				{
					this.onCheckInvalidAim.Invoke();
				}
			}
			Vector2 vector = this.mainCamera.WorldToViewportPoint(wordPosition);
			Vector2 vector2 = new Vector2(vector.x * this.canvas.sizeDelta.x - this.canvas.sizeDelta.x * 0.5f, vector.y * this.canvas.sizeDelta.y - this.canvas.sizeDelta.y * 0.5f);
			if (this.currentAimCanvas.aimCenterToAimTarget && this.aimCenter)
			{
				this.aimCenter.anchoredPosition = Vector2.Lerp(this.aimCenter.anchoredPosition, vector2, this.useAimCorrectionSmooth ? (this.aimCorrectionSmooth * Time.fixedDeltaTime) : 1f);
			}
			if (this.aimTarget)
			{
				this.aimTarget.anchoredPosition = Vector2.Lerp(this.aimTarget.anchoredPosition, vector2, this.useAimCorrectionSmooth ? (this.aimCorrectionSmooth * Time.fixedDeltaTime) : 1f);
			}
			this.UpdateAimSize();
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000CFE68 File Offset: 0x000CE068
		public void SetWordPosition(Vector3 centerPosition, Vector3 targetPosition, bool validPoint = true)
		{
			if (this.currentAimCanvas == null)
			{
				return;
			}
			if (validPoint != this.isValid)
			{
				this.isValid = validPoint;
				if (this.isValid)
				{
					this.onCheckvalidAim.Invoke();
				}
				else
				{
					this.onCheckInvalidAim.Invoke();
				}
			}
			if (this.aimCenter)
			{
				Vector2 vector = this.mainCamera.WorldToViewportPoint(centerPosition);
				Vector2 vector2 = new Vector2(vector.x * this.canvas.sizeDelta.x - this.canvas.sizeDelta.x * 0.5f, vector.y * this.canvas.sizeDelta.y - this.canvas.sizeDelta.y * 0.5f);
				this.aimCenter.anchoredPosition = Vector2.Lerp(this.aimCenter.anchoredPosition, vector2, this.useAimCorrectionSmooth ? (this.aimCorrectionSmooth * Time.fixedDeltaTime) : 1f);
			}
			if (this.aimTarget)
			{
				Vector2 vector3 = this.mainCamera.WorldToViewportPoint(targetPosition);
				Vector2 vector4 = new Vector2(vector3.x * this.canvas.sizeDelta.x - this.canvas.sizeDelta.x * 0.5f, vector3.y * this.canvas.sizeDelta.y - this.canvas.sizeDelta.y * 0.5f);
				this.aimTarget.anchoredPosition = Vector2.Lerp(this.aimTarget.anchoredPosition, vector4, this.useAimCorrectionSmooth ? (this.aimCorrectionSmooth * Time.fixedDeltaTime) : 1f);
			}
			this.UpdateAimSize();
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000D0034 File Offset: 0x000CE234
		private void UpdateAimSize()
		{
			float num = 1f;
			if (this.scaleAimWithMovement && (this.cc.input.magnitude > this.movementSensibility || Mathf.Abs(Input.GetAxis("Mouse X")) > this.movementSensibility || Mathf.Abs(Input.GetAxis("Mouse Y")) > this.movementSensibility))
			{
				num = this.scaleWithMovement;
			}
			if (this.aimCenter)
			{
				this.aimCenter.sizeDelta = Vector2.Lerp(this.aimCenter.sizeDelta, this.sizeDeltaCenter * Mathf.Abs(num), this.smoothChangeScale * Time.fixedDeltaTime);
			}
			if (this.aimTarget)
			{
				this.aimTarget.sizeDelta = Vector2.Lerp(this.aimTarget.sizeDelta, this.sizeDeltaTarget * Mathf.Abs(num), this.smoothChangeScale * Time.fixedDeltaTime);
			}
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000D012C File Offset: 0x000CE32C
		public void SetActiveAim(bool value)
		{
			if (this.currentAimCanvas == null)
			{
				return;
			}
			if (value != this.isAimActive)
			{
				this.isAimActive = value;
				if (value)
				{
					this.isValid = true;
					this.onEnableAim.Invoke();
					return;
				}
				this.onDisableAim.Invoke();
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000D0179 File Offset: 0x000CE379
		public void DisableScopeCamera()
		{
			this.scopeActive = false;
			this.scopeCameraTransformWeight = 0f;
			this.isScopeCameraActive = false;
			this.mainCamera.enabled = true;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000D01A0 File Offset: 0x000CE3A0
		public void DisableAim()
		{
			this.SetActiveAim(false);
			this.DisableScopeCamera();
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000D01B0 File Offset: 0x000CE3B0
		public void SetActiveScopeCamera(bool value, bool useUI = false)
		{
			if (this.currentAimCanvas == null || !this.scopeBackgroundCamera)
			{
				return;
			}
			if (this.scopeActive != value || this.isScopeUIActive != useUI)
			{
				this.isScopeUIActive = useUI;
				if (value)
				{
					this.scopeActive = true;
					if (value && useUI)
					{
						this.onEnableScopeUI.Invoke();
						this.isScopeUIActive = true;
						return;
					}
					this.onDisableScopeUI.Invoke();
					this.isScopeUIActive = false;
					return;
				}
				else
				{
					this.scopeActive = false;
					this.isScopeUIActive = false;
					this.onDisableScopeUI.Invoke();
				}
			}
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000D0240 File Offset: 0x000CE440
		public void UpdateScopeCamera(Vector3 position, Vector3 lookDirection, Vector3 upDirection, float zoom = 60f, bool isAiming = false)
		{
			if (this.currentAimCanvas == null || !this.scopeBackgroundCamera)
			{
				return;
			}
			float num = Mathf.Clamp(60f - zoom, 1f, 179f);
			if (isAiming)
			{
				this.scopeCameraTargetPos = position;
				this.scopeCameraTargetDir = lookDirection;
				this.scopeCameraUpDir = upDirection;
				this.scopeCameraTargetZoom = num;
			}
			this.UpdateScopeCameraTransition();
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000D02A8 File Offset: 0x000CE4A8
		public void SetAimCanvasID(int id)
		{
			if (this.aimCanvasCollection.Count > 0 && this.currentCanvasID != id)
			{
				if (this.currentAimCanvas != null)
				{
					this.currentAimCanvas.DisableAll();
				}
				if (id < this.aimCanvasCollection.Count)
				{
					this.currentAimCanvas = this.aimCanvasCollection[id];
					this.currentCanvasID = id;
					return;
				}
				this.currentAimCanvas = this.aimCanvasCollection[0];
				this.currentCanvasID = 0;
			}
		}

		// Token: 0x040024C8 RID: 9416
		public static vControlAimCanvas instance;

		// Token: 0x040024C9 RID: 9417
		public RectTransform canvas;

		// Token: 0x040024CA RID: 9418
		public List<vAimCanvas> aimCanvasCollection = new List<vAimCanvas>();

		// Token: 0x040024CB RID: 9419
		public bool useAimCorrectionSmooth = true;

		// Token: 0x040024CC RID: 9420
		public float aimCorrectionSmooth = 20f;

		// Token: 0x040024CD RID: 9421
		public Camera scopeBackgroundCamera;

		// Token: 0x040024CE RID: 9422
		protected vThirdPersonController cc;

		// Token: 0x040024CF RID: 9423
		public vAimCanvas currentAimCanvas;

		// Token: 0x040024D0 RID: 9424
		protected int currentCanvasID;

		// Token: 0x040024D3 RID: 9427
		private float scopeCameraTargetZoom;

		// Token: 0x040024D4 RID: 9428
		private Vector3 scopeCameraTargetDir;

		// Token: 0x040024D5 RID: 9429
		private Vector3 scopeCameraUpDir;

		// Token: 0x040024D6 RID: 9430
		private Vector3 scopeCameraTargetPos;

		// Token: 0x040024D7 RID: 9431
		public Camera mainCamera;
	}
}
