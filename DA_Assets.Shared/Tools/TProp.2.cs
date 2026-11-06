using System;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public struct TProp<T1, T2> : IEnableable
	{
		// Token: 0x06000056 RID: 86 RVA: 0x0000325B File Offset: 0x0000145B
		public TProp(bool enabled, T1 v1, T2 v2)
		{
			this = default(TProp<T1, T2>);
			this._enabled = enabled;
			this._value1 = v1;
			this._value2 = v2;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003279 File Offset: 0x00001479
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003281 File Offset: 0x00001481
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000328A File Offset: 0x0000148A
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003292 File Offset: 0x00001492
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000329B File Offset: 0x0000149B
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000032A3 File Offset: 0x000014A3
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

		// Token: 0x04000034 RID: 52
		[SerializeField]
		private bool _enabled;

		// Token: 0x04000035 RID: 53
		[SerializeField]
		private T1 _value1;

		// Token: 0x04000036 RID: 54
		[SerializeField]
		private T2 _value2;
	}
}
