using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x02000245 RID: 581
	internal sealed class NumberFormatter
	{
		// Token: 0x06001A8B RID: 6795
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void GetFormatterTables(out ulong* MantissaBitsTable, out int* TensExponentTable, out char* DigitLowerTable, out char* DigitUpperTable, out long* TenPowersList, out int* DecHexDigits);

		// Token: 0x06001A8C RID: 6796 RVA: 0x00061574 File Offset: 0x0005F774
		static NumberFormatter()
		{
			NumberFormatter.GetFormatterTables(out NumberFormatter.MantissaBitsTable, out NumberFormatter.TensExponentTable, out NumberFormatter.DigitLowerTable, out NumberFormatter.DigitUpperTable, out NumberFormatter.TenPowersList, out NumberFormatter.DecHexDigits);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x00061599 File Offset: 0x0005F799
		private unsafe static long GetTenPowerOf(int i)
		{
			return NumberFormatter.TenPowersList[i];
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000615A8 File Offset: 0x0005F7A8
		private void InitDecHexDigits(uint value)
		{
			if (value >= 100000000U)
			{
				int num = (int)(value / 100000000U);
				value -= (uint)(100000000 * num);
				this._val2 = NumberFormatter.FastToDecHex(num);
			}
			this._val1 = NumberFormatter.ToDecHex((int)value);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000615E8 File Offset: 0x0005F7E8
		private void InitDecHexDigits(ulong value)
		{
			if (value >= 100000000UL)
			{
				long num = (long)(value / 100000000UL);
				value -= (ulong)(100000000L * num);
				if (num >= 100000000L)
				{
					int num2 = (int)(num / 100000000L);
					num -= (long)num2 * 100000000L;
					this._val3 = NumberFormatter.ToDecHex(num2);
				}
				if (num != 0L)
				{
					this._val2 = NumberFormatter.ToDecHex((int)num);
				}
			}
			if (value != 0UL)
			{
				this._val1 = NumberFormatter.ToDecHex((int)value);
			}
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00061660 File Offset: 0x0005F860
		private void InitDecHexDigits(uint hi, ulong lo)
		{
			if (hi == 0U)
			{
				this.InitDecHexDigits(lo);
				return;
			}
			uint num = hi / 100000000U;
			ulong num2 = (ulong)(hi - num * 100000000U);
			ulong num3 = lo / 100000000UL;
			ulong num4 = lo - num3 * 100000000UL + num2 * 9551616UL;
			hi = num;
			lo = num3 + num2 * 184467440737UL;
			num3 = num4 / 100000000UL;
			num4 -= num3 * 100000000UL;
			lo += num3;
			this._val1 = NumberFormatter.ToDecHex((int)num4);
			num3 = lo / 100000000UL;
			num4 = lo - num3 * 100000000UL;
			lo = num3;
			if (hi != 0U)
			{
				lo += (ulong)hi * 184467440737UL;
				num4 += (ulong)hi * 9551616UL;
				num3 = num4 / 100000000UL;
				lo += num3;
				num4 -= num3 * 100000000UL;
			}
			this._val2 = NumberFormatter.ToDecHex((int)num4);
			if (lo >= 100000000UL)
			{
				num3 = lo / 100000000UL;
				lo -= num3 * 100000000UL;
				this._val4 = NumberFormatter.ToDecHex((int)num3);
			}
			this._val3 = NumberFormatter.ToDecHex((int)lo);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00061774 File Offset: 0x0005F974
		private unsafe static uint FastToDecHex(int val)
		{
			if (val < 100)
			{
				return (uint)NumberFormatter.DecHexDigits[val];
			}
			int num = val * 5243 >> 19;
			return (uint)((NumberFormatter.DecHexDigits[num] << 8) | NumberFormatter.DecHexDigits[val - num * 100]);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000617BC File Offset: 0x0005F9BC
		private static uint ToDecHex(int val)
		{
			uint num = 0U;
			if (val >= 10000)
			{
				int num2 = val / 10000;
				val -= num2 * 10000;
				num = NumberFormatter.FastToDecHex(num2) << 16;
			}
			return num | NumberFormatter.FastToDecHex(val);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000617F8 File Offset: 0x0005F9F8
		private static int FastDecHexLen(int val)
		{
			if (val < 256)
			{
				if (val < 16)
				{
					return 1;
				}
				return 2;
			}
			else
			{
				if (val < 4096)
				{
					return 3;
				}
				return 4;
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00061816 File Offset: 0x0005FA16
		private static int DecHexLen(uint val)
		{
			if (val < 65536U)
			{
				return NumberFormatter.FastDecHexLen((int)val);
			}
			return 4 + NumberFormatter.FastDecHexLen((int)(val >> 16));
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00061834 File Offset: 0x0005FA34
		private int DecHexLen()
		{
			if (this._val4 != 0U)
			{
				return NumberFormatter.DecHexLen(this._val4) + 24;
			}
			if (this._val3 != 0U)
			{
				return NumberFormatter.DecHexLen(this._val3) + 16;
			}
			if (this._val2 != 0U)
			{
				return NumberFormatter.DecHexLen(this._val2) + 8;
			}
			if (this._val1 != 0U)
			{
				return NumberFormatter.DecHexLen(this._val1);
			}
			return 0;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0006189C File Offset: 0x0005FA9C
		private static int ScaleOrder(long hi)
		{
			for (int i = 18; i >= 0; i--)
			{
				if (hi >= NumberFormatter.GetTenPowerOf(i))
				{
					return i + 1;
				}
			}
			return 1;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000618C4 File Offset: 0x0005FAC4
		private int InitialFloatingPrecision()
		{
			if (this._specifier == 'R')
			{
				return this._defPrecision + 2;
			}
			if (this._precision < this._defPrecision)
			{
				return this._defPrecision;
			}
			if (this._specifier == 'G')
			{
				return Math.Min(this._defPrecision + 2, this._precision);
			}
			if (this._specifier == 'E')
			{
				return Math.Min(this._defPrecision + 2, this._precision + 1);
			}
			return this._defPrecision;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00061940 File Offset: 0x0005FB40
		private static int ParsePrecision(string format)
		{
			int num = 0;
			for (int i = 1; i < format.Length; i++)
			{
				int num2 = (int)(format[i] - '0');
				num = num * 10 + num2;
				if (num2 < 0 || num2 > 9 || num > 99)
				{
					return -2;
				}
			}
			return num;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00061984 File Offset: 0x0005FB84
		private NumberFormatter(Thread current)
		{
			this._cbuf = EmptyArray<char>.Value;
			if (current == null)
			{
				return;
			}
			this.CurrentCulture = current.CurrentCulture;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000619A8 File Offset: 0x0005FBA8
		private void Init(string format)
		{
			this._val1 = (this._val2 = (this._val3 = (this._val4 = 0U)));
			this._offset = 0;
			this._NaN = (this._infinity = false);
			this._isCustomFormat = false;
			this._specifierIsUpper = true;
			this._precision = -1;
			if (format == null || format.Length == 0)
			{
				this._specifier = 'G';
				return;
			}
			char c = format[0];
			if (c >= 'a' && c <= 'z')
			{
				c = c - 'a' + 'A';
				this._specifierIsUpper = false;
			}
			else if (c < 'A' || c > 'Z')
			{
				this._isCustomFormat = true;
				this._specifier = '0';
				return;
			}
			this._specifier = c;
			if (format.Length > 1)
			{
				this._precision = NumberFormatter.ParsePrecision(format);
				if (this._precision == -2)
				{
					this._isCustomFormat = true;
					this._specifier = '0';
					this._precision = -1;
				}
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00061A94 File Offset: 0x0005FC94
		private void InitHex(ulong value)
		{
			if (this._defPrecision == 10)
			{
				value = (ulong)((uint)value);
			}
			this._val1 = (uint)value;
			this._val2 = (uint)(value >> 32);
			this._decPointPos = (this._digitsLen = this.DecHexLen());
			if (value == 0UL)
			{
				this._decPointPos = 1;
			}
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00061AE4 File Offset: 0x0005FCE4
		private void Init(string format, int value, int defPrecision)
		{
			this.Init(format);
			this._defPrecision = defPrecision;
			this._positive = value >= 0;
			if (value == 0 || this._specifier == 'X')
			{
				this.InitHex((ulong)((long)value));
				return;
			}
			if (value < 0)
			{
				value = -value;
			}
			this.InitDecHexDigits((uint)value);
			this._decPointPos = (this._digitsLen = this.DecHexLen());
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00061B48 File Offset: 0x0005FD48
		private void Init(string format, uint value, int defPrecision)
		{
			this.Init(format);
			this._defPrecision = defPrecision;
			this._positive = true;
			if (value == 0U || this._specifier == 'X')
			{
				this.InitHex((ulong)value);
				return;
			}
			this.InitDecHexDigits(value);
			this._decPointPos = (this._digitsLen = this.DecHexLen());
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00061B9C File Offset: 0x0005FD9C
		private void Init(string format, long value)
		{
			this.Init(format);
			this._defPrecision = 19;
			this._positive = value >= 0L;
			if (value == 0L || this._specifier == 'X')
			{
				this.InitHex((ulong)value);
				return;
			}
			if (value < 0L)
			{
				value = -value;
			}
			this.InitDecHexDigits((ulong)value);
			this._decPointPos = (this._digitsLen = this.DecHexLen());
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00061C00 File Offset: 0x0005FE00
		private void Init(string format, ulong value)
		{
			this.Init(format);
			this._defPrecision = 20;
			this._positive = true;
			if (value == 0UL || this._specifier == 'X')
			{
				this.InitHex(value);
				return;
			}
			this.InitDecHexDigits(value);
			this._decPointPos = (this._digitsLen = this.DecHexLen());
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00061C54 File Offset: 0x0005FE54
		private unsafe void Init(string format, double value, int defPrecision)
		{
			this.Init(format);
			this._defPrecision = defPrecision;
			long num = BitConverter.DoubleToInt64Bits(value);
			this._positive = num >= 0L;
			num &= long.MaxValue;
			if (num == 0L)
			{
				this._decPointPos = 1;
				this._digitsLen = 0;
				this._positive = true;
				return;
			}
			int num2 = (int)(num >> 52);
			long num3 = num & 4503599627370495L;
			if (num2 == 2047)
			{
				this._NaN = num3 != 0L;
				this._infinity = num3 == 0L;
				return;
			}
			int num4 = 0;
			if (num2 == 0)
			{
				num2 = 1;
				int num5 = NumberFormatter.ScaleOrder(num3);
				if (num5 < 15)
				{
					num4 = num5 - 15;
					num3 *= NumberFormatter.GetTenPowerOf(-num4);
				}
			}
			else
			{
				num3 = (num3 + 4503599627370495L + 1L) * 10L;
				num4 = -1;
			}
			ulong num6 = (ulong)((uint)num3);
			ulong num7 = (ulong)num3 >> 32;
			ulong num8 = NumberFormatter.MantissaBitsTable[num2];
			ulong num9 = num8 >> 32;
			num8 = (ulong)((uint)num8);
			ulong num10 = num7 * num8 + num6 * num9 + (num6 * num8 >> 32);
			long num11 = (long)(num7 * num9 + (num10 >> 32));
			while (num11 < 10000000000000000L)
			{
				num10 = (num10 & (ulong)(-1)) * 10UL;
				num11 = num11 * 10L + (long)(num10 >> 32);
				num4--;
			}
			if ((num10 & (ulong)(-2147483648)) != 0UL)
			{
				num11 += 1L;
			}
			int num12 = 17;
			this._decPointPos = NumberFormatter.TensExponentTable[num2] + num4 + num12;
			int num13 = this.InitialFloatingPrecision();
			if (num12 > num13)
			{
				long tenPowerOf = NumberFormatter.GetTenPowerOf(num12 - num13);
				num11 = (num11 + (tenPowerOf >> 1)) / tenPowerOf;
				num12 = num13;
			}
			if (num11 >= NumberFormatter.GetTenPowerOf(num12))
			{
				num12++;
				this._decPointPos++;
			}
			this.InitDecHexDigits((ulong)num11);
			this._offset = this.CountTrailingZeros();
			this._digitsLen = num12 - this._offset;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00061E20 File Offset: 0x00060020
		private void Init(string format, decimal value)
		{
			this.Init(format);
			this._defPrecision = 100;
			int[] bits = decimal.GetBits(value);
			int num = (bits[3] & 2031616) >> 16;
			this._positive = bits[3] >= 0;
			if (bits[0] == 0 && bits[1] == 0 && bits[2] == 0)
			{
				this._decPointPos = -num;
				this._positive = true;
				this._digitsLen = 0;
				return;
			}
			this.InitDecHexDigits((uint)bits[2], (ulong)(((long)bits[1] << 32) | (long)((ulong)bits[0])));
			this._digitsLen = this.DecHexLen();
			this._decPointPos = this._digitsLen - num;
			if (this._precision != -1 || this._specifier != 'G')
			{
				this._offset = this.CountTrailingZeros();
				this._digitsLen -= this._offset;
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00061EE6 File Offset: 0x000600E6
		private void ResetCharBuf(int size)
		{
			this._ind = 0;
			if (this._cbuf.Length < size)
			{
				this._cbuf = new char[size];
			}
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00061F06 File Offset: 0x00060106
		private void Resize(int len)
		{
			Array.Resize<char>(ref this._cbuf, len);
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00061F14 File Offset: 0x00060114
		private void Append(char c)
		{
			if (this._ind == this._cbuf.Length)
			{
				this.Resize(this._ind + 10);
			}
			char[] cbuf = this._cbuf;
			int ind = this._ind;
			this._ind = ind + 1;
			cbuf[ind] = c;
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00061F5C File Offset: 0x0006015C
		private void Append(char c, int cnt)
		{
			if (this._ind + cnt > this._cbuf.Length)
			{
				this.Resize(this._ind + cnt + 10);
			}
			while (cnt-- > 0)
			{
				char[] cbuf = this._cbuf;
				int ind = this._ind;
				this._ind = ind + 1;
				cbuf[ind] = c;
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00061FB0 File Offset: 0x000601B0
		private void Append(string s)
		{
			int length = s.Length;
			if (this._ind + length > this._cbuf.Length)
			{
				this.Resize(this._ind + length + 10);
			}
			for (int i = 0; i < length; i++)
			{
				char[] cbuf = this._cbuf;
				int ind = this._ind;
				this._ind = ind + 1;
				cbuf[ind] = s[i];
			}
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00062012 File Offset: 0x00060212
		private NumberFormatInfo GetNumberFormatInstance(IFormatProvider fp)
		{
			if (this._nfi != null && fp == null)
			{
				return this._nfi;
			}
			return NumberFormatInfo.GetInstance(fp);
		}

		// Token: 0x1700030B RID: 779
		// (set) Token: 0x06001AA8 RID: 6824 RVA: 0x0006202C File Offset: 0x0006022C
		private CultureInfo CurrentCulture
		{
			set
			{
				if (value != null && value.IsReadOnly)
				{
					this._nfi = value.NumberFormat;
					return;
				}
				this._nfi = null;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x0006204D File Offset: 0x0006024D
		private int IntegerDigits
		{
			get
			{
				if (this._decPointPos <= 0)
				{
					return 1;
				}
				return this._decPointPos;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x00062060 File Offset: 0x00060260
		private int DecimalDigits
		{
			get
			{
				if (this._digitsLen <= this._decPointPos)
				{
					return 0;
				}
				return this._digitsLen - this._decPointPos;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x0006207F File Offset: 0x0006027F
		private bool IsFloatingSource
		{
			get
			{
				return this._defPrecision == 15 || this._defPrecision == 7;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x00062096 File Offset: 0x00060296
		private bool IsZero
		{
			get
			{
				return this._digitsLen == 0;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001AAD RID: 6829 RVA: 0x000620A1 File Offset: 0x000602A1
		private bool IsZeroInteger
		{
			get
			{
				return this._digitsLen == 0 || this._decPointPos <= 0;
			}
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000620B9 File Offset: 0x000602B9
		private void RoundPos(int pos)
		{
			this.RoundBits(this._digitsLen - pos);
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x000620CA File Offset: 0x000602CA
		private bool RoundDecimal(int decimals)
		{
			return this.RoundBits(this._digitsLen - this._decPointPos - decimals);
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000620E4 File Offset: 0x000602E4
		private bool RoundBits(int shift)
		{
			if (shift <= 0)
			{
				return false;
			}
			if (shift > this._digitsLen)
			{
				this._digitsLen = 0;
				this._decPointPos = 1;
				this._val1 = (this._val2 = (this._val3 = (this._val4 = 0U)));
				this._positive = true;
				return false;
			}
			shift += this._offset;
			this._digitsLen += this._offset;
			while (shift > 8)
			{
				this._val1 = this._val2;
				this._val2 = this._val3;
				this._val3 = this._val4;
				this._val4 = 0U;
				this._digitsLen -= 8;
				shift -= 8;
			}
			shift = shift - 1 << 2;
			uint num = this._val1 >> shift;
			uint num2 = num & 15U;
			this._val1 = (num ^ num2) << shift;
			bool flag = false;
			if (num2 >= 5U)
			{
				this._val1 |= 2576980377U >> 28 - shift;
				this.AddOneToDecHex();
				int num3 = this.DecHexLen();
				flag = num3 != this._digitsLen;
				this._decPointPos = this._decPointPos + num3 - this._digitsLen;
				this._digitsLen = num3;
			}
			this.RemoveTrailingZeros();
			return flag;
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x00062221 File Offset: 0x00060421
		private void RemoveTrailingZeros()
		{
			this._offset = this.CountTrailingZeros();
			this._digitsLen -= this._offset;
			if (this._digitsLen == 0)
			{
				this._offset = 0;
				this._decPointPos = 1;
				this._positive = true;
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x00062260 File Offset: 0x00060460
		private void AddOneToDecHex()
		{
			if (this._val1 != 2576980377U)
			{
				this._val1 = NumberFormatter.AddOneToDecHex(this._val1);
				return;
			}
			this._val1 = 0U;
			if (this._val2 != 2576980377U)
			{
				this._val2 = NumberFormatter.AddOneToDecHex(this._val2);
				return;
			}
			this._val2 = 0U;
			if (this._val3 == 2576980377U)
			{
				this._val3 = 0U;
				this._val4 = NumberFormatter.AddOneToDecHex(this._val4);
				return;
			}
			this._val3 = NumberFormatter.AddOneToDecHex(this._val3);
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000622F0 File Offset: 0x000604F0
		private static uint AddOneToDecHex(uint val)
		{
			if ((val & 65535U) == 39321U)
			{
				if ((val & 16777215U) == 10066329U)
				{
					if ((val & 268435455U) == 161061273U)
					{
						return val + 107374183U;
					}
					return val + 6710887U;
				}
				else
				{
					if ((val & 1048575U) == 629145U)
					{
						return val + 419431U;
					}
					return val + 26215U;
				}
			}
			else if ((val & 255U) == 153U)
			{
				if ((val & 4095U) == 2457U)
				{
					return val + 1639U;
				}
				return val + 103U;
			}
			else
			{
				if ((val & 15U) == 9U)
				{
					return val + 7U;
				}
				return val + 1U;
			}
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00062390 File Offset: 0x00060590
		private int CountTrailingZeros()
		{
			if (this._val1 != 0U)
			{
				return NumberFormatter.CountTrailingZeros(this._val1);
			}
			if (this._val2 != 0U)
			{
				return NumberFormatter.CountTrailingZeros(this._val2) + 8;
			}
			if (this._val3 != 0U)
			{
				return NumberFormatter.CountTrailingZeros(this._val3) + 16;
			}
			if (this._val4 != 0U)
			{
				return NumberFormatter.CountTrailingZeros(this._val4) + 24;
			}
			return this._digitsLen;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000623FC File Offset: 0x000605FC
		private static int CountTrailingZeros(uint val)
		{
			if ((val & 65535U) == 0U)
			{
				if ((val & 16777215U) == 0U)
				{
					if ((val & 268435455U) == 0U)
					{
						return 7;
					}
					return 6;
				}
				else
				{
					if ((val & 1048575U) == 0U)
					{
						return 5;
					}
					return 4;
				}
			}
			else if ((val & 255U) == 0U)
			{
				if ((val & 4095U) == 0U)
				{
					return 3;
				}
				return 2;
			}
			else
			{
				if ((val & 15U) == 0U)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00062454 File Offset: 0x00060654
		private static NumberFormatter GetInstance(IFormatProvider fp)
		{
			if (fp != null)
			{
				if (NumberFormatter.userFormatProvider == null)
				{
					Interlocked.CompareExchange<NumberFormatter>(ref NumberFormatter.userFormatProvider, new NumberFormatter(null), null);
				}
				return NumberFormatter.userFormatProvider;
			}
			NumberFormatter numberFormatter = NumberFormatter.threadNumberFormatter;
			NumberFormatter.threadNumberFormatter = null;
			if (numberFormatter == null)
			{
				return new NumberFormatter(Thread.CurrentThread);
			}
			numberFormatter.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			return numberFormatter;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000624AE File Offset: 0x000606AE
		private void Release()
		{
			if (this != NumberFormatter.userFormatProvider)
			{
				NumberFormatter.threadNumberFormatter = this;
			}
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000624C0 File Offset: 0x000606C0
		public static string NumberToString(string format, uint value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, value, 10);
			string text = instance.IntegerToString(format, fp);
			instance.Release();
			return text;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000624EC File Offset: 0x000606EC
		public static string NumberToString(string format, int value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, value, 10);
			string text = instance.IntegerToString(format, fp);
			instance.Release();
			return text;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00062518 File Offset: 0x00060718
		public static string NumberToString(string format, ulong value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, value);
			string text = instance.IntegerToString(format, fp);
			instance.Release();
			return text;
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00062544 File Offset: 0x00060744
		public static string NumberToString(string format, long value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, value);
			string text = instance.IntegerToString(format, fp);
			instance.Release();
			return text;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00062570 File Offset: 0x00060770
		public static string NumberToString(string format, float value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, (double)value, 7);
			NumberFormatInfo numberFormatInstance = instance.GetNumberFormatInstance(fp);
			string text;
			if (instance._NaN)
			{
				text = numberFormatInstance.NaNSymbol;
			}
			else if (instance._infinity)
			{
				if (instance._positive)
				{
					text = numberFormatInstance.PositiveInfinitySymbol;
				}
				else
				{
					text = numberFormatInstance.NegativeInfinitySymbol;
				}
			}
			else if (instance._specifier == 'R')
			{
				text = instance.FormatRoundtrip(value, numberFormatInstance);
			}
			else
			{
				text = instance.NumberToString(format, numberFormatInstance);
			}
			instance.Release();
			return text;
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000625F0 File Offset: 0x000607F0
		public static string NumberToString(string format, double value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, value, 15);
			NumberFormatInfo numberFormatInstance = instance.GetNumberFormatInstance(fp);
			string text;
			if (instance._NaN)
			{
				text = numberFormatInstance.NaNSymbol;
			}
			else if (instance._infinity)
			{
				if (instance._positive)
				{
					text = numberFormatInstance.PositiveInfinitySymbol;
				}
				else
				{
					text = numberFormatInstance.NegativeInfinitySymbol;
				}
			}
			else if (instance._specifier == 'R')
			{
				text = instance.FormatRoundtrip(value, numberFormatInstance);
			}
			else
			{
				text = instance.NumberToString(format, numberFormatInstance);
			}
			instance.Release();
			return text;
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00062670 File Offset: 0x00060870
		public static string NumberToString(string format, decimal value, IFormatProvider fp)
		{
			NumberFormatter instance = NumberFormatter.GetInstance(fp);
			instance.Init(format, value);
			string text = instance.NumberToString(format, instance.GetNumberFormatInstance(fp));
			instance.Release();
			return text;
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000626A0 File Offset: 0x000608A0
		private string IntegerToString(string format, IFormatProvider fp)
		{
			NumberFormatInfo numberFormatInstance = this.GetNumberFormatInstance(fp);
			char specifier = this._specifier;
			if (specifier <= 'N')
			{
				switch (specifier)
				{
				case 'C':
					return this.FormatCurrency(this._precision, numberFormatInstance);
				case 'D':
					return this.FormatDecimal(this._precision, numberFormatInstance);
				case 'E':
					return this.FormatExponential(this._precision, numberFormatInstance);
				case 'F':
					return this.FormatFixedPoint(this._precision, numberFormatInstance);
				case 'G':
					if (this._precision <= 0)
					{
						return this.FormatDecimal(-1, numberFormatInstance);
					}
					return this.FormatGeneral(this._precision, numberFormatInstance);
				default:
					if (specifier == 'N')
					{
						return this.FormatNumber(this._precision, numberFormatInstance);
					}
					break;
				}
			}
			else
			{
				if (specifier == 'P')
				{
					return this.FormatPercent(this._precision, numberFormatInstance);
				}
				if (specifier == 'X')
				{
					return this.FormatHexadecimal(this._precision);
				}
			}
			if (this._isCustomFormat)
			{
				return this.FormatCustom(format, numberFormatInstance);
			}
			throw new FormatException("The specified format '" + format + "' is invalid");
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000627A0 File Offset: 0x000609A0
		private string NumberToString(string format, NumberFormatInfo nfi)
		{
			char specifier = this._specifier;
			if (specifier <= 'N')
			{
				switch (specifier)
				{
				case 'C':
					return this.FormatCurrency(this._precision, nfi);
				case 'D':
					break;
				case 'E':
					return this.FormatExponential(this._precision, nfi);
				case 'F':
					return this.FormatFixedPoint(this._precision, nfi);
				case 'G':
					return this.FormatGeneral(this._precision, nfi);
				default:
					if (specifier == 'N')
					{
						return this.FormatNumber(this._precision, nfi);
					}
					break;
				}
			}
			else
			{
				if (specifier == 'P')
				{
					return this.FormatPercent(this._precision, nfi);
				}
				if (specifier != 'X')
				{
				}
			}
			if (this._isCustomFormat)
			{
				return this.FormatCustom(format, nfi);
			}
			throw new FormatException("The specified format '" + format + "' is invalid");
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x00062864 File Offset: 0x00060A64
		private string FormatCurrency(int precision, NumberFormatInfo nfi)
		{
			precision = ((precision >= 0) ? precision : nfi.CurrencyDecimalDigits);
			this.RoundDecimal(precision);
			this.ResetCharBuf(this.IntegerDigits * 2 + precision * 2 + 16);
			if (this._positive)
			{
				int num = nfi.CurrencyPositivePattern;
				if (num != 0)
				{
					if (num == 2)
					{
						this.Append(nfi.CurrencySymbol);
						this.Append(' ');
					}
				}
				else
				{
					this.Append(nfi.CurrencySymbol);
				}
			}
			else
			{
				switch (nfi.CurrencyNegativePattern)
				{
				case 0:
					this.Append('(');
					this.Append(nfi.CurrencySymbol);
					break;
				case 1:
					this.Append(nfi.NegativeSign);
					this.Append(nfi.CurrencySymbol);
					break;
				case 2:
					this.Append(nfi.CurrencySymbol);
					this.Append(nfi.NegativeSign);
					break;
				case 3:
					this.Append(nfi.CurrencySymbol);
					break;
				case 4:
					this.Append('(');
					break;
				case 5:
					this.Append(nfi.NegativeSign);
					break;
				case 8:
					this.Append(nfi.NegativeSign);
					break;
				case 9:
					this.Append(nfi.NegativeSign);
					this.Append(nfi.CurrencySymbol);
					this.Append(' ');
					break;
				case 11:
					this.Append(nfi.CurrencySymbol);
					this.Append(' ');
					break;
				case 12:
					this.Append(nfi.CurrencySymbol);
					this.Append(' ');
					this.Append(nfi.NegativeSign);
					break;
				case 14:
					this.Append('(');
					this.Append(nfi.CurrencySymbol);
					this.Append(' ');
					break;
				case 15:
					this.Append('(');
					break;
				}
			}
			this.AppendIntegerStringWithGroupSeparator(nfi.CurrencyGroupSizes, nfi.CurrencyGroupSeparator);
			if (precision > 0)
			{
				this.Append(nfi.CurrencyDecimalSeparator);
				this.AppendDecimalString(precision);
			}
			if (this._positive)
			{
				int num = nfi.CurrencyPositivePattern;
				if (num != 1)
				{
					if (num == 3)
					{
						this.Append(' ');
						this.Append(nfi.CurrencySymbol);
					}
				}
				else
				{
					this.Append(nfi.CurrencySymbol);
				}
			}
			else
			{
				switch (nfi.CurrencyNegativePattern)
				{
				case 0:
					this.Append(')');
					break;
				case 3:
					this.Append(nfi.NegativeSign);
					break;
				case 4:
					this.Append(nfi.CurrencySymbol);
					this.Append(')');
					break;
				case 5:
					this.Append(nfi.CurrencySymbol);
					break;
				case 6:
					this.Append(nfi.NegativeSign);
					this.Append(nfi.CurrencySymbol);
					break;
				case 7:
					this.Append(nfi.CurrencySymbol);
					this.Append(nfi.NegativeSign);
					break;
				case 8:
					this.Append(' ');
					this.Append(nfi.CurrencySymbol);
					break;
				case 10:
					this.Append(' ');
					this.Append(nfi.CurrencySymbol);
					this.Append(nfi.NegativeSign);
					break;
				case 11:
					this.Append(nfi.NegativeSign);
					break;
				case 13:
					this.Append(nfi.NegativeSign);
					this.Append(' ');
					this.Append(nfi.CurrencySymbol);
					break;
				case 14:
					this.Append(')');
					break;
				case 15:
					this.Append(' ');
					this.Append(nfi.CurrencySymbol);
					this.Append(')');
					break;
				}
			}
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x00062C2C File Offset: 0x00060E2C
		private string FormatDecimal(int precision, NumberFormatInfo nfi)
		{
			if (precision < this._digitsLen)
			{
				precision = this._digitsLen;
			}
			if (precision == 0)
			{
				return "0";
			}
			this.ResetCharBuf(precision + 1);
			if (!this._positive)
			{
				this.Append(nfi.NegativeSign);
			}
			this.AppendDigits(0, precision);
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00062C8C File Offset: 0x00060E8C
		private unsafe string FormatHexadecimal(int precision)
		{
			int i = Math.Max(precision, this._decPointPos);
			char* ptr = (this._specifierIsUpper ? NumberFormatter.DigitUpperTable : NumberFormatter.DigitLowerTable);
			this.ResetCharBuf(i);
			this._ind = i;
			ulong num = (ulong)this._val1 | ((ulong)this._val2 << 32);
			while (i > 0)
			{
				this._cbuf[--i] = ptr[(num & 15UL) * 2UL / 2UL];
				num >>= 4;
			}
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00062D10 File Offset: 0x00060F10
		private string FormatFixedPoint(int precision, NumberFormatInfo nfi)
		{
			if (precision == -1)
			{
				precision = nfi.NumberDecimalDigits;
			}
			this.RoundDecimal(precision);
			this.ResetCharBuf(this.IntegerDigits + precision + 2);
			if (!this._positive)
			{
				this.Append(nfi.NegativeSign);
			}
			this.AppendIntegerString(this.IntegerDigits);
			if (precision > 0)
			{
				this.Append(nfi.NumberDecimalSeparator);
				this.AppendDecimalString(precision);
			}
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00062D8C File Offset: 0x00060F8C
		private string FormatRoundtrip(double origval, NumberFormatInfo nfi)
		{
			NumberFormatter clone = this.GetClone();
			if (origval >= -1.79769313486231E+308 && origval <= 1.79769313486231E+308)
			{
				string text = this.FormatGeneral(this._defPrecision, nfi);
				if (origval == double.Parse(text, nfi))
				{
					return text;
				}
			}
			return clone.FormatGeneral(this._defPrecision + 2, nfi);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00062DE4 File Offset: 0x00060FE4
		private string FormatRoundtrip(float origval, NumberFormatInfo nfi)
		{
			NumberFormatter clone = this.GetClone();
			string text = this.FormatGeneral(this._defPrecision, nfi);
			if (origval == float.Parse(text, nfi))
			{
				return text;
			}
			return clone.FormatGeneral(this._defPrecision + 2, nfi);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00062E24 File Offset: 0x00061024
		private string FormatGeneral(int precision, NumberFormatInfo nfi)
		{
			bool flag;
			if (precision == -1)
			{
				flag = this.IsFloatingSource;
				precision = this._defPrecision;
			}
			else
			{
				flag = true;
				if (precision == 0)
				{
					precision = this._defPrecision;
				}
				this.RoundPos(precision);
			}
			int num = this._decPointPos;
			int digitsLen = this._digitsLen;
			int num2 = digitsLen - num;
			if ((num > precision || num <= -4) && flag)
			{
				return this.FormatExponential(digitsLen - 1, nfi, 2);
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num < 0)
			{
				num = 0;
			}
			this.ResetCharBuf(num2 + num + 3);
			if (!this._positive)
			{
				this.Append(nfi.NegativeSign);
			}
			if (num == 0)
			{
				this.Append('0');
			}
			else
			{
				this.AppendDigits(digitsLen - num, digitsLen);
			}
			if (num2 > 0)
			{
				this.Append(nfi.NumberDecimalSeparator);
				this.AppendDigits(0, num2);
			}
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00062EF8 File Offset: 0x000610F8
		private string FormatNumber(int precision, NumberFormatInfo nfi)
		{
			precision = ((precision >= 0) ? precision : nfi.NumberDecimalDigits);
			this.ResetCharBuf(this.IntegerDigits * 3 + precision);
			this.RoundDecimal(precision);
			if (!this._positive)
			{
				switch (nfi.NumberNegativePattern)
				{
				case 0:
					this.Append('(');
					break;
				case 1:
					this.Append(nfi.NegativeSign);
					break;
				case 2:
					this.Append(nfi.NegativeSign);
					this.Append(' ');
					break;
				}
			}
			this.AppendIntegerStringWithGroupSeparator(nfi.NumberGroupSizes, nfi.NumberGroupSeparator);
			if (precision > 0)
			{
				this.Append(nfi.NumberDecimalSeparator);
				this.AppendDecimalString(precision);
			}
			if (!this._positive)
			{
				switch (nfi.NumberNegativePattern)
				{
				case 0:
					this.Append(')');
					break;
				case 3:
					this.Append(nfi.NegativeSign);
					break;
				case 4:
					this.Append(' ');
					this.Append(nfi.NegativeSign);
					break;
				}
			}
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00063010 File Offset: 0x00061210
		private string FormatPercent(int precision, NumberFormatInfo nfi)
		{
			precision = ((precision >= 0) ? precision : nfi.PercentDecimalDigits);
			this.Multiply10(2);
			this.RoundDecimal(precision);
			this.ResetCharBuf(this.IntegerDigits * 2 + precision + 16);
			if (this._positive)
			{
				if (nfi.PercentPositivePattern == 2)
				{
					this.Append(nfi.PercentSymbol);
				}
			}
			else
			{
				switch (nfi.PercentNegativePattern)
				{
				case 0:
					this.Append(nfi.NegativeSign);
					break;
				case 1:
					this.Append(nfi.NegativeSign);
					break;
				case 2:
					this.Append(nfi.NegativeSign);
					this.Append(nfi.PercentSymbol);
					break;
				}
			}
			this.AppendIntegerStringWithGroupSeparator(nfi.PercentGroupSizes, nfi.PercentGroupSeparator);
			if (precision > 0)
			{
				this.Append(nfi.PercentDecimalSeparator);
				this.AppendDecimalString(precision);
			}
			if (this._positive)
			{
				int num = nfi.PercentPositivePattern;
				if (num != 0)
				{
					if (num == 1)
					{
						this.Append(nfi.PercentSymbol);
					}
				}
				else
				{
					this.Append(' ');
					this.Append(nfi.PercentSymbol);
				}
			}
			else
			{
				int num = nfi.PercentNegativePattern;
				if (num != 0)
				{
					if (num == 1)
					{
						this.Append(nfi.PercentSymbol);
					}
				}
				else
				{
					this.Append(' ');
					this.Append(nfi.PercentSymbol);
				}
			}
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00063165 File Offset: 0x00061365
		private string FormatExponential(int precision, NumberFormatInfo nfi)
		{
			if (precision == -1)
			{
				precision = 6;
			}
			this.RoundPos(precision + 1);
			return this.FormatExponential(precision, nfi, 3);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00063180 File Offset: 0x00061380
		private string FormatExponential(int precision, NumberFormatInfo nfi, int expDigits)
		{
			int decPointPos = this._decPointPos;
			int digitsLen = this._digitsLen;
			int num = decPointPos - 1;
			this._decPointPos = 1;
			this.ResetCharBuf(precision + 8);
			if (!this._positive)
			{
				this.Append(nfi.NegativeSign);
			}
			this.AppendOneDigit(digitsLen - 1);
			if (precision > 0)
			{
				this.Append(nfi.NumberDecimalSeparator);
				this.AppendDigits(digitsLen - precision - 1, digitsLen - this._decPointPos);
			}
			this.AppendExponent(nfi, num, expDigits);
			return new string(this._cbuf, 0, this._ind);
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00063208 File Offset: 0x00061408
		private string FormatCustom(string format, NumberFormatInfo nfi)
		{
			bool positive = this._positive;
			int num = 0;
			int num2 = 0;
			NumberFormatter.CustomInfo.GetActiveSection(format, ref positive, this.IsZero, ref num, ref num2);
			if (num2 != 0)
			{
				this._positive = positive;
				NumberFormatter.CustomInfo customInfo = NumberFormatter.CustomInfo.Parse(format, num, num2, nfi);
				StringBuilder stringBuilder = new StringBuilder(customInfo.IntegerDigits * 2);
				StringBuilder stringBuilder2 = new StringBuilder(customInfo.DecimalDigits * 2);
				StringBuilder stringBuilder3 = (customInfo.UseExponent ? new StringBuilder(customInfo.ExponentDigits * 2) : null);
				int num3 = 0;
				if (customInfo.Percents > 0)
				{
					this.Multiply10(2 * customInfo.Percents);
				}
				if (customInfo.Permilles > 0)
				{
					this.Multiply10(3 * customInfo.Permilles);
				}
				if (customInfo.DividePlaces > 0)
				{
					this.Divide10(customInfo.DividePlaces);
				}
				bool flag = true;
				if (customInfo.UseExponent && (customInfo.DecimalDigits > 0 || customInfo.IntegerDigits > 0))
				{
					if (!this.IsZero)
					{
						this.RoundPos(customInfo.DecimalDigits + customInfo.IntegerDigits);
						num3 -= this._decPointPos - customInfo.IntegerDigits;
						this._decPointPos = customInfo.IntegerDigits;
					}
					flag = num3 <= 0;
					NumberFormatter.AppendNonNegativeNumber(stringBuilder3, (num3 < 0) ? (-num3) : num3);
				}
				else
				{
					this.RoundDecimal(customInfo.DecimalDigits);
				}
				if (customInfo.IntegerDigits != 0 || !this.IsZeroInteger)
				{
					this.AppendIntegerString(this.IntegerDigits, stringBuilder);
				}
				this.AppendDecimalString(this.DecimalDigits, stringBuilder2);
				if (customInfo.UseExponent)
				{
					if (customInfo.DecimalDigits <= 0 && customInfo.IntegerDigits <= 0)
					{
						this._positive = true;
					}
					if (stringBuilder.Length < customInfo.IntegerDigits)
					{
						stringBuilder.Insert(0, "0", customInfo.IntegerDigits - stringBuilder.Length);
					}
					while (stringBuilder3.Length < customInfo.ExponentDigits - customInfo.ExponentTailSharpDigits)
					{
						stringBuilder3.Insert(0, '0');
					}
					if (flag && !customInfo.ExponentNegativeSignOnly)
					{
						stringBuilder3.Insert(0, nfi.PositiveSign);
					}
					else if (!flag)
					{
						stringBuilder3.Insert(0, nfi.NegativeSign);
					}
				}
				else
				{
					if (stringBuilder.Length < customInfo.IntegerDigits - customInfo.IntegerHeadSharpDigits)
					{
						stringBuilder.Insert(0, "0", customInfo.IntegerDigits - customInfo.IntegerHeadSharpDigits - stringBuilder.Length);
					}
					if (customInfo.IntegerDigits == customInfo.IntegerHeadSharpDigits && NumberFormatter.IsZeroOnly(stringBuilder))
					{
						stringBuilder.Remove(0, stringBuilder.Length);
					}
				}
				NumberFormatter.ZeroTrimEnd(stringBuilder2, true);
				while (stringBuilder2.Length < customInfo.DecimalDigits - customInfo.DecimalTailSharpDigits)
				{
					stringBuilder2.Append('0');
				}
				if (stringBuilder2.Length > customInfo.DecimalDigits)
				{
					stringBuilder2.Remove(customInfo.DecimalDigits, stringBuilder2.Length - customInfo.DecimalDigits);
				}
				return customInfo.Format(format, num, num2, nfi, this._positive, stringBuilder, stringBuilder2, stringBuilder3);
			}
			if (!this._positive)
			{
				return nfi.NegativeSign;
			}
			return string.Empty;
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000634F8 File Offset: 0x000616F8
		private static void ZeroTrimEnd(StringBuilder sb, bool canEmpty)
		{
			int num = 0;
			int num2 = sb.Length - 1;
			while ((canEmpty ? (num2 >= 0) : (num2 > 0)) && sb[num2] == '0')
			{
				num++;
				num2--;
			}
			if (num > 0)
			{
				sb.Remove(sb.Length - num, num);
			}
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0006354C File Offset: 0x0006174C
		private static bool IsZeroOnly(StringBuilder sb)
		{
			for (int i = 0; i < sb.Length; i++)
			{
				if (char.IsDigit(sb[i]) && sb[i] != '0')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x00063588 File Offset: 0x00061788
		private static void AppendNonNegativeNumber(StringBuilder sb, int v)
		{
			if (v < 0)
			{
				throw new ArgumentException();
			}
			int num = NumberFormatter.ScaleOrder((long)v) - 1;
			do
			{
				int num2 = v / (int)NumberFormatter.GetTenPowerOf(num);
				sb.Append((char)(48 | num2));
				v -= (int)NumberFormatter.GetTenPowerOf(num--) * num2;
			}
			while (num >= 0);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000635D4 File Offset: 0x000617D4
		private void AppendIntegerString(int minLength, StringBuilder sb)
		{
			if (this._decPointPos <= 0)
			{
				sb.Append('0', minLength);
				return;
			}
			if (this._decPointPos < minLength)
			{
				sb.Append('0', minLength - this._decPointPos);
			}
			this.AppendDigits(this._digitsLen - this._decPointPos, this._digitsLen, sb);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0006362C File Offset: 0x0006182C
		private void AppendIntegerString(int minLength)
		{
			if (this._decPointPos <= 0)
			{
				this.Append('0', minLength);
				return;
			}
			if (this._decPointPos < minLength)
			{
				this.Append('0', minLength - this._decPointPos);
			}
			this.AppendDigits(this._digitsLen - this._decPointPos, this._digitsLen);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0006367E File Offset: 0x0006187E
		private void AppendDecimalString(int precision, StringBuilder sb)
		{
			this.AppendDigits(this._digitsLen - precision - this._decPointPos, this._digitsLen - this._decPointPos, sb);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000636A3 File Offset: 0x000618A3
		private void AppendDecimalString(int precision)
		{
			this.AppendDigits(this._digitsLen - precision - this._decPointPos, this._digitsLen - this._decPointPos);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000636C8 File Offset: 0x000618C8
		private void AppendIntegerStringWithGroupSeparator(int[] groups, string groupSeparator)
		{
			if (this.IsZeroInteger)
			{
				this.Append('0');
				return;
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < groups.Length; i++)
			{
				num += groups[i];
				if (num > this._decPointPos)
				{
					break;
				}
				num2 = i;
			}
			if (groups.Length != 0 && num > 0)
			{
				int num3 = groups[num2];
				int num4 = ((this._decPointPos > num) ? (this._decPointPos - num) : 0);
				if (num3 == 0)
				{
					while (num2 >= 0 && groups[num2] == 0)
					{
						num2--;
					}
					num3 = ((num4 > 0) ? num4 : groups[num2]);
				}
				int num5;
				if (num4 == 0)
				{
					num5 = num3;
				}
				else
				{
					num2 += num4 / num3;
					num5 = num4 % num3;
					if (num5 == 0)
					{
						num5 = num3;
					}
					else
					{
						num2++;
					}
				}
				if (num >= this._decPointPos)
				{
					int num6 = groups[0];
					if (num > num6)
					{
						int num7 = -(num6 - this._decPointPos);
						int num8;
						if (num7 < num6)
						{
							num5 = num7;
						}
						else if (num6 > 0 && (num8 = this._decPointPos % num6) > 0)
						{
							num5 = num8;
						}
					}
				}
				int num9 = 0;
				while (this._decPointPos - num9 > num5 && num5 != 0)
				{
					this.AppendDigits(this._digitsLen - num9 - num5, this._digitsLen - num9);
					num9 += num5;
					this.Append(groupSeparator);
					if (--num2 < groups.Length && num2 >= 0)
					{
						num3 = groups[num2];
					}
					num5 = num3;
				}
				this.AppendDigits(this._digitsLen - this._decPointPos, this._digitsLen - num9);
				return;
			}
			this.AppendDigits(this._digitsLen - this._decPointPos, this._digitsLen);
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00063840 File Offset: 0x00061A40
		private void AppendExponent(NumberFormatInfo nfi, int exponent, int minDigits)
		{
			if (this._specifierIsUpper || this._specifier == 'R')
			{
				this.Append('E');
			}
			else
			{
				this.Append('e');
			}
			if (exponent >= 0)
			{
				this.Append(nfi.PositiveSign);
			}
			else
			{
				this.Append(nfi.NegativeSign);
				exponent = -exponent;
			}
			if (exponent == 0)
			{
				this.Append('0', minDigits);
				return;
			}
			if (exponent < 10)
			{
				this.Append('0', minDigits - 1);
				this.Append((char)(48 | exponent));
				return;
			}
			uint num = NumberFormatter.FastToDecHex(exponent);
			if (exponent >= 100 || minDigits == 3)
			{
				this.Append((char)(48U | (num >> 8)));
			}
			this.Append((char)(48U | ((num >> 4) & 15U)));
			this.Append((char)(48U | (num & 15U)));
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x000638F8 File Offset: 0x00061AF8
		private void AppendOneDigit(int start)
		{
			if (this._ind == this._cbuf.Length)
			{
				this.Resize(this._ind + 10);
			}
			start += this._offset;
			uint num;
			if (start < 0)
			{
				num = 0U;
			}
			else if (start < 8)
			{
				num = this._val1;
			}
			else if (start < 16)
			{
				num = this._val2;
			}
			else if (start < 24)
			{
				num = this._val3;
			}
			else if (start < 32)
			{
				num = this._val4;
			}
			else
			{
				num = 0U;
			}
			num >>= (start & 7) << 2;
			char[] cbuf = this._cbuf;
			int ind = this._ind;
			this._ind = ind + 1;
			cbuf[ind] = (ushort)(48U | (num & 15U));
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0006399C File Offset: 0x00061B9C
		private void AppendDigits(int start, int end)
		{
			if (start >= end)
			{
				return;
			}
			int num = this._ind + (end - start);
			if (num > this._cbuf.Length)
			{
				this.Resize(num + 10);
			}
			this._ind = num;
			end += this._offset;
			start += this._offset;
			int num2 = start + 8 - (start & 7);
			for (;;)
			{
				uint num3;
				if (num2 == 8)
				{
					num3 = this._val1;
				}
				else if (num2 == 16)
				{
					num3 = this._val2;
				}
				else if (num2 == 24)
				{
					num3 = this._val3;
				}
				else if (num2 == 32)
				{
					num3 = this._val4;
				}
				else
				{
					num3 = 0U;
				}
				num3 >>= (start & 7) << 2;
				if (num2 > end)
				{
					num2 = end;
				}
				this._cbuf[--num] = (char)(48U | (num3 & 15U));
				switch (num2 - start)
				{
				case 1:
					goto IL_017F;
				case 2:
					goto IL_0167;
				case 3:
					goto IL_014F;
				case 4:
					goto IL_0137;
				case 5:
					goto IL_011F;
				case 6:
					goto IL_0107;
				case 7:
					goto IL_00EF;
				case 8:
					this._cbuf[--num] = (char)(48U | ((num3 >>= 4) & 15U));
					goto IL_00EF;
				}
				IL_0184:
				start = num2;
				num2 += 8;
				continue;
				IL_017F:
				if (num2 == end)
				{
					break;
				}
				goto IL_0184;
				IL_0167:
				this._cbuf[--num] = (char)(48U | ((num3 >> 4) & 15U));
				goto IL_017F;
				IL_014F:
				this._cbuf[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_0167;
				IL_0137:
				this._cbuf[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_014F;
				IL_011F:
				this._cbuf[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_0137;
				IL_0107:
				this._cbuf[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_011F;
				IL_00EF:
				this._cbuf[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_0107;
			}
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00063B38 File Offset: 0x00061D38
		private void AppendDigits(int start, int end, StringBuilder sb)
		{
			if (start >= end)
			{
				return;
			}
			int num = sb.Length + (end - start);
			sb.Length = num;
			end += this._offset;
			start += this._offset;
			int num2 = start + 8 - (start & 7);
			for (;;)
			{
				uint num3;
				if (num2 == 8)
				{
					num3 = this._val1;
				}
				else if (num2 == 16)
				{
					num3 = this._val2;
				}
				else if (num2 == 24)
				{
					num3 = this._val3;
				}
				else if (num2 == 32)
				{
					num3 = this._val4;
				}
				else
				{
					num3 = 0U;
				}
				num3 >>= (start & 7) << 2;
				if (num2 > end)
				{
					num2 = end;
				}
				sb[--num] = (char)(48U | (num3 & 15U));
				switch (num2 - start)
				{
				case 1:
					goto IL_0162;
				case 2:
					goto IL_014B;
				case 3:
					goto IL_0134;
				case 4:
					goto IL_011D;
				case 5:
					goto IL_0106;
				case 6:
					goto IL_00EF;
				case 7:
					goto IL_00D8;
				case 8:
					sb[--num] = (char)(48U | ((num3 >>= 4) & 15U));
					goto IL_00D8;
				}
				IL_0167:
				start = num2;
				num2 += 8;
				continue;
				IL_0162:
				if (num2 == end)
				{
					break;
				}
				goto IL_0167;
				IL_014B:
				sb[--num] = (char)(48U | ((num3 >> 4) & 15U));
				goto IL_0162;
				IL_0134:
				sb[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_014B;
				IL_011D:
				sb[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_0134;
				IL_0106:
				sb[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_011D;
				IL_00EF:
				sb[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_0106;
				IL_00D8:
				sb[--num] = (char)(48U | ((num3 >>= 4) & 15U));
				goto IL_00EF;
			}
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00063CB7 File Offset: 0x00061EB7
		private void Multiply10(int count)
		{
			if (count <= 0 || this._digitsLen == 0)
			{
				return;
			}
			this._decPointPos += count;
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00063CD4 File Offset: 0x00061ED4
		private void Divide10(int count)
		{
			if (count <= 0 || this._digitsLen == 0)
			{
				return;
			}
			this._decPointPos -= count;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x00063CF1 File Offset: 0x00061EF1
		private NumberFormatter GetClone()
		{
			return (NumberFormatter)base.MemberwiseClone();
		}

		// Token: 0x04001734 RID: 5940
		private const int DefaultExpPrecision = 6;

		// Token: 0x04001735 RID: 5941
		private const int HundredMillion = 100000000;

		// Token: 0x04001736 RID: 5942
		private const long SeventeenDigitsThreshold = 10000000000000000L;

		// Token: 0x04001737 RID: 5943
		private const ulong ULongDivHundredMillion = 184467440737UL;

		// Token: 0x04001738 RID: 5944
		private const ulong ULongModHundredMillion = 9551616UL;

		// Token: 0x04001739 RID: 5945
		private const int DoubleBitsExponentShift = 52;

		// Token: 0x0400173A RID: 5946
		private const int DoubleBitsExponentMask = 2047;

		// Token: 0x0400173B RID: 5947
		private const long DoubleBitsMantissaMask = 4503599627370495L;

		// Token: 0x0400173C RID: 5948
		private const int DecimalBitsScaleMask = 2031616;

		// Token: 0x0400173D RID: 5949
		private const int SingleDefPrecision = 7;

		// Token: 0x0400173E RID: 5950
		private const int DoubleDefPrecision = 15;

		// Token: 0x0400173F RID: 5951
		private const int Int32DefPrecision = 10;

		// Token: 0x04001740 RID: 5952
		private const int UInt32DefPrecision = 10;

		// Token: 0x04001741 RID: 5953
		private const int Int64DefPrecision = 19;

		// Token: 0x04001742 RID: 5954
		private const int UInt64DefPrecision = 20;

		// Token: 0x04001743 RID: 5955
		private const int DecimalDefPrecision = 100;

		// Token: 0x04001744 RID: 5956
		private const int TenPowersListLength = 19;

		// Token: 0x04001745 RID: 5957
		private const double MinRoundtripVal = -1.79769313486231E+308;

		// Token: 0x04001746 RID: 5958
		private const double MaxRoundtripVal = 1.79769313486231E+308;

		// Token: 0x04001747 RID: 5959
		private unsafe static readonly ulong* MantissaBitsTable;

		// Token: 0x04001748 RID: 5960
		private unsafe static readonly int* TensExponentTable;

		// Token: 0x04001749 RID: 5961
		private unsafe static readonly char* DigitLowerTable;

		// Token: 0x0400174A RID: 5962
		private unsafe static readonly char* DigitUpperTable;

		// Token: 0x0400174B RID: 5963
		private unsafe static readonly long* TenPowersList;

		// Token: 0x0400174C RID: 5964
		private unsafe static readonly int* DecHexDigits;

		// Token: 0x0400174D RID: 5965
		private NumberFormatInfo _nfi;

		// Token: 0x0400174E RID: 5966
		private char[] _cbuf;

		// Token: 0x0400174F RID: 5967
		private bool _NaN;

		// Token: 0x04001750 RID: 5968
		private bool _infinity;

		// Token: 0x04001751 RID: 5969
		private bool _isCustomFormat;

		// Token: 0x04001752 RID: 5970
		private bool _specifierIsUpper;

		// Token: 0x04001753 RID: 5971
		private bool _positive;

		// Token: 0x04001754 RID: 5972
		private char _specifier;

		// Token: 0x04001755 RID: 5973
		private int _precision;

		// Token: 0x04001756 RID: 5974
		private int _defPrecision;

		// Token: 0x04001757 RID: 5975
		private int _digitsLen;

		// Token: 0x04001758 RID: 5976
		private int _offset;

		// Token: 0x04001759 RID: 5977
		private int _decPointPos;

		// Token: 0x0400175A RID: 5978
		private uint _val1;

		// Token: 0x0400175B RID: 5979
		private uint _val2;

		// Token: 0x0400175C RID: 5980
		private uint _val3;

		// Token: 0x0400175D RID: 5981
		private uint _val4;

		// Token: 0x0400175E RID: 5982
		private int _ind;

		// Token: 0x0400175F RID: 5983
		[ThreadStatic]
		private static NumberFormatter threadNumberFormatter;

		// Token: 0x04001760 RID: 5984
		[ThreadStatic]
		private static NumberFormatter userFormatProvider;

		// Token: 0x02000246 RID: 582
		private class CustomInfo
		{
			// Token: 0x06001ADC RID: 6876 RVA: 0x00063D00 File Offset: 0x00061F00
			public static void GetActiveSection(string format, ref bool positive, bool zero, ref int offset, ref int length)
			{
				int[] array = new int[3];
				int num = 0;
				int num2 = 0;
				bool flag = false;
				for (int i = 0; i < format.Length; i++)
				{
					char c = format[i];
					if (c == '"' || c == '\'')
					{
						if (i == 0 || format[i - 1] != '\\')
						{
							flag = !flag;
						}
					}
					else if (c == ';' && !flag && (i == 0 || format[i - 1] != '\\'))
					{
						array[num++] = i - num2;
						num2 = i + 1;
						if (num == 3)
						{
							break;
						}
					}
				}
				if (num == 0)
				{
					offset = 0;
					length = format.Length;
					return;
				}
				if (num == 1)
				{
					if (positive || zero)
					{
						offset = 0;
						length = array[0];
						return;
					}
					if (array[0] + 1 < format.Length)
					{
						positive = true;
						offset = array[0] + 1;
						length = format.Length - offset;
						return;
					}
					offset = 0;
					length = array[0];
					return;
				}
				else if (zero)
				{
					if (num == 2)
					{
						if (format.Length - num2 == 0)
						{
							offset = 0;
							length = array[0];
							return;
						}
						offset = array[0] + array[1] + 2;
						length = format.Length - offset;
						return;
					}
					else
					{
						if (array[2] == 0)
						{
							offset = 0;
							length = array[0];
							return;
						}
						offset = array[0] + array[1] + 2;
						length = array[2];
						return;
					}
				}
				else
				{
					if (positive)
					{
						offset = 0;
						length = array[0];
						return;
					}
					if (array[1] > 0)
					{
						positive = true;
						offset = array[0] + 1;
						length = array[1];
						return;
					}
					offset = 0;
					length = array[0];
					return;
				}
			}

			// Token: 0x06001ADD RID: 6877 RVA: 0x00063E64 File Offset: 0x00062064
			public static NumberFormatter.CustomInfo Parse(string format, int offset, int length, NumberFormatInfo nfi)
			{
				char c = '\0';
				bool flag = true;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = true;
				NumberFormatter.CustomInfo customInfo = new NumberFormatter.CustomInfo();
				int num = 0;
				int num2 = offset;
				while (num2 - offset < length)
				{
					char c2 = format[num2];
					if (c2 == c && c2 != '\0')
					{
						c = '\0';
					}
					else if (c == '\0')
					{
						if (flag3 && c2 != '\0' && c2 != '0' && c2 != '#')
						{
							flag3 = false;
							flag = customInfo.DecimalPointPos < 0;
							flag2 = !flag;
							num2--;
						}
						else
						{
							if (c2 <= 'E')
							{
								switch (c2)
								{
								case '"':
								case '\'':
									if (c2 == '"' || c2 == '\'')
									{
										c = c2;
										goto IL_0292;
									}
									goto IL_0292;
								case '#':
									if (flag4 && flag)
									{
										customInfo.IntegerHeadSharpDigits++;
									}
									else if (flag2)
									{
										customInfo.DecimalTailSharpDigits++;
									}
									else if (flag3)
									{
										customInfo.ExponentTailSharpDigits++;
									}
									break;
								case '$':
								case '&':
									goto IL_0292;
								case '%':
									customInfo.Percents++;
									goto IL_0292;
								default:
									switch (c2)
									{
									case ',':
										if (flag && customInfo.IntegerDigits > 0)
										{
											num++;
											goto IL_0292;
										}
										goto IL_0292;
									case '-':
									case '/':
										goto IL_0292;
									case '.':
										flag = false;
										flag2 = true;
										flag3 = false;
										if (customInfo.DecimalPointPos == -1)
										{
											customInfo.DecimalPointPos = num2;
											goto IL_0292;
										}
										goto IL_0292;
									case '0':
										break;
									default:
										if (c2 != 'E')
										{
											goto IL_0292;
										}
										goto IL_01CC;
									}
									break;
								}
								if (c2 != '#')
								{
									flag4 = false;
									if (flag2)
									{
										customInfo.DecimalTailSharpDigits = 0;
									}
									else if (flag3)
									{
										customInfo.ExponentTailSharpDigits = 0;
									}
								}
								if (customInfo.IntegerHeadPos == -1)
								{
									customInfo.IntegerHeadPos = num2;
								}
								if (flag)
								{
									customInfo.IntegerDigits++;
									if (num > 0)
									{
										customInfo.UseGroup = true;
									}
									num = 0;
									goto IL_0292;
								}
								if (flag2)
								{
									customInfo.DecimalDigits++;
									goto IL_0292;
								}
								if (flag3)
								{
									customInfo.ExponentDigits++;
									goto IL_0292;
								}
								goto IL_0292;
							}
							else
							{
								if (c2 == '\\')
								{
									num2++;
									goto IL_0292;
								}
								if (c2 != 'e')
								{
									if (c2 != '‰')
									{
										goto IL_0292;
									}
									customInfo.Permilles++;
									goto IL_0292;
								}
							}
							IL_01CC:
							if (!customInfo.UseExponent)
							{
								customInfo.UseExponent = true;
								flag = false;
								flag2 = false;
								flag3 = true;
								if (num2 + 1 - offset < length)
								{
									char c3 = format[num2 + 1];
									if (c3 == '+')
									{
										customInfo.ExponentNegativeSignOnly = false;
									}
									if (c3 == '+' || c3 == '-')
									{
										num2++;
									}
									else if (c3 != '0' && c3 != '#')
									{
										customInfo.UseExponent = false;
										if (customInfo.DecimalPointPos < 0)
										{
											flag = true;
										}
									}
								}
							}
						}
					}
					IL_0292:
					num2++;
				}
				if (customInfo.ExponentDigits == 0)
				{
					customInfo.UseExponent = false;
				}
				else
				{
					customInfo.IntegerHeadSharpDigits = 0;
				}
				if (customInfo.DecimalDigits == 0)
				{
					customInfo.DecimalPointPos = -1;
				}
				customInfo.DividePlaces += num * 3;
				return customInfo;
			}

			// Token: 0x06001ADE RID: 6878 RVA: 0x00064154 File Offset: 0x00062354
			public string Format(string format, int offset, int length, NumberFormatInfo nfi, bool positive, StringBuilder sb_int, StringBuilder sb_dec, StringBuilder sb_exp)
			{
				StringBuilder stringBuilder = new StringBuilder();
				char c = '\0';
				bool flag = true;
				bool flag2 = false;
				int num = 0;
				int i = 0;
				int num2 = 0;
				int[] numberGroupSizes = nfi.NumberGroupSizes;
				string numberGroupSeparator = nfi.NumberGroupSeparator;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				if (this.UseGroup && numberGroupSizes.Length != 0)
				{
					num3 = sb_int.Length;
					for (int j = 0; j < numberGroupSizes.Length; j++)
					{
						num4 += numberGroupSizes[j];
						if (num4 <= num3)
						{
							num5 = j;
						}
					}
					num7 = numberGroupSizes[num5];
					int num8 = ((num3 > num4) ? (num3 - num4) : 0);
					if (num7 == 0)
					{
						while (num5 >= 0 && numberGroupSizes[num5] == 0)
						{
							num5--;
						}
						num7 = ((num8 > 0) ? num8 : numberGroupSizes[num5]);
					}
					if (num8 == 0)
					{
						num6 = num7;
					}
					else
					{
						num5 += num8 / num7;
						num6 = num8 % num7;
						if (num6 == 0)
						{
							num6 = num7;
						}
						else
						{
							num5++;
						}
					}
				}
				else
				{
					this.UseGroup = false;
				}
				int num9 = offset;
				while (num9 - offset < length)
				{
					char c2 = format[num9];
					if (c2 == c && c2 != '\0')
					{
						c = '\0';
					}
					else if (c != '\0')
					{
						stringBuilder.Append(c2);
					}
					else
					{
						if (c2 <= 'E')
						{
							switch (c2)
							{
							case '"':
							case '\'':
								if (c2 == '"' || c2 == '\'')
								{
									c = c2;
									goto IL_03CC;
								}
								goto IL_03CC;
							case '#':
								break;
							case '$':
							case '&':
								goto IL_03C3;
							case '%':
								stringBuilder.Append(nfi.PercentSymbol);
								goto IL_03CC;
							default:
								switch (c2)
								{
								case ',':
									goto IL_03CC;
								case '-':
								case '/':
									goto IL_03C3;
								case '.':
									if (this.DecimalPointPos == num9)
									{
										if (this.DecimalDigits > 0)
										{
											while (i < sb_int.Length)
											{
												stringBuilder.Append(sb_int[i++]);
											}
										}
										if (sb_dec.Length > 0)
										{
											stringBuilder.Append(nfi.NumberDecimalSeparator);
										}
									}
									flag = false;
									flag2 = true;
									goto IL_03CC;
								case '0':
									break;
								default:
									if (c2 != 'E')
									{
										goto IL_03C3;
									}
									goto IL_02A3;
								}
								break;
							}
							if (flag)
							{
								num++;
								if (this.IntegerDigits - num >= sb_int.Length + i)
								{
									if (c2 != '0')
									{
										goto IL_03CC;
									}
								}
								while (this.IntegerDigits - num + i < sb_int.Length)
								{
									stringBuilder.Append(sb_int[i++]);
									if (this.UseGroup && --num3 > 0 && --num6 == 0)
									{
										stringBuilder.Append(numberGroupSeparator);
										if (--num5 < numberGroupSizes.Length && num5 >= 0)
										{
											num7 = numberGroupSizes[num5];
										}
										num6 = num7;
									}
								}
								goto IL_03CC;
							}
							if (!flag2)
							{
								stringBuilder.Append(c2);
								goto IL_03CC;
							}
							if (num2 < sb_dec.Length)
							{
								stringBuilder.Append(sb_dec[num2++]);
								goto IL_03CC;
							}
							goto IL_03CC;
						}
						else if (c2 != '\\')
						{
							if (c2 != 'e')
							{
								if (c2 != '‰')
								{
									goto IL_03C3;
								}
								stringBuilder.Append(nfi.PerMilleSymbol);
								goto IL_03CC;
							}
						}
						else
						{
							num9++;
							if (num9 - offset < length)
							{
								stringBuilder.Append(format[num9]);
								goto IL_03CC;
							}
							goto IL_03CC;
						}
						IL_02A3:
						if (sb_exp == null || !this.UseExponent)
						{
							stringBuilder.Append(c2);
							goto IL_03CC;
						}
						bool flag3 = true;
						bool flag4 = false;
						int num10 = num9 + 1;
						while (num10 - offset < length)
						{
							if (format[num10] == '0')
							{
								flag4 = true;
							}
							else if (num10 != num9 + 1 || (format[num10] != '+' && format[num10] != '-'))
							{
								if (!flag4)
								{
									flag3 = false;
									break;
								}
								break;
							}
							num10++;
						}
						if (flag3)
						{
							num9 = num10 - 1;
							flag = this.DecimalPointPos < 0;
							flag2 = !flag;
							stringBuilder.Append(c2);
							stringBuilder.Append(sb_exp);
							sb_exp = null;
							goto IL_03CC;
						}
						stringBuilder.Append(c2);
						goto IL_03CC;
						IL_03C3:
						stringBuilder.Append(c2);
					}
					IL_03CC:
					num9++;
				}
				if (!positive)
				{
					stringBuilder.Insert(0, nfi.NegativeSign);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x04001761 RID: 5985
			public bool UseGroup;

			// Token: 0x04001762 RID: 5986
			public int DecimalDigits;

			// Token: 0x04001763 RID: 5987
			public int DecimalPointPos = -1;

			// Token: 0x04001764 RID: 5988
			public int DecimalTailSharpDigits;

			// Token: 0x04001765 RID: 5989
			public int IntegerDigits;

			// Token: 0x04001766 RID: 5990
			public int IntegerHeadSharpDigits;

			// Token: 0x04001767 RID: 5991
			public int IntegerHeadPos;

			// Token: 0x04001768 RID: 5992
			public bool UseExponent;

			// Token: 0x04001769 RID: 5993
			public int ExponentDigits;

			// Token: 0x0400176A RID: 5994
			public int ExponentTailSharpDigits;

			// Token: 0x0400176B RID: 5995
			public bool ExponentNegativeSignOnly = true;

			// Token: 0x0400176C RID: 5996
			public int DividePlaces;

			// Token: 0x0400176D RID: 5997
			public int Percents;

			// Token: 0x0400176E RID: 5998
			public int Permilles;
		}
	}
}
