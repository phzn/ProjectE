using System;
using System.Collections.Generic;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200000E RID: 14
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[ExcludeFromPreset]
	[AddComponentMenu("Cinemachine/CinemachineClearShot")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineClearShot.html")]
	public class CinemachineClearShot : CinemachineVirtualCameraBase
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000423C File Offset: 0x0000243C
		public override string Description
		{
			get
			{
				if (this.mActiveBlend != null)
				{
					return this.mActiveBlend.Description;
				}
				ICinemachineCamera liveChild = this.LiveChild;
				if (liveChild == null)
				{
					return "(none)";
				}
				StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
				stringBuilder.Append("[");
				stringBuilder.Append(liveChild.Name);
				stringBuilder.Append("]");
				string text = stringBuilder.ToString();
				CinemachineDebug.ReturnToPool(stringBuilder);
				return text;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000042A4 File Offset: 0x000024A4
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000042AC File Offset: 0x000024AC
		public ICinemachineCamera LiveChild { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000042B5 File Offset: 0x000024B5
		public override CameraState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000042BD File Offset: 0x000024BD
		public override bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			return vcam == this.LiveChild || (this.mActiveBlend != null && this.mActiveBlend.Uses(vcam));
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000042E0 File Offset: 0x000024E0
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000042EE File Offset: 0x000024EE
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000042F7 File Offset: 0x000024F7
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00004305 File Offset: 0x00002505
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

		// Token: 0x0600006F RID: 111 RVA: 0x00004310 File Offset: 0x00002510
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			this.UpdateListOfChildren();
			CinemachineVirtualCameraBase[] childCameras = this.m_ChildCameras;
			for (int i = 0; i < childCameras.Length; i++)
			{
				childCameras[i].OnTargetObjectWarped(target, positionDelta);
			}
			base.OnTargetObjectWarped(target, positionDelta);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000434C File Offset: 0x0000254C
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			this.UpdateListOfChildren();
			CinemachineVirtualCameraBase[] childCameras = this.m_ChildCameras;
			for (int i = 0; i < childCameras.Length; i++)
			{
				childCameras[i].ForceCameraPosition(pos, rot);
			}
			base.ForceCameraPosition(pos, rot);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004388 File Offset: 0x00002588
		public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			this.UpdateListOfChildren();
			ICinemachineCamera liveChild = this.LiveChild;
			this.LiveChild = this.ChooseCurrentCamera(worldUp);
			if (liveChild != this.LiveChild && this.LiveChild != null)
			{
				this.LiveChild.OnTransitionFromCamera(liveChild, worldUp, deltaTime);
				CinemachineCore.Instance.GenerateCameraActivationEvent(this.LiveChild, liveChild);
				if (liveChild != null)
				{
					this.mActiveBlend = base.CreateBlend(liveChild, this.LiveChild, this.LookupBlend(liveChild, this.LiveChild), this.mActiveBlend);
					if (this.mActiveBlend == null || !this.mActiveBlend.Uses(liveChild))
					{
						CinemachineCore.Instance.GenerateCameraCutEvent(this.LiveChild);
					}
				}
			}
			if (this.mActiveBlend != null)
			{
				this.mActiveBlend.TimeInBlend += ((deltaTime >= 0f) ? deltaTime : this.mActiveBlend.Duration);
				if (this.mActiveBlend.IsComplete)
				{
					this.mActiveBlend = null;
				}
			}
			if (this.mActiveBlend != null)
			{
				this.mActiveBlend.UpdateCameraState(worldUp, deltaTime);
				this.m_State = this.mActiveBlend.State;
			}
			else if (this.LiveChild != null)
			{
				if (this.m_TransitioningFrom != null)
				{
					this.LiveChild.OnTransitionFromCamera(this.m_TransitioningFrom, worldUp, deltaTime);
				}
				this.m_State = this.LiveChild.State;
			}
			this.m_TransitioningFrom = null;
			base.InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref this.m_State, deltaTime);
			this.PreviousStateIsValid = true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000044EC File Offset: 0x000026EC
		protected override void OnEnable()
		{
			base.OnEnable();
			this.InvalidateListOfChildren();
			this.mActiveBlend = null;
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Combine(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000454C File Offset: 0x0000274C
		protected override void OnDisable()
		{
			base.OnDisable();
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004574 File Offset: 0x00002774
		public void OnTransformChildrenChanged()
		{
			this.InvalidateListOfChildren();
			this.UpdateListOfChildren();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004584 File Offset: 0x00002784
		private void OnGuiHandler()
		{
			if (!this.m_ShowDebugText)
			{
				CinemachineDebug.ReleaseScreenPos(this);
				return;
			}
			StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
			stringBuilder.Append(base.Name);
			stringBuilder.Append(": ");
			stringBuilder.Append(this.Description);
			string text = stringBuilder.ToString();
			GUI.Label(CinemachineDebug.GetScreenPos(this, text, GUI.skin.box), text, GUI.skin.box);
			CinemachineDebug.ReturnToPool(stringBuilder);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000045F8 File Offset: 0x000027F8
		public bool IsBlending
		{
			get
			{
				return this.mActiveBlend != null;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00004603 File Offset: 0x00002803
		public CinemachineBlend ActiveBlend
		{
			get
			{
				return this.mActiveBlend;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000460B File Offset: 0x0000280B
		public CinemachineVirtualCameraBase[] ChildCameras
		{
			get
			{
				this.UpdateListOfChildren();
				return this.m_ChildCameras;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004619 File Offset: 0x00002819
		private void InvalidateListOfChildren()
		{
			this.m_ChildCameras = null;
			this.m_RandomizedChilden = null;
			this.LiveChild = null;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004630 File Offset: 0x00002830
		public void ResetRandomization()
		{
			this.m_RandomizedChilden = null;
			this.mRandomizeNow = true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004640 File Offset: 0x00002840
		private void UpdateListOfChildren()
		{
			if (this.m_ChildCameras != null)
			{
				return;
			}
			List<CinemachineVirtualCameraBase> list = new List<CinemachineVirtualCameraBase>();
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in base.GetComponentsInChildren<CinemachineVirtualCameraBase>(true))
			{
				if (cinemachineVirtualCameraBase.transform.parent == base.transform)
				{
					list.Add(cinemachineVirtualCameraBase);
				}
			}
			this.m_ChildCameras = list.ToArray();
			this.mActivationTime = (this.mPendingActivationTime = 0f);
			this.mPendingCamera = null;
			this.LiveChild = null;
			this.mActiveBlend = null;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000046CC File Offset: 0x000028CC
		private ICinemachineCamera ChooseCurrentCamera(Vector3 worldUp)
		{
			if (this.m_ChildCameras == null || this.m_ChildCameras.Length == 0)
			{
				this.mActivationTime = 0f;
				return null;
			}
			CinemachineVirtualCameraBase[] array = this.m_ChildCameras;
			if (!this.m_RandomizeChoice)
			{
				this.m_RandomizedChilden = null;
			}
			else if (this.m_ChildCameras.Length > 1)
			{
				if (this.m_RandomizedChilden == null)
				{
					this.m_RandomizedChilden = this.Randomize(this.m_ChildCameras);
				}
				array = this.m_RandomizedChilden;
			}
			if (this.LiveChild != null && !this.LiveChild.VirtualCameraGameObject.activeSelf)
			{
				this.LiveChild = null;
			}
			ICinemachineCamera cinemachineCamera = this.LiveChild;
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in array)
			{
				if (cinemachineVirtualCameraBase != null && cinemachineVirtualCameraBase.gameObject.activeInHierarchy && (cinemachineCamera == null || cinemachineVirtualCameraBase.State.ShotQuality > cinemachineCamera.State.ShotQuality || (cinemachineVirtualCameraBase.State.ShotQuality == cinemachineCamera.State.ShotQuality && cinemachineVirtualCameraBase.Priority > cinemachineCamera.Priority) || (this.m_RandomizeChoice && this.mRandomizeNow && cinemachineVirtualCameraBase != this.LiveChild && cinemachineVirtualCameraBase.State.ShotQuality == cinemachineCamera.State.ShotQuality && cinemachineVirtualCameraBase.Priority == cinemachineCamera.Priority)))
				{
					cinemachineCamera = cinemachineVirtualCameraBase;
				}
			}
			this.mRandomizeNow = false;
			float currentTime = CinemachineCore.CurrentTime;
			if (this.mActivationTime != 0f)
			{
				if (this.LiveChild == cinemachineCamera)
				{
					this.mPendingActivationTime = 0f;
					this.mPendingCamera = null;
					return cinemachineCamera;
				}
				if (this.PreviousStateIsValid && this.mPendingActivationTime != 0f && this.mPendingCamera == cinemachineCamera)
				{
					if (currentTime - this.mPendingActivationTime > this.m_ActivateAfter && currentTime - this.mActivationTime > this.m_MinDuration)
					{
						this.m_RandomizedChilden = null;
						this.mActivationTime = currentTime;
						this.mPendingActivationTime = 0f;
						this.mPendingCamera = null;
						return cinemachineCamera;
					}
					return this.LiveChild;
				}
			}
			this.mPendingActivationTime = 0f;
			this.mPendingCamera = null;
			if (this.PreviousStateIsValid && this.mActivationTime > 0f && (this.m_ActivateAfter > 0f || currentTime - this.mActivationTime < this.m_MinDuration))
			{
				this.mPendingCamera = cinemachineCamera;
				this.mPendingActivationTime = currentTime;
				return this.LiveChild;
			}
			this.m_RandomizedChilden = null;
			this.mActivationTime = currentTime;
			return cinemachineCamera;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004930 File Offset: 0x00002B30
		private CinemachineVirtualCameraBase[] Randomize(CinemachineVirtualCameraBase[] src)
		{
			List<CinemachineClearShot.Pair> list = new List<CinemachineClearShot.Pair>();
			for (int i = 0; i < src.Length; i++)
			{
				list.Add(new CinemachineClearShot.Pair
				{
					a = i,
					b = Random.Range(0f, 1000f)
				});
			}
			list.Sort((CinemachineClearShot.Pair p1, CinemachineClearShot.Pair p2) => (int)p1.b - (int)p2.b);
			CinemachineVirtualCameraBase[] array = new CinemachineVirtualCameraBase[src.Length];
			CinemachineClearShot.Pair[] array2 = list.ToArray();
			for (int j = 0; j < src.Length; j++)
			{
				array[j] = src[array2[j].a];
			}
			return array;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000049DC File Offset: 0x00002BDC
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

		// Token: 0x0600007F RID: 127 RVA: 0x00004A48 File Offset: 0x00002C48
		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
			base.InvokeOnTransitionInExtensions(fromCam, worldUp, deltaTime);
			this.m_TransitioningFrom = fromCam;
			if (this.m_RandomizeChoice && this.mActiveBlend == null)
			{
				this.m_RandomizedChilden = null;
				this.LiveChild = null;
			}
			this.InternalUpdateCameraState(worldUp, deltaTime);
		}

		// Token: 0x0400003E RID: 62
		[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all children specify targets of their own.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_LookAt;

		// Token: 0x0400003F RID: 63
		[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all children specify targets of their own.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_Follow;

		// Token: 0x04000040 RID: 64
		[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
		[NoSaveDuringPlay]
		public bool m_ShowDebugText;

		// Token: 0x04000041 RID: 65
		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		internal CinemachineVirtualCameraBase[] m_ChildCameras;

		// Token: 0x04000042 RID: 66
		[Tooltip("Wait this many seconds before activating a new child camera")]
		public float m_ActivateAfter;

		// Token: 0x04000043 RID: 67
		[Tooltip("An active camera must be active for at least this many seconds")]
		public float m_MinDuration;

		// Token: 0x04000044 RID: 68
		[Tooltip("If checked, camera choice will be randomized if multiple cameras are equally desirable.  Otherwise, child list order and child camera priority will be used.")]
		public bool m_RandomizeChoice;

		// Token: 0x04000045 RID: 69
		[CinemachineBlendDefinitionProperty]
		[Tooltip("The blend which is used if you don't explicitly define a blend between two Virtual Cameras")]
		public CinemachineBlendDefinition m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);

		// Token: 0x04000046 RID: 70
		[HideInInspector]
		public CinemachineBlenderSettings m_CustomBlends;

		// Token: 0x04000048 RID: 72
		private CameraState m_State = CameraState.Default;

		// Token: 0x04000049 RID: 73
		private float mActivationTime;

		// Token: 0x0400004A RID: 74
		private float mPendingActivationTime;

		// Token: 0x0400004B RID: 75
		private ICinemachineCamera mPendingCamera;

		// Token: 0x0400004C RID: 76
		private CinemachineBlend mActiveBlend;

		// Token: 0x0400004D RID: 77
		private bool mRandomizeNow;

		// Token: 0x0400004E RID: 78
		private CinemachineVirtualCameraBase[] m_RandomizedChilden;

		// Token: 0x0400004F RID: 79
		private ICinemachineCamera m_TransitioningFrom;

		// Token: 0x02000075 RID: 117
		private struct Pair
		{
			// Token: 0x040002C0 RID: 704
			public int a;

			// Token: 0x040002C1 RID: 705
			public float b;
		}
	}
}
