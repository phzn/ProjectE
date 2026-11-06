using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	// Token: 0x02000013 RID: 19
	[NativeHeader("Modules/UnityWebRequest/Public/UploadHandler/UploadHandlerRaw.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UploadHandlerRaw : UploadHandler
	{
		// Token: 0x0600011B RID: 283
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr Create(UploadHandlerRaw self, byte* data, int dataLength);

		// Token: 0x0600011C RID: 284 RVA: 0x000051FC File Offset: 0x000033FC
		public UploadHandlerRaw(byte[] data)
			: this((data == null || data.Length == 0) ? default(NativeArray<byte>) : new NativeArray<byte>(data, Allocator.Persistent), true)
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000522C File Offset: 0x0000342C
		public unsafe UploadHandlerRaw(NativeArray<byte> data, bool transferOwnership)
		{
			bool flag = !data.IsCreated || data.Length == 0;
			if (flag)
			{
				this.m_Ptr = UploadHandlerRaw.Create(this, null, 0);
			}
			else
			{
				if (transferOwnership)
				{
					this.m_Payload = data;
				}
				this.m_Ptr = UploadHandlerRaw.Create(this, (byte*)data.GetUnsafeReadOnlyPtr<byte>(), data.Length);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005298 File Offset: 0x00003498
		public unsafe UploadHandlerRaw(NativeArray<byte>.ReadOnly data)
		{
			bool flag = !data.IsCreated || data.Length == 0;
			if (flag)
			{
				this.m_Ptr = UploadHandlerRaw.Create(this, null, 0);
			}
			else
			{
				bool flag2 = data.Length == 0;
				if (flag2)
				{
					this.m_Ptr = UploadHandlerRaw.Create(this, null, 0);
				}
				else
				{
					this.m_Ptr = UploadHandlerRaw.Create(this, (byte*)data.GetUnsafeReadOnlyPtr<byte>(), data.Length);
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005314 File Offset: 0x00003514
		internal override byte[] GetData()
		{
			bool isCreated = this.m_Payload.IsCreated;
			byte[] array;
			if (isCreated)
			{
				array = this.m_Payload.ToArray();
			}
			else
			{
				array = null;
			}
			return array;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005344 File Offset: 0x00003544
		public override void Dispose()
		{
			bool isCreated = this.m_Payload.IsCreated;
			if (isCreated)
			{
				this.m_Payload.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x04000069 RID: 105
		private NativeArray<byte> m_Payload;
	}
}
