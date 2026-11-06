using System;
using System.Collections.Generic;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200001B RID: 27
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[ExcludeFromPreset]
	[AddComponentMenu("Cinemachine/CinemachineStateDrivenCamera")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineStateDrivenCamera.html")]
	public class CinemachineStateDrivenCamera : CinemachineVirtualCameraBase
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00008FE0 File Offset: 0x000071E0
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00009048 File Offset: 0x00007248
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00009050 File Offset: 0x00007250
		public ICinemachineCamera LiveChild { get; set; }

		// Token: 0x06000119 RID: 281 RVA: 0x00009059 File Offset: 0x00007259
		public override bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			return vcam == this.LiveChild || (this.mActiveBlend != null && this.mActiveBlend.Uses(vcam));
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000907C File Offset: 0x0000727C
		public override CameraState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00009084 File Offset: 0x00007284
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00009092 File Offset: 0x00007292
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000909B File Offset: 0x0000729B
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000090A9 File Offset: 0x000072A9
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

		// Token: 0x0600011F RID: 287 RVA: 0x000090B4 File Offset: 0x000072B4
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

		// Token: 0x06000120 RID: 288 RVA: 0x000090F0 File Offset: 0x000072F0
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

		// Token: 0x06000121 RID: 289 RVA: 0x0000912A File Offset: 0x0000732A
		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
			base.InvokeOnTransitionInExtensions(fromCam, worldUp, deltaTime);
			this.m_TransitioningFrom = fromCam;
			this.InternalUpdateCameraState(worldUp, deltaTime);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00009150 File Offset: 0x00007350
		public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			this.UpdateListOfChildren();
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = this.ChooseCurrentCamera();
			if (cinemachineVirtualCameraBase != null && !cinemachineVirtualCameraBase.gameObject.activeInHierarchy)
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

		// Token: 0x06000123 RID: 291 RVA: 0x000092E0 File Offset: 0x000074E0
		protected override void OnEnable()
		{
			base.OnEnable();
			this.InvalidateListOfChildren();
			this.mActiveBlend = null;
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Combine(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00009340 File Offset: 0x00007540
		protected override void OnDisable()
		{
			base.OnDisable();
			CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(this.OnGuiHandler));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00009368 File Offset: 0x00007568
		public void OnTransformChildrenChanged()
		{
			this.InvalidateListOfChildren();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009370 File Offset: 0x00007570
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000093E4 File Offset: 0x000075E4
		public CinemachineVirtualCameraBase[] ChildCameras
		{
			get
			{
				this.UpdateListOfChildren();
				return this.m_ChildCameras;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000093F2 File Offset: 0x000075F2
		public bool IsBlending
		{
			get
			{
				return this.mActiveBlend != null;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000093FD File Offset: 0x000075FD
		public CinemachineBlend ActiveBlend
		{
			get
			{
				return this.mActiveBlend;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009405 File Offset: 0x00007605
		public static int CreateFakeHash(int parentHash, AnimationClip clip)
		{
			return Animator.StringToHash(parentHash.ToString() + "_" + clip.name);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00009424 File Offset: 0x00007624
		private int LookupFakeHash(int parentHash, AnimationClip clip)
		{
			if (this.mHashCache == null)
			{
				this.mHashCache = new Dictionary<AnimationClip, List<CinemachineStateDrivenCamera.HashPair>>();
			}
			List<CinemachineStateDrivenCamera.HashPair> list = null;
			if (!this.mHashCache.TryGetValue(clip, out list))
			{
				list = new List<CinemachineStateDrivenCamera.HashPair>();
				this.mHashCache[clip] = list;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].parentHash == parentHash)
				{
					return list[i].hash;
				}
			}
			int num = CinemachineStateDrivenCamera.CreateFakeHash(parentHash, clip);
			list.Add(new CinemachineStateDrivenCamera.HashPair
			{
				parentHash = parentHash,
				hash = num
			});
			this.mStateParentLookup[num] = parentHash;
			return num;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000094CB File Offset: 0x000076CB
		private void InvalidateListOfChildren()
		{
			this.m_ChildCameras = null;
			this.LiveChild = null;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000094DC File Offset: 0x000076DC
		private void UpdateListOfChildren()
		{
			if (this.m_ChildCameras != null && this.mInstructionDictionary != null && this.mStateParentLookup != null)
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

		// Token: 0x0600012E RID: 302 RVA: 0x00009554 File Offset: 0x00007754
		internal void ValidateInstructions()
		{
			if (this.m_Instructions == null)
			{
				this.m_Instructions = Array.Empty<CinemachineStateDrivenCamera.Instruction>();
			}
			this.mInstructionDictionary = new Dictionary<int, int>();
			for (int i = 0; i < this.m_Instructions.Length; i++)
			{
				if (this.m_Instructions[i].m_VirtualCamera != null && this.m_Instructions[i].m_VirtualCamera.transform.parent != base.transform)
				{
					this.m_Instructions[i].m_VirtualCamera = null;
				}
				this.mInstructionDictionary[this.m_Instructions[i].m_FullHash] = i;
			}
			this.mStateParentLookup = new Dictionary<int, int>();
			if (this.m_ParentHash != null)
			{
				foreach (CinemachineStateDrivenCamera.ParentHash parentHash2 in this.m_ParentHash)
				{
					this.mStateParentLookup[parentHash2.m_Hash] = parentHash2.m_ParentHash;
				}
			}
			this.mHashCache = null;
			this.mActivationTime = (this.mPendingActivationTime = 0f);
			this.mActiveBlend = null;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009670 File Offset: 0x00007870
		private CinemachineVirtualCameraBase ChooseCurrentCamera()
		{
			if (this.m_ChildCameras == null || this.m_ChildCameras.Length == 0)
			{
				this.mActivationTime = 0f;
				return null;
			}
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = this.m_ChildCameras[0];
			if (this.m_AnimatedTarget == null || !this.m_AnimatedTarget.gameObject.activeSelf || this.m_AnimatedTarget.runtimeAnimatorController == null || this.m_LayerIndex < 0 || !this.m_AnimatedTarget.hasBoundPlayables || this.m_LayerIndex >= this.m_AnimatedTarget.layerCount)
			{
				this.mActivationTime = 0f;
				return cinemachineVirtualCameraBase;
			}
			int num;
			if (this.m_AnimatedTarget.IsInTransition(this.m_LayerIndex))
			{
				AnimatorStateInfo nextAnimatorStateInfo = this.m_AnimatedTarget.GetNextAnimatorStateInfo(this.m_LayerIndex);
				this.m_AnimatedTarget.GetNextAnimatorClipInfo(this.m_LayerIndex, this.m_clipInfoList);
				num = this.GetClipHash(nextAnimatorStateInfo.fullPathHash, this.m_clipInfoList);
			}
			else
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.m_AnimatedTarget.GetCurrentAnimatorStateInfo(this.m_LayerIndex);
				this.m_AnimatedTarget.GetCurrentAnimatorClipInfo(this.m_LayerIndex, this.m_clipInfoList);
				num = this.GetClipHash(currentAnimatorStateInfo.fullPathHash, this.m_clipInfoList);
			}
			while (num != 0 && !this.mInstructionDictionary.ContainsKey(num))
			{
				num = (this.mStateParentLookup.ContainsKey(num) ? this.mStateParentLookup[num] : 0);
			}
			float currentTime = CinemachineCore.CurrentTime;
			if (this.mActivationTime != 0f)
			{
				if (this.mActiveInstruction.m_FullHash == num)
				{
					this.mPendingActivationTime = 0f;
					return this.mActiveInstruction.m_VirtualCamera;
				}
				if (this.PreviousStateIsValid && this.mPendingActivationTime != 0f && this.mPendingInstruction.m_FullHash == num)
				{
					if (currentTime - this.mPendingActivationTime > this.mPendingInstruction.m_ActivateAfter && (currentTime - this.mActivationTime > this.mActiveInstruction.m_MinDuration || this.mPendingInstruction.m_VirtualCamera.Priority > this.mActiveInstruction.m_VirtualCamera.Priority))
					{
						this.mActiveInstruction = this.mPendingInstruction;
						this.mActivationTime = currentTime;
						this.mPendingActivationTime = 0f;
					}
					return this.mActiveInstruction.m_VirtualCamera;
				}
			}
			this.mPendingActivationTime = 0f;
			if (!this.mInstructionDictionary.ContainsKey(num))
			{
				if (this.mActivationTime != 0f)
				{
					return this.mActiveInstruction.m_VirtualCamera;
				}
				return cinemachineVirtualCameraBase;
			}
			else
			{
				CinemachineStateDrivenCamera.Instruction instruction = this.m_Instructions[this.mInstructionDictionary[num]];
				if (instruction.m_VirtualCamera == null)
				{
					instruction.m_VirtualCamera = cinemachineVirtualCameraBase;
				}
				if (!this.PreviousStateIsValid || this.mActivationTime <= 0f || (instruction.m_ActivateAfter <= 0f && (currentTime - this.mActivationTime >= this.mActiveInstruction.m_MinDuration || instruction.m_VirtualCamera.Priority > this.mActiveInstruction.m_VirtualCamera.Priority)))
				{
					this.mActiveInstruction = instruction;
					this.mActivationTime = currentTime;
					return this.mActiveInstruction.m_VirtualCamera;
				}
				this.mPendingInstruction = instruction;
				this.mPendingActivationTime = currentTime;
				if (this.mActivationTime != 0f)
				{
					return this.mActiveInstruction.m_VirtualCamera;
				}
				return cinemachineVirtualCameraBase;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000099A4 File Offset: 0x00007BA4
		private int GetClipHash(int hash, List<AnimatorClipInfo> clips)
		{
			int num = -1;
			for (int i = 0; i < clips.Count; i++)
			{
				if (num < 0 || clips[i].weight > clips[num].weight)
				{
					num = i;
				}
			}
			if (num >= 0 && clips[num].weight > 0f)
			{
				hash = this.LookupFakeHash(hash, clips[num].clip);
			}
			return hash;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009A20 File Offset: 0x00007C20
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

		// Token: 0x040000B4 RID: 180
		[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_LookAt;

		// Token: 0x040000B5 RID: 181
		[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_Follow;

		// Token: 0x040000B6 RID: 182
		[Space]
		[Tooltip("The state machine whose state changes will drive this camera's choice of active child")]
		[NoSaveDuringPlay]
		public Animator m_AnimatedTarget;

		// Token: 0x040000B7 RID: 183
		[Tooltip("Which layer in the target state machine to observe")]
		[NoSaveDuringPlay]
		public int m_LayerIndex;

		// Token: 0x040000B8 RID: 184
		[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
		public bool m_ShowDebugText;

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		internal CinemachineVirtualCameraBase[] m_ChildCameras;

		// Token: 0x040000BA RID: 186
		[Tooltip("The set of instructions associating virtual cameras with states.  These instructions are used to choose the live child at any given moment")]
		public CinemachineStateDrivenCamera.Instruction[] m_Instructions;

		// Token: 0x040000BB RID: 187
		[CinemachineBlendDefinitionProperty]
		[Tooltip("The blend which is used if you don't explicitly define a blend between two Virtual Camera children")]
		public CinemachineBlendDefinition m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f);

		// Token: 0x040000BC RID: 188
		[Tooltip("This is the asset which contains custom settings for specific child blends")]
		public CinemachineBlenderSettings m_CustomBlends;

		// Token: 0x040000BD RID: 189
		[HideInInspector]
		[SerializeField]
		internal CinemachineStateDrivenCamera.ParentHash[] m_ParentHash;

		// Token: 0x040000BF RID: 191
		private ICinemachineCamera m_TransitioningFrom;

		// Token: 0x040000C0 RID: 192
		private CameraState m_State = CameraState.Default;

		// Token: 0x040000C1 RID: 193
		private Dictionary<AnimationClip, List<CinemachineStateDrivenCamera.HashPair>> mHashCache;

		// Token: 0x040000C2 RID: 194
		private float mActivationTime;

		// Token: 0x040000C3 RID: 195
		private CinemachineStateDrivenCamera.Instruction mActiveInstruction;

		// Token: 0x040000C4 RID: 196
		private float mPendingActivationTime;

		// Token: 0x040000C5 RID: 197
		private CinemachineStateDrivenCamera.Instruction mPendingInstruction;

		// Token: 0x040000C6 RID: 198
		private CinemachineBlend mActiveBlend;

		// Token: 0x040000C7 RID: 199
		private Dictionary<int, int> mInstructionDictionary;

		// Token: 0x040000C8 RID: 200
		private Dictionary<int, int> mStateParentLookup;

		// Token: 0x040000C9 RID: 201
		private List<AnimatorClipInfo> m_clipInfoList = new List<AnimatorClipInfo>();

		// Token: 0x02000086 RID: 134
		[Serializable]
		public struct Instruction
		{
			// Token: 0x040002F4 RID: 756
			[Tooltip("The full hash of the animation state")]
			public int m_FullHash;

			// Token: 0x040002F5 RID: 757
			[Tooltip("The virtual camera to activate when the animation state becomes active")]
			public CinemachineVirtualCameraBase m_VirtualCamera;

			// Token: 0x040002F6 RID: 758
			[Tooltip("How long to wait (in seconds) before activating the virtual camera. This filters out very short state durations")]
			public float m_ActivateAfter;

			// Token: 0x040002F7 RID: 759
			[Tooltip("The minimum length of time (in seconds) to keep a virtual camera active")]
			public float m_MinDuration;
		}

		// Token: 0x02000087 RID: 135
		[DocumentationSorting(DocumentationSortingAttribute.Level.Undoc)]
		[Serializable]
		internal struct ParentHash
		{
			// Token: 0x0600042F RID: 1071 RVA: 0x00018D65 File Offset: 0x00016F65
			public ParentHash(int h, int p)
			{
				this.m_Hash = h;
				this.m_ParentHash = p;
			}

			// Token: 0x040002F8 RID: 760
			public int m_Hash;

			// Token: 0x040002F9 RID: 761
			public int m_ParentHash;
		}

		// Token: 0x02000088 RID: 136
		private struct HashPair
		{
			// Token: 0x040002FA RID: 762
			public int parentHash;

			// Token: 0x040002FB RID: 763
			public int hash;
		}
	}
}
