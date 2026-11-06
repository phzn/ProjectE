using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000169 RID: 361
	[AddComponentMenu("")]
	public class MMObservableDemoObserverAutoSleep : MonoBehaviour
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x00028889 File Offset: 0x00026A89
		protected virtual void OnSpeedChange()
		{
			base.transform.position = base.transform.position.MMSetY(this.TargetSubject.PositionX.Value);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x000288B6 File Offset: 0x00026AB6
		protected virtual void Awake()
		{
			MMObservableDemoSubject targetSubject = this.TargetSubject;
			targetSubject.PositionX.OnValueChanged = (Action)Delegate.Combine(targetSubject.PositionX.OnValueChanged, new Action(this.OnSpeedChange));
			base.enabled = false;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000288E9 File Offset: 0x00026AE9
		protected virtual void OnDestroy()
		{
			MMObservableDemoSubject targetSubject = this.TargetSubject;
			targetSubject.PositionX.OnValueChanged = (Action)Delegate.Remove(targetSubject.PositionX.OnValueChanged, new Action(this.OnSpeedChange));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00028915 File Offset: 0x00026B15
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00028917 File Offset: 0x00026B17
		protected virtual void OnDisable()
		{
		}

		// Token: 0x04000A34 RID: 2612
		public MMObservableDemoSubject TargetSubject;
	}
}
