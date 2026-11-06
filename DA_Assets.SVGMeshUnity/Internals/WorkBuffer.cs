using System;
using System.Linq;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x0200000B RID: 11
	public class WorkBuffer<T>
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00003D23 File Offset: 0x00001F23
		public WorkBuffer(int size = 32)
		{
			this.GrowSize = size;
			this.PrivateData = new T[size];
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003D3E File Offset: 0x00001F3E
		public T[] Data
		{
			get
			{
				return this.PrivateData;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003D46 File Offset: 0x00001F46
		public int UsedSize
		{
			get
			{
				return this.PrivateUsedSize;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003D50 File Offset: 0x00001F50
		private void Grow(int size)
		{
			T[] array = new T[size];
			this.PrivateData.CopyTo(array, 0);
			this.PrivateData = array;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003D78 File Offset: 0x00001F78
		private void GrowIfNeeded()
		{
			if (this.PrivateData.Length == this.PrivateUsedSize)
			{
				this.Grow(this.PrivateData.Length + this.GrowSize);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003D9F File Offset: 0x00001F9F
		public void Extend(int size)
		{
			if (this.PrivateData.Length < size)
			{
				this.Grow(size);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public void Fill(ref T val, int n)
		{
			if (this.PrivateData.Length < n)
			{
				this.Grow(n);
			}
			for (int i = 0; i < n; i++)
			{
				this.PrivateData[i] = val;
			}
			this.PrivateUsedSize = n;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public void Push(ref T val)
		{
			this.GrowIfNeeded();
			this.PrivateData[this.PrivateUsedSize] = val;
			this.PrivateUsedSize++;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003E28 File Offset: 0x00002028
		public T Push()
		{
			this.GrowIfNeeded();
			T t = this.PrivateData[this.PrivateUsedSize];
			if (t == null)
			{
				t = this.NewForClass();
				this.PrivateData[this.PrivateUsedSize] = t;
			}
			this.PrivateUsedSize++;
			return t;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003E82 File Offset: 0x00002082
		public T Pop()
		{
			T t = this.PrivateData[this.PrivateUsedSize - 1];
			this.PrivateUsedSize--;
			return t;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003EA8 File Offset: 0x000020A8
		public T Insert(int index)
		{
			if (index == this.PrivateUsedSize)
			{
				return this.Push();
			}
			this.GrowIfNeeded();
			T t = this.PrivateData[this.PrivateUsedSize];
			for (int i = this.PrivateUsedSize - 1; i >= index; i--)
			{
				this.PrivateData[i + 1] = this.PrivateData[i];
			}
			if (t == null)
			{
				t = this.NewForClass();
			}
			this.PrivateData[index] = t;
			this.PrivateUsedSize++;
			return t;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003F3C File Offset: 0x0000213C
		public void RemoveAt(int index)
		{
			T t = this.PrivateData[index];
			for (int i = index; i < this.PrivateUsedSize - 1; i++)
			{
				this.PrivateData[i] = this.PrivateData[i + 1];
			}
			this.PrivateData[this.PrivateUsedSize - 1] = t;
			this.PrivateUsedSize--;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003FA5 File Offset: 0x000021A5
		public static void Sort<G>(WorkBuffer<G> buf) where G : IComparable<G>
		{
			Sort<G>.QuickSort(buf.PrivateData, 0, buf.PrivateUsedSize - 1);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003FBB File Offset: 0x000021BB
		public void RemoveLast(int n)
		{
			this.PrivateUsedSize -= n;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003FCB File Offset: 0x000021CB
		public void Clear()
		{
			this.PrivateUsedSize = 0;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003FD4 File Offset: 0x000021D4
		public void Dump()
		{
			Debug.Log(this.PrivateData.Take<T>(this.PrivateUsedSize).Aggregate("", (string _, T s) => _ + s.ToString() + "\n"));
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004020 File Offset: 0x00002220
		public void DumpHash()
		{
			string text = "{0}{1}";
			object[] array = new object[2];
			array[0] = this.PrivateUsedSize;
			array[1] = this.PrivateData.Select<T, string>((T _) => string.Format("{0:x}", (_ != null) ? _.GetHashCode() : 0)).Aggregate("", (string _, string s) => _ + ", " + s);
			Debug.LogFormat(text, array);
		}

		// Token: 0x04000028 RID: 40
		public Func<T> NewForClass;

		// Token: 0x04000029 RID: 41
		private int GrowSize;

		// Token: 0x0400002A RID: 42
		private T[] PrivateData;

		// Token: 0x0400002B RID: 43
		private int PrivateUsedSize;
	}
}
