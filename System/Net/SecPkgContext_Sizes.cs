using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000557 RID: 1367
	[StructLayout(LayoutKind.Sequential)]
	internal class SecPkgContext_Sizes
	{
		// Token: 0x06002C7C RID: 11388 RVA: 0x00097768 File Offset: 0x00095968
		internal unsafe SecPkgContext_Sizes(byte[] memory)
		{
			fixed (byte[] array = memory)
			{
				void* ptr;
				if (memory == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = (void*)(&array[0]);
				}
				IntPtr intPtr = new IntPtr(ptr);
				checked
				{
					try
					{
						this.cbMaxToken = (int)((uint)Marshal.ReadInt32(intPtr));
						this.cbMaxSignature = (int)((uint)Marshal.ReadInt32(intPtr, 4));
						this.cbBlockSize = (int)((uint)Marshal.ReadInt32(intPtr, 8));
						this.cbSecurityTrailer = (int)((uint)Marshal.ReadInt32(intPtr, 12));
					}
					catch (OverflowException)
					{
						NetEventSource.Fail(this, "Negative size.", ".ctor");
						throw;
					}
				}
			}
		}

		// Token: 0x040017D3 RID: 6099
		public readonly int cbMaxToken;

		// Token: 0x040017D4 RID: 6100
		public readonly int cbMaxSignature;

		// Token: 0x040017D5 RID: 6101
		public readonly int cbBlockSize;

		// Token: 0x040017D6 RID: 6102
		public readonly int cbSecurityTrailer;

		// Token: 0x040017D7 RID: 6103
		public static readonly int SizeOf = Marshal.SizeOf<SecPkgContext_Sizes>();
	}
}
