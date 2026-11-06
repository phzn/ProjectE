using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000045 RID: 69
	[SaveDuringPlay]
	public abstract class CinemachineVirtualCameraBase : MonoBehaviour, ICinemachineCamera, ISerializationCallbackReceiver
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00012E15 File Offset: 0x00011015
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00012E2B File Offset: 0x0001102B
		public int ValidatingStreamVersion
		{
			get
			{
				if (!this.m_OnValidateCalled)
				{
					return CinemachineCore.kStreamingVersion;
				}
				return this.m_ValidatingStreamVersion;
			}
			private set
			{
				this.m_ValidatingStreamVersion = value;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00012E34 File Offset: 0x00011034
		public virtual float GetMaxDampTime()
		{
			float num = 0f;
			if (this.mExtensions != null)
			{
				for (int i = 0; i < this.mExtensions.Count; i++)
				{
					num = Mathf.Max(num, this.mExtensions[i].GetMaxDampTime());
				}
			}
			return num;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00012E7E File Offset: 0x0001107E
		public float DetachedFollowTargetDamp(float initial, float dampTime, float deltaTime)
		{
			dampTime = Mathf.Lerp(Mathf.Max(1f, dampTime), dampTime, this.FollowTargetAttachment);
			deltaTime = Mathf.Lerp(0f, deltaTime, this.FollowTargetAttachment);
			return Damper.Damp(initial, dampTime, deltaTime);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00012EB4 File Offset: 0x000110B4
		public Vector3 DetachedFollowTargetDamp(Vector3 initial, Vector3 dampTime, float deltaTime)
		{
			dampTime = Vector3.Lerp(Vector3.Max(Vector3.one, dampTime), dampTime, this.FollowTargetAttachment);
			deltaTime = Mathf.Lerp(0f, deltaTime, this.FollowTargetAttachment);
			return Damper.Damp(initial, dampTime, deltaTime);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00012EEA File Offset: 0x000110EA
		public Vector3 DetachedFollowTargetDamp(Vector3 initial, float dampTime, float deltaTime)
		{
			dampTime = Mathf.Lerp(Mathf.Max(1f, dampTime), dampTime, this.FollowTargetAttachment);
			deltaTime = Mathf.Lerp(0f, deltaTime, this.FollowTargetAttachment);
			return Damper.Damp(initial, dampTime, deltaTime);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00012F20 File Offset: 0x00011120
		public float DetachedLookAtTargetDamp(float initial, float dampTime, float deltaTime)
		{
			dampTime = Mathf.Lerp(Mathf.Max(1f, dampTime), dampTime, this.LookAtTargetAttachment);
			deltaTime = Mathf.Lerp(0f, deltaTime, this.LookAtTargetAttachment);
			return Damper.Damp(initial, dampTime, deltaTime);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00012F56 File Offset: 0x00011156
		public Vector3 DetachedLookAtTargetDamp(Vector3 initial, Vector3 dampTime, float deltaTime)
		{
			dampTime = Vector3.Lerp(Vector3.Max(Vector3.one, dampTime), dampTime, this.LookAtTargetAttachment);
			deltaTime = Mathf.Lerp(0f, deltaTime, this.LookAtTargetAttachment);
			return Damper.Damp(initial, dampTime, deltaTime);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00012F8C File Offset: 0x0001118C
		public Vector3 DetachedLookAtTargetDamp(Vector3 initial, float dampTime, float deltaTime)
		{
			dampTime = Mathf.Lerp(Mathf.Max(1f, dampTime), dampTime, this.LookAtTargetAttachment);
			deltaTime = Mathf.Lerp(0f, deltaTime, this.LookAtTargetAttachment);
			return Damper.Damp(initial, dampTime, deltaTime);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00012FC2 File Offset: 0x000111C2
		public virtual void AddExtension(CinemachineExtension extension)
		{
			if (this.mExtensions == null)
			{
				this.mExtensions = new List<CinemachineExtension>();
			}
			else
			{
				this.mExtensions.Remove(extension);
			}
			this.mExtensions.Add(extension);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00012FF2 File Offset: 0x000111F2
		public virtual void RemoveExtension(CinemachineExtension extension)
		{
			if (this.mExtensions != null)
			{
				this.mExtensions.Remove(extension);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00013009 File Offset: 0x00011209
		// (set) Token: 0x060002DC RID: 732 RVA: 0x00013011 File Offset: 0x00011211
		internal List<CinemachineExtension> mExtensions { get; private set; }

		// Token: 0x060002DD RID: 733 RVA: 0x0001301C File Offset: 0x0001121C
		protected void InvokePostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState newState, float deltaTime)
		{
			if (this.mExtensions != null)
			{
				for (int i = 0; i < this.mExtensions.Count; i++)
				{
					CinemachineExtension cinemachineExtension = this.mExtensions[i];
					if (cinemachineExtension == null)
					{
						this.mExtensions.RemoveAt(i);
						i--;
					}
					else if (cinemachineExtension.enabled)
					{
						cinemachineExtension.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
					}
				}
			}
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = this.ParentCamera as CinemachineVirtualCameraBase;
			if (cinemachineVirtualCameraBase != null)
			{
				cinemachineVirtualCameraBase.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000130A4 File Offset: 0x000112A4
		protected void InvokePrePipelineMutateCameraStateCallback(CinemachineVirtualCameraBase vcam, ref CameraState newState, float deltaTime)
		{
			if (this.mExtensions != null)
			{
				for (int i = 0; i < this.mExtensions.Count; i++)
				{
					CinemachineExtension cinemachineExtension = this.mExtensions[i];
					if (cinemachineExtension == null)
					{
						this.mExtensions.RemoveAt(i);
						i--;
					}
					else if (cinemachineExtension.enabled)
					{
						cinemachineExtension.PrePipelineMutateCameraStateCallback(vcam, ref newState, deltaTime);
					}
				}
			}
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = this.ParentCamera as CinemachineVirtualCameraBase;
			if (cinemachineVirtualCameraBase != null)
			{
				cinemachineVirtualCameraBase.InvokePrePipelineMutateCameraStateCallback(vcam, ref newState, deltaTime);
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00013128 File Offset: 0x00011328
		protected bool InvokeOnTransitionInExtensions(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			bool flag = false;
			if (this.mExtensions != null)
			{
				for (int i = 0; i < this.mExtensions.Count; i++)
				{
					CinemachineExtension cinemachineExtension = this.mExtensions[i];
					if (cinemachineExtension == null)
					{
						this.mExtensions.RemoveAt(i);
						i--;
					}
					else if (cinemachineExtension.enabled && cinemachineExtension.OnTransitionFromCamera(fromCam, worldUp, deltaTime))
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00013193 File Offset: 0x00011393
		public string Name
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0001319B File Offset: 0x0001139B
		public virtual string Description
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x000131A2 File Offset: 0x000113A2
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x000131AA File Offset: 0x000113AA
		public int Priority
		{
			get
			{
				return this.m_Priority;
			}
			set
			{
				this.m_Priority = value;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000131B3 File Offset: 0x000113B3
		protected void ApplyPositionBlendMethod(ref CameraState state, CinemachineVirtualCameraBase.BlendHint hint)
		{
			switch (hint)
			{
			case CinemachineVirtualCameraBase.BlendHint.SphericalPosition:
				state.BlendHint |= CameraState.BlendHintValue.SphericalPositionBlend;
				return;
			case CinemachineVirtualCameraBase.BlendHint.CylindricalPosition:
				state.BlendHint |= CameraState.BlendHintValue.CylindricalPositionBlend;
				return;
			case CinemachineVirtualCameraBase.BlendHint.ScreenSpaceAimWhenTargetsDiffer:
				state.BlendHint |= CameraState.BlendHintValue.RadialAimBlend;
				return;
			default:
				return;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x000131EE File Offset: 0x000113EE
		public GameObject VirtualCameraGameObject
		{
			get
			{
				if (this == null)
				{
					return null;
				}
				return base.gameObject;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00013201 File Offset: 0x00011401
		public bool IsValid
		{
			get
			{
				return !(this == null);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002E7 RID: 743
		public abstract CameraState State { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0001320D File Offset: 0x0001140D
		public ICinemachineCamera ParentCamera
		{
			get
			{
				if (!this.mSlaveStatusUpdated || !Application.isPlaying)
				{
					this.UpdateSlaveStatus();
				}
				return this.m_parentVcam;
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001322A File Offset: 0x0001142A
		public virtual bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			return false;
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002EA RID: 746
		// (set) Token: 0x060002EB RID: 747
		public abstract Transform LookAt { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002EC RID: 748
		// (set) Token: 0x060002ED RID: 749
		public abstract Transform Follow { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0001322D File Offset: 0x0001142D
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00013235 File Offset: 0x00011435
		public virtual bool PreviousStateIsValid { get; set; }

		// Token: 0x060002F0 RID: 752 RVA: 0x0001323E File Offset: 0x0001143E
		public void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			CinemachineCore.Instance.UpdateVirtualCamera(this, worldUp, deltaTime);
		}

		// Token: 0x060002F1 RID: 753
		public abstract void InternalUpdateCameraState(Vector3 worldUp, float deltaTime);

		// Token: 0x060002F2 RID: 754 RVA: 0x0001324D File Offset: 0x0001144D
		public virtual void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				this.PreviousStateIsValid = false;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00013263 File Offset: 0x00011463
		protected virtual void OnDestroy()
		{
			CinemachineCore.Instance.CameraDestroyed(this);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00013270 File Offset: 0x00011470
		protected virtual void OnTransformParentChanged()
		{
			CinemachineCore.Instance.CameraDisabled(this);
			CinemachineCore.Instance.CameraEnabled(this);
			this.UpdateSlaveStatus();
			this.UpdateVcamPoolStatus();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00013294 File Offset: 0x00011494
		protected virtual void Start()
		{
			this.m_WasStarted = true;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0001329D File Offset: 0x0001149D
		internal virtual bool RequiresUserInput()
		{
			if (this.mExtensions != null)
			{
				return this.mExtensions.Any<CinemachineExtension>((CinemachineExtension extension) => extension != null && extension.RequiresUserInput);
			}
			return false;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000132D4 File Offset: 0x000114D4
		internal void EnsureStarted()
		{
			if (!this.m_WasStarted)
			{
				this.m_WasStarted = true;
				CinemachineExtension[] componentsInChildren = base.GetComponentsInChildren<CinemachineExtension>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].EnsureStarted();
				}
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00013310 File Offset: 0x00011510
		public AxisState.IInputAxisProvider GetInputAxisProvider()
		{
			MonoBehaviour[] componentsInChildren = base.GetComponentsInChildren<MonoBehaviour>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				AxisState.IInputAxisProvider inputAxisProvider = componentsInChildren[i] as AxisState.IInputAxisProvider;
				if (inputAxisProvider != null)
				{
					return inputAxisProvider;
				}
			}
			return null;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00013341 File Offset: 0x00011541
		protected virtual void OnValidate()
		{
			this.m_OnValidateCalled = true;
			this.ValidatingStreamVersion = this.m_StreamingVersion;
			this.m_StreamingVersion = CinemachineCore.kStreamingVersion;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00013364 File Offset: 0x00011564
		protected virtual void OnEnable()
		{
			this.UpdateSlaveStatus();
			this.UpdateVcamPoolStatus();
			if (!CinemachineCore.Instance.IsLive(this))
			{
				this.PreviousStateIsValid = false;
			}
			CinemachineCore.Instance.CameraEnabled(this);
			this.InvalidateCachedTargets();
			CinemachineVirtualCameraBase[] components = base.GetComponents<CinemachineVirtualCameraBase>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].enabled && components[i] != this)
				{
					Debug.LogError(this.Name + " has multiple CinemachineVirtualCameraBase-derived components.  Disabling " + base.GetType().Name + ".");
					base.enabled = false;
				}
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000133F8 File Offset: 0x000115F8
		protected virtual void OnDisable()
		{
			this.UpdateVcamPoolStatus();
			CinemachineCore.Instance.CameraDisabled(this);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001340B File Offset: 0x0001160B
		protected virtual void Update()
		{
			if (this.m_Priority != this.m_QueuePriority)
			{
				this.UpdateVcamPoolStatus();
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00013424 File Offset: 0x00011624
		private void UpdateSlaveStatus()
		{
			this.mSlaveStatusUpdated = true;
			this.m_parentVcam = null;
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				parent.TryGetComponent<CinemachineVirtualCameraBase>(out this.m_parentVcam);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00013464 File Offset: 0x00011664
		public Transform ResolveLookAt(Transform localLookAt)
		{
			Transform transform = localLookAt;
			if (transform == null && this.ParentCamera != null)
			{
				transform = this.ParentCamera.LookAt;
			}
			return transform;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00013494 File Offset: 0x00011694
		public Transform ResolveFollow(Transform localFollow)
		{
			Transform transform = localFollow;
			if (transform == null && this.ParentCamera != null)
			{
				transform = this.ParentCamera.Follow;
			}
			return transform;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000134C1 File Offset: 0x000116C1
		private void UpdateVcamPoolStatus()
		{
			CinemachineCore.Instance.RemoveActiveCamera(this);
			if (this.m_parentVcam == null && base.isActiveAndEnabled)
			{
				CinemachineCore.Instance.AddActiveCamera(this);
			}
			this.m_QueuePriority = this.m_Priority;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000134FB File Offset: 0x000116FB
		public void MoveToTopOfPrioritySubqueue()
		{
			this.UpdateVcamPoolStatus();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00013504 File Offset: 0x00011704
		public virtual void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			if (this.mExtensions != null)
			{
				for (int i = 0; i < this.mExtensions.Count; i++)
				{
					this.mExtensions[i].OnTargetObjectWarped(target, positionDelta);
				}
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00013544 File Offset: 0x00011744
		public virtual void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			if (this.mExtensions != null)
			{
				for (int i = 0; i < this.mExtensions.Count; i++)
				{
					this.mExtensions[i].ForceCameraPosition(pos, rot);
				}
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00013582 File Offset: 0x00011782
		private bool GetInheritPosition(ICinemachineCamera cam)
		{
			if (cam is CinemachineVirtualCamera)
			{
				return (cam as CinemachineVirtualCamera).m_Transitions.m_InheritPosition;
			}
			return cam is CinemachineFreeLook && (cam as CinemachineFreeLook).m_Transitions.m_InheritPosition;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000135B8 File Offset: 0x000117B8
		protected CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, CinemachineBlendDefinition blendDef, CinemachineBlend activeBlend)
		{
			if (blendDef.BlendCurve == null || blendDef.BlendTime <= 0f || (camA == null && camB == null))
			{
				this.m_blendStartPosition = 0f;
				return null;
			}
			if (activeBlend != null)
			{
				if (activeBlend != null && !activeBlend.IsComplete && activeBlend.CamA == camB && activeBlend.CamB == camA)
				{
					float num = this.m_blendStartPosition + (1f - this.m_blendStartPosition) * activeBlend.TimeInBlend / activeBlend.Duration;
					blendDef.m_Time *= num;
					this.m_blendStartPosition = 1f - num;
				}
				else
				{
					this.m_blendStartPosition = 0f;
				}
				if (this.GetInheritPosition(camB))
				{
					camA = null;
				}
				else
				{
					camA = new BlendSourceVirtualCamera(activeBlend);
				}
			}
			if (camA == null)
			{
				camA = new StaticPointVirtualCamera(this.State, "(none)");
			}
			return new CinemachineBlend(camA, camB, blendDef.BlendCurve, blendDef.BlendTime, 0f);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000136A8 File Offset: 0x000118A8
		protected CameraState PullStateFromVirtualCamera(Vector3 worldUp, ref LensSettings lens)
		{
			CameraState @default = CameraState.Default;
			@default.RawPosition = TargetPositionCache.GetTargetPosition(base.transform);
			@default.RawOrientation = TargetPositionCache.GetTargetRotation(base.transform);
			@default.ReferenceUp = worldUp;
			CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this);
			if (cinemachineBrain != null)
			{
				lens.SnapshotCameraReadOnlyProperties(cinemachineBrain.OutputCamera);
			}
			@default.Lens = lens;
			return @default;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00013716 File Offset: 0x00011916
		private void InvalidateCachedTargets()
		{
			this.m_CachedFollowTarget = null;
			this.m_CachedFollowTargetVcam = null;
			this.m_CachedFollowTargetGroup = null;
			this.m_CachedLookAtTarget = null;
			this.m_CachedLookAtTargetVcam = null;
			this.m_CachedLookAtTargetGroup = null;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00013742 File Offset: 0x00011942
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0001374A File Offset: 0x0001194A
		public bool FollowTargetChanged { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00013753 File Offset: 0x00011953
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0001375B File Offset: 0x0001195B
		public bool LookAtTargetChanged { get; private set; }

		// Token: 0x0600030C RID: 780 RVA: 0x00013764 File Offset: 0x00011964
		protected void UpdateTargetCache()
		{
			Transform transform = this.ResolveFollow(this.Follow);
			this.FollowTargetChanged = transform != this.m_CachedFollowTarget;
			if (this.FollowTargetChanged)
			{
				this.m_CachedFollowTarget = transform;
				this.m_CachedFollowTargetVcam = null;
				this.m_CachedFollowTargetGroup = null;
				if (this.m_CachedFollowTarget != null)
				{
					transform.TryGetComponent<CinemachineVirtualCameraBase>(out this.m_CachedFollowTargetVcam);
					transform.TryGetComponent<ICinemachineTargetGroup>(out this.m_CachedFollowTargetGroup);
				}
			}
			transform = this.ResolveLookAt(this.LookAt);
			this.LookAtTargetChanged = transform != this.m_CachedLookAtTarget;
			if (this.LookAtTargetChanged)
			{
				this.m_CachedLookAtTarget = transform;
				this.m_CachedLookAtTargetVcam = null;
				this.m_CachedLookAtTargetGroup = null;
				if (transform != null)
				{
					transform.TryGetComponent<CinemachineVirtualCameraBase>(out this.m_CachedLookAtTargetVcam);
					transform.TryGetComponent<ICinemachineTargetGroup>(out this.m_CachedLookAtTargetGroup);
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00013834 File Offset: 0x00011A34
		public ICinemachineTargetGroup AbstractFollowTargetGroup
		{
			get
			{
				return this.m_CachedFollowTargetGroup;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0001383C File Offset: 0x00011A3C
		public CinemachineVirtualCameraBase FollowTargetAsVcam
		{
			get
			{
				return this.m_CachedFollowTargetVcam;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00013844 File Offset: 0x00011A44
		public ICinemachineTargetGroup AbstractLookAtTargetGroup
		{
			get
			{
				return this.m_CachedLookAtTargetGroup;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0001384C File Offset: 0x00011A4C
		public CinemachineVirtualCameraBase LookAtTargetAsVcam
		{
			get
			{
				return this.m_CachedLookAtTargetVcam;
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00013854 File Offset: 0x00011A54
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.OnBeforeSerialize();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0001385C File Offset: 0x00011A5C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.m_StreamingVersion < CinemachineCore.kStreamingVersion)
			{
				this.LegacyUpgrade(this.m_StreamingVersion);
			}
			this.m_StreamingVersion = CinemachineCore.kStreamingVersion;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00013882 File Offset: 0x00011A82
		protected internal virtual void LegacyUpgrade(int streamedVersion)
		{
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00013884 File Offset: 0x00011A84
		internal virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00013888 File Offset: 0x00011A88
		public void CancelDamping(bool updateNow = false)
		{
			this.PreviousStateIsValid = false;
			if (updateNow)
			{
				Vector3 vector = this.State.ReferenceUp;
				CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this);
				if (cinemachineBrain != null)
				{
					vector = cinemachineBrain.DefaultWorldUp;
				}
				this.InternalUpdateCameraState(vector, -1f);
			}
		}

		// Token: 0x040001F6 RID: 502
		[HideInInspector]
		[SerializeField]
		[NoSaveDuringPlay]
		public string[] m_ExcludedPropertiesInInspector = new string[] { "m_Script" };

		// Token: 0x040001F7 RID: 503
		[HideInInspector]
		[SerializeField]
		[NoSaveDuringPlay]
		public CinemachineCore.Stage[] m_LockStageInInspector;

		// Token: 0x040001F8 RID: 504
		private int m_ValidatingStreamVersion;

		// Token: 0x040001F9 RID: 505
		private bool m_OnValidateCalled;

		// Token: 0x040001FA RID: 506
		[HideInInspector]
		[SerializeField]
		[NoSaveDuringPlay]
		private int m_StreamingVersion;

		// Token: 0x040001FB RID: 507
		[NoSaveDuringPlay]
		[Tooltip("The priority will determine which camera becomes active based on the state of other cameras and this camera.  Higher numbers have greater priority.")]
		public int m_Priority = 10;

		// Token: 0x040001FC RID: 508
		internal int m_ActivationId;

		// Token: 0x040001FD RID: 509
		[NonSerialized]
		public float FollowTargetAttachment;

		// Token: 0x040001FE RID: 510
		[NonSerialized]
		public float LookAtTargetAttachment;

		// Token: 0x040001FF RID: 511
		[Tooltip("When the virtual camera is not live, this is how often the virtual camera will be updated.  Set this to tune for performance. Most of the time Never is fine, unless the virtual camera is doing shot evaluation.")]
		public CinemachineVirtualCameraBase.StandbyUpdateMode m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.RoundRobin;

		// Token: 0x04000202 RID: 514
		private bool m_WasStarted;

		// Token: 0x04000203 RID: 515
		private bool mSlaveStatusUpdated;

		// Token: 0x04000204 RID: 516
		private CinemachineVirtualCameraBase m_parentVcam;

		// Token: 0x04000205 RID: 517
		private int m_QueuePriority = int.MaxValue;

		// Token: 0x04000206 RID: 518
		private float m_blendStartPosition;

		// Token: 0x04000207 RID: 519
		private Transform m_CachedFollowTarget;

		// Token: 0x04000208 RID: 520
		private CinemachineVirtualCameraBase m_CachedFollowTargetVcam;

		// Token: 0x04000209 RID: 521
		private ICinemachineTargetGroup m_CachedFollowTargetGroup;

		// Token: 0x0400020A RID: 522
		private Transform m_CachedLookAtTarget;

		// Token: 0x0400020B RID: 523
		private CinemachineVirtualCameraBase m_CachedLookAtTargetVcam;

		// Token: 0x0400020C RID: 524
		private ICinemachineTargetGroup m_CachedLookAtTargetGroup;

		// Token: 0x020000B0 RID: 176
		public enum StandbyUpdateMode
		{
			// Token: 0x0400038E RID: 910
			Never,
			// Token: 0x0400038F RID: 911
			Always,
			// Token: 0x04000390 RID: 912
			RoundRobin
		}

		// Token: 0x020000B1 RID: 177
		public enum BlendHint
		{
			// Token: 0x04000392 RID: 914
			None,
			// Token: 0x04000393 RID: 915
			SphericalPosition,
			// Token: 0x04000394 RID: 916
			CylindricalPosition,
			// Token: 0x04000395 RID: 917
			ScreenSpaceAimWhenTargetsDiffer
		}

		// Token: 0x020000B2 RID: 178
		[Serializable]
		public struct TransitionParams
		{
			// Token: 0x04000396 RID: 918
			[Tooltip("Hint for blending positions to and from this virtual camera")]
			[FormerlySerializedAs("m_PositionBlending")]
			public CinemachineVirtualCameraBase.BlendHint m_BlendHint;

			// Token: 0x04000397 RID: 919
			[Tooltip("When this virtual camera goes Live, attempt to force the position to be the same as the current position of the Unity Camera")]
			public bool m_InheritPosition;

			// Token: 0x04000398 RID: 920
			[Tooltip("This event fires when the virtual camera goes Live")]
			public CinemachineBrain.VcamActivatedEvent m_OnCameraLive;
		}
	}
}
