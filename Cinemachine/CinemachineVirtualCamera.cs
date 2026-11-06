using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x0200001F RID: 31
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[ExcludeFromPreset]
	[AddComponentMenu("Cinemachine/CinemachineVirtualCamera")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineVirtualCamera.html")]
	public class CinemachineVirtualCamera : CinemachineVirtualCameraBase
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000B11A File Offset: 0x0000931A
		public override CameraState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000B122 File Offset: 0x00009322
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000B130 File Offset: 0x00009330
		public override Transform LookAt
		{
			get
			{
				return base.ResolveLookAt(this.m_LookAt);
			}
			set
			{
				this.m_LookAt = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000B139 File Offset: 0x00009339
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000B147 File Offset: 0x00009347
		public override Transform Follow
		{
			get
			{
				return base.ResolveFollow(this.m_Follow);
			}
			set
			{
				this.m_Follow = value;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B150 File Offset: 0x00009350
		public override float GetMaxDampTime()
		{
			float num = base.GetMaxDampTime();
			this.UpdateComponentPipeline();
			if (this.m_ComponentPipeline != null)
			{
				for (int i = 0; i < this.m_ComponentPipeline.Length; i++)
				{
					num = Mathf.Max(num, this.m_ComponentPipeline[i].GetMaxDampTime());
				}
			}
			return num;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000B19C File Offset: 0x0000939C
		public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			base.UpdateTargetCache();
			this.m_State = this.CalculateNewState(worldUp, deltaTime);
			base.ApplyPositionBlendMethod(ref this.m_State, this.m_Transitions.m_BlendHint);
			Vector3 rawPosition;
			Quaternion rawOrientation;
			base.transform.GetPositionAndRotation(out rawPosition, out rawOrientation);
			if (this.Follow != null)
			{
				rawPosition = this.m_State.RawPosition;
			}
			if (this.LookAt != null)
			{
				rawOrientation = this.m_State.RawOrientation;
			}
			base.transform.ConservativeSetPositionAndRotation(rawPosition, rawOrientation);
			this.PreviousStateIsValid = true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000B22C File Offset: 0x0000942C
		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_State = base.PullStateFromVirtualCamera(Vector3.up, ref this.m_Lens);
			this.InvalidateComponentPipeline();
			if (base.ValidatingStreamVersion < 20170927)
			{
				if (this.Follow != null && this.GetCinemachineComponent(CinemachineCore.Stage.Body) == null)
				{
					this.AddCinemachineComponent<CinemachineHardLockToTarget>();
				}
				if (this.LookAt != null && this.GetCinemachineComponent(CinemachineCore.Stage.Aim) == null)
				{
					this.AddCinemachineComponent<CinemachineHardLookAt>();
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000B2B4 File Offset: 0x000094B4
		protected override void OnDestroy()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.GetComponent<CinemachinePipeline>() != null)
				{
					transform.gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
				}
			}
			base.OnDestroy();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000B330 File Offset: 0x00009530
		protected override void OnValidate()
		{
			base.OnValidate();
			this.m_Lens.Validate();
			if (this.m_LegacyBlendHint != CinemachineVirtualCameraBase.BlendHint.None)
			{
				this.m_Transitions.m_BlendHint = this.m_LegacyBlendHint;
				this.m_LegacyBlendHint = CinemachineVirtualCameraBase.BlendHint.None;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B363 File Offset: 0x00009563
		private void OnTransformChildrenChanged()
		{
			this.InvalidateComponentPipeline();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000B36B File Offset: 0x0000956B
		private void Reset()
		{
			this.DestroyPipeline();
			this.UpdateComponentPipeline();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000B37C File Offset: 0x0000957C
		internal void DestroyPipeline()
		{
			List<Transform> list = new List<Transform>();
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.GetComponent<CinemachinePipeline>() != null)
				{
					list.Add(transform);
				}
			}
			foreach (Transform transform2 in list)
			{
				if (CinemachineVirtualCamera.DestroyPipelineOverride != null)
				{
					CinemachineVirtualCamera.DestroyPipelineOverride(transform2.gameObject);
				}
				else
				{
					CinemachineComponentBase[] components = transform2.GetComponents<CinemachineComponentBase>();
					for (int i = 0; i < components.Length; i++)
					{
						Object.Destroy(components[i]);
					}
					if (!RuntimeUtility.IsPrefab(base.gameObject))
					{
						Object.Destroy(transform2.gameObject);
					}
				}
			}
			this.m_ComponentOwner = null;
			this.InvalidateComponentPipeline();
			this.PreviousStateIsValid = false;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B490 File Offset: 0x00009690
		internal Transform CreatePipeline(CinemachineVirtualCamera copyFrom)
		{
			CinemachineComponentBase[] array = null;
			if (copyFrom != null)
			{
				copyFrom.InvalidateComponentPipeline();
				array = copyFrom.GetComponentPipeline();
			}
			Transform transform = null;
			if (CinemachineVirtualCamera.CreatePipelineOverride != null)
			{
				transform = CinemachineVirtualCamera.CreatePipelineOverride(this, "cm", array);
			}
			else if (!RuntimeUtility.IsPrefab(base.gameObject))
			{
				GameObject gameObject = new GameObject("cm");
				gameObject.transform.parent = base.transform;
				gameObject.AddComponent<CinemachinePipeline>();
				transform = gameObject.transform;
			}
			this.PreviousStateIsValid = false;
			return transform;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000B50F File Offset: 0x0000970F
		public void InvalidateComponentPipeline()
		{
			this.m_ComponentPipeline = null;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000B518 File Offset: 0x00009718
		public Transform GetComponentOwner()
		{
			this.UpdateComponentPipeline();
			return this.m_ComponentOwner;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000B526 File Offset: 0x00009726
		public CinemachineComponentBase[] GetComponentPipeline()
		{
			this.UpdateComponentPipeline();
			return this.m_ComponentPipeline;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000B534 File Offset: 0x00009734
		public CinemachineComponentBase GetCinemachineComponent(CinemachineCore.Stage stage)
		{
			CinemachineComponentBase[] componentPipeline = this.GetComponentPipeline();
			if (componentPipeline != null)
			{
				foreach (CinemachineComponentBase cinemachineComponentBase in componentPipeline)
				{
					if (cinemachineComponentBase.Stage == stage)
					{
						return cinemachineComponentBase;
					}
				}
			}
			return null;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000B56C File Offset: 0x0000976C
		public T GetCinemachineComponent<T>() where T : CinemachineComponentBase
		{
			CinemachineComponentBase[] componentPipeline = this.GetComponentPipeline();
			if (componentPipeline != null)
			{
				foreach (CinemachineComponentBase cinemachineComponentBase in componentPipeline)
				{
					if (cinemachineComponentBase is T)
					{
						return cinemachineComponentBase as T;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000B5B8 File Offset: 0x000097B8
		public T AddCinemachineComponent<T>() where T : CinemachineComponentBase
		{
			Transform componentOwner = this.GetComponentOwner();
			if (componentOwner == null)
			{
				return default(T);
			}
			CinemachineComponentBase[] components = componentOwner.GetComponents<CinemachineComponentBase>();
			T t = componentOwner.gameObject.AddComponent<T>();
			if (t != null && components != null)
			{
				CinemachineCore.Stage stage = t.Stage;
				for (int i = components.Length - 1; i >= 0; i--)
				{
					if (components[i].Stage == stage)
					{
						components[i].enabled = false;
						RuntimeUtility.DestroyObject(components[i]);
					}
				}
			}
			this.InvalidateComponentPipeline();
			return t;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B64C File Offset: 0x0000984C
		public void DestroyCinemachineComponent<T>() where T : CinemachineComponentBase
		{
			CinemachineComponentBase[] componentPipeline = this.GetComponentPipeline();
			if (componentPipeline != null)
			{
				foreach (CinemachineComponentBase cinemachineComponentBase in componentPipeline)
				{
					if (cinemachineComponentBase is T)
					{
						cinemachineComponentBase.enabled = false;
						RuntimeUtility.DestroyObject(cinemachineComponentBase);
						this.InvalidateComponentPipeline();
					}
				}
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000B694 File Offset: 0x00009894
		private void UpdateComponentPipeline()
		{
			if (this.m_ComponentOwner != null && this.m_ComponentPipeline != null)
			{
				return;
			}
			this.m_ComponentOwner = null;
			List<CinemachineComponentBase> list = new List<CinemachineComponentBase>();
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.GetComponent<CinemachinePipeline>() != null)
				{
					foreach (CinemachineComponentBase cinemachineComponentBase in transform.GetComponents<CinemachineComponentBase>())
					{
						if (cinemachineComponentBase.enabled)
						{
							list.Add(cinemachineComponentBase);
						}
					}
					this.m_ComponentOwner = transform;
					break;
				}
			}
			if (this.m_ComponentOwner == null)
			{
				this.m_ComponentOwner = this.CreatePipeline(null);
			}
			if (this.m_ComponentOwner != null && this.m_ComponentOwner.gameObject != null)
			{
				list.Sort((CinemachineComponentBase c1, CinemachineComponentBase c2) => c1.Stage - c2.Stage);
				this.m_ComponentPipeline = list.ToArray();
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000B7C4 File Offset: 0x000099C4
		internal static void SetFlagsForHiddenChild(GameObject child)
		{
			if (child != null)
			{
				if (CinemachineCore.sShowHiddenObjects)
				{
					child.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
					return;
				}
				child.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000B7F4 File Offset: 0x000099F4
		private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
		{
			this.FollowTargetAttachment = 1f;
			this.LookAtTargetAttachment = 1f;
			CameraState cameraState = base.PullStateFromVirtualCamera(worldUp, ref this.m_Lens);
			Transform lookAt = this.LookAt;
			if (lookAt != this.mCachedLookAtTarget)
			{
				this.mCachedLookAtTarget = lookAt;
				this.mCachedLookAtTargetVcam = null;
				if (lookAt != null)
				{
					this.mCachedLookAtTargetVcam = lookAt.GetComponent<CinemachineVirtualCameraBase>();
				}
			}
			if (lookAt != null)
			{
				if (this.mCachedLookAtTargetVcam != null)
				{
					cameraState.ReferenceLookAt = this.mCachedLookAtTargetVcam.State.FinalPosition;
				}
				else
				{
					cameraState.ReferenceLookAt = TargetPositionCache.GetTargetPosition(lookAt);
				}
			}
			this.UpdateComponentPipeline();
			base.InvokePrePipelineMutateCameraStateCallback(this, ref cameraState, deltaTime);
			bool flag = false;
			if (this.m_ComponentPipeline == null)
			{
				for (CinemachineCore.Stage stage = CinemachineCore.Stage.Body; stage <= CinemachineCore.Stage.Finalize; stage++)
				{
					base.InvokePostPipelineStageCallback(this, stage, ref cameraState, deltaTime);
				}
			}
			else
			{
				for (int i = 0; i < this.m_ComponentPipeline.Length; i++)
				{
					if (this.m_ComponentPipeline[i] != null)
					{
						this.m_ComponentPipeline[i].PrePipelineMutateCameraState(ref cameraState, deltaTime);
					}
				}
				int num = 0;
				CinemachineComponentBase cinemachineComponentBase = null;
				CinemachineCore.Stage stage2 = CinemachineCore.Stage.Body;
				while (stage2 <= CinemachineCore.Stage.Finalize)
				{
					CinemachineComponentBase cinemachineComponentBase2 = ((num < this.m_ComponentPipeline.Length) ? this.m_ComponentPipeline[num] : null);
					if (!(cinemachineComponentBase2 != null) || stage2 != cinemachineComponentBase2.Stage)
					{
						goto IL_0176;
					}
					num++;
					if (stage2 != CinemachineCore.Stage.Body || !cinemachineComponentBase2.BodyAppliesAfterAim)
					{
						cinemachineComponentBase2.MutateCameraState(ref cameraState, deltaTime);
						flag = stage2 == CinemachineCore.Stage.Aim;
						goto IL_0176;
					}
					cinemachineComponentBase = cinemachineComponentBase2;
					IL_01A6:
					stage2++;
					continue;
					IL_0176:
					base.InvokePostPipelineStageCallback(this, stage2, ref cameraState, deltaTime);
					if (stage2 == CinemachineCore.Stage.Aim && cinemachineComponentBase != null)
					{
						cinemachineComponentBase.MutateCameraState(ref cameraState, deltaTime);
						base.InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Body, ref cameraState, deltaTime);
						goto IL_01A6;
					}
					goto IL_01A6;
				}
			}
			if (!flag)
			{
				cameraState.BlendHint |= CameraState.BlendHintValue.IgnoreLookAtTarget;
			}
			return cameraState;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			if (target == this.Follow)
			{
				base.transform.position += positionDelta;
				this.m_State.RawPosition = this.m_State.RawPosition + positionDelta;
			}
			this.UpdateComponentPipeline();
			if (this.m_ComponentPipeline != null)
			{
				for (int i = 0; i < this.m_ComponentPipeline.Length; i++)
				{
					this.m_ComponentPipeline[i].OnTargetObjectWarped(target, positionDelta);
				}
			}
			base.OnTargetObjectWarped(target, positionDelta);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000BA50 File Offset: 0x00009C50
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			this.PreviousStateIsValid = true;
			base.transform.ConservativeSetPositionAndRotation(pos, rot);
			this.m_State.RawPosition = pos;
			this.m_State.RawOrientation = rot;
			this.UpdateComponentPipeline();
			if (this.m_ComponentPipeline != null)
			{
				for (int i = 0; i < this.m_ComponentPipeline.Length; i++)
				{
					this.m_ComponentPipeline[i].ForceCameraPosition(pos, rot);
				}
			}
			base.ForceCameraPosition(pos, rot);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000BAC1 File Offset: 0x00009CC1
		internal void SetStateRawPosition(Vector3 pos)
		{
			this.m_State.RawPosition = pos;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000BAD0 File Offset: 0x00009CD0
		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
			base.InvokeOnTransitionInExtensions(fromCam, worldUp, deltaTime);
			bool flag = false;
			if (this.m_Transitions.m_InheritPosition && fromCam != null && !CinemachineCore.Instance.IsLiveInBlend(this))
			{
				this.ForceCameraPosition(fromCam.State.FinalPosition, fromCam.State.FinalOrientation);
			}
			this.UpdateComponentPipeline();
			if (this.m_ComponentPipeline != null)
			{
				for (int i = 0; i < this.m_ComponentPipeline.Length; i++)
				{
					if (this.m_ComponentPipeline[i].OnTransitionFromCamera(fromCam, worldUp, deltaTime, ref this.m_Transitions))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.InternalUpdateCameraState(worldUp, deltaTime);
				this.InternalUpdateCameraState(worldUp, deltaTime);
			}
			else
			{
				base.UpdateCameraState(worldUp, deltaTime);
			}
			if (this.m_Transitions.m_OnCameraLive != null)
			{
				this.m_Transitions.m_OnCameraLive.Invoke(this, fromCam);
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		internal override bool RequiresUserInput()
		{
			if (base.RequiresUserInput())
			{
				return true;
			}
			if (this.m_ComponentPipeline != null)
			{
				return this.m_ComponentPipeline.Any<CinemachineComponentBase>((CinemachineComponentBase c) => c != null && c.RequiresUserInput);
			}
			return false;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		internal override void OnBeforeSerialize()
		{
			if (!this.m_Lens.IsPhysicalCamera)
			{
				this.m_Lens.SensorSize = Vector2.one;
			}
		}

		// Token: 0x040000E5 RID: 229
		[Tooltip("The object that the camera wants to look at (the Aim target).  If this is null, then the vcam's Transform orientation will define the camera's orientation.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_LookAt;

		// Token: 0x040000E6 RID: 230
		[Tooltip("The object that the camera wants to move with (the Body target).  If this is null, then the vcam's Transform position will define the camera's position.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_Follow;

		// Token: 0x040000E7 RID: 231
		[FormerlySerializedAs("m_LensAttributes")]
		[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active.")]
		public LensSettings m_Lens = LensSettings.Default;

		// Token: 0x040000E8 RID: 232
		public CinemachineVirtualCameraBase.TransitionParams m_Transitions;

		// Token: 0x040000E9 RID: 233
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_BlendHint")]
		[FormerlySerializedAs("m_PositionBlending")]
		private CinemachineVirtualCameraBase.BlendHint m_LegacyBlendHint;

		// Token: 0x040000EA RID: 234
		public const string PipelineName = "cm";

		// Token: 0x040000EB RID: 235
		public static CinemachineVirtualCamera.CreatePipelineDelegate CreatePipelineOverride;

		// Token: 0x040000EC RID: 236
		public static CinemachineVirtualCamera.DestroyPipelineDelegate DestroyPipelineOverride;

		// Token: 0x040000ED RID: 237
		private CameraState m_State = CameraState.Default;

		// Token: 0x040000EE RID: 238
		private CinemachineComponentBase[] m_ComponentPipeline;

		// Token: 0x040000EF RID: 239
		[SerializeField]
		[HideInInspector]
		private Transform m_ComponentOwner;

		// Token: 0x040000F0 RID: 240
		private Transform mCachedLookAtTarget;

		// Token: 0x040000F1 RID: 241
		private CinemachineVirtualCameraBase mCachedLookAtTargetVcam;

		// Token: 0x02000090 RID: 144
		// (Invoke) Token: 0x06000432 RID: 1074
		public delegate Transform CreatePipelineDelegate(CinemachineVirtualCamera vcam, string name, CinemachineComponentBase[] copyFrom);

		// Token: 0x02000091 RID: 145
		// (Invoke) Token: 0x06000436 RID: 1078
		public delegate void DestroyPipelineDelegate(GameObject pipeline);
	}
}
