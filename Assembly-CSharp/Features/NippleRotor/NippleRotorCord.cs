using System;
using System.Collections.Generic;
using Features.NipplePiercings;
using Features.RenderingPrecision;
using RayznGames.DressRigger;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

namespace Features.NippleRotor
{
	// Token: 0x02000F58 RID: 3928
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public sealed class NippleRotorCord : MonoBehaviour
	{
		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x060075F1 RID: 30193 RVA: 0x002FCAE4 File Offset: 0x002FACE4
		public bool IsHidden
		{
			get
			{
				return this._hidden;
			}
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x060075F2 RID: 30194 RVA: 0x002FCAEC File Offset: 0x002FACEC
		public bool IsConfigured
		{
			get
			{
				return this._configured;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x060075F3 RID: 30195 RVA: 0x002FCAF4 File Offset: 0x002FACF4
		public int NodeCount
		{
			get
			{
				if (this._positions == null)
				{
					return 0;
				}
				return this._nodeCount;
			}
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x060075F4 RID: 30196 RVA: 0x002FCB06 File Offset: 0x002FAD06
		public float BodyScale
		{
			get
			{
				return this.ComputeBodyScale();
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x060075F5 RID: 30197 RVA: 0x002FCB0E File Offset: 0x002FAD0E
		public int RightSocketNode
		{
			get
			{
				return this._rightStart;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x060075F6 RID: 30198 RVA: 0x002FCB16 File Offset: 0x002FAD16
		public int JunctionNode
		{
			get
			{
				return this._junction;
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x060075F7 RID: 30199 RVA: 0x002FCB1E File Offset: 0x002FAD1E
		public int RemoteSocketNode
		{
			get
			{
				return this._remoteIndex;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x060075F8 RID: 30200 RVA: 0x002FCB26 File Offset: 0x002FAD26
		public MeshRenderer CordRenderer
		{
			get
			{
				return this._renderer;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x060075F9 RID: 30201 RVA: 0x002FCB2E File Offset: 0x002FAD2E
		public bool IsSingleStrand
		{
			get
			{
				return this._single;
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x060075FA RID: 30202 RVA: 0x002FCB36 File Offset: 0x002FAD36
		// (set) Token: 0x060075FB RID: 30203 RVA: 0x002FCB3E File Offset: 0x002FAD3E
		public NippleRotorBreastSurface SkinSurface
		{
			get
			{
				return this._skinSurface;
			}
			set
			{
				this._skinSurface = value;
				this._skinValid = false;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x060075FC RID: 30204 RVA: 0x002FCB4E File Offset: 0x002FAD4E
		public bool IsCollidingWithSkin
		{
			get
			{
				return this._skinValid;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x060075FD RID: 30205 RVA: 0x002FCB56 File Offset: 0x002FAD56
		public bool HasFreeEnd
		{
			get
			{
				return this._single && this._freeEnd;
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x060075FE RID: 30206 RVA: 0x002FCB68 File Offset: 0x002FAD68
		public int LastNode
		{
			get
			{
				if (this._positions == null)
				{
					return -1;
				}
				return this._nodeCount - 1;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x060075FF RID: 30207 RVA: 0x002FCB7C File Offset: 0x002FAD7C
		// (set) Token: 0x06007600 RID: 30208 RVA: 0x002FCB84 File Offset: 0x002FAD84
		public Transform FreeEndFollower
		{
			get
			{
				return this._freeEndFollower;
			}
			set
			{
				this._freeEndFollower = value;
				this.PlaceFollower();
			}
		}

		// Token: 0x06007601 RID: 30209 RVA: 0x002FCB93 File Offset: 0x002FAD93
		public Vector3 GetNode(int index)
		{
			if (this._positions == null || index < 0 || index >= this._nodeCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._positions[index];
		}

		// Token: 0x06007602 RID: 30210 RVA: 0x002FCBC4 File Offset: 0x002FADC4
		public void Configure(NippleRotorCordSettings settings, Material material, Transform leftSocket, Transform rightSocket, Transform remoteSocket, Transform playerRoot, NipplePiercingAttachment attachment, Animator animator)
		{
			if (this._single)
			{
				this._topologyDirty = true;
			}
			this._single = (this._freeEnd = false);
			this._pinCount = 3;
			this._startSocket = (this._endSocket = (this._scaleSource = null));
			this._settings = settings ?? new NippleRotorCordSettings();
			this._material = material;
			this._leftSocket = leftSocket;
			this._rightSocket = rightSocket;
			this._remoteSocket = remoteSocket;
			this._playerRoot = playerRoot;
			this._attachment = attachment;
			this._animator = animator;
			this.EnsureRendering();
			this.ResolveColliders();
			this._configured = leftSocket != null && rightSocket != null && remoteSocket != null;
			if (!this._configured)
			{
				Debug.LogWarning("[NippleRotor] The cord needs both egg sockets and the remote socket.", this);
			}
			else if (material == null && Application.isPlaying)
			{
				Debug.LogWarning("[NippleRotor] The cord has no material.", this);
			}
			this._initialized = false;
			this.ApplyRendererState();
			this.ResetSimulation();
		}

		// Token: 0x06007603 RID: 30211 RVA: 0x002FCCCC File Offset: 0x002FAECC
		public void ConfigureSingle(NippleRotorCordSettings settings, Material material, Transform startSocket, Transform endSocket, Transform playerRoot, Animator animator)
		{
			bool flag = endSocket == null;
			bool flag2 = flag && this._single && !this._freeEnd && this._configured && this._initialized && !this._hidden && !this._pinsLost && !this._needsReset && startSocket != null && startSocket == this._startSocket && this._positions != null;
			int num = (flag2 ? this._nodeCount : (-1));
			if (!this._single || flag != this._freeEnd)
			{
				this._topologyDirty = true;
			}
			this._single = true;
			this._freeEnd = flag;
			this._pinCount = (flag ? 1 : 2);
			this._settings = settings ?? new NippleRotorCordSettings();
			this._material = material;
			this._startSocket = startSocket;
			this._endSocket = endSocket;
			this._leftSocket = (this._rightSocket = (this._remoteSocket = null));
			this._playerRoot = playerRoot;
			this._attachment = null;
			this._animator = animator;
			this._scaleSource = NippleRotorCord.FindBodyScaleSource(playerRoot);
			this.EnsureRendering();
			this.ResolveColliders();
			this._configured = startSocket != null;
			if (!this._configured)
			{
				Debug.LogWarning("[NippleRotor] The single cord needs a start socket.", this);
			}
			else if (material == null && Application.isPlaying)
			{
				Debug.LogWarning("[NippleRotor] The cord has no material.", this);
			}
			if (flag2 && this.ReleaseInPlace(num))
			{
				this.ApplyRendererState();
				return;
			}
			this._initialized = false;
			this.ApplyRendererState();
			this.ResetSimulation();
		}

		// Token: 0x06007604 RID: 30212 RVA: 0x002FCE51 File Offset: 0x002FB051
		public void ResetSimulation()
		{
			this._needsReset = true;
			if (!this._configured || this._hidden || !this.EnsureTopology())
			{
				return;
			}
			if (!this.ReadPins())
			{
				return;
			}
			this.ResetToTargets();
		}

		// Token: 0x06007605 RID: 30213 RVA: 0x002FCE82 File Offset: 0x002FB082
		public void SetHidden(bool hidden)
		{
			if (this._hidden != hidden)
			{
				this._hidden = hidden;
				if (hidden)
				{
					this._initialized = false;
				}
				else
				{
					this.ResetSimulation();
				}
			}
			this.ApplyRendererState();
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x002FCEAC File Offset: 0x002FB0AC
		public void Step(float deltaTime)
		{
			if (!this._configured || this._hidden || !this.EnsureTopology())
			{
				return;
			}
			if (!this.ReadPins())
			{
				return;
			}
			if (this._needsReset || !this._initialized || float.IsNaN(deltaTime) || float.IsInfinity(deltaTime) || deltaTime < 0f || deltaTime > 0.25f || this._hasRoot != this._lastHasRoot || Mathf.Abs(this._scale - this._lastScale) > this._lastScale * 0.02f)
			{
				this.ResetToTargets();
				return;
			}
			bool flag = deltaTime < 1E-05f;
			float teleportDistance = this.TeleportDistance;
			bool flag2 = false;
			if (this._hasRoot)
			{
				Vector3 vector = this._rootPosition - this._lastRootPosition;
				if ((vector != Vector3.zero || this._rootRotation != this._lastRootRotation) && (flag || vector.sqrMagnitude > teleportDistance * teleportDistance || Quaternion.Angle(this._lastRootRotation, this._rootRotation) > 60f))
				{
					this.Transport();
					flag2 = true;
				}
			}
			if (this.PinsJumped(flag ? (0.002f * this._scale) : teleportDistance))
			{
				this.ResetToTargets();
				return;
			}
			if (!flag)
			{
				this.Integrate(deltaTime);
				for (int i = 0; i < this._nodeCount; i++)
				{
					if (!NippleRotorCord.IsFinite(this._positions[i]))
					{
						this.ResetToTargets();
						return;
					}
				}
				this.Remember();
				this.RebuildMesh();
				return;
			}
			bool flag3 = this._skinValid && this.ProjectOntoSkin();
			if (flag2 || flag3)
			{
				this.RebuildMesh();
				return;
			}
			this.PlaceFollower();
		}

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06007607 RID: 30215 RVA: 0x002FD04C File Offset: 0x002FB24C
		private float TeleportDistance
		{
			get
			{
				return Mathf.Max(0.01f, this._settings.teleportDistance) * this._scale;
			}
		}

		// Token: 0x06007608 RID: 30216 RVA: 0x002FD06A File Offset: 0x002FB26A
		private void OnEnable()
		{
			NippleRotorCord.Register(this);
			if (!this._subscribed)
			{
				WorldOrigin.Shifted += this.HandleWorldOriginShifted;
				this._subscribed = true;
			}
			this.ResetSimulation();
			this.ApplyRendererState();
		}

		// Token: 0x06007609 RID: 30217 RVA: 0x002FD09E File Offset: 0x002FB29E
		private void OnDisable()
		{
			NippleRotorCord.Unregister(this);
			this.Unsubscribe();
			this._initialized = false;
			if (this._renderer != null)
			{
				this._renderer.enabled = false;
			}
		}

		// Token: 0x0600760A RID: 30218 RVA: 0x002FD0D0 File Offset: 0x002FB2D0
		private void OnDestroy()
		{
			NippleRotorCord.Unregister(this);
			this.Unsubscribe();
			if (this._mesh == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._mesh);
			}
			else
			{
				Object.DestroyImmediate(this._mesh);
			}
			this._mesh = null;
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x002FD11E File Offset: 0x002FB31E
		private void Unsubscribe()
		{
			if (!this._subscribed)
			{
				return;
			}
			WorldOrigin.Shifted -= this.HandleWorldOriginShifted;
			this._subscribed = false;
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x002FD144 File Offset: 0x002FB344
		private void HandleWorldOriginShifted(Vector3 delta)
		{
			if (this._positions == null || !this._initialized)
			{
				return;
			}
			for (int i = 0; i < this._nodeCount; i++)
			{
				this._positions[i] += delta;
				this._previous[i] += delta;
				this._targets[i] += delta;
				this._lastTargets[i] += delta;
				this._frameTargets[i] += delta;
			}
			for (int j = 0; j < 3; j++)
			{
				this._lastPin[j] += delta;
			}
			this._lastRootPosition += delta;
			for (int k = 0; k < this._capsuleCount; k++)
			{
				this._capsuleLastA[k] += delta;
				this._capsuleLastB[k] += delta;
				this._capsuleNowA[k] += delta;
				this._capsuleNowB[k] += delta;
			}
			for (int l = 0; l < 2; l++)
			{
				this._sphereLast[l] += delta;
				this._sphereNow[l] += delta;
			}
		}

		// Token: 0x0600760D RID: 30221 RVA: 0x002FD314 File Offset: 0x002FB514
		private void StepFromPlayerLoop(float deltaTime)
		{
			if (!this._configured || this._hidden || this._steppedFrame == Time.frameCount)
			{
				return;
			}
			this._steppedFrame = Time.frameCount;
			if (this._attachment != null && this._attachment.isActiveAndEnabled)
			{
				this._attachment.UpdatePose();
			}
			this.Step(deltaTime);
		}

		// Token: 0x0600760E RID: 30222 RVA: 0x002FD378 File Offset: 0x002FB578
		private void ApplyRendererState()
		{
			if (this._renderer == null)
			{
				return;
			}
			this._renderer.enabled = this._configured && !this._hidden && !this._pinsLost && base.enabled && base.gameObject.activeInHierarchy;
		}

		// Token: 0x0600760F RID: 30223 RVA: 0x002FD3D0 File Offset: 0x002FB5D0
		private bool ReadPins()
		{
			bool flag = this.ReadFrame();
			if (flag == !this._pinsLost)
			{
				return flag;
			}
			this._pinsLost = !flag;
			if (this._pinsLost)
			{
				this._needsReset = true;
			}
			this.ApplyRendererState();
			return flag;
		}

		// Token: 0x06007610 RID: 30224 RVA: 0x002FD414 File Offset: 0x002FB614
		private void EnsureRendering()
		{
			if (this._filter == null && !base.TryGetComponent<MeshFilter>(out this._filter))
			{
				this._filter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (this._renderer == null && !base.TryGetComponent<MeshRenderer>(out this._renderer))
			{
				this._renderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			if (this._mesh == null)
			{
				this._mesh = new Mesh
				{
					name = "NippleRotorCord",
					hideFlags = HideFlags.DontSave
				};
				this._mesh.MarkDynamic();
				this._topologyDirty = true;
			}
			this._filter.sharedMesh = this._mesh;
			this._renderer.sharedMaterial = this._material;
			this._renderer.shadowCastingMode = ShadowCastingMode.On;
			this._renderer.receiveShadows = true;
		}

		// Token: 0x06007611 RID: 30225 RVA: 0x002FD4F4 File Offset: 0x002FB6F4
		private static Transform FindBodyScaleSource(Transform playerRoot)
		{
			if (playerRoot == null)
			{
				return null;
			}
			Transform transform = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in playerRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.name != "Body_sotai_NSFW"))
				{
					if (skinnedMeshRenderer.gameObject.activeSelf)
					{
						return skinnedMeshRenderer.transform;
					}
					if (transform == null)
					{
						transform = skinnedMeshRenderer.transform;
					}
				}
			}
			if (transform != null)
			{
				return transform;
			}
			PlayerDresser componentInChildren = playerRoot.GetComponentInChildren<PlayerDresser>(true);
			SkinnedMeshRenderer skinnedMeshRenderer2 = ((componentInChildren != null) ? componentInChildren.GetCharacterBody() : null);
			if (!(skinnedMeshRenderer2 != null))
			{
				return null;
			}
			return skinnedMeshRenderer2.transform;
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x002FD5A0 File Offset: 0x002FB7A0
		private void ResolveColliders()
		{
			this._capsuleCount = 0;
			for (int i = 0; i < 5; i++)
			{
				this._capsuleFrom[i] = (this._capsuleTo[i] = null);
				this._capsuleValid[i] = false;
			}
			this._sphereValid[0] = (this._sphereValid[1] = false);
			if (this._animator == null || !this._animator.isHuman)
			{
				return;
			}
			if (this._single)
			{
				Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
				Transform boneTransform2 = this._animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
				this.AddCapsule(this._animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg), boneTransform, true, false);
				this.AddCapsule(this._animator.GetBoneTransform(HumanBodyBones.RightUpperLeg), boneTransform2, true, false);
				this.AddCapsule(boneTransform, this._animator.GetBoneTransform(HumanBodyBones.LeftFoot), false, true);
				this.AddCapsule(boneTransform2, this._animator.GetBoneTransform(HumanBodyBones.RightFoot), false, true);
				return;
			}
			Transform transform = null;
			this.ChainTorso(ref transform, this._animator.GetBoneTransform(HumanBodyBones.Hips));
			this.ChainTorso(ref transform, this._animator.GetBoneTransform(HumanBodyBones.Spine));
			this.ChainTorso(ref transform, this._animator.GetBoneTransform(HumanBodyBones.Chest));
			this.ChainTorso(ref transform, this._animator.GetBoneTransform(HumanBodyBones.UpperChest));
			if (this._capsuleCount > 0)
			{
				this._capsuleOpenEnd[this._capsuleCount - 1] = true;
			}
			this.AddCapsule(this._animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg), this._animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg), true, false);
			this.AddCapsule(this._animator.GetBoneTransform(HumanBodyBones.RightUpperLeg), this._animator.GetBoneTransform(HumanBodyBones.RightLowerLeg), true, false);
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x002FD736 File Offset: 0x002FB936
		private void ChainTorso(ref Transform previous, Transform bone)
		{
			if (bone == null)
			{
				return;
			}
			if (previous != null)
			{
				this.AddCapsule(previous, bone, false, false);
			}
			previous = bone;
		}

		// Token: 0x06007614 RID: 30228 RVA: 0x002FD75C File Offset: 0x002FB95C
		private void AddCapsule(Transform from, Transform to, bool thigh, bool shin = false)
		{
			if (from == null || to == null || from == to || this._capsuleCount >= 5)
			{
				return;
			}
			this._capsuleFrom[this._capsuleCount] = from;
			this._capsuleTo[this._capsuleCount] = to;
			this._capsuleThigh[this._capsuleCount] = thigh;
			this._capsuleShin[this._capsuleCount] = shin;
			this._capsuleOpenEnd[this._capsuleCount] = false;
			this._capsuleValid[this._capsuleCount] = false;
			this._capsuleCount++;
		}

		// Token: 0x06007615 RID: 30229 RVA: 0x002FD7F4 File Offset: 0x002FB9F4
		private bool EnsureTopology()
		{
			if (this._settings == null)
			{
				return false;
			}
			if (this._single)
			{
				int num = Mathf.Clamp(this._settings.mainNodes, 4, 96);
				int num2 = Mathf.Clamp(this._settings.radialSegments, 3, 16);
				int num3 = Mathf.Clamp(this._settings.renderSubdivisions, 1, 6);
				if (this._topologyDirty || this._positions == null || num != this._main || num2 != this._radial || num3 != this._subdivisions)
				{
					this.BuildSingleTopology(num, num2, num3);
				}
				else if (this._settings.mainLength != this._uvMainLength || this._settings.textureRepeatsPerUnit != this._uvRepeat)
				{
					this.WriteUVs();
				}
				return true;
			}
			int num4 = Mathf.Clamp(this._settings.branchNodes, 2, 32);
			int num5 = Mathf.Clamp(this._settings.mainNodes, 3, 96);
			int num6 = Mathf.Clamp(this._settings.radialSegments, 3, 16);
			int num7 = Mathf.Clamp(this._settings.renderSubdivisions, 1, 6);
			if (this._topologyDirty || this._positions == null || num4 != this._branch || num5 != this._main || num6 != this._radial || num7 != this._subdivisions)
			{
				this.BuildTopology(num4, num5, num6, num7);
			}
			else if (this._settings.branchLength != this._uvBranchLength || this._settings.mainLength != this._uvMainLength || this._settings.textureRepeatsPerUnit != this._uvRepeat)
			{
				this.WriteUVs();
			}
			return true;
		}

		// Token: 0x06007616 RID: 30230 RVA: 0x002FD990 File Offset: 0x002FBB90
		private void BuildTopology(int branch, int main, int radial, int subdivisions)
		{
			this._branch = branch;
			this._main = main;
			this._radial = radial;
			this._subdivisions = subdivisions;
			this._rightStart = branch;
			this._junction = 2 * branch;
			this._remoteIndex = this._junction + main;
			this._nodeCount = this._remoteIndex + 1;
			this._last = this._remoteIndex;
			this._strandCount = 3;
			this._heavyNode = -1;
			this._collisionScale = null;
			this._nodesKept = false;
			this._positions = new Vector3[this._nodeCount];
			this._previous = new Vector3[this._nodeCount];
			this._targets = new Vector3[this._nodeCount];
			this._lastTargets = new Vector3[this._nodeCount];
			this._frameTargets = new Vector3[this._nodeCount];
			this._pinned = new bool[this._nodeCount];
			this._pinned[0] = (this._pinned[1] = (this._pinned[this._rightStart] = (this._pinned[this._rightStart + 1] = true)));
			this._pinned[this._remoteIndex - 1] = (this._pinned[this._remoteIndex] = true);
			this._contactPoint = new Vector3[this._nodeCount];
			this._contactNormal = new Vector3[this._nodeCount];
			this._hasContact = new bool[this._nodeCount];
			this._contactHint = new int[this._nodeCount];
			for (int i = 0; i < this._nodeCount; i++)
			{
				this._contactHint[i] = -1;
			}
			this._linkCount = 2 * (branch - 1) + (main - 1);
			this._linkA = new int[this._linkCount];
			this._linkB = new int[this._linkCount];
			this._linkMain = new bool[this._linkCount];
			int num = 0;
			for (int j = 0; j < 2; j++)
			{
				int num2 = ((j == 0) ? 0 : this._rightStart);
				for (int k = 1; k < branch; k++)
				{
					this._linkA[num] = num2 + k;
					this._linkB[num] = ((k + 1 < branch) ? (num2 + k + 1) : this._junction);
					num++;
				}
			}
			for (int l = 0; l < main - 1; l++)
			{
				this._linkA[num] = this._junction + l;
				this._linkB[num] = this._junction + l + 1;
				this._linkMain[num] = true;
				num++;
			}
			this._strands = new int[3][];
			for (int m = 0; m < 2; m++)
			{
				int num3 = ((m == 0) ? 0 : this._rightStart);
				int[] array = new int[branch + 1];
				for (int n = 0; n < branch; n++)
				{
					array[n] = num3 + n;
				}
				array[branch] = this._junction;
				this._strands[m] = array;
			}
			int[] array2 = new int[main + 1];
			for (int num4 = 0; num4 <= main; num4++)
			{
				array2[num4] = this._junction + num4;
			}
			this._strands[2] = array2;
			this.BuildRings(radial, subdivisions);
		}

		// Token: 0x06007617 RID: 30231 RVA: 0x002FDCAC File Offset: 0x002FBEAC
		private void BuildSingleTopology(int segments, int radial, int subdivisions)
		{
			this._branch = 0;
			this._main = segments;
			this._radial = radial;
			this._subdivisions = subdivisions;
			this._rightStart = (this._junction = (this._remoteIndex = -1));
			this._last = segments;
			this._nodeCount = segments + 1;
			this._strandCount = 1;
			this._nodesKept = this._positions != null && this._positions.Length == this._nodeCount;
			if (!this._nodesKept)
			{
				this._positions = new Vector3[this._nodeCount];
				this._previous = new Vector3[this._nodeCount];
				this._targets = new Vector3[this._nodeCount];
				this._lastTargets = new Vector3[this._nodeCount];
				this._frameTargets = new Vector3[this._nodeCount];
			}
			this._pinned = new bool[this._nodeCount];
			this._pinned[0] = (this._pinned[1] = true);
			if (!this._freeEnd)
			{
				this._pinned[this._last - 1] = (this._pinned[this._last] = true);
			}
			this._heavyNode = (this._freeEnd ? this._last : (-1));
			this._linkCount = (this._freeEnd ? (segments - 1) : (segments - 2));
			this._linkA = new int[this._linkCount];
			this._linkB = new int[this._linkCount];
			this._linkMain = new bool[this._linkCount];
			for (int i = 0; i < this._linkCount; i++)
			{
				this._linkA[i] = i + 1;
				this._linkB[i] = i + 2;
				this._linkMain[i] = true;
			}
			this._collisionScale = new float[this._nodeCount];
			for (int j = 0; j < this._nodeCount; j++)
			{
				if (!this._pinned[j])
				{
					int num = (this._freeEnd ? (j - 1) : Mathf.Min(j - 1, this._last - 1 - j));
					this._collisionScale[j] = Mathf.Clamp01((float)num / 3f);
				}
			}
			int[] array = new int[this._nodeCount];
			for (int k = 0; k < this._nodeCount; k++)
			{
				array[k] = k;
			}
			this._strands = new int[1][];
			this._strands[0] = array;
			this.BuildRings(radial, subdivisions);
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x002FDF10 File Offset: 0x002FC110
		private void BuildRings(int radial, int subdivisions)
		{
			int strandCount = this._strandCount;
			this._ringStart = new int[strandCount + 1];
			for (int i = 0; i < strandCount; i++)
			{
				this._ringStart[i + 1] = this._ringStart[i] + (this._strands[i].Length - 1) * subdivisions + 1;
			}
			int num = this._ringStart[strandCount];
			this._ringPositions = new Vector3[num];
			this._ringTangents = new Vector3[num];
			this._ringHint = new int[num];
			for (int j = 0; j < num; j++)
			{
				this._ringHint[j] = -1;
			}
			int num2 = num * (radial + 1);
			this._vertices = new Vector3[num2];
			this._normals = new Vector3[num2];
			this._tangents = new Vector4[num2];
			this._uvs = new Vector2[num2];
			int num3 = 0;
			for (int k = 0; k < strandCount; k++)
			{
				num3 += (this._ringStart[k + 1] - this._ringStart[k] - 1) * radial;
			}
			this._triangles = new int[num3 * 6];
			int num4 = 0;
			for (int l = 0; l < strandCount; l++)
			{
				for (int m = this._ringStart[l]; m < this._ringStart[l + 1] - 1; m++)
				{
					for (int n = 0; n < radial; n++)
					{
						int num5 = m * (radial + 1) + n;
						int num6 = num5 + radial + 1;
						this._triangles[num4++] = num5;
						this._triangles[num4++] = num6 + 1;
						this._triangles[num4++] = num6;
						this._triangles[num4++] = num5;
						this._triangles[num4++] = num5 + 1;
						this._triangles[num4++] = num6 + 1;
					}
				}
			}
			this._cos = new float[radial + 1];
			this._sin = new float[radial + 1];
			for (int num7 = 0; num7 < radial; num7++)
			{
				float num8 = (float)num7 * (6.2831855f / (float)radial);
				this._cos[num7] = Mathf.Cos(num8);
				this._sin[num7] = Mathf.Sin(num8);
			}
			this._cos[radial] = 1f;
			this._sin[radial] = 0f;
			if (this._mesh != null)
			{
				this._mesh.Clear();
				this._mesh.SetVertices(this._vertices);
				this._mesh.SetNormals(this._normals);
				this._mesh.SetTangents(this._tangents);
				this._mesh.SetTriangles(this._triangles, 0, false);
			}
			this.WriteUVs();
			this._topologyDirty = false;
			this._initialized = false;
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x002FE1D8 File Offset: 0x002FC3D8
		private void WriteUVs()
		{
			this._uvBranchLength = this._settings.branchLength;
			this._uvMainLength = this._settings.mainLength;
			this._uvRepeat = this._settings.textureRepeatsPerUnit;
			float num = ((this._branch > 0) ? (Mathf.Max(0f, this._uvBranchLength) / (float)this._branch / (float)this._subdivisions * this._uvRepeat) : 0f);
			float num2 = Mathf.Max(0f, this._uvMainLength) / (float)this._main / (float)this._subdivisions * this._uvRepeat;
			int num3 = 0;
			for (int i = 0; i < this._strandCount; i++)
			{
				float num4 = ((!this._single && i < 2) ? num : num2);
				int num5 = this._ringStart[i + 1] - this._ringStart[i];
				for (int j = 0; j < num5; j++)
				{
					for (int k = 0; k <= this._radial; k++)
					{
						this._uvs[num3++] = new Vector2((float)k / (float)this._radial, (float)j * num4);
					}
				}
			}
			if (this._mesh != null && this._mesh.vertexCount == this._uvs.Length)
			{
				this._mesh.SetUVs(0, this._uvs);
			}
		}

		// Token: 0x0600761A RID: 30234 RVA: 0x002FE334 File Offset: 0x002FC534
		private bool ReadFrame()
		{
			this._scale = this.ComputeBodyScale();
			if (this._single)
			{
				this._skinValid = false;
				if (this._startSocket == null || !this.ReadPin(0, this._startSocket))
				{
					return false;
				}
				if (!this._freeEnd && (this._endSocket == null || !this.ReadPin(1, this._endSocket)))
				{
					return false;
				}
			}
			else
			{
				if (this._leftSocket == null || this._rightSocket == null || this._remoteSocket == null)
				{
					return false;
				}
				this._skinValid = this._skinSurface != null && this._skinSurface.BeginFrame();
				if (!this.ReadEggPin(0, this._leftSocket, (this._attachment != null) ? this._attachment.LeftAnchor : null) || !this.ReadEggPin(1, this._rightSocket, (this._attachment != null) ? this._attachment.RightAnchor : null) || !this.ReadPin(2, this._remoteSocket))
				{
					return false;
				}
			}
			this._hasRoot = false;
			if (this._playerRoot != null)
			{
				this._rootPosition = this._playerRoot.position;
				this._rootRotation = this._playerRoot.rotation;
				this._front = this._playerRoot.forward;
				this._hasRoot = NippleRotorCord.IsFinite(this._rootPosition) && NippleRotorCord.IsFinite(this._front);
			}
			if (!this._hasRoot)
			{
				this._rootPosition = Vector3.zero;
				this._rootRotation = Quaternion.identity;
				this._front = Vector3.forward;
			}
			this._segmentBranch = (this._single ? 0f : (Mathf.Max(1E-05f, this._settings.branchLength) * this._scale / (float)this._branch));
			this._segmentMain = Mathf.Max(1E-05f, this._settings.mainLength) * this._scale / (float)this._main;
			this.ReadColliders();
			return true;
		}

		// Token: 0x0600761B RID: 30235 RVA: 0x002FE554 File Offset: 0x002FC754
		private bool ReadPin(int index, Transform socket)
		{
			Vector3 position = socket.position;
			Vector3 forward = socket.forward;
			if (!NippleRotorCord.IsFinite(position) || !NippleRotorCord.IsFinite(forward))
			{
				return false;
			}
			this._pin[index] = position;
			this._pinDirection[index] = forward;
			this._pinUp[index] = socket.up;
			return true;
		}

		// Token: 0x0600761C RID: 30236 RVA: 0x002FE5B0 File Offset: 0x002FC7B0
		private bool ReadEggPin(int index, Transform socket, Transform anchor)
		{
			if (!this.ReadPin(index, socket))
			{
				return false;
			}
			if (anchor == null)
			{
				return true;
			}
			Vector3 forward = anchor.forward;
			Vector3 position = anchor.position;
			if (!NippleRotorCord.IsFinite(forward) || !NippleRotorCord.IsFinite(position) || forward.sqrMagnitude < 0.25f)
			{
				return true;
			}
			forward.Normalize();
			float num = Mathf.Max(0f, this._settings.exitStartHeight) * this._scale - Vector3.Dot(this._pin[index] - position, forward);
			if (num > 0f)
			{
				this._pin[index] += forward * num;
			}
			this._pinDirection[index] = NippleRotorCord.ClampExit(this._pinDirection[index], forward, this._settings.exitMinElevation);
			return true;
		}

		// Token: 0x0600761D RID: 30237 RVA: 0x002FE694 File Offset: 0x002FC894
		public static Vector3 ClampExit(Vector3 direction, Vector3 normal, float minElevationDegrees)
		{
			if (minElevationDegrees <= 0f || !NippleRotorCord.IsFinite(direction) || !NippleRotorCord.IsFinite(normal))
			{
				return direction;
			}
			float num = Mathf.Min(minElevationDegrees, 89f) * 0.017453292f;
			float num2 = Mathf.Sin(num);
			float num3 = Vector3.Dot(direction, normal);
			if (num3 >= num2)
			{
				return direction;
			}
			Vector3 vector = direction - normal * num3;
			if (vector.sqrMagnitude < 1E-10f)
			{
				vector = Vector3.ProjectOnPlane(NippleRotorCord.GravityDirection(), normal);
			}
			if (vector.sqrMagnitude < 1E-10f)
			{
				return normal;
			}
			return vector.normalized * Mathf.Cos(num) + normal * num2;
		}

		// Token: 0x0600761E RID: 30238 RVA: 0x002FE738 File Offset: 0x002FC938
		private float ComputeBodyScale()
		{
			float num = 0f;
			if (this._attachment != null)
			{
				SkinnedMeshRenderer body = this._attachment.Body;
				if (body != null)
				{
					num = Mathf.Abs(body.transform.lossyScale.x);
				}
			}
			if (num <= 0.0001f && this._scaleSource != null)
			{
				num = Mathf.Abs(this._scaleSource.lossyScale.x);
			}
			if (num <= 0.0001f && this._playerRoot != null)
			{
				num = Mathf.Abs(this._playerRoot.lossyScale.x);
			}
			if (num <= 0.0001f || !NippleRotorCord.IsFinite(num))
			{
				return 1f;
			}
			return num;
		}

		// Token: 0x0600761F RID: 30239 RVA: 0x002FE7F4 File Offset: 0x002FC9F4
		private void ReadColliders()
		{
			this._anyCollider = false;
			for (int i = 0; i < this._capsuleCount; i++)
			{
				Transform transform = this._capsuleFrom[i];
				Transform transform2 = this._capsuleTo[i];
				bool flag = transform != null && transform2 != null;
				if (flag)
				{
					this._capsuleNowA[i] = transform.position;
					this._capsuleNowB[i] = transform2.position;
					flag = NippleRotorCord.IsFinite(this._capsuleNowA[i]) && NippleRotorCord.IsFinite(this._capsuleNowB[i]);
				}
				if (flag && !this._capsuleValid[i])
				{
					this._capsuleLastA[i] = this._capsuleNowA[i];
					this._capsuleLastB[i] = this._capsuleNowB[i];
				}
				this._capsuleValid[i] = flag;
				this._capsuleRadius[i] = Mathf.Max(0f, this._capsuleShin[i] ? this._settings.shinRadius : (this._capsuleThigh[i] ? this._settings.thighRadius : this._settings.torsoRadius)) * this._scale;
				this._anyCollider |= flag && this._capsuleRadius[i] > 0f;
			}
			this._sphereRadius = Mathf.Max(0f, this._settings.breastRadius) * this._scale;
			if (this._skinValid)
			{
				this._sphereValid[0] = (this._sphereValid[1] = false);
				this._anyCollider = true;
				return;
			}
			this.ReadSphere(0, (this._attachment != null) ? this._attachment.LeftAnchor : null);
			this.ReadSphere(1, (this._attachment != null) ? this._attachment.RightAnchor : null);
		}

		// Token: 0x06007620 RID: 30240 RVA: 0x002FE9DC File Offset: 0x002FCBDC
		private void ReadSphere(int slot, Transform anchor)
		{
			bool flag = anchor != null && this._sphereRadius > 0f;
			if (flag)
			{
				this._sphereNow[slot] = anchor.position - anchor.forward * this._sphereRadius;
				flag = NippleRotorCord.IsFinite(this._sphereNow[slot]);
			}
			if (flag && !this._sphereValid[slot])
			{
				this._sphereLast[slot] = this._sphereNow[slot];
			}
			this._sphereValid[slot] = flag;
			this._anyCollider = this._anyCollider || flag;
		}

		// Token: 0x06007621 RID: 30241 RVA: 0x002FEA7C File Offset: 0x002FCC7C
		private void InterpolateColliders(float fraction)
		{
			for (int i = 0; i < this._capsuleCount; i++)
			{
				if (this._capsuleValid[i])
				{
					this._capsuleA[i] = Vector3.LerpUnclamped(this._capsuleLastA[i], this._capsuleNowA[i], fraction);
					this._capsuleB[i] = Vector3.LerpUnclamped(this._capsuleLastB[i], this._capsuleNowB[i], fraction);
				}
			}
			for (int j = 0; j < 2; j++)
			{
				if (this._sphereValid[j])
				{
					this._sphere[j] = Vector3.LerpUnclamped(this._sphereLast[j], this._sphereNow[j], fraction);
				}
			}
		}

		// Token: 0x06007622 RID: 30242 RVA: 0x002FEB38 File Offset: 0x002FCD38
		private bool PinsJumped(float limit)
		{
			Quaternion quaternion = (this._hasRoot ? (this._rootRotation * Quaternion.Inverse(this._lastRootRotation)) : Quaternion.identity);
			float num = limit * limit;
			for (int i = 0; i < this._pinCount; i++)
			{
				Vector3 vector = (this._hasRoot ? (this._rootPosition + quaternion * (this._lastPin[i] - this._lastRootPosition)) : this._lastPin[i]);
				if ((this._pin[i] - vector).sqrMagnitude > num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007623 RID: 30243 RVA: 0x002FEBE0 File Offset: 0x002FCDE0
		private void Transport()
		{
			Quaternion quaternion = this._rootRotation * Quaternion.Inverse(this._lastRootRotation);
			Vector3 lastRootPosition = this._lastRootPosition;
			Vector3 rootPosition = this._rootPosition;
			for (int i = 0; i < this._nodeCount; i++)
			{
				this._positions[i] = rootPosition + quaternion * (this._positions[i] - lastRootPosition);
				this._previous[i] = rootPosition + quaternion * (this._previous[i] - lastRootPosition);
				this._lastTargets[i] = rootPosition + quaternion * (this._lastTargets[i] - lastRootPosition);
			}
			for (int j = 0; j < 3; j++)
			{
				this._lastPin[j] = rootPosition + quaternion * (this._lastPin[j] - lastRootPosition);
			}
			for (int k = 0; k < this._capsuleCount; k++)
			{
				this._capsuleLastA[k] = rootPosition + quaternion * (this._capsuleLastA[k] - lastRootPosition);
				this._capsuleLastB[k] = rootPosition + quaternion * (this._capsuleLastB[k] - lastRootPosition);
			}
			for (int l = 0; l < 2; l++)
			{
				this._sphereLast[l] = rootPosition + quaternion * (this._sphereLast[l] - lastRootPosition);
			}
			this._lastRootPosition = rootPosition;
			this._lastRootRotation = this._rootRotation;
		}

		// Token: 0x06007624 RID: 30244 RVA: 0x002FEDA0 File Offset: 0x002FCFA0
		private void Integrate(float deltaTime)
		{
			float num = Mathf.Min(deltaTime, 0.06666667f);
			int num2 = Mathf.Clamp(this._settings.substepHz, 30, 480);
			int num3 = Mathf.Clamp(Mathf.CeilToInt(num * (float)num2 - 0.001f), 1, 16);
			float num4 = num / (float)num3;
			float num5 = num4 * num4;
			int num6 = Mathf.Clamp(this._settings.iterations, 1, 64);
			float num7 = Mathf.Clamp01(this._settings.motionInfluence);
			float num8 = Mathf.Exp(-Mathf.Max(0f, this._settings.damping) * num4);
			float num9 = Mathf.Max(0f, this._settings.shapeSpring);
			Vector3 vector = Physics.gravity * (this._settings.gravity * this._scale);
			float num10 = 1f * deltaTime / (float)num3;
			bool single = this._single;
			float num11 = 4f * this._scale * num4;
			float num12 = num11 * num11;
			this.ComputeTargets(this._frameTargets);
			for (int i = 0; i < num3; i++)
			{
				float num13 = (float)i / (float)num3;
				float num14 = ((float)i + 1f) / (float)num3;
				if (this._hasRoot)
				{
					this.Carry(num13, num14, num7, num10);
				}
				for (int j = 0; j < this._nodeCount; j++)
				{
					this._targets[j] = Vector3.LerpUnclamped(this._lastTargets[j], this._frameTargets[j], num14);
				}
				this.InterpolateColliders(num14);
				this.UpdateRestLengths();
				float num15 = ((this._previousStep > 0f) ? Mathf.Min(num4 / this._previousStep, 3f) : 1f) * num8;
				for (int k = 0; k < this._nodeCount; k++)
				{
					if (!this._pinned[k])
					{
						Vector3 vector2 = this._positions[k];
						Vector3 vector3 = vector + (this._targets[k] - vector2) * num9;
						if (single)
						{
							Vector3 vector4 = (vector2 - this._previous[k]) * num15;
							float sqrMagnitude = vector4.sqrMagnitude;
							if (sqrMagnitude > num12)
							{
								vector4 *= num11 / Mathf.Sqrt(sqrMagnitude);
							}
							this._positions[k] = vector2 + vector4 + vector3 * num5;
						}
						else
						{
							this._positions[k] = vector2 + (vector2 - this._previous[k]) * num15 + vector3 * num5;
						}
						this._previous[k] = vector2;
					}
				}
				this._previousStep = num4;
				for (int l = 0; l < this._nodeCount; l++)
				{
					if (this._pinned[l])
					{
						this._positions[l] = (this._previous[l] = this._targets[l]);
					}
				}
				if (this._skinValid)
				{
					this.RefreshSkinContacts();
				}
				this.Relax(num6);
			}
			if (this._skinValid)
			{
				this.ProjectOntoSkin();
			}
		}

		// Token: 0x06007625 RID: 30245 RVA: 0x002FF0E8 File Offset: 0x002FD2E8
		private void Carry(float before, float after, float influence, float residualLimit)
		{
			Vector3 vector = Vector3.LerpUnclamped(this._lastRootPosition, this._rootPosition, before);
			Vector3 vector2 = Vector3.LerpUnclamped(this._lastRootPosition, this._rootPosition, after) - vector;
			Quaternion quaternion = Quaternion.Slerp(this._lastRootRotation, this._rootRotation, after) * Quaternion.Inverse(Quaternion.Slerp(this._lastRootRotation, this._rootRotation, before));
			Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, quaternion, 1f - influence);
			Vector3 vector3 = vector2 - Vector3.ClampMagnitude(vector2 * influence, residualLimit);
			for (int i = 0; i < this._nodeCount; i++)
			{
				if (!this._pinned[i])
				{
					this._positions[i] = vector + quaternion2 * (this._positions[i] - vector) + vector3;
					this._previous[i] = vector + quaternion2 * (this._previous[i] - vector) + vector3;
				}
			}
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x002FF1FC File Offset: 0x002FD3FC
		private void Relax(int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				if ((i & 1) == 0)
				{
					for (int j = 0; j < this._linkCount; j++)
					{
						this.SolveLink(j);
					}
				}
				else
				{
					for (int k = this._linkCount - 1; k >= 0; k--)
					{
						this.SolveLink(k);
					}
				}
				if (this._anyCollider)
				{
					this.Collide();
				}
			}
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x002FF25C File Offset: 0x002FD45C
		private void SolveLink(int link)
		{
			int num = this._linkA[link];
			int num2 = this._linkB[link];
			Vector3 vector = this._positions[num2] - this._positions[num];
			float magnitude = vector.magnitude;
			if (magnitude < 1E-07f)
			{
				return;
			}
			float num3 = (this._linkMain[link] ? this._restMain : this._restBranch);
			Vector3 vector2 = vector * ((magnitude - num3) / magnitude);
			if (this._pinned[num])
			{
				this._positions[num2] -= vector2;
				return;
			}
			if (this._pinned[num2])
			{
				this._positions[num] += vector2;
				return;
			}
			if (num2 == this._heavyNode)
			{
				this._positions[num] += vector2 * (1f - this._endShare);
				this._positions[num2] -= vector2 * this._endShare;
				return;
			}
			vector2 *= 0.5f;
			this._positions[num] += vector2;
			this._positions[num2] -= vector2;
		}

		// Token: 0x06007628 RID: 30248 RVA: 0x002FF3D0 File Offset: 0x002FD5D0
		private void Collide()
		{
			if (this._single)
			{
				this.CollideSingle();
				return;
			}
			for (int i = 0; i < this._capsuleCount; i++)
			{
				float num = this._capsuleRadius[i];
				if (this._capsuleValid[i] && num > 0f)
				{
					Vector3 vector = this._capsuleA[i];
					Vector3 vector2 = this._capsuleB[i] - vector;
					float sqrMagnitude = vector2.sqrMagnitude;
					bool flag = this._capsuleOpenEnd[i];
					for (int j = 0; j < this._nodeCount; j++)
					{
						if (!this._pinned[j])
						{
							float num2 = ((sqrMagnitude > 1E-10f) ? (Vector3.Dot(this._positions[j] - vector, vector2) / sqrMagnitude) : 0f);
							if (num2 <= 1f || !flag)
							{
								this.PushOut(j, vector + vector2 * Mathf.Clamp01(num2), num);
							}
						}
					}
				}
			}
			for (int k = 0; k < 2; k++)
			{
				if (this._sphereValid[k])
				{
					for (int l = 0; l < this._nodeCount; l++)
					{
						if (!this._pinned[l])
						{
							this.PushOut(l, this._sphere[k], this._sphereRadius);
						}
					}
				}
			}
			if (this._skinValid)
			{
				this.CollideSkinContacts();
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06007629 RID: 30249 RVA: 0x002FF534 File Offset: 0x002FD734
		private float SkinClearance
		{
			get
			{
				return (Mathf.Max(0f, this._settings.radius) + Mathf.Max(0f, this._settings.skinClearance)) * this._scale;
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x0600762A RID: 30250 RVA: 0x002FF568 File Offset: 0x002FD768
		private float SkinReach
		{
			get
			{
				return Mathf.Max(0.0001f, this._settings.skinContactReach) * this._scale;
			}
		}

		// Token: 0x0600762B RID: 30251 RVA: 0x002FF588 File Offset: 0x002FD788
		private int SkinSide(int node, Vector3 point)
		{
			if (node < this._rightStart)
			{
				return 0;
			}
			if (node < this._junction)
			{
				return 1;
			}
			if ((point - this._pin[0]).sqrMagnitude > (point - this._pin[1]).sqrMagnitude)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600762C RID: 30252 RVA: 0x002FF5E4 File Offset: 0x002FD7E4
		private void RefreshSkinContacts()
		{
			float skinReach = this.SkinReach;
			for (int i = 0; i < this._nodeCount; i++)
			{
				if (this._pinned[i])
				{
					this._hasContact[i] = false;
				}
				else
				{
					this._hasContact[i] = this._skinSurface.TryContact(this.SkinSide(i, this._positions[i]), this._positions[i], skinReach, ref this._contactHint[i], out this._contactPoint[i], out this._contactNormal[i]);
				}
			}
		}

		// Token: 0x0600762D RID: 30253 RVA: 0x002FF678 File Offset: 0x002FD878
		private bool CollideSkinContacts()
		{
			bool flag = false;
			float skinClearance = this.SkinClearance;
			for (int i = 0; i < this._nodeCount; i++)
			{
				if (this._hasContact[i] && !this._pinned[i])
				{
					Vector3 vector = this._positions[i];
					float num = NippleRotorBreastSurface.PushOut(ref vector, this._contactPoint[i], this._contactNormal[i], skinClearance);
					if (num > 0f)
					{
						Vector3 vector2 = this._contactNormal[i];
						float num2 = Vector3.Dot(this._positions[i] - this._previous[i], vector2);
						this._positions[i] = vector;
						this._previous[i] += vector2 * num;
						if (num2 < 0f)
						{
							this._previous[i] += vector2 * num2;
						}
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600762E RID: 30254 RVA: 0x002FF791 File Offset: 0x002FD991
		private bool ProjectOntoSkin()
		{
			this.RefreshSkinContacts();
			return this.CollideSkinContacts();
		}

		// Token: 0x0600762F RID: 30255 RVA: 0x002FF7A0 File Offset: 0x002FD9A0
		private Vector3 KeepExitOffSkin(Vector3 exit, int side)
		{
			if (!this._skinValid)
			{
				return exit;
			}
			int num = this._exitHint[side];
			Vector3 vector;
			Vector3 vector2;
			if (this._skinSurface.TryContact(side, exit, this.SkinReach, ref num, out vector, out vector2))
			{
				NippleRotorBreastSurface.PushOut(ref exit, vector, vector2, this.SkinClearance);
			}
			this._exitHint[side] = num;
			return exit;
		}

		// Token: 0x06007630 RID: 30256 RVA: 0x002FF7F8 File Offset: 0x002FD9F8
		private void GuardRings(int strand, int first, int end)
		{
			float skinReach = this.SkinReach;
			float skinClearance = this.SkinClearance;
			for (int i = ((strand < 2) ? (first + 1) : first); i < end; i++)
			{
				Vector3 vector = this._ringPositions[i];
				int num = ((strand < 2) ? strand : (((vector - this._pin[0]).sqrMagnitude <= (vector - this._pin[1]).sqrMagnitude) ? 0 : 1));
				Vector3 vector2;
				Vector3 vector3;
				if (this._skinSurface.TryContact(num, vector, skinReach, ref this._ringHint[i], out vector2, out vector3) && NippleRotorBreastSurface.PushOut(ref vector, vector2, vector3, skinClearance) > 0f)
				{
					this._ringPositions[i] = vector;
				}
			}
		}

		// Token: 0x06007631 RID: 30257 RVA: 0x002FF8C4 File Offset: 0x002FDAC4
		private void CollideSingle()
		{
			float num = (this._freeEnd ? (Mathf.Max(0f, this._settings.freeEndRadius) * this._scale) : 0f);
			for (int i = 0; i < this._capsuleCount; i++)
			{
				float num2 = this._capsuleRadius[i];
				if (this._capsuleValid[i] && num2 > 0f)
				{
					Vector3 vector = this._capsuleA[i];
					Vector3 vector2 = this._capsuleB[i] - vector;
					float sqrMagnitude = vector2.sqrMagnitude;
					bool flag = this._capsuleOpenEnd[i];
					for (int j = 0; j < this._nodeCount; j++)
					{
						if (!this._pinned[j])
						{
							float num3 = num2 * this._collisionScale[j];
							if (j == this._heavyNode)
							{
								num3 += num;
							}
							if (num3 > 0f)
							{
								float num4 = ((sqrMagnitude > 1E-10f) ? (Vector3.Dot(this._positions[j] - vector, vector2) / sqrMagnitude) : 0f);
								if (num4 <= 1f || !flag)
								{
									this.PushOut(j, vector + vector2 * Mathf.Clamp01(num4), num3);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x002FFA14 File Offset: 0x002FDC14
		private void PushOut(int index, Vector3 centre, float radius)
		{
			Vector3 vector = this._positions[index] - centre;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude >= radius * radius)
			{
				return;
			}
			float num = Mathf.Sqrt(sqrMagnitude);
			Vector3 vector2 = ((num > 1E-06f) ? (vector / num) : this._front);
			float num2 = radius - num;
			float num3 = Vector3.Dot(this._positions[index] - this._previous[index], vector2);
			this._positions[index] += vector2 * num2;
			this._previous[index] += vector2 * num2;
			if (num3 < 0f)
			{
				this._previous[index] += vector2 * num3;
			}
		}

		// Token: 0x06007633 RID: 30259 RVA: 0x002FFB04 File Offset: 0x002FDD04
		private void UpdateRestLengths()
		{
			if (this._single)
			{
				this._restMain = this._segmentMain * (this._freeEnd ? 1f : this.SingleExtension(this._targets[1], this._targets[this._last - 1]));
				this._endShare = 1f / (1f + Mathf.Clamp(this._settings.freeEndMass, 0.1f, 100f));
				return;
			}
			float num;
			float num2;
			this.Extensions(this._targets[1], this._targets[this._rightStart + 1], this._targets[this._remoteIndex - 1], out num, out num2);
			this._restBranch = this._segmentBranch * num;
			this._restMain = this._segmentMain * num2;
		}

		// Token: 0x06007634 RID: 30260 RVA: 0x002FFBE0 File Offset: 0x002FDDE0
		private void Extensions(Vector3 left, Vector3 right, Vector3 remote, out float branch, out float main)
		{
			float num = Mathf.Max(1f, this._settings.maxStretch);
			float num2 = (float)(this._branch - 1) * this._segmentBranch;
			float num3 = (float)(this._main - 1) * this._segmentMain;
			branch = Mathf.Clamp(Vector3.Distance(left, right) / (2f * num2 * 0.98f), 1f, num);
			float num4 = Mathf.Max(Vector3.Distance(left, remote), Vector3.Distance(right, remote)) / 0.98f;
			main = Mathf.Clamp((num4 - num2 * branch) / num3, 1f, num);
			branch = Mathf.Max(branch, Mathf.Clamp((num4 - num3 * main) / num2, 1f, num));
		}

		// Token: 0x06007635 RID: 30261 RVA: 0x002FFC98 File Offset: 0x002FDE98
		private float SingleExtension(Vector3 startExit, Vector3 endExit)
		{
			float num = (float)(this._main - 2) * this._segmentMain;
			return Mathf.Clamp(Vector3.Distance(startExit, endExit) / (num * 0.98f), 1f, Mathf.Max(1f, this._settings.maxStretch));
		}

		// Token: 0x06007636 RID: 30262 RVA: 0x002FFCE4 File Offset: 0x002FDEE4
		private void ComputeTargets(Vector3[] output)
		{
			if (this._single)
			{
				this.ComputeSingleTargets(output);
				return;
			}
			Vector3 vector = this.KeepExitOffSkin(this._pin[0] + this._pinDirection[0] * this._segmentBranch, 0);
			Vector3 vector2 = this.KeepExitOffSkin(this._pin[1] + this._pinDirection[1] * this._segmentBranch, 1);
			Vector3 vector3 = this._pin[2] + this._pinDirection[2] * this._segmentMain;
			output[0] = this._pin[0];
			output[1] = vector;
			output[this._rightStart] = this._pin[1];
			output[this._rightStart + 1] = vector2;
			output[this._remoteIndex] = this._pin[2];
			output[this._remoteIndex - 1] = vector3;
			float num;
			float num2;
			this.Extensions(vector, vector2, vector3, out num, out num2);
			float num3 = (float)(this._branch - 1) * this._segmentBranch * num;
			float num4 = (float)(this._main - 1) * this._segmentMain * num2;
			Vector3 vector4 = NippleRotorCord.JunctionTarget(vector, vector2, vector3, num3);
			output[this._junction] = vector4;
			this.Sag(output, vector, vector4, num3, 2, this._branch - 1);
			this.Sag(output, vector2, vector4, num3, this._rightStart + 2, this._branch - 1);
			this.Sag(output, vector4, vector3, num4, this._junction + 1, this._main - 1);
		}

		// Token: 0x06007637 RID: 30263 RVA: 0x002FFE90 File Offset: 0x002FE090
		private void ComputeSingleTargets(Vector3[] output)
		{
			Vector3 vector = this._pin[0] + this._pinDirection[0] * this._segmentMain;
			output[0] = this._pin[0];
			output[1] = vector;
			if (this._freeEnd)
			{
				Vector3 vector2 = NippleRotorCord.GravityDirection();
				for (int i = 2; i <= this._last; i++)
				{
					output[i] = vector + vector2 * ((float)(i - 1) * this._segmentMain);
				}
				return;
			}
			Vector3 vector3 = this._pin[1] + this._pinDirection[1] * this._segmentMain;
			output[this._last] = this._pin[1];
			output[this._last - 1] = vector3;
			float num = (float)(this._main - 2) * this._segmentMain * this.SingleExtension(vector, vector3);
			this.Sag(output, vector, vector3, num, 2, this._main - 2);
		}

		// Token: 0x06007638 RID: 30264 RVA: 0x002FFFA4 File Offset: 0x002FE1A4
		private static Vector3 JunctionTarget(Vector3 left, Vector3 right, Vector3 remote, float freeBranch)
		{
			Vector3 vector = (left + right) * 0.5f;
			Vector3 vector2 = left - vector;
			Vector3 vector3 = remote - vector;
			float magnitude = vector3.magnitude;
			Vector3 vector4 = NippleRotorCord.GravityDirection();
			if (magnitude > 1E-06f)
			{
				vector4 += vector3 * (0.5f / magnitude);
			}
			vector4.Normalize();
			float num = Mathf.Abs(Vector3.Dot(vector4, vector2));
			float num2 = freeBranch * 0.97f;
			float num3 = num * num - vector2.sqrMagnitude + num2 * num2;
			float num4 = ((num3 > 0f) ? Mathf.Max(0f, Mathf.Sqrt(num3) - num) : 0f);
			return vector + vector4 * num4;
		}

		// Token: 0x06007639 RID: 30265 RVA: 0x0030006C File Offset: 0x002FE26C
		private void Sag(Vector3[] output, Vector3 from, Vector3 to, float length, int start, int segments)
		{
			if (segments < 2)
			{
				return;
			}
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			Vector3 vector2 = NippleRotorCord.GravityDirection();
			Vector3 vector3 = ((magnitude > 1E-06f) ? (vector / magnitude) : vector2);
			Vector3 vector4 = vector2 - vector3 * Vector3.Dot(vector2, vector3);
			float magnitude2 = vector4.magnitude;
			if (magnitude2 < 0.3f)
			{
				Vector3 vector5 = this._front - vector3 * Vector3.Dot(this._front, vector3);
				if (vector5.sqrMagnitude < 1E-08f)
				{
					vector5 = Vector3.Cross(vector3, (Mathf.Abs(vector3.x) < 0.9f) ? Vector3.right : Vector3.forward);
				}
				vector4 += vector5.normalized * (0.3f - magnitude2);
			}
			vector4.Normalize();
			float num = 0.5f * Mathf.Sqrt(Mathf.Max(0f, length * length - magnitude * magnitude));
			Vector3 vector6 = (from + to) * 0.5f + vector4 * (2f * num);
			for (int i = 1; i < segments; i++)
			{
				float num2 = (float)i / (float)segments;
				float num3 = 1f - num2;
				output[start + i - 1] = from * (num3 * num3) + vector6 * (2f * num3 * num2) + to * (num2 * num2);
			}
		}

		// Token: 0x0600763A RID: 30266 RVA: 0x003001F0 File Offset: 0x002FE3F0
		private void ResetToTargets()
		{
			this.Restart(true);
		}

		// Token: 0x0600763B RID: 30267 RVA: 0x003001F9 File Offset: 0x002FE3F9
		private bool ReleaseInPlace(int nodeCount)
		{
			if (!this.EnsureTopology() || !this._nodesKept || this._nodeCount != nodeCount)
			{
				return false;
			}
			if (!this.ReadPins())
			{
				return false;
			}
			this.Restart(false);
			return true;
		}

		// Token: 0x0600763C RID: 30268 RVA: 0x00300228 File Offset: 0x002FE428
		private void Restart(bool fromTargets)
		{
			this.ComputeTargets(this._frameTargets);
			if (fromTargets)
			{
				Array.Copy(this._frameTargets, this._positions, this._nodeCount);
			}
			else
			{
				for (int i = 0; i < this._nodeCount; i++)
				{
					if (this._pinned[i] || !NippleRotorCord.IsFinite(this._positions[i]))
					{
						this._positions[i] = this._frameTargets[i];
					}
				}
			}
			Array.Copy(this._frameTargets, this._targets, this._nodeCount);
			for (int j = 0; j < this._capsuleCount; j++)
			{
				this._capsuleLastA[j] = this._capsuleNowA[j];
				this._capsuleLastB[j] = this._capsuleNowB[j];
			}
			for (int k = 0; k < 2; k++)
			{
				this._sphereLast[k] = this._sphereNow[k];
			}
			if (this._anyCollider)
			{
				this.InterpolateColliders(1f);
			}
			this.UpdateRestLengths();
			if (this._skinValid)
			{
				this.RefreshSkinContacts();
			}
			this.Relax(Mathf.Clamp(this._settings.iterations * 4, 1, 256));
			if (this._skinValid)
			{
				this.ProjectOntoSkin();
			}
			Array.Copy(this._positions, this._previous, this._nodeCount);
			this._previousStep = 0f;
			this._needsReset = false;
			this._initialized = true;
			this.Remember();
			this.RebuildMesh();
		}

		// Token: 0x0600763D RID: 30269 RVA: 0x003003B0 File Offset: 0x002FE5B0
		private void Remember()
		{
			for (int i = 0; i < 3; i++)
			{
				this._lastPin[i] = this._pin[i];
			}
			this._lastRootPosition = this._rootPosition;
			this._lastRootRotation = this._rootRotation;
			this._lastHasRoot = this._hasRoot;
			this._lastScale = this._scale;
			Array.Copy(this._frameTargets, this._lastTargets, this._nodeCount);
			for (int j = 0; j < this._capsuleCount; j++)
			{
				this._capsuleLastA[j] = this._capsuleNowA[j];
				this._capsuleLastB[j] = this._capsuleNowB[j];
			}
			for (int k = 0; k < 2; k++)
			{
				this._sphereLast[k] = this._sphereNow[k];
			}
		}

		// Token: 0x0600763E RID: 30270 RVA: 0x0030048D File Offset: 0x002FE68D
		private void RebuildMesh()
		{
			this.BuildTube();
			this.PlaceFollower();
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x0030049C File Offset: 0x002FE69C
		private void BuildTube()
		{
			if (this._mesh == null || this._vertices == null || this._mesh.vertexCount != this._vertices.Length)
			{
				return;
			}
			float num = Mathf.Max(1E-05f, this._settings.radius) * this._scale;
			for (int i = 0; i < this._strandCount; i++)
			{
				int num2 = this._ringStart[i];
				int num3 = this._ringStart[i + 1];
				this.SampleStrand(this._strands[i], num2);
				if (this._skinValid && !this._single)
				{
					this.GuardRings(i, num2, num3);
				}
				this.ComputeRingTangents(num2, num3);
				if (!this._single && i < 2)
				{
					this._ringPositions[num3 - 1] += this._ringTangents[num3 - 1] * num;
				}
			}
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			Matrix4x4 transpose = base.transform.localToWorldMatrix.transpose;
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			int num4 = 0;
			for (int j = 0; j < this._strandCount; j++)
			{
				int num5 = this._ringStart[j];
				int num6 = this._ringStart[j + 1];
				Vector3 vector3 = ((j == 0) ? this._pinUp[0] : ((j == 1) ? this._pinUp[1] : this._front));
				for (int k = num5; k < num6; k++)
				{
					Vector3 vector4 = this._ringTangents[k];
					vector3 -= vector4 * Vector3.Dot(vector3, vector4);
					if (vector3.sqrMagnitude < 1E-10f)
					{
						vector3 = Vector3.Cross(vector4, (Mathf.Abs(vector4.y) < 0.9f) ? Vector3.up : Vector3.right);
					}
					vector3.Normalize();
					Vector3 vector5 = Vector3.Cross(vector4, vector3);
					Vector3 vector6 = this._ringPositions[k];
					for (int l = 0; l <= this._radial; l++)
					{
						float num7 = this._cos[l];
						float num8 = this._sin[l];
						Vector3 vector7 = vector3 * num7 + vector5 * num8;
						Vector3 vector8 = vector5 * num7 - vector3 * num8;
						Vector3 vector9 = worldToLocalMatrix.MultiplyPoint3x4(vector6 + vector7 * num);
						Vector3 normalized = worldToLocalMatrix.MultiplyVector(vector8).normalized;
						this._vertices[num4] = vector9;
						this._normals[num4] = transpose.MultiplyVector(vector7).normalized;
						this._tangents[num4] = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
						vector = Vector3.Min(vector, vector9);
						vector2 = Vector3.Max(vector2, vector9);
						num4++;
					}
				}
				if (this._single)
				{
					this._endNormal = vector3;
				}
			}
			this._mesh.SetVertices(this._vertices);
			this._mesh.SetNormals(this._normals);
			this._mesh.SetTangents(this._tangents);
			this._mesh.bounds = new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x00300850 File Offset: 0x002FEA50
		private void PlaceFollower()
		{
			if (!this._single || !this._freeEnd || !this._initialized || this._hidden || this._positions == null || this._freeEndFollower == null)
			{
				return;
			}
			Vector3 vector = this._positions[this._last];
			Vector3 vector2 = this._positions[this._last - 1] - vector;
			if (!NippleRotorCord.IsFinite(vector) || !NippleRotorCord.IsFinite(vector2))
			{
				return;
			}
			float magnitude = vector2.magnitude;
			if (magnitude < 1E-06f)
			{
				this._freeEndFollower.position = vector;
				return;
			}
			Vector3 vector3 = vector2 / magnitude;
			Vector3 vector4 = this._endNormal - vector3 * Vector3.Dot(this._endNormal, vector3);
			if (vector4.sqrMagnitude < 1E-08f)
			{
				vector4 = this._front - vector3 * Vector3.Dot(this._front, vector3);
			}
			if (vector4.sqrMagnitude < 1E-08f)
			{
				vector4 = Vector3.Cross(vector3, (Mathf.Abs(vector3.x) < 0.9f) ? Vector3.right : Vector3.forward);
			}
			this._freeEndFollower.SetPositionAndRotation(vector, Quaternion.LookRotation(vector3, vector4));
		}

		// Token: 0x06007641 RID: 30273 RVA: 0x0030098C File Offset: 0x002FEB8C
		private void SampleStrand(int[] nodes, int ring)
		{
			int num = nodes.Length;
			for (int i = 0; i < num - 1; i++)
			{
				Vector3 vector = this._positions[nodes[i]];
				Vector3 vector2 = this._positions[nodes[i + 1]];
				Vector3 vector3 = ((i > 0) ? this._positions[nodes[i - 1]] : (2f * vector - vector2));
				Vector3 vector4 = ((i + 2 < num) ? this._positions[nodes[i + 2]] : (2f * vector2 - vector));
				for (int j = 0; j < this._subdivisions; j++)
				{
					this._ringPositions[ring++] = NippleRotorCord.CatmullRom(vector3, vector, vector2, vector4, (float)j / (float)this._subdivisions);
				}
			}
			this._ringPositions[ring] = this._positions[nodes[num - 1]];
		}

		// Token: 0x06007642 RID: 30274 RVA: 0x00300A80 File Offset: 0x002FEC80
		private void ComputeRingTangents(int first, int end)
		{
			for (int i = first; i < end; i++)
			{
				Vector3 vector = this._ringPositions[Mathf.Min(i + 1, end - 1)] - this._ringPositions[Mathf.Max(i - 1, first)];
				this._ringTangents[i] = ((vector.sqrMagnitude > 1E-14f) ? vector.normalized : ((i > first) ? this._ringTangents[i - 1] : NippleRotorCord.GravityDirection()));
			}
		}

		// Token: 0x06007643 RID: 30275 RVA: 0x00300B08 File Offset: 0x002FED08
		private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float num = t * t;
			float num2 = num * t;
			return 0.5f * (2f * p1 + (p2 - p0) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (3f * p1 - p0 - 3f * p2 + p3) * num2);
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x00300BB8 File Offset: 0x002FEDB8
		private static Vector3 GravityDirection()
		{
			Vector3 gravity = Physics.gravity;
			if (gravity.sqrMagnitude <= 1E-08f)
			{
				return Vector3.down;
			}
			return gravity.normalized;
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x00300BE6 File Offset: 0x002FEDE6
		private static bool IsFinite(Vector3 value)
		{
			return NippleRotorCord.IsFinite(value.x) && NippleRotorCord.IsFinite(value.y) && NippleRotorCord.IsFinite(value.z);
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool IsFinite(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x06007647 RID: 30279 RVA: 0x00300C0F File Offset: 0x002FEE0F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			NippleRotorCord.ActiveCords.Clear();
			NippleRotorCord.FrameCords.Clear();
		}

		// Token: 0x06007648 RID: 30280 RVA: 0x00300C28 File Offset: 0x002FEE28
		private static void Register(NippleRotorCord cord)
		{
			if (!NippleRotorCord.ActiveCords.Contains(cord))
			{
				NippleRotorCord.ActiveCords.Add(cord);
			}
			if (!Application.isPlaying)
			{
				return;
			}
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			if (NippleRotorCord.InstallLoop(ref currentPlayerLoop))
			{
				PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			}
		}

		// Token: 0x06007649 RID: 30281 RVA: 0x00300C6A File Offset: 0x002FEE6A
		private static void Unregister(NippleRotorCord cord)
		{
			NippleRotorCord.ActiveCords.Remove(cord);
		}

		// Token: 0x0600764A RID: 30282 RVA: 0x00300C78 File Offset: 0x002FEE78
		private static bool InstallLoop(ref PlayerLoopSystem loop)
		{
			if (loop.subSystemList == null)
			{
				return false;
			}
			for (int i = 0; i < loop.subSystemList.Length; i++)
			{
				if (!(loop.subSystemList[i].type != typeof(PostLateUpdate)))
				{
					PlayerLoopSystem playerLoopSystem = loop.subSystemList[i];
					PlayerLoopSystem[] subSystemList = playerLoopSystem.subSystemList;
					if (subSystemList == null)
					{
						return false;
					}
					PlayerLoopSystem[] array = subSystemList;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].type == typeof(NippleRotorCord.NippleRotorCordUpdate))
						{
							return false;
						}
					}
					for (int k = 0; k < subSystemList.Length; k++)
					{
						if (!(subSystemList[k].type != typeof(PostLateUpdate.UpdateAllRenderers)))
						{
							PlayerLoopSystem[] array2 = new PlayerLoopSystem[subSystemList.Length + 1];
							Array.Copy(subSystemList, 0, array2, 0, k);
							array2[k] = new PlayerLoopSystem
							{
								type = typeof(NippleRotorCord.NippleRotorCordUpdate),
								updateDelegate = new PlayerLoopSystem.UpdateFunction(NippleRotorCord.RunLoop)
							};
							Array.Copy(subSystemList, k, array2, k + 1, subSystemList.Length - k);
							playerLoopSystem.subSystemList = array2;
							loop.subSystemList[i] = playerLoopSystem;
							return true;
						}
					}
				}
			}
			Debug.LogError("[NippleRotor] Could not locate the final renderer update in the player loop.");
			return false;
		}

		// Token: 0x0600764B RID: 30283 RVA: 0x00300DD8 File Offset: 0x002FEFD8
		private static void RunLoop()
		{
			if (!Application.isPlaying || NippleRotorCord.ActiveCords.Count == 0)
			{
				return;
			}
			NippleRotorCord.FrameCords.Clear();
			NippleRotorCord.FrameCords.AddRange(NippleRotorCord.ActiveCords);
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < NippleRotorCord.FrameCords.Count; i++)
			{
				NippleRotorCord nippleRotorCord = NippleRotorCord.FrameCords[i];
				if (!(nippleRotorCord == null) && nippleRotorCord.isActiveAndEnabled)
				{
					try
					{
						nippleRotorCord.StepFromPlayerLoop(deltaTime);
					}
					catch (Exception ex)
					{
						nippleRotorCord._needsReset = true;
						Debug.LogException(ex, nippleRotorCord);
					}
				}
			}
			NippleRotorCord.FrameCords.Clear();
		}

		// Token: 0x040078EE RID: 30958
		private const float MaxResetDelta = 0.25f;

		// Token: 0x040078EF RID: 30959
		private const float MaxIntegratedFrame = 0.06666667f;

		// Token: 0x040078F0 RID: 30960
		private const float MinIntegratedDelta = 1E-05f;

		// Token: 0x040078F1 RID: 30961
		private const int MaxSubsteps = 16;

		// Token: 0x040078F2 RID: 30962
		private const int MaxCapsules = 5;

		// Token: 0x040078F3 RID: 30963
		private const int Left = 0;

		// Token: 0x040078F4 RID: 30964
		private const int Right = 1;

		// Token: 0x040078F5 RID: 30965
		private const int Remote = 2;

		// Token: 0x040078F6 RID: 30966
		private const float TransportAngle = 60f;

		// Token: 0x040078F7 RID: 30967
		private const float PoseEpsilon = 0.002f;

		// Token: 0x040078F8 RID: 30968
		private const float ScaleTolerance = 0.02f;

		// Token: 0x040078F9 RID: 30969
		private const float MaxUncarriedSpeed = 1f;

		// Token: 0x040078FA RID: 30970
		private const float JunctionSlack = 0.97f;

		// Token: 0x040078FB RID: 30971
		private const float TautSlack = 0.98f;

		// Token: 0x040078FC RID: 30972
		private const float MaxVelocityRatio = 3f;

		// Token: 0x040078FD RID: 30973
		private const float MaxSingleSpeed = 4f;

		// Token: 0x040078FE RID: 30974
		private const int PinCollisionRampLinks = 3;

		// Token: 0x040078FF RID: 30975
		private const int StartPin = 0;

		// Token: 0x04007900 RID: 30976
		private const int EndPin = 1;

		// Token: 0x04007901 RID: 30977
		private const string BodyRendererName = "Body_sotai_NSFW";

		// Token: 0x04007902 RID: 30978
		private static readonly List<NippleRotorCord> ActiveCords = new List<NippleRotorCord>();

		// Token: 0x04007903 RID: 30979
		private static readonly List<NippleRotorCord> FrameCords = new List<NippleRotorCord>();

		// Token: 0x04007904 RID: 30980
		private NippleRotorCordSettings _settings;

		// Token: 0x04007905 RID: 30981
		private Material _material;

		// Token: 0x04007906 RID: 30982
		private Transform _leftSocket;

		// Token: 0x04007907 RID: 30983
		private Transform _rightSocket;

		// Token: 0x04007908 RID: 30984
		private Transform _remoteSocket;

		// Token: 0x04007909 RID: 30985
		private Transform _playerRoot;

		// Token: 0x0400790A RID: 30986
		private Transform _startSocket;

		// Token: 0x0400790B RID: 30987
		private Transform _endSocket;

		// Token: 0x0400790C RID: 30988
		private Transform _scaleSource;

		// Token: 0x0400790D RID: 30989
		private Transform _freeEndFollower;

		// Token: 0x0400790E RID: 30990
		private NipplePiercingAttachment _attachment;

		// Token: 0x0400790F RID: 30991
		private Animator _animator;

		// Token: 0x04007910 RID: 30992
		private MeshFilter _filter;

		// Token: 0x04007911 RID: 30993
		private MeshRenderer _renderer;

		// Token: 0x04007912 RID: 30994
		private Mesh _mesh;

		// Token: 0x04007913 RID: 30995
		private bool _configured;

		// Token: 0x04007914 RID: 30996
		private bool _hidden;

		// Token: 0x04007915 RID: 30997
		private bool _initialized;

		// Token: 0x04007916 RID: 30998
		private bool _needsReset;

		// Token: 0x04007917 RID: 30999
		private bool _subscribed;

		// Token: 0x04007918 RID: 31000
		private bool _topologyDirty = true;

		// Token: 0x04007919 RID: 31001
		private NippleRotorBreastSurface _skinSurface;

		// Token: 0x0400791A RID: 31002
		private bool _skinValid;

		// Token: 0x0400791B RID: 31003
		private Vector3[] _contactPoint;

		// Token: 0x0400791C RID: 31004
		private Vector3[] _contactNormal;

		// Token: 0x0400791D RID: 31005
		private bool[] _hasContact;

		// Token: 0x0400791E RID: 31006
		private int[] _contactHint;

		// Token: 0x0400791F RID: 31007
		private int[] _ringHint;

		// Token: 0x04007920 RID: 31008
		private readonly int[] _exitHint = new int[] { -1, -1 };

		// Token: 0x04007921 RID: 31009
		private bool _single;

		// Token: 0x04007922 RID: 31010
		private bool _freeEnd;

		// Token: 0x04007923 RID: 31011
		private bool _nodesKept;

		// Token: 0x04007924 RID: 31012
		private int _steppedFrame = -1;

		// Token: 0x04007925 RID: 31013
		private int _branch;

		// Token: 0x04007926 RID: 31014
		private int _main;

		// Token: 0x04007927 RID: 31015
		private int _radial;

		// Token: 0x04007928 RID: 31016
		private int _subdivisions;

		// Token: 0x04007929 RID: 31017
		private int _rightStart;

		// Token: 0x0400792A RID: 31018
		private int _junction;

		// Token: 0x0400792B RID: 31019
		private int _remoteIndex;

		// Token: 0x0400792C RID: 31020
		private int _nodeCount;

		// Token: 0x0400792D RID: 31021
		private int _linkCount;

		// Token: 0x0400792E RID: 31022
		private int _pinCount = 3;

		// Token: 0x0400792F RID: 31023
		private int _strandCount = 3;

		// Token: 0x04007930 RID: 31024
		private int _last;

		// Token: 0x04007931 RID: 31025
		private int _heavyNode = -1;

		// Token: 0x04007932 RID: 31026
		private Vector3[] _positions;

		// Token: 0x04007933 RID: 31027
		private Vector3[] _previous;

		// Token: 0x04007934 RID: 31028
		private Vector3[] _targets;

		// Token: 0x04007935 RID: 31029
		private Vector3[] _lastTargets;

		// Token: 0x04007936 RID: 31030
		private Vector3[] _frameTargets;

		// Token: 0x04007937 RID: 31031
		private bool[] _pinned;

		// Token: 0x04007938 RID: 31032
		private int[] _linkA;

		// Token: 0x04007939 RID: 31033
		private int[] _linkB;

		// Token: 0x0400793A RID: 31034
		private bool[] _linkMain;

		// Token: 0x0400793B RID: 31035
		private float[] _collisionScale;

		// Token: 0x0400793C RID: 31036
		private int[][] _strands;

		// Token: 0x0400793D RID: 31037
		private int[] _ringStart;

		// Token: 0x0400793E RID: 31038
		private Vector3[] _ringPositions;

		// Token: 0x0400793F RID: 31039
		private Vector3[] _ringTangents;

		// Token: 0x04007940 RID: 31040
		private Vector3[] _vertices;

		// Token: 0x04007941 RID: 31041
		private Vector3[] _normals;

		// Token: 0x04007942 RID: 31042
		private Vector4[] _tangents;

		// Token: 0x04007943 RID: 31043
		private Vector2[] _uvs;

		// Token: 0x04007944 RID: 31044
		private int[] _triangles;

		// Token: 0x04007945 RID: 31045
		private float[] _cos;

		// Token: 0x04007946 RID: 31046
		private float[] _sin;

		// Token: 0x04007947 RID: 31047
		private float _uvBranchLength = -1f;

		// Token: 0x04007948 RID: 31048
		private float _uvMainLength = -1f;

		// Token: 0x04007949 RID: 31049
		private float _uvRepeat = -1f;

		// Token: 0x0400794A RID: 31050
		private readonly Vector3[] _pin = new Vector3[3];

		// Token: 0x0400794B RID: 31051
		private readonly Vector3[] _pinDirection = new Vector3[3];

		// Token: 0x0400794C RID: 31052
		private readonly Vector3[] _pinUp = new Vector3[3];

		// Token: 0x0400794D RID: 31053
		private readonly Vector3[] _lastPin = new Vector3[3];

		// Token: 0x0400794E RID: 31054
		private Vector3 _rootPosition;

		// Token: 0x0400794F RID: 31055
		private Vector3 _lastRootPosition;

		// Token: 0x04007950 RID: 31056
		private Vector3 _front = Vector3.forward;

		// Token: 0x04007951 RID: 31057
		private Vector3 _endNormal;

		// Token: 0x04007952 RID: 31058
		private Quaternion _rootRotation = Quaternion.identity;

		// Token: 0x04007953 RID: 31059
		private Quaternion _lastRootRotation = Quaternion.identity;

		// Token: 0x04007954 RID: 31060
		private bool _hasRoot;

		// Token: 0x04007955 RID: 31061
		private bool _lastHasRoot;

		// Token: 0x04007956 RID: 31062
		private float _scale = 1f;

		// Token: 0x04007957 RID: 31063
		private float _lastScale = 1f;

		// Token: 0x04007958 RID: 31064
		private float _segmentBranch;

		// Token: 0x04007959 RID: 31065
		private float _segmentMain;

		// Token: 0x0400795A RID: 31066
		private float _restBranch;

		// Token: 0x0400795B RID: 31067
		private float _restMain;

		// Token: 0x0400795C RID: 31068
		private float _previousStep;

		// Token: 0x0400795D RID: 31069
		private float _endShare = 0.5f;

		// Token: 0x0400795E RID: 31070
		private readonly Transform[] _capsuleFrom = new Transform[5];

		// Token: 0x0400795F RID: 31071
		private readonly Transform[] _capsuleTo = new Transform[5];

		// Token: 0x04007960 RID: 31072
		private readonly bool[] _capsuleThigh = new bool[5];

		// Token: 0x04007961 RID: 31073
		private readonly bool[] _capsuleOpenEnd = new bool[5];

		// Token: 0x04007962 RID: 31074
		private readonly bool[] _capsuleShin = new bool[5];

		// Token: 0x04007963 RID: 31075
		private readonly bool[] _capsuleValid = new bool[5];

		// Token: 0x04007964 RID: 31076
		private readonly float[] _capsuleRadius = new float[5];

		// Token: 0x04007965 RID: 31077
		private readonly Vector3[] _capsuleA = new Vector3[5];

		// Token: 0x04007966 RID: 31078
		private readonly Vector3[] _capsuleB = new Vector3[5];

		// Token: 0x04007967 RID: 31079
		private readonly Vector3[] _capsuleNowA = new Vector3[5];

		// Token: 0x04007968 RID: 31080
		private readonly Vector3[] _capsuleNowB = new Vector3[5];

		// Token: 0x04007969 RID: 31081
		private readonly Vector3[] _capsuleLastA = new Vector3[5];

		// Token: 0x0400796A RID: 31082
		private readonly Vector3[] _capsuleLastB = new Vector3[5];

		// Token: 0x0400796B RID: 31083
		private readonly Vector3[] _sphere = new Vector3[2];

		// Token: 0x0400796C RID: 31084
		private readonly Vector3[] _sphereNow = new Vector3[2];

		// Token: 0x0400796D RID: 31085
		private readonly Vector3[] _sphereLast = new Vector3[2];

		// Token: 0x0400796E RID: 31086
		private readonly bool[] _sphereValid = new bool[2];

		// Token: 0x0400796F RID: 31087
		private float _sphereRadius;

		// Token: 0x04007970 RID: 31088
		private int _capsuleCount;

		// Token: 0x04007971 RID: 31089
		private bool _anyCollider;

		// Token: 0x04007972 RID: 31090
		private bool _pinsLost;

		// Token: 0x02000F59 RID: 3929
		private struct NippleRotorCordUpdate
		{
		}
	}
}
