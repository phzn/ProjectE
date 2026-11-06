using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000035 RID: 53
	public sealed class CinemachineCore
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00011BAF File Offset: 0x0000FDAF
		public static CinemachineCore Instance
		{
			get
			{
				if (CinemachineCore.sInstance == null)
				{
					CinemachineCore.sInstance = new CinemachineCore();
				}
				return CinemachineCore.sInstance;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00011BC7 File Offset: 0x0000FDC7
		public static float DeltaTime
		{
			get
			{
				if (CinemachineCore.UniformDeltaTimeOverride < 0f)
				{
					return Time.deltaTime;
				}
				return CinemachineCore.UniformDeltaTimeOverride;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00011BE0 File Offset: 0x0000FDE0
		public static float CurrentTime
		{
			get
			{
				if (CinemachineCore.CurrentTimeOverride < 0f)
				{
					return Time.time;
				}
				return CinemachineCore.CurrentTimeOverride;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00011BF9 File Offset: 0x0000FDF9
		public int BrainCount
		{
			get
			{
				return this.mActiveBrains.Count;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00011C06 File Offset: 0x0000FE06
		public CinemachineBrain GetActiveBrain(int index)
		{
			return this.mActiveBrains[index];
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00011C14 File Offset: 0x0000FE14
		internal void AddActiveBrain(CinemachineBrain brain)
		{
			this.RemoveActiveBrain(brain);
			this.mActiveBrains.Insert(0, brain);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00011C2A File Offset: 0x0000FE2A
		internal void RemoveActiveBrain(CinemachineBrain brain)
		{
			this.mActiveBrains.Remove(brain);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00011C39 File Offset: 0x0000FE39
		public int VirtualCameraCount
		{
			get
			{
				return this.mActiveCameras.Count;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00011C48 File Offset: 0x0000FE48
		public CinemachineVirtualCameraBase GetVirtualCamera(int index)
		{
			if (!this.m_ActiveCamerasAreSorted && this.mActiveCameras.Count > 1)
			{
				this.mActiveCameras.Sort(delegate(CinemachineVirtualCameraBase x, CinemachineVirtualCameraBase y)
				{
					if (x.Priority != y.Priority)
					{
						return y.Priority.CompareTo(x.Priority);
					}
					return y.m_ActivationId.CompareTo(x.m_ActivationId);
				});
				this.m_ActiveCamerasAreSorted = true;
			}
			return this.mActiveCameras[index];
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00011CA8 File Offset: 0x0000FEA8
		internal void AddActiveCamera(CinemachineVirtualCameraBase vcam)
		{
			int activationSequence = this.m_ActivationSequence;
			this.m_ActivationSequence = activationSequence + 1;
			vcam.m_ActivationId = activationSequence;
			this.mActiveCameras.Add(vcam);
			this.m_ActiveCamerasAreSorted = false;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00011CDF File Offset: 0x0000FEDF
		internal void RemoveActiveCamera(CinemachineVirtualCameraBase vcam)
		{
			if (this.mActiveCameras.Contains(vcam))
			{
				this.mActiveCameras.Remove(vcam);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00011CFC File Offset: 0x0000FEFC
		internal void CameraDestroyed(CinemachineVirtualCameraBase vcam)
		{
			if (this.mActiveCameras.Contains(vcam))
			{
				this.mActiveCameras.Remove(vcam);
			}
			if (this.mUpdateStatus != null && this.mUpdateStatus.ContainsKey(vcam))
			{
				this.mUpdateStatus.Remove(vcam);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00011D3C File Offset: 0x0000FF3C
		internal void CameraEnabled(CinemachineVirtualCameraBase vcam)
		{
			int num = 0;
			for (ICinemachineCamera cinemachineCamera = vcam.ParentCamera; cinemachineCamera != null; cinemachineCamera = cinemachineCamera.ParentCamera)
			{
				num++;
			}
			while (this.mAllCameras.Count <= num)
			{
				this.mAllCameras.Add(new List<CinemachineVirtualCameraBase>());
			}
			this.mAllCameras[num].Add(vcam);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00011D94 File Offset: 0x0000FF94
		internal void CameraDisabled(CinemachineVirtualCameraBase vcam)
		{
			for (int i = 0; i < this.mAllCameras.Count; i++)
			{
				this.mAllCameras[i].Remove(vcam);
			}
			if (this.mRoundRobinVcamLastFrame == vcam)
			{
				this.mRoundRobinVcamLastFrame = null;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00011DE0 File Offset: 0x0000FFE0
		internal void UpdateAllActiveVirtualCameras(int layerMask, Vector3 worldUp, float deltaTime)
		{
			CinemachineCore.UpdateFilter currentUpdateFilter = this.m_CurrentUpdateFilter;
			bool flag = currentUpdateFilter != CinemachineCore.UpdateFilter.Smart;
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = this.mRoundRobinVcamLastFrame;
			float currentTime = CinemachineCore.CurrentTime;
			if (currentTime != CinemachineCore.s_LastUpdateTime)
			{
				CinemachineCore.s_LastUpdateTime = currentTime;
				if ((currentUpdateFilter & (CinemachineCore.UpdateFilter)(-9)) == CinemachineCore.UpdateFilter.Fixed)
				{
					CinemachineCore.s_FixedFrameCount++;
				}
			}
			for (int i = this.mAllCameras.Count - 1; i >= 0; i--)
			{
				List<CinemachineVirtualCameraBase> list = this.mAllCameras[i];
				for (int j = list.Count - 1; j >= 0; j--)
				{
					CinemachineVirtualCameraBase cinemachineVirtualCameraBase2 = list[j];
					if (flag && cinemachineVirtualCameraBase2 == this.mRoundRobinVcamLastFrame)
					{
						cinemachineVirtualCameraBase = null;
					}
					if (cinemachineVirtualCameraBase2 == null)
					{
						list.RemoveAt(j);
					}
					else if (cinemachineVirtualCameraBase2.m_StandbyUpdate == CinemachineVirtualCameraBase.StandbyUpdateMode.Always || this.IsLive(cinemachineVirtualCameraBase2))
					{
						if (((1 << cinemachineVirtualCameraBase2.gameObject.layer) & layerMask) != 0)
						{
							this.UpdateVirtualCamera(cinemachineVirtualCameraBase2, worldUp, deltaTime);
						}
					}
					else if (cinemachineVirtualCameraBase == null && this.mRoundRobinVcamLastFrame != cinemachineVirtualCameraBase2 && flag && cinemachineVirtualCameraBase2.m_StandbyUpdate != CinemachineVirtualCameraBase.StandbyUpdateMode.Never && cinemachineVirtualCameraBase2.isActiveAndEnabled)
					{
						this.m_CurrentUpdateFilter &= (CinemachineCore.UpdateFilter)(-9);
						this.UpdateVirtualCamera(cinemachineVirtualCameraBase2, worldUp, deltaTime);
						this.m_CurrentUpdateFilter = currentUpdateFilter;
						cinemachineVirtualCameraBase = cinemachineVirtualCameraBase2;
					}
				}
			}
			if (flag)
			{
				if (cinemachineVirtualCameraBase == this.mRoundRobinVcamLastFrame)
				{
					cinemachineVirtualCameraBase = null;
				}
				this.mRoundRobinVcamLastFrame = cinemachineVirtualCameraBase;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00011F50 File Offset: 0x00010150
		internal void UpdateVirtualCamera(CinemachineVirtualCameraBase vcam, Vector3 worldUp, float deltaTime)
		{
			if (vcam == null)
			{
				return;
			}
			bool flag = (this.m_CurrentUpdateFilter & CinemachineCore.UpdateFilter.Smart) == CinemachineCore.UpdateFilter.Smart;
			UpdateTracker.UpdateClock updateClock = (UpdateTracker.UpdateClock)(this.m_CurrentUpdateFilter & (CinemachineCore.UpdateFilter)(-9));
			if (flag)
			{
				Transform updateTarget = CinemachineCore.GetUpdateTarget(vcam);
				if (updateTarget == null)
				{
					return;
				}
				if (UpdateTracker.GetPreferredUpdate(updateTarget) != updateClock)
				{
					return;
				}
			}
			if (this.mUpdateStatus == null)
			{
				this.mUpdateStatus = new Dictionary<CinemachineVirtualCameraBase, CinemachineCore.UpdateStatus>();
			}
			CinemachineCore.UpdateStatus updateStatus;
			if (!this.mUpdateStatus.TryGetValue(vcam, out updateStatus))
			{
				updateStatus = new CinemachineCore.UpdateStatus
				{
					lastUpdateDeltaTime = -2f,
					lastUpdateMode = UpdateTracker.UpdateClock.Late,
					lastUpdateFrame = Time.frameCount + 2,
					lastUpdateFixedFrame = CinemachineCore.s_FixedFrameCount + 2
				};
				this.mUpdateStatus.Add(vcam, updateStatus);
			}
			int num = ((updateClock == UpdateTracker.UpdateClock.Late) ? (Time.frameCount - updateStatus.lastUpdateFrame) : (CinemachineCore.s_FixedFrameCount - updateStatus.lastUpdateFixedFrame));
			if (deltaTime >= 0f)
			{
				if (num == 0 && updateStatus.lastUpdateMode == updateClock && updateStatus.lastUpdateDeltaTime == deltaTime)
				{
					return;
				}
				if (CinemachineCore.FrameDeltaCompensationEnabled && num > 0)
				{
					deltaTime *= (float)num;
				}
			}
			vcam.InternalUpdateCameraState(worldUp, deltaTime);
			updateStatus.lastUpdateFrame = Time.frameCount;
			updateStatus.lastUpdateFixedFrame = CinemachineCore.s_FixedFrameCount;
			updateStatus.lastUpdateMode = updateClock;
			updateStatus.lastUpdateDeltaTime = deltaTime;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00012075 File Offset: 0x00010275
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeModule()
		{
			CinemachineCore.Instance.mUpdateStatus = new Dictionary<CinemachineVirtualCameraBase, CinemachineCore.UpdateStatus>();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00012088 File Offset: 0x00010288
		private static Transform GetUpdateTarget(CinemachineVirtualCameraBase vcam)
		{
			if (vcam == null || vcam.gameObject == null)
			{
				return null;
			}
			Transform transform = vcam.LookAt;
			if (transform != null)
			{
				return transform;
			}
			transform = vcam.Follow;
			if (transform != null)
			{
				return transform;
			}
			return vcam.transform;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000120D8 File Offset: 0x000102D8
		internal UpdateTracker.UpdateClock GetVcamUpdateStatus(CinemachineVirtualCameraBase vcam)
		{
			CinemachineCore.UpdateStatus updateStatus;
			if (this.mUpdateStatus == null || !this.mUpdateStatus.TryGetValue(vcam, out updateStatus))
			{
				return UpdateTracker.UpdateClock.Late;
			}
			return updateStatus.lastUpdateMode;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00012108 File Offset: 0x00010308
		public bool IsLive(ICinemachineCamera vcam)
		{
			if (vcam != null)
			{
				for (int i = 0; i < this.BrainCount; i++)
				{
					CinemachineBrain activeBrain = this.GetActiveBrain(i);
					if (activeBrain != null && activeBrain.IsLive(vcam, false))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00012148 File Offset: 0x00010348
		public bool IsLiveInBlend(ICinemachineCamera vcam)
		{
			if (vcam != null)
			{
				for (int i = 0; i < this.BrainCount; i++)
				{
					CinemachineBrain activeBrain = this.GetActiveBrain(i);
					if (activeBrain != null && activeBrain.IsLiveInBlend(vcam))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00012188 File Offset: 0x00010388
		public void GenerateCameraActivationEvent(ICinemachineCamera vcam, ICinemachineCamera vcamFrom)
		{
			if (vcam != null)
			{
				for (int i = 0; i < this.BrainCount; i++)
				{
					CinemachineBrain activeBrain = this.GetActiveBrain(i);
					if (activeBrain != null && activeBrain.IsLive(vcam, false))
					{
						activeBrain.m_CameraActivatedEvent.Invoke(vcam, vcamFrom);
					}
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000121D4 File Offset: 0x000103D4
		public void GenerateCameraCutEvent(ICinemachineCamera vcam)
		{
			if (vcam != null)
			{
				for (int i = 0; i < this.BrainCount; i++)
				{
					CinemachineBrain activeBrain = this.GetActiveBrain(i);
					if (activeBrain != null && activeBrain.IsLive(vcam, false))
					{
						if (activeBrain.m_CameraCutEvent != null)
						{
							activeBrain.m_CameraCutEvent.Invoke(activeBrain);
						}
						if (CinemachineCore.CameraCutEvent != null)
						{
							CinemachineCore.CameraCutEvent.Invoke(activeBrain);
						}
					}
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00012238 File Offset: 0x00010438
		public CinemachineBrain FindPotentialTargetBrain(CinemachineVirtualCameraBase vcam)
		{
			if (vcam != null)
			{
				int brainCount = this.BrainCount;
				for (int i = 0; i < brainCount; i++)
				{
					CinemachineBrain activeBrain = this.GetActiveBrain(i);
					if (activeBrain != null && activeBrain.OutputCamera != null && activeBrain.IsLive(vcam, false))
					{
						return activeBrain;
					}
				}
				int num = 1 << vcam.gameObject.layer;
				for (int j = 0; j < brainCount; j++)
				{
					CinemachineBrain activeBrain2 = this.GetActiveBrain(j);
					if (activeBrain2 != null && activeBrain2.OutputCamera != null && (activeBrain2.OutputCamera.cullingMask & num) != 0)
					{
						return activeBrain2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000122E8 File Offset: 0x000104E8
		public void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			int virtualCameraCount = this.VirtualCameraCount;
			for (int i = 0; i < virtualCameraCount; i++)
			{
				this.GetVirtualCamera(i).OnTargetObjectWarped(target, positionDelta);
			}
		}

		// Token: 0x040001CA RID: 458
		public static readonly int kStreamingVersion = 20170927;

		// Token: 0x040001CB RID: 459
		private static CinemachineCore sInstance = null;

		// Token: 0x040001CC RID: 460
		public static bool sShowHiddenObjects = false;

		// Token: 0x040001CD RID: 461
		public static CinemachineCore.AxisInputDelegate GetInputAxis = new CinemachineCore.AxisInputDelegate(Input.GetAxis);

		// Token: 0x040001CE RID: 462
		public static float UniformDeltaTimeOverride = -1f;

		// Token: 0x040001CF RID: 463
		public static float CurrentTimeOverride = -1f;

		// Token: 0x040001D0 RID: 464
		public static CinemachineCore.GetBlendOverrideDelegate GetBlendOverride;

		// Token: 0x040001D1 RID: 465
		public static CinemachineBrain.BrainEvent CameraUpdatedEvent = new CinemachineBrain.BrainEvent();

		// Token: 0x040001D2 RID: 466
		public static CinemachineBrain.BrainEvent CameraCutEvent = new CinemachineBrain.BrainEvent();

		// Token: 0x040001D3 RID: 467
		private List<CinemachineBrain> mActiveBrains = new List<CinemachineBrain>();

		// Token: 0x040001D4 RID: 468
		internal static bool FrameDeltaCompensationEnabled = true;

		// Token: 0x040001D5 RID: 469
		private List<CinemachineVirtualCameraBase> mActiveCameras = new List<CinemachineVirtualCameraBase>();

		// Token: 0x040001D6 RID: 470
		private bool m_ActiveCamerasAreSorted;

		// Token: 0x040001D7 RID: 471
		private int m_ActivationSequence;

		// Token: 0x040001D8 RID: 472
		private List<List<CinemachineVirtualCameraBase>> mAllCameras = new List<List<CinemachineVirtualCameraBase>>();

		// Token: 0x040001D9 RID: 473
		private CinemachineVirtualCameraBase mRoundRobinVcamLastFrame;

		// Token: 0x040001DA RID: 474
		private static float s_LastUpdateTime;

		// Token: 0x040001DB RID: 475
		private static int s_FixedFrameCount;

		// Token: 0x040001DC RID: 476
		private Dictionary<CinemachineVirtualCameraBase, CinemachineCore.UpdateStatus> mUpdateStatus;

		// Token: 0x040001DD RID: 477
		internal CinemachineCore.UpdateFilter m_CurrentUpdateFilter;

		// Token: 0x020000A7 RID: 167
		public enum Stage
		{
			// Token: 0x04000372 RID: 882
			Body,
			// Token: 0x04000373 RID: 883
			Aim,
			// Token: 0x04000374 RID: 884
			Noise,
			// Token: 0x04000375 RID: 885
			Finalize
		}

		// Token: 0x020000A8 RID: 168
		// (Invoke) Token: 0x06000452 RID: 1106
		public delegate float AxisInputDelegate(string axisName);

		// Token: 0x020000A9 RID: 169
		// (Invoke) Token: 0x06000456 RID: 1110
		public delegate CinemachineBlendDefinition GetBlendOverrideDelegate(ICinemachineCamera fromVcam, ICinemachineCamera toVcam, CinemachineBlendDefinition defaultBlend, MonoBehaviour owner);

		// Token: 0x020000AA RID: 170
		private class UpdateStatus
		{
			// Token: 0x04000376 RID: 886
			public int lastUpdateFrame;

			// Token: 0x04000377 RID: 887
			public int lastUpdateFixedFrame;

			// Token: 0x04000378 RID: 888
			public UpdateTracker.UpdateClock lastUpdateMode;

			// Token: 0x04000379 RID: 889
			public float lastUpdateDeltaTime;
		}

		// Token: 0x020000AB RID: 171
		internal enum UpdateFilter
		{
			// Token: 0x0400037B RID: 891
			Fixed,
			// Token: 0x0400037C RID: 892
			Late,
			// Token: 0x0400037D RID: 893
			Smart = 8,
			// Token: 0x0400037E RID: 894
			SmartFixed = 8,
			// Token: 0x0400037F RID: 895
			SmartLate
		}
	}
}
