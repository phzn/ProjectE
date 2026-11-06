using System;
using UnityEngine.Events;

namespace Invector.Throw
{
	// Token: 0x02000505 RID: 1285
	public abstract class vThrowVisualControlBase : vMonoBehaviour
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x000BA328 File Offset: 0x000B8528
		protected virtual void Awake()
		{
			this.tm = base.GetComponentInParent<vThrowManagerBase>();
			if (this.tm)
			{
				this.OnInit(this.tm);
				this.tm.onChangeVisualSettings.AddListener(new UnityAction<vThrowVisualSettings>(this.OnChangeVisual));
			}
		}

		// Token: 0x06001F42 RID: 8002
		protected abstract void OnInit(vThrowManagerBase manager);

		// Token: 0x06001F43 RID: 8003
		public abstract void OnChangeVisual(vThrowVisualSettings settings);

		// Token: 0x04001FDC RID: 8156
		protected vThrowManagerBase tm;
	}
}
