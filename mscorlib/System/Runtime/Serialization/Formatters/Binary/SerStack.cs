using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020006B9 RID: 1721
	internal sealed class SerStack
	{
		// Token: 0x06003FA9 RID: 16297 RVA: 0x000DF3B2 File Offset: 0x000DD5B2
		internal SerStack()
		{
			this.stackId = "System";
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x000DF3D8 File Offset: 0x000DD5D8
		internal SerStack(string stackId)
		{
			this.stackId = stackId;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x000DF3FC File Offset: 0x000DD5FC
		internal void Push(object obj)
		{
			if (this.top == this.objects.Length - 1)
			{
				this.IncreaseCapacity();
			}
			object[] array = this.objects;
			int num = this.top + 1;
			this.top = num;
			array[num] = obj;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x000DF43C File Offset: 0x000DD63C
		internal object Pop()
		{
			if (this.top < 0)
			{
				return null;
			}
			object obj = this.objects[this.top];
			object[] array = this.objects;
			int num = this.top;
			this.top = num - 1;
			array[num] = null;
			return obj;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x000DF47C File Offset: 0x000DD67C
		internal void IncreaseCapacity()
		{
			object[] array = new object[this.objects.Length * 2];
			Array.Copy(this.objects, 0, array, 0, this.objects.Length);
			this.objects = array;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x000DF4B6 File Offset: 0x000DD6B6
		internal object Peek()
		{
			if (this.top < 0)
			{
				return null;
			}
			return this.objects[this.top];
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x000DF4D0 File Offset: 0x000DD6D0
		internal object PeekPeek()
		{
			if (this.top < 1)
			{
				return null;
			}
			return this.objects[this.top - 1];
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x000DF4EC File Offset: 0x000DD6EC
		internal int Count()
		{
			return this.top + 1;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x000DF4F6 File Offset: 0x000DD6F6
		internal bool IsEmpty()
		{
			return this.top <= 0;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x000DF504 File Offset: 0x000DD704
		[Conditional("SER_LOGGING")]
		internal void Dump()
		{
			for (int i = 0; i < this.Count(); i++)
			{
			}
		}

		// Token: 0x040029A9 RID: 10665
		internal object[] objects = new object[5];

		// Token: 0x040029AA RID: 10666
		internal string stackId;

		// Token: 0x040029AB RID: 10667
		internal int top = -1;

		// Token: 0x040029AC RID: 10668
		internal int next;
	}
}
