using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000037 RID: 55
	internal struct Offset
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0001D844 File Offset: 0x0001BA44
		// (set) Token: 0x0600016D RID: 365 RVA: 0x0001D85C File Offset: 0x0001BA5C
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0001D868 File Offset: 0x0001BA68
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0001D880 File Offset: 0x0001BA80
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0001D88C File Offset: 0x0001BA8C
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0001D8A4 File Offset: 0x0001BAA4
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0001D8D4 File Offset: 0x0001BAD4
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0001D8EC File Offset: 0x0001BAEC
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0001D900 File Offset: 0x0001BB00
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0001D918 File Offset: 0x0001BB18
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0001D92C File Offset: 0x0001BB2C
		public static Offset zero
		{
			get
			{
				return Offset.k_ZeroOffset;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0001D943 File Offset: 0x0001BB43
		public Offset(float left, float right, float top, float bottom)
		{
			this.m_Left = left;
			this.m_Right = right;
			this.m_Top = top;
			this.m_Bottom = bottom;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0001D963 File Offset: 0x0001BB63
		public Offset(float horizontal, float vertical)
		{
			this.m_Left = horizontal;
			this.m_Right = horizontal;
			this.m_Top = vertical;
			this.m_Bottom = vertical;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001D984 File Offset: 0x0001BB84
		public static bool operator ==(Offset lhs, Offset rhs)
		{
			return lhs.m_Left == rhs.m_Left && lhs.m_Right == rhs.m_Right && lhs.m_Top == rhs.m_Top && lhs.m_Bottom == rhs.m_Bottom;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0001D9D4 File Offset: 0x0001BBD4
		public static bool operator !=(Offset lhs, Offset rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0001D9F0 File Offset: 0x0001BBF0
		public static Offset operator *(Offset a, float b)
		{
			return new Offset(a.m_Left * b, a.m_Right * b, a.m_Top * b, a.m_Bottom * b);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0001DA28 File Offset: 0x0001BC28
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0001DA4C File Offset: 0x0001BC4C
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0001DA70 File Offset: 0x0001BC70
		public bool Equals(Offset other)
		{
			return base.Equals(other);
		}

		// Token: 0x04000262 RID: 610
		private float m_Left;

		// Token: 0x04000263 RID: 611
		private float m_Right;

		// Token: 0x04000264 RID: 612
		private float m_Top;

		// Token: 0x04000265 RID: 613
		private float m_Bottom;

		// Token: 0x04000266 RID: 614
		private static readonly Offset k_ZeroOffset = new Offset(0f, 0f, 0f, 0f);
	}
}
