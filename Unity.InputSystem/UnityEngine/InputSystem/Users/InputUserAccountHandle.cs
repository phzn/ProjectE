using System;

namespace UnityEngine.InputSystem.Users
{
	// Token: 0x02000081 RID: 129
	public struct InputUserAccountHandle : IEquatable<InputUserAccountHandle>
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0003B007 File Offset: 0x00039207
		public string apiName
		{
			get
			{
				return this.m_ApiName;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0003B00F File Offset: 0x0003920F
		public ulong handle
		{
			get
			{
				return this.m_Handle;
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0003B017 File Offset: 0x00039217
		public InputUserAccountHandle(string apiName, ulong handle)
		{
			if (string.IsNullOrEmpty(apiName))
			{
				throw new ArgumentNullException("apiName");
			}
			this.m_ApiName = apiName;
			this.m_Handle = handle;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0003B03A File Offset: 0x0003923A
		public override string ToString()
		{
			if (this.m_ApiName == null)
			{
				return base.ToString();
			}
			return string.Format("{0}({1})", this.m_ApiName, this.m_Handle);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0003B070 File Offset: 0x00039270
		public bool Equals(InputUserAccountHandle other)
		{
			return string.Equals(this.apiName, other.apiName) && object.Equals(this.handle, other.handle);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003B0A4 File Offset: 0x000392A4
		public override bool Equals(object obj)
		{
			return obj != null && obj is InputUserAccountHandle && this.Equals((InputUserAccountHandle)obj);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003B0C1 File Offset: 0x000392C1
		public static bool operator ==(InputUserAccountHandle left, InputUserAccountHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003B0CB File Offset: 0x000392CB
		public static bool operator !=(InputUserAccountHandle left, InputUserAccountHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003B0D8 File Offset: 0x000392D8
		public override int GetHashCode()
		{
			return (((this.apiName != null) ? this.apiName.GetHashCode() : 0) * 397) ^ this.handle.GetHashCode();
		}

		// Token: 0x040003C0 RID: 960
		private string m_ApiName;

		// Token: 0x040003C1 RID: 961
		private ulong m_Handle;
	}
}
