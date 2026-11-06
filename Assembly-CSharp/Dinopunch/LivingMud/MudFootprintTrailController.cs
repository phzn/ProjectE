using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000743 RID: 1859
	public class MudFootprintTrailController : MonoBehaviour
	{
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x001102FB File Offset: 0x0010E4FB
		public float MudResidue
		{
			get
			{
				return this.mudResidue;
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x00110303 File Offset: 0x0010E503
		private void Awake()
		{
			if (this.footMudController == null)
			{
				this.footMudController = base.GetComponent<FootMudController>();
			}
			this.previousPosition = base.transform.position;
			this.lastSpawnPosition = base.transform.position;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00110344 File Offset: 0x0010E544
		private void Update()
		{
			this.UpdateResidue(Time.deltaTime);
			Vector3 position = base.transform.position;
			float num = (position - this.previousPosition).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
			this.previousPosition = position;
			if (this.mudResidue < this.spawnResidueThreshold)
			{
				return;
			}
			if (this.footMudController != null && this.footMudController.IsInMud && this.footMudController.CurrentSurface != null && (this.footMudController.CurrentSurface.Thickness & this.suppressFootprintTypes) != (MudType)0)
			{
				return;
			}
			if (num < this.minMovementSpeed)
			{
				return;
			}
			if (Vector3.Distance(position, this.lastSpawnPosition) < this.minStepDistance)
			{
				return;
			}
			Transform transform = (this.useLeftFootNext ? this.leftFootBone : this.rightFootBone);
			if (this.TrySpawnFootprint(transform, num))
			{
				this.lastSpawnPosition = position;
			}
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x00110432 File Offset: 0x0010E632
		public void AddResidue(float amount)
		{
			this.mudResidue = Mathf.Clamp01(this.mudResidue + amount);
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x00110447 File Offset: 0x0010E647
		public void ClearResidueImmediate()
		{
			this.mudResidue = 0f;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x00110454 File Offset: 0x0010E654
		private void UpdateResidue(float dt)
		{
			if (this.footMudController == null)
			{
				return;
			}
			if (!this.footMudController.IsInMud)
			{
				this.mudResidue = Mathf.Max(0f, this.mudResidue - this.residueDecayPerSecond * dt);
				return;
			}
			if ((this.footMudController.CurrentMudType & MudType.Watery) > (MudType)0)
			{
				this.mudResidue = Mathf.Max(0f, this.mudResidue - this.residueDecayInWaterPerSecond * dt);
				return;
			}
			this.mudResidue = Mathf.Clamp01(this.mudResidue + this.residueGainPerSecond * dt);
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x001104EC File Offset: 0x0010E6EC
		private bool TrySpawnFootprint(Transform footBone, float speed)
		{
			if (footBone == null || this.decalPoolService == null)
			{
				return false;
			}
			Vector3 vector = footBone.position + Vector3.up * this.raycastStartHeight;
			RaycastHit raycastHit;
			if (!Physics.Raycast(vector, Vector3.down, out raycastHit, this.raycastDistance, this.walkableMask, QueryTriggerInteraction.Ignore))
			{
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[MudFootprintTrail] Raycast miss from {0} at {1}", footBone.name, vector));
				}
				return false;
			}
			int num = LayerMask.NameToLayer("TransparentFX");
			if (num >= 0 && raycastHit.collider.gameObject.layer == num)
			{
				return false;
			}
			DynamicMudSurface componentInParent = raycastHit.collider.GetComponentInParent<DynamicMudSurface>();
			if (componentInParent != null && (componentInParent.Thickness & this.suppressFootprintTypes) != (MudType)0)
			{
				return false;
			}
			float num2 = Mathf.Clamp01(speed / 3f);
			float num3 = this.footprintSize + Random.Range(-this.sizeJitter, this.sizeJitter);
			float num4 = Mathf.Clamp01(this.baseOpacity * Mathf.Lerp(0.4f, 1f, this.mudResidue) * Mathf.Lerp(0.5f, 1f, num2));
			float num5 = Mathf.Lerp(this.decalLifetime * 0.6f, this.decalLifetime, this.mudResidue);
			float num6 = Mathf.Min(this.decalFadeDuration, num5);
			Vector3 vector2 = base.transform.forward;
			if (this.rotationJitter > 0f)
			{
				vector2 = Quaternion.AngleAxis(Random.Range(-this.rotationJitter, this.rotationJitter), raycastHit.normal) * vector2;
			}
			this.decalPoolService.Spawn(raycastHit.point, raycastHit.normal, vector2, num3, num5, num6, num4, this.useLeftFootNext);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[MudFootprintTrail] Spawned {0} at {1}, residue={2:F2}", this.useLeftFootNext ? "LEFT" : "RIGHT", raycastHit.point, this.mudResidue));
			}
			this.useLeftFootNext = !this.useLeftFootNext;
			return true;
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00110708 File Offset: 0x0010E908
		private void OnValidate()
		{
			this.minStepDistance = Mathf.Max(0.05f, this.minStepDistance);
			this.raycastDistance = Mathf.Max(0.1f, this.raycastDistance);
			this.minMovementSpeed = Mathf.Max(0f, this.minMovementSpeed);
			this.footprintSize = Mathf.Max(0.01f, this.footprintSize);
			this.sizeJitter = Mathf.Clamp(this.sizeJitter, 0f, this.footprintSize * 0.8f);
			this.decalLifetime = Mathf.Max(0.1f, this.decalLifetime);
			this.decalFadeDuration = Mathf.Max(0f, this.decalFadeDuration);
			this.baseOpacity = Mathf.Clamp01(this.baseOpacity);
			this.spawnResidueThreshold = Mathf.Clamp01(this.spawnResidueThreshold);
			this.rotationJitter = Mathf.Clamp(this.rotationJitter, 0f, 45f);
		}

		// Token: 0x040030A0 RID: 12448
		[Header("References")]
		[SerializeField]
		private FootMudController footMudController;

		// Token: 0x040030A1 RID: 12449
		[SerializeField]
		private MudFootprintDecalPoolService decalPoolService;

		// Token: 0x040030A2 RID: 12450
		[Header("Foot Bones")]
		[Tooltip("Left foot bone transform (e.g. Humanoid LeftFoot). Raycast origin.")]
		[SerializeField]
		private Transform leftFootBone;

		// Token: 0x040030A3 RID: 12451
		[Tooltip("Right foot bone transform (e.g. Humanoid RightFoot). Raycast origin.")]
		[SerializeField]
		private Transform rightFootBone;

		// Token: 0x040030A4 RID: 12452
		[Header("Surface Detection")]
		[SerializeField]
		private LayerMask walkableMask = -1;

		// Token: 0x040030A5 RID: 12453
		[Tooltip("Raycast starts this far above the foot bone position")]
		[SerializeField]
		private float raycastStartHeight = 0.15f;

		// Token: 0x040030A6 RID: 12454
		[Tooltip("Total raycast distance downward")]
		[SerializeField]
		private float raycastDistance = 0.6f;

		// Token: 0x040030A7 RID: 12455
		[Header("Residue")]
		[SerializeField]
		[Range(0f, 1f)]
		private float mudResidue;

		// Token: 0x040030A8 RID: 12456
		[SerializeField]
		private float residueGainPerSecond = 0.75f;

		// Token: 0x040030A9 RID: 12457
		[SerializeField]
		private float residueDecayPerSecond = 0.08f;

		// Token: 0x040030AA RID: 12458
		[SerializeField]
		private float residueDecayInWaterPerSecond = 0.35f;

		// Token: 0x040030AB RID: 12459
		[SerializeField]
		[Range(0f, 1f)]
		private float spawnResidueThreshold = 0.08f;

		// Token: 0x040030AC RID: 12460
		[Tooltip("Mud types that should NOT spawn footprints (e.g. Watery for water surfaces)")]
		[SerializeField]
		private MudType suppressFootprintTypes = MudType.Watery;

		// Token: 0x040030AD RID: 12461
		[Header("Footstep Spawn")]
		[Tooltip("Minimum distance the character must travel between spawns")]
		[SerializeField]
		private float minStepDistance = 0.35f;

		// Token: 0x040030AE RID: 12462
		[SerializeField]
		private float minMovementSpeed = 0.15f;

		// Token: 0x040030AF RID: 12463
		[SerializeField]
		private float footprintSize = 0.22f;

		// Token: 0x040030B0 RID: 12464
		[SerializeField]
		private float sizeJitter = 0.03f;

		// Token: 0x040030B1 RID: 12465
		[Tooltip("Random rotation jitter in degrees applied to each footprint")]
		[SerializeField]
		private float rotationJitter = 8f;

		// Token: 0x040030B2 RID: 12466
		[Header("Lifetime")]
		[SerializeField]
		private float decalLifetime = 14f;

		// Token: 0x040030B3 RID: 12467
		[SerializeField]
		private float decalFadeDuration = 4f;

		// Token: 0x040030B4 RID: 12468
		[SerializeField]
		private float baseOpacity = 0.9f;

		// Token: 0x040030B5 RID: 12469
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x040030B6 RID: 12470
		private bool useLeftFootNext = true;

		// Token: 0x040030B7 RID: 12471
		private Vector3 previousPosition;

		// Token: 0x040030B8 RID: 12472
		private Vector3 lastSpawnPosition;
	}
}
