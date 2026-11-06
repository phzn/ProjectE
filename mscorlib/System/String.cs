using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace System
{
	/// <summary>Represents text as a sequence of UTF-16 code units.</summary>
	// Token: 0x020000CD RID: 205
	[Serializable]
	public sealed class String : IComparable, IEnumerable, IEnumerable<char>, IComparable<string>, IEquatable<string>, IConvertible, ICloneable
	{
		// Token: 0x060004DF RID: 1247 RVA: 0x00017E10 File Offset: 0x00016010
		private unsafe static int CompareOrdinalIgnoreCaseHelper(string strA, string strB)
		{
			int num = Math.Min(strA.Length, strB.Length);
			fixed (char* ptr = &strA._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &strB._firstChar)
				{
					char* ptr4 = ptr3;
					char* ptr5 = ptr2;
					char* ptr6 = ptr4;
					while (num != 0)
					{
						int num2 = (int)(*ptr5);
						int num3 = (int)(*ptr6);
						if (num2 - 97 <= 25)
						{
							num2 -= 32;
						}
						if (num3 - 97 <= 25)
						{
							num3 -= 32;
						}
						if (num2 != num3)
						{
							return num2 - num3;
						}
						ptr5++;
						ptr6++;
						num--;
					}
					return strA.Length - strB.Length;
				}
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00017EA5 File Offset: 0x000160A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool EqualsHelper(string strA, string strB)
		{
			return SpanHelpers.SequenceEqual(Unsafe.As<char, byte>(strA.GetRawStringData()), Unsafe.As<char, byte>(strB.GetRawStringData()), (ulong)((long)strA.Length * 2L));
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00017ECC File Offset: 0x000160CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CompareOrdinalHelper(string strA, int indexA, int countA, string strB, int indexB, int countB)
		{
			return SpanHelpers.SequenceCompareTo(Unsafe.Add<char>(strA.GetRawStringData(), indexA), countA, Unsafe.Add<char>(strB.GetRawStringData(), indexB), countB);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00017EF0 File Offset: 0x000160F0
		private unsafe static bool EqualsIgnoreCaseAsciiHelper(string strA, string strB)
		{
			int num = strA.Length;
			fixed (char* ptr = &strA._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &strB._firstChar)
				{
					char* ptr4 = ptr3;
					char* ptr5 = ptr2;
					char* ptr6 = ptr4;
					while (num != 0)
					{
						int num2 = (int)(*ptr5);
						int num3 = (int)(*ptr6);
						if (num2 != num3 && ((num2 | 32) != (num3 | 32) || (num2 | 32) - 97 > 25))
						{
							return false;
						}
						ptr5++;
						ptr6++;
						num--;
					}
					return true;
				}
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00017F60 File Offset: 0x00016160
		private unsafe static int CompareOrdinalHelper(string strA, string strB)
		{
			int i = Math.Min(strA.Length, strB.Length);
			fixed (char* ptr = &strA._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &strB._firstChar)
				{
					char* ptr4 = ptr3;
					char* ptr5 = ptr2;
					char* ptr6 = ptr4;
					if (ptr5[1] == ptr6[1])
					{
						i -= 2;
						ptr5 += 2;
						ptr6 += 2;
						while (i >= 12)
						{
							if (*(long*)ptr5 == *(long*)ptr6)
							{
								if (*(long*)(ptr5 + 4) == *(long*)(ptr6 + 4))
								{
									if (*(long*)(ptr5 + 8) == *(long*)(ptr6 + 8))
									{
										i -= 12;
										ptr5 += 12;
										ptr6 += 12;
										continue;
									}
									ptr5 += 4;
									ptr6 += 4;
								}
								ptr5 += 4;
								ptr6 += 4;
							}
							if (*(int*)ptr5 == *(int*)ptr6)
							{
								ptr5 += 2;
								ptr6 += 2;
							}
							IL_010E:
							if (*ptr5 != *ptr6)
							{
								return (int)(*ptr5 - *ptr6);
							}
							goto IL_011E;
						}
						while (i > 0)
						{
							if (*(int*)ptr5 != *(int*)ptr6)
							{
								goto IL_010E;
							}
							i -= 2;
							ptr5 += 2;
							ptr6 += 2;
						}
						return strA.Length - strB.Length;
					}
					IL_011E:
					return (int)(ptr5[1] - ptr6[1]);
				}
			}
		}

		/// <summary>Compares two specified <see cref="T:System.String" /> objects and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to compare.</param>
		/// <param name="strB">The second string to compare.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///  <paramref name="strA" /> precedes <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///  <paramref name="strA" /> occurs in the same position as <paramref name="strB" /> in the sort order.  
		///
		///   Greater than zero  
		///
		///  <paramref name="strA" /> follows <paramref name="strB" /> in the sort order.</returns>
		// Token: 0x060004E4 RID: 1252 RVA: 0x00018096 File Offset: 0x00016296
		public static int Compare(string strA, string strB)
		{
			return string.Compare(strA, strB, StringComparison.CurrentCulture);
		}

		/// <summary>Compares two specified <see cref="T:System.String" /> objects, ignoring or honoring their case, and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to compare.</param>
		/// <param name="strB">The second string to compare.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to ignore case during the comparison; otherwise, <see langword="false" />.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///  <paramref name="strA" /> precedes <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///  <paramref name="strA" /> occurs in the same position as <paramref name="strB" /> in the sort order.  
		///
		///   Greater than zero  
		///
		///  <paramref name="strA" /> follows <paramref name="strB" /> in the sort order.</returns>
		// Token: 0x060004E5 RID: 1253 RVA: 0x000180A0 File Offset: 0x000162A0
		public static int Compare(string strA, string strB, bool ignoreCase)
		{
			StringComparison stringComparison = (ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			return string.Compare(strA, strB, stringComparison);
		}

		/// <summary>Compares two specified <see cref="T:System.String" /> objects using the specified rules, and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to compare.</param>
		/// <param name="strB">The second string to compare.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///  <paramref name="strA" /> precedes <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///  <paramref name="strA" /> is in the same position as <paramref name="strB" /> in the sort order.  
		///
		///   Greater than zero  
		///
		///  <paramref name="strA" /> follows <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a <see cref="T:System.StringComparison" /> value.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="T:System.StringComparison" /> is not supported.</exception>
		// Token: 0x060004E6 RID: 1254 RVA: 0x000180C0 File Offset: 0x000162C0
		public static int Compare(string strA, string strB, StringComparison comparisonType)
		{
			if (strA == strB)
			{
				string.CheckStringComparison(comparisonType);
				return 0;
			}
			if (strA == null)
			{
				string.CheckStringComparison(comparisonType);
				return -1;
			}
			if (strB == null)
			{
				string.CheckStringComparison(comparisonType);
				return 1;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.Compare(strA, strB, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.Compare(strA, strB, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				if (strA._firstChar != strB._firstChar)
				{
					return (int)(strA._firstChar - strB._firstChar);
				}
				return string.CompareOrdinalHelper(strA, strB);
			case StringComparison.OrdinalIgnoreCase:
				return CompareInfo.CompareOrdinalIgnoreCase(strA, 0, strA.Length, strB, 0, strB.Length);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Compares two specified <see cref="T:System.String" /> objects using the specified comparison options and culture-specific information to influence the comparison, and returns an integer that indicates the relationship of the two strings to each other in the sort order.</summary>
		/// <param name="strA">The first string to compare.</param>
		/// <param name="strB">The second string to compare.</param>
		/// <param name="culture">The culture that supplies culture-specific comparison information.</param>
		/// <param name="options">Options to use when performing the comparison (such as ignoring case or symbols).</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between <paramref name="strA" /> and <paramref name="strB" />, as shown in the following table  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///  <paramref name="strA" /> precedes <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///  <paramref name="strA" /> occurs in the same position as <paramref name="strB" /> in the sort order.  
		///
		///   Greater than zero  
		///
		///  <paramref name="strA" /> follows <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not a <see cref="T:System.Globalization.CompareOptions" /> value.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x060004E7 RID: 1255 RVA: 0x0001819A File Offset: 0x0001639A
		public static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.CompareInfo.Compare(strA, strB, options);
		}

		/// <summary>Compares two specified <see cref="T:System.String" /> objects, ignoring or honoring their case, and using culture-specific information to influence the comparison, and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to compare.</param>
		/// <param name="strB">The second string to compare.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to ignore case during the comparison; otherwise, <see langword="false" />.</param>
		/// <param name="culture">An object that supplies culture-specific comparison information.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///  <paramref name="strA" /> precedes <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///  <paramref name="strA" /> occurs in the same position as <paramref name="strB" /> in the sort order.  
		///
		///   Greater than zero  
		///
		///  <paramref name="strA" /> follows <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x060004E8 RID: 1256 RVA: 0x000181B8 File Offset: 0x000163B8
		public static int Compare(string strA, string strB, bool ignoreCase, CultureInfo culture)
		{
			CompareOptions compareOptions = (ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
			return string.Compare(strA, strB, culture, compareOptions);
		}

		/// <summary>Compares substrings of two specified <see cref="T:System.String" /> objects and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to use in the comparison.</param>
		/// <param name="indexA">The position of the substring within <paramref name="strA" />.</param>
		/// <param name="strB">The second string to use in the comparison.</param>
		/// <param name="indexB">The position of the substring within <paramref name="strB" />.</param>
		/// <param name="length">The maximum number of characters in the substrings to compare.</param>
		/// <returns>A 32-bit signed integer indicating the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   The substring in <paramref name="strA" /> precedes the substring in <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///   The substrings occur in the same position in the sort order, or <paramref name="length" /> is zero.  
		///
		///   Greater than zero  
		///
		///   The substring in <paramref name="strA" /> follows the substring in <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="indexA" /> is greater than <paramref name="strA" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexB" /> is greater than <paramref name="strB" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexA" />, <paramref name="indexB" />, or <paramref name="length" /> is negative.  
		/// -or-  
		/// Either <paramref name="indexA" /> or <paramref name="indexB" /> is <see langword="null" />, and <paramref name="length" /> is greater than zero.</exception>
		// Token: 0x060004E9 RID: 1257 RVA: 0x000181D6 File Offset: 0x000163D6
		public static int Compare(string strA, int indexA, string strB, int indexB, int length)
		{
			return string.Compare(strA, indexA, strB, indexB, length, false);
		}

		/// <summary>Compares substrings of two specified <see cref="T:System.String" /> objects, ignoring or honoring their case, and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to use in the comparison.</param>
		/// <param name="indexA">The position of the substring within <paramref name="strA" />.</param>
		/// <param name="strB">The second string to use in the comparison.</param>
		/// <param name="indexB">The position of the substring within <paramref name="strB" />.</param>
		/// <param name="length">The maximum number of characters in the substrings to compare.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to ignore case during the comparison; otherwise, <see langword="false" />.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   The substring in <paramref name="strA" /> precedes the substring in <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///   The substrings occur in the same position in the sort order, or <paramref name="length" /> is zero.  
		///
		///   Greater than zero  
		///
		///   The substring in <paramref name="strA" /> follows the substring in <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="indexA" /> is greater than <paramref name="strA" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexB" /> is greater than <paramref name="strB" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexA" />, <paramref name="indexB" />, or <paramref name="length" /> is negative.  
		/// -or-  
		/// Either <paramref name="indexA" /> or <paramref name="indexB" /> is <see langword="null" />, and <paramref name="length" /> is greater than zero.</exception>
		// Token: 0x060004EA RID: 1258 RVA: 0x000181E4 File Offset: 0x000163E4
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase)
		{
			int num = length;
			int num2 = length;
			if (strA != null)
			{
				num = Math.Min(num, strA.Length - indexA);
			}
			if (strB != null)
			{
				num2 = Math.Min(num2, strB.Length - indexB);
			}
			CompareOptions compareOptions = (ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, compareOptions);
		}

		/// <summary>Compares substrings of two specified <see cref="T:System.String" /> objects, ignoring or honoring their case and using culture-specific information to influence the comparison, and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to use in the comparison.</param>
		/// <param name="indexA">The position of the substring within <paramref name="strA" />.</param>
		/// <param name="strB">The second string to use in the comparison.</param>
		/// <param name="indexB">The position of the substring within <paramref name="strB" />.</param>
		/// <param name="length">The maximum number of characters in the substrings to compare.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to ignore case during the comparison; otherwise, <see langword="false" />.</param>
		/// <param name="culture">An object that supplies culture-specific comparison information.</param>
		/// <returns>An integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   The substring in <paramref name="strA" /> precedes the substring in <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///   The substrings occur in the same position in the sort order, or <paramref name="length" /> is zero.  
		///
		///   Greater than zero  
		///
		///   The substring in <paramref name="strA" /> follows the substring in <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="indexA" /> is greater than <paramref name="strA" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexB" /> is greater than <paramref name="strB" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexA" />, <paramref name="indexB" />, or <paramref name="length" /> is negative.  
		/// -or-  
		/// Either <paramref name="strA" /> or <paramref name="strB" /> is <see langword="null" />, and <paramref name="length" /> is greater than zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x060004EB RID: 1259 RVA: 0x0001823C File Offset: 0x0001643C
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
		{
			CompareOptions compareOptions = (ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
			return string.Compare(strA, indexA, strB, indexB, length, culture, compareOptions);
		}

		/// <summary>Compares substrings of two specified <see cref="T:System.String" /> objects using the specified comparison options and culture-specific information to influence the comparison, and returns an integer that indicates the relationship of the two substrings to each other in the sort order.</summary>
		/// <param name="strA">The first string to use in the comparison.</param>
		/// <param name="indexA">The starting position of the substring within <paramref name="strA" />.</param>
		/// <param name="strB">The second string to use in the comparison.</param>
		/// <param name="indexB">The starting position of the substring within <paramref name="strB" />.</param>
		/// <param name="length">The maximum number of characters in the substrings to compare.</param>
		/// <param name="culture">An object that supplies culture-specific comparison information.</param>
		/// <param name="options">Options to use when performing the comparison (such as ignoring case or symbols).</param>
		/// <returns>An integer that indicates the lexical relationship between the two substrings, as shown in the following table.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   The substring in <paramref name="strA" /> precedes the substring in <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///   The substrings occur in the same position in the sort order, or <paramref name="length" /> is zero.  
		///
		///   Greater than zero  
		///
		///   The substring in <paramref name="strA" /> follows the substring in <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not a <see cref="T:System.Globalization.CompareOptions" /> value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="indexA" /> is greater than <paramref name="strA" /><see langword=".Length" />.  
		/// -or-  
		/// <paramref name="indexB" /> is greater than <paramref name="strB" /><see langword=".Length" />.  
		/// -or-  
		/// <paramref name="indexA" />, <paramref name="indexB" />, or <paramref name="length" /> is negative.  
		/// -or-  
		/// Either <paramref name="strA" /> or <paramref name="strB" /> is <see langword="null" />, and <paramref name="length" /> is greater than zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x060004EC RID: 1260 RVA: 0x00018260 File Offset: 0x00016460
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			int num = length;
			int num2 = length;
			if (strA != null)
			{
				num = Math.Min(num, strA.Length - indexA);
			}
			if (strB != null)
			{
				num2 = Math.Min(num2, strB.Length - indexB);
			}
			return culture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, options);
		}

		/// <summary>Compares substrings of two specified <see cref="T:System.String" /> objects using the specified rules, and returns an integer that indicates their relative position in the sort order.</summary>
		/// <param name="strA">The first string to use in the comparison.</param>
		/// <param name="indexA">The position of the substring within <paramref name="strA" />.</param>
		/// <param name="strB">The second string to use in the comparison.</param>
		/// <param name="indexB">The position of the substring within <paramref name="strB" />.</param>
		/// <param name="length">The maximum number of characters in the substrings to compare.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   The substring in <paramref name="strA" /> precedes the substring in <paramref name="strB" /> in the sort order.  
		///
		///   Zero  
		///
		///   The substrings occur in the same position in the sort order, or the <paramref name="length" /> parameter is zero.  
		///
		///   Greater than zero  
		///
		///   The substring in <paramref name="strA" /> follllows the substring in <paramref name="strB" /> in the sort order.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="indexA" /> is greater than <paramref name="strA" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexB" /> is greater than <paramref name="strB" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexA" />, <paramref name="indexB" />, or <paramref name="length" /> is negative.  
		/// -or-  
		/// Either <paramref name="indexA" /> or <paramref name="indexB" /> is <see langword="null" />, and <paramref name="length" /> is greater than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x060004ED RID: 1261 RVA: 0x000182BC File Offset: 0x000164BC
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType)
		{
			string.CheckStringComparison(comparisonType);
			if (strA == null || strB == null)
			{
				if (strA == strB)
				{
					return 0;
				}
				if (strA != null)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
				}
				if (indexA < 0 || indexB < 0)
				{
					throw new ArgumentOutOfRangeException((indexA < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (strA.Length - indexA < 0 || strB.Length - indexB < 0)
				{
					throw new ArgumentOutOfRangeException((strA.Length - indexA < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (length == 0 || (strA == strB && indexA == indexB))
				{
					return 0;
				}
				int num = Math.Min(length, strA.Length - indexA);
				int num2 = Math.Min(length, strB.Length - indexB);
				switch (comparisonType)
				{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
				case StringComparison.InvariantCulture:
					return CompareInfo.Invariant.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
				case StringComparison.InvariantCultureIgnoreCase:
					return CompareInfo.Invariant.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return string.CompareOrdinalHelper(strA, indexA, num, strB, indexB, num2);
				case StringComparison.OrdinalIgnoreCase:
					return CompareInfo.CompareOrdinalIgnoreCase(strA, indexA, num, strB, indexB, num2);
				default:
					throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
				}
			}
		}

		/// <summary>Compares two specified <see cref="T:System.String" /> objects by evaluating the numeric values of the corresponding <see cref="T:System.Char" /> objects in each string.</summary>
		/// <param name="strA">The first string to compare.</param>
		/// <param name="strB">The second string to compare.</param>
		/// <returns>An integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///  <paramref name="strA" /> is less than <paramref name="strB" />.  
		///
		///   Zero  
		///
		///  <paramref name="strA" /> and <paramref name="strB" /> are equal.  
		///
		///   Greater than zero  
		///
		///  <paramref name="strA" /> is greater than <paramref name="strB" />.</returns>
		// Token: 0x060004EE RID: 1262 RVA: 0x0001841B File Offset: 0x0001661B
		public static int CompareOrdinal(string strA, string strB)
		{
			if (strA == strB)
			{
				return 0;
			}
			if (strA == null)
			{
				return -1;
			}
			if (strB == null)
			{
				return 1;
			}
			if (strA._firstChar != strB._firstChar)
			{
				return (int)(strA._firstChar - strB._firstChar);
			}
			return string.CompareOrdinalHelper(strA, strB);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00018450 File Offset: 0x00016650
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int CompareOrdinal(ReadOnlySpan<char> strA, ReadOnlySpan<char> strB)
		{
			return SpanHelpers.SequenceCompareTo(MemoryMarshal.GetReference<char>(strA), strA.Length, MemoryMarshal.GetReference<char>(strB), strB.Length);
		}

		/// <summary>Compares substrings of two specified <see cref="T:System.String" /> objects by evaluating the numeric values of the corresponding <see cref="T:System.Char" /> objects in each substring.</summary>
		/// <param name="strA">The first string to use in the comparison.</param>
		/// <param name="indexA">The starting index of the substring in <paramref name="strA" />.</param>
		/// <param name="strB">The second string to use in the comparison.</param>
		/// <param name="indexB">The starting index of the substring in <paramref name="strB" />.</param>
		/// <param name="length">The maximum number of characters in the substrings to compare.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   The substring in <paramref name="strA" /> is less than the substring in <paramref name="strB" />.  
		///
		///   Zero  
		///
		///   The substrings are equal, or <paramref name="length" /> is zero.  
		///
		///   Greater than zero  
		///
		///   The substring in <paramref name="strA" /> is greater than the substring in <paramref name="strB" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="strA" /> is not <see langword="null" /> and <paramref name="indexA" /> is greater than <paramref name="strA" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="strB" /> is not <see langword="null" /> and <paramref name="indexB" /> is greater than <paramref name="strB" />.<see cref="P:System.String.Length" />.  
		/// -or-  
		/// <paramref name="indexA" />, <paramref name="indexB" />, or <paramref name="length" /> is negative.</exception>
		// Token: 0x060004F0 RID: 1264 RVA: 0x00018474 File Offset: 0x00016674
		public static int CompareOrdinal(string strA, int indexA, string strB, int indexB, int length)
		{
			if (strA == null || strB == null)
			{
				if (strA == strB)
				{
					return 0;
				}
				if (strA != null)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException("length", "Count cannot be less than zero.");
				}
				if (indexA < 0 || indexB < 0)
				{
					throw new ArgumentOutOfRangeException((indexA < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				int num = Math.Min(length, strA.Length - indexA);
				int num2 = Math.Min(length, strB.Length - indexB);
				if (num < 0 || num2 < 0)
				{
					throw new ArgumentOutOfRangeException((num < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (length == 0 || (strA == strB && indexA == indexB))
				{
					return 0;
				}
				return string.CompareOrdinalHelper(strA, indexA, num, strB, indexB, num2);
			}
		}

		/// <summary>Compares this instance with a specified <see cref="T:System.Object" /> and indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified <see cref="T:System.Object" />.</summary>
		/// <param name="value">An object that evaluates to a <see cref="T:System.String" />.</param>
		/// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the <paramref name="value" /> parameter.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   This instance precedes <paramref name="value" />.  
		///
		///   Zero  
		///
		///   This instance has the same position in the sort order as <paramref name="value" />.  
		///
		///   Greater than zero  
		///
		///   This instance follows <paramref name="value" />.  
		///
		///  -or-  
		///
		///  <paramref name="value" /> is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.String" />.</exception>
		// Token: 0x060004F1 RID: 1265 RVA: 0x00018528 File Offset: 0x00016728
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			string text = value as string;
			if (text == null)
			{
				throw new ArgumentException("Object must be of type String.");
			}
			return this.CompareTo(text);
		}

		/// <summary>Compares this instance with a specified <see cref="T:System.String" /> object and indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified string.</summary>
		/// <param name="strB">The string to compare with this instance.</param>
		/// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the <paramref name="strB" /> parameter.  
		///   Value  
		///
		///   Condition  
		///
		///   Less than zero  
		///
		///   This instance precedes <paramref name="strB" />.  
		///
		///   Zero  
		///
		///   This instance has the same position in the sort order as <paramref name="strB" />.  
		///
		///   Greater than zero  
		///
		///   This instance follows <paramref name="strB" />.  
		///
		///  -or-  
		///
		///  <paramref name="strB" /> is <see langword="null" />.</returns>
		// Token: 0x060004F2 RID: 1266 RVA: 0x00018096 File Offset: 0x00016296
		public int CompareTo(string strB)
		{
			return string.Compare(this, strB, StringComparison.CurrentCulture);
		}

		/// <summary>Determines whether the end of this string instance matches the specified string.</summary>
		/// <param name="value">The string to compare to the substring at the end of this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> matches the end of this instance; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x060004F3 RID: 1267 RVA: 0x00018556 File Offset: 0x00016756
		public bool EndsWith(string value)
		{
			return this.EndsWith(value, StringComparison.CurrentCulture);
		}

		/// <summary>Determines whether the end of this string instance matches the specified string when compared using the specified comparison option.</summary>
		/// <param name="value">The string to compare to the substring at the end of this instance.</param>
		/// <param name="comparisonType">One of the enumeration values that determines how this string and <paramref name="value" /> are compared.</param>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="value" /> parameter matches the end of this string; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x060004F4 RID: 1268 RVA: 0x00018560 File Offset: 0x00016760
		public bool EndsWith(string value, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this == value)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (value.Length == 0)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IsSuffix(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IsSuffix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Length >= value.Length && string.CompareOrdinalHelper(this, this.Length - value.Length, value.Length, value, 0, value.Length) == 0;
			case StringComparison.OrdinalIgnoreCase:
				return this.Length >= value.Length && CompareInfo.CompareOrdinalIgnoreCase(this, this.Length - value.Length, value.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Determines whether the end of this string instance matches the specified string when compared using the specified culture.</summary>
		/// <param name="value">The string to compare to the substring at the end of this instance.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to ignore case during the comparison; otherwise, <see langword="false" />.</param>
		/// <param name="culture">Cultural information that determines how this instance and <paramref name="value" /> are compared. If <paramref name="culture" /> is <see langword="null" />, the current culture is used.</param>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="value" /> parameter matches the end of this string; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x060004F5 RID: 1269 RVA: 0x00018675 File Offset: 0x00016875
		public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this == value || (culture ?? CultureInfo.CurrentCulture).CompareInfo.IsSuffix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000186A8 File Offset: 0x000168A8
		public bool EndsWith(char value)
		{
			int length = this.Length;
			return length != 0 && this[length - 1] == value;
		}

		/// <summary>Determines whether this instance and a specified object, which must also be a <see cref="T:System.String" /> object, have the same value.</summary>
		/// <param name="obj">The string to compare to this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.String" /> and its value is the same as this instance; otherwise, <see langword="false" />.  If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
		// Token: 0x060004F7 RID: 1271 RVA: 0x000186D0 File Offset: 0x000168D0
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			string text = obj as string;
			return text != null && this.Length == text.Length && string.EqualsHelper(this, text);
		}

		/// <summary>Determines whether this instance and another specified <see cref="T:System.String" /> object have the same value.</summary>
		/// <param name="value">The string to compare to this instance.</param>
		/// <returns>
		///   <see langword="true" /> if the value of the <paramref name="value" /> parameter is the same as the value of this instance; otherwise, <see langword="false" />. If <paramref name="value" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
		// Token: 0x060004F8 RID: 1272 RVA: 0x00018706 File Offset: 0x00016906
		public bool Equals(string value)
		{
			return this == value || (value != null && this.Length == value.Length && string.EqualsHelper(this, value));
		}

		/// <summary>Determines whether this string and a specified <see cref="T:System.String" /> object have the same value. A parameter specifies the culture, case, and sort rules used in the comparison.</summary>
		/// <param name="value">The string to compare to this instance.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>
		///   <see langword="true" /> if the value of the <paramref name="value" /> parameter is the same as this string; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x060004F9 RID: 1273 RVA: 0x0001872C File Offset: 0x0001692C
		public bool Equals(string value, StringComparison comparisonType)
		{
			if (this == value)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (value == null)
			{
				string.CheckStringComparison(comparisonType);
				return false;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.None) == 0;
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.IgnoreCase) == 0;
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.Compare(this, value, CompareOptions.None) == 0;
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.Compare(this, value, CompareOptions.IgnoreCase) == 0;
			case StringComparison.Ordinal:
				return this.Length == value.Length && string.EqualsHelper(this, value);
			case StringComparison.OrdinalIgnoreCase:
				return this.Length == value.Length && CompareInfo.CompareOrdinalIgnoreCase(this, 0, this.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Determines whether two specified <see cref="T:System.String" /> objects have the same value.</summary>
		/// <param name="a">The first string to compare, or <see langword="null" />.</param>
		/// <param name="b">The second string to compare, or <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="a" /> is the same as the value of <paramref name="b" />; otherwise, <see langword="false" />. If both <paramref name="a" /> and <paramref name="b" /> are <see langword="null" />, the method returns <see langword="true" />.</returns>
		// Token: 0x060004FA RID: 1274 RVA: 0x00018811 File Offset: 0x00016A11
		public static bool Equals(string a, string b)
		{
			return a == b || (a != null && b != null && a.Length == b.Length && string.EqualsHelper(a, b));
		}

		/// <summary>Determines whether two specified <see cref="T:System.String" /> objects have the same value. A parameter specifies the culture, case, and sort rules used in the comparison.</summary>
		/// <param name="a">The first string to compare, or <see langword="null" />.</param>
		/// <param name="b">The second string to compare, or <see langword="null" />.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
		/// <returns>
		///   <see langword="true" /> if the value of the <paramref name="a" /> parameter is equal to the value of the <paramref name="b" /> parameter; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x060004FB RID: 1275 RVA: 0x00018838 File Offset: 0x00016A38
		public static bool Equals(string a, string b, StringComparison comparisonType)
		{
			if (a == b)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (a == null || b == null)
			{
				string.CheckStringComparison(comparisonType);
				return false;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0;
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0;
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.Compare(a, b, CompareOptions.None) == 0;
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.Compare(a, b, CompareOptions.IgnoreCase) == 0;
			case StringComparison.Ordinal:
				return a.Length == b.Length && string.EqualsHelper(a, b);
			case StringComparison.OrdinalIgnoreCase:
				return a.Length == b.Length && CompareInfo.CompareOrdinalIgnoreCase(a, 0, a.Length, b, 0, b.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Determines whether two specified strings have the same value.</summary>
		/// <param name="a">The first string to compare, or <see langword="null" />.</param>
		/// <param name="b">The second string to compare, or <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="a" /> is the same as the value of <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060004FC RID: 1276 RVA: 0x00018920 File Offset: 0x00016B20
		public static bool operator ==(string a, string b)
		{
			return string.Equals(a, b);
		}

		/// <summary>Determines whether two specified strings have different values.</summary>
		/// <param name="a">The first string to compare, or <see langword="null" />.</param>
		/// <param name="b">The second string to compare, or <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="a" /> is different from the value of <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060004FD RID: 1277 RVA: 0x00018929 File Offset: 0x00016B29
		public static bool operator !=(string a, string b)
		{
			return !string.Equals(a, b);
		}

		/// <summary>Returns the hash code for this string.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x060004FE RID: 1278 RVA: 0x00018935 File Offset: 0x00016B35
		public override int GetHashCode()
		{
			return this.GetLegacyNonRandomizedHashCode();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001893D File Offset: 0x00016B3D
		public int GetHashCode(StringComparison comparisonType)
		{
			return StringComparer.FromComparison(comparisonType).GetHashCode(this);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001894C File Offset: 0x00016B4C
		internal unsafe int GetLegacyNonRandomizedHashCode()
		{
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				int num = 5381;
				int num2 = num;
				char* ptr3 = ptr2;
				int num3;
				while ((num3 = (int)(*ptr3)) != 0)
				{
					num = ((num << 5) + num) ^ num3;
					num3 = (int)ptr3[1];
					if (num3 == 0)
					{
						break;
					}
					num2 = ((num2 << 5) + num2) ^ num3;
					ptr3 += 2;
				}
				return num + num2 * 1566083941;
			}
		}

		/// <summary>Determines whether the beginning of this string instance matches the specified string.</summary>
		/// <param name="value">The string to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> matches the beginning of this string; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x06000501 RID: 1281 RVA: 0x000189A0 File Offset: 0x00016BA0
		public bool StartsWith(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this.StartsWith(value, StringComparison.CurrentCulture);
		}

		/// <summary>Determines whether the beginning of this string instance matches the specified string when compared using the specified comparison option.</summary>
		/// <param name="value">The string to compare.</param>
		/// <param name="comparisonType">One of the enumeration values that determines how this string and <paramref name="value" /> are compared.</param>
		/// <returns>
		///   <see langword="true" /> if this instance begins with <paramref name="value" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x06000502 RID: 1282 RVA: 0x000189B8 File Offset: 0x00016BB8
		public bool StartsWith(string value, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this == value)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (value.Length == 0)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IsPrefix(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IsPrefix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Length >= value.Length && this._firstChar == value._firstChar && (value.Length == 1 || SpanHelpers.SequenceEqual(Unsafe.As<char, byte>(this.GetRawStringData()), Unsafe.As<char, byte>(value.GetRawStringData()), (ulong)((long)value.Length * 2L)));
			case StringComparison.OrdinalIgnoreCase:
				return this.Length >= value.Length && CompareInfo.CompareOrdinalIgnoreCase(this, 0, value.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Determines whether the beginning of this string instance matches the specified string when compared using the specified culture.</summary>
		/// <param name="value">The string to compare.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to ignore case during the comparison; otherwise, <see langword="false" />.</param>
		/// <param name="culture">Cultural information that determines how this string and <paramref name="value" /> are compared. If <paramref name="culture" /> is <see langword="null" />, the current culture is used.</param>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="value" /> parameter matches the beginning of this string; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x06000503 RID: 1283 RVA: 0x00018ADB File Offset: 0x00016CDB
		public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this == value || (culture ?? CultureInfo.CurrentCulture).CompareInfo.IsPrefix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00018B0E File Offset: 0x00016D0E
		public bool StartsWith(char value)
		{
			return this.Length != 0 && this._firstChar == value;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00018B23 File Offset: 0x00016D23
		internal static void CheckStringComparison(StringComparison comparisonType)
		{
			if (comparisonType - StringComparison.CurrentCulture > 5)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.NotSupported_StringComparison, ExceptionArgument.comparisonType);
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00018B34 File Offset: 0x00016D34
		private unsafe static void FillStringChecked(string dest, int destPos, string src)
		{
			if (src.Length > dest.Length - destPos)
			{
				throw new IndexOutOfRangeException();
			}
			fixed (char* ptr = &dest._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &src._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr2 + destPos, ptr4, src.Length);
				}
			}
		}

		/// <summary>Creates the string  representation of a specified object.</summary>
		/// <param name="arg0">The object to represent, or <see langword="null" />.</param>
		/// <returns>The string representation of the value of <paramref name="arg0" />, or <see cref="F:System.String.Empty" /> if <paramref name="arg0" /> is <see langword="null" />.</returns>
		// Token: 0x06000507 RID: 1287 RVA: 0x00018B81 File Offset: 0x00016D81
		public static string Concat(object arg0)
		{
			if (arg0 == null)
			{
				return string.Empty;
			}
			return arg0.ToString();
		}

		/// <summary>Concatenates the string representations of two specified objects.</summary>
		/// <param name="arg0">The first object to concatenate.</param>
		/// <param name="arg1">The second object to concatenate.</param>
		/// <returns>The concatenated string representations of the values of <paramref name="arg0" /> and <paramref name="arg1" />.</returns>
		// Token: 0x06000508 RID: 1288 RVA: 0x00018B92 File Offset: 0x00016D92
		public static string Concat(object arg0, object arg1)
		{
			if (arg0 == null)
			{
				arg0 = string.Empty;
			}
			if (arg1 == null)
			{
				arg1 = string.Empty;
			}
			return arg0.ToString() + arg1.ToString();
		}

		/// <summary>Concatenates the string representations of three specified objects.</summary>
		/// <param name="arg0">The first object to concatenate.</param>
		/// <param name="arg1">The second object to concatenate.</param>
		/// <param name="arg2">The third object to concatenate.</param>
		/// <returns>The concatenated string representations of the values of <paramref name="arg0" />, <paramref name="arg1" />, and <paramref name="arg2" />.</returns>
		// Token: 0x06000509 RID: 1289 RVA: 0x00018BB9 File Offset: 0x00016DB9
		public static string Concat(object arg0, object arg1, object arg2)
		{
			if (arg0 == null)
			{
				arg0 = string.Empty;
			}
			if (arg1 == null)
			{
				arg1 = string.Empty;
			}
			if (arg2 == null)
			{
				arg2 = string.Empty;
			}
			return arg0.ToString() + arg1.ToString() + arg2.ToString();
		}

		/// <summary>Concatenates the string representations of the elements in a specified <see cref="T:System.Object" /> array.</summary>
		/// <param name="args">An object array that contains the elements to concatenate.</param>
		/// <returns>The concatenated string representations of the values of the elements in <paramref name="args" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="args" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OutOfMemoryException">Out of memory.</exception>
		// Token: 0x0600050A RID: 1290 RVA: 0x00018BF0 File Offset: 0x00016DF0
		public static string Concat(params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (args.Length <= 1)
			{
				string text;
				if (args.Length != 0)
				{
					object obj = args[0];
					if ((text = ((obj != null) ? obj.ToString() : null)) == null)
					{
						return string.Empty;
					}
				}
				else
				{
					text = string.Empty;
				}
				return text;
			}
			string[] array = new string[args.Length];
			int num = 0;
			for (int i = 0; i < args.Length; i++)
			{
				object obj2 = args[i];
				string text2 = ((obj2 != null) ? obj2.ToString() : null) ?? string.Empty;
				array[i] = text2;
				num += text2.Length;
				if (num < 0)
				{
					throw new OutOfMemoryException();
				}
			}
			if (num == 0)
			{
				return string.Empty;
			}
			string text3 = string.FastAllocateString(num);
			int num2 = 0;
			foreach (string text4 in array)
			{
				string.FillStringChecked(text3, num2, text4);
				num2 += text4.Length;
			}
			return text3;
		}

		/// <summary>Concatenates the members of an <see cref="T:System.Collections.Generic.IEnumerable`1" /> implementation.</summary>
		/// <param name="values">A collection object that implements the <see cref="T:System.Collections.Generic.IEnumerable`1" /> interface.</param>
		/// <typeparam name="T">The type of the members of <paramref name="values" />.</typeparam>
		/// <returns>The concatenated members in <paramref name="values" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is <see langword="null" />.</exception>
		// Token: 0x0600050B RID: 1291 RVA: 0x00018CC4 File Offset: 0x00016EC4
		public static string Concat<T>(IEnumerable<T> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (typeof(T) == typeof(char))
			{
				using (IEnumerator<char> enumerator = Unsafe.As<IEnumerable<char>>(values).GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return string.Empty;
					}
					char c = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						return string.CreateFromChar(c);
					}
					StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
					stringBuilder.Append(c);
					do
					{
						c = enumerator.Current;
						stringBuilder.Append(c);
					}
					while (enumerator.MoveNext());
					return StringBuilderCache.GetStringAndRelease(stringBuilder);
				}
			}
			string text;
			using (IEnumerator<T> enumerator2 = values.GetEnumerator())
			{
				if (!enumerator2.MoveNext())
				{
					text = string.Empty;
				}
				else
				{
					T t = enumerator2.Current;
					string text2 = ((t != null) ? t.ToString() : null);
					if (!enumerator2.MoveNext())
					{
						text = text2 ?? string.Empty;
					}
					else
					{
						StringBuilder stringBuilder2 = StringBuilderCache.Acquire(16);
						stringBuilder2.Append(text2);
						do
						{
							t = enumerator2.Current;
							if (t != null)
							{
								stringBuilder2.Append(t.ToString());
							}
						}
						while (enumerator2.MoveNext());
						text = StringBuilderCache.GetStringAndRelease(stringBuilder2);
					}
				}
			}
			return text;
		}

		/// <summary>Concatenates the members of a constructed <see cref="T:System.Collections.Generic.IEnumerable`1" /> collection of type <see cref="T:System.String" />.</summary>
		/// <param name="values">A collection object that implements <see cref="T:System.Collections.Generic.IEnumerable`1" /> and whose generic type argument is <see cref="T:System.String" />.</param>
		/// <returns>The concatenated strings in <paramref name="values" />, or <see cref="F:System.String.Empty" /> if <paramref name="values" /> is an empty <see langword="IEnumerable(Of String)" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is <see langword="null" />.</exception>
		// Token: 0x0600050C RID: 1292 RVA: 0x00018E40 File Offset: 0x00017040
		public static string Concat(IEnumerable<string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string text;
			using (IEnumerator<string> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					text = string.Empty;
				}
				else
				{
					string text2 = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						text = text2 ?? string.Empty;
					}
					else
					{
						StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
						stringBuilder.Append(text2);
						do
						{
							stringBuilder.Append(enumerator.Current);
						}
						while (enumerator.MoveNext());
						text = StringBuilderCache.GetStringAndRelease(stringBuilder);
					}
				}
			}
			return text;
		}

		/// <summary>Concatenates two specified instances of <see cref="T:System.String" />.</summary>
		/// <param name="str0">The first string to concatenate.</param>
		/// <param name="str1">The second string to concatenate.</param>
		/// <returns>The concatenation of <paramref name="str0" /> and <paramref name="str1" />.</returns>
		// Token: 0x0600050D RID: 1293 RVA: 0x00018ED8 File Offset: 0x000170D8
		public static string Concat(string str0, string str1)
		{
			if (string.IsNullOrEmpty(str0))
			{
				if (string.IsNullOrEmpty(str1))
				{
					return string.Empty;
				}
				return str1;
			}
			else
			{
				if (string.IsNullOrEmpty(str1))
				{
					return str0;
				}
				int length = str0.Length;
				string text = string.FastAllocateString(length + str1.Length);
				string.FillStringChecked(text, 0, str0);
				string.FillStringChecked(text, length, str1);
				return text;
			}
		}

		/// <summary>Concatenates three specified instances of <see cref="T:System.String" />.</summary>
		/// <param name="str0">The first string to concatenate.</param>
		/// <param name="str1">The second string to concatenate.</param>
		/// <param name="str2">The third string to concatenate.</param>
		/// <returns>The concatenation of <paramref name="str0" />, <paramref name="str1" />, and <paramref name="str2" />.</returns>
		// Token: 0x0600050E RID: 1294 RVA: 0x00018F2C File Offset: 0x0001712C
		public static string Concat(string str0, string str1, string str2)
		{
			if (string.IsNullOrEmpty(str0))
			{
				return str1 + str2;
			}
			if (string.IsNullOrEmpty(str1))
			{
				return str0 + str2;
			}
			if (string.IsNullOrEmpty(str2))
			{
				return str0 + str1;
			}
			string text = string.FastAllocateString(str0.Length + str1.Length + str2.Length);
			string.FillStringChecked(text, 0, str0);
			string.FillStringChecked(text, str0.Length, str1);
			string.FillStringChecked(text, str0.Length + str1.Length, str2);
			return text;
		}

		/// <summary>Concatenates four specified instances of <see cref="T:System.String" />.</summary>
		/// <param name="str0">The first string to concatenate.</param>
		/// <param name="str1">The second string to concatenate.</param>
		/// <param name="str2">The third string to concatenate.</param>
		/// <param name="str3">The fourth string to concatenate.</param>
		/// <returns>The concatenation of <paramref name="str0" />, <paramref name="str1" />, <paramref name="str2" />, and <paramref name="str3" />.</returns>
		// Token: 0x0600050F RID: 1295 RVA: 0x00018FAC File Offset: 0x000171AC
		public static string Concat(string str0, string str1, string str2, string str3)
		{
			if (string.IsNullOrEmpty(str0))
			{
				return str1 + str2 + str3;
			}
			if (string.IsNullOrEmpty(str1))
			{
				return str0 + str2 + str3;
			}
			if (string.IsNullOrEmpty(str2))
			{
				return str0 + str1 + str3;
			}
			if (string.IsNullOrEmpty(str3))
			{
				return str0 + str1 + str2;
			}
			string text = string.FastAllocateString(str0.Length + str1.Length + str2.Length + str3.Length);
			string.FillStringChecked(text, 0, str0);
			string.FillStringChecked(text, str0.Length, str1);
			string.FillStringChecked(text, str0.Length + str1.Length, str2);
			string.FillStringChecked(text, str0.Length + str1.Length + str2.Length, str3);
			return text;
		}

		/// <summary>Concatenates the elements of a specified <see cref="T:System.String" /> array.</summary>
		/// <param name="values">An array of string instances.</param>
		/// <returns>The concatenated elements of <paramref name="values" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OutOfMemoryException">Out of memory.</exception>
		// Token: 0x06000510 RID: 1296 RVA: 0x00019064 File Offset: 0x00017264
		public static string Concat(params string[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length <= 1)
			{
				string text;
				if (values.Length != 0)
				{
					if ((text = values[0]) == null)
					{
						return string.Empty;
					}
				}
				else
				{
					text = string.Empty;
				}
				return text;
			}
			long num = 0L;
			foreach (string text2 in values)
			{
				if (text2 != null)
				{
					num += (long)text2.Length;
				}
			}
			if (num > 2147483647L)
			{
				throw new OutOfMemoryException();
			}
			int num2 = (int)num;
			if (num2 == 0)
			{
				return string.Empty;
			}
			string text3 = string.FastAllocateString(num2);
			int num3 = 0;
			foreach (string text4 in values)
			{
				if (!string.IsNullOrEmpty(text4))
				{
					int length = text4.Length;
					if (length > num2 - num3)
					{
						num3 = -1;
						break;
					}
					string.FillStringChecked(text3, num3, text4);
					num3 += length;
				}
			}
			if (num3 != num2)
			{
				return string.Concat((string[])values.Clone());
			}
			return text3;
		}

		/// <summary>Replaces one or more format items in a string with the string representation of a specified object.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The object to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which any format items are replaced by the string representation of <paramref name="arg0" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format item in <paramref name="format" /> is invalid.  
		///  -or-  
		///  The index of a format item is not zero.</exception>
		// Token: 0x06000511 RID: 1297 RVA: 0x00019143 File Offset: 0x00017343
		public static string Format(string format, object arg0)
		{
			return string.FormatHelper(null, format, new ParamsArray(arg0));
		}

		/// <summary>Replaces the format items in a string with the string representation of two specified objects.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which format items are replaced by the string representations of <paramref name="arg0" /> and <paramref name="arg1" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is not zero or one.</exception>
		// Token: 0x06000512 RID: 1298 RVA: 0x00019152 File Offset: 0x00017352
		public static string Format(string format, object arg0, object arg1)
		{
			return string.FormatHelper(null, format, new ParamsArray(arg0, arg1));
		}

		/// <summary>Replaces the format items in a string with the string representation of three specified objects.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		/// <param name="arg2">The third object to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which the format items have been replaced by the string representations of <paramref name="arg0" />, <paramref name="arg1" />, and <paramref name="arg2" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is less than zero, or greater than two.</exception>
		// Token: 0x06000513 RID: 1299 RVA: 0x00019162 File Offset: 0x00017362
		public static string Format(string format, object arg0, object arg1, object arg2)
		{
			return string.FormatHelper(null, format, new ParamsArray(arg0, arg1, arg2));
		}

		/// <summary>Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="args" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is less than zero, or greater than or equal to the length of the <paramref name="args" /> array.</exception>
		// Token: 0x06000514 RID: 1300 RVA: 0x00019173 File Offset: 0x00017373
		public static string Format(string format, params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}
			return string.FormatHelper(null, format, new ParamsArray(args));
		}

		/// <summary>Replaces the format item or items in a specified string with the string representation of the corresponding object. A parameter supplies culture-specific formatting information.</summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The object to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which the format item or items have been replaced by the string representation of <paramref name="arg0" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is not zero.</exception>
		// Token: 0x06000515 RID: 1301 RVA: 0x0001919A File Offset: 0x0001739A
		public static string Format(IFormatProvider provider, string format, object arg0)
		{
			return string.FormatHelper(provider, format, new ParamsArray(arg0));
		}

		/// <summary>Replaces the format items in a string with the string representation of two specified objects. A parameter supplies culture-specific formatting information.</summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which format items are replaced by the string representations of <paramref name="arg0" /> and <paramref name="arg1" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is not zero or one.</exception>
		// Token: 0x06000516 RID: 1302 RVA: 0x000191A9 File Offset: 0x000173A9
		public static string Format(IFormatProvider provider, string format, object arg0, object arg1)
		{
			return string.FormatHelper(provider, format, new ParamsArray(arg0, arg1));
		}

		/// <summary>Replaces the format items in a string with the string representation of three specified objects. An parameter supplies culture-specific formatting information.</summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format.</param>
		/// <param name="arg1">The second object to format.</param>
		/// <param name="arg2">The third object to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which the format items have been replaced by the string representations of <paramref name="arg0" />, <paramref name="arg1" />, and <paramref name="arg2" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is less than zero, or greater than two.</exception>
		// Token: 0x06000517 RID: 1303 RVA: 0x000191B9 File Offset: 0x000173B9
		public static string Format(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
		{
			return string.FormatHelper(provider, format, new ParamsArray(arg0, arg1, arg2));
		}

		/// <summary>Replaces the format items in a string with the string representations of corresponding objects in a specified array. A parameter supplies culture-specific formatting information.</summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <returns>A copy of <paramref name="format" /> in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="args" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.  
		/// -or-  
		/// The index of a format item is less than zero, or greater than or equal to the length of the <paramref name="args" /> array.</exception>
		// Token: 0x06000518 RID: 1304 RVA: 0x000191CB File Offset: 0x000173CB
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}
			return string.FormatHelper(provider, format, new ParamsArray(args));
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000191F2 File Offset: 0x000173F2
		private static string FormatHelper(IFormatProvider provider, string format, ParamsArray args)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			return StringBuilderCache.GetStringAndRelease(StringBuilderCache.Acquire(format.Length + args.Length * 8).AppendFormatHelper(provider, format, args));
		}

		/// <summary>Returns a new string in which a specified string is inserted at a specified index position in this instance.</summary>
		/// <param name="startIndex">The zero-based index position of the insertion.</param>
		/// <param name="value">The string to insert.</param>
		/// <returns>A new string that is equivalent to this instance, but with <paramref name="value" /> inserted at position <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is negative or greater than the length of this instance.</exception>
		// Token: 0x0600051A RID: 1306 RVA: 0x00019224 File Offset: 0x00017424
		public unsafe string Insert(int startIndex, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			int length = this.Length;
			int length2 = value.Length;
			if (length == 0)
			{
				return value;
			}
			if (length2 == 0)
			{
				return this;
			}
			string text = string.FastAllocateString(length + length2);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &value._firstChar)
				{
					char* ptr4 = ptr3;
					fixed (char* ptr5 = &text._firstChar)
					{
						char* ptr6 = ptr5;
						string.wstrcpy(ptr6, ptr2, startIndex);
						string.wstrcpy(ptr6 + startIndex, ptr4, length2);
						string.wstrcpy(ptr6 + startIndex + length2, ptr2 + startIndex, length - startIndex);
					}
				}
			}
			return text;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000192D2 File Offset: 0x000174D2
		public static string Join(char separator, params string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return string.Join(separator, value, 0, value.Length);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000192ED File Offset: 0x000174ED
		public unsafe static string Join(char separator, params object[] values)
		{
			return string.JoinCore(&separator, 1, values);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000192F9 File Offset: 0x000174F9
		public unsafe static string Join<T>(char separator, IEnumerable<T> values)
		{
			return string.JoinCore<T>(&separator, 1, values);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019305 File Offset: 0x00017505
		public unsafe static string Join(char separator, string[] value, int startIndex, int count)
		{
			return string.JoinCore(&separator, 1, value, startIndex, count);
		}

		/// <summary>Concatenates all the elements of a string array, using the specified separator between each element.</summary>
		/// <param name="separator">The string to use as a separator. <paramref name="separator" /> is included in the returned string only if <paramref name="value" /> has more than one element.</param>
		/// <param name="value">An array that contains the elements to concatenate.</param>
		/// <returns>A string that consists of the elements in <paramref name="value" /> delimited by the <paramref name="separator" /> string. If <paramref name="value" /> is an empty array, the method returns <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x0600051F RID: 1311 RVA: 0x00019313 File Offset: 0x00017513
		public static string Join(string separator, params string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return string.Join(separator, value, 0, value.Length);
		}

		/// <summary>Concatenates the elements of an object array, using the specified separator between each element.</summary>
		/// <param name="separator">The string to use as a separator. <paramref name="separator" /> is included in the returned string only if <paramref name="values" /> has more than one element.</param>
		/// <param name="values">An array that contains the elements to concatenate.</param>
		/// <returns>A string that consists of the elements of <paramref name="values" /> delimited by the <paramref name="separator" /> string. If <paramref name="values" /> is an empty array, the method returns <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is <see langword="null" />.</exception>
		// Token: 0x06000520 RID: 1312 RVA: 0x00019330 File Offset: 0x00017530
		public unsafe static string Join(string separator, params object[] values)
		{
			separator = separator ?? string.Empty;
			fixed (char* ptr = &separator._firstChar)
			{
				return string.JoinCore(ptr, separator.Length, values);
			}
		}

		/// <summary>Concatenates the members of a collection, using the specified separator between each member.</summary>
		/// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="values" /> has more than one element.</param>
		/// <param name="values">A collection that contains the objects to concatenate.</param>
		/// <typeparam name="T">The type of the members of <paramref name="values" />.</typeparam>
		/// <returns>A string that consists of the members of <paramref name="values" /> delimited by the <paramref name="separator" /> string. If <paramref name="values" /> has no members, the method returns <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is <see langword="null" />.</exception>
		// Token: 0x06000521 RID: 1313 RVA: 0x00019360 File Offset: 0x00017560
		public unsafe static string Join<T>(string separator, IEnumerable<T> values)
		{
			separator = separator ?? string.Empty;
			fixed (char* ptr = &separator._firstChar)
			{
				return string.JoinCore<T>(ptr, separator.Length, values);
			}
		}

		/// <summary>Concatenates the members of a constructed <see cref="T:System.Collections.Generic.IEnumerable`1" /> collection of type <see cref="T:System.String" />, using the specified separator between each member.</summary>
		/// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="values" /> has more than one element.</param>
		/// <param name="values">A collection that contains the strings to concatenate.</param>
		/// <returns>A string that consists of the members of <paramref name="values" /> delimited by the <paramref name="separator" /> string. If <paramref name="values" /> has no members, the method returns <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is <see langword="null" />.</exception>
		// Token: 0x06000522 RID: 1314 RVA: 0x00019390 File Offset: 0x00017590
		public static string Join(string separator, IEnumerable<string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string text;
			using (IEnumerator<string> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					text = string.Empty;
				}
				else
				{
					string text2 = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						text = text2 ?? string.Empty;
					}
					else
					{
						StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
						stringBuilder.Append(text2);
						do
						{
							stringBuilder.Append(separator);
							stringBuilder.Append(enumerator.Current);
						}
						while (enumerator.MoveNext());
						text = StringBuilderCache.GetStringAndRelease(stringBuilder);
					}
				}
			}
			return text;
		}

		/// <summary>Concatenates the specified elements of a string array, using the specified separator between each element.</summary>
		/// <param name="separator">The string to use as a separator. <paramref name="separator" /> is included in the returned string only if <paramref name="value" /> has more than one element.</param>
		/// <param name="value">An array that contains the elements to concatenate.</param>
		/// <param name="startIndex">The first element in <paramref name="value" /> to use.</param>
		/// <param name="count">The number of elements of <paramref name="value" /> to use.</param>
		/// <returns>A string that consists of the strings in <paramref name="value" /> delimited by the <paramref name="separator" /> string.  
		///  -or-  
		///  <see cref="F:System.String.Empty" /> if <paramref name="count" /> is zero, <paramref name="value" /> has no elements, or <paramref name="separator" /> and all the elements of <paramref name="value" /> are <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="count" /> is less than 0.  
		/// -or-  
		/// <paramref name="startIndex" /> plus <paramref name="count" /> is greater than the number of elements in <paramref name="value" />.</exception>
		/// <exception cref="T:System.OutOfMemoryException">Out of memory.</exception>
		// Token: 0x06000523 RID: 1315 RVA: 0x00019430 File Offset: 0x00017630
		public unsafe static string Join(string separator, string[] value, int startIndex, int count)
		{
			separator = separator ?? string.Empty;
			fixed (char* ptr = &separator._firstChar)
			{
				return string.JoinCore(ptr, separator.Length, value, startIndex, count);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00019460 File Offset: 0x00017660
		private unsafe static string JoinCore(char* separator, int separatorLength, object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				return string.Empty;
			}
			object obj = values[0];
			string text = ((obj != null) ? obj.ToString() : null);
			if (values.Length == 1)
			{
				return text ?? string.Empty;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			stringBuilder.Append(text);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder.Append(separator, separatorLength);
				object obj2 = values[i];
				if (obj2 != null)
				{
					stringBuilder.Append(obj2.ToString());
				}
			}
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000194E8 File Offset: 0x000176E8
		private unsafe static string JoinCore<T>(char* separator, int separatorLength, IEnumerable<T> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string text;
			using (IEnumerator<T> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					text = string.Empty;
				}
				else
				{
					T t = enumerator.Current;
					string text2 = ((t != null) ? t.ToString() : null);
					if (!enumerator.MoveNext())
					{
						text = text2 ?? string.Empty;
					}
					else
					{
						StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
						stringBuilder.Append(text2);
						do
						{
							t = enumerator.Current;
							stringBuilder.Append(separator, separatorLength);
							if (t != null)
							{
								stringBuilder.Append(t.ToString());
							}
						}
						while (enumerator.MoveNext());
						text = StringBuilderCache.GetStringAndRelease(stringBuilder);
					}
				}
			}
			return text;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000195C0 File Offset: 0x000177C0
		private unsafe static string JoinCore(char* separator, int separatorLength, string[] value, int startIndex, int count)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (startIndex > value.Length - count)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index and count must refer to a location within the buffer.");
			}
			if (count <= 1)
			{
				string text;
				if (count != 0)
				{
					if ((text = value[startIndex]) == null)
					{
						return string.Empty;
					}
				}
				else
				{
					text = string.Empty;
				}
				return text;
			}
			long num = (long)(count - 1) * (long)separatorLength;
			if (num > 2147483647L)
			{
				throw new OutOfMemoryException();
			}
			int num2 = (int)num;
			int i = startIndex;
			int num3 = startIndex + count;
			while (i < num3)
			{
				string text2 = value[i];
				if (text2 != null)
				{
					num2 += text2.Length;
					if (num2 < 0)
					{
						throw new OutOfMemoryException();
					}
				}
				i++;
			}
			string text3 = string.FastAllocateString(num2);
			int num4 = 0;
			int j = startIndex;
			int num5 = startIndex + count;
			while (j < num5)
			{
				string text4 = value[j];
				if (text4 != null)
				{
					int length = text4.Length;
					if (length > num2 - num4)
					{
						num4 = -1;
						break;
					}
					string.FillStringChecked(text3, num4, text4);
					num4 += length;
				}
				if (j < num5 - 1)
				{
					fixed (char* ptr = &text3._firstChar)
					{
						char* ptr2 = ptr;
						if (separatorLength == 1)
						{
							ptr2[num4] = *separator;
						}
						else
						{
							string.wstrcpy(ptr2 + num4, separator, separatorLength);
						}
					}
					num4 += separatorLength;
				}
				j++;
			}
			if (num4 != num2)
			{
				return string.JoinCore(separator, separatorLength, (string[])value.Clone(), startIndex, count);
			}
			return text3;
		}

		/// <summary>Returns a new string that right-aligns the characters in this instance by padding them with spaces on the left, for a specified total length.</summary>
		/// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
		/// <returns>A new string that is equivalent to this instance, but right-aligned and padded on the left with as many spaces as needed to create a length of <paramref name="totalWidth" />. However, if <paramref name="totalWidth" /> is less than the length of this instance, the method returns a reference to the existing instance. If <paramref name="totalWidth" /> is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="totalWidth" /> is less than zero.</exception>
		// Token: 0x06000527 RID: 1319 RVA: 0x00019724 File Offset: 0x00017924
		public string PadLeft(int totalWidth)
		{
			return this.PadLeft(totalWidth, ' ');
		}

		/// <summary>Returns a new string that right-aligns the characters in this instance by padding them on the left with a specified Unicode character, for a specified total length.</summary>
		/// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
		/// <param name="paddingChar">A Unicode padding character.</param>
		/// <returns>A new string that is equivalent to this instance, but right-aligned and padded on the left with as many <paramref name="paddingChar" /> characters as needed to create a length of <paramref name="totalWidth" />. However, if <paramref name="totalWidth" /> is less than the length of this instance, the method returns a reference to the existing instance. If <paramref name="totalWidth" /> is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="totalWidth" /> is less than zero.</exception>
		// Token: 0x06000528 RID: 1320 RVA: 0x00019730 File Offset: 0x00017930
		public unsafe string PadLeft(int totalWidth, char paddingChar)
		{
			if (totalWidth < 0)
			{
				throw new ArgumentOutOfRangeException("totalWidth", "Non-negative number required.");
			}
			int length = this.Length;
			int num = totalWidth - length;
			if (num <= 0)
			{
				return this;
			}
			string text = string.FastAllocateString(totalWidth);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				for (int i = 0; i < num; i++)
				{
					ptr2[i] = paddingChar;
				}
				fixed (char* ptr3 = &this._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr2 + num, ptr4, length);
				}
			}
			return text;
		}

		/// <summary>Returns a new string that left-aligns the characters in this string by padding them with spaces on the right, for a specified total length.</summary>
		/// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
		/// <returns>A new string that is equivalent to this instance, but left-aligned and padded on the right with as many spaces as needed to create a length of <paramref name="totalWidth" />. However, if <paramref name="totalWidth" /> is less than the length of this instance, the method returns a reference to the existing instance. If <paramref name="totalWidth" /> is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="totalWidth" /> is less than zero.</exception>
		// Token: 0x06000529 RID: 1321 RVA: 0x000197B2 File Offset: 0x000179B2
		public string PadRight(int totalWidth)
		{
			return this.PadRight(totalWidth, ' ');
		}

		/// <summary>Returns a new string that left-aligns the characters in this string by padding them on the right with a specified Unicode character, for a specified total length.</summary>
		/// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
		/// <param name="paddingChar">A Unicode padding character.</param>
		/// <returns>A new string that is equivalent to this instance, but left-aligned and padded on the right with as many <paramref name="paddingChar" /> characters as needed to create a length of <paramref name="totalWidth" />. However, if <paramref name="totalWidth" /> is less than the length of this instance, the method returns a reference to the existing instance. If <paramref name="totalWidth" /> is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="totalWidth" /> is less than zero.</exception>
		// Token: 0x0600052A RID: 1322 RVA: 0x000197C0 File Offset: 0x000179C0
		public unsafe string PadRight(int totalWidth, char paddingChar)
		{
			if (totalWidth < 0)
			{
				throw new ArgumentOutOfRangeException("totalWidth", "Non-negative number required.");
			}
			int length = this.Length;
			int num = totalWidth - length;
			if (num <= 0)
			{
				return this;
			}
			string text = string.FastAllocateString(totalWidth);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &this._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr2, ptr4, length);
				}
				for (int i = 0; i < num; i++)
				{
					ptr2[length + i] = paddingChar;
				}
			}
			return text;
		}

		/// <summary>Returns a new string in which a specified number of characters in the current instance beginning at a specified position have been deleted.</summary>
		/// <param name="startIndex">The zero-based position to begin deleting characters.</param>
		/// <param name="count">The number of characters to delete.</param>
		/// <returns>A new string that is equivalent to this instance except for the removed characters.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="startIndex" /> or <paramref name="count" /> is less than zero.  
		///  -or-  
		///  <paramref name="startIndex" /> plus <paramref name="count" /> specify a position outside this instance.</exception>
		// Token: 0x0600052B RID: 1323 RVA: 0x00019840 File Offset: 0x00017A40
		public unsafe string Remove(int startIndex, int count)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			int length = this.Length;
			if (count > length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Index and count must refer to a location within the string.");
			}
			if (count == 0)
			{
				return this;
			}
			int num = length - count;
			if (num == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(num);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &text._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr4, ptr2, startIndex);
					string.wstrcpy(ptr4 + startIndex, ptr2 + startIndex + count, num - startIndex);
				}
			}
			return text;
		}

		/// <summary>Returns a new string in which all the characters in the current instance, beginning at a specified position and continuing through the last position, have been deleted.</summary>
		/// <param name="startIndex">The zero-based position to begin deleting characters.</param>
		/// <returns>A new string that is equivalent to this string except for the removed characters.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> specifies a position that is not within this string.</exception>
		// Token: 0x0600052C RID: 1324 RVA: 0x000198E6 File Offset: 0x00017AE6
		public string Remove(int startIndex)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "startIndex must be less than length of string.");
			}
			return this.Substring(0, startIndex);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001991D File Offset: 0x00017B1D
		public string Replace(string oldValue, string newValue, bool ignoreCase, CultureInfo culture)
		{
			return this.ReplaceCore(oldValue, newValue, culture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00019930 File Offset: 0x00017B30
		public string Replace(string oldValue, string newValue, StringComparison comparisonType)
		{
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.CurrentCulture, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.InvariantCulture, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Replace(oldValue, newValue);
			case StringComparison.OrdinalIgnoreCase:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000199C4 File Offset: 0x00017BC4
		private unsafe string ReplaceCore(string oldValue, string newValue, CultureInfo culture, CompareOptions options)
		{
			if (oldValue == null)
			{
				throw new ArgumentNullException("oldValue");
			}
			if (oldValue.Length == 0)
			{
				throw new ArgumentException("String cannot be of zero length.", "oldValue");
			}
			if (newValue == null)
			{
				newValue = string.Empty;
			}
			CultureInfo cultureInfo = culture ?? CultureInfo.CurrentCulture;
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			int num = 0;
			int num2 = 0;
			bool flag = false;
			CompareInfo compareInfo = cultureInfo.CompareInfo;
			for (;;)
			{
				int num3 = compareInfo.IndexOf(this, oldValue, num, this.Length - num, options, &num2);
				if (num3 >= 0)
				{
					stringBuilder.Append(this, num, num3 - num);
					stringBuilder.Append(newValue);
					num = num3 + num2;
					flag = true;
				}
				else
				{
					if (!flag)
					{
						break;
					}
					stringBuilder.Append(this, num, this.Length - num);
				}
				if (num3 < 0)
				{
					goto Block_7;
				}
			}
			StringBuilderCache.Release(stringBuilder);
			return this;
			Block_7:
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		/// <summary>Returns a new string in which all occurrences of a specified Unicode character in this instance are replaced with another specified Unicode character.</summary>
		/// <param name="oldChar">The Unicode character to be replaced.</param>
		/// <param name="newChar">The Unicode character to replace all occurrences of <paramref name="oldChar" />.</param>
		/// <returns>A string that is equivalent to this instance except that all instances of <paramref name="oldChar" /> are replaced with <paramref name="newChar" />. If <paramref name="oldChar" /> is not found in the current instance, the method returns the current instance unchanged.</returns>
		// Token: 0x06000530 RID: 1328 RVA: 0x00019A84 File Offset: 0x00017C84
		public unsafe string Replace(char oldChar, char newChar)
		{
			if (oldChar == newChar)
			{
				return this;
			}
			int num = this.Length;
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				while (num > 0 && *ptr2 != oldChar)
				{
					num--;
					ptr2++;
				}
			}
			if (num == 0)
			{
				return this;
			}
			string text = string.FastAllocateString(this.Length);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr3 = ptr;
				fixed (char* ptr4 = &text._firstChar)
				{
					char* ptr5 = ptr4;
					int num2 = this.Length - num;
					if (num2 > 0)
					{
						string.wstrcpy(ptr5, ptr3, num2);
					}
					char* ptr6 = ptr3 + num2;
					char* ptr7 = ptr5 + num2;
					do
					{
						char c = *ptr6;
						if (c == oldChar)
						{
							c = newChar;
						}
						*ptr7 = c;
						num--;
						ptr6++;
						ptr7++;
					}
					while (num > 0);
				}
			}
			return text;
		}

		/// <summary>Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.</summary>
		/// <param name="oldValue">The string to be replaced.</param>
		/// <param name="newValue">The string to replace all occurrences of <paramref name="oldValue" />.</param>
		/// <returns>A string that is equivalent to the current string except that all instances of <paramref name="oldValue" /> are replaced with <paramref name="newValue" />. If <paramref name="oldValue" /> is not found in the current instance, the method returns the current instance unchanged.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="oldValue" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="oldValue" /> is the empty string ("").</exception>
		// Token: 0x06000531 RID: 1329 RVA: 0x00019B44 File Offset: 0x00017D44
		public unsafe string Replace(string oldValue, string newValue)
		{
			if (oldValue == null)
			{
				throw new ArgumentNullException("oldValue");
			}
			if (oldValue.Length == 0)
			{
				throw new ArgumentException("String cannot be of zero length.", "oldValue");
			}
			if (newValue == null)
			{
				newValue = string.Empty;
			}
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(span);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				int i = 0;
				int num = this.Length - oldValue.Length;
				IL_00B6:
				while (i <= num)
				{
					char* ptr3 = ptr2 + i;
					for (int j = 0; j < oldValue.Length; j++)
					{
						if (ptr3[j] != oldValue[j])
						{
							i++;
							goto IL_00B6;
						}
					}
					valueListBuilder.Append(i);
					i += oldValue.Length;
				}
			}
			if (valueListBuilder.Length == 0)
			{
				return this;
			}
			string text = this.ReplaceHelper(oldValue.Length, newValue, valueListBuilder.AsSpan());
			valueListBuilder.Dispose();
			return text;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00019C38 File Offset: 0x00017E38
		private unsafe string ReplaceHelper(int oldValueLength, string newValue, ReadOnlySpan<int> indices)
		{
			long num = (long)this.Length + (long)(newValue.Length - oldValueLength) * (long)indices.Length;
			if (num > 2147483647L)
			{
				throw new OutOfMemoryException();
			}
			string text = string.FastAllocateString((int)num);
			Span<char> span = new Span<char>(text.GetRawStringData(), text.Length);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < indices.Length; i++)
			{
				int num4 = *indices[i];
				int num5 = num4 - num2;
				if (num5 != 0)
				{
					this.AsSpan(num2, num5).CopyTo(span.Slice(num3));
					num3 += num5;
				}
				num2 = num4 + oldValueLength;
				newValue.AsSpan().CopyTo(span.Slice(num3));
				num3 += newValue.Length;
			}
			this.AsSpan(num2).CopyTo(span.Slice(num3));
			return text;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00019D0F File Offset: 0x00017F0F
		public string[] Split(char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(new ReadOnlySpan<char>(ref separator, 1), int.MaxValue, options);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00019D25 File Offset: 0x00017F25
		public string[] Split(char separator, int count, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(new ReadOnlySpan<char>(ref separator, 1), count, options);
		}

		/// <summary>Splits a string into substrings that are based on the characters in an array.</summary>
		/// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or <see langword="null" />.</param>
		/// <returns>An array whose elements contain the substrings from this instance that are delimited by one or more characters in <paramref name="separator" />. For more information, see the Remarks section.</returns>
		// Token: 0x06000535 RID: 1333 RVA: 0x00019D37 File Offset: 0x00017F37
		public string[] Split(params char[] separator)
		{
			return this.SplitInternal(separator, int.MaxValue, StringSplitOptions.None);
		}

		/// <summary>Splits a string into a maximum number of substrings based on the characters in an array. You also specify the maximum number of substrings to return.</summary>
		/// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or <see langword="null" />.</param>
		/// <param name="count">The maximum number of substrings to return.</param>
		/// <returns>An array whose elements contain the substrings in this instance that are delimited by one or more characters in <paramref name="separator" />. For more information, see the Remarks section.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative.</exception>
		// Token: 0x06000536 RID: 1334 RVA: 0x00019D4B File Offset: 0x00017F4B
		public string[] Split(char[] separator, int count)
		{
			return this.SplitInternal(separator, count, StringSplitOptions.None);
		}

		/// <summary>Splits a string into substrings based on the characters in an array. You can specify whether the substrings include empty array elements.</summary>
		/// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or <see langword="null" />.</param>
		/// <param name="options">
		///   <see cref="F:System.StringSplitOptions.RemoveEmptyEntries" /> to omit empty array elements from the array returned; or <see cref="F:System.StringSplitOptions.None" /> to include empty array elements in the array returned.</param>
		/// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in <paramref name="separator" />. For more information, see the Remarks section.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not one of the <see cref="T:System.StringSplitOptions" /> values.</exception>
		// Token: 0x06000537 RID: 1335 RVA: 0x00019D5B File Offset: 0x00017F5B
		public string[] Split(char[] separator, StringSplitOptions options)
		{
			return this.SplitInternal(separator, int.MaxValue, options);
		}

		/// <summary>Splits a string into a maximum number of substrings based on the characters in an array.</summary>
		/// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or <see langword="null" />.</param>
		/// <param name="count">The maximum number of substrings to return.</param>
		/// <param name="options">
		///   <see cref="F:System.StringSplitOptions.RemoveEmptyEntries" /> to omit empty array elements from the array returned; or <see cref="F:System.StringSplitOptions.None" /> to include empty array elements in the array returned.</param>
		/// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in <paramref name="separator" />. For more information, see the Remarks section.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not one of the <see cref="T:System.StringSplitOptions" /> values.</exception>
		// Token: 0x06000538 RID: 1336 RVA: 0x00019D6F File Offset: 0x00017F6F
		public string[] Split(char[] separator, int count, StringSplitOptions options)
		{
			return this.SplitInternal(separator, count, options);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00019D80 File Offset: 0x00017F80
		private unsafe string[] SplitInternal(ReadOnlySpan<char> separators, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				throw new ArgumentException(SR.Format("Illegal enum value: {0}.", options));
			}
			bool flag = options == StringSplitOptions.RemoveEmptyEntries;
			if (count == 0 || (flag && this.Length == 0))
			{
				return Array.Empty<string>();
			}
			if (count == 1)
			{
				return new string[] { this };
			}
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(span);
			this.MakeSeparatorList(separators, ref valueListBuilder);
			ReadOnlySpan<int> readOnlySpan = valueListBuilder.AsSpan();
			if (readOnlySpan.Length == 0)
			{
				return new string[] { this };
			}
			string[] array = (flag ? this.SplitOmitEmptyEntries(readOnlySpan, default(ReadOnlySpan<int>), 1, count) : this.SplitKeepEmptyEntries(readOnlySpan, default(ReadOnlySpan<int>), 1, count));
			valueListBuilder.Dispose();
			return array;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019E59 File Offset: 0x00018059
		public string[] Split(string separator, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(separator ?? string.Empty, null, int.MaxValue, options);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00019E72 File Offset: 0x00018072
		public string[] Split(string separator, int count, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(separator ?? string.Empty, null, count, options);
		}

		/// <summary>Splits a string into substrings based on the strings in an array. You can specify whether the substrings include empty array elements.</summary>
		/// <param name="separator">A string array that delimits the substrings in this string, an empty array that contains no delimiters, or <see langword="null" />.</param>
		/// <param name="options">
		///   <see cref="F:System.StringSplitOptions.RemoveEmptyEntries" /> to omit empty array elements from the array returned; or <see cref="F:System.StringSplitOptions.None" /> to include empty array elements in the array returned.</param>
		/// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in <paramref name="separator" />. For more information, see the Remarks section.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not one of the <see cref="T:System.StringSplitOptions" /> values.</exception>
		// Token: 0x0600053C RID: 1340 RVA: 0x00019E87 File Offset: 0x00018087
		public string[] Split(string[] separator, StringSplitOptions options)
		{
			return this.SplitInternal(null, separator, int.MaxValue, options);
		}

		/// <summary>Splits a string into a maximum number of substrings based on the strings in an array. You can specify whether the substrings include empty array elements.</summary>
		/// <param name="separator">A string array that delimits the substrings in this string, an empty array that contains no delimiters, or <see langword="null" />.</param>
		/// <param name="count">The maximum number of substrings to return.</param>
		/// <param name="options">
		///   <see cref="F:System.StringSplitOptions.RemoveEmptyEntries" /> to omit empty array elements from the array returned; or <see cref="F:System.StringSplitOptions.None" /> to include empty array elements in the array returned.</param>
		/// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in <paramref name="separator" />. For more information, see the Remarks section.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not one of the <see cref="T:System.StringSplitOptions" /> values.</exception>
		// Token: 0x0600053D RID: 1341 RVA: 0x00019E97 File Offset: 0x00018097
		public string[] Split(string[] separator, int count, StringSplitOptions options)
		{
			return this.SplitInternal(null, separator, count, options);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00019EA4 File Offset: 0x000180A4
		private unsafe string[] SplitInternal(string separator, string[] separators, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				throw new ArgumentException(SR.Format("Illegal enum value: {0}.", (int)options));
			}
			bool flag = options == StringSplitOptions.RemoveEmptyEntries;
			bool flag2 = separator != null;
			if (!flag2 && (separators == null || separators.Length == 0))
			{
				return this.SplitInternal(null, count, options);
			}
			if (count == 0 || (flag && this.Length == 0))
			{
				return Array.Empty<string>();
			}
			if (count == 1 || (flag2 && separator.Length == 0))
			{
				return new string[] { this };
			}
			if (flag2)
			{
				return this.SplitInternal(separator, count, options);
			}
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(span);
			Span<int> span2 = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder2 = new ValueListBuilder<int>(span2);
			this.MakeSeparatorList(separators, ref valueListBuilder, ref valueListBuilder2);
			ReadOnlySpan<int> readOnlySpan = valueListBuilder.AsSpan();
			ReadOnlySpan<int> readOnlySpan2 = valueListBuilder2.AsSpan();
			if (readOnlySpan.Length == 0)
			{
				return new string[] { this };
			}
			string[] array = (flag ? this.SplitOmitEmptyEntries(readOnlySpan, readOnlySpan2, 0, count) : this.SplitKeepEmptyEntries(readOnlySpan, readOnlySpan2, 0, count));
			valueListBuilder.Dispose();
			valueListBuilder2.Dispose();
			return array;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00019FDC File Offset: 0x000181DC
		private unsafe string[] SplitInternal(string separator, int count, StringSplitOptions options)
		{
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(span);
			this.MakeSeparatorList(separator, ref valueListBuilder);
			ReadOnlySpan<int> readOnlySpan = valueListBuilder.AsSpan();
			if (readOnlySpan.Length == 0)
			{
				return new string[] { this };
			}
			string[] array = ((options == StringSplitOptions.RemoveEmptyEntries) ? this.SplitOmitEmptyEntries(readOnlySpan, default(ReadOnlySpan<int>), separator.Length, count) : this.SplitKeepEmptyEntries(readOnlySpan, default(ReadOnlySpan<int>), separator.Length, count));
			valueListBuilder.Dispose();
			return array;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001A064 File Offset: 0x00018264
		private unsafe string[] SplitKeepEmptyEntries(ReadOnlySpan<int> sepList, ReadOnlySpan<int> lengthList, int defaultLength, int count)
		{
			int num = 0;
			int num2 = 0;
			count--;
			int num3 = ((sepList.Length < count) ? sepList.Length : count);
			string[] array = new string[num3 + 1];
			int num4 = 0;
			while (num4 < num3 && num < this.Length)
			{
				array[num2++] = this.Substring(num, *sepList[num4] - num);
				num = *sepList[num4] + (lengthList.IsEmpty ? defaultLength : (*lengthList[num4]));
				num4++;
			}
			if (num < this.Length && num3 >= 0)
			{
				array[num2] = this.Substring(num);
			}
			else if (num2 == num3)
			{
				array[num2] = string.Empty;
			}
			return array;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001A118 File Offset: 0x00018318
		private unsafe string[] SplitOmitEmptyEntries(ReadOnlySpan<int> sepList, ReadOnlySpan<int> lengthList, int defaultLength, int count)
		{
			int length = sepList.Length;
			int num = ((length < count) ? (length + 1) : count);
			string[] array = new string[num];
			int num2 = 0;
			int num3 = 0;
			int i = 0;
			while (i < length && num2 < this.Length)
			{
				if (*sepList[i] - num2 > 0)
				{
					array[num3++] = this.Substring(num2, *sepList[i] - num2);
				}
				num2 = *sepList[i] + (lengthList.IsEmpty ? defaultLength : (*lengthList[i]));
				if (num3 == count - 1)
				{
					while (i < length - 1)
					{
						if (num2 != *sepList[++i])
						{
							break;
						}
						num2 += (lengthList.IsEmpty ? defaultLength : (*lengthList[i]));
					}
					break;
				}
				i++;
			}
			if (num2 < this.Length)
			{
				array[num3++] = this.Substring(num2);
			}
			string[] array2 = array;
			if (num3 != num)
			{
				array2 = new string[num3];
				for (int j = 0; j < num3; j++)
				{
					array2[j] = array[j];
				}
			}
			return array2;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001A238 File Offset: 0x00018438
		private unsafe void MakeSeparatorList(ReadOnlySpan<char> separators, ref ValueListBuilder<int> sepListBuilder)
		{
			switch (separators.Length)
			{
			case 0:
			{
				for (int i = 0; i < this.Length; i++)
				{
					if (char.IsWhiteSpace(this[i]))
					{
						sepListBuilder.Append(i);
					}
				}
				return;
			}
			case 1:
			{
				char c = (char)(*separators[0]);
				for (int j = 0; j < this.Length; j++)
				{
					if (this[j] == c)
					{
						sepListBuilder.Append(j);
					}
				}
				return;
			}
			case 2:
			{
				char c = (char)(*separators[0]);
				char c2 = (char)(*separators[1]);
				for (int k = 0; k < this.Length; k++)
				{
					char c3 = this[k];
					if (c3 == c || c3 == c2)
					{
						sepListBuilder.Append(k);
					}
				}
				return;
			}
			case 3:
			{
				char c = (char)(*separators[0]);
				char c2 = (char)(*separators[1]);
				char c4 = (char)(*separators[2]);
				for (int l = 0; l < this.Length; l++)
				{
					char c5 = this[l];
					if (c5 == c || c5 == c2 || c5 == c4)
					{
						sepListBuilder.Append(l);
					}
				}
				return;
			}
			default:
			{
				string.ProbabilisticMap probabilisticMap = default(string.ProbabilisticMap);
				uint* ptr = (uint*)(&probabilisticMap);
				string.InitializeProbabilisticMap(ptr, separators);
				for (int m = 0; m < this.Length; m++)
				{
					char c6 = this[m];
					if (string.IsCharBitSet(ptr, (byte)c6) && string.IsCharBitSet(ptr, (byte)(c6 >> 8)) && separators.Contains(c6))
					{
						sepListBuilder.Append(m);
					}
				}
				return;
			}
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001A3C4 File Offset: 0x000185C4
		private void MakeSeparatorList(string separator, ref ValueListBuilder<int> sepListBuilder)
		{
			int length = separator.Length;
			for (int i = 0; i < this.Length; i++)
			{
				if (this[i] == separator[0] && length <= this.Length - i && (length == 1 || this.AsSpan(i, length).SequenceEqual<char>(separator)))
				{
					sepListBuilder.Append(i);
					i += length - 1;
				}
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001A42C File Offset: 0x0001862C
		private void MakeSeparatorList(string[] separators, ref ValueListBuilder<int> sepListBuilder, ref ValueListBuilder<int> lengthListBuilder)
		{
			int num = separators.Length;
			for (int i = 0; i < this.Length; i++)
			{
				foreach (string text in separators)
				{
					if (!string.IsNullOrEmpty(text))
					{
						int length = text.Length;
						if (this[i] == text[0] && length <= this.Length - i && (length == 1 || this.AsSpan(i, length).SequenceEqual<char>(text)))
						{
							sepListBuilder.Append(i);
							lengthListBuilder.Append(length);
							i += length - 1;
							break;
						}
					}
				}
			}
		}

		/// <summary>Retrieves a substring from this instance. The substring starts at a specified character position and continues to the end of the string.</summary>
		/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
		/// <returns>A string that is equivalent to the substring that begins at <paramref name="startIndex" /> in this instance, or <see cref="F:System.String.Empty" /> if <paramref name="startIndex" /> is equal to the length of this instance.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of this instance.</exception>
		// Token: 0x06000545 RID: 1349 RVA: 0x0001A4B9 File Offset: 0x000186B9
		public string Substring(int startIndex)
		{
			return this.Substring(startIndex, this.Length - startIndex);
		}

		/// <summary>Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.</summary>
		/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
		/// <param name="length">The number of characters in the substring.</param>
		/// <returns>A string that is equivalent to the substring of length <paramref name="length" /> that begins at <paramref name="startIndex" /> in this instance, or <see cref="F:System.String.Empty" /> if <paramref name="startIndex" /> is equal to the length of this instance and <paramref name="length" /> is zero.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> plus <paramref name="length" /> indicates a position not within this instance.  
		/// -or-  
		/// <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.</exception>
		// Token: 0x06000546 RID: 1350 RVA: 0x0001A4CC File Offset: 0x000186CC
		public string Substring(int startIndex, int length)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "startIndex cannot be larger than length of string.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (startIndex > this.Length - length)
			{
				throw new ArgumentOutOfRangeException("length", "Index and length must refer to a location within the string.");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			if (startIndex == 0 && length == this.Length)
			{
				return this;
			}
			return this.InternalSubString(startIndex, length);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001A554 File Offset: 0x00018754
		private unsafe string InternalSubString(int startIndex, int length)
		{
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &this._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr2, ptr4 + startIndex, length);
				}
			}
			return text;
		}

		/// <summary>Returns a copy of this string converted to lowercase.</summary>
		/// <returns>A string in lowercase.</returns>
		// Token: 0x06000548 RID: 1352 RVA: 0x0001A58C File Offset: 0x0001878C
		public string ToLower()
		{
			return CultureInfo.CurrentCulture.TextInfo.ToLower(this);
		}

		/// <summary>Returns a copy of this string converted to lowercase, using the casing rules of the specified culture.</summary>
		/// <param name="culture">An object that supplies culture-specific casing rules.</param>
		/// <returns>The lowercase equivalent of the current string.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x06000549 RID: 1353 RVA: 0x0001A59E File Offset: 0x0001879E
		public string ToLower(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToLower(this);
		}

		/// <summary>Returns a copy of this <see cref="T:System.String" /> object converted to lowercase using the casing rules of the invariant culture.</summary>
		/// <returns>The lowercase equivalent of the current string.</returns>
		// Token: 0x0600054A RID: 1354 RVA: 0x0001A5BA File Offset: 0x000187BA
		public string ToLowerInvariant()
		{
			return CultureInfo.InvariantCulture.TextInfo.ToLower(this);
		}

		/// <summary>Returns a copy of this string converted to uppercase.</summary>
		/// <returns>The uppercase equivalent of the current string.</returns>
		// Token: 0x0600054B RID: 1355 RVA: 0x0001A5CC File Offset: 0x000187CC
		public string ToUpper()
		{
			return CultureInfo.CurrentCulture.TextInfo.ToUpper(this);
		}

		/// <summary>Returns a copy of this string converted to uppercase, using the casing rules of the specified culture.</summary>
		/// <param name="culture">An object that supplies culture-specific casing rules.</param>
		/// <returns>The uppercase equivalent of the current string.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x0600054C RID: 1356 RVA: 0x0001A5DE File Offset: 0x000187DE
		public string ToUpper(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToUpper(this);
		}

		/// <summary>Returns a copy of this <see cref="T:System.String" /> object converted to uppercase using the casing rules of the invariant culture.</summary>
		/// <returns>The uppercase equivalent of the current string.</returns>
		// Token: 0x0600054D RID: 1357 RVA: 0x0001A5FA File Offset: 0x000187FA
		public string ToUpperInvariant()
		{
			return CultureInfo.InvariantCulture.TextInfo.ToUpper(this);
		}

		/// <summary>Removes all leading and trailing white-space characters from the current <see cref="T:System.String" /> object.</summary>
		/// <returns>The string that remains after all white-space characters are removed from the start and end of the current string. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
		// Token: 0x0600054E RID: 1358 RVA: 0x0001A60C File Offset: 0x0001880C
		public string Trim()
		{
			return this.TrimWhiteSpaceHelper(string.TrimType.Both);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001A615 File Offset: 0x00018815
		public unsafe string Trim(char trimChar)
		{
			return this.TrimHelper(&trimChar, 1, string.TrimType.Both);
		}

		/// <summary>Removes all leading and trailing occurrences of a set of characters specified in an array from the current <see cref="T:System.String" /> object.</summary>
		/// <param name="trimChars">An array of Unicode characters to remove, or <see langword="null" />.</param>
		/// <returns>The string that remains after all occurrences of the characters in the <paramref name="trimChars" /> parameter are removed from the start and end of the current string. If <paramref name="trimChars" /> is <see langword="null" /> or an empty array, white-space characters are removed instead. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
		// Token: 0x06000550 RID: 1360 RVA: 0x0001A624 File Offset: 0x00018824
		public unsafe string Trim(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				return this.TrimWhiteSpaceHelper(string.TrimType.Both);
			}
			fixed (char* ptr = &trimChars[0])
			{
				char* ptr2 = ptr;
				return this.TrimHelper(ptr2, trimChars.Length, string.TrimType.Both);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001A656 File Offset: 0x00018856
		public string TrimStart()
		{
			return this.TrimWhiteSpaceHelper(string.TrimType.Head);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001A65F File Offset: 0x0001885F
		public unsafe string TrimStart(char trimChar)
		{
			return this.TrimHelper(&trimChar, 1, string.TrimType.Head);
		}

		/// <summary>Removes all leading occurrences of a set of characters specified in an array from the current <see cref="T:System.String" /> object.</summary>
		/// <param name="trimChars">An array of Unicode characters to remove, or <see langword="null" />.</param>
		/// <returns>The string that remains after all occurrences of characters in the <paramref name="trimChars" /> parameter are removed from the start of the current string. If <paramref name="trimChars" /> is <see langword="null" /> or an empty array, white-space characters are removed instead.</returns>
		// Token: 0x06000553 RID: 1363 RVA: 0x0001A66C File Offset: 0x0001886C
		public unsafe string TrimStart(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				return this.TrimWhiteSpaceHelper(string.TrimType.Head);
			}
			fixed (char* ptr = &trimChars[0])
			{
				char* ptr2 = ptr;
				return this.TrimHelper(ptr2, trimChars.Length, string.TrimType.Head);
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001A69E File Offset: 0x0001889E
		public string TrimEnd()
		{
			return this.TrimWhiteSpaceHelper(string.TrimType.Tail);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001A6A7 File Offset: 0x000188A7
		public unsafe string TrimEnd(char trimChar)
		{
			return this.TrimHelper(&trimChar, 1, string.TrimType.Tail);
		}

		/// <summary>Removes all trailing occurrences of a set of characters specified in an array from the current <see cref="T:System.String" /> object.</summary>
		/// <param name="trimChars">An array of Unicode characters to remove, or <see langword="null" />.</param>
		/// <returns>The string that remains after all occurrences of the characters in the <paramref name="trimChars" /> parameter are removed from the end of the current string. If <paramref name="trimChars" /> is <see langword="null" /> or an empty array, Unicode white-space characters are removed instead. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
		// Token: 0x06000556 RID: 1366 RVA: 0x0001A6B4 File Offset: 0x000188B4
		public unsafe string TrimEnd(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				return this.TrimWhiteSpaceHelper(string.TrimType.Tail);
			}
			fixed (char* ptr = &trimChars[0])
			{
				char* ptr2 = ptr;
				return this.TrimHelper(ptr2, trimChars.Length, string.TrimType.Tail);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A6E8 File Offset: 0x000188E8
		private string TrimWhiteSpaceHelper(string.TrimType trimType)
		{
			int num = this.Length - 1;
			int num2 = 0;
			if (trimType != string.TrimType.Tail)
			{
				num2 = 0;
				while (num2 < this.Length && char.IsWhiteSpace(this[num2]))
				{
					num2++;
				}
			}
			if (trimType != string.TrimType.Head)
			{
				num = this.Length - 1;
				while (num >= num2 && char.IsWhiteSpace(this[num]))
				{
					num--;
				}
			}
			return this.CreateTrimmedString(num2, num);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A750 File Offset: 0x00018950
		private unsafe string TrimHelper(char* trimChars, int trimCharsLength, string.TrimType trimType)
		{
			int i = this.Length - 1;
			int j = 0;
			if (trimType != string.TrimType.Tail)
			{
				for (j = 0; j < this.Length; j++)
				{
					char c = this[j];
					int num = 0;
					while (num < trimCharsLength && trimChars[num] != c)
					{
						num++;
					}
					if (num == trimCharsLength)
					{
						break;
					}
				}
			}
			if (trimType != string.TrimType.Head)
			{
				for (i = this.Length - 1; i >= j; i--)
				{
					char c2 = this[i];
					int num2 = 0;
					while (num2 < trimCharsLength && trimChars[num2] != c2)
					{
						num2++;
					}
					if (num2 == trimCharsLength)
					{
						break;
					}
				}
			}
			return this.CreateTrimmedString(j, i);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001A7EC File Offset: 0x000189EC
		private string CreateTrimmedString(int start, int end)
		{
			int num = end - start + 1;
			if (num == this.Length)
			{
				return this;
			}
			if (num != 0)
			{
				return this.InternalSubString(start, num);
			}
			return string.Empty;
		}

		/// <summary>Returns a value indicating whether a specified substring occurs within this string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="value" /> parameter occurs within this string, or if <paramref name="value" /> is the empty string (""); otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x0600055A RID: 1370 RVA: 0x0001A81B File Offset: 0x00018A1B
		public bool Contains(string value)
		{
			return this.IndexOf(value, StringComparison.Ordinal) >= 0;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A82B File Offset: 0x00018A2B
		public bool Contains(string value, StringComparison comparisonType)
		{
			return this.IndexOf(value, comparisonType) >= 0;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A83B File Offset: 0x00018A3B
		public bool Contains(char value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A84A File Offset: 0x00018A4A
		public bool Contains(char value, StringComparison comparisonType)
		{
			return this.IndexOf(value, comparisonType) != -1;
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified Unicode character in this string.</summary>
		/// <param name="value">A Unicode character to seek.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> if that character is found, or -1 if it is not.</returns>
		// Token: 0x0600055E RID: 1374 RVA: 0x0001A85A File Offset: 0x00018A5A
		public int IndexOf(char value)
		{
			return SpanHelpers.IndexOf(ref this._firstChar, value, this.Length);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified Unicode character in this string. The search starts at a specified character position.</summary>
		/// <param name="value">A Unicode character to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> from the start of the string if that character is found, or -1 if it is not.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than 0 (zero) or greater than the length of the string.</exception>
		// Token: 0x0600055F RID: 1375 RVA: 0x0001A86E File Offset: 0x00018A6E
		public int IndexOf(char value, int startIndex)
		{
			return this.IndexOf(value, startIndex, this.Length - startIndex);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001A880 File Offset: 0x00018A80
		public int IndexOf(char value, StringComparison comparisonType)
		{
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.OrdinalIgnoreCase);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified character in this instance. The search starts at a specified character position and examines a specified number of character positions.</summary>
		/// <param name="value">A Unicode character to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> from the start of the string if that character is found, or -1 if it is not.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> or <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// <paramref name="startIndex" /> is greater than the length of this string.  
		/// -or-  
		/// <paramref name="count" /> is greater than the length of this string minus <paramref name="startIndex" />.</exception>
		// Token: 0x06000561 RID: 1377 RVA: 0x0001A924 File Offset: 0x00018B24
		public int IndexOf(char value, int startIndex, int count)
		{
			if (startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			int num = SpanHelpers.IndexOf(Unsafe.Add<char>(ref this._firstChar, startIndex), value, count);
			if (num != -1)
			{
				return num + startIndex;
			}
			return num;
		}

		/// <summary>Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters.</summary>
		/// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
		/// <returns>The zero-based index position of the first occurrence in this instance where any character in <paramref name="anyOf" /> was found; -1 if no character in <paramref name="anyOf" /> was found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="anyOf" /> is <see langword="null" />.</exception>
		// Token: 0x06000562 RID: 1378 RVA: 0x0001A982 File Offset: 0x00018B82
		public int IndexOfAny(char[] anyOf)
		{
			return this.IndexOfAny(anyOf, 0, this.Length);
		}

		/// <summary>Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters. The search starts at a specified character position.</summary>
		/// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <returns>The zero-based index position of the first occurrence in this instance where any character in <paramref name="anyOf" /> was found; -1 if no character in <paramref name="anyOf" /> was found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="anyOf" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// <paramref name="startIndex" /> is greater than the number of characters in this instance.</exception>
		// Token: 0x06000563 RID: 1379 RVA: 0x0001A992 File Offset: 0x00018B92
		public int IndexOfAny(char[] anyOf, int startIndex)
		{
			return this.IndexOfAny(anyOf, startIndex, this.Length - startIndex);
		}

		/// <summary>Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters. The search starts at a specified character position and examines a specified number of character positions.</summary>
		/// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <returns>The zero-based index position of the first occurrence in this instance where any character in <paramref name="anyOf" /> was found; -1 if no character in <paramref name="anyOf" /> was found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="anyOf" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> or <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// <paramref name="count" /> + <paramref name="startIndex" /> is greater than the number of characters in this instance.</exception>
		// Token: 0x06000564 RID: 1380 RVA: 0x0001A9A4 File Offset: 0x00018BA4
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			if (anyOf == null)
			{
				throw new ArgumentNullException("anyOf");
			}
			if (startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (anyOf.Length == 2)
			{
				return this.IndexOfAny(anyOf[0], anyOf[1], startIndex, count);
			}
			if (anyOf.Length == 3)
			{
				return this.IndexOfAny(anyOf[0], anyOf[1], anyOf[2], startIndex, count);
			}
			if (anyOf.Length > 3)
			{
				return this.IndexOfCharArray(anyOf, startIndex, count);
			}
			if (anyOf.Length == 1)
			{
				return this.IndexOf(anyOf[0], startIndex, count);
			}
			return -1;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001AA44 File Offset: 0x00018C44
		private unsafe int IndexOfAny(char value1, char value2, int startIndex, int count)
		{
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + startIndex;
				while (count > 0)
				{
					char c = *ptr3;
					if (c == value1 || c == value2)
					{
						return (int)((long)(ptr3 - ptr2));
					}
					c = ptr3[1];
					if (c == value1 || c == value2)
					{
						if (count != 1)
						{
							return (int)((long)(ptr3 - ptr2)) + 1;
						}
						return -1;
					}
					else
					{
						ptr3 += 2;
						count -= 2;
					}
				}
				return -1;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001AAA8 File Offset: 0x00018CA8
		private unsafe int IndexOfAny(char value1, char value2, char value3, int startIndex, int count)
		{
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + startIndex;
				while (count > 0)
				{
					char c = *ptr3;
					if (c == value1 || c == value2 || c == value3)
					{
						return (int)((long)(ptr3 - ptr2));
					}
					ptr3++;
					count--;
				}
				return -1;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001AAF0 File Offset: 0x00018CF0
		private unsafe int IndexOfCharArray(char[] anyOf, int startIndex, int count)
		{
			string.ProbabilisticMap probabilisticMap = default(string.ProbabilisticMap);
			uint* ptr = (uint*)(&probabilisticMap);
			string.InitializeProbabilisticMap(ptr, anyOf);
			fixed (char* ptr2 = &this._firstChar)
			{
				char* ptr3 = ptr2;
				char* ptr4 = ptr3 + startIndex;
				while (count > 0)
				{
					int num = (int)(*ptr4);
					if (string.IsCharBitSet(ptr, (byte)num) && string.IsCharBitSet(ptr, (byte)(num >> 8)) && string.ArrayContains((char)num, anyOf))
					{
						return (int)((long)(ptr4 - ptr3));
					}
					count--;
					ptr4++;
				}
				return -1;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001AB6C File Offset: 0x00018D6C
		private unsafe static void InitializeProbabilisticMap(uint* charMap, ReadOnlySpan<char> anyOf)
		{
			bool flag = false;
			for (int i = 0; i < anyOf.Length; i++)
			{
				int num = (int)(*anyOf[i]);
				string.SetCharBit(charMap, (byte)num);
				num >>= 8;
				if (num == 0)
				{
					flag = true;
				}
				else
				{
					string.SetCharBit(charMap, (byte)num);
				}
			}
			if (flag)
			{
				*charMap |= 1U;
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001ABC0 File Offset: 0x00018DC0
		private static bool ArrayContains(char searchChar, char[] anyOf)
		{
			for (int i = 0; i < anyOf.Length; i++)
			{
				if (anyOf[i] == searchChar)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001ABE4 File Offset: 0x00018DE4
		private unsafe static bool IsCharBitSet(uint* charMap, byte value)
		{
			return (charMap[value & 7] & (1U << (value >> 3))) > 0U;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001ABFB File Offset: 0x00018DFB
		private unsafe static void SetCharBit(uint* charMap, byte value)
		{
			charMap[value & 7] |= 1U << (value >> 3);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified string in this instance.</summary>
		/// <param name="value">The string to seek.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is 0.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x0600056C RID: 1388 RVA: 0x0001AC11 File Offset: 0x00018E11
		public int IndexOf(string value)
		{
			return this.IndexOf(value, StringComparison.CurrentCulture);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified string in this instance. The search starts at a specified character position.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> from the start of the current instance if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than 0 (zero) or greater than the length of this string.</exception>
		// Token: 0x0600056D RID: 1389 RVA: 0x0001AC1B File Offset: 0x00018E1B
		public int IndexOf(string value, int startIndex)
		{
			return this.IndexOf(value, startIndex, StringComparison.CurrentCulture);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified string in this instance. The search starts at a specified character position and examines a specified number of character positions.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> from the start of the current instance if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> or <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// <paramref name="startIndex" /> is greater than the length of this string.  
		/// -or-  
		/// <paramref name="count" /> is greater than the length of this string minus <paramref name="startIndex" />.</exception>
		// Token: 0x0600056E RID: 1390 RVA: 0x0001AC28 File Offset: 0x00018E28
		public int IndexOf(string value, int startIndex, int count)
		{
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			return this.IndexOf(value, startIndex, count, StringComparison.CurrentCulture);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified string in the current <see cref="T:System.String" /> object. A parameter specifies the type of search to use for the specified string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>The index position of the <paramref name="value" /> parameter if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is 0.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x0600056F RID: 1391 RVA: 0x0001AC7B File Offset: 0x00018E7B
		public int IndexOf(string value, StringComparison comparisonType)
		{
			return this.IndexOf(value, 0, this.Length, comparisonType);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified string in the current <see cref="T:System.String" /> object. Parameters specify the starting search position in the current string and the type of search to use for the specified string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>The zero-based index position of the <paramref name="value" /> parameter from the start of the current instance if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than 0 (zero) or greater than the length of this string.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x06000570 RID: 1392 RVA: 0x0001AC8C File Offset: 0x00018E8C
		public int IndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return this.IndexOf(value, startIndex, this.Length - startIndex, comparisonType);
		}

		/// <summary>Reports the zero-based index of the first occurrence of the specified string in the current <see cref="T:System.String" /> object. Parameters specify the starting search position in the current string, the number of characters in the current string to search, and the type of search to use for the specified string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>The zero-based index position of the <paramref name="value" /> parameter from the start of the current instance if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> or <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// <paramref name="startIndex" /> is greater than the length of this instance.  
		/// -or-  
		/// <paramref name="count" /> is greater than the length of this string minus <paramref name="startIndex" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x06000571 RID: 1393 RVA: 0x0001ACA0 File Offset: 0x00018EA0
		public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex > this.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IndexOf(this, value, startIndex, count, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CompareInfo.Invariant.IndexOfOrdinal(this, value, startIndex, count, false);
			case StringComparison.OrdinalIgnoreCase:
				return CompareInfo.Invariant.IndexOfOrdinal(this, value, startIndex, count, true);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		/// <summary>Reports the zero-based index position of the last occurrence of a specified Unicode character within this instance.</summary>
		/// <param name="value">The Unicode character to seek.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> if that character is found, or -1 if it is not.</returns>
		// Token: 0x06000572 RID: 1394 RVA: 0x0001AD91 File Offset: 0x00018F91
		public int LastIndexOf(char value)
		{
			return SpanHelpers.LastIndexOf(ref this._firstChar, value, this.Length);
		}

		/// <summary>Reports the zero-based index position of the last occurrence of a specified Unicode character within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string.</summary>
		/// <param name="value">The Unicode character to seek.</param>
		/// <param name="startIndex">The starting position of the search. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> if that character is found, or -1 if it is not found or if the current instance equals <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is less than zero or greater than or equal to the length of this instance.</exception>
		// Token: 0x06000573 RID: 1395 RVA: 0x0001ADA5 File Offset: 0x00018FA5
		public int LastIndexOf(char value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1);
		}

		/// <summary>Reports the zero-based index position of the last occurrence of the specified Unicode character in a substring within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.</summary>
		/// <param name="value">The Unicode character to seek.</param>
		/// <param name="startIndex">The starting position of the search. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <returns>The zero-based index position of <paramref name="value" /> if that character is found, or -1 if it is not found or if the current instance equals <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is less than zero or greater than or equal to the length of this instance.  
		///  -or-  
		///  The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> - <paramref name="count" /> + 1 is less than zero.</exception>
		// Token: 0x06000574 RID: 1396 RVA: 0x0001ADB4 File Offset: 0x00018FB4
		public int LastIndexOf(char value, int startIndex, int count)
		{
			if (this.Length == 0)
			{
				return -1;
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > startIndex + 1)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			int num = startIndex + 1 - count;
			int num2 = SpanHelpers.LastIndexOf(Unsafe.Add<char>(ref this._firstChar, num), value, count);
			if (num2 != -1)
			{
				return num2 + num;
			}
			return num2;
		}

		/// <summary>Reports the zero-based index position of the last occurrence in this instance of one or more characters specified in a Unicode array.</summary>
		/// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
		/// <returns>The index position of the last occurrence in this instance where any character in <paramref name="anyOf" /> was found; -1 if no character in <paramref name="anyOf" /> was found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="anyOf" /> is <see langword="null" />.</exception>
		// Token: 0x06000575 RID: 1397 RVA: 0x0001AE1D File Offset: 0x0001901D
		public int LastIndexOfAny(char[] anyOf)
		{
			return this.LastIndexOfAny(anyOf, this.Length - 1, this.Length);
		}

		/// <summary>Reports the zero-based index position of the last occurrence in this instance of one or more characters specified in a Unicode array. The search starts at a specified character position and proceeds backward toward the beginning of the string.</summary>
		/// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
		/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <returns>The index position of the last occurrence in this instance where any character in <paramref name="anyOf" /> was found; -1 if no character in <paramref name="anyOf" /> was found or if the current instance equals <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="anyOf" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> specifies a position that is not within this instance.</exception>
		// Token: 0x06000576 RID: 1398 RVA: 0x0001AE34 File Offset: 0x00019034
		public int LastIndexOfAny(char[] anyOf, int startIndex)
		{
			return this.LastIndexOfAny(anyOf, startIndex, startIndex + 1);
		}

		/// <summary>Reports the zero-based index position of the last occurrence in this instance of one or more characters specified in a Unicode array. The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.</summary>
		/// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
		/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <returns>The index position of the last occurrence in this instance where any character in <paramref name="anyOf" /> was found; -1 if no character in <paramref name="anyOf" /> was found or if the current instance equals <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="anyOf" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="count" /> or <paramref name="startIndex" /> is negative.  
		///  -or-  
		///  The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> minus <paramref name="count" /> + 1 is less than zero.</exception>
		// Token: 0x06000577 RID: 1399 RVA: 0x0001AE44 File Offset: 0x00019044
		public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
		{
			if (anyOf == null)
			{
				throw new ArgumentNullException("anyOf");
			}
			if (this.Length == 0)
			{
				return -1;
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count - 1 > startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (anyOf.Length > 1)
			{
				return this.LastIndexOfCharArray(anyOf, startIndex, count);
			}
			if (anyOf.Length == 1)
			{
				return this.LastIndexOf(anyOf[0], startIndex, count);
			}
			return -1;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001AEC0 File Offset: 0x000190C0
		private unsafe int LastIndexOfCharArray(char[] anyOf, int startIndex, int count)
		{
			string.ProbabilisticMap probabilisticMap = default(string.ProbabilisticMap);
			uint* ptr = (uint*)(&probabilisticMap);
			string.InitializeProbabilisticMap(ptr, anyOf);
			fixed (char* ptr2 = &this._firstChar)
			{
				char* ptr3 = ptr2;
				char* ptr4 = ptr3 + startIndex;
				while (count > 0)
				{
					int num = (int)(*ptr4);
					if (string.IsCharBitSet(ptr, (byte)num) && string.IsCharBitSet(ptr, (byte)(num >> 8)) && string.ArrayContains((char)num, anyOf))
					{
						return (int)((long)(ptr4 - ptr3));
					}
					count--;
					ptr4--;
				}
				return -1;
			}
		}

		/// <summary>Reports the zero-based index position of the last occurrence of a specified string within this instance.</summary>
		/// <param name="value">The string to seek.</param>
		/// <returns>The zero-based starting index position of <paramref name="value" /> if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is the last index position in this instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x06000579 RID: 1401 RVA: 0x0001AF3A File Offset: 0x0001913A
		public int LastIndexOf(string value)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length, StringComparison.CurrentCulture);
		}

		/// <summary>Reports the zero-based index position of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <returns>The zero-based starting index position of <paramref name="value" /> if that string is found, or -1 if it is not found or if the current instance equals <see cref="F:System.String.Empty" />. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is the smaller of <paramref name="startIndex" /> and the last index position in this instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is less than zero or greater than the length of the current instance.  
		///  -or-  
		///  The current instance equals <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is less than -1 or greater than zero.</exception>
		// Token: 0x0600057A RID: 1402 RVA: 0x0001AF52 File Offset: 0x00019152
		public int LastIndexOf(string value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1, StringComparison.CurrentCulture);
		}

		/// <summary>Reports the zero-based index position of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <returns>The zero-based starting index position of <paramref name="value" /> if that string is found, or -1 if it is not found or if the current instance equals <see cref="F:System.String.Empty" />. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is the smaller of <paramref name="startIndex" /> and the last index position in this instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative.  
		/// -or-  
		/// The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is greater than the length of this instance.  
		/// -or-  
		/// The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> - <paramref name="count" />+ 1 specifies a position that is not within this instance.  
		/// -or-  
		/// The current instance equals <see cref="F:System.String.Empty" /> and <paramref name="start" /> is less than -1 or greater than zero.  
		/// -or-  
		/// The current instance equals <see cref="F:System.String.Empty" /> and <paramref name="count" /> is greater than 1.</exception>
		// Token: 0x0600057B RID: 1403 RVA: 0x0001AF60 File Offset: 0x00019160
		public int LastIndexOf(string value, int startIndex, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			return this.LastIndexOf(value, startIndex, count, StringComparison.CurrentCulture);
		}

		/// <summary>Reports the zero-based index of the last occurrence of a specified string within the current <see cref="T:System.String" /> object. A parameter specifies the type of search to use for the specified string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>The zero-based starting index position of the <paramref name="value" /> parameter if that string is found, or -1 if it is not. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is the last index position in this instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x0600057C RID: 1404 RVA: 0x0001AF80 File Offset: 0x00019180
		public int LastIndexOf(string value, StringComparison comparisonType)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length, comparisonType);
		}

		/// <summary>Reports the zero-based index of the last occurrence of a specified string within the current <see cref="T:System.String" /> object. The search starts at a specified character position and proceeds backward toward the beginning of the string. A parameter specifies the type of comparison to perform when searching for the specified string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>The zero-based starting index position of the <paramref name="value" /> parameter if that string is found, or -1 if it is not found or if the current instance equals <see cref="F:System.String.Empty" />. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is the smaller of <paramref name="startIndex" /> and the last index position in this instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is less than zero or greater than the length of the current instance.  
		///  -or-  
		///  The current instance equals <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is less than -1 or greater than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x0600057D RID: 1405 RVA: 0x0001AF98 File Offset: 0x00019198
		public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1, comparisonType);
		}

		/// <summary>Reports the zero-based index position of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string for the specified number of character positions. A parameter specifies the type of comparison to perform when searching for the specified string.</summary>
		/// <param name="value">The string to seek.</param>
		/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of this instance.</param>
		/// <param name="count">The number of character positions to examine.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
		/// <returns>The zero-based starting index position of the <paramref name="value" /> parameter if that string is found, or -1 if it is not found or if the current instance equals <see cref="F:System.String.Empty" />. If <paramref name="value" /> is <see cref="F:System.String.Empty" />, the return value is the smaller of <paramref name="startIndex" /> and the last index position in this instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative.  
		/// -or-  
		/// The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is negative.  
		/// -or-  
		/// The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> is greater than the length of this instance.  
		/// -or-  
		/// The current instance does not equal <see cref="F:System.String.Empty" />, and <paramref name="startIndex" /> + 1 - <paramref name="count" /> specifies a position that is not within this instance.  
		/// -or-  
		/// The current instance equals <see cref="F:System.String.Empty" /> and <paramref name="start" /> is less than -1 or greater than zero.  
		/// -or-  
		/// The current instance equals <see cref="F:System.String.Empty" /> and <paramref name="count" /> is greater than 1.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		// Token: 0x0600057E RID: 1406 RVA: 0x0001AFA8 File Offset: 0x000191A8
		public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				if (value.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (startIndex < 0 || startIndex > this.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (startIndex == this.Length)
				{
					startIndex--;
					if (count > 0)
					{
						count--;
					}
				}
				if (count < 0 || startIndex - count + 1 < 0)
				{
					throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
				}
				if (value.Length == 0)
				{
					return startIndex;
				}
				switch (comparisonType)
				{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.InvariantCulture:
					return CompareInfo.Invariant.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
				case StringComparison.InvariantCultureIgnoreCase:
					return CompareInfo.Invariant.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return CompareInfo.Invariant.LastIndexOfOrdinal(this, value, startIndex, count, false);
				case StringComparison.OrdinalIgnoreCase:
					return CompareInfo.Invariant.LastIndexOfOrdinal(this, value, startIndex, count, true);
				default:
					throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
				}
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by an array of Unicode characters.</summary>
		/// <param name="value">An array of Unicode characters.</param>
		// Token: 0x0600057F RID: 1407
		[PreserveDependency("CreateString(System.Char[])", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char[] value);

		// Token: 0x06000580 RID: 1408 RVA: 0x0001B0D4 File Offset: 0x000192D4
		private unsafe static string Ctor(char[] value)
		{
			if (value == null || value.Length == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(value.Length);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char[] array = value)
				{
					char* ptr3;
					if (value == null || array.Length == 0)
					{
						ptr3 = null;
					}
					else
					{
						ptr3 = &array[0];
					}
					string.wstrcpy(ptr2, ptr3, value.Length);
					ptr = null;
				}
				return text;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by an array of Unicode characters, a starting character position within that array, and a length.</summary>
		/// <param name="value">An array of Unicode characters.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <param name="length">The number of characters within <paramref name="value" /> to use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.  
		/// -or-  
		/// The sum of <paramref name="startIndex" /> and <paramref name="length" /> is greater than the number of elements in <paramref name="value" />.</exception>
		// Token: 0x06000581 RID: 1409
		[PreserveDependency("CreateString(System.Char[], System.Int32, System.Int32)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char[] value, int startIndex, int length);

		// Token: 0x06000582 RID: 1410 RVA: 0x0001B128 File Offset: 0x00019328
		private unsafe static string Ctor(char[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (startIndex > value.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char[] array = value)
				{
					char* ptr3;
					if (value == null || array.Length == 0)
					{
						ptr3 = null;
					}
					else
					{
						ptr3 = &array[0];
					}
					string.wstrcpy(ptr2, ptr3 + startIndex, length);
					ptr = null;
				}
				return text;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by a specified pointer to an array of Unicode characters.</summary>
		/// <param name="value">A pointer to a null-terminated array of Unicode characters.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The current process does not have read access to all the addressed characters.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> specifies an array that contains an invalid Unicode character, or <paramref name="value" /> specifies an address less than 64000.</exception>
		// Token: 0x06000583 RID: 1411
		[CLSCompliant(false)]
		[PreserveDependency("CreateString(System.Char*)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value);

		// Token: 0x06000584 RID: 1412 RVA: 0x0001B1C4 File Offset: 0x000193C4
		private unsafe static string Ctor(char* ptr)
		{
			if (ptr == null)
			{
				return string.Empty;
			}
			int num = string.wcslen(ptr);
			if (num == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(num);
			fixed (char* ptr2 = &text._firstChar)
			{
				string.wstrcpy(ptr2, ptr, num);
			}
			return text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by a specified pointer to an array of Unicode characters, a starting character position within that array, and a length.</summary>
		/// <param name="value">A pointer to an array of Unicode characters.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <param name="length">The number of characters within <paramref name="value" /> to use.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero, <paramref name="value" /> + <paramref name="startIndex" /> cause a pointer overflow, or the current process does not have read access to all the addressed characters.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> specifies an array that contains an invalid Unicode character, or <paramref name="value" /> + <paramref name="startIndex" /> specifies an address less than 64000.</exception>
		// Token: 0x06000585 RID: 1413
		[PreserveDependency("CreateString(System.Char*, System.Int32, System.Int32)", "System.String")]
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value, int startIndex, int length);

		// Token: 0x06000586 RID: 1414 RVA: 0x0001B208 File Offset: 0x00019408
		private unsafe static string Ctor(char* ptr, int startIndex, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			char* ptr2 = ptr + startIndex;
			if (ptr2 < ptr)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Pointer startIndex and length do not refer to a valid string.");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			if (ptr == null)
			{
				throw new ArgumentOutOfRangeException("ptr", "Pointer startIndex and length do not refer to a valid string.");
			}
			string text = string.FastAllocateString(length);
			fixed (char* ptr3 = &text._firstChar)
			{
				string.wstrcpy(ptr3, ptr2, length);
			}
			return text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by a pointer to an array of 8-bit signed integers.</summary>
		/// <param name="value">A pointer to a null-terminated array of 8-bit signed integers. The integers are interpreted using the current system code page encoding (that is, the encoding specified by <see cref="P:System.Text.Encoding.Default" />).</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">A new instance of <see cref="T:System.String" /> could not be initialized using <paramref name="value" />, assuming <paramref name="value" /> is encoded in ANSI.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of the new string to initialize, which is determined by the null termination character of <paramref name="value" />, is too large to allocate.</exception>
		/// <exception cref="T:System.AccessViolationException">
		///   <paramref name="value" /> specifies an invalid address.</exception>
		// Token: 0x06000587 RID: 1415
		[PreserveDependency("CreateString(System.SByte*)", "System.String")]
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value);

		// Token: 0x06000588 RID: 1416 RVA: 0x0001B290 File Offset: 0x00019490
		private unsafe static string Ctor(sbyte* value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			int num = new ReadOnlySpan<byte>((void*)value, int.MaxValue).IndexOf(0);
			if (num < 0)
			{
				throw new ArgumentException("The string must be null-terminated.");
			}
			return string.CreateStringForSByteConstructor((byte*)value, num);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by a specified pointer to an array of 8-bit signed integers, a starting position within that array, and a length.</summary>
		/// <param name="value">A pointer to an array of 8-bit signed integers. The integers are interpreted using the current system code page encoding (that is, the encoding specified by <see cref="P:System.Text.Encoding.Default" />).</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <param name="length">The number of characters within <paramref name="value" /> to use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.  
		/// -or-  
		/// The address specified by <paramref name="value" /> + <paramref name="startIndex" /> is too large for the current platform; that is, the address calculation overflowed.  
		/// -or-  
		/// The length of the new string to initialize is too large to allocate.</exception>
		/// <exception cref="T:System.ArgumentException">The address specified by <paramref name="value" /> + <paramref name="startIndex" /> is less than 64K.  
		///  -or-  
		///  A new instance of <see cref="T:System.String" /> could not be initialized using <paramref name="value" />, assuming <paramref name="value" /> is encoded in ANSI.</exception>
		/// <exception cref="T:System.AccessViolationException">
		///   <paramref name="value" />, <paramref name="startIndex" />, and <paramref name="length" /> collectively specify an invalid address.</exception>
		// Token: 0x06000589 RID: 1417
		[CLSCompliant(false)]
		[PreserveDependency("CreateString(System.SByte*, System.Int32, System.Int32)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value, int startIndex, int length);

		// Token: 0x0600058A RID: 1418 RVA: 0x0001B2D4 File Offset: 0x000194D4
		private unsafe static string Ctor(sbyte* value, int startIndex, int length)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (value == null)
			{
				if (length == 0)
				{
					return string.Empty;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				byte* ptr = (byte*)(value + startIndex);
				if (ptr < (byte*)value)
				{
					throw new ArgumentOutOfRangeException("value", "Pointer startIndex and length do not refer to a valid string.");
				}
				return string.CreateStringForSByteConstructor(ptr, length);
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001B341 File Offset: 0x00019541
		private unsafe static string CreateStringForSByteConstructor(byte* pb, int numBytes)
		{
			if (numBytes == 0)
			{
				return string.Empty;
			}
			return Encoding.UTF8.GetString(pb, numBytes);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by a specified pointer to an array of 8-bit signed integers, a starting position within that array, a length, and an <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <param name="value">A pointer to an array of 8-bit signed integers.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <param name="length">The number of characters within <paramref name="value" /> to use.</param>
		/// <param name="enc">An object that specifies how the array referenced by <paramref name="value" /> is encoded. If <paramref name="enc" /> is <see langword="null" />, ANSI encoding is assumed.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.  
		/// -or-  
		/// The address specified by <paramref name="value" /> + <paramref name="startIndex" /> is too large for the current platform; that is, the address calculation overflowed.  
		/// -or-  
		/// The length of the new string to initialize is too large to allocate.</exception>
		/// <exception cref="T:System.ArgumentException">The address specified by <paramref name="value" /> + <paramref name="startIndex" /> is less than 64K.  
		///  -or-  
		///  A new instance of <see cref="T:System.String" /> could not be initialized using <paramref name="value" />, assuming <paramref name="value" /> is encoded as specified by <paramref name="enc" />.</exception>
		/// <exception cref="T:System.AccessViolationException">
		///   <paramref name="value" />, <paramref name="startIndex" />, and <paramref name="length" /> collectively specify an invalid address.</exception>
		// Token: 0x0600058C RID: 1420
		[CLSCompliant(false)]
		[PreserveDependency("CreateString(System.SByte*, System.Int32, System.Int32, System.Text.Encoding)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value, int startIndex, int length, Encoding enc);

		// Token: 0x0600058D RID: 1421 RVA: 0x0001B358 File Offset: 0x00019558
		private unsafe static string Ctor(sbyte* value, int startIndex, int length, Encoding enc)
		{
			if (enc == null)
			{
				return new string(value, startIndex, length);
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (value == null)
			{
				if (length == 0)
				{
					return string.Empty;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				byte* ptr = (byte*)(value + startIndex);
				if (ptr < (byte*)value)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Pointer startIndex and length do not refer to a valid string.");
				}
				return enc.GetString(new ReadOnlySpan<byte>((void*)ptr, length));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.String" /> class to the value indicated by a specified Unicode character repeated a specified number of times.</summary>
		/// <param name="c">A Unicode character.</param>
		/// <param name="count">The number of times <paramref name="c" /> occurs.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.</exception>
		// Token: 0x0600058E RID: 1422
		[PreserveDependency("CreateString(System.Char, System.Int32)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char c, int count);

		// Token: 0x0600058F RID: 1423 RVA: 0x0001B3D8 File Offset: 0x000195D8
		private unsafe static string Ctor(char c, int count)
		{
			if (count > 0)
			{
				string text = string.FastAllocateString(count);
				if (c != '\0')
				{
					fixed (char* ptr = &text._firstChar)
					{
						uint* ptr2 = (uint*)ptr;
						uint num = (uint)(((uint)c << 16) | c);
						uint* ptr3 = ptr2;
						if (count >= 4)
						{
							count -= 4;
							do
							{
								*ptr3 = num;
								ptr3[1] = num;
								ptr3 += 2;
								count -= 4;
							}
							while (count >= 0);
						}
						if ((count & 2) != 0)
						{
							*ptr3 = num;
							ptr3++;
						}
						if ((count & 1) != 0)
						{
							*(short*)ptr3 = (short)c;
						}
					}
				}
				return text;
			}
			if (count == 0)
			{
				return string.Empty;
			}
			throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
		}

		// Token: 0x06000590 RID: 1424
		[PreserveDependency("CreateString(System.ReadOnlySpan`1<System.Char>)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(ReadOnlySpan<char> value);

		// Token: 0x06000591 RID: 1425 RVA: 0x0001B458 File Offset: 0x00019658
		private unsafe static string Ctor(ReadOnlySpan<char> value)
		{
			if (value.Length == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(value.Length);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* reference = MemoryMarshal.GetReference<char>(value))
				{
					char* ptr3 = reference;
					string.wstrcpy(ptr2, ptr3, value.Length);
					ptr = null;
				}
				return text;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001B4A8 File Offset: 0x000196A8
		public static string Create<TState>(int length, TState state, SpanAction<char, TState> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (length > 0)
			{
				string text = string.FastAllocateString(length);
				action(new Span<char>(text.GetRawStringData(), length), state);
				return text;
			}
			if (length == 0)
			{
				return string.Empty;
			}
			throw new ArgumentOutOfRangeException("length");
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001B4F8 File Offset: 0x000196F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<char>(string value)
		{
			if (value == null)
			{
				return default(ReadOnlySpan<char>);
			}
			return new ReadOnlySpan<char>(value.GetRawStringData(), value.Length);
		}

		/// <summary>Returns a reference to this instance of <see cref="T:System.String" />.</summary>
		/// <returns>This instance of <see cref="T:System.String" />.</returns>
		// Token: 0x06000594 RID: 1428 RVA: 0x0000270D File Offset: 0x0000090D
		public object Clone()
		{
			return this;
		}

		/// <summary>Creates a new instance of <see cref="T:System.String" /> with the same value as a specified <see cref="T:System.String" />.</summary>
		/// <param name="str">The string to copy.</param>
		/// <returns>A new string with the same value as <paramref name="str" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is <see langword="null" />.</exception>
		// Token: 0x06000595 RID: 1429 RVA: 0x0001B524 File Offset: 0x00019724
		public unsafe static string Copy(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			string text = string.FastAllocateString(str.Length);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &str._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr2, ptr4, str.Length);
					ptr = null;
				}
				return text;
			}
		}

		/// <summary>Copies a specified number of characters from a specified position in this instance to a specified position in an array of Unicode characters.</summary>
		/// <param name="sourceIndex">The index of the first character in this instance to copy.</param>
		/// <param name="destination">An array of Unicode characters to which characters in this instance are copied.</param>
		/// <param name="destinationIndex">The index in <paramref name="destination" /> at which the copy operation begins.</param>
		/// <param name="count">The number of characters in this instance to copy to <paramref name="destination" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destination" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" />, <paramref name="destinationIndex" />, or <paramref name="count" /> is negative  
		/// -or-  
		/// <paramref name="sourceIndex" /> does not identify a position in the current instance.  
		/// -or-  
		/// <paramref name="destinationIndex" /> does not identify a valid index in the <paramref name="destination" /> array.  
		/// -or-  
		/// <paramref name="count" /> is greater than the length of the substring from <paramref name="sourceIndex" /> to the end of this instance  
		/// -or-  
		/// <paramref name="count" /> is greater than the length of the subarray from <paramref name="destinationIndex" /> to the end of the <paramref name="destination" /> array.</exception>
		// Token: 0x06000596 RID: 1430 RVA: 0x0001B570 File Offset: 0x00019770
		public unsafe void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > this.Length - sourceIndex)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Index and count must refer to a location within the string.");
			}
			if (destinationIndex > destination.Length - count || destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Index and count must refer to a location within the string.");
			}
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char[] array = destination)
				{
					char* ptr3;
					if (destination == null || array.Length == 0)
					{
						ptr3 = null;
					}
					else
					{
						ptr3 = &array[0];
					}
					string.wstrcpy(ptr3 + destinationIndex, ptr2 + sourceIndex, count);
					ptr = null;
				}
				return;
			}
		}

		/// <summary>Copies the characters in this instance to a Unicode character array.</summary>
		/// <returns>A Unicode character array whose elements are the individual characters of this instance. If this instance is an empty string, the returned array is empty and has a zero length.</returns>
		// Token: 0x06000597 RID: 1431 RVA: 0x0001B628 File Offset: 0x00019828
		public unsafe char[] ToCharArray()
		{
			if (this.Length == 0)
			{
				return Array.Empty<char>();
			}
			char[] array = new char[this.Length];
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &array[0])
				{
					string.wstrcpy(ptr3, ptr2, this.Length);
					ptr = null;
				}
				return array;
			}
		}

		/// <summary>Copies the characters in a specified substring in this instance to a Unicode character array.</summary>
		/// <param name="startIndex">The starting position of a substring in this instance.</param>
		/// <param name="length">The length of the substring in this instance.</param>
		/// <returns>A Unicode character array whose elements are the <paramref name="length" /> number of characters in this instance starting from character position <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> plus <paramref name="length" /> is greater than the length of this instance.</exception>
		// Token: 0x06000598 RID: 1432 RVA: 0x0001B674 File Offset: 0x00019874
		public unsafe char[] ToCharArray(int startIndex, int length)
		{
			if (startIndex < 0 || startIndex > this.Length || startIndex > this.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (length > 0)
			{
				char[] array = new char[length];
				fixed (char* ptr = &this._firstChar)
				{
					char* ptr2 = ptr;
					fixed (char* ptr3 = &array[0])
					{
						string.wstrcpy(ptr3, ptr2 + startIndex, length);
						ptr = null;
					}
					return array;
				}
			}
			if (length == 0)
			{
				return Array.Empty<char>();
			}
			throw new ArgumentOutOfRangeException("length", "Index was out of range. Must be non-negative and less than the size of the collection.");
		}

		/// <summary>Indicates whether the specified string is <see langword="null" /> or an empty string ("").</summary>
		/// <param name="value">The string to test.</param>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="value" /> parameter is <see langword="null" /> or an empty string (""); otherwise, <see langword="false" />.</returns>
		// Token: 0x06000599 RID: 1433 RVA: 0x0001B6F2 File Offset: 0x000198F2
		[NonVersionable]
		public static bool IsNullOrEmpty(string value)
		{
			return value == null || 0 >= value.Length;
		}

		/// <summary>Indicates whether a specified string is <see langword="null" />, empty, or consists only of white-space characters.</summary>
		/// <param name="value">The string to test.</param>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="value" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, or if <paramref name="value" /> consists exclusively of white-space characters.</returns>
		// Token: 0x0600059A RID: 1434 RVA: 0x0001B704 File Offset: 0x00019904
		public static bool IsNullOrWhiteSpace(string value)
		{
			if (value == null)
			{
				return true;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001B738 File Offset: 0x00019938
		internal ref char GetRawStringData()
		{
			return ref this._firstChar;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001B740 File Offset: 0x00019940
		internal unsafe static string CreateStringFromEncoding(byte* bytes, int byteLength, Encoding encoding)
		{
			int charCount = encoding.GetCharCount(bytes, byteLength, null);
			if (charCount == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(charCount);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				encoding.GetChars(bytes, byteLength, ptr2, charCount, null);
			}
			return text;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001B77F File Offset: 0x0001997F
		internal static string CreateFromChar(char c)
		{
			string text = string.FastAllocateString(1);
			text._firstChar = c;
			return text;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001B78E File Offset: 0x0001998E
		internal unsafe static void wstrcpy(char* dmem, char* smem, int charCount)
		{
			Buffer.Memmove((byte*)dmem, (byte*)smem, (uint)(charCount * 2));
		}

		/// <summary>Returns this instance of <see cref="T:System.String" />; no actual conversion is performed.</summary>
		/// <returns>The current string.</returns>
		// Token: 0x0600059F RID: 1439 RVA: 0x0000270D File Offset: 0x0000090D
		public override string ToString()
		{
			return this;
		}

		/// <summary>Returns this instance of <see cref="T:System.String" />; no actual conversion is performed.</summary>
		/// <param name="provider">(Reserved) An object that supplies culture-specific formatting information.</param>
		/// <returns>The current string.</returns>
		// Token: 0x060005A0 RID: 1440 RVA: 0x0000270D File Offset: 0x0000090D
		public string ToString(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>Retrieves an object that can iterate through the individual characters in this string.</summary>
		/// <returns>An enumerator object.</returns>
		// Token: 0x060005A1 RID: 1441 RVA: 0x0001B79A File Offset: 0x0001999A
		public CharEnumerator GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		/// <summary>Returns an enumerator that iterates through the current <see cref="T:System.String" /> object. </summary>
		/// <returns>A strongly-typed enumerator that can be used to iterate through the current <see cref="T:System.String" /> object. </returns>
		// Token: 0x060005A2 RID: 1442 RVA: 0x0001B79A File Offset: 0x0001999A
		IEnumerator<char> IEnumerable<char>.GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		/// <summary>Returns an enumerator that iterates through the current <see cref="T:System.String" /> object.</summary>
		/// <returns>An enumerator that can be used to iterate through the current string.</returns>
		// Token: 0x060005A3 RID: 1443 RVA: 0x0001B79A File Offset: 0x0001999A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001B7A4 File Offset: 0x000199A4
		internal unsafe static int wcslen(char* ptr)
		{
			char* ptr2 = ptr;
			int num = IntPtr.Size - 1;
			while ((ptr2 & (uint)num) != 0U)
			{
				if (*ptr2 != '\0')
				{
					ptr2++;
				}
				else
				{
					IL_006E:
					int num2 = (int)((long)(ptr2 - ptr));
					if (ptr + num2 != ptr2)
					{
						throw new ArgumentException("The string must be null-terminated.");
					}
					return num2;
				}
			}
			for (;;)
			{
				if (((*(long*)ptr2 + 9223231297218904063L) | 9223231297218904063L) == -1L)
				{
					ptr2 += 4;
				}
				else
				{
					if (*ptr2 == '\0')
					{
						goto IL_006E;
					}
					if (ptr2[1] == '\0')
					{
						goto IL_006A;
					}
					if (ptr2[2] == '\0')
					{
						goto IL_0066;
					}
					if (ptr2[3] == '\0')
					{
						break;
					}
					ptr2 += 4;
				}
			}
			ptr2++;
			IL_0066:
			ptr2++;
			IL_006A:
			ptr2++;
			goto IL_006E;
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for class <see cref="T:System.String" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.String" />.</returns>
		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B83C File Offset: 0x00019A3C
		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>
		///   <see langword="true" /> if the value of the current string is <see cref="F:System.Boolean.TrueString" />; <see langword="false" /> if the value of the current string is <see cref="F:System.Boolean.FalseString" />.</returns>
		/// <exception cref="T:System.FormatException">The value of the current string is not <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B840 File Offset: 0x00019A40
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The character at index 0 in the current <see cref="T:System.String" /> object.</returns>
		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B849 File Offset: 0x00019A49
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B852 File Offset: 0x00019A52
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />.</exception>
		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B85B File Offset: 0x00019A5B
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />.</exception>
		// Token: 0x060005AA RID: 1450 RVA: 0x0001B864 File Offset: 0x00019A64
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number greater than <see cref="F:System.UInt16.MaxValue" /> or less than <see cref="F:System.UInt16.MinValue" />.</exception>
		// Token: 0x060005AB RID: 1451 RVA: 0x0001B86D File Offset: 0x00019A6D
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		// Token: 0x060005AC RID: 1452 RVA: 0x0001B876 File Offset: 0x00019A76
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number greater <see cref="F:System.UInt32.MaxValue" /> or less than <see cref="F:System.UInt32.MinValue" /></exception>
		// Token: 0x060005AD RID: 1453 RVA: 0x0001B87F File Offset: 0x00019A7F
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		// Token: 0x060005AE RID: 1454 RVA: 0x0001B888 File Offset: 0x00019A88
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		// Token: 0x060005AF RID: 1455 RVA: 0x0001B891 File Offset: 0x00019A91
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		// Token: 0x060005B0 RID: 1456 RVA: 0x0001B89A File Offset: 0x00019A9A
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
		// Token: 0x060005B1 RID: 1457 RVA: 0x0001B8A3 File Offset: 0x00019AA3
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.FormatException">The value of the current <see cref="T:System.String" /> object cannot be parsed.</exception>
		/// <exception cref="T:System.OverflowException">The value of the current <see cref="T:System.String" /> object is a number less than <see cref="F:System.Decimal.MinValue" /> or than <see cref="F:System.Decimal.MaxValue" /> greater.</exception>
		// Token: 0x060005B2 RID: 1458 RVA: 0x0001B8AC File Offset: 0x00019AAC
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDateTime(System.IFormatProvider)" />.</summary>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B8B5 File Offset: 0x00019AB5
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(this, provider);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
		/// <param name="type">The type of the returned object.</param>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns>The converted value of the current <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The value of the current <see cref="T:System.String" /> object cannot be converted to the type specified by the <paramref name="type" /> parameter.</exception>
		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B8BE File Offset: 0x00019ABE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		/// <summary>Indicates whether this string is in Unicode normalization form C.</summary>
		/// <returns>
		///   <see langword="true" /> if this string is in normalization form C; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current instance contains invalid Unicode characters.</exception>
		// Token: 0x060005B5 RID: 1461 RVA: 0x0001B8C8 File Offset: 0x00019AC8
		public bool IsNormalized()
		{
			return this.IsNormalized(NormalizationForm.FormC);
		}

		/// <summary>Indicates whether this string is in the specified Unicode normalization form.</summary>
		/// <param name="normalizationForm">A Unicode normalization form.</param>
		/// <returns>
		///   <see langword="true" /> if this string is in the normalization form specified by the <paramref name="normalizationForm" /> parameter; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current instance contains invalid Unicode characters.</exception>
		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B8D1 File Offset: 0x00019AD1
		public bool IsNormalized(NormalizationForm normalizationForm)
		{
			return Normalization.IsNormalized(this, normalizationForm);
		}

		/// <summary>Returns a new string whose textual value is the same as this string, but whose binary representation is in Unicode normalization form C.</summary>
		/// <returns>A new, normalized string whose textual value is the same as this string, but whose binary representation is in normalization form C.</returns>
		/// <exception cref="T:System.ArgumentException">The current instance contains invalid Unicode characters.</exception>
		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B8DA File Offset: 0x00019ADA
		public string Normalize()
		{
			return this.Normalize(NormalizationForm.FormC);
		}

		/// <summary>Returns a new string whose textual value is the same as this string, but whose binary representation is in the specified Unicode normalization form.</summary>
		/// <param name="normalizationForm">A Unicode normalization form.</param>
		/// <returns>A new string whose textual value is the same as this string, but whose binary representation is in the normalization form specified by the <paramref name="normalizationForm" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">The current instance contains invalid Unicode characters.</exception>
		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B8E3 File Offset: 0x00019AE3
		public string Normalize(NormalizationForm normalizationForm)
		{
			return Normalization.Normalize(this, normalizationForm);
		}

		/// <summary>Gets the number of characters in the current <see cref="T:System.String" /> object.</summary>
		/// <returns>The number of characters in the current string.</returns>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001B8EC File Offset: 0x00019AEC
		public int Length
		{
			get
			{
				return this._stringLength;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B8F4 File Offset: 0x00019AF4
		internal unsafe int IndexOfUnchecked(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 + count - length + 1;
					while (ptr4 != ptr5)
					{
						if (*ptr4 == *ptr3)
						{
							for (int i = 1; i < length; i++)
							{
								if (ptr4[i] != ptr3[i])
								{
									goto IL_007B;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_007B:
						ptr4++;
					}
					ptr = null;
				}
				return -1;
			}
		}

		/// <summary>Concatenates the string representations of four specified objects and any objects specified in an optional variable length parameter list.</summary>
		/// <param name="arg0">The first object to concatenate.</param>
		/// <param name="arg1">The second object to concatenate.</param>
		/// <param name="arg2">The third object to concatenate.</param>
		/// <param name="arg3">The fourth object to concatenate.</param>
		/// <param name="…">An optional comma-delimited list of one or more additional objects to concatenate. </param>
		/// <returns>The concatenated string representation of each value in the parameter list.</returns>
		// Token: 0x060005BB RID: 1467 RVA: 0x0001B98F File Offset: 0x00019B8F
		[CLSCompliant(false)]
		public static string Concat(object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001B998 File Offset: 0x00019B98
		internal unsafe int IndexOfUncheckedIgnoreCase(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 + count - length + 1;
					char c = textInfo.ToUpper(*ptr3);
					while (ptr4 != ptr5)
					{
						if (textInfo.ToUpper(*ptr4) == c)
						{
							for (int i = 1; i < length; i++)
							{
								if (textInfo.ToUpper(ptr4[i]) != textInfo.ToUpper(ptr3[i]))
								{
									goto IL_00A4;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_00A4:
						ptr4++;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001BA60 File Offset: 0x00019C60
		internal unsafe int LastIndexOfUnchecked(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 - count + length - 1;
					char* ptr6 = ptr3 + length - 1;
					while (ptr4 != ptr5)
					{
						if (*ptr4 == *ptr6)
						{
							char* ptr7 = ptr4;
							while (ptr3 != ptr6)
							{
								ptr6--;
								ptr4--;
								if (*ptr4 != *ptr6)
								{
									ptr6 = ptr3 + length - 1;
									ptr4 = ptr7;
									goto IL_0092;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_0092:
						ptr4--;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001BB14 File Offset: 0x00019D14
		internal unsafe int LastIndexOfUncheckedIgnoreCase(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 - count + length - 1;
					char* ptr6 = ptr3 + length - 1;
					char c = textInfo.ToUpper(*ptr6);
					while (ptr4 != ptr5)
					{
						if (textInfo.ToUpper(*ptr4) == c)
						{
							char* ptr7 = ptr4;
							while (ptr3 != ptr6)
							{
								ptr6--;
								ptr4--;
								if (textInfo.ToUpper(*ptr4) != textInfo.ToUpper(*ptr6))
								{
									ptr6 = ptr3 + length - 1;
									ptr4 = ptr7;
									goto IL_00BB;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_00BB:
						ptr4--;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001BBF0 File Offset: 0x00019DF0
		internal bool StartsWithOrdinalUnchecked(string value)
		{
			return this.Length >= value.Length && this._firstChar == value._firstChar && (value.Length == 1 || SpanHelpers.SequenceEqual(Unsafe.As<char, byte>(this.GetRawStringData()), Unsafe.As<char, byte>(value.GetRawStringData()), (ulong)((long)value.Length * 2L)));
		}

		// Token: 0x060005C0 RID: 1472
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string FastAllocateString(int length);

		// Token: 0x060005C1 RID: 1473
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string InternalIsInterned(string str);

		// Token: 0x060005C2 RID: 1474
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string InternalIntern(string str);

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001BC4C File Offset: 0x00019E4C
		private unsafe static int FastCompareStringHelper(uint* strAChars, int countA, uint* strBChars, int countB)
		{
			char* ptr = (char*)strAChars;
			char* ptr2 = (char*)strBChars;
			char* ptr3 = ptr + Math.Min(countA, countB);
			while (ptr < ptr3)
			{
				if (*ptr != *ptr2)
				{
					return (int)(*ptr - *ptr2);
				}
				ptr++;
				ptr2++;
			}
			return countA - countB;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001BC88 File Offset: 0x00019E88
		private unsafe static void memset(byte* dest, int val, int len)
		{
			if (len < 8)
			{
				while (len != 0)
				{
					*dest = (byte)val;
					dest++;
					len--;
				}
				return;
			}
			if (val != 0)
			{
				val |= val << 8;
				val |= val << 16;
			}
			int num = dest & 3;
			if (num != 0)
			{
				num = 4 - num;
				len -= num;
				do
				{
					*dest = (byte)val;
					dest++;
					num--;
				}
				while (num != 0);
			}
			while (len >= 16)
			{
				*(int*)dest = val;
				*(int*)(dest + 4) = val;
				*(int*)(dest + (IntPtr)2 * 4) = val;
				*(int*)(dest + (IntPtr)3 * 4) = val;
				dest += 16;
				len -= 16;
			}
			while (len >= 4)
			{
				*(int*)dest = val;
				dest += 4;
				len -= 4;
			}
			while (len > 0)
			{
				*dest = (byte)val;
				dest++;
				len--;
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001BD32 File Offset: 0x00019F32
		private unsafe static void memcpy(byte* dest, byte* src, int size)
		{
			Buffer.Memcpy(dest, src, size);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001BD3C File Offset: 0x00019F3C
		internal unsafe static void bzero(byte* dest, int len)
		{
			string.memset(dest, 0, len);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001BD46 File Offset: 0x00019F46
		internal unsafe static void bzero_aligned_1(byte* dest, int len)
		{
			*dest = 0;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001BD4B File Offset: 0x00019F4B
		internal unsafe static void bzero_aligned_2(byte* dest, int len)
		{
			*(short*)dest = 0;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001BD50 File Offset: 0x00019F50
		internal unsafe static void bzero_aligned_4(byte* dest, int len)
		{
			*(int*)dest = 0;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001BD55 File Offset: 0x00019F55
		internal unsafe static void bzero_aligned_8(byte* dest, int len)
		{
			*(long*)dest = 0L;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001BD5B File Offset: 0x00019F5B
		internal unsafe static void memcpy_aligned_1(byte* dest, byte* src, int size)
		{
			*dest = *src;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001BD61 File Offset: 0x00019F61
		internal unsafe static void memcpy_aligned_2(byte* dest, byte* src, int size)
		{
			*(short*)dest = *(short*)src;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001BD67 File Offset: 0x00019F67
		internal unsafe static void memcpy_aligned_4(byte* dest, byte* src, int size)
		{
			*(int*)dest = *(int*)src;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001BD6D File Offset: 0x00019F6D
		internal unsafe static void memcpy_aligned_8(byte* dest, byte* src, int size)
		{
			*(long*)dest = *(long*)src;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001BD73 File Offset: 0x00019F73
		private unsafe string CreateString(sbyte* value)
		{
			return string.Ctor(value);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001BD7B File Offset: 0x00019F7B
		private unsafe string CreateString(sbyte* value, int startIndex, int length)
		{
			return string.Ctor(value, startIndex, length);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001BD85 File Offset: 0x00019F85
		private unsafe string CreateString(char* value)
		{
			return string.Ctor(value);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001BD8D File Offset: 0x00019F8D
		private unsafe string CreateString(char* value, int startIndex, int length)
		{
			return string.Ctor(value, startIndex, length);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001BD97 File Offset: 0x00019F97
		private string CreateString(char[] val, int startIndex, int length)
		{
			return string.Ctor(val, startIndex, length);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001BDA1 File Offset: 0x00019FA1
		private string CreateString(char[] val)
		{
			return string.Ctor(val);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001BDA9 File Offset: 0x00019FA9
		private string CreateString(char c, int count)
		{
			return string.Ctor(c, count);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001BDB2 File Offset: 0x00019FB2
		private unsafe string CreateString(sbyte* value, int startIndex, int length, Encoding enc)
		{
			return string.Ctor(value, startIndex, length, enc);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001BDBE File Offset: 0x00019FBE
		private string CreateString(ReadOnlySpan<char> value)
		{
			return string.Ctor(value);
		}

		/// <summary>Gets the <see cref="T:System.Char" /> object at a specified position in the current <see cref="T:System.String" /> object.</summary>
		/// <param name="index">A position in the current string.</param>
		/// <returns>The object at position <paramref name="index" />.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is greater than or equal to the length of this object or less than zero.</exception>
		// Token: 0x1700007F RID: 127
		[IndexerName("Chars")]
		public unsafe char this[int index]
		{
			[Intrinsic]
			get
			{
				if ((ulong)index >= (ulong)((long)this._stringLength))
				{
					ThrowHelper.ThrowIndexOutOfRangeException();
				}
				return *Unsafe.Add<char>(ref this._firstChar, index);
			}
		}

		/// <summary>Retrieves the system's reference to the specified <see cref="T:System.String" />.</summary>
		/// <param name="str">A string to search for in the intern pool.</param>
		/// <returns>The system's reference to <paramref name="str" />, if it is interned; otherwise, a new reference to a string with the value of <paramref name="str" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is <see langword="null" />.</exception>
		// Token: 0x060005D9 RID: 1497 RVA: 0x0001BDE5 File Offset: 0x00019FE5
		public static string Intern(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return string.InternalIntern(str);
		}

		/// <summary>Retrieves a reference to a specified <see cref="T:System.String" />.</summary>
		/// <param name="str">The string to search for in the intern pool.</param>
		/// <returns>A reference to <paramref name="str" /> if it is in the common language runtime intern pool; otherwise, <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is <see langword="null" />.</exception>
		// Token: 0x060005DA RID: 1498 RVA: 0x0001BDFB File Offset: 0x00019FFB
		public static string IsInterned(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return string.InternalIsInterned(str);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001BE14 File Offset: 0x0001A014
		private unsafe int LegacyStringGetHashCode()
		{
			int num = 5381;
			int num2 = num;
			fixed (string text = this)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char* ptr2 = ptr;
				int num3;
				while ((num3 = (int)(*ptr2)) != 0)
				{
					num = ((num << 5) + num) ^ num3;
					num3 = (int)ptr2[1];
					if (num3 == 0)
					{
						break;
					}
					num2 = ((num2 << 5) + num2) ^ num3;
					ptr2 += 2;
				}
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x04000FEA RID: 4074
		private const int StackallocIntBufferSizeLimit = 128;

		// Token: 0x04000FEB RID: 4075
		private const int PROBABILISTICMAP_BLOCK_INDEX_MASK = 7;

		// Token: 0x04000FEC RID: 4076
		private const int PROBABILISTICMAP_BLOCK_INDEX_SHIFT = 3;

		// Token: 0x04000FED RID: 4077
		private const int PROBABILISTICMAP_SIZE = 8;

		// Token: 0x04000FEE RID: 4078
		[NonSerialized]
		private int _stringLength;

		// Token: 0x04000FEF RID: 4079
		[NonSerialized]
		private char _firstChar;

		/// <summary>Represents the empty string. This field is read-only.</summary>
		// Token: 0x04000FF0 RID: 4080
		public static readonly string Empty;

		// Token: 0x020000CE RID: 206
		private enum TrimType
		{
			// Token: 0x04000FF2 RID: 4082
			Head,
			// Token: 0x04000FF3 RID: 4083
			Tail,
			// Token: 0x04000FF4 RID: 4084
			Both
		}

		// Token: 0x020000CF RID: 207
		[StructLayout(LayoutKind.Explicit, Size = 32)]
		private struct ProbabilisticMap
		{
		}
	}
}
