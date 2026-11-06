using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x0200142E RID: 5166
	[CreateAssetMenu(fileName = "CielOSStoryBook", menuName = "CielOS/Story Book", order = 30)]
	public class CielOSStoryBookSO : ScriptableObject
	{
		// Token: 0x0400A382 RID: 41858
		[Tooltip("The notebook pages, in reading order.")]
		public List<CielOSStoryBookSO.Page> pages = new List<CielOSStoryBookSO.Page>();

		// Token: 0x0200142F RID: 5167
		[Serializable]
		public class Sticker
		{
			// Token: 0x0400A383 RID: 41859
			[Tooltip("The photo pasted on the page (phone-photo style: rounded corners + thin sticker edge). Any Sprite works.")]
			public Sprite sprite;

			// Token: 0x0400A384 RID: 41860
			[Tooltip("Center of the sticker in px from the page TOP-LEFT (x → right, y → down).")]
			public Vector2 position = new Vector2(560f, 300f);

			// Token: 0x0400A385 RID: 41861
			[Tooltip("Photo width in px (height follows the sprite's aspect ratio).")]
			public float width = 240f;

			// Token: 0x0400A386 RID: 41862
			[Tooltip("Tilt in degrees, positive = clockwise — a couple of degrees sells the 'pasted by hand' look.")]
			public float rotation = -4f;

			// Token: 0x0400A387 RID: 41863
			[Tooltip("Optional caption written over the bottom of the photo. Empty = none.")]
			public string caption = "";
		}

		// Token: 0x02001430 RID: 5168
		[Serializable]
		public class Page
		{
			// Token: 0x0400A388 RID: 41864
			[Tooltip("Page title, rendered as a markdown-style '# heading' in the accent colour.")]
			public string title = "";

			// Token: 0x0400A389 RID: 41865
			[TextArea(8, 40)]
			[Tooltip("Body text. TMP rich text allowed. Lines starting with '# ' render as mid-page headings styled like the page title (violet pen + underline). Leave blank lines / space where stickers go.")]
			public string body = "";

			// Token: 0x0400A38A RID: 41866
			[Tooltip("Images pasted on this page.")]
			public List<CielOSStoryBookSO.Sticker> stickers = new List<CielOSStoryBookSO.Sticker>();

			// Token: 0x0400A38B RID: 41867
			[Tooltip("Minimum sheet height in px (0 = auto from text + stickers).")]
			public float minHeight;
		}
	}
}
