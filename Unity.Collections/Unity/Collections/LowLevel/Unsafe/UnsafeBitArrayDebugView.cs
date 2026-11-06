using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000108 RID: 264
	internal sealed class UnsafeBitArrayDebugView
	{
		// Token: 0x06000B46 RID: 2886 RVA: 0x0002250E File Offset: 0x0002070E
		public UnsafeBitArrayDebugView(UnsafeBitArray data)
		{
			this.Data = data;
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00022520 File Offset: 0x00020720
		public bool[] Bits
		{
			get
			{
				bool[] array = new bool[this.Data.Length];
				for (int i = 0; i < this.Data.Length; i++)
				{
					array[i] = this.Data.IsSet(i);
				}
				return array;
			}
		}

		// Token: 0x040004AC RID: 1196
		private UnsafeBitArray Data;
	}
}
