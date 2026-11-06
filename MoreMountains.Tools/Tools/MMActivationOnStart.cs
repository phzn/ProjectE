using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000005 RID: 5
	[AddComponentMenu("More Mountains/Tools/Activation/MMActivationOnStart")]
	public class MMActivationOnStart : MonoBehaviour
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000024D1 File Offset: 0x000006D1
		protected virtual void Awake()
		{
			if (this.Mode != MMActivationOnStart.Modes.Awake)
			{
				return;
			}
			this.SetState();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024E2 File Offset: 0x000006E2
		protected virtual void Start()
		{
			if (this.Mode != MMActivationOnStart.Modes.Start)
			{
				return;
			}
			this.SetState();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024F4 File Offset: 0x000006F4
		protected virtual void SetState()
		{
			foreach (GameObject gameObject in this.TargetObjects)
			{
				gameObject.SetActive(this.StateOnStart);
			}
		}

		// Token: 0x04000012 RID: 18
		public MMActivationOnStart.Modes Mode = MMActivationOnStart.Modes.Start;

		// Token: 0x04000013 RID: 19
		public bool StateOnStart = true;

		// Token: 0x04000014 RID: 20
		public List<GameObject> TargetObjects;

		// Token: 0x02000397 RID: 919
		public enum Modes
		{
			// Token: 0x04001D32 RID: 7474
			Awake,
			// Token: 0x04001D33 RID: 7475
			Start
		}
	}
}
