using System;
using System.Diagnostics;
using Unity.Burst.LowLevel;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Burst
{
	// Token: 0x0200001A RID: 26
	public static class BurstRuntime
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00003128 File Offset: 0x00001328
		public static int GetHashCode32<T>()
		{
			return BurstRuntime.HashCode32<T>.Value;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000312F File Offset: 0x0000132F
		public static int GetHashCode32(Type type)
		{
			return BurstRuntime.HashStringWithFNV1A32(type.AssemblyQualifiedName);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000313C File Offset: 0x0000133C
		public static long GetHashCode64<T>()
		{
			return BurstRuntime.HashCode64<T>.Value;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003143 File Offset: 0x00001343
		public static long GetHashCode64(Type type)
		{
			return BurstRuntime.HashStringWithFNV1A64(type.AssemblyQualifiedName);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003150 File Offset: 0x00001350
		internal static int HashStringWithFNV1A32(string text)
		{
			uint num = 2166136261U;
			foreach (char c in text)
			{
				num = 16777619U * (num ^ (uint)((byte)(c & 'ÿ')));
				num = 16777619U * (num ^ (uint)((byte)(c >> 8)));
			}
			return (int)num;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000031A0 File Offset: 0x000013A0
		internal static long HashStringWithFNV1A64(string text)
		{
			ulong num = 14695981039346656037UL;
			foreach (char c in text)
			{
				num = 1099511628211UL * (num ^ (ulong)((byte)(c & 'ÿ')));
				num = 1099511628211UL * (num ^ (ulong)((byte)(c >> 8)));
			}
			return (long)num;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000031FB File Offset: 0x000013FB
		public static bool LoadAdditionalLibrary(string pathToLibBurstGenerated)
		{
			return BurstCompiler.IsLoadAdditionalLibrarySupported() && BurstRuntime.LoadAdditionalLibraryInternal(pathToLibBurstGenerated);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000320C File Offset: 0x0000140C
		internal static bool LoadAdditionalLibraryInternal(string pathToLibBurstGenerated)
		{
			return (bool)typeof(BurstCompilerService).GetMethod("LoadBurstLibrary").Invoke(null, new object[] { pathToLibBurstGenerated });
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003237 File Offset: 0x00001437
		[BurstRuntime.PreserveAttribute]
		internal unsafe static void RuntimeLog(byte* message, int logType, byte* fileName, int lineNumber)
		{
			BurstCompilerService.RuntimeLog(null, (BurstCompilerService.BurstLogType)logType, message, fileName, lineNumber);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002551 File Offset: 0x00000751
		internal static void Initialize()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003244 File Offset: 0x00001444
		[BurstRuntime.PreserveAttribute]
		internal static void PreventRequiredAttributeStrip()
		{
			new BurstDiscardAttribute();
			new ConditionalAttribute("HEJSA");
			new JobProducerTypeAttribute(typeof(BurstRuntime));
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003267 File Offset: 0x00001467
		[BurstRuntime.PreserveAttribute]
		internal unsafe static void Log(byte* message, int logType, byte* fileName, int lineNumber)
		{
			BurstCompilerService.Log(null, (BurstCompilerService.BurstLogType)logType, message, null, lineNumber);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003275 File Offset: 0x00001475
		public unsafe static byte* GetUTF8LiteralPointer(string str, out int byteCount)
		{
			throw new NotImplementedException("This function only works from Burst");
		}

		// Token: 0x0200001B RID: 27
		private struct HashCode32<T>
		{
			// Token: 0x040000D0 RID: 208
			public static readonly int Value = BurstRuntime.HashStringWithFNV1A32(typeof(T).AssemblyQualifiedName);
		}

		// Token: 0x0200001C RID: 28
		private struct HashCode64<T>
		{
			// Token: 0x040000D1 RID: 209
			public static readonly long Value = BurstRuntime.HashStringWithFNV1A64(typeof(T).AssemblyQualifiedName);
		}

		// Token: 0x0200001D RID: 29
		internal class PreserveAttribute : Attribute
		{
		}
	}
}
