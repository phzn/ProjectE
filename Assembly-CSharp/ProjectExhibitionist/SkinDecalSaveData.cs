using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x02000905 RID: 2309
	[Serializable]
	public class SkinDecalSaveData
	{
		// Token: 0x04003E91 RID: 16017
		public List<SkinDecalSaveData.Entry> entries = new List<SkinDecalSaveData.Entry>();

		// Token: 0x02000906 RID: 2310
		[Serializable]
		public class Entry
		{
			// Token: 0x04003E92 RID: 16018
			public string decalName;

			// Token: 0x04003E93 RID: 16019
			public Color tint;

			// Token: 0x04003E94 RID: 16020
			public float opacity;

			// Token: 0x04003E95 RID: 16021
			public bool isActive;
		}
	}
}
