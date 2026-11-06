using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x0200020F RID: 527
	[CreateAssetMenu(fileName = "New Icon Library", menuName = "Modern UI Pack/New Icon Library")]
	public class IconLibrary : ScriptableObject
	{
		// Token: 0x04000F7A RID: 3962
		public bool alwaysUpdate;

		// Token: 0x04000F7B RID: 3963
		public bool optimizeUpdates = true;

		// Token: 0x04000F7C RID: 3964
		public Texture2D searchIcon;

		// Token: 0x04000F7D RID: 3965
		public List<IconLibrary.IconItem> icons = new List<IconLibrary.IconItem>();

		// Token: 0x02000210 RID: 528
		[Serializable]
		public class IconItem
		{
			// Token: 0x04000F7E RID: 3966
			public string iconTitle = "Icon";

			// Token: 0x04000F7F RID: 3967
			public Texture2D iconPreview;

			// Token: 0x04000F80 RID: 3968
			public Sprite iconSprite32;

			// Token: 0x04000F81 RID: 3969
			public Sprite iconSprite64;

			// Token: 0x04000F82 RID: 3970
			public Sprite iconSprite128;

			// Token: 0x04000F83 RID: 3971
			public Sprite iconSprite256;
		}
	}
}
