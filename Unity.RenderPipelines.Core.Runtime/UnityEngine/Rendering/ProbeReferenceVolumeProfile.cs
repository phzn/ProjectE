using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200008E RID: 142
	public sealed class ProbeReferenceVolumeProfile : ScriptableObject
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x000177A2 File Offset: 0x000159A2
		public int cellSizeInBricks
		{
			get
			{
				return (int)Mathf.Pow(3f, (float)this.simplificationLevels);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x000177B6 File Offset: 0x000159B6
		public int maxSubdivision
		{
			get
			{
				return this.simplificationLevels + 1;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x000177C0 File Offset: 0x000159C0
		public float minBrickSize
		{
			get
			{
				return Mathf.Max(0.01f, this.minDistanceBetweenProbes * 3f);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000177D8 File Offset: 0x000159D8
		public float cellSizeInMeters
		{
			get
			{
				return (float)this.cellSizeInBricks * this.minBrickSize;
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000177E8 File Offset: 0x000159E8
		private void OnEnable()
		{
			ProbeReferenceVolumeProfile.Version version = this.version;
			CoreUtils.GetLastEnumValue<ProbeReferenceVolumeProfile.Version>();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000177F8 File Offset: 0x000159F8
		public bool IsEquivalent(ProbeReferenceVolumeProfile otherProfile)
		{
			return this.minDistanceBetweenProbes == otherProfile.minDistanceBetweenProbes && this.cellSizeInMeters == otherProfile.cellSizeInMeters && this.simplificationLevels == otherProfile.simplificationLevels && this.renderersLayerMask == otherProfile.renderersLayerMask;
		}

		// Token: 0x04000302 RID: 770
		[SerializeField]
		private ProbeReferenceVolumeProfile.Version version = CoreUtils.GetLastEnumValue<ProbeReferenceVolumeProfile.Version>();

		// Token: 0x04000303 RID: 771
		[SerializeField]
		internal bool freezePlacement;

		// Token: 0x04000304 RID: 772
		[Range(2f, 5f)]
		public int simplificationLevels = 3;

		// Token: 0x04000305 RID: 773
		[Min(0.1f)]
		public float minDistanceBetweenProbes = 1f;

		// Token: 0x04000306 RID: 774
		public LayerMask renderersLayerMask = -1;

		// Token: 0x04000307 RID: 775
		[Min(0f)]
		public float minRendererVolumeSize = 0.1f;

		// Token: 0x020001A8 RID: 424
		internal enum Version
		{
			// Token: 0x04000703 RID: 1795
			Initial
		}
	}
}
