using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F5 RID: 757
	[Serializable]
	public struct Length : IEquatable<Length>
	{
		// Token: 0x060019A3 RID: 6563 RVA: 0x00067BA0 File Offset: 0x00065DA0
		public static Length Percent(float value)
		{
			return new Length(value, LengthUnit.Percent);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00067BBC File Offset: 0x00065DBC
		public static Length Auto()
		{
			return new Length(0f, Length.Unit.Auto);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00067BDC File Offset: 0x00065DDC
		public static Length None()
		{
			return new Length(0f, Length.Unit.None);
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060019A6 RID: 6566 RVA: 0x00067BF9 File Offset: 0x00065DF9
		// (set) Token: 0x060019A7 RID: 6567 RVA: 0x00067C01 File Offset: 0x00065E01
		public float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = Mathf.Clamp(value, -8388608f, 8388608f);
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x00067C19 File Offset: 0x00065E19
		// (set) Token: 0x060019A9 RID: 6569 RVA: 0x00067C21 File Offset: 0x00065E21
		public LengthUnit unit
		{
			get
			{
				return (LengthUnit)this.m_Unit;
			}
			set
			{
				this.m_Unit = (Length.Unit)value;
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00067C2A File Offset: 0x00065E2A
		public bool IsAuto()
		{
			return this.m_Unit == Length.Unit.Auto;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00067C35 File Offset: 0x00065E35
		public bool IsNone()
		{
			return this.m_Unit == Length.Unit.None;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00067C40 File Offset: 0x00065E40
		public Length(float value)
		{
			this = new Length(value, Length.Unit.Pixel);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00067C4C File Offset: 0x00065E4C
		public Length(float value, LengthUnit unit)
		{
			this = new Length(value, (Length.Unit)unit);
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00067C58 File Offset: 0x00065E58
		private Length(float value, Length.Unit unit)
		{
			this = default(Length);
			this.value = value;
			this.m_Unit = unit;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00067C74 File Offset: 0x00065E74
		public static implicit operator Length(float value)
		{
			return new Length(value, LengthUnit.Pixel);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00067C90 File Offset: 0x00065E90
		public static bool operator ==(Length lhs, Length rhs)
		{
			return lhs.m_Value == rhs.m_Value && lhs.m_Unit == rhs.m_Unit;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00067CC4 File Offset: 0x00065EC4
		public static bool operator !=(Length lhs, Length rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00067CE0 File Offset: 0x00065EE0
		public bool Equals(Length other)
		{
			return other == this;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00067D00 File Offset: 0x00065F00
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Length)
			{
				Length length = (Length)obj;
				flag = this.Equals(length);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00067D2C File Offset: 0x00065F2C
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Unit;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00067D58 File Offset: 0x00065F58
		public override string ToString()
		{
			string text = this.value.ToString(CultureInfo.InvariantCulture.NumberFormat);
			string text2 = string.Empty;
			switch (this.m_Unit)
			{
			case Length.Unit.Pixel:
			{
				bool flag = !Mathf.Approximately(0f, this.value);
				if (flag)
				{
					text2 = "px";
				}
				break;
			}
			case Length.Unit.Percent:
				text2 = "%";
				break;
			case Length.Unit.Auto:
				text = "auto";
				break;
			case Length.Unit.None:
				text = "none";
				break;
			}
			return text + text2;
		}

		// Token: 0x04000ACA RID: 2762
		internal const float k_MaxValue = 8388608f;

		// Token: 0x04000ACB RID: 2763
		[SerializeField]
		private float m_Value;

		// Token: 0x04000ACC RID: 2764
		[SerializeField]
		private Length.Unit m_Unit;

		// Token: 0x020002F6 RID: 758
		private enum Unit
		{
			// Token: 0x04000ACE RID: 2766
			Pixel,
			// Token: 0x04000ACF RID: 2767
			Percent,
			// Token: 0x04000AD0 RID: 2768
			Auto,
			// Token: 0x04000AD1 RID: 2769
			None
		}
	}
}
