using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000026 RID: 38
	[MayOnlyLiveInBlobStorage]
	[DebuggerDisplay("Cannot display the value of a BlobArray by itself. Please inspect the root BlobAssetReference instead.")]
	public struct BlobArray<T> where T : struct
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003530 File Offset: 0x00001730
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003538 File Offset: 0x00001738
		public unsafe void* GetUnsafePtr()
		{
			fixed (int* ptr = &this.m_OffsetPtr)
			{
				return (void*)((byte*)ptr + this.m_OffsetPtr);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003555 File Offset: 0x00001755
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void AssertIndexInRange(int index)
		{
			if (index >= this.m_Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range Length {1}", index, this.m_Length));
			}
		}

		// Token: 0x17000013 RID: 19
		public unsafe ref T this[int index]
		{
			get
			{
				fixed (int* ptr = &this.m_OffsetPtr)
				{
					return UnsafeUtility.ArrayElementAsRef<T>((void*)((byte*)ptr + this.m_OffsetPtr), index);
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowOnCopyOfBlobArrayFields(Type t)
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000035A8 File Offset: 0x000017A8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowOnCopyOfBlobArrayFieldsRecursive(Type t, Type rootType)
		{
			if (t.IsPointer || t.IsPrimitive || t.IsEnum || t == typeof(string))
			{
				return;
			}
			if (t.IsGenericType)
			{
				if (t.GetGenericTypeDefinition() == typeof(BlobArray<>))
				{
					throw new InvalidOperationException("Type " + rootType.Name + " cannot be copied because it contains nested fields of type BlobArray<>.");
				}
				if (t.GetGenericTypeDefinition() == typeof(BlobPtr<>))
				{
					throw new InvalidOperationException("Type " + rootType.Name + " cannot be copied because it contains nested fields of type BlobPtr<>.");
				}
			}
			FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int i = 0;
			int num = ((fields != null) ? fields.Length : 0);
			while (i < num)
			{
				FieldInfo fieldInfo = fields[i];
				i++;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003670 File Offset: 0x00001870
		public unsafe T[] ToArray()
		{
			T[] array = new T[this.m_Length];
			if (this.m_Length > 0)
			{
				void* unsafePtr = this.GetUnsafePtr();
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				UnsafeUtility.MemCpy((void*)gchandle.AddrOfPinnedObject(), unsafePtr, (long)(this.m_Length * UnsafeUtility.SizeOf<T>()));
				gchandle.Free();
			}
			return array;
		}

		// Token: 0x04000035 RID: 53
		internal int m_OffsetPtr;

		// Token: 0x04000036 RID: 54
		internal int m_Length;
	}
}
