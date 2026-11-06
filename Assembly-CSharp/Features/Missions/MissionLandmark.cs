using System;
using UnityEngine;

namespace Features.Missions
{
	// Token: 0x02000F82 RID: 3970
	[DisallowMultipleComponent]
	public sealed class MissionLandmark : MonoBehaviour
	{
		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x060077AF RID: 30639 RVA: 0x0030C1DF File Offset: 0x0030A3DF
		public string LandmarkId
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.landmarkId))
				{
					return this.landmarkId.Trim();
				}
				return base.gameObject.name.ToUpperInvariant().Replace(" ", "_");
			}
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x060077B0 RID: 30640 RVA: 0x0030C219 File Offset: 0x0030A419
		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.displayName))
				{
					return this.displayName.Trim();
				}
				return base.gameObject.name;
			}
		}

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x060077B1 RID: 30641 RVA: 0x0030C23F File Offset: 0x0030A43F
		public float ActivationRange
		{
			get
			{
				return Mathf.Max(0.5f, this.activationRange);
			}
		}

		// Token: 0x04007ADE RID: 31454
		[SerializeField]
		private string landmarkId = "LANDMARK";

		// Token: 0x04007ADF RID: 31455
		[SerializeField]
		private string displayName = "a landmark";

		// Token: 0x04007AE0 RID: 31456
		[SerializeField]
		[Min(0.5f)]
		private float activationRange = 4f;
	}
}
