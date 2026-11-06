using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000F2 RID: 242
	[GenerateTestsForBurstCompatibility]
	public static class UTF8ArrayUnsafeUtility
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x0001F31B File Offset: 0x0001D51B
		public unsafe static CopyError Copy(byte* dest, out int destLength, int destUTF8MaxLengthInBytes, char* src, int srcLength)
		{
			if (Unicode.Utf16ToUtf8(src, srcLength, dest, out destLength, destUTF8MaxLengthInBytes) == ConversionError.None)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0001F330 File Offset: 0x0001D530
		public unsafe static CopyError Copy(byte* dest, out ushort destLength, ushort destUTF8MaxLengthInBytes, char* src, int srcLength)
		{
			int num;
			bool flag = Unicode.Utf16ToUtf8(src, srcLength, dest, out num, (int)destUTF8MaxLengthInBytes) != ConversionError.None;
			destLength = (ushort)num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0001F354 File Offset: 0x0001D554
		public unsafe static CopyError Copy(byte* dest, out int destLength, int destUTF8MaxLengthInBytes, byte* src, int srcLength)
		{
			int num;
			bool flag = Unicode.Utf8ToUtf8(src, srcLength, dest, out num, destUTF8MaxLengthInBytes) != ConversionError.None;
			destLength = num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0001F378 File Offset: 0x0001D578
		public unsafe static CopyError Copy(byte* dest, out ushort destLength, ushort destUTF8MaxLengthInBytes, byte* src, ushort srcLength)
		{
			int num;
			bool flag = Unicode.Utf8ToUtf8(src, (int)srcLength, dest, out num, (int)destUTF8MaxLengthInBytes) != ConversionError.None;
			destLength = (ushort)num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0001F39A File Offset: 0x0001D59A
		public unsafe static CopyError Copy(char* dest, out int destLength, int destUCS2MaxLengthInChars, byte* src, int srcLength)
		{
			if (Unicode.Utf8ToUtf16(src, srcLength, dest, out destLength, destUCS2MaxLengthInChars) == ConversionError.None)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0001F3AC File Offset: 0x0001D5AC
		public unsafe static CopyError Copy(char* dest, out ushort destLength, ushort destUCS2MaxLengthInChars, byte* src, ushort srcLength)
		{
			int num;
			bool flag = Unicode.Utf8ToUtf16(src, (int)srcLength, dest, out num, (int)destUCS2MaxLengthInChars) != ConversionError.None;
			destLength = (ushort)num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0001F3CE File Offset: 0x0001D5CE
		public unsafe static FormatError AppendUTF8Bytes(byte* dest, ref int destLength, int destCapacity, byte* src, int srcLength)
		{
			if (destLength + srcLength > destCapacity)
			{
				return FormatError.Overflow;
			}
			UnsafeUtility.MemCpy((void*)(dest + destLength), (void*)src, (long)srcLength);
			destLength += srcLength;
			return FormatError.None;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0001F3F0 File Offset: 0x0001D5F0
		public unsafe static CopyError Append(byte* dest, ref ushort destLength, ushort destUTF8MaxLengthInBytes, byte* src, ushort srcLength)
		{
			int num;
			bool flag = Unicode.Utf8ToUtf8(src, (int)srcLength, dest + destLength, out num, (int)(destUTF8MaxLengthInBytes - destLength)) != ConversionError.None;
			destLength += (ushort)num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0001F41C File Offset: 0x0001D61C
		public unsafe static CopyError Append(byte* dest, ref ushort destLength, ushort destUTF8MaxLengthInBytes, char* src, int srcLength)
		{
			int num;
			bool flag = Unicode.Utf16ToUtf8(src, srcLength, dest + destLength, out num, (int)(destUTF8MaxLengthInBytes - destLength)) != ConversionError.None;
			destLength += (ushort)num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001F448 File Offset: 0x0001D648
		public unsafe static CopyError Append(char* dest, ref ushort destLength, ushort destUCS2MaxLengthInChars, byte* src, ushort srcLength)
		{
			int num;
			bool flag = Unicode.Utf8ToUtf16(src, (int)srcLength, dest + destLength, out num, (int)(destUCS2MaxLengthInChars - destLength)) != ConversionError.None;
			destLength += (ushort)num;
			if (!flag)
			{
				return CopyError.None;
			}
			return CopyError.Truncation;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0001F478 File Offset: 0x0001D678
		public unsafe static int StrCmp(byte* utf8BufferA, int utf8LengthInBytesA, byte* utf8BufferB, int utf8LengthInBytesB)
		{
			int num = 0;
			int num2 = 0;
			UTF8ArrayUnsafeUtility.Comparison comparison;
			do
			{
				Unicode.Rune rune;
				ConversionError conversionError = Unicode.Utf8ToUcs(out rune, utf8BufferA, ref num, utf8LengthInBytesA);
				Unicode.Rune rune2;
				ConversionError conversionError2 = Unicode.Utf8ToUcs(out rune2, utf8BufferB, ref num2, utf8LengthInBytesB);
				comparison = new UTF8ArrayUnsafeUtility.Comparison(rune, conversionError, rune2, conversionError2);
			}
			while (!comparison.terminates);
			return comparison.result;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0001F4C0 File Offset: 0x0001D6C0
		internal unsafe static int StrCmp(byte* utf8BufferA, int utf8LengthInBytesA, Unicode.Rune* runeBufferB, int lengthInRunesB)
		{
			int num = 0;
			int num2 = 0;
			UTF8ArrayUnsafeUtility.Comparison comparison;
			do
			{
				Unicode.Rune rune;
				ConversionError conversionError = Unicode.Utf8ToUcs(out rune, utf8BufferA, ref num, utf8LengthInBytesA);
				Unicode.Rune rune2;
				ConversionError conversionError2 = Unicode.UcsToUcs(out rune2, runeBufferB, ref num2, lengthInRunesB);
				comparison = new UTF8ArrayUnsafeUtility.Comparison(rune, conversionError, rune2, conversionError2);
			}
			while (!comparison.terminates);
			return comparison.result;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0001F508 File Offset: 0x0001D708
		public unsafe static int StrCmp(char* utf16BufferA, int utf16LengthInCharsA, char* utf16BufferB, int utf16LengthInCharsB)
		{
			int num = 0;
			int num2 = 0;
			UTF8ArrayUnsafeUtility.Comparison comparison;
			do
			{
				Unicode.Rune rune;
				ConversionError conversionError = Unicode.Utf16ToUcs(out rune, utf16BufferA, ref num, utf16LengthInCharsA);
				Unicode.Rune rune2;
				ConversionError conversionError2 = Unicode.Utf16ToUcs(out rune2, utf16BufferB, ref num2, utf16LengthInCharsB);
				comparison = new UTF8ArrayUnsafeUtility.Comparison(rune, conversionError, rune2, conversionError2);
			}
			while (!comparison.terminates);
			return comparison.result;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0001F54F File Offset: 0x0001D74F
		public unsafe static bool EqualsUTF8Bytes(byte* aBytes, int aLength, byte* bBytes, int bLength)
		{
			return aLength == bLength && UTF8ArrayUnsafeUtility.StrCmp(aBytes, aLength, bBytes, bLength) == 0;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0001F564 File Offset: 0x0001D764
		public unsafe static int StrCmp(byte* utf8Buffer, int utf8LengthInBytes, char* utf16Buffer, int utf16LengthInChars)
		{
			int num = 0;
			int num2 = 0;
			UTF8ArrayUnsafeUtility.Comparison comparison;
			do
			{
				Unicode.Rune rune;
				ConversionError conversionError = Unicode.Utf8ToUcs(out rune, utf8Buffer, ref num, utf8LengthInBytes);
				Unicode.Rune rune2;
				ConversionError conversionError2 = Unicode.Utf16ToUcs(out rune2, utf16Buffer, ref num2, utf16LengthInChars);
				comparison = new UTF8ArrayUnsafeUtility.Comparison(rune, conversionError, rune2, conversionError2);
			}
			while (!comparison.terminates);
			return comparison.result;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0001F5AB File Offset: 0x0001D7AB
		public unsafe static int StrCmp(char* utf16Buffer, int utf16LengthInChars, byte* utf8Buffer, int utf8LengthInBytes)
		{
			return -UTF8ArrayUnsafeUtility.StrCmp(utf8Buffer, utf8LengthInBytes, utf16Buffer, utf16LengthInChars);
		}

		// Token: 0x020000F3 RID: 243
		internal struct Comparison
		{
			// Token: 0x06000A84 RID: 2692 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
			public Comparison(Unicode.Rune runeA, ConversionError errorA, Unicode.Rune runeB, ConversionError errorB)
			{
				if (errorA != ConversionError.None)
				{
					runeA.value = 0;
				}
				if (errorB != ConversionError.None)
				{
					runeB.value = 0;
				}
				if (runeA.value != runeB.value)
				{
					this.result = runeA.value - runeB.value;
					this.terminates = true;
					return;
				}
				this.result = 0;
				this.terminates = runeA.value == 0 && runeB.value == 0;
			}

			// Token: 0x04000470 RID: 1136
			public bool terminates;

			// Token: 0x04000471 RID: 1137
			public int result;
		}
	}
}
