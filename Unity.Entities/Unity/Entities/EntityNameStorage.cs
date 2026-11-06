using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000188 RID: 392
	[DebuggerTypeProxy(typeof(EntityNameStorageDebugView))]
	[GenerateTestsForBurstCompatibility]
	internal struct EntityNameStorage
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00031152 File Offset: 0x0002F352
		public static int Entries
		{
			get
			{
				return EntityNameStorage.s_State.Data.entries;
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00031164 File Offset: 0x0002F364
		public static void Initialize()
		{
			if (EntityNameStorage.s_State.Data.initialized != 0)
			{
				return;
			}
			EntityNameStorage.s_State.Data.buffer = new UnsafeList<byte>(1048576, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			EntityNameStorage.s_State.Data.buffer.Length = EntityNameStorage.s_State.Data.buffer.Capacity;
			EntityNameStorage.s_State.Data.entry = new UnsafeList<EntityNameStorage.Entry>(16384, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			EntityNameStorage.s_State.Data.entry.Length = EntityNameStorage.s_State.Data.entry.Capacity;
			EntityNameStorage.s_State.Data.hash = new UnsafeParallelMultiHashMap<int, int>(16384, Allocator.Persistent);
			EntityNameStorage.Clear();
			EntityNameStorage.s_State.Data.initialized = 1;
			EntityNameStorage.s_State.Data.hasLoggedError = 0;
			EntityNameStorage.s_State.Data.kMaxEntriesMsg = "Max unique Entity Name capacity exceeded. If you require more storage, edit EntityNameStorage.cs and change the value of kMaxEntries to pre-allocate more space.";
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00031270 File Offset: 0x0002F470
		public static void Shutdown()
		{
			if (EntityNameStorage.s_State.Data.initialized == 0)
			{
				return;
			}
			EntityNameStorage.s_State.Data.buffer.Dispose();
			EntityNameStorage.s_State.Data.entry.Dispose();
			EntityNameStorage.s_State.Data.hash.Dispose();
			EntityNameStorage.s_State.Data.initialized = 0;
			EntityNameStorage.s_State.Data.hasLoggedError = 0;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x000312EC File Offset: 0x0002F4EC
		public static void Clear()
		{
			EntityNameStorage.s_State.Data.chars = 0;
			EntityNameStorage.s_State.Data.entries = 0;
			EntityNameStorage.s_State.Data.hash.Clear();
			FixedString64Bytes fixedString64Bytes = default(FixedString64Bytes);
			EntityNameStorage.GetOrCreateIndex(in fixedString64Bytes);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00031340 File Offset: 0x0002F540
		public unsafe static void GetFixedString(int index, ref FixedString64Bytes temp)
		{
			EntityNameStorage.Entry entry = EntityNameStorage.s_State.Data.entry[index];
			temp.Length = math.min(entry.length, temp.Capacity);
			UnsafeUtility.MemCpy((void*)temp.GetUnsafePtr(), (void*)(EntityNameStorage.s_State.Data.buffer.Ptr + entry.offset), (long)temp.Length);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x000313A8 File Offset: 0x0002F5A8
		public static int GetIndexFromHashAndFixedString(int hash, in FixedString64Bytes fixedString)
		{
			int num;
			NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
			if (EntityNameStorage.s_State.Data.hash.TryGetFirstValue(hash, out num, out nativeParallelMultiHashMapIterator))
			{
				for (;;)
				{
					EntityNameStorage.Entry entry = EntityNameStorage.s_State.Data.entry[num];
					if (entry.length == fixedString.Length)
					{
						int num2 = 0;
						while (num2 < entry.length && fixedString[num2] == EntityNameStorage.s_State.Data.buffer[entry.offset + num2])
						{
							num2++;
						}
						if (num2 == fixedString.Length)
						{
							break;
						}
					}
					if (!EntityNameStorage.s_State.Data.hash.TryGetNextValue(out num, ref nativeParallelMultiHashMapIterator))
					{
						return -1;
					}
				}
				return num;
			}
			return -1;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00031450 File Offset: 0x0002F650
		public static bool Contains(in FixedString64Bytes value)
		{
			FixedString64Bytes fixedString64Bytes = value;
			return EntityNameStorage.GetIndexFromHashAndFixedString(fixedString64Bytes.GetHashCode(), in value) != -1;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00031480 File Offset: 0x0002F680
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static bool Contains(string value)
		{
			FixedString64Bytes fixedString64Bytes = value;
			return EntityNameStorage.Contains(in fixedString64Bytes);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003149C File Offset: 0x0002F69C
		public static int GetOrCreateIndex(in FixedString64Bytes value)
		{
			FixedString64Bytes fixedString64Bytes = value;
			int hashCode = fixedString64Bytes.GetHashCode();
			int indexFromHashAndFixedString = EntityNameStorage.GetIndexFromHashAndFixedString(hashCode, in value);
			if (indexFromHashAndFixedString != -1)
			{
				return indexFromHashAndFixedString;
			}
			if (EntityNameStorage.s_State.Data.entries >= 16384)
			{
				return -1;
			}
			int chars = EntityNameStorage.s_State.Data.chars;
			ushort num = (ushort)value.Length;
			int num2;
			for (int i = 0; i < (int)num; i++)
			{
				ref EntityNameStorage.State data = ref EntityNameStorage.s_State.Data;
				ref EntityNameStorage.State data2 = ref EntityNameStorage.s_State.Data;
				num2 = data2.chars;
				data2.chars = num2 + 1;
				data.buffer[num2] = value[i];
			}
			EntityNameStorage.s_State.Data.entry[EntityNameStorage.s_State.Data.entries] = new EntityNameStorage.Entry
			{
				offset = chars,
				length = (int)num
			};
			EntityNameStorage.s_State.Data.hash.Add(hashCode, EntityNameStorage.s_State.Data.entries);
			ref EntityNameStorage.State data3 = ref EntityNameStorage.s_State.Data;
			num2 = data3.entries;
			data3.entries = num2 + 1;
			return num2;
		}

		// Token: 0x040004A0 RID: 1184
		internal static readonly SharedStatic<EntityNameStorage.State> s_State = SharedStatic<EntityNameStorage.State>.GetOrCreateUnsafe(0U, 1424792471359754152L, 0L);

		// Token: 0x040004A1 RID: 1185
		internal const int kMaxEntries = 16384;

		// Token: 0x040004A2 RID: 1186
		internal const int kMaxChars = 1048576;

		// Token: 0x040004A3 RID: 1187
		internal const int kErrorExceedMaxEntryCapacity = -1;

		// Token: 0x040004A4 RID: 1188
		public const int kEntityNameMaxLengthBytes = 61;

		// Token: 0x02000189 RID: 393
		internal struct Entry
		{
			// Token: 0x040004A5 RID: 1189
			public int offset;

			// Token: 0x040004A6 RID: 1190
			public int length;
		}

		// Token: 0x0200018A RID: 394
		internal struct State
		{
			// Token: 0x040004A7 RID: 1191
			public byte initialized;

			// Token: 0x040004A8 RID: 1192
			internal byte hasLoggedError;

			// Token: 0x040004A9 RID: 1193
			public UnsafeList<byte> buffer;

			// Token: 0x040004AA RID: 1194
			public UnsafeList<EntityNameStorage.Entry> entry;

			// Token: 0x040004AB RID: 1195
			public UnsafeParallelMultiHashMap<int, int> hash;

			// Token: 0x040004AC RID: 1196
			public int chars;

			// Token: 0x040004AD RID: 1197
			public int entries;

			// Token: 0x040004AE RID: 1198
			public FixedString512Bytes kMaxEntriesMsg;
		}
	}
}
