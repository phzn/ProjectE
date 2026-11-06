using System;
using System.Diagnostics;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C5 RID: 1221
	[DebuggerTypeProxy(typeof(BlobStructProxy<>))]
	[DebuggerDisplay("{Description,nq}")]
	internal struct BlobStruct<T> where T : struct
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0006654D File Offset: 0x0006474D
		public string Description
		{
			get
			{
				return UnsafeUtility.AsRef<T>(this.m_BasePtr).ToString();
			}
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00066565 File Offset: 0x00064765
		public override string ToString()
		{
			return this.Description;
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x0006656D File Offset: 0x0006476D
		public BlobStruct(IntPtr basePtr)
		{
			this.m_BasePtr = basePtr.ToPointer();
			this.m_Members = null;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x00066584 File Offset: 0x00064784
		private unsafe void Init()
		{
			if (this.m_Members != null)
			{
				return;
			}
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
			this.m_Members = new BlobMember[fields.Length];
			for (int i = 0; i < this.m_Members.Length; i++)
			{
				byte* ptr = (byte*)this.m_BasePtr + UnsafeUtility.GetFieldOffset(fields[i]);
				this.m_Members[i] = new BlobMember
				{
					Key = fields[i].Name,
					Value = BlobProxy.UnpackValue((void*)ptr, fields[i].FieldType)
				};
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x00066616 File Offset: 0x00064816
		public BlobMember[] Members
		{
			get
			{
				this.Init();
				return this.m_Members;
			}
		}

		// Token: 0x04000A8C RID: 2700
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private unsafe readonly void* m_BasePtr;

		// Token: 0x04000A8D RID: 2701
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BlobMember[] m_Members;
	}
}
