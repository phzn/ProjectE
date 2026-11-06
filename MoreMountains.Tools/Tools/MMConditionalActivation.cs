using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200000A RID: 10
	[AddComponentMenu("More Mountains/Tools/Activation/MMConditionalActivation")]
	public class MMConditionalActivation : MonoBehaviour
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000289C File Offset: 0x00000A9C
		protected virtual void Update()
		{
			if (this._enabled)
			{
				return;
			}
			bool flag = true;
			MonoBehaviour[] array = this.AfterTheseAreAllDisabled;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].isActiveAndEnabled)
				{
					flag = false;
				}
			}
			if (flag)
			{
				array = this.EnableThese;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = true;
				}
				this._enabled = true;
			}
		}

		// Token: 0x04000021 RID: 33
		public MonoBehaviour[] EnableThese;

		// Token: 0x04000022 RID: 34
		public MonoBehaviour[] AfterTheseAreAllDisabled;

		// Token: 0x04000023 RID: 35
		protected bool _enabled;
	}
}
