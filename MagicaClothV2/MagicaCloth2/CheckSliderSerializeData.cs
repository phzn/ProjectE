using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class CheckSliderSerializeData
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020C3 File Offset: 0x000002C3
		public CheckSliderSerializeData()
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020CB File Offset: 0x000002CB
		public CheckSliderSerializeData(bool use, float value)
		{
			this.use = use;
			this.value = value;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E1 File Offset: 0x000002E1
		public float GetValue(float unusedValue)
		{
			if (!this.use)
			{
				return unusedValue;
			}
			return this.value;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F3 File Offset: 0x000002F3
		public void SetValue(bool use, float value)
		{
			this.use = use;
			this.value = value;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002103 File Offset: 0x00000303
		public void DataValidate(float min, float max)
		{
			this.value = Mathf.Clamp(this.value, min, max);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002118 File Offset: 0x00000318
		public CheckSliderSerializeData Clone()
		{
			return new CheckSliderSerializeData
			{
				value = this.value,
				use = this.use
			};
		}

		// Token: 0x04000006 RID: 6
		public float value;

		// Token: 0x04000007 RID: 7
		public bool use;
	}
}
