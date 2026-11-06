using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.SpriteAssetUtilities
{
	// Token: 0x02000075 RID: 117
	public class TexturePacker_JsonArray
	{
		// Token: 0x020000AA RID: 170
		[Serializable]
		public struct SpriteFrame
		{
			// Token: 0x06000652 RID: 1618 RVA: 0x00039240 File Offset: 0x00037440
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"x: ",
					this.x.ToString("f2"),
					" y: ",
					this.y.ToString("f2"),
					" h: ",
					this.h.ToString("f2"),
					" w: ",
					this.w.ToString("f2")
				});
			}

			// Token: 0x04000613 RID: 1555
			public float x;

			// Token: 0x04000614 RID: 1556
			public float y;

			// Token: 0x04000615 RID: 1557
			public float w;

			// Token: 0x04000616 RID: 1558
			public float h;
		}

		// Token: 0x020000AB RID: 171
		[Serializable]
		public struct SpriteSize
		{
			// Token: 0x06000653 RID: 1619 RVA: 0x000392C4 File Offset: 0x000374C4
			public override string ToString()
			{
				return "w: " + this.w.ToString("f2") + " h: " + this.h.ToString("f2");
			}

			// Token: 0x04000617 RID: 1559
			public float w;

			// Token: 0x04000618 RID: 1560
			public float h;
		}

		// Token: 0x020000AC RID: 172
		[Serializable]
		public struct Frame
		{
			// Token: 0x04000619 RID: 1561
			public string filename;

			// Token: 0x0400061A RID: 1562
			public TexturePacker_JsonArray.SpriteFrame frame;

			// Token: 0x0400061B RID: 1563
			public bool rotated;

			// Token: 0x0400061C RID: 1564
			public bool trimmed;

			// Token: 0x0400061D RID: 1565
			public TexturePacker_JsonArray.SpriteFrame spriteSourceSize;

			// Token: 0x0400061E RID: 1566
			public TexturePacker_JsonArray.SpriteSize sourceSize;

			// Token: 0x0400061F RID: 1567
			public Vector2 pivot;
		}

		// Token: 0x020000AD RID: 173
		[Serializable]
		public struct Meta
		{
			// Token: 0x04000620 RID: 1568
			public string app;

			// Token: 0x04000621 RID: 1569
			public string version;

			// Token: 0x04000622 RID: 1570
			public string image;

			// Token: 0x04000623 RID: 1571
			public string format;

			// Token: 0x04000624 RID: 1572
			public TexturePacker_JsonArray.SpriteSize size;

			// Token: 0x04000625 RID: 1573
			public float scale;

			// Token: 0x04000626 RID: 1574
			public string smartupdate;
		}

		// Token: 0x020000AE RID: 174
		[Serializable]
		public class SpriteDataObject
		{
			// Token: 0x04000627 RID: 1575
			public List<TexturePacker_JsonArray.Frame> frames;

			// Token: 0x04000628 RID: 1576
			public TexturePacker_JsonArray.Meta meta;
		}
	}
}
