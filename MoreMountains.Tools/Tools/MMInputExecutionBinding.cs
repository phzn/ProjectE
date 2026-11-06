using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class MMInputExecutionBinding
	{
		// Token: 0x0600002B RID: 43 RVA: 0x0000291C File Offset: 0x00000B1C
		public virtual void ProcessInput()
		{
			bool key = Input.GetKey(this.TargetKey);
			bool keyDown = Input.GetKeyDown(this.TargetKey);
			bool keyUp = Input.GetKeyUp(this.TargetKey);
			if (this.OnKey != null && key)
			{
				this.OnKey.Invoke();
			}
			if (this.OnKeyDown != null && keyDown)
			{
				this.OnKeyDown.Invoke();
			}
			if (this.OnKeyUp != null && keyUp)
			{
				this.OnKeyUp.Invoke();
			}
		}

		// Token: 0x04000024 RID: 36
		public KeyCode TargetKey = KeyCode.Space;

		// Token: 0x04000025 RID: 37
		public UnityEvent OnKeyDown;

		// Token: 0x04000026 RID: 38
		public UnityEvent OnKey;

		// Token: 0x04000027 RID: 39
		public UnityEvent OnKeyUp;
	}
}
