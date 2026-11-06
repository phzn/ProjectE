using System;
using System.Collections.Generic;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000922 RID: 2338
	[Serializable]
	public class SubwayDoorway
	{
		// Token: 0x04003F3A RID: 16186
		public string key;

		// Token: 0x04003F3B RID: 16187
		public List<SubwayDoorPanel> panels = new List<SubwayDoorPanel>();

		// Token: 0x04003F3C RID: 16188
		public Collider zoneCollider;

		// Token: 0x04003F3D RID: 16189
		public InteractablePromptUI promptUI;

		// Token: 0x04003F3E RID: 16190
		[NonSerialized]
		public bool PlayerInRange;

		// Token: 0x04003F3F RID: 16191
		[NonSerialized]
		public bool PromptShown;

		// Token: 0x04003F40 RID: 16192
		[NonSerialized]
		public bool PromptShownOpen;
	}
}
