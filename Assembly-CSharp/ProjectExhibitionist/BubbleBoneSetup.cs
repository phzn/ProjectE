using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008E8 RID: 2280
	[CreateAssetMenu(fileName = "BubbleBoneSetup", menuName = "Shower/Bubble Bone Setup")]
	public class BubbleBoneSetup : ScriptableObject
	{
		// Token: 0x04003DA4 RID: 15780
		[Tooltip("Path of the skeleton root relative to the character root that owns this setup. Bone paths above are resolved from here.")]
		public string skeletonRootPath = "3D Model/Ciel_07_05_26_ass/Ciel/root.x";

		// Token: 0x04003DA5 RID: 15781
		public List<BubbleBoneSetup.BubblePlacement> placements = new List<BubbleBoneSetup.BubblePlacement>();

		// Token: 0x020008E9 RID: 2281
		[Serializable]
		public class BubblePlacement
		{
			// Token: 0x04003DA6 RID: 15782
			[Tooltip("Bone path relative to the skeleton root (e.g. spine_01.x/.../c_breast_01.l).")]
			public string bonePath;

			// Token: 0x04003DA7 RID: 15783
			[Tooltip("Prefab to instantiate for this bubble.")]
			public GameObject prefab;

			// Token: 0x04003DA8 RID: 15784
			[Tooltip("Original GameObject name in the scene (for reference / debugging).")]
			public string sourceName;

			// Token: 0x04003DA9 RID: 15785
			public Vector3 localPosition;

			// Token: 0x04003DAA RID: 15786
			public Vector3 localEulerAngles;

			// Token: 0x04003DAB RID: 15787
			public Vector3 localScale = Vector3.one;
		}
	}
}
