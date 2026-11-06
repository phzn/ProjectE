using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000034 RID: 52
	internal struct ArchetypeListMap : IDisposable
	{
		// Token: 0x0600012B RID: 299 RVA: 0x00006AA0 File Offset: 0x00004CA0
		private unsafe static uint GetHashCode(ComponentTypeInArchetype* type, int types)
		{
			uint num = math.hash((void*)type, types * sizeof(ComponentTypeInArchetype), 0U);
			if (num == 0U || num == 4294967295U)
			{
				num = 1U;
			}
			return num;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006AC7 File Offset: 0x00004CC7
		public int Size
		{
			get
			{
				return this.hashes.Length;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public int UnoccupiedNodes
		{
			get
			{
				return this.emptyNodes + this.skipNodes;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006AE3 File Offset: 0x00004CE3
		public int OccupiedNodes
		{
			get
			{
				return this.Size - this.UnoccupiedNodes;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006AF2 File Offset: 0x00004CF2
		public bool IsEmpty
		{
			get
			{
				return this.OccupiedNodes == 0;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006AFD File Offset: 0x00004CFD
		private int hashMask
		{
			get
			{
				return this.Size - 1;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000668C File Offset: 0x0000488C
		public int MinimumSize
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006B07 File Offset: 0x00004D07
		public void SetCapacity(int capacity)
		{
			if (capacity < this.MinimumSize)
			{
				capacity = this.MinimumSize;
			}
			this.archetypes.SetCapacity(capacity);
			this.hashes.SetCapacity(capacity);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006B34 File Offset: 0x00004D34
		public void Init(int count)
		{
			if (count < this.MinimumSize)
			{
				count = this.MinimumSize;
			}
			this.hashes = new UnsafeList<uint>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.hashes.Resize(count, NativeArrayOptions.ClearMemory);
			this.archetypes = new UnsafePtrList<Archetype>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.archetypes.Resize(count, NativeArrayOptions.UninitializedMemory);
			this.emptyNodes = count;
			this.skipNodes = 0;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006BA0 File Offset: 0x00004DA0
		public unsafe void AppendFrom(ArchetypeListMap* src)
		{
			for (int i = 0; i < src->Size; i++)
			{
				uint num = src->hashes.Ptr[i];
				if (num != 0U && num != 4294967295U)
				{
					this.Add(*(IntPtr*)(src->archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*)));
				}
			}
			src->Dispose();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006BFC File Offset: 0x00004DFC
		public unsafe Archetype* TryGet(ComponentTypeInArchetype* type, int types)
		{
			uint hashCode = ArchetypeListMap.GetHashCode(type, types);
			int num = (int)(hashCode & (uint)this.hashMask);
			int num2 = 0;
			for (;;)
			{
				uint num3 = this.hashes.Ptr[num];
				if (num3 == 0U)
				{
					break;
				}
				if (num3 == hashCode)
				{
					Archetype* ptr = *(IntPtr*)(this.archetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
					if (ptr->TypesCount == types && UnsafeUtility.MemCmp((void*)ptr->Types, (void*)type, (long)(types * sizeof(ComponentTypeInArchetype))) == 0)
					{
						return ptr;
					}
				}
				num = (num + 1) & this.hashMask;
				num2++;
				if (num2 == this.Size)
				{
					goto Block_5;
				}
			}
			return null;
			Block_5:
			return null;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006C8F File Offset: 0x00004E8F
		public unsafe Archetype* Get(ComponentTypeInArchetype* type, int types)
		{
			return this.TryGet(type, types);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006C9C File Offset: 0x00004E9C
		public unsafe void Resize(int size)
		{
			if (size < this.MinimumSize)
			{
				size = this.MinimumSize;
			}
			if (size == this.Size)
			{
				return;
			}
			ArchetypeListMap archetypeListMap = this;
			this = default(ArchetypeListMap);
			this.Init(size);
			this.AppendFrom(&archetypeListMap);
			archetypeListMap.Dispose();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006CE9 File Offset: 0x00004EE9
		public void PossiblyGrow()
		{
			if (this.UnoccupiedNodes < this.Size / 3)
			{
				this.Resize(this.Size * 2);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006D09 File Offset: 0x00004F09
		public void PossiblyShrink()
		{
			if (this.OccupiedNodes < this.Size / 3)
			{
				this.Resize(this.Size / 2);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006D2C File Offset: 0x00004F2C
		public unsafe void Add(Archetype* archetype)
		{
			uint hashCode = ArchetypeListMap.GetHashCode(archetype->Types, archetype->TypesCount);
			int num = (int)(hashCode & (uint)this.hashMask);
			int num2 = 0;
			for (;;)
			{
				uint num3 = this.hashes.Ptr[num];
				if (num3 == 0U)
				{
					break;
				}
				if (num3 == 4294967295U)
				{
					goto Block_2;
				}
				num = (num + 1) & this.hashMask;
				num2++;
			}
			this.hashes.Ptr[num] = hashCode;
			*(IntPtr*)(this.archetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*)) = archetype;
			this.emptyNodes--;
			this.PossiblyGrow();
			return;
			Block_2:
			this.hashes.Ptr[num] = hashCode;
			*(IntPtr*)(this.archetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*)) = archetype;
			this.skipNodes--;
			this.PossiblyGrow();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006E00 File Offset: 0x00005000
		public unsafe int IndexOf(Archetype* archetype)
		{
			uint hashCode = ArchetypeListMap.GetHashCode(archetype->Types, archetype->TypesCount);
			int num = (int)(hashCode & (uint)this.hashMask);
			uint num2 = 0U;
			for (;;)
			{
				uint num3 = this.hashes.Ptr[num];
				if (num3 == 0U)
				{
					break;
				}
				if (num3 == hashCode && *(IntPtr*)(this.archetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*)) == archetype)
				{
					return num;
				}
				num = (num + 1) & this.hashMask;
				num2 += 1U;
				if ((ulong)num2 == (ulong)((long)this.Size))
				{
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006E7C File Offset: 0x0000507C
		public unsafe void Remove(Archetype* archetype)
		{
			int num = this.IndexOf(archetype);
			this.hashes.Ptr[num] = uint.MaxValue;
			this.skipNodes++;
			this.PossiblyShrink();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006EB7 File Offset: 0x000050B7
		public unsafe bool Contains(Archetype* archetype)
		{
			return this.IndexOf(archetype) != -1;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006EC6 File Offset: 0x000050C6
		public void Dispose()
		{
			this.hashes.Dispose();
			this.archetypes.Dispose();
			this.emptyNodes = 0;
			this.skipNodes = 0;
		}

		// Token: 0x0400004E RID: 78
		private const uint kAValidHashCode = 1U;

		// Token: 0x0400004F RID: 79
		private const uint kSkipCode = 4294967295U;

		// Token: 0x04000050 RID: 80
		public UnsafeList<uint> hashes;

		// Token: 0x04000051 RID: 81
		public UnsafePtrList<Archetype> archetypes;

		// Token: 0x04000052 RID: 82
		public int emptyNodes;

		// Token: 0x04000053 RID: 83
		public int skipNodes;
	}
}
