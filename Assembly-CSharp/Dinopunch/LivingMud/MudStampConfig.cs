using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000746 RID: 1862
	[RequireComponent(typeof(MudStamp))]
	public class MudStampConfig : MonoBehaviour
	{
		// Token: 0x0600303F RID: 12351 RVA: 0x001117F1 File Offset: 0x0010F9F1
		private void Start()
		{
			this.ApplyStampSize();
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x001117F1 File Offset: 0x0010F9F1
		private void OnValidate()
		{
			this.ApplyStampSize();
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x001117FC File Offset: 0x0010F9FC
		private void FixedUpdate()
		{
			if (this.trackBoneTransform != null)
			{
				base.transform.position = this.trackBoneTransform.TransformPoint(this.boneOffset);
				if (this.trackBoneRotation)
				{
					Vector3 forward = this.trackBoneTransform.forward;
					forward.y = 0f;
					if (forward.sqrMagnitude > 0.001f)
					{
						base.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
					}
				}
			}
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x00111878 File Offset: 0x0010FA78
		private void ApplyStampSize()
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x = this.stampSize;
			localScale.y = this.stampSize;
			localScale.z = this.stampSize;
			base.transform.localScale = localScale;
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x001118C4 File Offset: 0x0010FAC4
		public void SetStampSize(float size)
		{
			this.stampSize = size;
			this.ApplyStampSize();
		}

		// Token: 0x040030D6 RID: 12502
		[Header("Size")]
		[Tooltip("Override stamp size. MudStamp reads transform.localScale.x for scale.\nThis value sets localScale.x at startup. 1.0 = 1 unit footprint (default for 10-unit surface).")]
		[SerializeField]
		private float stampSize = 0.3f;

		// Token: 0x040030D7 RID: 12503
		[Header("Position Tracking")]
		[Tooltip("If assigned, stamp position will track this bone transform each FixedUpdate.\nUse this when the MudStamp GO is NOT a direct child of the foot bone.")]
		[SerializeField]
		private Transform trackBoneTransform;

		// Token: 0x040030D8 RID: 12504
		[Tooltip("Local offset from the tracked bone (e.g. to position at sole of foot)")]
		[SerializeField]
		private Vector3 boneOffset = Vector3.zero;

		// Token: 0x040030D9 RID: 12505
		[Tooltip("If true, the stamp Y rotation follows the bone forward direction projected onto the mud surface plane")]
		[SerializeField]
		private bool trackBoneRotation = true;
	}
}
