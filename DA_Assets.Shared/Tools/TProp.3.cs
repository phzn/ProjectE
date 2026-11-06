using System;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public struct TProp<T1, T2, T3> : IEnableable
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000032AC File Offset: 0x000014AC
		public TProp(bool enabled, T1 v1, T2 v2, T3 v3)
		{
			this = default(TProp<T1, T2, T3>);
			this._enabled = enabled;
			this._value1 = v1;
			this._value2 = v2;
			this._value3 = v3;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000032D2 File Offset: 0x000014D2
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000032DA File Offset: 0x000014DA
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000032E3 File Offset: 0x000014E3
		// (set) Token: 0x06000061 RID: 97 RVA: 0x000032EB File Offset: 0x000014EB
		public T1 Value1
		{
			get
			{
				return this._value1;
			}
			set
			{
				this._value1 = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000032F4 File Offset: 0x000014F4
		// (set) Token: 0x06000063 RID: 99 RVA: 0x000032FC File Offset: 0x000014FC
		public T2 Value2
		{
			get
			{
				return this._value2;
			}
			set
			{
				this._value2 = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003305 File Offset: 0x00001505
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000330D File Offset: 0x0000150D
		public T3 Value3
		{
			get
			{
				return this._value3;
			}
			set
			{
				this._value3 = value;
			}
		}

		// Token: 0x04000037 RID: 55
		[SerializeField]
		private bool _enabled;

		// Token: 0x04000038 RID: 56
		[SerializeField]
		private T1 _value1;

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private T2 _value2;

		// Token: 0x0400003A RID: 58
		[SerializeField]
		private T3 _value3;
	}
}
