using System;

namespace TMPro
{
	// Token: 0x02000022 RID: 34
	public struct TMP_Offset
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000174C4 File Offset: 0x000156C4
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000174CC File Offset: 0x000156CC
		public float left
		{
			get
			{
				return this.m_Left;
			}
			set
			{
				this.m_Left = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000174D5 File Offset: 0x000156D5
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000174DD File Offset: 0x000156DD
		public float right
		{
			get
			{
				return this.m_Right;
			}
			set
			{
				this.m_Right = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000174E6 File Offset: 0x000156E6
		// (set) Token: 0x06000120 RID: 288 RVA: 0x000174EE File Offset: 0x000156EE
		public float top
		{
			get
			{
				return this.m_Top;
			}
			set
			{
				this.m_Top = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000174F7 File Offset: 0x000156F7
		// (set) Token: 0x06000122 RID: 290 RVA: 0x000174FF File Offset: 0x000156FF
		public float bottom
		{
			get
			{
				return this.m_Bottom;
			}
			set
			{
				this.m_Bottom = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00017508 File Offset: 0x00015708
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00017510 File Offset: 0x00015710
		public float horizontal
		{
			get
			{
				return this.m_Left;
			}
			set
			{
				this.m_Left = value;
				this.m_Right = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00017520 File Offset: 0x00015720
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00017528 File Offset: 0x00015728
		public float vertical
		{
			get
			{
				return this.m_Top;
			}
			set
			{
				this.m_Top = value;
				this.m_Bottom = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00017538 File Offset: 0x00015738
		public static TMP_Offset zero
		{
			get
			{
				return TMP_Offset.k_ZeroOffset;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0001753F File Offset: 0x0001573F
		public TMP_Offset(float left, float right, float top, float bottom)
		{
			this.m_Left = left;
			this.m_Right = right;
			this.m_Top = top;
			this.m_Bottom = bottom;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0001755E File Offset: 0x0001575E
		public TMP_Offset(float horizontal, float vertical)
		{
			this.m_Left = horizontal;
			this.m_Right = horizontal;
			this.m_Top = vertical;
			this.m_Bottom = vertical;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0001757C File Offset: 0x0001577C
		public static bool operator ==(TMP_Offset lhs, TMP_Offset rhs)
		{
			return lhs.m_Left == rhs.m_Left && lhs.m_Right == rhs.m_Right && lhs.m_Top == rhs.m_Top && lhs.m_Bottom == rhs.m_Bottom;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000175B8 File Offset: 0x000157B8
		public static bool operator !=(TMP_Offset lhs, TMP_Offset rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000175C4 File Offset: 0x000157C4
		public static TMP_Offset operator *(TMP_Offset a, float b)
		{
			return new TMP_Offset(a.m_Left * b, a.m_Right * b, a.m_Top * b, a.m_Bottom * b);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000175EB File Offset: 0x000157EB
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000175FD File Offset: 0x000157FD
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00017610 File Offset: 0x00015810
		public bool Equals(TMP_Offset other)
		{
			return base.Equals(other);
		}

		// Token: 0x04000111 RID: 273
		private float m_Left;

		// Token: 0x04000112 RID: 274
		private float m_Right;

		// Token: 0x04000113 RID: 275
		private float m_Top;

		// Token: 0x04000114 RID: 276
		private float m_Bottom;

		// Token: 0x04000115 RID: 277
		private static readonly TMP_Offset k_ZeroOffset = new TMP_Offset(0f, 0f, 0f, 0f);
	}
}
