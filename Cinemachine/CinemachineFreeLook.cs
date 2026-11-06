using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000015 RID: 21
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[ExcludeFromPreset]
	[AddComponentMenu("Cinemachine/CinemachineFreeLook")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineFreeLook.html")]
	public class CinemachineFreeLook : CinemachineVirtualCameraBase
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00006BC4 File Offset: 0x00004DC4
		protected override void OnValidate()
		{
			base.OnValidate();
			if (this.m_LegacyHeadingBias != 3.4028235E+38f)
			{
				this.m_Heading.m_Bias = this.m_LegacyHeadingBias;
				this.m_LegacyHeadingBias = float.MaxValue;
				int definition = (int)this.m_Heading.m_Definition;
				if (this.m_RecenterToTargetHeading.LegacyUpgrade(ref definition, ref this.m_Heading.m_VelocityFilterStrength))
				{
					this.m_Heading.m_Definition = (CinemachineOrbitalTransposer.Heading.HeadingDefinition)definition;
				}
				this.mUseLegacyRigDefinitions = true;
			}
			if (this.m_LegacyBlendHint != CinemachineVirtualCameraBase.BlendHint.None)
			{
				this.m_Transitions.m_BlendHint = this.m_LegacyBlendHint;
				this.m_LegacyBlendHint = CinemachineVirtualCameraBase.BlendHint.None;
			}
			this.m_YAxis.Validate();
			this.m_XAxis.Validate();
			this.m_RecenterToTargetHeading.Validate();
			this.m_YAxisRecentering.Validate();
			this.m_Lens.Validate();
			this.InvalidateRigCache();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006C96 File Offset: 0x00004E96
		public CinemachineVirtualCamera GetRig(int i)
		{
			if (!this.UpdateRigCache() || i < 0 || i >= 3)
			{
				return null;
			}
			return this.m_Rigs[i];
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00006CB2 File Offset: 0x00004EB2
		internal bool RigsAreCreated
		{
			get
			{
				return this.m_Rigs != null && this.m_Rigs.Length == 3;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00006CC9 File Offset: 0x00004EC9
		public static string[] RigNames
		{
			get
			{
				return new string[] { "TopRig", "MiddleRig", "BottomRig" };
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00006CE9 File Offset: 0x00004EE9
		protected override void OnEnable()
		{
			this.mIsDestroyed = false;
			base.OnEnable();
			this.InvalidateRigCache();
			this.UpdateInputAxisProvider();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006D04 File Offset: 0x00004F04
		public void UpdateInputAxisProvider()
		{
			this.m_XAxis.SetInputAxisProvider(0, null);
			this.m_YAxis.SetInputAxisProvider(1, null);
			AxisState.IInputAxisProvider inputAxisProvider = base.GetInputAxisProvider();
			if (inputAxisProvider != null)
			{
				this.m_XAxis.SetInputAxisProvider(0, inputAxisProvider);
				this.m_YAxis.SetInputAxisProvider(1, inputAxisProvider);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006D50 File Offset: 0x00004F50
		protected override void OnDestroy()
		{
			if (this.m_Rigs != null)
			{
				foreach (CinemachineVirtualCamera cinemachineVirtualCamera in this.m_Rigs)
				{
					if (cinemachineVirtualCamera != null && cinemachineVirtualCamera.gameObject != null)
					{
						cinemachineVirtualCamera.gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
					}
				}
			}
			this.mIsDestroyed = true;
			base.OnDestroy();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006DB6 File Offset: 0x00004FB6
		private void OnTransformChildrenChanged()
		{
			this.InvalidateRigCache();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006DBE File Offset: 0x00004FBE
		private void Reset()
		{
			this.DestroyRigs();
			this.UpdateRigCache();
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00006DCD File Offset: 0x00004FCD
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public override bool PreviousStateIsValid
		{
			get
			{
				return base.PreviousStateIsValid;
			}
			set
			{
				if (!value)
				{
					int num = 0;
					while (this.m_Rigs != null && num < this.m_Rigs.Length)
					{
						if (this.m_Rigs[num] != null)
						{
							this.m_Rigs[num].PreviousStateIsValid = value;
						}
						num++;
					}
				}
				base.PreviousStateIsValid = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00006E28 File Offset: 0x00005028
		public override CameraState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00006E30 File Offset: 0x00005030
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00006E3E File Offset: 0x0000503E
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00006E47 File Offset: 0x00005047
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00006E55 File Offset: 0x00005055
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

		// Token: 0x060000C8 RID: 200 RVA: 0x00006E60 File Offset: 0x00005060
		public override bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			if (!this.RigsAreCreated)
			{
				return false;
			}
			float yaxisValue = this.GetYAxisValue();
			if (dominantChildOnly)
			{
				if (vcam == this.m_Rigs[0])
				{
					return yaxisValue > 0.666f;
				}
				if (vcam == this.m_Rigs[2])
				{
					return (double)yaxisValue < 0.333;
				}
				return vcam == this.m_Rigs[1] && yaxisValue >= 0.333f && yaxisValue <= 0.666f;
			}
			else
			{
				if (vcam == this.m_Rigs[1])
				{
					return true;
				}
				if (yaxisValue < 0.5f)
				{
					return vcam == this.m_Rigs[2];
				}
				return vcam == this.m_Rigs[0];
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006F00 File Offset: 0x00005100
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			this.UpdateRigCache();
			if (this.RigsAreCreated)
			{
				CinemachineVirtualCamera[] rigs = this.m_Rigs;
				for (int i = 0; i < rigs.Length; i++)
				{
					rigs[i].OnTargetObjectWarped(target, positionDelta);
				}
			}
			base.OnTargetObjectWarped(target, positionDelta);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006F44 File Offset: 0x00005144
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			Vector3 referenceUp = this.m_State.ReferenceUp;
			this.m_YAxis.Value = this.GetYAxisClosestValue(pos, referenceUp);
			this.PreviousStateIsValid = true;
			base.transform.ConservativeSetPositionAndRotation(pos, rot);
			this.m_State.RawPosition = pos;
			this.m_State.RawOrientation = rot;
			if (this.UpdateRigCache())
			{
				for (int i = 0; i < 3; i++)
				{
					this.m_Rigs[i].ForceCameraPosition(pos, rot);
				}
				if (this.m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					this.m_XAxis.Value = this.mOrbitals[1].m_XAxis.Value;
				}
				this.PushSettingsToRigs();
				this.InternalUpdateCameraState(referenceUp, -1f);
			}
			base.ForceCameraPosition(pos, rot);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007000 File Offset: 0x00005200
		public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			base.UpdateTargetCache();
			this.UpdateRigCache();
			if (!this.RigsAreCreated)
			{
				return;
			}
			this.m_State = this.CalculateNewState(worldUp, deltaTime);
			base.ApplyPositionBlendMethod(ref this.m_State, this.m_Transitions.m_BlendHint);
			if (this.Follow != null)
			{
				Vector3 vector = this.m_State.RawPosition - base.transform.position;
				base.transform.position = this.m_State.RawPosition;
				this.m_Rigs[0].transform.position -= vector;
				this.m_Rigs[1].transform.position -= vector;
				this.m_Rigs[2].transform.position -= vector;
			}
			base.InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref this.m_State, deltaTime);
			if (this.PreviousStateIsValid && CinemachineCore.Instance.IsLive(this) && deltaTime >= 0f && this.m_YAxis.Update(deltaTime))
			{
				this.m_YAxisRecentering.CancelRecentering();
			}
			this.PushSettingsToRigs();
			if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				this.m_XAxis.Value = 0f;
			}
			this.PreviousStateIsValid = true;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007154 File Offset: 0x00005354
		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
			if (!this.RigsAreCreated)
			{
				return;
			}
			base.InvokeOnTransitionInExtensions(fromCam, worldUp, deltaTime);
			if (fromCam != null && this.m_Transitions.m_InheritPosition && !CinemachineCore.Instance.IsLiveInBlend(this))
			{
				Vector3 vector = fromCam.State.RawPosition;
				if (fromCam is CinemachineFreeLook)
				{
					CinemachineFreeLook cinemachineFreeLook = fromCam as CinemachineFreeLook;
					CinemachineOrbitalTransposer cinemachineOrbitalTransposer = ((cinemachineFreeLook.mOrbitals != null) ? cinemachineFreeLook.mOrbitals[1] : null);
					if (cinemachineOrbitalTransposer != null)
					{
						vector = cinemachineOrbitalTransposer.GetTargetCameraPosition(worldUp);
					}
				}
				this.ForceCameraPosition(vector, fromCam.State.FinalOrientation);
			}
			base.UpdateCameraState(worldUp, deltaTime);
			if (this.m_Transitions.m_OnCameraLive != null)
			{
				this.m_Transitions.m_OnCameraLive.Invoke(this, fromCam);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007216 File Offset: 0x00005416
		internal override bool RequiresUserInput()
		{
			return true;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000721C File Offset: 0x0000541C
		private float GetYAxisClosestValue(Vector3 cameraPos, Vector3 up)
		{
			if (this.Follow != null)
			{
				Vector3 vector = Quaternion.FromToRotation(up, Vector3.up) * (cameraPos - this.Follow.position);
				Vector3 vector2 = vector;
				vector2.y = 0f;
				if (!vector2.AlmostZero())
				{
					vector = Quaternion.AngleAxis(UnityVectorExtensions.SignedAngle(vector2, Vector3.back, Vector3.up), Vector3.up) * vector;
				}
				vector.x = 0f;
				return this.SteepestDescent(vector.normalized * (cameraPos - this.Follow.position).magnitude);
			}
			return this.m_YAxis.Value;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000072D8 File Offset: 0x000054D8
		private float SteepestDescent(Vector3 cameraOffset)
		{
			CinemachineFreeLook.<>c__DisplayClass47_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cameraOffset = cameraOffset;
			float num = this.<SteepestDescent>g__InitialGuess|47_2(ref CS$<>8__locals1);
			for (int i = 0; i < 10; i++)
			{
				float num2 = this.<SteepestDescent>g__AngleFunction|47_0(num, ref CS$<>8__locals1);
				float num3 = this.<SteepestDescent>g__SlopeOfAngleFunction|47_1(num, ref CS$<>8__locals1);
				if (Mathf.Abs(num3) < 0.005f || Mathf.Abs(num2) < 0.005f)
				{
					break;
				}
				num = Mathf.Clamp01(num - num2 / num3);
			}
			return num;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007348 File Offset: 0x00005548
		private void InvalidateRigCache()
		{
			this.mOrbitals = null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007354 File Offset: 0x00005554
		private void DestroyRigs()
		{
			List<CinemachineVirtualCamera> list = new List<CinemachineVirtualCamera>(3);
			for (int i = 0; i < CinemachineFreeLook.RigNames.Length; i++)
			{
				foreach (object obj in base.transform)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.name == CinemachineFreeLook.RigNames[i])
					{
						list.Add(transform.GetComponent<CinemachineVirtualCamera>());
					}
				}
			}
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in list)
			{
				if (cinemachineVirtualCamera != null)
				{
					if (CinemachineFreeLook.DestroyRigOverride != null)
					{
						CinemachineFreeLook.DestroyRigOverride(cinemachineVirtualCamera.gameObject);
					}
					else
					{
						cinemachineVirtualCamera.DestroyPipeline();
						Object.Destroy(cinemachineVirtualCamera);
						if (!RuntimeUtility.IsPrefab(base.gameObject))
						{
							Object.Destroy(cinemachineVirtualCamera.gameObject);
						}
					}
				}
			}
			this.mOrbitals = null;
			this.m_Rigs = null;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00007478 File Offset: 0x00005678
		private CinemachineVirtualCamera[] CreateRigs(CinemachineVirtualCamera[] copyFrom)
		{
			float[] array = new float[] { 0.5f, 0.55f, 0.6f };
			this.mOrbitals = null;
			this.m_Rigs = null;
			CinemachineVirtualCamera[] array2 = new CinemachineVirtualCamera[3];
			for (int i = 0; i < array2.Length; i++)
			{
				CinemachineVirtualCamera cinemachineVirtualCamera = ((copyFrom != null && copyFrom.Length > i) ? copyFrom[i] : null);
				if (CinemachineFreeLook.CreateRigOverride != null)
				{
					array2[i] = CinemachineFreeLook.CreateRigOverride(this, CinemachineFreeLook.RigNames[i], cinemachineVirtualCamera);
				}
				else
				{
					GameObject gameObject = null;
					foreach (object obj in base.transform)
					{
						Transform transform = (Transform)obj;
						if (transform.gameObject.name == CinemachineFreeLook.RigNames[i])
						{
							gameObject = transform.gameObject;
							break;
						}
					}
					if (gameObject == null && !RuntimeUtility.IsPrefab(base.gameObject))
					{
						gameObject = new GameObject(CinemachineFreeLook.RigNames[i]);
						gameObject.transform.parent = base.transform;
					}
					if (gameObject == null)
					{
						array2[i] = null;
					}
					else
					{
						array2[i] = gameObject.AddComponent<CinemachineVirtualCamera>();
						array2[i].AddCinemachineComponent<CinemachineOrbitalTransposer>();
						array2[i].AddCinemachineComponent<CinemachineComposer>();
					}
				}
				if (array2[i] != null)
				{
					array2[i].InvalidateComponentPipeline();
					CinemachineOrbitalTransposer cinemachineOrbitalTransposer = array2[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
					if (cinemachineOrbitalTransposer == null)
					{
						cinemachineOrbitalTransposer = array2[i].AddCinemachineComponent<CinemachineOrbitalTransposer>();
					}
					if (cinemachineVirtualCamera == null)
					{
						cinemachineOrbitalTransposer.m_YawDamping = 0f;
						CinemachineComposer cinemachineComponent = array2[i].GetCinemachineComponent<CinemachineComposer>();
						if (cinemachineComponent != null)
						{
							cinemachineComponent.m_HorizontalDamping = (cinemachineComponent.m_VerticalDamping = 0f);
							cinemachineComponent.m_ScreenX = 0.5f;
							cinemachineComponent.m_ScreenY = array[i];
							cinemachineComponent.m_DeadZoneWidth = (cinemachineComponent.m_DeadZoneHeight = 0f);
							cinemachineComponent.m_SoftZoneWidth = (cinemachineComponent.m_SoftZoneHeight = 0.8f);
							cinemachineComponent.m_BiasX = (cinemachineComponent.m_BiasY = 0f);
						}
					}
				}
			}
			return array2;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000076A4 File Offset: 0x000058A4
		private bool UpdateRigCache()
		{
			if (this.mIsDestroyed)
			{
				return false;
			}
			if (this.mOrbitals != null && this.mOrbitals.Length == 3)
			{
				return true;
			}
			this.m_CachedXAxisHeading = 0f;
			this.m_Rigs = null;
			this.mOrbitals = null;
			List<CinemachineVirtualCamera> list = this.LocateExistingRigs(false);
			if (list == null || list.Count != 3)
			{
				this.DestroyRigs();
				this.CreateRigs(null);
				list = this.LocateExistingRigs(true);
			}
			if (list != null && list.Count == 3)
			{
				this.m_Rigs = list.ToArray();
			}
			if (this.RigsAreCreated)
			{
				this.mOrbitals = new CinemachineOrbitalTransposer[this.m_Rigs.Length];
				for (int i = 0; i < this.m_Rigs.Length; i++)
				{
					this.mOrbitals[i] = this.m_Rigs[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
				}
				this.mBlendA = new CinemachineBlend(this.m_Rigs[1], this.m_Rigs[0], AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0f);
				this.mBlendB = new CinemachineBlend(this.m_Rigs[2], this.m_Rigs[1], AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0f);
				return true;
			}
			return false;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000077F0 File Offset: 0x000059F0
		private List<CinemachineVirtualCamera> LocateExistingRigs(bool forceOrbital)
		{
			this.m_CachedXAxisHeading = this.m_XAxis.Value;
			this.m_LastHeadingUpdateFrame = -1f;
			List<CinemachineVirtualCamera> list = new List<CinemachineVirtualCamera>(3);
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				CinemachineVirtualCamera component = transform.GetComponent<CinemachineVirtualCamera>();
				if (component != null)
				{
					GameObject gameObject = transform.gameObject;
					for (int i = 0; i < CinemachineFreeLook.RigNames.Length; i++)
					{
						if (!(gameObject.name != CinemachineFreeLook.RigNames[i]))
						{
							CinemachineOrbitalTransposer cinemachineOrbitalTransposer = component.GetCinemachineComponent<CinemachineOrbitalTransposer>();
							if (cinemachineOrbitalTransposer == null && forceOrbital)
							{
								cinemachineOrbitalTransposer = component.AddCinemachineComponent<CinemachineOrbitalTransposer>();
							}
							if (cinemachineOrbitalTransposer != null)
							{
								cinemachineOrbitalTransposer.m_HeadingIsSlave = true;
								cinemachineOrbitalTransposer.HideOffsetInInspector = true;
								cinemachineOrbitalTransposer.m_XAxis.m_InputAxisName = string.Empty;
								cinemachineOrbitalTransposer.HeadingUpdater = new CinemachineOrbitalTransposer.UpdateHeadingDelegate(this.UpdateXAxisHeading);
								cinemachineOrbitalTransposer.m_RecenterToTargetHeading.m_enabled = false;
								component.m_StandbyUpdate = this.m_StandbyUpdate;
								list.Add(component);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007938 File Offset: 0x00005B38
		private float UpdateXAxisHeading(CinemachineOrbitalTransposer orbital, float deltaTime, Vector3 up)
		{
			if (this == null)
			{
				return 0f;
			}
			if (!this.PreviousStateIsValid)
			{
				deltaTime = -1f;
			}
			if (this.m_LastHeadingUpdateFrame != (float)Time.frameCount || deltaTime < 0f)
			{
				this.m_LastHeadingUpdateFrame = (float)Time.frameCount;
				float value = this.m_XAxis.Value;
				this.m_CachedXAxisHeading = orbital.UpdateHeading(deltaTime, up, ref this.m_XAxis, ref this.m_RecenterToTargetHeading, CinemachineCore.Instance.IsLive(this));
				if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					this.m_XAxis.Value = value;
				}
			}
			return this.m_CachedXAxisHeading;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000079D4 File Offset: 0x00005BD4
		private void PushSettingsToRigs()
		{
			for (int i = 0; i < this.m_Rigs.Length; i++)
			{
				if (this.m_CommonLens)
				{
					this.m_Rigs[i].m_Lens = this.m_Lens;
				}
				if (this.mUseLegacyRigDefinitions)
				{
					this.mUseLegacyRigDefinitions = false;
					this.m_Orbits[i].m_Height = this.mOrbitals[i].m_FollowOffset.y;
					this.m_Orbits[i].m_Radius = -this.mOrbitals[i].m_FollowOffset.z;
					if (this.m_Rigs[i].Follow != null)
					{
						this.Follow = this.m_Rigs[i].Follow;
					}
				}
				this.m_Rigs[i].Follow = null;
				this.m_Rigs[i].m_StandbyUpdate = this.m_StandbyUpdate;
				this.m_Rigs[i].FollowTargetAttachment = this.FollowTargetAttachment;
				this.m_Rigs[i].LookAtTargetAttachment = this.LookAtTargetAttachment;
				if (!this.PreviousStateIsValid)
				{
					this.m_Rigs[i].PreviousStateIsValid = false;
					this.m_Rigs[i].transform.ConservativeSetPositionAndRotation(base.transform.position, base.transform.rotation);
				}
				this.mOrbitals[i].m_FollowOffset = this.GetLocalPositionForCameraFromInput(this.GetYAxisValue());
				this.mOrbitals[i].m_BindingMode = this.m_BindingMode;
				this.mOrbitals[i].m_Heading = this.m_Heading;
				this.mOrbitals[i].m_XAxis.Value = this.m_XAxis.Value;
				if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					this.m_Rigs[i].SetStateRawPosition(this.State.RawPosition);
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007B98 File Offset: 0x00005D98
		private float GetYAxisValue()
		{
			float num = this.m_YAxis.m_MaxValue - this.m_YAxis.m_MinValue;
			if (num <= 0.0001f)
			{
				return 0.5f;
			}
			return this.m_YAxis.Value / num;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007BD8 File Offset: 0x00005DD8
		private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
		{
			CameraState cameraState = base.PullStateFromVirtualCamera(worldUp, ref this.m_Lens);
			this.m_YAxisRecentering.DoRecentering(ref this.m_YAxis, deltaTime, 0.5f);
			float yaxisValue = this.GetYAxisValue();
			if (yaxisValue > 0.5f)
			{
				if (this.mBlendA != null)
				{
					this.mBlendA.TimeInBlend = (yaxisValue - 0.5f) * 2f;
					this.mBlendA.UpdateCameraState(worldUp, deltaTime);
					cameraState = this.mBlendA.State;
				}
			}
			else if (this.mBlendB != null)
			{
				this.mBlendB.TimeInBlend = yaxisValue * 2f;
				this.mBlendB.UpdateCameraState(worldUp, deltaTime);
				cameraState = this.mBlendB.State;
			}
			return cameraState;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007C88 File Offset: 0x00005E88
		public Vector3 GetLocalPositionForCameraFromInput(float t)
		{
			if (this.mOrbitals == null)
			{
				return Vector3.zero;
			}
			this.UpdateCachedSpline();
			int num = 1;
			if (t > 0.5f)
			{
				t -= 0.5f;
				num = 2;
			}
			return SplineHelpers.Bezier3(t * 2f, this.m_CachedKnots[num], this.m_CachedCtrl1[num], this.m_CachedCtrl2[num], this.m_CachedKnots[num + 1]);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007D10 File Offset: 0x00005F10
		private void UpdateCachedSpline()
		{
			bool flag = this.m_CachedOrbits != null && this.m_CachedOrbits.Length == 3 && this.m_CachedTension == this.m_SplineCurvature;
			int num = 0;
			while (num < 3 && flag)
			{
				flag = this.m_CachedOrbits[num].m_Height == this.m_Orbits[num].m_Height && this.m_CachedOrbits[num].m_Radius == this.m_Orbits[num].m_Radius;
				num++;
			}
			if (!flag)
			{
				float splineCurvature = this.m_SplineCurvature;
				this.m_CachedKnots = new Vector4[5];
				this.m_CachedCtrl1 = new Vector4[5];
				this.m_CachedCtrl2 = new Vector4[5];
				this.m_CachedKnots[1] = new Vector4(0f, this.m_Orbits[2].m_Height, -this.m_Orbits[2].m_Radius, 0f);
				this.m_CachedKnots[2] = new Vector4(0f, this.m_Orbits[1].m_Height, -this.m_Orbits[1].m_Radius, 0f);
				this.m_CachedKnots[3] = new Vector4(0f, this.m_Orbits[0].m_Height, -this.m_Orbits[0].m_Radius, 0f);
				this.m_CachedKnots[0] = Vector4.Lerp(this.m_CachedKnots[1], Vector4.zero, splineCurvature);
				this.m_CachedKnots[4] = Vector4.Lerp(this.m_CachedKnots[3], Vector4.zero, splineCurvature);
				SplineHelpers.ComputeSmoothControlPoints(ref this.m_CachedKnots, ref this.m_CachedCtrl1, ref this.m_CachedCtrl2);
				this.m_CachedOrbits = new CinemachineFreeLook.Orbit[3];
				for (int i = 0; i < 3; i++)
				{
					this.m_CachedOrbits[i] = this.m_Orbits[i];
				}
				this.m_CachedTension = this.m_SplineCurvature;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00007F22 File Offset: 0x00006122
		internal override void OnBeforeSerialize()
		{
			if (!this.m_Lens.IsPhysicalCamera)
			{
				this.m_Lens.SensorSize = Vector2.one;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00008084 File Offset: 0x00006284
		[CompilerGenerated]
		private float <SteepestDescent>g__AngleFunction|47_0(float input, ref CinemachineFreeLook.<>c__DisplayClass47_0 A_2)
		{
			Vector3 localPositionForCameraFromInput = this.GetLocalPositionForCameraFromInput(input);
			return Mathf.Abs(UnityVectorExtensions.SignedAngle(A_2.cameraOffset, localPositionForCameraFromInput, Vector3.right));
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000080B0 File Offset: 0x000062B0
		[CompilerGenerated]
		private float <SteepestDescent>g__SlopeOfAngleFunction|47_1(float input, ref CinemachineFreeLook.<>c__DisplayClass47_0 A_2)
		{
			float num = this.<SteepestDescent>g__AngleFunction|47_0(input - 0.005f, ref A_2);
			return (this.<SteepestDescent>g__AngleFunction|47_0(input + 0.005f, ref A_2) - num) / 0.01f;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000080E4 File Offset: 0x000062E4
		[CompilerGenerated]
		private float <SteepestDescent>g__InitialGuess|47_2(ref CinemachineFreeLook.<>c__DisplayClass47_0 A_1)
		{
			this.UpdateCachedSpline();
			CinemachineFreeLook.<>c__DisplayClass47_1 CS$<>8__locals1;
			CS$<>8__locals1.best = 0.5f;
			CS$<>8__locals1.bestAngle = this.<SteepestDescent>g__AngleFunction|47_0(CS$<>8__locals1.best, ref A_1);
			for (int i = 0; i <= 5; i++)
			{
				float num = (float)i * 0.1f;
				this.<SteepestDescent>g__ChooseBestAngle|47_3(0.5f + num, ref A_1, ref CS$<>8__locals1);
				this.<SteepestDescent>g__ChooseBestAngle|47_3(0.5f - num, ref A_1, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.best;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00008154 File Offset: 0x00006354
		[CompilerGenerated]
		private void <SteepestDescent>g__ChooseBestAngle|47_3(float referenceAngle, ref CinemachineFreeLook.<>c__DisplayClass47_0 A_2, ref CinemachineFreeLook.<>c__DisplayClass47_1 A_3)
		{
			float num = this.<SteepestDescent>g__AngleFunction|47_0(referenceAngle, ref A_2);
			if (num < A_3.bestAngle)
			{
				A_3.bestAngle = num;
				A_3.best = referenceAngle;
			}
		}

		// Token: 0x0400007F RID: 127
		[Tooltip("Object for the camera children to look at (the aim target).")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_LookAt;

		// Token: 0x04000080 RID: 128
		[Tooltip("Object for the camera children wants to move with (the body target).")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_Follow;

		// Token: 0x04000081 RID: 129
		[Tooltip("If enabled, this lens setting will apply to all three child rigs, otherwise the child rig lens settings will be used")]
		[FormerlySerializedAs("m_UseCommonLensSetting")]
		public bool m_CommonLens = true;

		// Token: 0x04000082 RID: 130
		[FormerlySerializedAs("m_LensAttributes")]
		[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active")]
		public LensSettings m_Lens = LensSettings.Default;

		// Token: 0x04000083 RID: 131
		public CinemachineVirtualCameraBase.TransitionParams m_Transitions;

		// Token: 0x04000084 RID: 132
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_BlendHint")]
		[FormerlySerializedAs("m_PositionBlending")]
		private CinemachineVirtualCameraBase.BlendHint m_LegacyBlendHint;

		// Token: 0x04000085 RID: 133
		[Header("Axis Control")]
		[Tooltip("The Vertical axis.  Value is 0..1.  Chooses how to blend the child rigs")]
		[AxisStateProperty]
		public AxisState m_YAxis = new AxisState(0f, 1f, false, true, 2f, 0.2f, 0.1f, "Mouse Y", false);

		// Token: 0x04000086 RID: 134
		[Tooltip("Controls how automatic recentering of the Y axis is accomplished")]
		public AxisState.Recentering m_YAxisRecentering = new AxisState.Recentering(false, 1f, 2f);

		// Token: 0x04000087 RID: 135
		[Tooltip("The Horizontal axis.  Value is -180...180.  This is passed on to the rigs' OrbitalTransposer component")]
		[AxisStateProperty]
		public AxisState m_XAxis = new AxisState(-180f, 180f, true, false, 300f, 0.1f, 0.1f, "Mouse X", true);

		// Token: 0x04000088 RID: 136
		[OrbitalTransposerHeadingProperty]
		[Tooltip("The definition of Forward.  Camera will follow behind.")]
		public CinemachineOrbitalTransposer.Heading m_Heading = new CinemachineOrbitalTransposer.Heading(CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward, 4, 0f);

		// Token: 0x04000089 RID: 137
		[Tooltip("Controls how automatic recentering of the X axis is accomplished")]
		public AxisState.Recentering m_RecenterToTargetHeading = new AxisState.Recentering(false, 1f, 2f);

		// Token: 0x0400008A RID: 138
		[Header("Orbits")]
		[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
		public CinemachineTransposer.BindingMode m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

		// Token: 0x0400008B RID: 139
		[Tooltip("Controls how taut is the line that connects the rigs' orbits, which determines final placement on the Y axis")]
		[Range(0f, 1f)]
		[FormerlySerializedAs("m_SplineTension")]
		public float m_SplineCurvature = 0.2f;

		// Token: 0x0400008C RID: 140
		[Tooltip("The radius and height of the three orbiting rigs.")]
		public CinemachineFreeLook.Orbit[] m_Orbits = new CinemachineFreeLook.Orbit[]
		{
			new CinemachineFreeLook.Orbit(4.5f, 1.75f),
			new CinemachineFreeLook.Orbit(2.5f, 3f),
			new CinemachineFreeLook.Orbit(0.4f, 1.3f)
		};

		// Token: 0x0400008D RID: 141
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_HeadingBias")]
		private float m_LegacyHeadingBias = float.MaxValue;

		// Token: 0x0400008E RID: 142
		private bool mUseLegacyRigDefinitions;

		// Token: 0x0400008F RID: 143
		private bool mIsDestroyed;

		// Token: 0x04000090 RID: 144
		private CameraState m_State = CameraState.Default;

		// Token: 0x04000091 RID: 145
		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		private CinemachineVirtualCamera[] m_Rigs = new CinemachineVirtualCamera[3];

		// Token: 0x04000092 RID: 146
		private CinemachineOrbitalTransposer[] mOrbitals;

		// Token: 0x04000093 RID: 147
		private CinemachineBlend mBlendA;

		// Token: 0x04000094 RID: 148
		private CinemachineBlend mBlendB;

		// Token: 0x04000095 RID: 149
		public static CinemachineFreeLook.CreateRigDelegate CreateRigOverride;

		// Token: 0x04000096 RID: 150
		public static CinemachineFreeLook.DestroyRigDelegate DestroyRigOverride;

		// Token: 0x04000097 RID: 151
		private float m_CachedXAxisHeading;

		// Token: 0x04000098 RID: 152
		private float m_LastHeadingUpdateFrame;

		// Token: 0x04000099 RID: 153
		private CinemachineFreeLook.Orbit[] m_CachedOrbits;

		// Token: 0x0400009A RID: 154
		private float m_CachedTension;

		// Token: 0x0400009B RID: 155
		private Vector4[] m_CachedKnots;

		// Token: 0x0400009C RID: 156
		private Vector4[] m_CachedCtrl1;

		// Token: 0x0400009D RID: 157
		private Vector4[] m_CachedCtrl2;

		// Token: 0x0200007F RID: 127
		[Serializable]
		public struct Orbit
		{
			// Token: 0x06000424 RID: 1060 RVA: 0x00018CDC File Offset: 0x00016EDC
			public Orbit(float h, float r)
			{
				this.m_Height = h;
				this.m_Radius = r;
			}

			// Token: 0x040002E9 RID: 745
			public float m_Height;

			// Token: 0x040002EA RID: 746
			public float m_Radius;
		}

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x06000426 RID: 1062
		public delegate CinemachineVirtualCamera CreateRigDelegate(CinemachineFreeLook vcam, string name, CinemachineVirtualCamera copyFrom);

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x0600042A RID: 1066
		public delegate void DestroyRigDelegate(GameObject rig);
	}
}
