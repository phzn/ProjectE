using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x0200007A RID: 122
	[GenerateTestsForBurstCompatibility]
	public static class FixedString
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x0000F218 File Offset: 0x0000D418
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000F288 File Offset: 0x0000D488
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000F368 File Offset: 0x0000D568
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000F434 File Offset: 0x0000D634
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000F518 File Offset: 0x0000D718
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000F578 File Offset: 0x0000D778
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000F658 File Offset: 0x0000D858
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, int arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000F724 File Offset: 0x0000D924
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000F780 File Offset: 0x0000D980
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000F83C File Offset: 0x0000DA3C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, int arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000F888 File Offset: 0x0000DA88
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000F96C File Offset: 0x0000DB6C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000F9DC File Offset: 0x0000DBDC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000FA3C File Offset: 0x0000DC3C
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0000FB24 File Offset: 0x0000DD24
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0000FB98 File Offset: 0x0000DD98
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0000FC68 File Offset: 0x0000DE68
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, float arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0000FD4C File Offset: 0x0000DF4C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0000FDAC File Offset: 0x0000DFAC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0000FE0C File Offset: 0x0000E00C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0000FE6C File Offset: 0x0000E06C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0000FECC File Offset: 0x0000E0CC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, float arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0000FF1C File Offset: 0x0000E11C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000FF8C File Offset: 0x0000E18C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001006C File Offset: 0x0000E26C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000100C8 File Offset: 0x0000E2C8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00010138 File Offset: 0x0000E338
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000101AC File Offset: 0x0000E3AC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001021C File Offset: 0x0000E41C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001027C File Offset: 0x0000E47C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000102EC File Offset: 0x0000E4EC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001035C File Offset: 0x0000E55C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, string arg2, int arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000103CC File Offset: 0x0000E5CC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00010428 File Offset: 0x0000E628
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00010484 File Offset: 0x0000E684
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000104E4 File Offset: 0x0000E6E4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00010540 File Offset: 0x0000E740
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, string arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001058C File Offset: 0x0000E78C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, int arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000105E8 File Offset: 0x0000E7E8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, int arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00010648 File Offset: 0x0000E848
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, int arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000106A4 File Offset: 0x0000E8A4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, int arg1, T2 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000106F0 File Offset: 0x0000E8F0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, float arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00010750 File Offset: 0x0000E950
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, float arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000107B0 File Offset: 0x0000E9B0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, float arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00010810 File Offset: 0x0000EA10
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, float arg1, T2 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00010860 File Offset: 0x0000EA60
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, string arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000108BC File Offset: 0x0000EABC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, string arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001091C File Offset: 0x0000EB1C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, string arg1, T1 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00010978 File Offset: 0x0000EB78
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, string arg1, T2 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000109C4 File Offset: 0x0000EBC4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, T1 arg1, T2 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00010A10 File Offset: 0x0000EC10
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, T1 arg1, T2 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00010A60 File Offset: 0x0000EC60
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, T1 arg1, T2 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00010AAC File Offset: 0x0000ECAC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, T2 arg1, T3 arg2, int arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, T3, FixedString32Bytes>(in formatString, in arg0, in arg1, in arg2, in fixedString32Bytes);
			return fixedString512Bytes;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00010B58 File Offset: 0x0000ED58
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00010BCC File Offset: 0x0000EDCC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00010C3C File Offset: 0x0000EE3C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00010C9C File Offset: 0x0000EE9C
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00010D10 File Offset: 0x0000EF10
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00010D84 File Offset: 0x0000EF84
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00010E58 File Offset: 0x0000F058
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00010F3C File Offset: 0x0000F13C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, int arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00010FAC File Offset: 0x0000F1AC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001100C File Offset: 0x0000F20C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001106C File Offset: 0x0000F26C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000110CC File Offset: 0x0000F2CC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001112C File Offset: 0x0000F32C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, int arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001117C File Offset: 0x0000F37C
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000111F0 File Offset: 0x0000F3F0
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00011264 File Offset: 0x0000F464
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000112D8 File Offset: 0x0000F4D8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00011338 File Offset: 0x0000F538
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000113AC File Offset: 0x0000F5AC
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00011424 File Offset: 0x0000F624
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00011498 File Offset: 0x0000F698
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000114FC File Offset: 0x0000F6FC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00011570 File Offset: 0x0000F770
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000115E4 File Offset: 0x0000F7E4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, float arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00011658 File Offset: 0x0000F858
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000116B8 File Offset: 0x0000F8B8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00011718 File Offset: 0x0000F918
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001177C File Offset: 0x0000F97C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000117DC File Offset: 0x0000F9DC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, float arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001182C File Offset: 0x0000FA2C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001189C File Offset: 0x0000FA9C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00011910 File Offset: 0x0000FB10
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00011980 File Offset: 0x0000FB80
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000119E0 File Offset: 0x0000FBE0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00011A54 File Offset: 0x0000FC54
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00011AC8 File Offset: 0x0000FCC8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00011B3C File Offset: 0x0000FD3C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00011B9C File Offset: 0x0000FD9C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00011C0C File Offset: 0x0000FE0C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00011C80 File Offset: 0x0000FE80
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, string arg2, float arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00011CF0 File Offset: 0x0000FEF0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00011D50 File Offset: 0x0000FF50
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00011DB0 File Offset: 0x0000FFB0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00011E10 File Offset: 0x00010010
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00011E70 File Offset: 0x00010070
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, string arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00011EC0 File Offset: 0x000100C0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, int arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00011F20 File Offset: 0x00010120
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, int arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00011F80 File Offset: 0x00010180
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, int arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00011FE0 File Offset: 0x000101E0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, int arg1, T2 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00012030 File Offset: 0x00010230
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, float arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00012090 File Offset: 0x00010290
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, float arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000120F4 File Offset: 0x000102F4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, float arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00012154 File Offset: 0x00010354
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, float arg1, T2 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000121A4 File Offset: 0x000103A4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, string arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00012204 File Offset: 0x00010404
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, string arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00012264 File Offset: 0x00010464
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, string arg1, T1 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000122C4 File Offset: 0x000104C4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, string arg1, T2 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00012314 File Offset: 0x00010514
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, T1 arg1, T2 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00012364 File Offset: 0x00010564
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, T1 arg1, T2 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000123B4 File Offset: 0x000105B4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, T1 arg1, T2 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00012404 File Offset: 0x00010604
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, T2 arg1, T3 arg2, float arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg3, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, T3, FixedString32Bytes>(in formatString, in arg0, in arg1, in arg2, in fixedString32Bytes);
			return fixedString512Bytes;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00012440 File Offset: 0x00010640
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000124B0 File Offset: 0x000106B0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00012520 File Offset: 0x00010720
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00012590 File Offset: 0x00010790
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000125EC File Offset: 0x000107EC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001265C File Offset: 0x0001085C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000126D0 File Offset: 0x000108D0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00012740 File Offset: 0x00010940
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000127A0 File Offset: 0x000109A0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00012810 File Offset: 0x00010A10
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00012880 File Offset: 0x00010A80
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, int arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000128F0 File Offset: 0x00010AF0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001294C File Offset: 0x00010B4C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000129A8 File Offset: 0x00010BA8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00012A08 File Offset: 0x00010C08
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00012A64 File Offset: 0x00010C64
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, int arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00012AB0 File Offset: 0x00010CB0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00012B20 File Offset: 0x00010D20
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00012B94 File Offset: 0x00010D94
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00012C04 File Offset: 0x00010E04
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00012C64 File Offset: 0x00010E64
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00012CD8 File Offset: 0x00010ED8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00012D4C File Offset: 0x00010F4C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00012DC0 File Offset: 0x00010FC0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00012E20 File Offset: 0x00011020
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00012E90 File Offset: 0x00011090
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00012F04 File Offset: 0x00011104
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, float arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00012F74 File Offset: 0x00011174
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00012FD4 File Offset: 0x000111D4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00013034 File Offset: 0x00011234
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00013094 File Offset: 0x00011294
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000130F4 File Offset: 0x000112F4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, float arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00013144 File Offset: 0x00011344
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, int arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000131B4 File Offset: 0x000113B4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, int arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00013224 File Offset: 0x00011424
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, int arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00013294 File Offset: 0x00011494
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, int arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000132F0 File Offset: 0x000114F0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, float arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00013360 File Offset: 0x00011560
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, float arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000133D4 File Offset: 0x000115D4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, float arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00013444 File Offset: 0x00011644
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, float arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000134A4 File Offset: 0x000116A4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, int arg0, string arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00013514 File Offset: 0x00011714
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, float arg0, string arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00013584 File Offset: 0x00011784
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format(FixedString512Bytes formatString, string arg0, string arg1, string arg2, string arg3)
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes4 = default(FixedString32Bytes);
			(ref fixedString32Bytes4).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in fixedString32Bytes4);
			return fixedString512Bytes;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000135F4 File Offset: 0x000117F4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, T1 arg0, string arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00013650 File Offset: 0x00011850
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, T1 arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000136AC File Offset: 0x000118AC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, T1 arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001370C File Offset: 0x0001190C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, T1 arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00013768 File Offset: 0x00011968
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, T2 arg1, string arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000137B4 File Offset: 0x000119B4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, int arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00013810 File Offset: 0x00011A10
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, int arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00013870 File Offset: 0x00011A70
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, int arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000138CC File Offset: 0x00011ACC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, int arg1, T2 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00013918 File Offset: 0x00011B18
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, float arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00013978 File Offset: 0x00011B78
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, float arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000139D8 File Offset: 0x00011BD8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, float arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00013A38 File Offset: 0x00011C38
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, float arg1, T2 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00013A88 File Offset: 0x00011C88
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, string arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00013AE4 File Offset: 0x00011CE4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, string arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00013B44 File Offset: 0x00011D44
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, string arg1, T1 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in fixedString32Bytes3);
			return fixedString512Bytes;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00013BA0 File Offset: 0x00011DA0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, string arg1, T2 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00013BEC File Offset: 0x00011DEC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, T1 arg1, T2 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00013C38 File Offset: 0x00011E38
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, T1 arg1, T2 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00013C88 File Offset: 0x00011E88
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, T1 arg1, T2 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in arg2, in fixedString32Bytes2);
			return fixedString512Bytes;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00013CD4 File Offset: 0x00011ED4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, T2 arg1, T3 arg2, string arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg3);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, T3, FixedString32Bytes>(in formatString, in arg0, in arg1, in arg2, in fixedString32Bytes);
			return fixedString512Bytes;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00013D10 File Offset: 0x00011F10
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, int arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00013D6C File Offset: 0x00011F6C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, int arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00013DCC File Offset: 0x00011FCC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, int arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00013E28 File Offset: 0x00012028
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, int arg1, int arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00013E74 File Offset: 0x00012074
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, float arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00013ED4 File Offset: 0x000120D4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, float arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00013F34 File Offset: 0x00012134
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, float arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00013F94 File Offset: 0x00012194
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, float arg1, int arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00013FE0 File Offset: 0x000121E0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, string arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001403C File Offset: 0x0001223C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, string arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001409C File Offset: 0x0001229C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, string arg1, int arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000140F8 File Offset: 0x000122F8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, string arg1, int arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00014144 File Offset: 0x00012344
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, T1 arg1, int arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00014190 File Offset: 0x00012390
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, T1 arg1, int arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000141DC File Offset: 0x000123DC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, T1 arg1, int arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00014228 File Offset: 0x00012428
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, T2 arg1, int arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, T3>(in formatString, in arg0, in arg1, in fixedString32Bytes, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00014264 File Offset: 0x00012464
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, int arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x000142C4 File Offset: 0x000124C4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, int arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00014324 File Offset: 0x00012524
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, int arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00014384 File Offset: 0x00012584
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, int arg1, float arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x000143D0 File Offset: 0x000125D0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, float arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00014430 File Offset: 0x00012630
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, float arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00014494 File Offset: 0x00012694
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, float arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000144F4 File Offset: 0x000126F4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, float arg1, float arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00014544 File Offset: 0x00012744
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, string arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000145A4 File Offset: 0x000127A4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, string arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00014604 File Offset: 0x00012804
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, string arg1, float arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00014664 File Offset: 0x00012864
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, string arg1, float arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000146B0 File Offset: 0x000128B0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, T1 arg1, float arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x000146FC File Offset: 0x000128FC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, T1 arg1, float arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001474C File Offset: 0x0001294C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, T1 arg1, float arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00014798 File Offset: 0x00012998
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, T2 arg1, float arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, T3>(in formatString, in arg0, in arg1, in fixedString32Bytes, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000147D4 File Offset: 0x000129D4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, int arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00014830 File Offset: 0x00012A30
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, int arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00014890 File Offset: 0x00012A90
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, int arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000148EC File Offset: 0x00012AEC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, int arg1, string arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00014938 File Offset: 0x00012B38
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, float arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00014998 File Offset: 0x00012B98
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, float arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000149F8 File Offset: 0x00012BF8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, float arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00014A58 File Offset: 0x00012C58
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, float arg1, string arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00014AA4 File Offset: 0x00012CA4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, int arg0, string arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00014B00 File Offset: 0x00012D00
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, float arg0, string arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00014B60 File Offset: 0x00012D60
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString512Bytes formatString, string arg0, string arg1, string arg2, T1 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00014BBC File Offset: 0x00012DBC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, T1 arg0, string arg1, string arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00014C08 File Offset: 0x00012E08
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, T1 arg1, string arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00014C54 File Offset: 0x00012E54
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, T1 arg1, string arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00014CA0 File Offset: 0x00012EA0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, T1 arg1, string arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, FixedString32Bytes, T2>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00014CEC File Offset: 0x00012EEC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, T2 arg1, string arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, FixedString32Bytes, T3>(in formatString, in arg0, in arg1, in fixedString32Bytes, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00014D28 File Offset: 0x00012F28
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, int arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00014D74 File Offset: 0x00012F74
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, int arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00014DC0 File Offset: 0x00012FC0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, int arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00014E0C File Offset: 0x0001300C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, int arg1, T2 arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, T3>(in formatString, in arg0, in fixedString32Bytes, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00014E48 File Offset: 0x00013048
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, float arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00014E94 File Offset: 0x00013094
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, float arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00014EE4 File Offset: 0x000130E4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, float arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00014F30 File Offset: 0x00013130
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, float arg1, T2 arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, T3>(in formatString, in arg0, in fixedString32Bytes, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00014F6C File Offset: 0x0001316C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, int arg0, string arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00014FB8 File Offset: 0x000131B8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, float arg0, string arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00015004 File Offset: 0x00013204
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString512Bytes formatString, string arg0, string arg1, T1 arg2, T2 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00015050 File Offset: 0x00013250
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, T1 arg0, string arg1, T2 arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, FixedString32Bytes, T2, T3>(in formatString, in arg0, in fixedString32Bytes, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001508C File Offset: 0x0001328C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, int arg0, T1 arg1, T2 arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, T3>(in formatString, in fixedString32Bytes, in arg1, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000150C8 File Offset: 0x000132C8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, float arg0, T1 arg1, T2 arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, T3>(in formatString, in fixedString32Bytes, in arg1, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00015104 File Offset: 0x00013304
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString512Bytes formatString, string arg0, T1 arg1, T2 arg2, T3 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, FixedString32Bytes, T1, T2, T3>(in formatString, in fixedString32Bytes, in arg1, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00015140 File Offset: 0x00013340
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString512Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3, [global::System.Runtime.CompilerServices.IsUnmanaged] T4>(FixedString512Bytes formatString, T1 arg0, T2 arg1, T3 arg2, T4 arg3) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T4 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			(ref fixedString512Bytes).AppendFormat<FixedString512Bytes, FixedString512Bytes, T1, T2, T3, T4>(in formatString, in arg0, in arg1, in arg2, in arg3);
			return fixedString512Bytes;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00015168 File Offset: 0x00013368
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, int arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000151C4 File Offset: 0x000133C4
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, int arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00015220 File Offset: 0x00013420
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, int arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001527C File Offset: 0x0001347C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, int arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000152C4 File Offset: 0x000134C4
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, float arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00015320 File Offset: 0x00013520
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, float arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00015380 File Offset: 0x00013580
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, float arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000153DC File Offset: 0x000135DC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, float arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00015428 File Offset: 0x00013628
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, string arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00015484 File Offset: 0x00013684
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, string arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000154E0 File Offset: 0x000136E0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, string arg1, int arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001553C File Offset: 0x0001373C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, string arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00015584 File Offset: 0x00013784
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, T1 arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000155CC File Offset: 0x000137CC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, T1 arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00015618 File Offset: 0x00013818
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, T1 arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00015660 File Offset: 0x00013860
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, T2 arg1, int arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, T2, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00015698 File Offset: 0x00013898
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, int arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000156F4 File Offset: 0x000138F4
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, int arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00015754 File Offset: 0x00013954
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, int arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000157B0 File Offset: 0x000139B0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, int arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000157FC File Offset: 0x000139FC
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, float arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001585C File Offset: 0x00013A5C
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, float arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000158BC File Offset: 0x00013ABC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, float arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001591C File Offset: 0x00013B1C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, float arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00015968 File Offset: 0x00013B68
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, string arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000159C4 File Offset: 0x00013BC4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, string arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00015A24 File Offset: 0x00013C24
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, string arg1, float arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00015A80 File Offset: 0x00013C80
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, string arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00015ACC File Offset: 0x00013CCC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, T1 arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00015B18 File Offset: 0x00013D18
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, T1 arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00015B64 File Offset: 0x00013D64
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, T1 arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00015BB0 File Offset: 0x00013DB0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, T2 arg1, float arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, T2, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00015BEC File Offset: 0x00013DEC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, int arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00015C48 File Offset: 0x00013E48
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, int arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00015CA4 File Offset: 0x00013EA4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, int arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00015D00 File Offset: 0x00013F00
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, int arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00015D48 File Offset: 0x00013F48
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, float arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00015DA4 File Offset: 0x00013FA4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, float arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00015E04 File Offset: 0x00014004
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, float arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00015E60 File Offset: 0x00014060
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, float arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00015EAC File Offset: 0x000140AC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, string arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00015F08 File Offset: 0x00014108
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, string arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00015F64 File Offset: 0x00014164
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, string arg1, string arg2)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes3 = default(FixedString32Bytes);
			(ref fixedString32Bytes3).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in fixedString32Bytes3);
			return fixedString128Bytes;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00015FC0 File Offset: 0x000141C0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, string arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00016008 File Offset: 0x00014208
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, T1 arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00016050 File Offset: 0x00014250
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, T1 arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001609C File Offset: 0x0001429C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, T1 arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, FixedString32Bytes>(in formatString, in fixedString32Bytes, in arg1, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000160E4 File Offset: 0x000142E4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, T2 arg1, string arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg2);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, T2, FixedString32Bytes>(in formatString, in arg0, in arg1, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001611C File Offset: 0x0001431C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, int arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00016164 File Offset: 0x00014364
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, int arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000161B0 File Offset: 0x000143B0
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, int arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000161F8 File Offset: 0x000143F8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, int arg1, T2 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00016230 File Offset: 0x00014430
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, float arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001627C File Offset: 0x0001447C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, float arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000162C8 File Offset: 0x000144C8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, float arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00016314 File Offset: 0x00014514
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, float arg1, T2 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00016350 File Offset: 0x00014550
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, string arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00016398 File Offset: 0x00014598
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, string arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000163E4 File Offset: 0x000145E4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, string arg1, T1 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in fixedString32Bytes2, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001642C File Offset: 0x0001462C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, string arg1, T2 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes, T2>(in formatString, in arg0, in fixedString32Bytes, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00016464 File Offset: 0x00014664
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, int arg0, T1 arg1, T2 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in arg1, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001649C File Offset: 0x0001469C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, float arg0, T1 arg1, T2 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in arg1, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000164D8 File Offset: 0x000146D8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, string arg0, T1 arg1, T2 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1, T2>(in formatString, in fixedString32Bytes, in arg1, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00016510 File Offset: 0x00014710
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2, [global::System.Runtime.CompilerServices.IsUnmanaged] T3>(FixedString128Bytes formatString, T1 arg0, T2 arg1, T3 arg2) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T3 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, T2, T3>(in formatString, in arg0, in arg1, in arg2);
			return fixedString128Bytes;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00016538 File Offset: 0x00014738
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, int arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00016580 File Offset: 0x00014780
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, int arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000165C8 File Offset: 0x000147C8
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, int arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00016610 File Offset: 0x00014810
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, int arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00016648 File Offset: 0x00014848
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, float arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00016690 File Offset: 0x00014890
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, float arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000166DC File Offset: 0x000148DC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, float arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00016724 File Offset: 0x00014924
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, float arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001675C File Offset: 0x0001495C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0, string arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000167A4 File Offset: 0x000149A4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0, string arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000167EC File Offset: 0x000149EC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0, string arg1)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			FixedString32Bytes fixedString32Bytes2 = default(FixedString32Bytes);
			(ref fixedString32Bytes2).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes, in fixedString32Bytes2);
			return fixedString128Bytes;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00016834 File Offset: 0x00014A34
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0, string arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg1);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, FixedString32Bytes>(in formatString, in arg0, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001686C File Offset: 0x00014A6C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, int arg0, T1 arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in arg1);
			return fixedString128Bytes;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x000168A4 File Offset: 0x00014AA4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, float arg0, T1 arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in arg1);
			return fixedString128Bytes;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000168DC File Offset: 0x00014ADC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, string arg0, T1 arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes, T1>(in formatString, in fixedString32Bytes, in arg1);
			return fixedString128Bytes;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00016914 File Offset: 0x00014B14
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(FixedString32Bytes),
			typeof(FixedString32Bytes)
		})]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1, [global::System.Runtime.CompilerServices.IsUnmanaged] T2>(FixedString128Bytes formatString, T1 arg0, T2 arg1) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes where T2 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1, T2>(in formatString, in arg0, in arg1);
			return fixedString128Bytes;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00016938 File Offset: 0x00014B38
		public static FixedString128Bytes Format(FixedString128Bytes formatString, int arg0)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001696C File Offset: 0x00014B6C
		public static FixedString128Bytes Format(FixedString128Bytes formatString, float arg0)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0, '.');
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000169A4 File Offset: 0x00014BA4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static FixedString128Bytes Format(FixedString128Bytes formatString, string arg0)
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(arg0);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, FixedString32Bytes>(in formatString, in fixedString32Bytes);
			return fixedString128Bytes;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000169D8 File Offset: 0x00014BD8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedString32Bytes) })]
		public static FixedString128Bytes Format<[global::System.Runtime.CompilerServices.IsUnmanaged] T1>(FixedString128Bytes formatString, T1 arg0) where T1 : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			(ref fixedString128Bytes).AppendFormat<FixedString128Bytes, FixedString128Bytes, T1>(in formatString, in arg0);
			return fixedString128Bytes;
		}
	}
}
