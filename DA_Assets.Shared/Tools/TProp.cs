using System;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public struct TProp<T> : IEnableable
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00003222 File Offset: 0x00001422
		public TProp(bool enabled, T v)
		{
			this = default(TProp<T>);
			this._enabled = enabled;
			this._value = v;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003239 File Offset: 0x00001439
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00003241 File Offset: 0x00001441
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000324A File Offset: 0x0000144A
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003252 File Offset: 0x00001452
		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04000032 RID: 50
		[SerializeField]
		private bool _enabled;

		// Token: 0x04000033 RID: 51
		[SerializeField]
		private T _value;
	}
}
