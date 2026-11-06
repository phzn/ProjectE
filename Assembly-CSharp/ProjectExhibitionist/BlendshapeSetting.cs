using System;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008D6 RID: 2262
	[Serializable]
	public class BlendshapeSetting
	{
		// Token: 0x04003CE7 RID: 15591
		[Tooltip("The SkinnedMeshRenderer containing the blendshape")]
		public SkinnedMeshRenderer targetRenderer;

		// Token: 0x04003CE8 RID: 15592
		[Tooltip("Name of the blendshape")]
		public string blendshapeName;

		// Token: 0x04003CE9 RID: 15593
		[Tooltip("Blendshape value when clothed (0-100)")]
		[Range(0f, 100f)]
		public float clothedValue;

		// Token: 0x04003CEA RID: 15594
		[Tooltip("Blendshape value when naked (0-100)")]
		[Range(0f, 100f)]
		public float nakedValue = 100f;
	}
}
