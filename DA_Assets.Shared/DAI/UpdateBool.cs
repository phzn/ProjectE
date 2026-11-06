using System;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public class UpdateBool
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00006366 File Offset: 0x00004566
		public UpdateBool(bool value, bool temp)
		{
			this._value = value;
			this._temp = temp;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000637C File Offset: 0x0000457C
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00006384 File Offset: 0x00004584
		public bool Value
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000638D File Offset: 0x0000458D
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00006395 File Offset: 0x00004595
		public bool Temp
		{
			get
			{
				return this._temp;
			}
			set
			{
				this._temp = value;
			}
		}

		// Token: 0x04000082 RID: 130
		[SerializeField]
		private bool _value;

		// Token: 0x04000083 RID: 131
		[SerializeField]
		private bool _temp;
	}
}
