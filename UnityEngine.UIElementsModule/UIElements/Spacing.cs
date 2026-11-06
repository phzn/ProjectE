using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002BF RID: 703
	internal struct Spacing
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0004883C File Offset: 0x00046A3C
		public float horizontal
		{
			get
			{
				return this.left + this.right;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x0004885C File Offset: 0x00046A5C
		public float vertical
		{
			get
			{
				return this.top + this.bottom;
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004887B File Offset: 0x00046A7B
		public Spacing(float left, float top, float right, float bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004889C File Offset: 0x00046A9C
		public static Rect operator +(Rect r, Spacing a)
		{
			r.x -= a.left;
			r.y -= a.top;
			r.width += a.horizontal;
			r.height += a.vertical;
			return r;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00048908 File Offset: 0x00046B08
		public static Rect operator -(Rect r, Spacing a)
		{
			r.x += a.left;
			r.y += a.top;
			r.width = Mathf.Max(0f, r.width - a.horizontal);
			r.height = Mathf.Max(0f, r.height - a.vertical);
			return r;
		}

		// Token: 0x0400097A RID: 2426
		public float left;

		// Token: 0x0400097B RID: 2427
		public float top;

		// Token: 0x0400097C RID: 2428
		public float right;

		// Token: 0x0400097D RID: 2429
		public float bottom;
	}
}
