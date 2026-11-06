using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F7 RID: 759
	public struct Rotate : IEquatable<Rotate>
	{
		// Token: 0x060019B6 RID: 6582 RVA: 0x00067DEE File Offset: 0x00065FEE
		internal Rotate(Angle angle, Vector3 axis)
		{
			this.m_Angle = angle;
			this.m_Axis = axis;
			this.m_IsNone = false;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00067E06 File Offset: 0x00066006
		public Rotate(Angle angle)
		{
			this.m_Angle = angle;
			this.m_Axis = Vector3.forward;
			this.m_IsNone = false;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00067E24 File Offset: 0x00066024
		internal static Rotate Initial()
		{
			return new Rotate(0f);
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00067E48 File Offset: 0x00066048
		public static Rotate None()
		{
			Rotate rotate = Rotate.Initial();
			rotate.m_IsNone = true;
			return rotate;
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x00067E69 File Offset: 0x00066069
		// (set) Token: 0x060019BB RID: 6587 RVA: 0x00067E71 File Offset: 0x00066071
		public Angle angle
		{
			get
			{
				return this.m_Angle;
			}
			set
			{
				this.m_Angle = value;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x00067E7A File Offset: 0x0006607A
		// (set) Token: 0x060019BD RID: 6589 RVA: 0x00067E82 File Offset: 0x00066082
		internal Vector3 axis
		{
			get
			{
				return this.m_Axis;
			}
			set
			{
				this.m_Axis = value;
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00067E8B File Offset: 0x0006608B
		internal bool IsNone()
		{
			return this.m_IsNone;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00067E94 File Offset: 0x00066094
		public static bool operator ==(Rotate lhs, Rotate rhs)
		{
			return lhs.m_Angle == rhs.m_Angle && lhs.m_Axis == rhs.m_Axis && lhs.m_IsNone == rhs.m_IsNone;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00067EE0 File Offset: 0x000660E0
		public static bool operator !=(Rotate lhs, Rotate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00067EFC File Offset: 0x000660FC
		public bool Equals(Rotate other)
		{
			return other == this;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00067F1C File Offset: 0x0006611C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Rotate)
			{
				Rotate rotate = (Rotate)obj;
				flag = this.Equals(rotate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x00067F48 File Offset: 0x00066148
		public override int GetHashCode()
		{
			return (this.m_Angle.GetHashCode() * 793) ^ (this.m_Axis.GetHashCode() * 791) ^ (this.m_IsNone.GetHashCode() * 197);
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00067F9C File Offset: 0x0006619C
		public override string ToString()
		{
			return this.m_Angle.ToString() + " " + this.m_Axis.ToString();
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00067FDC File Offset: 0x000661DC
		internal Quaternion ToQuaternion()
		{
			return Quaternion.AngleAxis(this.m_Angle.ToDegrees(), this.m_Axis);
		}

		// Token: 0x04000AD2 RID: 2770
		private Angle m_Angle;

		// Token: 0x04000AD3 RID: 2771
		private Vector3 m_Axis;

		// Token: 0x04000AD4 RID: 2772
		private bool m_IsNone;
	}
}
