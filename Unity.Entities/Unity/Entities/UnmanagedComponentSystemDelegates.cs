using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x020002CE RID: 718
	internal struct UnmanagedComponentSystemDelegates
	{
		// Token: 0x0600133B RID: 4923 RVA: 0x0004B580 File Offset: 0x00049780
		internal unsafe void Dispose()
		{
			for (int i = 5; i >= 0; i--)
			{
				if (*((ref this.ManagedFunctions.FixedElementField) + (IntPtr)i * 8) != 0UL)
				{
					GCHandle.FromIntPtr((IntPtr)((long)(*((ref this.ManagedFunctions.FixedElementField) + (IntPtr)i * 8)))).Free();
				}
				if (*((ref this.GCDefeat1.FixedElementField) + (IntPtr)i * 8) != 0UL)
				{
					GCHandle.FromIntPtr((IntPtr)((long)(*((ref this.GCDefeat1.FixedElementField) + (IntPtr)i * 8)))).Free();
				}
			}
		}

		// Token: 0x040006EF RID: 1775
		[FixedBuffer(typeof(ulong), 6)]
		internal UnmanagedComponentSystemDelegates.<BurstFunctions>e__FixedBuffer BurstFunctions;

		// Token: 0x040006F0 RID: 1776
		[FixedBuffer(typeof(ulong), 6)]
		internal UnmanagedComponentSystemDelegates.<ManagedFunctions>e__FixedBuffer ManagedFunctions;

		// Token: 0x040006F1 RID: 1777
		[FixedBuffer(typeof(ulong), 6)]
		internal UnmanagedComponentSystemDelegates.<GCDefeat1>e__FixedBuffer GCDefeat1;

		// Token: 0x040006F2 RID: 1778
		internal ushort PresentFunctionBits;

		// Token: 0x040006F3 RID: 1779
		internal ushort BurstFunctionBits;

		// Token: 0x020002CF RID: 719
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 48)]
		public struct <BurstFunctions>e__FixedBuffer
		{
			// Token: 0x040006F4 RID: 1780
			public ulong FixedElementField;
		}

		// Token: 0x020002D0 RID: 720
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 48)]
		public struct <GCDefeat1>e__FixedBuffer
		{
			// Token: 0x040006F5 RID: 1781
			public ulong FixedElementField;
		}

		// Token: 0x020002D1 RID: 721
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 48)]
		public struct <ManagedFunctions>e__FixedBuffer
		{
			// Token: 0x040006F6 RID: 1782
			public ulong FixedElementField;
		}
	}
}
