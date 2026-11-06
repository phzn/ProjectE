using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Cinemachine
{
	// Token: 0x0200000D RID: 13
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[AddComponentMenu("Cinemachine/CinemachineBrain")]
	[SaveDuringPlay]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineBrainProperties.html")]
	public class CinemachineBrain : MonoBehaviour, ICameraOverrideStack
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000309D File Offset: 0x0000129D
		public Camera OutputCamera
		{
			get
			{
				if (this.m_OutputCamera == null && !Application.isPlaying)
				{
					this.ControlledObject.TryGetComponent<Camera>(out this.m_OutputCamera);
				}
				return this.m_OutputCamera;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000030CC File Offset: 0x000012CC
		// (set) Token: 0x06000041 RID: 65 RVA: 0x000030E9 File Offset: 0x000012E9
		public GameObject ControlledObject
		{
			get
			{
				if (!(this.m_TargetOverride == null))
				{
					return this.m_TargetOverride;
				}
				return base.gameObject;
			}
			set
			{
				if (this.m_TargetOverride != value)
				{
					this.m_TargetOverride = value;
					this.ControlledObject.TryGetComponent<Camera>(out this.m_OutputCamera);
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000310D File Offset: 0x0000130D
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003114 File Offset: 0x00001314
		public static ICinemachineCamera SoloCamera
		{
			get
			{
				return CinemachineBrain.mSoloCamera;
			}
			set
			{
				if (value != null && !CinemachineCore.Instance.IsLive(value))
				{
					value.OnTransitionFromCamera(null, Vector3.up, CinemachineCore.DeltaTime);
				}
				CinemachineBrain.mSoloCamera = value;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000313D File Offset: 0x0000133D
		public static Color GetSoloGUIColor()
		{
			return Color.Lerp(Color.red, Color.yellow, 0.8f);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003153 File Offset: 0x00001353
		public Vector3 DefaultWorldUp
		{
			get
			{
				if (!(this.m_WorldUpOverride != null))
				{
					return Vector3.up;
				}
				return this.m_WorldUpOverride.transform.up;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000317C File Offset: 0x0000137C
		private void OnEnable()
		{
			if (this.mFrameStack.Count == 0)
			{
				this.mFrameStack.Add(new CinemachineBrain.BrainFrame());
			}
			CinemachineCore.Instance.AddActiveBrain(this);
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Combine(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
			this.mPhysicsCoroutine = base.StartCoroutine(this.AfterPhysics());
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			SceneManager.sceneUnloaded += this.OnSceneUnloaded;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003228 File Offset: 0x00001428
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
			CinemachineCore.Instance.RemoveActiveBrain(this);
			this.mFrameStack.Clear();
			base.StopCoroutine(this.mPhysicsCoroutine);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003299 File Offset: 0x00001499
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (Time.frameCount == this.m_LastFrameUpdated && this.mFrameStack.Count > 0)
			{
				this.ManualUpdate();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000032BC File Offset: 0x000014BC
		private void OnSceneUnloaded(Scene scene)
		{
			if (Time.frameCount == this.m_LastFrameUpdated && this.mFrameStack.Count > 0)
			{
				this.ManualUpdate();
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000032DF File Offset: 0x000014DF
		private void Awake()
		{
			this.ControlledObject.TryGetComponent<Camera>(out this.m_OutputCamera);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000032F3 File Offset: 0x000014F3
		private void Start()
		{
			this.m_LastFrameUpdated = -1;
			this.UpdateVirtualCameras(CinemachineCore.UpdateFilter.Late, -1f);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003308 File Offset: 0x00001508
		private void OnGuiHandler()
		{
			if (!this.m_ShowDebugText)
			{
				CinemachineDebug.ReleaseScreenPos(this);
				return;
			}
			StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
			Color color = GUI.color;
			stringBuilder.Length = 0;
			stringBuilder.Append("CM ");
			stringBuilder.Append(base.gameObject.name);
			stringBuilder.Append(": ");
			if (CinemachineBrain.SoloCamera != null)
			{
				stringBuilder.Append("SOLO ");
				GUI.color = CinemachineBrain.GetSoloGUIColor();
			}
			if (this.IsBlending)
			{
				stringBuilder.Append(this.ActiveBlend.Description);
			}
			else
			{
				ICinemachineCamera activeVirtualCamera = this.ActiveVirtualCamera;
				if (activeVirtualCamera == null)
				{
					stringBuilder.Append("(none)");
				}
				else
				{
					stringBuilder.Append("[");
					stringBuilder.Append(activeVirtualCamera.Name);
					stringBuilder.Append("]");
				}
			}
			string text = stringBuilder.ToString();
			GUI.Label(CinemachineDebug.GetScreenPos(this, text, GUI.skin.box), text, GUI.skin.box);
			GUI.color = color;
			CinemachineDebug.ReturnToPool(stringBuilder);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000340B File Offset: 0x0000160B
		private IEnumerator AfterPhysics()
		{
			for (;;)
			{
				yield return this.mWaitForFixedUpdate;
				if (this.m_UpdateMethod == CinemachineBrain.UpdateMethod.FixedUpdate || this.m_UpdateMethod == CinemachineBrain.UpdateMethod.SmartUpdate)
				{
					CinemachineCore.UpdateFilter updateFilter = CinemachineCore.UpdateFilter.Fixed;
					if (this.m_UpdateMethod == CinemachineBrain.UpdateMethod.SmartUpdate)
					{
						UpdateTracker.OnUpdate(UpdateTracker.UpdateClock.Fixed);
						updateFilter = CinemachineCore.UpdateFilter.Smart;
					}
					this.UpdateVirtualCameras(updateFilter, this.GetEffectiveDeltaTime(true));
				}
				if (this.m_BlendUpdateMethod == CinemachineBrain.BrainUpdateMethod.FixedUpdate)
				{
					this.UpdateFrame0(Time.fixedDeltaTime);
					this.ProcessActiveCamera(Time.fixedDeltaTime);
				}
			}
			yield break;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000341A File Offset: 0x0000161A
		private void LateUpdate()
		{
			if (this.m_UpdateMethod != CinemachineBrain.UpdateMethod.ManualUpdate)
			{
				this.ManualUpdate();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000342C File Offset: 0x0000162C
		public void ManualUpdate()
		{
			this.m_LastFrameUpdated = Time.frameCount;
			float effectiveDeltaTime = this.GetEffectiveDeltaTime(false);
			if (!Application.isPlaying || this.m_BlendUpdateMethod != CinemachineBrain.BrainUpdateMethod.FixedUpdate)
			{
				this.UpdateFrame0(effectiveDeltaTime);
			}
			this.ComputeCurrentBlend(ref this.mCurrentLiveCameras, 0);
			if (Application.isPlaying && this.m_UpdateMethod == CinemachineBrain.UpdateMethod.FixedUpdate)
			{
				if (this.m_BlendUpdateMethod != CinemachineBrain.BrainUpdateMethod.FixedUpdate)
				{
					CinemachineCore.Instance.m_CurrentUpdateFilter = CinemachineCore.UpdateFilter.Fixed;
					if (CinemachineBrain.SoloCamera == null)
					{
						this.mCurrentLiveCameras.UpdateCameraState(this.DefaultWorldUp, this.GetEffectiveDeltaTime(true));
					}
				}
			}
			else
			{
				CinemachineCore.UpdateFilter updateFilter = CinemachineCore.UpdateFilter.Late;
				if (this.m_UpdateMethod == CinemachineBrain.UpdateMethod.SmartUpdate)
				{
					UpdateTracker.OnUpdate(UpdateTracker.UpdateClock.Late);
					updateFilter = CinemachineCore.UpdateFilter.SmartLate;
				}
				this.UpdateVirtualCameras(updateFilter, effectiveDeltaTime);
			}
			if (!Application.isPlaying || this.m_BlendUpdateMethod != CinemachineBrain.BrainUpdateMethod.FixedUpdate)
			{
				this.ProcessActiveCamera(effectiveDeltaTime);
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000034E4 File Offset: 0x000016E4
		private float GetEffectiveDeltaTime(bool fixedDelta)
		{
			if (CinemachineCore.UniformDeltaTimeOverride >= 0f)
			{
				return CinemachineCore.UniformDeltaTimeOverride;
			}
			if (CinemachineBrain.SoloCamera != null)
			{
				return Time.unscaledDeltaTime;
			}
			if (!Application.isPlaying)
			{
				for (int i = this.mFrameStack.Count - 1; i > 0; i--)
				{
					CinemachineBrain.BrainFrame brainFrame = this.mFrameStack[i];
					if (brainFrame.Active)
					{
						return brainFrame.deltaTimeOverride;
					}
				}
				return -1f;
			}
			if (this.m_IgnoreTimeScale)
			{
				if (!fixedDelta)
				{
					return Time.unscaledDeltaTime;
				}
				return Time.fixedDeltaTime;
			}
			else
			{
				if (!fixedDelta)
				{
					return Time.deltaTime;
				}
				return Time.fixedDeltaTime;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003578 File Offset: 0x00001778
		private void UpdateVirtualCameras(CinemachineCore.UpdateFilter updateFilter, float deltaTime)
		{
			CinemachineCore.Instance.m_CurrentUpdateFilter = updateFilter;
			Camera outputCamera = this.OutputCamera;
			CinemachineCore.Instance.UpdateAllActiveVirtualCameras((outputCamera == null) ? (-1) : outputCamera.cullingMask, this.DefaultWorldUp, deltaTime);
			if (CinemachineBrain.SoloCamera != null)
			{
				CinemachineBrain.SoloCamera.UpdateCameraState(this.DefaultWorldUp, deltaTime);
			}
			this.mCurrentLiveCameras.UpdateCameraState(this.DefaultWorldUp, deltaTime);
			updateFilter = CinemachineCore.UpdateFilter.Late;
			if (Application.isPlaying)
			{
				if (this.m_UpdateMethod == CinemachineBrain.UpdateMethod.SmartUpdate)
				{
					updateFilter |= CinemachineCore.UpdateFilter.Smart;
				}
				else if (this.m_UpdateMethod == CinemachineBrain.UpdateMethod.FixedUpdate)
				{
					updateFilter = CinemachineCore.UpdateFilter.Fixed;
				}
			}
			CinemachineCore.Instance.m_CurrentUpdateFilter = updateFilter;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003614 File Offset: 0x00001814
		public ICinemachineCamera ActiveVirtualCamera
		{
			get
			{
				if (CinemachineBrain.SoloCamera != null)
				{
					return CinemachineBrain.SoloCamera;
				}
				return CinemachineBrain.DeepCamBFromBlend(this.mCurrentLiveCameras);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003630 File Offset: 0x00001830
		private static ICinemachineCamera DeepCamBFromBlend(CinemachineBlend blend)
		{
			ICinemachineCamera cinemachineCamera;
			BlendSourceVirtualCamera blendSourceVirtualCamera;
			for (cinemachineCamera = blend.CamB; cinemachineCamera != null; cinemachineCamera = blendSourceVirtualCamera.Blend.CamB)
			{
				if (!cinemachineCamera.IsValid)
				{
					return null;
				}
				blendSourceVirtualCamera = cinemachineCamera as BlendSourceVirtualCamera;
				if (blendSourceVirtualCamera == null)
				{
					break;
				}
			}
			return cinemachineCamera;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000366C File Offset: 0x0000186C
		public bool IsLiveInBlend(ICinemachineCamera vcam)
		{
			if (vcam == this.mCurrentLiveCameras.CamA)
			{
				return true;
			}
			BlendSourceVirtualCamera blendSourceVirtualCamera = this.mCurrentLiveCameras.CamA as BlendSourceVirtualCamera;
			if (blendSourceVirtualCamera != null && blendSourceVirtualCamera.Blend.Uses(vcam))
			{
				return true;
			}
			ICinemachineCamera parentCamera = vcam.ParentCamera;
			return parentCamera != null && parentCamera.IsLiveChild(vcam, false) && this.IsLiveInBlend(parentCamera);
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000036CA File Offset: 0x000018CA
		public bool IsBlending
		{
			get
			{
				return this.ActiveBlend != null;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000036D5 File Offset: 0x000018D5
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003710 File Offset: 0x00001910
		public CinemachineBlend ActiveBlend
		{
			get
			{
				if (CinemachineBrain.SoloCamera != null)
				{
					return null;
				}
				if (this.mCurrentLiveCameras.CamA == null || this.mCurrentLiveCameras.Equals(null) || this.mCurrentLiveCameras.IsComplete)
				{
					return null;
				}
				return this.mCurrentLiveCameras;
			}
			set
			{
				if (value == null)
				{
					this.mFrameStack[0].blend.Duration = 0f;
					return;
				}
				this.mFrameStack[0].blend = value;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003744 File Offset: 0x00001944
		private int GetBrainFrame(int withId)
		{
			for (int i = this.mFrameStack.Count - 1; i > 0; i--)
			{
				if (this.mFrameStack[i].id == withId)
				{
					return i;
				}
			}
			this.mFrameStack.Add(new CinemachineBrain.BrainFrame
			{
				id = withId
			});
			return this.mFrameStack.Count - 1;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000037A4 File Offset: 0x000019A4
		public int SetCameraOverride(int overrideId, ICinemachineCamera camA, ICinemachineCamera camB, float weightB, float deltaTime)
		{
			if (overrideId < 0)
			{
				int num = this.mNextFrameId;
				this.mNextFrameId = num + 1;
				overrideId = num;
			}
			CinemachineBrain.BrainFrame brainFrame = this.mFrameStack[this.GetBrainFrame(overrideId)];
			brainFrame.deltaTimeOverride = deltaTime;
			brainFrame.blend.CamA = camA;
			brainFrame.blend.CamB = camB;
			brainFrame.blend.BlendCurve = CinemachineBrain.mDefaultLinearAnimationCurve;
			brainFrame.blend.Duration = 1f;
			brainFrame.blend.TimeInBlend = weightB;
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = camA as CinemachineVirtualCameraBase;
			if (cinemachineVirtualCameraBase != null)
			{
				cinemachineVirtualCameraBase.EnsureStarted();
			}
			cinemachineVirtualCameraBase = camB as CinemachineVirtualCameraBase;
			if (cinemachineVirtualCameraBase != null)
			{
				cinemachineVirtualCameraBase.EnsureStarted();
			}
			return overrideId;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003854 File Offset: 0x00001A54
		public void ReleaseCameraOverride(int overrideId)
		{
			for (int i = this.mFrameStack.Count - 1; i > 0; i--)
			{
				if (this.mFrameStack[i].id == overrideId)
				{
					this.mFrameStack.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000389C File Offset: 0x00001A9C
		private void ProcessActiveCamera(float deltaTime)
		{
			ICinemachineCamera activeVirtualCamera = this.ActiveVirtualCamera;
			if (CinemachineBrain.SoloCamera != null)
			{
				CameraState state = CinemachineBrain.SoloCamera.State;
				this.PushStateToUnityCamera(ref state);
			}
			else if (activeVirtualCamera == null)
			{
				CameraState @default = CameraState.Default;
				Transform transform = this.ControlledObject.transform;
				@default.RawPosition = transform.position;
				@default.RawOrientation = transform.rotation;
				@default.Lens = LensSettings.FromCamera(this.m_OutputCamera);
				@default.BlendHint |= (CameraState.BlendHintValue)67;
				this.PushStateToUnityCamera(ref @default);
			}
			else
			{
				if (this.mActiveCameraPreviousFrameGameObject == null)
				{
					this.mActiveCameraPreviousFrame = null;
				}
				if (activeVirtualCamera != this.mActiveCameraPreviousFrame)
				{
					activeVirtualCamera.OnTransitionFromCamera(this.mActiveCameraPreviousFrame, this.DefaultWorldUp, deltaTime);
					if (this.m_CameraActivatedEvent != null)
					{
						this.m_CameraActivatedEvent.Invoke(activeVirtualCamera, this.mActiveCameraPreviousFrame);
					}
					if (!this.IsBlending || (this.mActiveCameraPreviousFrame != null && !this.ActiveBlend.Uses(this.mActiveCameraPreviousFrame)))
					{
						if (this.m_CameraCutEvent != null)
						{
							this.m_CameraCutEvent.Invoke(this);
						}
						if (CinemachineCore.CameraCutEvent != null)
						{
							CinemachineCore.CameraCutEvent.Invoke(this);
						}
					}
					activeVirtualCamera.UpdateCameraState(this.DefaultWorldUp, deltaTime);
				}
				CameraState state2 = this.mCurrentLiveCameras.State;
				this.PushStateToUnityCamera(ref state2);
			}
			this.mActiveCameraPreviousFrame = activeVirtualCamera;
			this.mActiveCameraPreviousFrameGameObject = ((activeVirtualCamera == null) ? null : activeVirtualCamera.VirtualCameraGameObject);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000039FC File Offset: 0x00001BFC
		private void UpdateFrame0(float deltaTime)
		{
			if (this.mFrameStack.Count == 0)
			{
				this.mFrameStack.Add(new CinemachineBrain.BrainFrame());
			}
			CinemachineBrain.BrainFrame brainFrame = this.mFrameStack[0];
			ICinemachineCamera cinemachineCamera = this.TopCameraFromPriorityQueue();
			ICinemachineCamera camB = brainFrame.blend.CamB;
			if (cinemachineCamera != camB)
			{
				if ((Object)cinemachineCamera != null && (Object)camB != null && deltaTime >= 0f)
				{
					CinemachineBlendDefinition cinemachineBlendDefinition = this.LookupBlend(camB, cinemachineCamera);
					float num = cinemachineBlendDefinition.BlendTime;
					float num2 = 0f;
					if (cinemachineBlendDefinition.BlendCurve != null && num > 0.0001f)
					{
						if (brainFrame.blend.IsComplete)
						{
							brainFrame.blend.CamA = camB;
						}
						else
						{
							if (brainFrame.blend.CamA != cinemachineCamera)
							{
								BlendSourceVirtualCamera blendSourceVirtualCamera = brainFrame.blend.CamA as BlendSourceVirtualCamera;
								if (((blendSourceVirtualCamera != null) ? blendSourceVirtualCamera.Blend.CamB : null) != cinemachineCamera)
								{
									goto IL_013E;
								}
							}
							if (brainFrame.blend.CamB == camB)
							{
								float num3 = brainFrame.blendStartPosition + (1f - brainFrame.blendStartPosition) * brainFrame.blend.TimeInBlend / brainFrame.blend.Duration;
								num *= num3;
								num2 = 1f - num3;
							}
							IL_013E:
							brainFrame.blend.CamA = new BlendSourceVirtualCamera(new CinemachineBlend(brainFrame.blend.CamA, brainFrame.blend.CamB, brainFrame.blend.BlendCurve, brainFrame.blend.Duration, brainFrame.blend.TimeInBlend));
						}
					}
					brainFrame.blend.BlendCurve = cinemachineBlendDefinition.BlendCurve;
					brainFrame.blend.Duration = num;
					brainFrame.blend.TimeInBlend = 0f;
					brainFrame.blendStartPosition = num2;
				}
				brainFrame.blend.CamB = cinemachineCamera;
			}
			if (brainFrame.blend.CamA != null)
			{
				brainFrame.blend.TimeInBlend += ((deltaTime >= 0f) ? deltaTime : brainFrame.blend.Duration);
				if (brainFrame.blend.IsComplete)
				{
					brainFrame.blend.CamA = null;
					brainFrame.blend.BlendCurve = null;
					brainFrame.blend.Duration = 0f;
					brainFrame.blend.TimeInBlend = 0f;
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003C50 File Offset: 0x00001E50
		public void ComputeCurrentBlend(ref CinemachineBlend outputBlend, int numTopLayersToExclude)
		{
			if (this.mFrameStack.Count == 0)
			{
				this.mFrameStack.Add(new CinemachineBrain.BrainFrame());
			}
			int num = 0;
			int num2 = Mathf.Max(1, this.mFrameStack.Count - numTopLayersToExclude);
			for (int i = 0; i < num2; i++)
			{
				CinemachineBrain.BrainFrame brainFrame = this.mFrameStack[i];
				if (i == 0 || brainFrame.Active)
				{
					brainFrame.workingBlend.CamA = brainFrame.blend.CamA;
					brainFrame.workingBlend.CamB = brainFrame.blend.CamB;
					brainFrame.workingBlend.BlendCurve = brainFrame.blend.BlendCurve;
					brainFrame.workingBlend.Duration = brainFrame.blend.Duration;
					brainFrame.workingBlend.TimeInBlend = brainFrame.blend.TimeInBlend;
					if (i > 0 && !brainFrame.blend.IsComplete)
					{
						if (brainFrame.workingBlend.CamA == null)
						{
							if (this.mFrameStack[num].blend.IsComplete)
							{
								brainFrame.workingBlend.CamA = this.mFrameStack[num].blend.CamB;
							}
							else
							{
								brainFrame.workingBlendSource.Blend = this.mFrameStack[num].workingBlend;
								brainFrame.workingBlend.CamA = brainFrame.workingBlendSource;
							}
						}
						else if (brainFrame.workingBlend.CamB == null)
						{
							if (this.mFrameStack[num].blend.IsComplete)
							{
								brainFrame.workingBlend.CamB = this.mFrameStack[num].blend.CamB;
							}
							else
							{
								brainFrame.workingBlendSource.Blend = this.mFrameStack[num].workingBlend;
								brainFrame.workingBlend.CamB = brainFrame.workingBlendSource;
							}
						}
					}
					num = i;
				}
			}
			CinemachineBlend workingBlend = this.mFrameStack[num].workingBlend;
			outputBlend.CamA = workingBlend.CamA;
			outputBlend.CamB = workingBlend.CamB;
			outputBlend.BlendCurve = workingBlend.BlendCurve;
			outputBlend.Duration = workingBlend.Duration;
			outputBlend.TimeInBlend = workingBlend.TimeInBlend;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003EA0 File Offset: 0x000020A0
		public bool IsLive(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			if (CinemachineBrain.SoloCamera == vcam)
			{
				return true;
			}
			if (this.mCurrentLiveCameras.Uses(vcam))
			{
				return true;
			}
			ICinemachineCamera cinemachineCamera = vcam.ParentCamera;
			while (cinemachineCamera != null && cinemachineCamera.IsLiveChild(vcam, dominantChildOnly))
			{
				if (CinemachineBrain.SoloCamera == cinemachineCamera || this.mCurrentLiveCameras.Uses(cinemachineCamera))
				{
					return true;
				}
				vcam = cinemachineCamera;
				cinemachineCamera = vcam.ParentCamera;
			}
			return false;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003F00 File Offset: 0x00002100
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003F08 File Offset: 0x00002108
		public CameraState CurrentCameraState { get; private set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00003F14 File Offset: 0x00002114
		protected virtual ICinemachineCamera TopCameraFromPriorityQueue()
		{
			CinemachineCore instance = CinemachineCore.Instance;
			Camera outputCamera = this.OutputCamera;
			int num = ((outputCamera == null) ? (-1) : outputCamera.cullingMask);
			int virtualCameraCount = instance.VirtualCameraCount;
			for (int i = 0; i < virtualCameraCount; i++)
			{
				CinemachineVirtualCameraBase virtualCamera = instance.GetVirtualCamera(i);
				GameObject gameObject = ((virtualCamera != null) ? virtualCamera.gameObject : null);
				if (gameObject != null && (num & (1 << gameObject.layer)) != 0)
				{
					return virtualCamera;
				}
			}
			return null;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003F98 File Offset: 0x00002198
		private CinemachineBlendDefinition LookupBlend(ICinemachineCamera fromKey, ICinemachineCamera toKey)
		{
			CinemachineBlendDefinition cinemachineBlendDefinition = this.m_DefaultBlend;
			if (this.m_CustomBlends != null)
			{
				string text = ((fromKey != null) ? fromKey.Name : string.Empty);
				string text2 = ((toKey != null) ? toKey.Name : string.Empty);
				cinemachineBlendDefinition = this.m_CustomBlends.GetBlendForVirtualCameras(text, text2, cinemachineBlendDefinition);
			}
			if (CinemachineCore.GetBlendOverride != null)
			{
				cinemachineBlendDefinition = CinemachineCore.GetBlendOverride(fromKey, toKey, cinemachineBlendDefinition, this);
			}
			return cinemachineBlendDefinition;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004004 File Offset: 0x00002204
		private void PushStateToUnityCamera(ref CameraState state)
		{
			this.CurrentCameraState = state;
			Transform transform = this.ControlledObject.transform;
			Vector3 vector = transform.position;
			Quaternion quaternion = transform.rotation;
			if ((state.BlendHint & CameraState.BlendHintValue.NoPosition) == CameraState.BlendHintValue.Nothing)
			{
				vector = state.FinalPosition;
			}
			if ((state.BlendHint & CameraState.BlendHintValue.NoOrientation) == CameraState.BlendHintValue.Nothing)
			{
				quaternion = state.FinalOrientation;
			}
			transform.ConservativeSetPositionAndRotation(vector, quaternion);
			if ((state.BlendHint & CameraState.BlendHintValue.NoLens) == CameraState.BlendHintValue.Nothing)
			{
				Camera outputCamera = this.OutputCamera;
				if (outputCamera != null)
				{
					outputCamera.nearClipPlane = state.Lens.NearClipPlane;
					outputCamera.farClipPlane = state.Lens.FarClipPlane;
					outputCamera.orthographicSize = state.Lens.OrthographicSize;
					outputCamera.fieldOfView = state.Lens.FieldOfView;
					outputCamera.lensShift = state.Lens.LensShift;
					if (state.Lens.ModeOverride != LensSettings.OverrideModes.None)
					{
						outputCamera.orthographic = state.Lens.Orthographic;
					}
					bool flag = ((state.Lens.ModeOverride == LensSettings.OverrideModes.None) ? outputCamera.usePhysicalProperties : state.Lens.IsPhysicalCamera);
					outputCamera.usePhysicalProperties = flag;
					if (flag && state.Lens.IsPhysicalCamera)
					{
						outputCamera.sensorSize = state.Lens.SensorSize;
						outputCamera.gateFit = state.Lens.GateFit;
						outputCamera.focalLength = Camera.FieldOfViewToFocalLength(state.Lens.FieldOfView, state.Lens.SensorSize.y);
						outputCamera.focusDistance = state.Lens.FocusDistance;
					}
				}
			}
			if (CinemachineCore.CameraUpdatedEvent != null)
			{
				CinemachineCore.CameraUpdatedEvent.Invoke(this);
			}
		}

		// Token: 0x04000027 RID: 39
		[Tooltip("When enabled, the current camera and blend will be indicated in the game window, for debugging")]
		public bool m_ShowDebugText;

		// Token: 0x04000028 RID: 40
		[Tooltip("When enabled, the camera's frustum will be shown at all times in the scene view")]
		public bool m_ShowCameraFrustum = true;

		// Token: 0x04000029 RID: 41
		[Tooltip("When enabled, the cameras will always respond in real-time to user input and damping, even if the game is running in slow motion")]
		public bool m_IgnoreTimeScale;

		// Token: 0x0400002A RID: 42
		[Tooltip("If set, this object's Y axis will define the worldspace Up vector for all the virtual cameras.  This is useful for instance in top-down game environments.  If not set, Up is worldspace Y.  Setting this appropriately is important, because Virtual Cameras don't like looking straight up or straight down.")]
		public Transform m_WorldUpOverride;

		// Token: 0x0400002B RID: 43
		[Tooltip("The update time for the vcams.  Use FixedUpdate if all your targets are animated during FixedUpdate (e.g. RigidBodies), LateUpdate if all your targets are animated during the normal Update loop, and SmartUpdate if you want Cinemachine to do the appropriate thing on a per-target basis.  SmartUpdate is the recommended setting")]
		public CinemachineBrain.UpdateMethod m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;

		// Token: 0x0400002C RID: 44
		[Tooltip("The update time for the Brain, i.e. when the blends are evaluated and the brain's transform is updated")]
		public CinemachineBrain.BrainUpdateMethod m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;

		// Token: 0x0400002D RID: 45
		[CinemachineBlendDefinitionProperty]
		[Tooltip("The blend that is used in cases where you haven't explicitly defined a blend between two Virtual Cameras")]
		public CinemachineBlendDefinition m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);

		// Token: 0x0400002E RID: 46
		[Tooltip("This is the asset that contains custom settings for blends between specific virtual cameras in your scene")]
		public CinemachineBlenderSettings m_CustomBlends;

		// Token: 0x0400002F RID: 47
		private Camera m_OutputCamera;

		// Token: 0x04000030 RID: 48
		private GameObject m_TargetOverride;

		// Token: 0x04000031 RID: 49
		[Tooltip("This event will fire whenever a virtual camera goes live and there is no blend")]
		public CinemachineBrain.BrainEvent m_CameraCutEvent = new CinemachineBrain.BrainEvent();

		// Token: 0x04000032 RID: 50
		[Tooltip("This event will fire whenever a virtual camera goes live.  If a blend is involved, then the event will fire on the first frame of the blend.")]
		public CinemachineBrain.VcamActivatedEvent m_CameraActivatedEvent = new CinemachineBrain.VcamActivatedEvent();

		// Token: 0x04000033 RID: 51
		private static ICinemachineCamera mSoloCamera;

		// Token: 0x04000034 RID: 52
		private Coroutine mPhysicsCoroutine;

		// Token: 0x04000035 RID: 53
		private int m_LastFrameUpdated;

		// Token: 0x04000036 RID: 54
		private WaitForFixedUpdate mWaitForFixedUpdate = new WaitForFixedUpdate();

		// Token: 0x04000037 RID: 55
		private List<CinemachineBrain.BrainFrame> mFrameStack = new List<CinemachineBrain.BrainFrame>();

		// Token: 0x04000038 RID: 56
		private int mNextFrameId = 1;

		// Token: 0x04000039 RID: 57
		private CinemachineBlend mCurrentLiveCameras = new CinemachineBlend(null, null, null, 0f, 0f);

		// Token: 0x0400003A RID: 58
		private static readonly AnimationCurve mDefaultLinearAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400003B RID: 59
		private ICinemachineCamera mActiveCameraPreviousFrame;

		// Token: 0x0400003C RID: 60
		private GameObject mActiveCameraPreviousFrameGameObject;

		// Token: 0x0200006F RID: 111
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum UpdateMethod
		{
			// Token: 0x040002B0 RID: 688
			FixedUpdate,
			// Token: 0x040002B1 RID: 689
			LateUpdate,
			// Token: 0x040002B2 RID: 690
			SmartUpdate,
			// Token: 0x040002B3 RID: 691
			ManualUpdate
		}

		// Token: 0x02000070 RID: 112
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum BrainUpdateMethod
		{
			// Token: 0x040002B5 RID: 693
			FixedUpdate,
			// Token: 0x040002B6 RID: 694
			LateUpdate
		}

		// Token: 0x02000071 RID: 113
		[Serializable]
		public class BrainEvent : UnityEvent<CinemachineBrain>
		{
		}

		// Token: 0x02000072 RID: 114
		[Serializable]
		public class VcamActivatedEvent : UnityEvent<ICinemachineCamera, ICinemachineCamera>
		{
		}

		// Token: 0x02000073 RID: 115
		private class BrainFrame
		{
			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x0600040D RID: 1037 RVA: 0x00018796 File Offset: 0x00016996
			public bool Active
			{
				get
				{
					return this.blend.IsValid;
				}
			}

			// Token: 0x040002B7 RID: 695
			public int id;

			// Token: 0x040002B8 RID: 696
			public CinemachineBlend blend = new CinemachineBlend(null, null, null, 0f, 0f);

			// Token: 0x040002B9 RID: 697
			public CinemachineBlend workingBlend = new CinemachineBlend(null, null, null, 0f, 0f);

			// Token: 0x040002BA RID: 698
			public BlendSourceVirtualCamera workingBlendSource = new BlendSourceVirtualCamera(null);

			// Token: 0x040002BB RID: 699
			public float deltaTimeOverride;

			// Token: 0x040002BC RID: 700
			public float blendStartPosition;
		}
	}
}
