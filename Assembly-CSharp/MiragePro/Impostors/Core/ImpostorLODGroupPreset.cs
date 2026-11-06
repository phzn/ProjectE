using System;
using UnityEngine;

namespace MiragePro.Impostors.Core
{
	// Token: 0x02000276 RID: 630
	public class ImpostorLODGroupPreset : ScriptableObject
	{
		// Token: 0x040011F6 RID: 4598
		public bool setupLOD = true;

		// Token: 0x040011F7 RID: 4599
		public float lodPerformance = 0.15f;

		// Token: 0x040011F8 RID: 4600
		public float lodSizeCulling = 0.01f;
	}
}
