using System;
using System.Text;

namespace Pathfinding.Util
{
	// Token: 0x020002A4 RID: 676
	public struct Guid
	{
		// Token: 0x060010E9 RID: 4329 RVA: 0x00065DCC File Offset: 0x00063FCC
		public Guid(byte[] bytes)
		{
			ulong num = (ulong)bytes[0] | ((ulong)bytes[1] << 8) | ((ulong)bytes[2] << 16) | ((ulong)bytes[3] << 24) | ((ulong)bytes[4] << 32) | ((ulong)bytes[5] << 40) | ((ulong)bytes[6] << 48) | ((ulong)bytes[7] << 56);
			ulong num2 = (ulong)bytes[8] | ((ulong)bytes[9] << 8) | ((ulong)bytes[10] << 16) | ((ulong)bytes[11] << 24) | ((ulong)bytes[12] << 32) | ((ulong)bytes[13] << 40) | ((ulong)bytes[14] << 48) | ((ulong)bytes[15] << 56);
			this._a = (BitConverter.IsLittleEndian ? num : Guid.SwapEndianness(num));
			this._b = (BitConverter.IsLittleEndian ? num2 : Guid.SwapEndianness(num2));
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00065E84 File Offset: 0x00064084
		public Guid(string str)
		{
			this._a = 0UL;
			this._b = 0UL;
			if (str.Length < 32)
			{
				throw new FormatException("Invalid Guid format");
			}
			int i = 0;
			int num = 0;
			int num2 = 60;
			while (i < 16)
			{
				if (num >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c = str[num];
				if (c != '-')
				{
					int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
					if (num3 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c.ToString() + " is not a hexadecimal character");
					}
					this._a |= (ulong)((ulong)((long)num3) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
			num2 = 60;
			while (i < 32)
			{
				if (num >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c2 = str[num];
				if (c2 != '-')
				{
					int num4 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c2));
					if (num4 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c2.ToString() + " is not a hexadecimal character");
					}
					this._b |= (ulong)((ulong)((long)num4) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00065FBB File Offset: 0x000641BB
		public static Guid Parse(string input)
		{
			return new Guid(input);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00065FC4 File Offset: 0x000641C4
		private static ulong SwapEndianness(ulong value)
		{
			ulong num = value & 255UL;
			ulong num2 = (value >> 8) & 255UL;
			ulong num3 = (value >> 16) & 255UL;
			ulong num4 = (value >> 24) & 255UL;
			ulong num5 = (value >> 32) & 255UL;
			ulong num6 = (value >> 40) & 255UL;
			ulong num7 = (value >> 48) & 255UL;
			ulong num8 = (value >> 56) & 255UL;
			return (num << 56) | (num2 << 48) | (num3 << 40) | (num4 << 32) | (num5 << 24) | (num6 << 16) | (num7 << 8) | num8;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00066054 File Offset: 0x00064254
		public static Guid NewGuid()
		{
			byte[] array = new byte[16];
			Guid.random.NextBytes(array);
			return new Guid(array);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0006607A File Offset: 0x0006427A
		public static bool operator ==(Guid lhs, Guid rhs)
		{
			return lhs._a == rhs._a && lhs._b == rhs._b;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0006609A File Offset: 0x0006429A
		public static bool operator !=(Guid lhs, Guid rhs)
		{
			return lhs._a != rhs._a || lhs._b != rhs._b;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x000660C0 File Offset: 0x000642C0
		public override bool Equals(object _rhs)
		{
			if (!(_rhs is Guid))
			{
				return false;
			}
			Guid guid = (Guid)_rhs;
			return this._a == guid._a && this._b == guid._b;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000660FC File Offset: 0x000642FC
		public override int GetHashCode()
		{
			ulong num = this._a ^ this._b;
			return (int)(num >> 32) ^ (int)num;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00066120 File Offset: 0x00064320
		public override string ToString()
		{
			if (Guid.text == null)
			{
				Guid.text = new StringBuilder();
			}
			StringBuilder stringBuilder = Guid.text;
			string text;
			lock (stringBuilder)
			{
				Guid.text.Length = 0;
				Guid.text.Append(this._a.ToString("x16")).Append('-').Append(this._b.ToString("x16"));
				text = Guid.text.ToString();
			}
			return text;
		}

		// Token: 0x04000C23 RID: 3107
		private const string hex = "0123456789ABCDEF";

		// Token: 0x04000C24 RID: 3108
		public static readonly Guid zero = new Guid(new byte[16]);

		// Token: 0x04000C25 RID: 3109
		public static readonly string zeroString = new Guid(new byte[16]).ToString();

		// Token: 0x04000C26 RID: 3110
		private readonly ulong _a;

		// Token: 0x04000C27 RID: 3111
		private readonly ulong _b;

		// Token: 0x04000C28 RID: 3112
		private static Random random = new Random();

		// Token: 0x04000C29 RID: 3113
		private static StringBuilder text;
	}
}
