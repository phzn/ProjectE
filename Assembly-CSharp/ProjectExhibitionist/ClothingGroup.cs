using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008D5 RID: 2261
	[Serializable]
	public class ClothingGroup
	{
		// Token: 0x04003CE2 RID: 15586
		[Tooltip("Name of this clothing group (e.g., 'Top', 'Bottom', 'Underwear')")]
		public string groupName;

		// Token: 0x04003CE3 RID: 15587
		[Tooltip("Is this group currently active/visible?")]
		public bool isActive = true;

		// Token: 0x04003CE4 RID: 15588
		[Tooltip("If true, this group stays visible even when switching to naked state.")]
		public bool keepActiveWhenNaked;

		// Token: 0x04003CE5 RID: 15589
		[Tooltip("GameObjects to toggle for this group")]
		public List<GameObject> outfitObjects = new List<GameObject>();

		// Token: 0x04003CE6 RID: 15590
		[Tooltip("Blendshapes to adjust when toggling this group")]
		public List<BlendshapeSetting> blendshapeSettings = new List<BlendshapeSetting>();
	}
}
