using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200001A RID: 26
	[NativeHeader("Modules/Physics/ArticulationBody.h")]
	public struct ArticulationReducedSpace
	{
		// Token: 0x17000033 RID: 51
		public unsafe float this[int i]
		{
			get
			{
				bool flag = i < 0 || i >= this.dofCount;
				if (flag)
				{
					throw new IndexOutOfRangeException();
				}
				return *((ref this.x.FixedElementField) + (IntPtr)i * 4);
			}
			set
			{
				bool flag = i < 0 || i >= this.dofCount;
				if (flag)
				{
					throw new IndexOutOfRangeException();
				}
				*((ref this.x.FixedElementField) + (IntPtr)i * 4) = value;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002A45 File Offset: 0x00000C45
		public ArticulationReducedSpace(float a)
		{
			this.x.FixedElementField = a;
			this.dofCount = 1;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002A5C File Offset: 0x00000C5C
		public unsafe ArticulationReducedSpace(float a, float b)
		{
			this.x.FixedElementField = a;
			*((ref this.x.FixedElementField) + 4) = b;
			this.dofCount = 2;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002A82 File Offset: 0x00000C82
		public unsafe ArticulationReducedSpace(float a, float b, float c)
		{
			this.x.FixedElementField = a;
			*((ref this.x.FixedElementField) + 4) = b;
			*((ref this.x.FixedElementField) + (IntPtr)2 * 4) = c;
			this.dofCount = 3;
		}

		// Token: 0x04000079 RID: 121
		[FixedBuffer(typeof(float), 3)]
		private ArticulationReducedSpace.<x>e__FixedBuffer x;

		// Token: 0x0400007A RID: 122
		public int dofCount;

		// Token: 0x0200001B RID: 27
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 12)]
		public struct <x>e__FixedBuffer
		{
			// Token: 0x0400007B RID: 123
			public float FixedElementField;
		}
	}
}
