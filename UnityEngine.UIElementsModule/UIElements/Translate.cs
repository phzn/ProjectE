using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000313 RID: 787
	public struct Translate : IEquatable<Translate>
	{
		// Token: 0x06001B33 RID: 6963 RVA: 0x0006ADE7 File Offset: 0x00068FE7
		public Translate(Length x, Length y, float z)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = z;
			this.m_isNone = false;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0006AE06 File Offset: 0x00069006
		public Translate(Length x, Length y)
		{
			this = new Translate(x, y, 0f);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0006AE18 File Offset: 0x00069018
		public static Translate None()
		{
			return new Translate
			{
				m_isNone = true
			};
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001B36 RID: 6966 RVA: 0x0006AE3B File Offset: 0x0006903B
		// (set) Token: 0x06001B37 RID: 6967 RVA: 0x0006AE43 File Offset: 0x00069043
		public Length x
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x0006AE4C File Offset: 0x0006904C
		// (set) Token: 0x06001B39 RID: 6969 RVA: 0x0006AE54 File Offset: 0x00069054
		public Length y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x0006AE5D File Offset: 0x0006905D
		// (set) Token: 0x06001B3B RID: 6971 RVA: 0x0006AE65 File Offset: 0x00069065
		public float z
		{
			get
			{
				return this.m_Z;
			}
			set
			{
				this.m_Z = value;
			}
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0006AE6E File Offset: 0x0006906E
		internal bool IsNone()
		{
			return this.m_isNone;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0006AE78 File Offset: 0x00069078
		public static bool operator ==(Translate lhs, Translate rhs)
		{
			return lhs.m_X == rhs.m_X && lhs.m_Y == rhs.m_Y && lhs.m_Z == rhs.m_Z && lhs.m_isNone == rhs.m_isNone;
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0006AED0 File Offset: 0x000690D0
		public static bool operator !=(Translate lhs, Translate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0006AEEC File Offset: 0x000690EC
		public bool Equals(Translate other)
		{
			return other == this;
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0006AF0C File Offset: 0x0006910C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Translate)
			{
				Translate translate = (Translate)obj;
				flag = this.Equals(translate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0006AF38 File Offset: 0x00069138
		public override int GetHashCode()
		{
			return (this.m_X.GetHashCode() * 793) ^ (this.m_Y.GetHashCode() * 791) ^ (this.m_Z.GetHashCode() * 571);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0006AF8C File Offset: 0x0006918C
		public override string ToString()
		{
			string text = this.m_Z.ToString(CultureInfo.InvariantCulture.NumberFormat);
			return string.Concat(new string[]
			{
				this.m_X.ToString(),
				" ",
				this.m_Y.ToString(),
				" ",
				text
			});
		}

		// Token: 0x04000B0E RID: 2830
		private Length m_X;

		// Token: 0x04000B0F RID: 2831
		private Length m_Y;

		// Token: 0x04000B10 RID: 2832
		private float m_Z;

		// Token: 0x04000B11 RID: 2833
		private bool m_isNone;
	}
}
