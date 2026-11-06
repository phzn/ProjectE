using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000168 RID: 360
	[AddComponentMenu("")]
	public class MMObservableDemoObserver : MonoBehaviour
	{
		// Token: 0x06000888 RID: 2184 RVA: 0x000287FC File Offset: 0x000269FC
		protected virtual void OnPositionChange()
		{
			base.transform.position = base.transform.position.MMSetY(this.TargetSubject.PositionX.Value);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00028829 File Offset: 0x00026A29
		protected virtual void OnEnable()
		{
			MMObservableDemoSubject targetSubject = this.TargetSubject;
			targetSubject.PositionX.OnValueChanged = (Action)Delegate.Combine(targetSubject.PositionX.OnValueChanged, new Action(this.OnPositionChange));
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00028855 File Offset: 0x00026A55
		protected virtual void OnDisable()
		{
			MMObservableDemoSubject targetSubject = this.TargetSubject;
			targetSubject.PositionX.OnValueChanged = (Action)Delegate.Remove(targetSubject.PositionX.OnValueChanged, new Action(this.OnPositionChange));
		}

		// Token: 0x04000A33 RID: 2611
		public MMObservableDemoSubject TargetSubject;
	}
}
