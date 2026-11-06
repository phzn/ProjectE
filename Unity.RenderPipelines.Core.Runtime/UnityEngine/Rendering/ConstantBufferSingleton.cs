using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200003A RID: 58
	internal class ConstantBufferSingleton<CBType> : ConstantBuffer<CBType> where CBType : struct
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000A90C File Offset: 0x00008B0C
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000A92E File Offset: 0x00008B2E
		internal static ConstantBufferSingleton<CBType> instance
		{
			get
			{
				if (ConstantBufferSingleton<CBType>.s_Instance == null)
				{
					ConstantBufferSingleton<CBType>.s_Instance = new ConstantBufferSingleton<CBType>();
					ConstantBuffer.Register(ConstantBufferSingleton<CBType>.s_Instance);
				}
				return ConstantBufferSingleton<CBType>.s_Instance;
			}
			set
			{
				ConstantBufferSingleton<CBType>.s_Instance = value;
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A936 File Offset: 0x00008B36
		public override void Release()
		{
			base.Release();
			ConstantBufferSingleton<CBType>.s_Instance = null;
		}

		// Token: 0x04000141 RID: 321
		private static ConstantBufferSingleton<CBType> s_Instance;
	}
}
