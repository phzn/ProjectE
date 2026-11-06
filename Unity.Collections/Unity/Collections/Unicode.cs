using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000E8 RID: 232
	[GenerateTestsForBurstCompatibility]
	public struct Unicode
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x0001E124 File Offset: 0x0001C324
		public static bool IsValidCodePoint(int codepoint)
		{
			return codepoint <= 1114111 && codepoint >= 0;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0001E137 File Offset: 0x0001C337
		public static bool NotTrailer(byte b)
		{
			return (b & 192) != 128;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0001E14C File Offset: 0x0001C34C
		public static Unicode.Rune ReplacementCharacter
		{
			get
			{
				return new Unicode.Rune
				{
					value = 65533
				};
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0001E170 File Offset: 0x0001C370
		public static Unicode.Rune BadRune
		{
			get
			{
				return new Unicode.Rune
				{
					value = 0
				};
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0001E190 File Offset: 0x0001C390
		public unsafe static ConversionError Utf8ToUcs(out Unicode.Rune rune, byte* buffer, ref int index, int capacity)
		{
			rune = Unicode.ReplacementCharacter;
			if (index + 1 > capacity)
			{
				return ConversionError.Overflow;
			}
			if ((buffer[index] & 128) == 0)
			{
				rune.value = (int)buffer[index];
				index++;
				return ConversionError.None;
			}
			if ((buffer[index] & 224) == 192)
			{
				if (index + 2 > capacity)
				{
					index++;
					return ConversionError.Overflow;
				}
				int num = (int)(buffer[index] & 31);
				num = (num << 6) | (int)(buffer[index + 1] & 63);
				if (num < 128 || Unicode.NotTrailer(buffer[index + 1]))
				{
					index++;
					return ConversionError.Encoding;
				}
				rune.value = num;
				index += 2;
				return ConversionError.None;
			}
			else if ((buffer[index] & 240) == 224)
			{
				if (index + 3 > capacity)
				{
					index++;
					return ConversionError.Overflow;
				}
				int num = (int)(buffer[index] & 15);
				num = (num << 6) | (int)(buffer[index + 1] & 63);
				num = (num << 6) | (int)(buffer[index + 2] & 63);
				if (num < 2048 || !Unicode.IsValidCodePoint(num) || Unicode.NotTrailer(buffer[index + 1]) || Unicode.NotTrailer(buffer[index + 2]))
				{
					index++;
					return ConversionError.Encoding;
				}
				rune.value = num;
				index += 3;
				return ConversionError.None;
			}
			else
			{
				if ((buffer[index] & 248) != 240)
				{
					index++;
					return ConversionError.Encoding;
				}
				if (index + 4 > capacity)
				{
					index++;
					return ConversionError.Overflow;
				}
				int num = (int)(buffer[index] & 7);
				num = (num << 6) | (int)(buffer[index + 1] & 63);
				num = (num << 6) | (int)(buffer[index + 2] & 63);
				num = (num << 6) | (int)(buffer[index + 3] & 63);
				if (num < 65536 || !Unicode.IsValidCodePoint(num) || Unicode.NotTrailer(buffer[index + 1]) || Unicode.NotTrailer(buffer[index + 2]) || Unicode.NotTrailer(buffer[index + 3]))
				{
					index++;
					return ConversionError.Encoding;
				}
				rune.value = num;
				index += 4;
				return ConversionError.None;
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001E375 File Offset: 0x0001C575
		private unsafe static int FindUtf8CharStartInReverse(byte* ptr, ref int index)
		{
			while (index > 0)
			{
				index--;
				if ((ptr[index] & 192) != 128)
				{
					return index;
				}
			}
			return 0;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001E398 File Offset: 0x0001C598
		internal unsafe static ConversionError Utf8ToUcsReverse(out Unicode.Rune rune, byte* buffer, ref int index, int capacity)
		{
			int num = index;
			index--;
			index = Unicode.FindUtf8CharStartInReverse(buffer, ref index);
			if (index == num)
			{
				rune = Unicode.ReplacementCharacter;
				return ConversionError.Overflow;
			}
			int num2 = index;
			return Unicode.Utf8ToUcs(out rune, buffer, ref num2, capacity);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001E3D6 File Offset: 0x0001C5D6
		private static bool IsLeadingSurrogate(char c)
		{
			return c >= '\ud800' && c <= '\udbff';
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001E3ED File Offset: 0x0001C5ED
		private static bool IsTrailingSurrogate(char c)
		{
			return c >= '\udc00' && c <= '\udfff';
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0001E404 File Offset: 0x0001C604
		public unsafe static ConversionError Utf16ToUcs(out Unicode.Rune rune, char* buffer, ref int index, int capacity)
		{
			rune = Unicode.ReplacementCharacter;
			if (index + 1 > capacity)
			{
				return ConversionError.Overflow;
			}
			if (!Unicode.IsLeadingSurrogate(buffer[index]) || index + 2 > capacity)
			{
				rune.value = (int)buffer[index];
				index++;
				return ConversionError.None;
			}
			int num = (int)(buffer[index] & 'Ͽ');
			if (!Unicode.IsTrailingSurrogate(buffer[index + 1]))
			{
				rune.value = (int)buffer[index];
				index++;
				return ConversionError.None;
			}
			num = (num << 10) | (int)(buffer[index + 1] & 'Ͽ');
			num += 65536;
			rune.value = num;
			index += 2;
			return ConversionError.None;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001E4B5 File Offset: 0x0001C6B5
		internal unsafe static ConversionError UcsToUcs(out Unicode.Rune rune, Unicode.Rune* buffer, ref int index, int capacity)
		{
			rune = Unicode.ReplacementCharacter;
			if (index + 1 > capacity)
			{
				return ConversionError.Overflow;
			}
			rune = buffer[index];
			index++;
			return ConversionError.None;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0001E4EC File Offset: 0x0001C6EC
		public unsafe static ConversionError UcsToUtf8(byte* buffer, ref int index, int capacity, Unicode.Rune rune)
		{
			if (!Unicode.IsValidCodePoint(rune.value))
			{
				return ConversionError.CodePoint;
			}
			if (index + 1 > capacity)
			{
				return ConversionError.Overflow;
			}
			if (rune.value <= 127)
			{
				int num = index;
				index = num + 1;
				buffer[num] = (byte)rune.value;
				return ConversionError.None;
			}
			if (rune.value <= 2047)
			{
				if (index + 2 > capacity)
				{
					return ConversionError.Overflow;
				}
				int num = index;
				index = num + 1;
				buffer[num] = (byte)(192 | (rune.value >> 6));
				num = index;
				index = num + 1;
				buffer[num] = (byte)(128 | (rune.value & 63));
				return ConversionError.None;
			}
			else if (rune.value <= 65535)
			{
				if (index + 3 > capacity)
				{
					return ConversionError.Overflow;
				}
				int num = index;
				index = num + 1;
				buffer[num] = (byte)(224 | (rune.value >> 12));
				num = index;
				index = num + 1;
				buffer[num] = (byte)(128 | ((rune.value >> 6) & 63));
				num = index;
				index = num + 1;
				buffer[num] = (byte)(128 | (rune.value & 63));
				return ConversionError.None;
			}
			else
			{
				if (rune.value > 2097151)
				{
					return ConversionError.Encoding;
				}
				if (index + 4 > capacity)
				{
					return ConversionError.Overflow;
				}
				int num = index;
				index = num + 1;
				buffer[num] = (byte)(240 | (rune.value >> 18));
				num = index;
				index = num + 1;
				buffer[num] = (byte)(128 | ((rune.value >> 12) & 63));
				num = index;
				index = num + 1;
				buffer[num] = (byte)(128 | ((rune.value >> 6) & 63));
				num = index;
				index = num + 1;
				buffer[num] = (byte)(128 | (rune.value & 63));
				return ConversionError.None;
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0001E680 File Offset: 0x0001C880
		public unsafe static ConversionError UcsToUtf16(char* buffer, ref int index, int capacity, Unicode.Rune rune)
		{
			if (!Unicode.IsValidCodePoint(rune.value))
			{
				return ConversionError.CodePoint;
			}
			if (index + 1 > capacity)
			{
				return ConversionError.Overflow;
			}
			int num;
			if (rune.value < 65536)
			{
				num = index;
				index = num + 1;
				buffer[num] = (char)rune.value;
				return ConversionError.None;
			}
			if (index + 2 > capacity)
			{
				return ConversionError.Overflow;
			}
			int num2 = rune.value - 65536;
			if (num2 >= 1048576)
			{
				return ConversionError.Encoding;
			}
			num = index;
			index = num + 1;
			buffer[num] = (char)(55296 | (num2 >> 10));
			num = index;
			index = num + 1;
			buffer[num] = (char)(56320 | (num2 & 1023));
			return ConversionError.None;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0001E724 File Offset: 0x0001C924
		public unsafe static ConversionError Utf16ToUtf8(char* utf16Buffer, int utf16Length, byte* utf8Buffer, out int utf8Length, int utf8Capacity)
		{
			utf8Length = 0;
			int i = 0;
			while (i < utf16Length)
			{
				Unicode.Rune rune;
				Unicode.Utf16ToUcs(out rune, utf16Buffer, ref i, utf16Length);
				if (Unicode.UcsToUtf8(utf8Buffer, ref utf8Length, utf8Capacity, rune) == ConversionError.Overflow)
				{
					return ConversionError.Overflow;
				}
			}
			return ConversionError.None;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0001E758 File Offset: 0x0001C958
		public unsafe static ConversionError Utf8ToUtf8(byte* srcBuffer, int srcLength, byte* destBuffer, out int destLength, int destCapacity)
		{
			if (destCapacity >= srcLength)
			{
				UnsafeUtility.MemCpy((void*)destBuffer, (void*)srcBuffer, (long)srcLength);
				destLength = srcLength;
				return ConversionError.None;
			}
			destLength = 0;
			int i = 0;
			while (i < srcLength)
			{
				Unicode.Rune rune;
				Unicode.Utf8ToUcs(out rune, srcBuffer, ref i, srcLength);
				if (Unicode.UcsToUtf8(destBuffer, ref destLength, destCapacity, rune) == ConversionError.Overflow)
				{
					return ConversionError.Overflow;
				}
			}
			return ConversionError.None;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0001E7A0 File Offset: 0x0001C9A0
		public unsafe static ConversionError Utf8ToUtf16(byte* utf8Buffer, int utf8Length, char* utf16Buffer, out int utf16Length, int utf16Capacity)
		{
			utf16Length = 0;
			int i = 0;
			while (i < utf8Length)
			{
				Unicode.Rune rune;
				Unicode.Utf8ToUcs(out rune, utf8Buffer, ref i, utf8Length);
				if (Unicode.UcsToUtf16(utf16Buffer, ref utf16Length, utf16Capacity, rune) == ConversionError.Overflow)
				{
					return ConversionError.Overflow;
				}
			}
			return ConversionError.None;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0001E7D4 File Offset: 0x0001C9D4
		private unsafe static int CountRunes(byte* utf8Buffer, int utf8Length, int maxRunes = 2147483647)
		{
			int num = 0;
			int num2 = 0;
			while (num < maxRunes && num2 < utf8Length)
			{
				if ((utf8Buffer[num2] & 192) != 128)
				{
					num++;
				}
				num2++;
			}
			return num;
		}

		// Token: 0x04000459 RID: 1113
		public const int kMaximumValidCodePoint = 1114111;

		// Token: 0x020000E9 RID: 233
		[GenerateTestsForBurstCompatibility]
		public struct Rune
		{
			// Token: 0x06000A3C RID: 2620 RVA: 0x0001E809 File Offset: 0x0001CA09
			public Rune(int codepoint)
			{
				this.value = codepoint;
			}

			// Token: 0x06000A3D RID: 2621 RVA: 0x0001E814 File Offset: 0x0001CA14
			public static implicit operator Unicode.Rune(char codepoint)
			{
				return new Unicode.Rune
				{
					value = (int)codepoint
				};
			}

			// Token: 0x06000A3E RID: 2622 RVA: 0x0001E832 File Offset: 0x0001CA32
			public static bool operator ==(Unicode.Rune lhs, Unicode.Rune rhs)
			{
				return lhs.value == rhs.value;
			}

			// Token: 0x06000A3F RID: 2623 RVA: 0x0001E842 File Offset: 0x0001CA42
			[ExcludeFromBurstCompatTesting("Takes managed object")]
			public override bool Equals(object obj)
			{
				return obj is Unicode.Rune && this.value == ((Unicode.Rune)obj).value;
			}

			// Token: 0x06000A40 RID: 2624 RVA: 0x0001E861 File Offset: 0x0001CA61
			public override int GetHashCode()
			{
				return this.value;
			}

			// Token: 0x06000A41 RID: 2625 RVA: 0x0001E869 File Offset: 0x0001CA69
			public static bool operator !=(Unicode.Rune lhs, Unicode.Rune rhs)
			{
				return lhs.value != rhs.value;
			}

			// Token: 0x06000A42 RID: 2626 RVA: 0x0001E87C File Offset: 0x0001CA7C
			public static bool IsDigit(Unicode.Rune r)
			{
				return r.IsDigit();
			}

			// Token: 0x06000A43 RID: 2627 RVA: 0x0001E885 File Offset: 0x0001CA85
			internal bool IsAscii()
			{
				return this.value < 128;
			}

			// Token: 0x06000A44 RID: 2628 RVA: 0x0001E894 File Offset: 0x0001CA94
			internal bool IsLatin1()
			{
				return this.value < 256;
			}

			// Token: 0x06000A45 RID: 2629 RVA: 0x0001E8A3 File Offset: 0x0001CAA3
			internal bool IsDigit()
			{
				return this.value >= 48 && this.value <= 57;
			}

			// Token: 0x06000A46 RID: 2630 RVA: 0x0001E8C0 File Offset: 0x0001CAC0
			internal bool IsWhiteSpace()
			{
				if (this.IsLatin1())
				{
					return this.value == 32 || (this.value >= 9 && this.value <= 13) || this.value == 160 || this.value == 133;
				}
				return this.value == 5760 || (this.value >= 8192 && this.value <= 8202) || this.value == 8232 || this.value == 8233 || this.value == 8239 || this.value == 8287 || this.value == 12288;
			}

			// Token: 0x06000A47 RID: 2631 RVA: 0x0001E97A File Offset: 0x0001CB7A
			internal Unicode.Rune ToLowerAscii()
			{
				return new Unicode.Rune(this.value + ((this.value - 65 <= 25) ? 32 : 0));
			}

			// Token: 0x06000A48 RID: 2632 RVA: 0x0001E99A File Offset: 0x0001CB9A
			internal Unicode.Rune ToUpperAscii()
			{
				return new Unicode.Rune(this.value - ((this.value - 97 <= 25) ? 32 : 0));
			}

			// Token: 0x06000A49 RID: 2633 RVA: 0x0001E9BC File Offset: 0x0001CBBC
			public int LengthInUtf8Bytes()
			{
				if (this.value < 0)
				{
					return 4;
				}
				if (this.value <= 127)
				{
					return 1;
				}
				if (this.value <= 2047)
				{
					return 2;
				}
				if (this.value <= 65535)
				{
					return 3;
				}
				int num = this.value;
				return 4;
			}

			// Token: 0x0400045A RID: 1114
			public int value;
		}
	}
}
