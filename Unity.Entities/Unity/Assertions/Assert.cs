using System;
using System.Diagnostics;

namespace Unity.Assertions
{
	// Token: 0x02000009 RID: 9
	[DebuggerStepThrough]
	public static class Assert
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000227A File Offset: 0x0000047A
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsTrue(bool condition)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000227A File Offset: 0x0000047A
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsTrue(bool condition, string message)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000227A File Offset: 0x0000047A
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsFalse(bool condition)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000227A File Offset: 0x0000047A
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsFalse(bool condition, string message)
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000227E File Offset: 0x0000047E
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNull<T>(T value) where T : class
		{
			T t = value;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000227E File Offset: 0x0000047E
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNull<T>(T value, string message) where T : class
		{
			T t = value;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000227E File Offset: 0x0000047E
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNotNull<T>(T value) where T : class
		{
			T t = value;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000227E File Offset: 0x0000047E
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNotNull<T>(T value, string message) where T : class
		{
			T t = value;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual, string message)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002289 File Offset: 0x00000489
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual<T>(T expected, T actual) where T : IEquatable<T>
		{
			expected.Equals(actual);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002289 File Offset: 0x00000489
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual<T>(T expected, T actual, string message) where T : IEquatable<T>
		{
			expected.Equals(actual);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002289 File Offset: 0x00000489
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual<T>(T expected, T actual) where T : IEquatable<T>
		{
			expected.Equals(actual);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002289 File Offset: 0x00000489
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual<T>(T expected, T actual, string message) where T : IEquatable<T>
		{
			expected.Equals(actual);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000229A File Offset: 0x0000049A
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual(int expected, int actual)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000229A File Offset: 0x0000049A
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual(int expected, int actual)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000229A File Offset: 0x0000049A
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual(bool expected, bool actual)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000022A0 File Offset: 0x000004A0
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual(Type expected, Type actual)
		{
			expected == actual;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000022AA File Offset: 0x000004AA
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual(IntPtr expected, IntPtr actual)
		{
			expected == actual;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000229A File Offset: 0x0000049A
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual(bool expected, bool actual)
		{
		}
	}
}
