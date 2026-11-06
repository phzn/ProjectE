using System;
using System.Collections.Generic;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200000B RID: 11
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[ExcludeFromPreset]
	[AddComponentMenu("Cinemachine/CinemachineBlendListCamera")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineBlendListCamera.html")]
	public class CinemachineBlendListCamera : CinemachineVirtualCameraBase
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002968 File Offset: 0x00000B68
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

		// Token: 0x06000024 RID: 36 RVA: 0x000029D0 File Offset: 0x00000BD0
		private void Reset()
		{
			this.m_LookAt = null;
			this.m_Follow = null;
			this.m_ShowDebugText = false;
			this.m_Loop = false;
			this.m_Instructions = null;
			this.m_ChildCameras = null;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000029FC File Offset: 0x00000BFC
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002A04 File Offset: 0x00000C04
		public ICinemachineCamera LiveChild { get; set; }

		// Token: 0x06000027 RID: 39 RVA: 0x00002A0D File Offset: 0x00000C0D
		public override bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			return vcam == this.LiveChild || (this.mActiveBlend != null && this.mActiveBlend.Uses(vcam));
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002A30 File Offset: 0x00000C30
		public override CameraState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002A38 File Offset: 0x00000C38
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002A46 File Offset: 0x00000C46
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

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002A4F File Offset: 0x00000C4F
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002A5D File Offset: 0x00000C5D
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

		// Token: 0x0600002D RID: 45 RVA: 0x00002A68 File Offset: 0x00000C68
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

		// Token: 0x0600002E RID: 46 RVA: 0x00002AA4 File Offset: 0x00000CA4
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

		// Token: 0x0600002F RID: 47 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
			base.InvokeOnTransitionInExtensions(fromCam, worldUp, deltaTime);
			this.mActivationTime = CinemachineCore.CurrentTime;
			this.mCurrentInstruction = 0;
			this.LiveChild = null;
			this.mActiveBlend = null;
			this.m_TransitioningFrom = fromCam;
			this.InternalUpdateCameraState(worldUp, deltaTime);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B30 File Offset: 0x00000D30
		public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			if (!this.PreviousStateIsValid)
			{
				this.mCurrentInstruction = -1;
				this.mActiveBlend = null;
			}
			this.UpdateListOfChildren();
			this.AdvanceCurrentInstruction(deltaTime);
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = null;
			if (this.mCurrentInstruction >= 0 && this.mCurrentInstruction < this.m_Instructions.Length)
			{
				cinemachineVirtualCameraBase = this.m_Instructions[this.mCurrentInstruction].m_VirtualCamera;
			}
			if (cinemachineVirtualCameraBase != null)
			{
				if (!cinemachineVirtualCameraBase.gameObject.activeInHierarchy)
				{
					cinemachineVirtualCameraBase.gameObject.SetActive(true);
					cinemachineVirtualCameraBase.UpdateCameraState(worldUp, deltaTime);
				}
				ICinemachineCamera liveChild = this.LiveChild;
				this.LiveChild = cinemachineVirtualCameraBase;
				if (liveChild != this.LiveChild && this.LiveChild != null)
				{
					this.LiveChild.OnTransitionFromCamera(liveChild, worldUp, deltaTime);
					CinemachineCore.Instance.GenerateCameraActivationEvent(this.LiveChild, liveChild);
					if (liveChild != null)
					{
						this.mActiveBlend = base.CreateBlend(liveChild, this.LiveChild, this.m_Instructions[this.mCurrentInstruction].m_Blend, this.mActiveBlend);
						if (this.mActiveBlend == null || !this.mActiveBlend.Uses(liveChild))
						{
							CinemachineCore.Instance.GenerateCameraCutEvent(this.LiveChild);
						}
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

		// Token: 0x06000031 RID: 49 RVA: 0x00002D14 File Offset: 0x00000F14
		protected override void OnEnable()
		{
			base.OnEnable();
			this.InvalidateListOfChildren();
			this.LiveChild = null;
			this.mActiveBlend = null;
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Combine(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D7B File Offset: 0x00000F7B
		protected override void OnDisable()
		{
			base.OnDisable();
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002DA3 File Offset: 0x00000FA3
		private void OnTransformChildrenChanged()
		{
			this.InvalidateListOfChildren();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002DAC File Offset: 0x00000FAC
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002E20 File Offset: 0x00001020
		public CinemachineVirtualCameraBase[] ChildCameras
		{
			get
			{
				this.UpdateListOfChildren();
				return this.m_ChildCameras;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002E2E File Offset: 0x0000102E
		public bool IsBlending
		{
			get
			{
				return this.mActiveBlend != null;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002E39 File Offset: 0x00001039
		private void InvalidateListOfChildren()
		{
			this.m_ChildCameras = null;
			this.LiveChild = null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E4C File Offset: 0x0000104C
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
			this.ValidateInstructions();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002EB4 File Offset: 0x000010B4
		internal void ValidateInstructions()
		{
			if (this.m_Instructions == null)
			{
				this.m_Instructions = Array.Empty<CinemachineBlendListCamera.Instruction>();
			}
			for (int i = 0; i < this.m_Instructions.Length; i++)
			{
				if (this.m_Instructions[i].m_VirtualCamera != null && this.m_Instructions[i].m_VirtualCamera.transform.parent != base.transform)
				{
					this.m_Instructions[i].m_VirtualCamera = null;
				}
			}
			this.mActiveBlend = null;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002F44 File Offset: 0x00001144
		private void AdvanceCurrentInstruction(float deltaTime)
		{
			if (this.m_ChildCameras == null || this.m_ChildCameras.Length == 0 || this.mActivationTime < 0f || this.m_Instructions.Length == 0)
			{
				this.mActivationTime = -1f;
				this.mCurrentInstruction = -1;
				this.mActiveBlend = null;
				return;
			}
			float currentTime = CinemachineCore.CurrentTime;
			if (this.mCurrentInstruction < 0 || deltaTime < 0f)
			{
				this.mActivationTime = currentTime;
				this.mCurrentInstruction = 0;
			}
			if (this.mCurrentInstruction > this.m_Instructions.Length - 1)
			{
				this.mActivationTime = currentTime;
				this.mCurrentInstruction = this.m_Instructions.Length - 1;
			}
			float num = this.m_Instructions[this.mCurrentInstruction].m_Hold + this.m_Instructions[this.mCurrentInstruction].m_Blend.BlendTime;
			float num2 = ((this.mCurrentInstruction < this.m_Instructions.Length - 1 || this.m_Loop) ? 0f : float.MaxValue);
			if (currentTime - this.mActivationTime > Mathf.Max(num2, num))
			{
				this.mActivationTime = currentTime;
				this.mCurrentInstruction++;
				if (this.m_Loop && this.mCurrentInstruction == this.m_Instructions.Length)
				{
					this.mCurrentInstruction = 0;
				}
			}
		}

		// Token: 0x0400001B RID: 27
		[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_LookAt;

		// Token: 0x0400001C RID: 28
		[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_Follow;

		// Token: 0x0400001D RID: 29
		[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
		public bool m_ShowDebugText;

		// Token: 0x0400001E RID: 30
		[Tooltip("When enabled, the child vcams will cycle indefinitely instead of just stopping at the last one")]
		public bool m_Loop;

		// Token: 0x0400001F RID: 31
		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		internal CinemachineVirtualCameraBase[] m_ChildCameras;

		// Token: 0x04000020 RID: 32
		[Tooltip("The set of instructions for enabling child cameras.")]
		public CinemachineBlendListCamera.Instruction[] m_Instructions;

		// Token: 0x04000022 RID: 34
		private ICinemachineCamera m_TransitioningFrom;

		// Token: 0x04000023 RID: 35
		private CameraState m_State = CameraState.Default;

		// Token: 0x04000024 RID: 36
		private float mActivationTime = -1f;

		// Token: 0x04000025 RID: 37
		private int mCurrentInstruction;

		// Token: 0x04000026 RID: 38
		private CinemachineBlend mActiveBlend;

		// Token: 0x0200006E RID: 110
		[Serializable]
		public struct Instruction
		{
			// Token: 0x040002AC RID: 684
			[Tooltip("The virtual camera to activate when this instruction becomes active")]
			public CinemachineVirtualCameraBase m_VirtualCamera;

			// Token: 0x040002AD RID: 685
			[Tooltip("How long to wait (in seconds) before activating the next virtual camera in the list (if any)")]
			public float m_Hold;

			// Token: 0x040002AE RID: 686
			[CinemachineBlendDefinitionProperty]
			[Tooltip("How to blend to the next virtual camera in the list (if any)")]
			public CinemachineBlendDefinition m_Blend;
		}
	}
}
