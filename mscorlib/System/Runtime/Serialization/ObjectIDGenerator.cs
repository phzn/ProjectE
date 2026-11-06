using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Generates IDs for objects.</summary>
	// Token: 0x02000660 RID: 1632
	[ComVisible(true)]
	[Serializable]
	public class ObjectIDGenerator
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" /> class.</summary>
		// Token: 0x06003CE5 RID: 15589 RVA: 0x000D2AD8 File Offset: 0x000D0CD8
		public ObjectIDGenerator()
		{
			this.m_currentCount = 1;
			this.m_currentSize = ObjectIDGenerator.sizes[0];
			this.m_ids = new long[this.m_currentSize * 4];
			this.m_objs = new object[this.m_currentSize * 4];
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x000D2B28 File Offset: 0x000D0D28
		private int FindElement(object obj, out bool found)
		{
			int num = RuntimeHelpers.GetHashCode(obj);
			int num2 = 1 + (num & int.MaxValue) % (this.m_currentSize - 2);
			int i;
			for (;;)
			{
				int num3 = (num & int.MaxValue) % this.m_currentSize * 4;
				for (i = num3; i < num3 + 4; i++)
				{
					if (this.m_objs[i] == null)
					{
						goto Block_1;
					}
					if (this.m_objs[i] == obj)
					{
						goto Block_2;
					}
				}
				num += num2;
			}
			Block_1:
			found = false;
			return i;
			Block_2:
			found = true;
			return i;
		}

		/// <summary>Returns the ID for the specified object, generating a new ID if the specified object has not already been identified by the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />.</summary>
		/// <param name="obj">The object you want an ID for.</param>
		/// <param name="firstTime">
		///   <see langword="true" /> if <paramref name="obj" /> was not previously known to the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />; otherwise, <see langword="false" />.</param>
		/// <returns>The object's ID is used for serialization. <paramref name="firstTime" /> is set to <see langword="true" /> if this is the first time the object has been identified; otherwise, it is set to <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" /> has been asked to keep track of too many objects.</exception>
		// Token: 0x06003CE7 RID: 15591 RVA: 0x000D2B94 File Offset: 0x000D0D94
		public virtual long GetId(object obj, out bool firstTime)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", Environment.GetResourceString("Object cannot be null."));
			}
			bool flag;
			int num = this.FindElement(obj, out flag);
			long num3;
			if (!flag)
			{
				this.m_objs[num] = obj;
				long[] ids = this.m_ids;
				int num2 = num;
				int currentCount = this.m_currentCount;
				this.m_currentCount = currentCount + 1;
				ids[num2] = (long)currentCount;
				num3 = this.m_ids[num];
				if (this.m_currentCount > this.m_currentSize * 4 / 2)
				{
					this.Rehash();
				}
			}
			else
			{
				num3 = this.m_ids[num];
			}
			firstTime = !flag;
			return num3;
		}

		/// <summary>Determines whether an object has already been assigned an ID.</summary>
		/// <param name="obj">The object you are asking for.</param>
		/// <param name="firstTime">
		///   <see langword="true" /> if <paramref name="obj" /> was not previously known to the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />; otherwise, <see langword="false" />.</param>
		/// <returns>The object ID of <paramref name="obj" /> if previously known to the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />; otherwise, zero.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003CE8 RID: 15592 RVA: 0x000D2C1C File Offset: 0x000D0E1C
		public virtual long HasId(object obj, out bool firstTime)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", Environment.GetResourceString("Object cannot be null."));
			}
			bool flag;
			int num = this.FindElement(obj, out flag);
			if (flag)
			{
				firstTime = false;
				return this.m_ids[num];
			}
			firstTime = true;
			return 0L;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x000D2C60 File Offset: 0x000D0E60
		private void Rehash()
		{
			int num = 0;
			int currentSize = this.m_currentSize;
			while (num < ObjectIDGenerator.sizes.Length && ObjectIDGenerator.sizes[num] <= currentSize)
			{
				num++;
			}
			if (num == ObjectIDGenerator.sizes.Length)
			{
				throw new SerializationException(Environment.GetResourceString("The internal array cannot expand to greater than Int32.MaxValue elements."));
			}
			this.m_currentSize = ObjectIDGenerator.sizes[num];
			long[] array = new long[this.m_currentSize * 4];
			object[] array2 = new object[this.m_currentSize * 4];
			long[] ids = this.m_ids;
			object[] objs = this.m_objs;
			this.m_ids = array;
			this.m_objs = array2;
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i] != null)
				{
					bool flag;
					int num2 = this.FindElement(objs[i], out flag);
					this.m_objs[num2] = objs[i];
					this.m_ids[num2] = ids[i];
				}
			}
		}

		// Token: 0x0400273C RID: 10044
		private const int numbins = 4;

		// Token: 0x0400273D RID: 10045
		internal int m_currentCount;

		// Token: 0x0400273E RID: 10046
		internal int m_currentSize;

		// Token: 0x0400273F RID: 10047
		internal long[] m_ids;

		// Token: 0x04002740 RID: 10048
		internal object[] m_objs;

		// Token: 0x04002741 RID: 10049
		private static readonly int[] sizes = new int[]
		{
			5, 11, 29, 47, 97, 197, 397, 797, 1597, 3203,
			6421, 12853, 25717, 51437, 102877, 205759, 411527, 823117, 1646237, 3292489,
			6584983
		};
	}
}
