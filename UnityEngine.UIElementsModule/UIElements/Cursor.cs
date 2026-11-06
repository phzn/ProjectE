using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000153 RID: 339
	public struct Cursor : IEquatable<Cursor>
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0002C42F File Offset: 0x0002A62F
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x0002C437 File Offset: 0x0002A637
		public Texture2D texture { readonly get; set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0002C440 File Offset: 0x0002A640
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x0002C448 File Offset: 0x0002A648
		public Vector2 hotspot { readonly get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0002C451 File Offset: 0x0002A651
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x0002C459 File Offset: 0x0002A659
		internal int defaultCursorId { readonly get; set; }

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002C464 File Offset: 0x0002A664
		public override bool Equals(object obj)
		{
			return obj is Cursor && this.Equals((Cursor)obj);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002C490 File Offset: 0x0002A690
		public bool Equals(Cursor other)
		{
			return EqualityComparer<Texture2D>.Default.Equals(this.texture, other.texture) && this.hotspot.Equals(other.hotspot) && this.defaultCursorId == other.defaultCursorId;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002C4E4 File Offset: 0x0002A6E4
		public override int GetHashCode()
		{
			int num = 1500536833;
			num = num * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(this.texture);
			num = num * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(this.hotspot);
			return num * -1521134295 + this.defaultCursorId.GetHashCode();
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0002C548 File Offset: 0x0002A748
		internal static IEnumerable<Type> allowedAssetTypes
		{
			get
			{
				yield return typeof(Texture2D);
				yield break;
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002C560 File Offset: 0x0002A760
		public static bool operator ==(Cursor style1, Cursor style2)
		{
			return style1.Equals(style2);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002C57C File Offset: 0x0002A77C
		public static bool operator !=(Cursor style1, Cursor style2)
		{
			return !(style1 == style2);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002C598 File Offset: 0x0002A798
		public override string ToString()
		{
			return string.Format("texture={0}, hotspot={1}", this.texture, this.hotspot);
		}
	}
}
