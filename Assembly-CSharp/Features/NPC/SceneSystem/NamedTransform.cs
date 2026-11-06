using System;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F4C RID: 3916
	[Serializable]
	public class NamedTransform
	{
		// Token: 0x04007836 RID: 30774
		[Tooltip("Identifier name for this point")]
		public string name;

		// Token: 0x04007837 RID: 30775
		[Tooltip("The Transform in the scene")]
		public Transform transform;
	}
}
