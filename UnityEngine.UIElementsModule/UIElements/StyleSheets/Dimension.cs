using System;
using System.Globalization;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x0200048A RID: 1162
	[Serializable]
	internal struct Dimension : IEquatable<Dimension>
	{
		// Token: 0x0600245E RID: 9310 RVA: 0x0009734D File Offset: 0x0009554D
		public Dimension(float value, Dimension.Unit unit)
		{
			this.unit = unit;
			this.value = value;
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x00097360 File Offset: 0x00095560
		public Length ToLength()
		{
			LengthUnit lengthUnit = ((this.unit == Dimension.Unit.Percent) ? LengthUnit.Percent : LengthUnit.Pixel);
			return new Length(this.value, lengthUnit);
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0009738C File Offset: 0x0009558C
		public TimeValue ToTime()
		{
			TimeUnit timeUnit = ((this.unit == Dimension.Unit.Millisecond) ? TimeUnit.Millisecond : TimeUnit.Second);
			return new TimeValue(this.value, timeUnit);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000973B8 File Offset: 0x000955B8
		public Angle ToAngle()
		{
			Angle angle;
			switch (this.unit)
			{
			case Dimension.Unit.Degree:
				angle = new Angle(this.value, AngleUnit.Degree);
				break;
			case Dimension.Unit.Gradian:
				angle = new Angle(this.value, AngleUnit.Gradian);
				break;
			case Dimension.Unit.Radian:
				angle = new Angle(this.value, AngleUnit.Radian);
				break;
			case Dimension.Unit.Turn:
				angle = new Angle(this.value, AngleUnit.Turn);
				break;
			default:
				angle = new Angle(this.value, AngleUnit.Degree);
				break;
			}
			return angle;
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x00097438 File Offset: 0x00095638
		public static bool operator ==(Dimension lhs, Dimension rhs)
		{
			return lhs.value == rhs.value && lhs.unit == rhs.unit;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0009746C File Offset: 0x0009566C
		public static bool operator !=(Dimension lhs, Dimension rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x00097488 File Offset: 0x00095688
		public bool Equals(Dimension other)
		{
			return other == this;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000974A8 File Offset: 0x000956A8
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Dimension);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Dimension dimension = (Dimension)obj;
				flag2 = dimension == this;
			}
			return flag2;
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000974E4 File Offset: 0x000956E4
		public override int GetHashCode()
		{
			int num = -799583767;
			num = num * -1521134295 + this.unit.GetHashCode();
			return num * -1521134295 + this.value.GetHashCode();
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0009752C File Offset: 0x0009572C
		public override string ToString()
		{
			string text = string.Empty;
			switch (this.unit)
			{
			case Dimension.Unit.Unitless:
				text = string.Empty;
				break;
			case Dimension.Unit.Pixel:
				text = "px";
				break;
			case Dimension.Unit.Percent:
				text = "%";
				break;
			case Dimension.Unit.Second:
				text = "s";
				break;
			case Dimension.Unit.Millisecond:
				text = "ms";
				break;
			case Dimension.Unit.Degree:
				text = "deg";
				break;
			case Dimension.Unit.Gradian:
				text = "grad";
				break;
			case Dimension.Unit.Radian:
				text = "rad";
				break;
			case Dimension.Unit.Turn:
				text = "turn";
				break;
			}
			return this.value.ToString(CultureInfo.InvariantCulture.NumberFormat) + text;
		}

		// Token: 0x04001171 RID: 4465
		public Dimension.Unit unit;

		// Token: 0x04001172 RID: 4466
		public float value;

		// Token: 0x0200048B RID: 1163
		public enum Unit
		{
			// Token: 0x04001174 RID: 4468
			Unitless,
			// Token: 0x04001175 RID: 4469
			Pixel,
			// Token: 0x04001176 RID: 4470
			Percent,
			// Token: 0x04001177 RID: 4471
			Second,
			// Token: 0x04001178 RID: 4472
			Millisecond,
			// Token: 0x04001179 RID: 4473
			Degree,
			// Token: 0x0400117A RID: 4474
			Gradian,
			// Token: 0x0400117B RID: 4475
			Radian,
			// Token: 0x0400117C RID: 4476
			Turn
		}
	}
}
