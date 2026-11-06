using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x02000129 RID: 297
	[DebuggerDisplay("Length = {Length}")]
	[DebuggerTypeProxy(typeof(ArraySliceDebugView<>))]
	internal struct ArraySlice<T> : IEquatable<ArraySlice<T>> where T : struct
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x000420E8 File Offset: 0x000402E8
		public unsafe ArraySlice(NativeArray<T> array, int start, int length)
		{
			this.m_Stride = UnsafeUtility.SizeOf<T>();
			byte* ptr = (byte*)array.GetUnsafePtr<T>() + this.m_Stride * start;
			this.m_Buffer = ptr;
			this.m_Length = length;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0004211E File Offset: 0x0004031E
		public bool Equals(ArraySlice<T> other)
		{
			return this.m_Buffer == other.m_Buffer && this.m_Stride == other.m_Stride && this.m_Length == other.m_Length;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0004214C File Offset: 0x0004034C
		public override bool Equals(object obj)
		{
			return obj != null && obj is ArraySlice<T> && this.Equals((ArraySlice<T>)obj);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00042169 File Offset: 0x00040369
		public override int GetHashCode()
		{
			return (((this.m_Buffer * 397) ^ this.m_Stride) * 397) ^ this.m_Length;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0004218C File Offset: 0x0004038C
		public static bool operator ==(ArraySlice<T> left, ArraySlice<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00042196 File Offset: 0x00040396
		public static bool operator !=(ArraySlice<T> left, ArraySlice<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000421A4 File Offset: 0x000403A4
		public unsafe static ArraySlice<T> ConvertExistingDataToArraySlice(void* dataPointer, int stride, int length)
		{
			if (length < 0)
			{
				throw new ArgumentException(string.Format("Invalid length of '{0}'. It must be greater than 0.", length), "length");
			}
			if (stride < 0)
			{
				throw new ArgumentException(string.Format("Invalid stride '{0}'. It must be greater than 0.", stride), "stride");
			}
			return new ArraySlice<T>
			{
				m_Stride = stride,
				m_Buffer = (byte*)dataPointer,
				m_Length = length
			};
		}

		// Token: 0x17000240 RID: 576
		public unsafe T this[int index]
		{
			get
			{
				return UnsafeUtility.ReadArrayElementWithStride<T>((void*)this.m_Buffer, index, this.m_Stride);
			}
			[WriteAccessRequired]
			set
			{
				UnsafeUtility.WriteArrayElementWithStride<T>((void*)this.m_Buffer, index, this.m_Stride, value);
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00042239 File Offset: 0x00040439
		internal unsafe void* GetUnsafeReadOnlyPtr()
		{
			return (void*)this.m_Buffer;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00042244 File Offset: 0x00040444
		internal unsafe void CopyTo(T[] array)
		{
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			int num = UnsafeUtility.SizeOf<T>();
			UnsafeUtility.MemCpyStride((void*)intPtr, num, this.GetUnsafeReadOnlyPtr(), this.Stride, num, this.m_Length);
			gchandle.Free();
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0004228C File Offset: 0x0004048C
		internal T[] ToArray()
		{
			T[] array = new T[this.Length];
			this.CopyTo(array);
			return array;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x000422AD File Offset: 0x000404AD
		public int Stride
		{
			get
			{
				return this.m_Stride;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x000422B5 File Offset: 0x000404B5
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x040008D5 RID: 2261
		[NativeDisableUnsafePtrRestriction]
		internal unsafe byte* m_Buffer;

		// Token: 0x040008D6 RID: 2262
		internal int m_Stride;

		// Token: 0x040008D7 RID: 2263
		internal int m_Length;
	}
}
