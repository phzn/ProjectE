using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020002C3 RID: 707
	public struct Angle : IEquatable<Angle>
	{
		// Token: 0x0600146E RID: 5230 RVA: 0x00048E5C File Offset: 0x0004705C
		public static Angle Degrees(float value)
		{
			return new Angle(value, AngleUnit.Degree);
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00048E78 File Offset: 0x00047078
		public static Angle Gradians(float value)
		{
			return new Angle(value, AngleUnit.Gradian);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00048E94 File Offset: 0x00047094
		public static Angle Radians(float value)
		{
			return new Angle(value, AngleUnit.Radian);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00048EB0 File Offset: 0x000470B0
		public static Angle Turns(float value)
		{
			return new Angle(value, AngleUnit.Turn);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00048ECC File Offset: 0x000470CC
		internal static Angle None()
		{
			return new Angle(0f, Angle.Unit.None);
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x00048EE9 File Offset: 0x000470E9
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x00048EF1 File Offset: 0x000470F1
		public float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x00048EFA File Offset: 0x000470FA
		// (set) Token: 0x06001476 RID: 5238 RVA: 0x00048F02 File Offset: 0x00047102
		public AngleUnit unit
		{
			get
			{
				return (AngleUnit)this.m_Unit;
			}
			set
			{
				this.m_Unit = (Angle.Unit)value;
			}
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00048F0B File Offset: 0x0004710B
		internal bool IsNone()
		{
			return this.m_Unit == Angle.Unit.None;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00048F16 File Offset: 0x00047116
		public Angle(float value)
		{
			this = new Angle(value, Angle.Unit.Degree);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00048F22 File Offset: 0x00047122
		public Angle(float value, AngleUnit unit)
		{
			this = new Angle(value, (Angle.Unit)unit);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00048F2E File Offset: 0x0004712E
		private Angle(float value, Angle.Unit unit)
		{
			this.m_Value = value;
			this.m_Unit = unit;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00048F40 File Offset: 0x00047140
		public float ToDegrees()
		{
			float num;
			switch (this.m_Unit)
			{
			case Angle.Unit.Degree:
				num = this.m_Value;
				break;
			case Angle.Unit.Gradian:
				num = this.m_Value * 360f / 400f;
				break;
			case Angle.Unit.Radian:
				num = this.m_Value * 180f / 3.1415927f;
				break;
			case Angle.Unit.Turn:
				num = this.m_Value * 360f;
				break;
			case Angle.Unit.None:
				num = 0f;
				break;
			default:
				num = 0f;
				break;
			}
			return num;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00048FC8 File Offset: 0x000471C8
		public float ToGradians()
		{
			float num;
			switch (this.m_Unit)
			{
			case Angle.Unit.Degree:
				num = this.m_Value * 10f / 9f;
				break;
			case Angle.Unit.Gradian:
				num = this.m_Value;
				break;
			case Angle.Unit.Radian:
				num = this.m_Value * 200f / 3.1415927f;
				break;
			case Angle.Unit.Turn:
				num = this.m_Value * 400f;
				break;
			case Angle.Unit.None:
				num = 0f;
				break;
			default:
				num = 0f;
				break;
			}
			return num;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00049050 File Offset: 0x00047250
		public float ToRadians()
		{
			float num;
			switch (this.m_Unit)
			{
			case Angle.Unit.Degree:
				num = this.m_Value * 3.1415927f / 180f;
				break;
			case Angle.Unit.Gradian:
				num = this.m_Value * 3.1415927f / 200f;
				break;
			case Angle.Unit.Radian:
				num = this.m_Value;
				break;
			case Angle.Unit.Turn:
				num = this.m_Value * 3.1415927f * 2f;
				break;
			case Angle.Unit.None:
				num = 0f;
				break;
			default:
				num = 0f;
				break;
			}
			return num;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x000490DC File Offset: 0x000472DC
		public float ToTurns()
		{
			float num;
			switch (this.m_Unit)
			{
			case Angle.Unit.Degree:
				num = this.m_Value / 360f;
				break;
			case Angle.Unit.Gradian:
				num = this.m_Value / 400f;
				break;
			case Angle.Unit.Radian:
				num = this.m_Value / 6.2831855f;
				break;
			case Angle.Unit.Turn:
				num = this.m_Value;
				break;
			case Angle.Unit.None:
				num = 0f;
				break;
			default:
				num = 0f;
				break;
			}
			return num;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00049158 File Offset: 0x00047358
		internal void ConvertTo(AngleUnit newUnit)
		{
			if (!true)
			{
			}
			float num;
			switch (newUnit)
			{
			case AngleUnit.Degree:
				num = this.ToDegrees();
				break;
			case AngleUnit.Gradian:
				num = this.ToGradians();
				break;
			case AngleUnit.Radian:
				num = this.ToRadians();
				break;
			case AngleUnit.Turn:
				num = this.ToTurns();
				break;
			default:
				throw new NotImplementedException();
			}
			if (!true)
			{
			}
			this.m_Value = num;
			this.m_Unit = (Angle.Unit)newUnit;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x000491C0 File Offset: 0x000473C0
		public static implicit operator Angle(float value)
		{
			return new Angle(value, AngleUnit.Degree);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x000491DC File Offset: 0x000473DC
		public static bool operator ==(Angle lhs, Angle rhs)
		{
			return lhs.m_Value == rhs.m_Value && lhs.m_Unit == rhs.m_Unit;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00049210 File Offset: 0x00047410
		public static bool operator !=(Angle lhs, Angle rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0004922C File Offset: 0x0004742C
		public bool Equals(Angle other)
		{
			return other == this;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004924C File Offset: 0x0004744C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Angle)
			{
				Angle angle = (Angle)obj;
				flag = this.Equals(angle);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00049278 File Offset: 0x00047478
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Unit;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x000492A4 File Offset: 0x000474A4
		public override string ToString()
		{
			string text = this.value.ToString(CultureInfo.InvariantCulture.NumberFormat);
			string text2 = string.Empty;
			switch (this.m_Unit)
			{
			case Angle.Unit.Degree:
			{
				bool flag = !Mathf.Approximately(0f, this.value);
				if (flag)
				{
					text2 = "deg";
				}
				break;
			}
			case Angle.Unit.Gradian:
				text2 = "grad";
				break;
			case Angle.Unit.Radian:
				text2 = "rad";
				break;
			case Angle.Unit.Turn:
				text2 = "turn";
				break;
			case Angle.Unit.None:
				text = "";
				break;
			}
			return text + text2;
		}

		// Token: 0x04000985 RID: 2437
		private float m_Value;

		// Token: 0x04000986 RID: 2438
		private Angle.Unit m_Unit;

		// Token: 0x020002C4 RID: 708
		private enum Unit
		{
			// Token: 0x04000988 RID: 2440
			Degree,
			// Token: 0x04000989 RID: 2441
			Gradian,
			// Token: 0x0400098A RID: 2442
			Radian,
			// Token: 0x0400098B RID: 2443
			Turn,
			// Token: 0x0400098C RID: 2444
			None
		}
	}
}
