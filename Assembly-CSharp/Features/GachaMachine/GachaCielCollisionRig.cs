using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Features.BoobTouch;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011DB RID: 4571
	[DisallowMultipleComponent]
	public sealed class GachaCielCollisionRig : MonoBehaviour
	{
		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06008C7B RID: 35963 RVA: 0x00395CCF File Offset: 0x00393ECF
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06008C7C RID: 35964 RVA: 0x00395CD7 File Offset: 0x00393ED7
		public Animator Animator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x06008C7D RID: 35965 RVA: 0x00395CDF File Offset: 0x00393EDF
		public int CurrentBoobsType
		{
			get
			{
				return this._currentBoobsType;
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06008C7E RID: 35966 RVA: 0x00395CE7 File Offset: 0x00393EE7
		public int OwnerCount
		{
			get
			{
				return this._owners.Count;
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06008C7F RID: 35967 RVA: 0x00395CF4 File Offset: 0x00393EF4
		public int ColliderCount
		{
			get
			{
				return this._colliders.Count;
			}
		}

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x06008C80 RID: 35968 RVA: 0x00395D01 File Offset: 0x00393F01
		public IReadOnlyList<Collider> Colliders
		{
			get
			{
				return this._colliders;
			}
		}

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x06008C81 RID: 35969 RVA: 0x00395D01 File Offset: 0x00393F01
		public IReadOnlyList<Collider> ActiveColliders
		{
			get
			{
				return this._colliders;
			}
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x06008C82 RID: 35970 RVA: 0x00395D09 File Offset: 0x00393F09
		public IReadOnlyList<Collider> BreastColliders
		{
			get
			{
				return this._breastColliders;
			}
		}

		// Token: 0x06008C83 RID: 35971 RVA: 0x00395D14 File Offset: 0x00393F14
		public static GachaCielCollisionRig GetOrCreate(Transform player)
		{
			if (player == null)
			{
				return null;
			}
			GachaCielCollisionRig gachaCielCollisionRig = player.GetComponent<GachaCielCollisionRig>();
			if (gachaCielCollisionRig == null)
			{
				gachaCielCollisionRig = player.gameObject.AddComponent<GachaCielCollisionRig>();
			}
			return gachaCielCollisionRig;
		}

		// Token: 0x06008C84 RID: 35972 RVA: 0x00395D4C File Offset: 0x00393F4C
		public bool Acquire(Object owner, Animator resolvedAnimator = null)
		{
			int num = ((owner != null) ? owner.GetInstanceID() : base.GetInstanceID());
			this.PurgeDestroyedOwners();
			this._owners[num] = ((owner != null) ? owner : this);
			Animator animator = this.ResolveAnimator(resolvedAnimator);
			if (animator == null || !animator.isHuman)
			{
				this._owners.Remove(num);
				Debug.LogWarning("[GachaCielCollisionRig] A valid humanoid Animator was not found on Ciel.", this);
				return false;
			}
			if (this._active && this._animator == animator)
			{
				this.RefreshBreastSizing(false);
				return true;
			}
			this.TearDownShapes();
			this._animator = animator;
			this._boobsController = animator.transform.root.GetComponentInChildren<BoobsController>(true);
			this._bodyRenderer = this.ResolveBodyRenderer(animator);
			this.BuildHumanoidShapes();
			this.BuildBreastShapes();
			this._active = this._colliders.Count > 0;
			Physics.SyncTransforms();
			if (!this._active)
			{
				this._owners.Remove(num);
				Debug.LogWarning("[GachaCielCollisionRig] No collision shapes could be built for Ciel.", this);
				return false;
			}
			if (this._breastColliders.Count != 2)
			{
				Debug.LogWarning("[GachaCielCollisionRig] Breast coverage is incomplete: " + string.Format("{0}/2 shapes. Expected bones ", this._breastColliders.Count) + "'c_breast_01.l' and 'c_breast_01.r'.", this);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaCielCollisionRig] Built {0} shapes ", this._colliders.Count) + string.Format("({0} breast), BoobsType={1}.", this._breastColliders.Count, this._currentBoobsType), this);
			}
			return true;
		}

		// Token: 0x06008C85 RID: 35973 RVA: 0x00395EEA File Offset: 0x003940EA
		public bool BeginSession(Animator resolvedAnimator, GachaMachineController machineController = null)
		{
			if (machineController != null)
			{
				this._machineController = machineController;
			}
			else if (this._machineController == null)
			{
				this._machineController = base.GetComponentInParent<GachaMachineController>();
			}
			return this.Acquire(this, resolvedAnimator);
		}

		// Token: 0x06008C86 RID: 35974 RVA: 0x00395F20 File Offset: 0x00394120
		public void EndSession()
		{
			this.ClearPull();
			this.Release(this);
			if (!this._active)
			{
				this._machineController = null;
			}
		}

		// Token: 0x06008C87 RID: 35975 RVA: 0x00395F3E File Offset: 0x0039413E
		public void BindMachineController(GachaMachineController machineController)
		{
			this._machineController = machineController;
		}

		// Token: 0x06008C88 RID: 35976 RVA: 0x00395F48 File Offset: 0x00394148
		public void Release(Object owner)
		{
			int num = ((owner != null) ? owner.GetInstanceID() : base.GetInstanceID());
			this._owners.Remove(num);
			this.PurgeDestroyedOwners();
			if (this._owners.Count == 0)
			{
				this.TearDownShapes();
			}
		}

		// Token: 0x06008C89 RID: 35977 RVA: 0x00395F93 File Offset: 0x00394193
		public void ForceReleaseAll()
		{
			this._owners.Clear();
			this.TearDownShapes();
		}

		// Token: 0x06008C8A RID: 35978 RVA: 0x00395FA8 File Offset: 0x003941A8
		public void SetSuspended(bool suspended)
		{
			if (this._suspended == suspended)
			{
				return;
			}
			this._suspended = suspended;
			for (int i = 0; i < this._colliders.Count; i++)
			{
				Collider collider = this._colliders[i];
				if (collider != null)
				{
					collider.enabled = !suspended;
				}
			}
			if (!suspended && this._active)
			{
				Physics.SyncTransforms();
			}
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x06008C8B RID: 35979 RVA: 0x0039600C File Offset: 0x0039420C
		public bool IsSuspended
		{
			get
			{
				return this._suspended;
			}
		}

		// Token: 0x06008C8C RID: 35980 RVA: 0x00396014 File Offset: 0x00394214
		public bool IsRigCollider(Collider candidate)
		{
			return candidate != null && this._regions.ContainsKey(candidate);
		}

		// Token: 0x06008C8D RID: 35981 RVA: 0x0039602D File Offset: 0x0039422D
		public bool TryGetRegion(Collider candidate, out GachaCielBodyRegion region)
		{
			if (candidate != null && this._regions.TryGetValue(candidate, out region))
			{
				return true;
			}
			region = GachaCielBodyRegion.Unknown;
			return false;
		}

		// Token: 0x06008C8E RID: 35982 RVA: 0x0039604D File Offset: 0x0039424D
		public bool TryResolveGrabZone(Collider candidate, out GachaCielBodyRegion region)
		{
			return this.TryGetRegion(candidate, out region);
		}

		// Token: 0x06008C8F RID: 35983 RVA: 0x00396058 File Offset: 0x00394258
		public bool TryGetClosestRegion(Vector3 worldPoint, out GachaCielBodyRegion region, out Collider regionCollider)
		{
			region = GachaCielBodyRegion.Unknown;
			regionCollider = null;
			float num = float.PositiveInfinity;
			int num2 = int.MinValue;
			for (int i = 0; i < this._colliders.Count; i++)
			{
				Collider collider = this._colliders[i];
				if (!(collider == null) && collider.enabled)
				{
					float sqrMagnitude = (collider.ClosestPoint(worldPoint) - worldPoint).sqrMagnitude;
					GachaCielBodyRegion gachaCielBodyRegion = this._regions[collider];
					int num3 = GachaCielCollisionRig.RegionPriority(gachaCielBodyRegion);
					if (sqrMagnitude <= num + 1E-06f && (Mathf.Abs(sqrMagnitude - num) > 1E-06f || num3 > num2))
					{
						num = sqrMagnitude;
						num2 = num3;
						region = gachaCielBodyRegion;
						regionCollider = collider;
					}
				}
			}
			return regionCollider != null;
		}

		// Token: 0x06008C90 RID: 35984 RVA: 0x00396112 File Offset: 0x00394312
		public bool TryResolveGrabZone(Vector3 worldPoint, out GachaCielBodyRegion region, out Collider regionCollider)
		{
			return this.TryGetClosestRegion(worldPoint, out region, out regionCollider);
		}

		// Token: 0x06008C91 RID: 35985 RVA: 0x0039611D File Offset: 0x0039431D
		public bool TryGetRegionBone(GachaCielBodyRegion region, out Transform bone)
		{
			return this._regionBones.TryGetValue(region, out bone) && bone != null;
		}

		// Token: 0x06008C92 RID: 35986 RVA: 0x00396138 File Offset: 0x00394338
		public bool TryGetBreastAnchor(GachaCielBodyRegion region, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			Transform transform;
			if (!GachaCielCollisionRig.IsBreast(region) || !this._regionBones.TryGetValue(region, out transform) || transform == null)
			{
				return false;
			}
			anchorWorld = transform.position;
			Transform parent = transform.parent;
			Vector3 vector = ((parent != null) ? parent.forward : transform.forward);
			if (vector.sqrMagnitude > 1E-06f)
			{
				outwardWorld = vector.normalized;
			}
			return true;
		}

		// Token: 0x06008C93 RID: 35987 RVA: 0x003961C8 File Offset: 0x003943C8
		public bool TryGetBreastAnchor(bool left, out Transform tip)
		{
			GachaCielBodyRegion gachaCielBodyRegion = (left ? GachaCielBodyRegion.LeftBreast : GachaCielBodyRegion.RightBreast);
			return this._regionBones.TryGetValue(gachaCielBodyRegion, out tip) && tip != null;
		}

		// Token: 0x06008C94 RID: 35988 RVA: 0x003961F8 File Offset: 0x003943F8
		public bool ApplyPull(GachaCielBodyRegion region, Vector3 requestedWorldOffset, float strength = 1f)
		{
			if (!this._active || region == GachaCielBodyRegion.Unknown)
			{
				return false;
			}
			if (this._machineController == null)
			{
				this._machineController = base.GetComponentInParent<GachaMachineController>();
			}
			if (this._machineController == null)
			{
				return false;
			}
			this._machineController.SetClawGrabOffset(requestedWorldOffset * Mathf.Clamp01(strength));
			return true;
		}

		// Token: 0x06008C95 RID: 35989 RVA: 0x00396254 File Offset: 0x00394454
		public bool ApplyBreastPull(GachaCielBodyRegion region, Vector3 requestedWorldOffset, float strength = 1f)
		{
			return GachaCielCollisionRig.IsBreast(region) && this.ApplyPull(region, requestedWorldOffset, strength);
		}

		// Token: 0x06008C96 RID: 35990 RVA: 0x00396269 File Offset: 0x00394469
		public void ClearPull()
		{
			if (this._machineController != null)
			{
				this._machineController.ClearClawGrabOffset();
			}
		}

		// Token: 0x06008C97 RID: 35991 RVA: 0x00396284 File Offset: 0x00394484
		public static bool IsBreast(GachaCielBodyRegion region)
		{
			return region == GachaCielBodyRegion.LeftBreast || region == GachaCielBodyRegion.RightBreast;
		}

		// Token: 0x06008C98 RID: 35992 RVA: 0x00396290 File Offset: 0x00394490
		private void LateUpdate()
		{
			if (this._active && !this._suspended)
			{
				this.RefreshBreastSizing(false);
			}
		}

		// Token: 0x06008C99 RID: 35993 RVA: 0x003962A9 File Offset: 0x003944A9
		private void OnDisable()
		{
			this.ForceReleaseAll();
		}

		// Token: 0x06008C9A RID: 35994 RVA: 0x003962A9 File Offset: 0x003944A9
		private void OnDestroy()
		{
			this.ForceReleaseAll();
		}

		// Token: 0x06008C9B RID: 35995 RVA: 0x003962B4 File Offset: 0x003944B4
		private Animator ResolveAnimator(Animator candidate)
		{
			if (candidate != null && candidate.isHuman)
			{
				return candidate;
			}
			Animator component = base.GetComponent<Animator>();
			if (component != null && component.isHuman)
			{
				return component;
			}
			Animator[] componentsInChildren = base.GetComponentsInChildren<Animator>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].isHuman)
				{
					return componentsInChildren[i];
				}
			}
			return null;
		}

		// Token: 0x06008C9C RID: 35996 RVA: 0x00396320 File Offset: 0x00394520
		private void BuildHumanoidShapes()
		{
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Hips);
			Transform boneTransform2 = this._animator.GetBoneTransform(HumanBodyBones.Chest);
			Transform boneTransform3 = this._animator.GetBoneTransform(HumanBodyBones.Head);
			Transform boneTransform4 = this._animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
			Transform boneTransform5 = this._animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
			Transform boneTransform6 = this._animator.GetBoneTransform(HumanBodyBones.LeftFoot);
			Transform boneTransform7 = this._animator.GetBoneTransform(HumanBodyBones.LeftToes);
			Transform boneTransform8 = this._animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
			Transform boneTransform9 = this._animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
			Transform boneTransform10 = this._animator.GetBoneTransform(HumanBodyBones.RightFoot);
			Transform boneTransform11 = this._animator.GetBoneTransform(HumanBodyBones.RightToes);
			Transform boneTransform12 = this._animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
			Transform boneTransform13 = this._animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
			Transform boneTransform14 = this._animator.GetBoneTransform(HumanBodyBones.LeftHand);
			Transform boneTransform15 = this._animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
			Transform boneTransform16 = this._animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
			Transform boneTransform17 = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
			if (boneTransform != null && boneTransform2 != null && boneTransform4 != null && boneTransform8 != null)
			{
				this.CreatePelvisBox(boneTransform, boneTransform2, boneTransform4, boneTransform8);
			}
			if (boneTransform2 != null && boneTransform != null && boneTransform3 != null && boneTransform12 != null && boneTransform15 != null)
			{
				this.CreateChestBox(boneTransform2, boneTransform, boneTransform3, boneTransform12, boneTransform15);
			}
			if (boneTransform3 != null && boneTransform2 != null && boneTransform12 != null && boneTransform15 != null)
			{
				this.CreateHeadSphere(boneTransform3, boneTransform2, boneTransform12, boneTransform15);
			}
			this.CreateBoneCapsule(boneTransform4, boneTransform5, GachaCielBodyRegion.LeftUpperLeg, 0.3f);
			this.CreateBoneCapsule(boneTransform5, boneTransform6, GachaCielBodyRegion.LeftLowerLeg, 0.25f);
			this.CreateBoneCapsule(boneTransform8, boneTransform9, GachaCielBodyRegion.RightUpperLeg, 0.3f);
			this.CreateBoneCapsule(boneTransform9, boneTransform10, GachaCielBodyRegion.RightLowerLeg, 0.25f);
			this.CreateBoneCapsule(boneTransform12, boneTransform13, GachaCielBodyRegion.LeftUpperArm, 0.25f);
			this.CreateBoneCapsule(boneTransform13, boneTransform14, GachaCielBodyRegion.LeftLowerArm, 0.22f);
			this.CreateBoneCapsule(boneTransform15, boneTransform16, GachaCielBodyRegion.RightUpperArm, 0.25f);
			this.CreateBoneCapsule(boneTransform16, boneTransform17, GachaCielBodyRegion.RightLowerArm, 0.22f);
			if (this.includeFeet)
			{
				this.CreateExtremityCapsule(boneTransform6, boneTransform7, boneTransform5, GachaCielBodyRegion.LeftFoot, 0.32f);
				this.CreateExtremityCapsule(boneTransform10, boneTransform11, boneTransform9, GachaCielBodyRegion.RightFoot, 0.32f);
			}
			if (this.includeHands)
			{
				this.CreateExtremitySphere(boneTransform14, boneTransform13, GachaCielBodyRegion.LeftHand, 0.18f);
				this.CreateExtremitySphere(boneTransform17, boneTransform16, GachaCielBodyRegion.RightHand, 0.18f);
			}
		}

		// Token: 0x06008C9D RID: 35997 RVA: 0x0039659C File Offset: 0x0039479C
		private void BuildBreastShapes()
		{
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Chest);
			Transform transform = ((boneTransform != null) ? boneTransform : this._animator.transform);
			Transform transform2 = GachaCielCollisionRig.FindDescendant(transform, "c_breast_01.l");
			Transform transform3 = GachaCielCollisionRig.FindDescendant(transform, "c_breast_01.r");
			if (transform2 != null)
			{
				this.CreateBreastSphere(transform2, GachaCielBodyRegion.LeftBreast);
			}
			if (transform3 != null)
			{
				this.CreateBreastSphere(transform3, GachaCielBodyRegion.RightBreast);
			}
			this.RefreshBreastSizing(true);
		}

		// Token: 0x06008C9E RID: 35998 RVA: 0x00396610 File Offset: 0x00394810
		private void CreatePelvisBox(Transform hips, Transform chest, Transform leftUpperLeg, Transform rightUpperLeg)
		{
			GameObject gameObject = this.CreateProxy(hips, GachaCielBodyRegion.Pelvis);
			Vector3 vector = Vector3.Lerp(hips.position, chest.position, 0.48f);
			Bounds bounds = GachaCielCollisionRig.BoundsFromWorldPoints(gameObject.transform, new Vector3[] { leftUpperLeg.position, rightUpperLeg.position, vector });
			GachaCielCollisionRig.ExpandThinAxis(ref bounds, 0.55f, 0.12f);
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.center = bounds.center;
			boxCollider.size = GachaCielCollisionRig.MaxComponents(bounds.size, new Vector3(0.16f, 0.14f, 0.12f));
			this.RegisterCollider(boxCollider, GachaCielBodyRegion.Pelvis, hips);
		}

		// Token: 0x06008C9F RID: 35999 RVA: 0x003966C8 File Offset: 0x003948C8
		private void CreateChestBox(Transform chest, Transform hips, Transform head, Transform leftUpperArm, Transform rightUpperArm)
		{
			GameObject gameObject = this.CreateProxy(chest, GachaCielBodyRegion.Torso);
			Vector3 vector = Vector3.Lerp(hips.position, chest.position, 0.52f);
			Vector3 vector2 = Vector3.Lerp(chest.position, head.position, 0.48f);
			Bounds bounds = GachaCielCollisionRig.BoundsFromWorldPoints(gameObject.transform, new Vector3[] { vector, vector2, leftUpperArm.position, rightUpperArm.position });
			GachaCielCollisionRig.ExpandThinAxis(ref bounds, 0.42f, 0.115f);
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.center = bounds.center;
			boxCollider.size = GachaCielCollisionRig.MaxComponents(bounds.size, new Vector3(0.18f, 0.18f, 0.115f));
			this.RegisterCollider(boxCollider, GachaCielBodyRegion.Torso, chest);
		}

		// Token: 0x06008CA0 RID: 36000 RVA: 0x003967A4 File Offset: 0x003949A4
		private void CreateHeadSphere(Transform head, Transform chest, Transform leftUpperArm, Transform rightUpperArm)
		{
			GameObject gameObject = this.CreateProxy(head, GachaCielBodyRegion.Head);
			float num = Mathf.Clamp(Vector3.Distance(leftUpperArm.position, rightUpperArm.position) * 0.22f, 0.09f, 0.18f);
			float num2 = GachaCielCollisionRig.MaximumAbsComponent(gameObject.transform.lossyScale);
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = num / Mathf.Max(num2, 0.0001f);
			Vector3 vector = gameObject.transform.InverseTransformDirection((head.position - chest.position).normalized);
			sphereCollider.center = vector * sphereCollider.radius * 0.35f;
			this.RegisterCollider(sphereCollider, GachaCielBodyRegion.Head, head);
		}

		// Token: 0x06008CA1 RID: 36001 RVA: 0x00396858 File Offset: 0x00394A58
		private void CreateBoneCapsule(Transform start, Transform end, GachaCielBodyRegion region, float radiusScale)
		{
			if (start == null || end == null)
			{
				return;
			}
			GameObject gameObject = this.CreateProxy(start, region);
			CapsuleCollider capsuleCollider;
			if (!GachaCielCollisionRig.ConfigureCapsule(gameObject.transform, end.position, radiusScale, out capsuleCollider))
			{
				GachaCielCollisionRig.DestroyProxy(gameObject);
				return;
			}
			this.RegisterCollider(capsuleCollider, region, start);
		}

		// Token: 0x06008CA2 RID: 36002 RVA: 0x003968A8 File Offset: 0x00394AA8
		private void CreateExtremityCapsule(Transform start, Transform end, Transform sizeReference, GachaCielBodyRegion region, float radiusScale)
		{
			if (start == null)
			{
				return;
			}
			if (end != null)
			{
				this.CreateBoneCapsule(start, end, region, radiusScale);
				return;
			}
			this.CreateExtremitySphere(start, sizeReference, region, 0.2f);
		}

		// Token: 0x06008CA3 RID: 36003 RVA: 0x003968DC File Offset: 0x00394ADC
		private void CreateExtremitySphere(Transform extremity, Transform previousBone, GachaCielBodyRegion region, float lengthScale)
		{
			if (extremity == null)
			{
				return;
			}
			float num = Mathf.Clamp(((previousBone != null) ? Vector3.Distance(extremity.position, previousBone.position) : 0.2f) * lengthScale, 0.025f, 0.065f);
			GameObject gameObject = this.CreateProxy(extremity, region);
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = num / Mathf.Max(GachaCielCollisionRig.MaximumAbsComponent(gameObject.transform.lossyScale), 0.0001f);
			this.RegisterCollider(sphereCollider, region, extremity);
		}

		// Token: 0x06008CA4 RID: 36004 RVA: 0x00396964 File Offset: 0x00394B64
		private void CreateBreastSphere(Transform bone, GachaCielBodyRegion region)
		{
			SphereCollider sphereCollider = this.CreateProxy(bone, region).AddComponent<SphereCollider>();
			this.RegisterCollider(sphereCollider, region, bone);
			this._breastShapes[region] = sphereCollider;
			this._breastColliders.Add(sphereCollider);
		}

		// Token: 0x06008CA5 RID: 36005 RVA: 0x003969A4 File Offset: 0x00394BA4
		private GameObject CreateProxy(Transform bone, GachaCielBodyRegion region)
		{
			GameObject gameObject = new GameObject("__GachaCielProxy_" + region.ToString())
			{
				hideFlags = HideFlags.DontSave,
				layer = this.ResolveBodyPartLayer()
			};
			gameObject.transform.SetParent(bone, false);
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.mass = 1f;
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			rigidbody.interpolation = RigidbodyInterpolation.None;
			rigidbody.collisionDetectionMode = this.collisionDetection;
			rigidbody.detectCollisions = true;
			rigidbody.maxDepenetrationVelocity = this.maximumDepenetrationVelocity;
			this._proxyObjects.Add(gameObject);
			return gameObject;
		}

		// Token: 0x06008CA6 RID: 36006 RVA: 0x00396A40 File Offset: 0x00394C40
		private void RegisterCollider(Collider collider, GachaCielBodyRegion region, Transform bone)
		{
			collider.isTrigger = false;
			collider.enabled = true;
			this._colliders.Add(collider);
			this._regions[collider] = region;
			this._regionBones[region] = bone;
		}

		// Token: 0x06008CA7 RID: 36007 RVA: 0x00396A78 File Offset: 0x00394C78
		private void RefreshBreastSizing(bool force)
		{
			if (this._breastShapes.Count == 0)
			{
				return;
			}
			if (this._boobsController == null && this._animator != null)
			{
				this._boobsController = this._animator.transform.root.GetComponentInChildren<BoobsController>(true);
			}
			int num = ((this._boobsController != null) ? this._boobsController.CurrentBoobsTypeValue : 3);
			num = Mathf.Clamp(num, 0, 5);
			if (!force && num == this._currentBoobsType)
			{
				return;
			}
			this._currentBoobsType = num;
			BreastInteractionSizeProfile breastInteractionSizeProfile = BreastInteractionSizing.Resolve(num);
			float num2 = Mathf.Clamp(this.defaultBreastRadius * breastInteractionSizeProfile.interactionScale, this.minimumBreastRadius, this.maximumBreastRadius);
			for (int i = 0; i < GachaCielCollisionRig.BreastRegions.Length; i++)
			{
				GachaCielBodyRegion gachaCielBodyRegion = GachaCielCollisionRig.BreastRegions[i];
				SphereCollider sphereCollider;
				Transform transform;
				if (this._breastShapes.TryGetValue(gachaCielBodyRegion, out sphereCollider) && !(sphereCollider == null) && this._regionBones.TryGetValue(gachaCielBodyRegion, out transform) && !(transform == null))
				{
					Vector3 vector = ((transform.parent != null) ? transform.parent.forward : transform.forward);
					if (vector.sqrMagnitude < 1E-06f)
					{
						vector = this._animator.transform.forward;
					}
					vector.Normalize();
					float num3 = num2;
					Vector3 vector2 = transform.position - vector * (num3 * this.breastCenterBackFraction);
					Vector3 vector3;
					float num4;
					if (this.refineBreastsFromSkinnedMesh && this.TryFitBreastSphere(transform, vector, num2, out vector3, out num4))
					{
						vector2 = vector3;
						num3 = num4;
					}
					Transform transform2 = sphereCollider.transform;
					sphereCollider.center = transform2.InverseTransformPoint(vector2);
					sphereCollider.radius = num3 / Mathf.Max(GachaCielCollisionRig.MaximumAbsComponent(transform2.lossyScale), 0.0001f);
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06008CA8 RID: 36008 RVA: 0x00396C5C File Offset: 0x00394E5C
		private bool TryFitBreastSphere(Transform breastBone, Vector3 outwardWorld, float authoredRadius, out Vector3 fittedCenter, out float fittedRadius)
		{
			fittedCenter = Vector3.zero;
			fittedRadius = authoredRadius;
			SkinnedMeshRenderer bodyRenderer = this._bodyRenderer;
			if (bodyRenderer == null || bodyRenderer.sharedMesh == null)
			{
				return false;
			}
			int num = Array.IndexOf<Transform>(bodyRenderer.bones, breastBone);
			if (num < 0)
			{
				return false;
			}
			BoneWeight[] boneWeights = bodyRenderer.sharedMesh.boneWeights;
			if (boneWeights == null || boneWeights.Length == 0)
			{
				return false;
			}
			Mesh mesh = new Mesh
			{
				name = "__GachaCielBreastFit",
				hideFlags = HideFlags.HideAndDontSave
			};
			bool flag;
			try
			{
				bodyRenderer.BakeMesh(mesh, true);
				Vector3[] vertices = mesh.vertices;
				int num2 = Mathf.Min(vertices.Length, boneWeights.Length);
				float num3 = authoredRadius * this.maximumFitDistanceInRadii;
				float num4 = num3 * num3;
				Vector3 vector = breastBone.position - outwardWorld * (authoredRadius * this.breastCenterBackFraction);
				Vector3 vector2 = Vector3.zero;
				float num5 = 0f;
				int num6 = 0;
				for (int i = 0; i < num2; i++)
				{
					float num7 = GachaCielCollisionRig.BoneInfluence(boneWeights[i], num);
					if (num7 >= this.minimumBreastBoneWeight)
					{
						Vector3 vector3 = bodyRenderer.transform.TransformPoint(vertices[i]);
						if ((vector3 - vector).sqrMagnitude <= num4)
						{
							vector2 += vector3 * num7;
							num5 += num7;
							num6++;
						}
					}
				}
				if (num6 < 8 || num5 <= 0.0001f)
				{
					flag = false;
				}
				else
				{
					Vector3 vector4 = vector2 / num5;
					Vector3 vector5 = Vector3.Lerp(vector, vector4, 0.28f);
					List<float> list = new List<float>(num6);
					for (int j = 0; j < num2; j++)
					{
						if (GachaCielCollisionRig.BoneInfluence(boneWeights[j], num) >= this.minimumBreastBoneWeight)
						{
							Vector3 vector6 = bodyRenderer.transform.TransformPoint(vertices[j]);
							if ((vector6 - vector).sqrMagnitude <= num4)
							{
								list.Add(Vector3.Distance(vector6, vector5));
							}
						}
					}
					if (list.Count < 8)
					{
						flag = false;
					}
					else
					{
						list.Sort();
						int num8 = Mathf.Clamp(Mathf.RoundToInt((float)(list.Count - 1) * 0.9f), 0, list.Count - 1);
						float num9 = list[num8] + this.fittedSurfacePadding;
						num9 = Mathf.Clamp(num9, Mathf.Max(this.minimumBreastRadius, authoredRadius * 0.82f), Mathf.Min(this.maximumBreastRadius, authoredRadius * 1.22f));
						float num10 = Vector3.Dot(breastBone.position - vector5, outwardWorld);
						float num11 = num9 * 0.7f;
						if (num10 < num11)
						{
							vector5 -= outwardWorld * (num11 - num10);
						}
						fittedCenter = vector5;
						fittedRadius = num9;
						flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning(string.Concat(new string[] { "[GachaCielCollisionRig] Skinned breast fit failed on '", bodyRenderer.name, "': ", ex.Message, ". Using authored sizing." }), this);
				}
				flag = false;
			}
			finally
			{
				if (Application.isPlaying)
				{
					Object.Destroy(mesh);
				}
				else
				{
					Object.DestroyImmediate(mesh);
				}
			}
			return flag;
		}

		// Token: 0x06008CA9 RID: 36009 RVA: 0x00396FA4 File Offset: 0x003951A4
		private SkinnedMeshRenderer ResolveBodyRenderer(Animator animator)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			int num = int.MinValue;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in animator.transform.root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2 == null) && !(skinnedMeshRenderer2.sharedMesh == null))
				{
					Transform[] bones = skinnedMeshRenderer2.bones;
					bool flag = false;
					bool flag2 = false;
					foreach (Transform transform in bones)
					{
						if (!(transform == null))
						{
							if (transform.name == "c_breast_01.l")
							{
								flag = true;
							}
							else if (transform.name == "c_breast_01.r")
							{
								flag2 = true;
							}
						}
					}
					if (flag && flag2)
					{
						int num2 = skinnedMeshRenderer2.sharedMesh.vertexCount;
						if (skinnedMeshRenderer2.enabled && skinnedMeshRenderer2.gameObject.activeInHierarchy)
						{
							num2 += 1000000;
						}
						if (skinnedMeshRenderer2.name == "Body_sotai_NSFW")
						{
							num2 += 2000000;
						}
						else if (skinnedMeshRenderer2.name == "Body")
						{
							num2 += 1000000;
						}
						if (num2 > num)
						{
							num = num2;
							skinnedMeshRenderer = skinnedMeshRenderer2;
						}
					}
				}
			}
			return skinnedMeshRenderer;
		}

		// Token: 0x06008CAA RID: 36010 RVA: 0x003970E4 File Offset: 0x003952E4
		private void TearDownShapes()
		{
			for (int i = 0; i < this._colliders.Count; i++)
			{
				Collider collider = this._colliders[i];
				if (collider != null)
				{
					collider.enabled = false;
				}
			}
			for (int j = 0; j < this._proxyObjects.Count; j++)
			{
				GachaCielCollisionRig.DestroyProxy(this._proxyObjects[j]);
			}
			this._colliders.Clear();
			this._breastColliders.Clear();
			this._proxyObjects.Clear();
			this._regions.Clear();
			this._regionBones.Clear();
			this._breastShapes.Clear();
			this._animator = null;
			this._boobsController = null;
			this._bodyRenderer = null;
			this._currentBoobsType = -1;
			this._active = false;
			this._suspended = false;
		}

		// Token: 0x06008CAB RID: 36011 RVA: 0x003971B8 File Offset: 0x003953B8
		private void PurgeDestroyedOwners()
		{
			if (this._owners.Count == 0)
			{
				return;
			}
			List<int> list = null;
			foreach (KeyValuePair<int, Object> keyValuePair in this._owners)
			{
				if (!(keyValuePair.Value != null))
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(keyValuePair.Key);
				}
			}
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				this._owners.Remove(list[i]);
			}
		}

		// Token: 0x06008CAC RID: 36012 RVA: 0x00397264 File Offset: 0x00395464
		private int ResolveBodyPartLayer()
		{
			int num = LayerMask.NameToLayer(this.bodyPartLayer);
			if (num < 0)
			{
				return base.gameObject.layer;
			}
			return num;
		}

		// Token: 0x06008CAD RID: 36013 RVA: 0x00397290 File Offset: 0x00395490
		private static bool ConfigureCapsule(Transform proxy, Vector3 worldEnd, float radiusScale, out CapsuleCollider capsule)
		{
			capsule = null;
			Vector3 vector = proxy.InverseTransformPoint(worldEnd);
			int num = GachaCielCollisionRig.LargestComponent(vector);
			float num2 = vector[num];
			float num3 = Mathf.Abs(num2);
			if (num3 < 0.001f)
			{
				return false;
			}
			float num4 = Mathf.Max(0.025f, num3 * radiusScale);
			capsule = proxy.gameObject.AddComponent<CapsuleCollider>();
			capsule.direction = num;
			capsule.center = GachaCielCollisionRig.AxisVector(num, num2 * 0.5f);
			capsule.radius = num4;
			capsule.height = Mathf.Max(num3, num4 * 2f);
			return true;
		}

		// Token: 0x06008CAE RID: 36014 RVA: 0x00397320 File Offset: 0x00395520
		private static float BoneInfluence(BoneWeight weight, int boneIndex)
		{
			float num = 0f;
			if (weight.boneIndex0 == boneIndex)
			{
				num += weight.weight0;
			}
			if (weight.boneIndex1 == boneIndex)
			{
				num += weight.weight1;
			}
			if (weight.boneIndex2 == boneIndex)
			{
				num += weight.weight2;
			}
			if (weight.boneIndex3 == boneIndex)
			{
				num += weight.weight3;
			}
			return num;
		}

		// Token: 0x06008CAF RID: 36015 RVA: 0x00397384 File Offset: 0x00395584
		private static Transform FindDescendant(Transform root, string exactName)
		{
			if (root == null || string.IsNullOrEmpty(exactName))
			{
				return null;
			}
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (transform != null && string.Equals(transform.name, exactName, StringComparison.OrdinalIgnoreCase))
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06008CB0 RID: 36016 RVA: 0x003973D9 File Offset: 0x003955D9
		private static int RegionPriority(GachaCielBodyRegion region)
		{
			if (GachaCielCollisionRig.IsBreast(region))
			{
				return 100;
			}
			if (region == GachaCielBodyRegion.Head)
			{
				return 30;
			}
			if (region == GachaCielBodyRegion.LeftHand || region == GachaCielBodyRegion.RightHand || region == GachaCielBodyRegion.LeftFoot || region == GachaCielBodyRegion.RightFoot)
			{
				return 20;
			}
			return 10;
		}

		// Token: 0x06008CB1 RID: 36017 RVA: 0x00397408 File Offset: 0x00395608
		private static Bounds BoundsFromWorldPoints(Transform relativeTo, params Vector3[] points)
		{
			Bounds bounds = new Bounds(relativeTo.InverseTransformPoint(points[0]), Vector3.zero);
			for (int i = 1; i < points.Length; i++)
			{
				bounds.Encapsulate(relativeTo.InverseTransformPoint(points[i]));
			}
			return bounds;
		}

		// Token: 0x06008CB2 RID: 36018 RVA: 0x00397454 File Offset: 0x00395654
		private static void ExpandThinAxis(ref Bounds bounds, float fractionOfLargest, float minimum)
		{
			Vector3 size = bounds.size;
			int num = GachaCielCollisionRig.SmallestComponent(size);
			int num2 = GachaCielCollisionRig.LargestComponent(size);
			size[num] = Mathf.Max(size[num], Mathf.Max(size[num2] * fractionOfLargest, minimum));
			bounds.size = size;
		}

		// Token: 0x06008CB3 RID: 36019 RVA: 0x00119C8A File Offset: 0x00117E8A
		private static Vector3 MaxComponents(Vector3 value, Vector3 minimum)
		{
			return new Vector3(Mathf.Max(value.x, minimum.x), Mathf.Max(value.y, minimum.y), Mathf.Max(value.z, minimum.z));
		}

		// Token: 0x06008CB4 RID: 36020 RVA: 0x003974A4 File Offset: 0x003956A4
		private static Vector3 AxisVector(int axis, float value)
		{
			Vector3 zero = Vector3.zero;
			zero[axis] = value;
			return zero;
		}

		// Token: 0x06008CB5 RID: 36021 RVA: 0x003974C4 File Offset: 0x003956C4
		private static int LargestComponent(Vector3 value)
		{
			int num = 0;
			if (Mathf.Abs(value.y) > Mathf.Abs(value.x))
			{
				num = 1;
			}
			if (Mathf.Abs(value.z) > Mathf.Abs(value[num]))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06008CB6 RID: 36022 RVA: 0x0039750C File Offset: 0x0039570C
		private static int SmallestComponent(Vector3 value)
		{
			int num = 0;
			if (Mathf.Abs(value.y) < Mathf.Abs(value.x))
			{
				num = 1;
			}
			if (Mathf.Abs(value.z) < Mathf.Abs(value[num]))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06008CB7 RID: 36023 RVA: 0x00397552 File Offset: 0x00395752
		private static float MaximumAbsComponent(Vector3 value)
		{
			return Mathf.Max(new float[]
			{
				Mathf.Abs(value.x),
				Mathf.Abs(value.y),
				Mathf.Abs(value.z)
			});
		}

		// Token: 0x06008CB8 RID: 36024 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyProxy(GameObject proxy)
		{
			if (proxy == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(proxy);
				return;
			}
			Object.DestroyImmediate(proxy);
		}

		// Token: 0x06008CB9 RID: 36025 RVA: 0x0039758C File Offset: 0x0039578C
		private void OnDrawGizmosSelected()
		{
			if (!this.drawColliderBounds)
			{
				return;
			}
			Color color = Gizmos.color;
			for (int i = 0; i < this._colliders.Count; i++)
			{
				Collider collider = this._colliders[i];
				if (!(collider == null) && collider.enabled)
				{
					GachaCielBodyRegion gachaCielBodyRegion;
					Gizmos.color = ((this._regions.TryGetValue(collider, out gachaCielBodyRegion) && GachaCielCollisionRig.IsBreast(gachaCielBodyRegion)) ? new Color(1f, 0.25f, 0.65f, 0.9f) : new Color(0.2f, 0.85f, 1f, 0.65f));
					Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
				}
			}
			Gizmos.color = color;
		}

		// Token: 0x06008CBA RID: 36026 RVA: 0x00397660 File Offset: 0x00395860
		private void OnValidate()
		{
			this.defaultBreastRadius = Mathf.Max(0.01f, this.defaultBreastRadius);
			this.minimumBreastRadius = Mathf.Max(0.005f, this.minimumBreastRadius);
			this.maximumBreastRadius = Mathf.Max(this.minimumBreastRadius, this.maximumBreastRadius);
			this.maximumDepenetrationVelocity = Mathf.Max(0.1f, this.maximumDepenetrationVelocity);
			this.maximumFitDistanceInRadii = Mathf.Max(1f, this.maximumFitDistanceInRadii);
		}

		// Token: 0x04008F87 RID: 36743
		private const string LeftBreastBoneName = "c_breast_01.l";

		// Token: 0x04008F88 RID: 36744
		private const string RightBreastBoneName = "c_breast_01.r";

		// Token: 0x04008F89 RID: 36745
		private static readonly GachaCielBodyRegion[] BreastRegions = new GachaCielBodyRegion[]
		{
			GachaCielBodyRegion.LeftBreast,
			GachaCielBodyRegion.RightBreast
		};

		// Token: 0x04008F8A RID: 36746
		[Header("Physics")]
		[SerializeField]
		private string bodyPartLayer = "BodyPart";

		// Token: 0x04008F8B RID: 36747
		[SerializeField]
		private CollisionDetectionMode collisionDetection = CollisionDetectionMode.ContinuousSpeculative;

		// Token: 0x04008F8C RID: 36748
		[SerializeField]
		[Min(0.1f)]
		private float maximumDepenetrationVelocity = 6f;

		// Token: 0x04008F8D RID: 36749
		[Header("Breast volumes")]
		[Tooltip("World-space radius for the Default (BoobsType 3) body. Other sizes use the shared BreastInteractionSizing profile.")]
		[SerializeField]
		[Min(0.01f)]
		private float defaultBreastRadius = 0.075f;

		// Token: 0x04008F8E RID: 36750
		[Tooltip("The custom _01 bone sits at the nipple. Move the sphere centre behind it so the front of the collider follows the visible skin instead of protruding past it.")]
		[SerializeField]
		[Range(0.5f, 1.2f)]
		private float breastCenterBackFraction = 0.88f;

		// Token: 0x04008F8F RID: 36751
		[SerializeField]
		[Min(0.005f)]
		private float minimumBreastRadius = 0.034f;

		// Token: 0x04008F90 RID: 36752
		[SerializeField]
		[Min(0.02f)]
		private float maximumBreastRadius = 0.115f;

		// Token: 0x04008F91 RID: 36753
		[Tooltip("Use vertices influenced by c_breast_01 to refine the authored profile without allowing an unrelated torso vertex to inflate the collider.")]
		[SerializeField]
		private bool refineBreastsFromSkinnedMesh = true;

		// Token: 0x04008F92 RID: 36754
		[SerializeField]
		[Range(0.01f, 0.75f)]
		private float minimumBreastBoneWeight = 0.08f;

		// Token: 0x04008F93 RID: 36755
		[SerializeField]
		[Range(1f, 3f)]
		private float maximumFitDistanceInRadii = 2.35f;

		// Token: 0x04008F94 RID: 36756
		[SerializeField]
		[Range(0f, 0.03f)]
		private float fittedSurfacePadding = 0.004f;

		// Token: 0x04008F95 RID: 36757
		[Header("Coverage")]
		[SerializeField]
		private bool includeHands = true;

		// Token: 0x04008F96 RID: 36758
		[SerializeField]
		private bool includeFeet = true;

		// Token: 0x04008F97 RID: 36759
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008F98 RID: 36760
		[SerializeField]
		private bool drawColliderBounds;

		// Token: 0x04008F99 RID: 36761
		private readonly List<Collider> _colliders = new List<Collider>(17);

		// Token: 0x04008F9A RID: 36762
		private readonly List<Collider> _breastColliders = new List<Collider>(2);

		// Token: 0x04008F9B RID: 36763
		private readonly List<GameObject> _proxyObjects = new List<GameObject>(17);

		// Token: 0x04008F9C RID: 36764
		private readonly Dictionary<Collider, GachaCielBodyRegion> _regions = new Dictionary<Collider, GachaCielBodyRegion>(17);

		// Token: 0x04008F9D RID: 36765
		private readonly Dictionary<GachaCielBodyRegion, Transform> _regionBones = new Dictionary<GachaCielBodyRegion, Transform>(17);

		// Token: 0x04008F9E RID: 36766
		private readonly Dictionary<GachaCielBodyRegion, SphereCollider> _breastShapes = new Dictionary<GachaCielBodyRegion, SphereCollider>(2);

		// Token: 0x04008F9F RID: 36767
		private readonly Dictionary<int, Object> _owners = new Dictionary<int, Object>(2);

		// Token: 0x04008FA0 RID: 36768
		private Animator _animator;

		// Token: 0x04008FA1 RID: 36769
		private BoobsController _boobsController;

		// Token: 0x04008FA2 RID: 36770
		private SkinnedMeshRenderer _bodyRenderer;

		// Token: 0x04008FA3 RID: 36771
		private GachaMachineController _machineController;

		// Token: 0x04008FA4 RID: 36772
		private int _currentBoobsType = -1;

		// Token: 0x04008FA5 RID: 36773
		private bool _active;

		// Token: 0x04008FA6 RID: 36774
		private bool _suspended;
	}
}
