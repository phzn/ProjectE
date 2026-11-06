using System;
using Invector.vCharacterController;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000775 RID: 1909
	[DefaultExecutionOrder(-500)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
	public sealed class CielRagdollBootstrap : MonoBehaviour
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x001190DB File Offset: 0x001172DB
		// (set) Token: 0x060031C8 RID: 12744 RVA: 0x001190E3 File Offset: 0x001172E3
		public bool IsReady { get; private set; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x001190EC File Offset: 0x001172EC
		public vRagdoll Ragdoll
		{
			get
			{
				return this.ragdoll;
			}
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x001190F4 File Offset: 0x001172F4
		private void Awake()
		{
			if (base.GetComponent<CustomizationPreviewPhysicsGuard>() != null)
			{
				base.enabled = false;
				return;
			}
			this.animator = base.GetComponent<Animator>();
			this.character = base.GetComponent<vThirdPersonController>();
			if (this.animator == null || !this.animator.isHuman)
			{
				Debug.LogError("[CielRagdollBootstrap] Ciel needs a valid humanoid Animator.", this);
				return;
			}
			if (this.character == null)
			{
				Debug.LogError("[CielRagdollBootstrap] vThirdPersonController was not found.", this);
				return;
			}
			CielRagdollBootstrap.HumanoidBones humanoidBones;
			if (!this.TryGetRequiredBones(out humanoidBones))
			{
				Debug.LogError("[CielRagdollBootstrap] The humanoid avatar is missing one or more bones required by the ragdoll.", this);
				return;
			}
			this.BuildOrRepairRagdoll(humanoidBones);
			this.ragdoll = base.GetComponent<vRagdoll>();
			if (this.ragdoll == null)
			{
				this.ragdoll = base.gameObject.AddComponent<vRagdoll>();
			}
			this.ragdoll.startRagdolled = false;
			this.ragdoll.keepRagdolled = false;
			this.ragdoll.ignoreGetUpAnimation = false;
			this.ragdoll.disableColliders = this.disableBodyCollidersWhileAnimated;
			this.ragdoll.horizontalMultiplier = this.inheritedHorizontalVelocity;
			this.ragdoll.verticalMultiplier = this.inheritedVerticalVelocity;
			this.ragdoll.groundLayer = this.character.groundLayer;
			this.IsReady = true;
			if (this.logSetup)
			{
				int num = humanoidBones.Hips.GetComponentsInChildren<Rigidbody>(true).Length;
				Debug.Log(string.Format("[CielRagdollBootstrap] Ragdoll ready with {0} physical bodies.", num), this);
			}
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x0011925C File Offset: 0x0011745C
		private bool TryGetRequiredBones(out CielRagdollBootstrap.HumanoidBones bones)
		{
			bones = new CielRagdollBootstrap.HumanoidBones
			{
				Hips = this.animator.GetBoneTransform(HumanBodyBones.Hips),
				Chest = this.animator.GetBoneTransform(HumanBodyBones.Chest),
				Head = this.animator.GetBoneTransform(HumanBodyBones.Head),
				LeftUpperLeg = this.animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg),
				LeftLowerLeg = this.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg),
				LeftFoot = this.animator.GetBoneTransform(HumanBodyBones.LeftFoot),
				RightUpperLeg = this.animator.GetBoneTransform(HumanBodyBones.RightUpperLeg),
				RightLowerLeg = this.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg),
				RightFoot = this.animator.GetBoneTransform(HumanBodyBones.RightFoot),
				LeftUpperArm = this.animator.GetBoneTransform(HumanBodyBones.LeftUpperArm),
				LeftLowerArm = this.animator.GetBoneTransform(HumanBodyBones.LeftLowerArm),
				LeftHand = this.animator.GetBoneTransform(HumanBodyBones.LeftHand),
				RightUpperArm = this.animator.GetBoneTransform(HumanBodyBones.RightUpperArm),
				RightLowerArm = this.animator.GetBoneTransform(HumanBodyBones.RightLowerArm),
				RightHand = this.animator.GetBoneTransform(HumanBodyBones.RightHand)
			};
			return bones.Hips != null && bones.Chest != null && bones.Head != null && bones.LeftUpperLeg != null && bones.LeftLowerLeg != null && bones.LeftFoot != null && bones.RightUpperLeg != null && bones.RightLowerLeg != null && bones.RightFoot != null && bones.LeftUpperArm != null && bones.LeftLowerArm != null && bones.LeftHand != null && bones.RightUpperArm != null && bones.RightLowerArm != null && bones.RightHand != null;
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x00119478 File Offset: 0x00117678
		private void BuildOrRepairRagdoll(CielRagdollBootstrap.HumanoidBones b)
		{
			int num = LayerMask.NameToLayer(this.bodyPartLayer);
			Rigidbody rigidbody = this.EnsureBody(b.Hips, 10f, num);
			Rigidbody rigidbody2 = this.EnsureBody(b.Chest, 14f, num);
			this.EnsureBody(b.Head, 5f, num);
			Rigidbody rigidbody3 = this.EnsureBody(b.LeftUpperLeg, 6f, num);
			this.EnsureBody(b.LeftLowerLeg, 4f, num);
			Rigidbody rigidbody4 = this.EnsureBody(b.RightUpperLeg, 6f, num);
			this.EnsureBody(b.RightLowerLeg, 4f, num);
			Rigidbody rigidbody5 = this.EnsureBody(b.LeftUpperArm, 2f, num);
			this.EnsureBody(b.LeftLowerArm, 1.5f, num);
			Rigidbody rigidbody6 = this.EnsureBody(b.RightUpperArm, 2f, num);
			this.EnsureBody(b.RightLowerArm, 1.5f, num);
			this.EnsureCapsule(b.LeftUpperLeg, b.LeftLowerLeg, 0.3f);
			this.EnsureCapsule(b.LeftLowerLeg, b.LeftFoot, 0.25f);
			this.EnsureCapsule(b.RightUpperLeg, b.RightLowerLeg, 0.3f);
			this.EnsureCapsule(b.RightLowerLeg, b.RightFoot, 0.25f);
			this.EnsureCapsule(b.LeftUpperArm, b.LeftLowerArm, 0.25f);
			this.EnsureCapsule(b.LeftLowerArm, b.LeftHand, 0.22f);
			this.EnsureCapsule(b.RightUpperArm, b.RightLowerArm, 0.25f);
			this.EnsureCapsule(b.RightLowerArm, b.RightHand, 0.22f);
			this.EnsurePelvisBox(b);
			this.EnsureChestBox(b);
			this.EnsureHeadSphere(b);
			Vector3 right = base.transform.right;
			Vector3 up = base.transform.up;
			Vector3 forward = base.transform.forward;
			CielRagdollBootstrap.EnsureJoint(b.LeftUpperLeg, rigidbody, right, forward, -25f, 65f, 30f);
			CielRagdollBootstrap.EnsureJoint(b.LeftLowerLeg, rigidbody3, right, forward, -85f, 5f, 8f);
			CielRagdollBootstrap.EnsureJoint(b.RightUpperLeg, rigidbody, right, forward, -25f, 65f, 30f);
			CielRagdollBootstrap.EnsureJoint(b.RightLowerLeg, rigidbody4, right, forward, -85f, 5f, 8f);
			CielRagdollBootstrap.EnsureJoint(b.Chest, rigidbody, right, forward, -25f, 25f, 18f);
			CielRagdollBootstrap.EnsureJoint(b.LeftUpperArm, rigidbody2, up, forward, -75f, 20f, 55f);
			CielRagdollBootstrap.EnsureJoint(b.LeftLowerArm, rigidbody5, forward, up, -100f, 5f, 8f);
			CielRagdollBootstrap.EnsureJoint(b.RightUpperArm, rigidbody2, up, forward, -75f, 20f, 55f);
			CielRagdollBootstrap.EnsureJoint(b.RightLowerArm, rigidbody6, forward, up, -100f, 5f, 8f);
			CielRagdollBootstrap.EnsureJoint(b.Head, rigidbody2, right, forward, -40f, 30f, 30f);
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x00119794 File Offset: 0x00117994
		private Rigidbody EnsureBody(Transform bone, float mass, int layer)
		{
			Rigidbody rigidbody = bone.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = bone.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.mass = mass;
			rigidbody.drag = this.bodyDrag;
			rigidbody.angularDrag = this.bodyAngularDrag;
			rigidbody.maxAngularVelocity = this.maxAngularVelocity;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = true;
			rigidbody.interpolation = RigidbodyInterpolation.None;
			rigidbody.collisionDetectionMode = this.collisionDetection;
			rigidbody.constraints = RigidbodyConstraints.None;
			if (layer >= 0)
			{
				bone.gameObject.layer = layer;
			}
			return rigidbody;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x00119824 File Offset: 0x00117A24
		private void EnsureCapsule(Transform bone, Transform endBone, float radiusScale)
		{
			CapsuleCollider capsuleCollider = bone.GetComponent<CapsuleCollider>();
			if (capsuleCollider == null)
			{
				capsuleCollider = bone.gameObject.AddComponent<CapsuleCollider>();
			}
			Vector3 vector = bone.InverseTransformPoint(endBone.position);
			int num = CielRagdollBootstrap.LargestComponent(vector);
			float num2 = vector[num];
			float num3 = Mathf.Abs(num2);
			float num4 = Mathf.Max(0.025f, num3 * radiusScale);
			Vector3 zero = Vector3.zero;
			zero[num] = num2 * 0.5f;
			capsuleCollider.direction = num;
			capsuleCollider.center = zero;
			capsuleCollider.radius = num4;
			capsuleCollider.height = Mathf.Max(num3, num4 * 2f);
			capsuleCollider.isTrigger = false;
			capsuleCollider.enabled = false;
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x001198D4 File Offset: 0x00117AD4
		private void EnsurePelvisBox(CielRagdollBootstrap.HumanoidBones b)
		{
			BoxCollider boxCollider = b.Hips.GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = b.Hips.gameObject.AddComponent<BoxCollider>();
			}
			Vector3 vector = Vector3.Lerp(b.Hips.position, b.Chest.position, 0.48f);
			Bounds bounds = CielRagdollBootstrap.BoundsFromWorldPoints(b.Hips, new Vector3[]
			{
				b.LeftUpperLeg.position,
				b.RightUpperLeg.position,
				vector
			});
			CielRagdollBootstrap.ExpandThinAxis(ref bounds, 0.55f, 0.12f);
			boxCollider.center = bounds.center;
			boxCollider.size = CielRagdollBootstrap.MaxComponents(bounds.size, new Vector3(0.16f, 0.14f, 0.12f));
			boxCollider.isTrigger = false;
			boxCollider.enabled = false;
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x001199B8 File Offset: 0x00117BB8
		private void EnsureChestBox(CielRagdollBootstrap.HumanoidBones b)
		{
			BoxCollider boxCollider = b.Chest.GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = b.Chest.gameObject.AddComponent<BoxCollider>();
			}
			Vector3 vector = Vector3.Lerp(b.Hips.position, b.Chest.position, 0.52f);
			Vector3 vector2 = Vector3.Lerp(b.Chest.position, b.Head.position, 0.48f);
			Bounds bounds = CielRagdollBootstrap.BoundsFromWorldPoints(b.Chest, new Vector3[]
			{
				vector,
				vector2,
				b.LeftUpperArm.position,
				b.RightUpperArm.position
			});
			CielRagdollBootstrap.ExpandThinAxis(ref bounds, 0.45f, 0.14f);
			boxCollider.center = bounds.center;
			boxCollider.size = CielRagdollBootstrap.MaxComponents(bounds.size, new Vector3(0.18f, 0.18f, 0.14f));
			boxCollider.isTrigger = false;
			boxCollider.enabled = false;
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x00119AC8 File Offset: 0x00117CC8
		private void EnsureHeadSphere(CielRagdollBootstrap.HumanoidBones b)
		{
			SphereCollider sphereCollider = b.Head.GetComponent<SphereCollider>();
			if (sphereCollider == null)
			{
				sphereCollider = b.Head.gameObject.AddComponent<SphereCollider>();
			}
			float num = Mathf.Clamp(Vector3.Distance(b.LeftUpperArm.position, b.RightUpperArm.position) * 0.22f, 0.09f, 0.18f);
			float num2 = Mathf.Max(new float[]
			{
				Mathf.Abs(b.Head.lossyScale.x),
				Mathf.Abs(b.Head.lossyScale.y),
				Mathf.Abs(b.Head.lossyScale.z)
			});
			sphereCollider.radius = num / Mathf.Max(num2, 0.0001f);
			Vector3 vector = b.Head.InverseTransformDirection((b.Head.position - b.Chest.position).normalized);
			sphereCollider.center = vector * sphereCollider.radius * 0.35f;
			sphereCollider.isTrigger = false;
			sphereCollider.enabled = false;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x00119BF0 File Offset: 0x00117DF0
		private static Bounds BoundsFromWorldPoints(Transform relativeTo, params Vector3[] points)
		{
			Bounds bounds = new Bounds(relativeTo.InverseTransformPoint(points[0]), Vector3.zero);
			for (int i = 1; i < points.Length; i++)
			{
				bounds.Encapsulate(relativeTo.InverseTransformPoint(points[i]));
			}
			return bounds;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x00119C3C File Offset: 0x00117E3C
		private static void ExpandThinAxis(ref Bounds bounds, float fractionOfLargest, float minimum)
		{
			Vector3 size = bounds.size;
			int num = CielRagdollBootstrap.SmallestComponent(size);
			int num2 = CielRagdollBootstrap.LargestComponent(size);
			size[num] = Mathf.Max(size[num], Mathf.Max(size[num2] * fractionOfLargest, minimum));
			bounds.size = size;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x00119C8A File Offset: 0x00117E8A
		private static Vector3 MaxComponents(Vector3 value, Vector3 minimum)
		{
			return new Vector3(Mathf.Max(value.x, minimum.x), Mathf.Max(value.y, minimum.y), Mathf.Max(value.z, minimum.z));
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x00119CC4 File Offset: 0x00117EC4
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

		// Token: 0x060031D6 RID: 12758 RVA: 0x00119D0C File Offset: 0x00117F0C
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

		// Token: 0x060031D7 RID: 12759 RVA: 0x00119D54 File Offset: 0x00117F54
		private static Vector3 DominantAxis(Vector3 value)
		{
			int num = CielRagdollBootstrap.LargestComponent(value);
			Vector3 zero = Vector3.zero;
			zero[num] = ((value[num] >= 0f) ? 1f : (-1f));
			return zero;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x00119D92 File Offset: 0x00117F92
		private static void SetLimit(ref SoftJointLimit limit, float value)
		{
			limit.limit = value;
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x00119D9C File Offset: 0x00117F9C
		private static void EnsureJoint(Transform bone, Rigidbody connectedBody, Vector3 worldTwistAxis, Vector3 worldSwingAxis, float lowTwist, float highTwist, float swing)
		{
			CharacterJoint characterJoint = bone.GetComponent<CharacterJoint>();
			if (characterJoint == null)
			{
				characterJoint = bone.gameObject.AddComponent<CharacterJoint>();
			}
			characterJoint.connectedBody = connectedBody;
			characterJoint.anchor = Vector3.zero;
			characterJoint.axis = CielRagdollBootstrap.DominantAxis(bone.InverseTransformDirection(worldTwistAxis));
			characterJoint.swingAxis = CielRagdollBootstrap.DominantAxis(bone.InverseTransformDirection(worldSwingAxis));
			characterJoint.enableProjection = true;
			characterJoint.enableCollision = false;
			SoftJointLimit softJointLimit = characterJoint.lowTwistLimit;
			CielRagdollBootstrap.SetLimit(ref softJointLimit, lowTwist);
			characterJoint.lowTwistLimit = softJointLimit;
			softJointLimit = characterJoint.highTwistLimit;
			CielRagdollBootstrap.SetLimit(ref softJointLimit, highTwist);
			characterJoint.highTwistLimit = softJointLimit;
			softJointLimit = characterJoint.swing1Limit;
			CielRagdollBootstrap.SetLimit(ref softJointLimit, swing);
			characterJoint.swing1Limit = softJointLimit;
			softJointLimit = characterJoint.swing2Limit;
			CielRagdollBootstrap.SetLimit(ref softJointLimit, swing * 0.65f);
			characterJoint.swing2Limit = softJointLimit;
		}

		// Token: 0x04003264 RID: 12900
		[Header("Physics")]
		[SerializeField]
		private string bodyPartLayer = "BodyPart";

		// Token: 0x04003265 RID: 12901
		[SerializeField]
		private CollisionDetectionMode collisionDetection = CollisionDetectionMode.ContinuousSpeculative;

		// Token: 0x04003266 RID: 12902
		[SerializeField]
		[Min(0f)]
		private float bodyDrag = 0.02f;

		// Token: 0x04003267 RID: 12903
		[SerializeField]
		[Min(0f)]
		private float bodyAngularDrag = 0.12f;

		// Token: 0x04003268 RID: 12904
		[SerializeField]
		[Min(1f)]
		private float maxAngularVelocity = 25f;

		// Token: 0x04003269 RID: 12905
		[Header("Invector get-up")]
		[SerializeField]
		private bool disableBodyCollidersWhileAnimated = true;

		// Token: 0x0400326A RID: 12906
		[SerializeField]
		[Range(0f, 2f)]
		private float inheritedHorizontalVelocity = 1f;

		// Token: 0x0400326B RID: 12907
		[SerializeField]
		[Range(0f, 2f)]
		private float inheritedVerticalVelocity = 1f;

		// Token: 0x0400326C RID: 12908
		[Header("Debug")]
		[SerializeField]
		private bool logSetup;

		// Token: 0x0400326D RID: 12909
		private Animator animator;

		// Token: 0x0400326E RID: 12910
		private vThirdPersonController character;

		// Token: 0x0400326F RID: 12911
		private vRagdoll ragdoll;

		// Token: 0x02000776 RID: 1910
		private sealed class HumanoidBones
		{
			// Token: 0x04003271 RID: 12913
			public Transform Hips;

			// Token: 0x04003272 RID: 12914
			public Transform Chest;

			// Token: 0x04003273 RID: 12915
			public Transform Head;

			// Token: 0x04003274 RID: 12916
			public Transform LeftUpperLeg;

			// Token: 0x04003275 RID: 12917
			public Transform LeftLowerLeg;

			// Token: 0x04003276 RID: 12918
			public Transform LeftFoot;

			// Token: 0x04003277 RID: 12919
			public Transform RightUpperLeg;

			// Token: 0x04003278 RID: 12920
			public Transform RightLowerLeg;

			// Token: 0x04003279 RID: 12921
			public Transform RightFoot;

			// Token: 0x0400327A RID: 12922
			public Transform LeftUpperArm;

			// Token: 0x0400327B RID: 12923
			public Transform LeftLowerArm;

			// Token: 0x0400327C RID: 12924
			public Transform LeftHand;

			// Token: 0x0400327D RID: 12925
			public Transform RightUpperArm;

			// Token: 0x0400327E RID: 12926
			public Transform RightLowerArm;

			// Token: 0x0400327F RID: 12927
			public Transform RightHand;
		}
	}
}
