using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System
{
	/// <summary>Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.</summary>
	// Token: 0x02000154 RID: 340
	public static class Math
	{
		/// <summary>Returns the absolute value of a 16-bit signed integer.</summary>
		/// <param name="value">A number that is greater than <see cref="F:System.Int16.MinValue" />, but less than or equal to <see cref="F:System.Int16.MaxValue" />.</param>
		/// <returns>A 16-bit signed integer, x, such that 0 ≤ x ≤<see cref="F:System.Int16.MaxValue" />.</returns>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> equals <see cref="F:System.Int16.MinValue" />.</exception>
		// Token: 0x06000CC8 RID: 3272 RVA: 0x00032FEC File Offset: 0x000311EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Abs(short value)
		{
			if (value < 0)
			{
				value = -value;
				if (value < 0)
				{
					Math.ThrowAbsOverflow();
				}
			}
			return value;
		}

		/// <summary>Returns the absolute value of a 32-bit signed integer.</summary>
		/// <param name="value">A number that is greater than <see cref="F:System.Int32.MinValue" />, but less than or equal to <see cref="F:System.Int32.MaxValue" />.</param>
		/// <returns>A 32-bit signed integer, x, such that 0 ≤ x ≤<see cref="F:System.Int32.MaxValue" />.</returns>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> equals <see cref="F:System.Int32.MinValue" />.</exception>
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00033001 File Offset: 0x00031201
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Abs(int value)
		{
			if (value < 0)
			{
				value = -value;
				if (value < 0)
				{
					Math.ThrowAbsOverflow();
				}
			}
			return value;
		}

		/// <summary>Returns the absolute value of a 64-bit signed integer.</summary>
		/// <param name="value">A number that is greater than <see cref="F:System.Int64.MinValue" />, but less than or equal to <see cref="F:System.Int64.MaxValue" />.</param>
		/// <returns>A 64-bit signed integer, x, such that 0 ≤ x ≤<see cref="F:System.Int64.MaxValue" />.</returns>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> equals <see cref="F:System.Int64.MinValue" />.</exception>
		// Token: 0x06000CCA RID: 3274 RVA: 0x00033015 File Offset: 0x00031215
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long Abs(long value)
		{
			if (value < 0L)
			{
				value = -value;
				if (value < 0L)
				{
					Math.ThrowAbsOverflow();
				}
			}
			return value;
		}

		/// <summary>Returns the absolute value of an 8-bit signed integer.</summary>
		/// <param name="value">A number that is greater than <see cref="F:System.SByte.MinValue" />, but less than or equal to <see cref="F:System.SByte.MaxValue" />.</param>
		/// <returns>An 8-bit signed integer, x, such that 0 ≤ x ≤<see cref="F:System.SByte.MaxValue" />.</returns>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> equals <see cref="F:System.SByte.MinValue" />.</exception>
		// Token: 0x06000CCB RID: 3275 RVA: 0x0003302B File Offset: 0x0003122B
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Abs(sbyte value)
		{
			if (value < 0)
			{
				value = -value;
				if (value < 0)
				{
					Math.ThrowAbsOverflow();
				}
			}
			return value;
		}

		/// <summary>Returns the absolute value of a <see cref="T:System.Decimal" /> number.</summary>
		/// <param name="value">A number that is greater than or equal to <see cref="F:System.Decimal.MinValue" />, but less than or equal to <see cref="F:System.Decimal.MaxValue" />.</param>
		/// <returns>A decimal number, x, such that 0 ≤ x ≤<see cref="F:System.Decimal.MaxValue" />.</returns>
		// Token: 0x06000CCC RID: 3276 RVA: 0x00033040 File Offset: 0x00031240
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Abs(decimal value)
		{
			return decimal.Abs(ref value);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00033049 File Offset: 0x00031249
		[StackTraceHidden]
		private static void ThrowAbsOverflow()
		{
			throw new OverflowException("Negating the minimum value of a twos complement number is invalid.");
		}

		/// <summary>Produces the full product of two 32-bit numbers.</summary>
		/// <param name="a">The first number to multiply.</param>
		/// <param name="b">The second number to multiply.</param>
		/// <returns>The number containing the product of the specified numbers.</returns>
		// Token: 0x06000CCE RID: 3278 RVA: 0x00033055 File Offset: 0x00031255
		public static long BigMul(int a, int b)
		{
			return (long)a * (long)b;
		}

		/// <summary>Calculates the quotient of two 32-bit signed integers and also returns the remainder in an output parameter.</summary>
		/// <param name="a">The dividend.</param>
		/// <param name="b">The divisor.</param>
		/// <param name="result">The remainder.</param>
		/// <returns>The quotient of the specified numbers.</returns>
		/// <exception cref="T:System.DivideByZeroException">
		///   <paramref name="b" /> is zero.</exception>
		// Token: 0x06000CCF RID: 3279 RVA: 0x0003305C File Offset: 0x0003125C
		public static int DivRem(int a, int b, out int result)
		{
			int num = a / b;
			result = a - num * b;
			return num;
		}

		/// <summary>Calculates the quotient of two 64-bit signed integers and also returns the remainder in an output parameter.</summary>
		/// <param name="a">The dividend.</param>
		/// <param name="b">The divisor.</param>
		/// <param name="result">The remainder.</param>
		/// <returns>The quotient of the specified numbers.</returns>
		/// <exception cref="T:System.DivideByZeroException">
		///   <paramref name="b" /> is zero.</exception>
		// Token: 0x06000CD0 RID: 3280 RVA: 0x00033078 File Offset: 0x00031278
		public static long DivRem(long a, long b, out long result)
		{
			long num = a / b;
			result = a - num * b;
			return num;
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00033094 File Offset: 0x00031294
		internal static uint DivRem(uint a, uint b, out uint result)
		{
			uint num = a / b;
			result = a - num * b;
			return num;
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000330B0 File Offset: 0x000312B0
		internal static ulong DivRem(ulong a, ulong b, out ulong result)
		{
			ulong num = a / b;
			result = a - num * b;
			return num;
		}

		/// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
		/// <param name="d">A decimal number.</param>
		/// <returns>The smallest integral value that is greater than or equal to <paramref name="d" />. Note that this method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
		// Token: 0x06000CD3 RID: 3283 RVA: 0x000330C9 File Offset: 0x000312C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Ceiling(decimal d)
		{
			return decimal.Ceiling(d);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000330D1 File Offset: 0x000312D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Clamp(byte value, byte min, byte max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<byte>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x000330EB File Offset: 0x000312EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Clamp(decimal value, decimal min, decimal max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<decimal>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00033114 File Offset: 0x00031314
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Clamp(double value, double min, double max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<double>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003312E File Offset: 0x0003132E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Clamp(short value, short min, short max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<short>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00033148 File Offset: 0x00031348
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clamp(int value, int min, int max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<int>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00033162 File Offset: 0x00031362
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long Clamp(long value, long min, long max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<long>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0003317C File Offset: 0x0003137C
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<sbyte>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00033196 File Offset: 0x00031396
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float value, float min, float max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<float>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x000331B0 File Offset: 0x000313B0
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort Clamp(ushort value, ushort min, ushort max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<ushort>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x000331CA File Offset: 0x000313CA
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Clamp(uint value, uint min, uint max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<uint>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000331E4 File Offset: 0x000313E4
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Clamp(ulong value, ulong min, ulong max)
		{
			if (min > max)
			{
				Math.ThrowMinMaxException<ulong>(min, max);
			}
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		/// <summary>Returns the largest integral value less than or equal to the specified decimal number.</summary>
		/// <param name="d">A decimal number.</param>
		/// <returns>The largest integral value less than or equal to <paramref name="d" />.  Note that the method returns an integral value of type <see cref="T:System.Decimal" />.</returns>
		// Token: 0x06000CDF RID: 3295 RVA: 0x000331FE File Offset: 0x000313FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Floor(decimal d)
		{
			return decimal.Floor(d);
		}

		/// <summary>Returns the remainder resulting from the division of a specified number by another specified number.</summary>
		/// <param name="x">A dividend.</param>
		/// <param name="y">A divisor.</param>
		/// <returns>A number equal to <paramref name="x" /> - (<paramref name="y" /> Q), where Q is the quotient of <paramref name="x" /> / <paramref name="y" /> rounded to the nearest integer (if <paramref name="x" /> / <paramref name="y" /> falls halfway between two integers, the even integer is returned).  
		///  If <paramref name="x" /> - (<paramref name="y" /> Q) is zero, the value +0 is returned if <paramref name="x" /> is positive, or -0 if <paramref name="x" /> is negative.  
		///  If <paramref name="y" /> = 0, <see cref="F:System.Double.NaN" /> is returned.</returns>
		// Token: 0x06000CE0 RID: 3296 RVA: 0x00033208 File Offset: 0x00031408
		public static double IEEERemainder(double x, double y)
		{
			if (double.IsNaN(x))
			{
				return x;
			}
			if (double.IsNaN(y))
			{
				return y;
			}
			double num = x % y;
			if (double.IsNaN(num))
			{
				return double.NaN;
			}
			if (num == 0.0 && double.IsNegative(x))
			{
				return -0.0;
			}
			double num2 = num - Math.Abs(y) * (double)Math.Sign(x);
			if (Math.Abs(num2) == Math.Abs(num))
			{
				double num3 = x / y;
				if (Math.Abs(Math.Round(num3)) > Math.Abs(num3))
				{
					return num2;
				}
				return num;
			}
			else
			{
				if (Math.Abs(num2) < Math.Abs(num))
				{
					return num2;
				}
				return num;
			}
		}

		/// <summary>Returns the logarithm of a specified number in a specified base.</summary>
		/// <param name="a">The number whose logarithm is to be found.</param>
		/// <param name="newBase">The base of the logarithm.</param>
		/// <returns>One of the values in the following table. (+Infinity denotes <see cref="F:System.Double.PositiveInfinity" />, -Infinity denotes <see cref="F:System.Double.NegativeInfinity" />, and NaN denotes <see cref="F:System.Double.NaN" />.)  
		///  <paramref name="a" /><paramref name="newBase" /> Return value  
		///
		///  <paramref name="a" />&gt; 0  
		///
		///   (0 &lt;<paramref name="newBase" />&lt; 1) -or-(<paramref name="newBase" />&gt; 1)  
		///
		///   lognewBase(a)  
		///
		///  <paramref name="a" />&lt; 0  
		///
		///   (any value)  
		///
		///   NaN  
		///
		///   (any value)  
		///
		///  <paramref name="newBase" />&lt; 0  
		///
		///   NaN  
		///
		///  <paramref name="a" /> != 1  
		///
		///  <paramref name="newBase" /> = 0  
		///
		///   NaN  
		///
		///  <paramref name="a" /> != 1  
		///
		///  <paramref name="newBase" /> = +Infinity  
		///
		///   NaN  
		///
		///  <paramref name="a" /> = NaN  
		///
		///   (any value)  
		///
		///   NaN  
		///
		///   (any value)  
		///
		///  <paramref name="newBase" /> = NaN  
		///
		///   NaN  
		///
		///   (any value)  
		///
		///  <paramref name="newBase" /> = 1  
		///
		///   NaN  
		///
		///  <paramref name="a" /> = 0  
		///
		///   0 &lt;<paramref name="newBase" />&lt; 1  
		///
		///   +Infinity  
		///
		///  <paramref name="a" /> = 0  
		///
		///  <paramref name="newBase" />&gt; 1  
		///
		///   -Infinity  
		///
		///  <paramref name="a" /> =  +Infinity  
		///
		///   0 &lt;<paramref name="newBase" />&lt; 1  
		///
		///   -Infinity  
		///
		///  <paramref name="a" /> =  +Infinity  
		///
		///  <paramref name="newBase" />&gt; 1  
		///
		///   +Infinity  
		///
		///  <paramref name="a" /> = 1  
		///
		///  <paramref name="newBase" /> = 0  
		///
		///   0  
		///
		///  <paramref name="a" /> = 1  
		///
		///  <paramref name="newBase" /> = +Infinity  
		///
		///   0</returns>
		// Token: 0x06000CE1 RID: 3297 RVA: 0x000332A8 File Offset: 0x000314A8
		public static double Log(double a, double newBase)
		{
			if (double.IsNaN(a))
			{
				return a;
			}
			if (double.IsNaN(newBase))
			{
				return newBase;
			}
			if (newBase == 1.0)
			{
				return double.NaN;
			}
			if (a != 1.0 && (newBase == 0.0 || double.IsPositiveInfinity(newBase)))
			{
				return double.NaN;
			}
			return Math.Log(a) / Math.Log(newBase);
		}

		/// <summary>Returns the larger of two 8-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CE2 RID: 3298 RVA: 0x00033316 File Offset: 0x00031516
		[NonVersionable]
		public static byte Max(byte val1, byte val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two decimal numbers.</summary>
		/// <param name="val1">The first of two decimal numbers to compare.</param>
		/// <param name="val2">The second of two decimal numbers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CE3 RID: 3299 RVA: 0x0003331F File Offset: 0x0003151F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static decimal Max(decimal val1, decimal val2)
		{
			return *decimal.Max(ref val1, ref val2);
		}

		/// <summary>Returns the larger of two double-precision floating-point numbers.</summary>
		/// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
		/// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger. If <paramref name="val1" />, <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
		// Token: 0x06000CE4 RID: 3300 RVA: 0x0003332F File Offset: 0x0003152F
		public static double Max(double val1, double val2)
		{
			if (val1 > val2)
			{
				return val1;
			}
			if (double.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		/// <summary>Returns the larger of two 16-bit signed integers.</summary>
		/// <param name="val1">The first of two 16-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 16-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CE5 RID: 3301 RVA: 0x00033316 File Offset: 0x00031516
		[NonVersionable]
		public static short Max(short val1, short val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two 32-bit signed integers.</summary>
		/// <param name="val1">The first of two 32-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 32-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CE6 RID: 3302 RVA: 0x00033316 File Offset: 0x00031516
		[NonVersionable]
		public static int Max(int val1, int val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two 64-bit signed integers.</summary>
		/// <param name="val1">The first of two 64-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 64-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CE7 RID: 3303 RVA: 0x00033316 File Offset: 0x00031516
		[NonVersionable]
		public static long Max(long val1, long val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two 8-bit signed integers.</summary>
		/// <param name="val1">The first of two 8-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 8-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CE8 RID: 3304 RVA: 0x00033316 File Offset: 0x00031516
		[CLSCompliant(false)]
		[NonVersionable]
		public static sbyte Max(sbyte val1, sbyte val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two single-precision floating-point numbers.</summary>
		/// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
		/// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger. If <paramref name="val1" />, or <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
		// Token: 0x06000CE9 RID: 3305 RVA: 0x00033342 File Offset: 0x00031542
		public static float Max(float val1, float val2)
		{
			if (val1 > val2)
			{
				return val1;
			}
			if (float.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		/// <summary>Returns the larger of two 16-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CEA RID: 3306 RVA: 0x00033316 File Offset: 0x00031516
		[NonVersionable]
		[CLSCompliant(false)]
		public static ushort Max(ushort val1, ushort val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two 32-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CEB RID: 3307 RVA: 0x00033355 File Offset: 0x00031555
		[NonVersionable]
		[CLSCompliant(false)]
		public static uint Max(uint val1, uint val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the larger of two 64-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
		// Token: 0x06000CEC RID: 3308 RVA: 0x00033355 File Offset: 0x00031555
		[NonVersionable]
		[CLSCompliant(false)]
		public static ulong Max(ulong val1, ulong val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two 8-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CED RID: 3309 RVA: 0x0003335E File Offset: 0x0003155E
		[NonVersionable]
		public static byte Min(byte val1, byte val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two decimal numbers.</summary>
		/// <param name="val1">The first of two decimal numbers to compare.</param>
		/// <param name="val2">The second of two decimal numbers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CEE RID: 3310 RVA: 0x00033367 File Offset: 0x00031567
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static decimal Min(decimal val1, decimal val2)
		{
			return *decimal.Min(ref val1, ref val2);
		}

		/// <summary>Returns the smaller of two double-precision floating-point numbers.</summary>
		/// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
		/// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller. If <paramref name="val1" />, <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
		// Token: 0x06000CEF RID: 3311 RVA: 0x00033377 File Offset: 0x00031577
		public static double Min(double val1, double val2)
		{
			if (val1 < val2)
			{
				return val1;
			}
			if (double.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		/// <summary>Returns the smaller of two 16-bit signed integers.</summary>
		/// <param name="val1">The first of two 16-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 16-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003335E File Offset: 0x0003155E
		[NonVersionable]
		public static short Min(short val1, short val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two 32-bit signed integers.</summary>
		/// <param name="val1">The first of two 32-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 32-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF1 RID: 3313 RVA: 0x0003335E File Offset: 0x0003155E
		[NonVersionable]
		public static int Min(int val1, int val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two 64-bit signed integers.</summary>
		/// <param name="val1">The first of two 64-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 64-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF2 RID: 3314 RVA: 0x0003335E File Offset: 0x0003155E
		[NonVersionable]
		public static long Min(long val1, long val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two 8-bit signed integers.</summary>
		/// <param name="val1">The first of two 8-bit signed integers to compare.</param>
		/// <param name="val2">The second of two 8-bit signed integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003335E File Offset: 0x0003155E
		[NonVersionable]
		[CLSCompliant(false)]
		public static sbyte Min(sbyte val1, sbyte val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two single-precision floating-point numbers.</summary>
		/// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
		/// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller. If <paramref name="val1" />, <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
		// Token: 0x06000CF4 RID: 3316 RVA: 0x0003338A File Offset: 0x0003158A
		public static float Min(float val1, float val2)
		{
			if (val1 < val2)
			{
				return val1;
			}
			if (float.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		/// <summary>Returns the smaller of two 16-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF5 RID: 3317 RVA: 0x0003335E File Offset: 0x0003155E
		[NonVersionable]
		[CLSCompliant(false)]
		public static ushort Min(ushort val1, ushort val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two 32-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF6 RID: 3318 RVA: 0x0003339D File Offset: 0x0003159D
		[CLSCompliant(false)]
		[NonVersionable]
		public static uint Min(uint val1, uint val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Returns the smaller of two 64-bit unsigned integers.</summary>
		/// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
		/// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
		/// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
		// Token: 0x06000CF7 RID: 3319 RVA: 0x0003339D File Offset: 0x0003159D
		[NonVersionable]
		[CLSCompliant(false)]
		public static ulong Min(ulong val1, ulong val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		/// <summary>Rounds a decimal value to the nearest integral value, and rounds midpoint values to the nearest even number.</summary>
		/// <param name="d">A decimal number to be rounded.</param>
		/// <returns>The integer nearest the <paramref name="d" /> parameter. If the fractional component of <paramref name="d" /> is halfway between two integers, one of which is even and the other odd, the even number is returned. Note that this method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
		// Token: 0x06000CF8 RID: 3320 RVA: 0x000333A6 File Offset: 0x000315A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Round(decimal d)
		{
			return decimal.Round(d, 0);
		}

		/// <summary>Rounds a decimal value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.</summary>
		/// <param name="d">A decimal number to be rounded.</param>
		/// <param name="decimals">The number of decimal places in the return value.</param>
		/// <returns>The number nearest to <paramref name="d" /> that contains a number of fractional digits equal to <paramref name="decimals" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="decimals" /> is less than 0 or greater than 28.</exception>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
		// Token: 0x06000CF9 RID: 3321 RVA: 0x000333AF File Offset: 0x000315AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Round(decimal d, int decimals)
		{
			return decimal.Round(d, decimals);
		}

		/// <summary>Rounds a decimal value to the nearest integer, and uses the specified rounding convention for midpoint values.</summary>
		/// <param name="d">A decimal number to be rounded.</param>
		/// <param name="mode">Specification for how to round <paramref name="d" /> if it is midway between two other numbers.</param>
		/// <returns>The integer nearest <paramref name="d" />. If <paramref name="d" /> is halfway between two numbers, one of which is even and the other odd, then <paramref name="mode" /> determines which of the two is returned. Note that this method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
		// Token: 0x06000CFA RID: 3322 RVA: 0x000333B8 File Offset: 0x000315B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return decimal.Round(d, 0, mode);
		}

		/// <summary>Rounds a decimal value to a specified number of fractional digits, and uses the specified rounding convention for midpoint values.</summary>
		/// <param name="d">A decimal number to be rounded.</param>
		/// <param name="decimals">The number of decimal places in the return value.</param>
		/// <param name="mode">Specification for how to round <paramref name="d" /> if it is midway between two other numbers.</param>
		/// <returns>The number nearest to <paramref name="d" /> that contains a number of fractional digits equal to <paramref name="decimals" />. If <paramref name="d" /> has fewer fractional digits than <paramref name="decimals" />, <paramref name="d" /> is returned unchanged.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="decimals" /> is less than 0 or greater than 28.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
		// Token: 0x06000CFB RID: 3323 RVA: 0x000333C2 File Offset: 0x000315C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Round(decimal d, int decimals, MidpointRounding mode)
		{
			return decimal.Round(d, decimals, mode);
		}

		/// <summary>Rounds a double-precision floating-point value to the nearest integral value, and rounds midpoint values to the nearest even number.</summary>
		/// <param name="a">A double-precision floating-point number to be rounded.</param>
		/// <returns>The integer nearest <paramref name="a" />. If the fractional component of <paramref name="a" /> is halfway between two integers, one of which is even and the other odd, then the even number is returned. Note that this method returns a <see cref="T:System.Double" /> instead of an integral type.</returns>
		// Token: 0x06000CFC RID: 3324
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Round(double a);

		/// <summary>Rounds a double-precision floating-point value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.</summary>
		/// <param name="value">A double-precision floating-point number to be rounded.</param>
		/// <param name="digits">The number of fractional digits in the return value.</param>
		/// <returns>The number nearest to <paramref name="value" /> that contains a number of fractional digits equal to <paramref name="digits" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="digits" /> is less than 0 or greater than 15.</exception>
		// Token: 0x06000CFD RID: 3325 RVA: 0x000333CC File Offset: 0x000315CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Round(double value, int digits)
		{
			return Math.Round(value, digits, MidpointRounding.ToEven);
		}

		/// <summary>Rounds a double-precision floating-point value to the nearest integer, and uses the specified rounding convention for midpoint values.</summary>
		/// <param name="value">A double-precision floating-point number to be rounded.</param>
		/// <param name="mode">Specification for how to round <paramref name="value" /> if it is midway between two other numbers.</param>
		/// <returns>The integer nearest <paramref name="value" />. If <paramref name="value" /> is halfway between two integers, one of which is even and the other odd, then <paramref name="mode" /> determines which of the two is returned. Note that this method returns a <see cref="T:System.Double" /> instead of an integral type.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
		// Token: 0x06000CFE RID: 3326 RVA: 0x000333D6 File Offset: 0x000315D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Round(double value, MidpointRounding mode)
		{
			return Math.Round(value, 0, mode);
		}

		/// <summary>Rounds a double-precision floating-point value to a specified number of fractional digits, and uses the specified rounding convention for midpoint values.</summary>
		/// <param name="value">A double-precision floating-point number to be rounded.</param>
		/// <param name="digits">The number of fractional digits in the return value.</param>
		/// <param name="mode">Specification for how to round <paramref name="value" /> if it is midway between two other numbers.</param>
		/// <returns>The number nearest to <paramref name="value" /> that has a number of fractional digits equal to <paramref name="digits" />. If <paramref name="value" /> has fewer fractional digits than <paramref name="digits" />, <paramref name="value" /> is returned unchanged.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="digits" /> is less than 0 or greater than 15.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
		// Token: 0x06000CFF RID: 3327 RVA: 0x000333E0 File Offset: 0x000315E0
		public unsafe static double Round(double value, int digits, MidpointRounding mode)
		{
			if (digits < 0 || digits > 15)
			{
				throw new ArgumentOutOfRangeException("digits", "Rounding digits must be between 0 and 15, inclusive.");
			}
			if (mode < MidpointRounding.ToEven || mode > MidpointRounding.AwayFromZero)
			{
				throw new ArgumentException(SR.Format("The value '{0}' is not valid for this usage of the type {1}.", mode, "MidpointRounding"), "mode");
			}
			if (Math.Abs(value) < Math.doubleRoundLimit)
			{
				double num = Math.roundPower10Double[digits];
				value *= num;
				if (mode == MidpointRounding.AwayFromZero)
				{
					double num2 = Math.ModF(value, &value);
					if (Math.Abs(num2) >= 0.5)
					{
						value += (double)Math.Sign(num2);
					}
				}
				else
				{
					value = Math.Round(value);
				}
				value /= num;
			}
			return value;
		}

		/// <summary>Returns an integer that indicates the sign of a decimal number.</summary>
		/// <param name="value">A signed decimal number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		// Token: 0x06000D00 RID: 3328 RVA: 0x00033482 File Offset: 0x00031682
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(decimal value)
		{
			return decimal.Sign(ref value);
		}

		/// <summary>Returns an integer that indicates the sign of a double-precision floating-point number.</summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		/// <exception cref="T:System.ArithmeticException">
		///   <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
		// Token: 0x06000D01 RID: 3329 RVA: 0x0003348B File Offset: 0x0003168B
		public static int Sign(double value)
		{
			if (value < 0.0)
			{
				return -1;
			}
			if (value > 0.0)
			{
				return 1;
			}
			if (value == 0.0)
			{
				return 0;
			}
			throw new ArithmeticException("Function does not accept floating point Not-a-Number values.");
		}

		/// <summary>Returns an integer that indicates the sign of a 16-bit signed integer.</summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		// Token: 0x06000D02 RID: 3330 RVA: 0x000334C1 File Offset: 0x000316C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(short value)
		{
			return Math.Sign((int)value);
		}

		/// <summary>Returns an integer that indicates the sign of a 32-bit signed integer.</summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		// Token: 0x06000D03 RID: 3331 RVA: 0x000334C9 File Offset: 0x000316C9
		public static int Sign(int value)
		{
			return (value >> 31) | (int)((uint)(-(uint)value) >> 31);
		}

		/// <summary>Returns an integer that indicates the sign of a 64-bit signed integer.</summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		// Token: 0x06000D04 RID: 3332 RVA: 0x000334D5 File Offset: 0x000316D5
		public static int Sign(long value)
		{
			return (int)((value >> 63) | (long)((ulong)(-(ulong)value) >> 63));
		}

		/// <summary>Returns an integer that indicates the sign of an 8-bit signed integer.</summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		// Token: 0x06000D05 RID: 3333 RVA: 0x000334C1 File Offset: 0x000316C1
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(sbyte value)
		{
			return Math.Sign((int)value);
		}

		/// <summary>Returns an integer that indicates the sign of a single-precision floating-point number.</summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.  
		///   Return value  
		///
		///   Meaning  
		///
		///   -1  
		///
		///  <paramref name="value" /> is less than zero.  
		///
		///   0  
		///
		///  <paramref name="value" /> is equal to zero.  
		///
		///   1  
		///
		///  <paramref name="value" /> is greater than zero.</returns>
		/// <exception cref="T:System.ArithmeticException">
		///   <paramref name="value" /> is equal to <see cref="F:System.Single.NaN" />.</exception>
		// Token: 0x06000D06 RID: 3334 RVA: 0x000334E2 File Offset: 0x000316E2
		public static int Sign(float value)
		{
			if (value < 0f)
			{
				return -1;
			}
			if (value > 0f)
			{
				return 1;
			}
			if (value == 0f)
			{
				return 0;
			}
			throw new ArithmeticException("Function does not accept floating point Not-a-Number values.");
		}

		/// <summary>Calculates the integral part of a specified decimal number.</summary>
		/// <param name="d">A number to truncate.</param>
		/// <returns>The integral part of <paramref name="d" />; that is, the number that remains after any fractional digits have been discarded.</returns>
		// Token: 0x06000D07 RID: 3335 RVA: 0x0003350C File Offset: 0x0003170C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Truncate(decimal d)
		{
			return decimal.Truncate(d);
		}

		/// <summary>Calculates the integral part of a specified double-precision floating-point number.</summary>
		/// <param name="d">A number to truncate.</param>
		/// <returns>The integral part of <paramref name="d" />; that is, the number that remains after any fractional digits have been discarded, or one of the values listed in the following table.  
		///  <paramref name="d" /> Return value  
		///
		///  <see cref="F:System.Double.NaN" /><see cref="F:System.Double.NaN" /><see cref="F:System.Double.NegativeInfinity" /><see cref="F:System.Double.NegativeInfinity" /><see cref="F:System.Double.PositiveInfinity" /><see cref="F:System.Double.PositiveInfinity" /></returns>
		// Token: 0x06000D08 RID: 3336 RVA: 0x00033514 File Offset: 0x00031714
		public unsafe static double Truncate(double d)
		{
			Math.ModF(d, &d);
			return d;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00033524 File Offset: 0x00031724
		private static double copysign(double x, double y)
		{
			long num = BitConverter.DoubleToInt64Bits(x);
			long num2 = BitConverter.DoubleToInt64Bits(y);
			if ((num ^ num2) >> 63 != 0L)
			{
				return BitConverter.Int64BitsToDouble(num ^ long.MinValue);
			}
			return x;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00033559 File Offset: 0x00031759
		private static void ThrowMinMaxException<T>(T min, T max)
		{
			throw new ArgumentException(SR.Format("'{0}' cannot be greater than {1}.", min, max));
		}

		/// <summary>Returns the absolute value of a double-precision floating-point number.</summary>
		/// <param name="value">A number that is greater than or equal to <see cref="F:System.Double.MinValue" />, but less than or equal to <see cref="F:System.Double.MaxValue" />.</param>
		/// <returns>A double-precision floating-point number, x, such that 0 ≤ x ≤<see cref="F:System.Double.MaxValue" />.</returns>
		// Token: 0x06000D0B RID: 3339
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Abs(double value);

		/// <summary>Returns the absolute value of a single-precision floating-point number.</summary>
		/// <param name="value">A number that is greater than or equal to <see cref="F:System.Single.MinValue" />, but less than or equal to <see cref="F:System.Single.MaxValue" />.</param>
		/// <returns>A single-precision floating-point number, x, such that 0 ≤ x ≤<see cref="F:System.Single.MaxValue" />.</returns>
		// Token: 0x06000D0C RID: 3340
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Abs(float value);

		/// <summary>Returns the angle whose cosine is the specified number.</summary>
		/// <param name="d">A number representing a cosine, where <paramref name="d" /> must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>An angle, θ, measured in radians, such that 0 ≤θ≤π  
		///  -or-  
		///  <see cref="F:System.Double.NaN" /> if <paramref name="d" /> &lt; -1 or <paramref name="d" /> &gt; 1 or <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D0D RID: 3341
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Acos(double d);

		// Token: 0x06000D0E RID: 3342
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Acosh(double d);

		/// <summary>Returns the angle whose sine is the specified number.</summary>
		/// <param name="d">A number representing a sine, where <paramref name="d" /> must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>An angle, θ, measured in radians, such that -π/2 ≤θ≤π/2  
		///  -or-  
		///  <see cref="F:System.Double.NaN" /> if <paramref name="d" /> &lt; -1 or <paramref name="d" /> &gt; 1 or <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D0F RID: 3343
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Asin(double d);

		// Token: 0x06000D10 RID: 3344
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Asinh(double d);

		/// <summary>Returns the angle whose tangent is the specified number.</summary>
		/// <param name="d">A number representing a tangent.</param>
		/// <returns>An angle, θ, measured in radians, such that -π/2 ≤θ≤π/2.  
		///  -or-  
		///  <see cref="F:System.Double.NaN" /> if <paramref name="d" /> equals <see cref="F:System.Double.NaN" />, -π/2 rounded to double precision (-1.5707963267949) if <paramref name="d" /> equals <see cref="F:System.Double.NegativeInfinity" />, or π/2 rounded to double precision (1.5707963267949) if <paramref name="d" /> equals <see cref="F:System.Double.PositiveInfinity" />.</returns>
		// Token: 0x06000D11 RID: 3345
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan(double d);

		/// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
		/// <param name="y">The y coordinate of a point.</param>
		/// <param name="x">The x coordinate of a point.</param>
		/// <returns>An angle, θ, measured in radians, such that -π≤θ≤π, and tan(θ) = <paramref name="y" /> / <paramref name="x" />, where (<paramref name="x" />, <paramref name="y" />) is a point in the Cartesian plane. Observe the following:  
		///
		/// For (<paramref name="x" />, <paramref name="y" />) in quadrant 1, 0 &lt; θ &lt; π/2.  
		///
		/// For (<paramref name="x" />, <paramref name="y" />) in quadrant 2, π/2 &lt; θ≤π.  
		///
		/// For (<paramref name="x" />, <paramref name="y" />) in quadrant 3, -π &lt; θ &lt; -π/2.  
		///
		/// For (<paramref name="x" />, <paramref name="y" />) in quadrant 4, -π/2 &lt; θ &lt; 0.  
		///
		///
		///  For points on the boundaries of the quadrants, the return value is the following:  
		///
		/// If y is 0 and x is not negative, θ = 0.  
		///
		/// If y is 0 and x is negative, θ = π.  
		///
		/// If y is positive and x is 0, θ = π/2.  
		///
		/// If y is negative and x is 0, θ = -π/2.  
		///
		/// If y is 0 and x is 0, θ = 0.  
		///
		///
		///  If <paramref name="x" /> or <paramref name="y" /> is <see cref="F:System.Double.NaN" />, or if <paramref name="x" /> and <paramref name="y" /> are either <see cref="F:System.Double.PositiveInfinity" /> or <see cref="F:System.Double.NegativeInfinity" />, the method returns <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D12 RID: 3346
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan2(double y, double x);

		// Token: 0x06000D13 RID: 3347
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atanh(double d);

		// Token: 0x06000D14 RID: 3348
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cbrt(double d);

		/// <summary>Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.</summary>
		/// <param name="a">A double-precision floating-point number.</param>
		/// <returns>The smallest integral value that is greater than or equal to <paramref name="a" />. If <paramref name="a" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, that value is returned. Note that this method returns a <see cref="T:System.Double" /> instead of an integral type.</returns>
		// Token: 0x06000D15 RID: 3349
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Ceiling(double a);

		/// <summary>Returns the cosine of the specified angle.</summary>
		/// <param name="d">An angle, measured in radians.</param>
		/// <returns>The cosine of <paramref name="d" />. If <paramref name="d" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D16 RID: 3350
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cos(double d);

		/// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic cosine of <paramref name="value" />. If <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" /> or <see cref="F:System.Double.PositiveInfinity" />, <see cref="F:System.Double.PositiveInfinity" /> is returned. If <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
		// Token: 0x06000D17 RID: 3351
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cosh(double value);

		/// <summary>Returns <see langword="e" /> raised to the specified power.</summary>
		/// <param name="d">A number specifying a power.</param>
		/// <returns>The number <see langword="e" /> raised to the power <paramref name="d" />. If <paramref name="d" /> equals <see cref="F:System.Double.NaN" /> or <see cref="F:System.Double.PositiveInfinity" />, that value is returned. If <paramref name="d" /> equals <see cref="F:System.Double.NegativeInfinity" />, 0 is returned.</returns>
		// Token: 0x06000D18 RID: 3352
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Exp(double d);

		/// <summary>Returns the largest integral value less than or equal to the specified double-precision floating-point number.</summary>
		/// <param name="d">A double-precision floating-point number.</param>
		/// <returns>The largest integral value less than or equal to <paramref name="d" />. If <paramref name="d" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, that value is returned.</returns>
		// Token: 0x06000D19 RID: 3353
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Floor(double d);

		/// <summary>Returns the natural (base <see langword="e" />) logarithm of a specified number.</summary>
		/// <param name="d">The number whose logarithm is to be found.</param>
		/// <returns>One of the values in the following table.  
		///  <paramref name="d" /> parameter  
		///
		///   Return value  
		///
		///   Positive  
		///
		///   The natural logarithm of <paramref name="d" />; that is, ln <paramref name="d" />, or log e <paramref name="d" /> Zero  
		///
		///  <see cref="F:System.Double.NegativeInfinity" /> Negative  
		///
		///  <see cref="F:System.Double.NaN" /> Equal to <see cref="F:System.Double.NaN" /><see cref="F:System.Double.NaN" /> Equal to <see cref="F:System.Double.PositiveInfinity" /><see cref="F:System.Double.PositiveInfinity" /></returns>
		// Token: 0x06000D1A RID: 3354
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log(double d);

		/// <summary>Returns the base 10 logarithm of a specified number.</summary>
		/// <param name="d">A number whose logarithm is to be found.</param>
		/// <returns>One of the values in the following table.  
		///  <paramref name="d" /> parameter  
		///
		///   Return value  
		///
		///   Positive  
		///
		///   The base 10 log of <paramref name="d" />; that is, log 10<paramref name="d" />.  
		///
		///   Zero  
		///
		///  <see cref="F:System.Double.NegativeInfinity" /> Negative  
		///
		///  <see cref="F:System.Double.NaN" /> Equal to <see cref="F:System.Double.NaN" /><see cref="F:System.Double.NaN" /> Equal to <see cref="F:System.Double.PositiveInfinity" /><see cref="F:System.Double.PositiveInfinity" /></returns>
		// Token: 0x06000D1B RID: 3355
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log10(double d);

		/// <summary>Returns a specified number raised to the specified power.</summary>
		/// <param name="x">A double-precision floating-point number to be raised to a power.</param>
		/// <param name="y">A double-precision floating-point number that specifies a power.</param>
		/// <returns>The number <paramref name="x" /> raised to the power <paramref name="y" />.</returns>
		// Token: 0x06000D1C RID: 3356
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Pow(double x, double y);

		/// <summary>Returns the sine of the specified angle.</summary>
		/// <param name="a">An angle, measured in radians.</param>
		/// <returns>The sine of <paramref name="a" />. If <paramref name="a" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D1D RID: 3357
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sin(double a);

		/// <summary>Returns the hyperbolic sine of the specified angle.</summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic sine of <paramref name="value" />. If <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NaN" />, this method returns a <see cref="T:System.Double" /> equal to <paramref name="value" />.</returns>
		// Token: 0x06000D1E RID: 3358
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sinh(double value);

		/// <summary>Returns the square root of a specified number.</summary>
		/// <param name="d">The number whose square root is to be found.</param>
		/// <returns>One of the values in the following table.  
		///  <paramref name="d" /> parameter  
		///
		///   Return value  
		///
		///   Zero or positive  
		///
		///   The positive square root of <paramref name="d" />.  
		///
		///   Negative  
		///
		///  <see cref="F:System.Double.NaN" /> Equals <see cref="F:System.Double.NaN" /><see cref="F:System.Double.NaN" /> Equals <see cref="F:System.Double.PositiveInfinity" /><see cref="F:System.Double.PositiveInfinity" /></returns>
		// Token: 0x06000D1F RID: 3359
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sqrt(double d);

		/// <summary>Returns the tangent of the specified angle.</summary>
		/// <param name="a">An angle, measured in radians.</param>
		/// <returns>The tangent of <paramref name="a" />. If <paramref name="a" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D20 RID: 3360
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tan(double a);

		/// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic tangent of <paramref name="value" />. If <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" />, this method returns -1. If value is equal to <see cref="F:System.Double.PositiveInfinity" />, this method returns 1. If <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x06000D21 RID: 3361
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tanh(double value);

		// Token: 0x06000D22 RID: 3362
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double FMod(double x, double y);

		// Token: 0x06000D23 RID: 3363
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern double ModF(double x, double* intptr);

		/// <summary>Represents the natural logarithmic base, specified by the constant, <see langword="e" />.</summary>
		// Token: 0x04001272 RID: 4722
		public const double E = 2.718281828459045;

		/// <summary>Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.</summary>
		// Token: 0x04001273 RID: 4723
		public const double PI = 3.141592653589793;

		// Token: 0x04001274 RID: 4724
		private const int maxRoundingDigits = 15;

		// Token: 0x04001275 RID: 4725
		private static double doubleRoundLimit = 10000000000000000.0;

		// Token: 0x04001276 RID: 4726
		private static double[] roundPower10Double = new double[]
		{
			1.0, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, 10000000.0, 100000000.0, 1000000000.0,
			10000000000.0, 100000000000.0, 1000000000000.0, 10000000000000.0, 100000000000000.0, 1000000000000000.0
		};
	}
}
