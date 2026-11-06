using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200043D RID: 1085
	public class vCheckOnEnterCoverSide : MonoBehaviour
	{
		// Token: 0x06001B4A RID: 6986 RVA: 0x000AC170 File Offset: 0x000AA370
		private void Start()
		{
			vCoverPoint[] componentsInChildren = base.transform.parent.GetComponentsInChildren<vCoverPoint>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].onEnterCover.AddListener(new UnityAction<vCoverPoint, GameObject>(this.OnEnterCover));
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000AC1B8 File Offset: 0x000AA3B8
		private void OnEnterCover(vCoverPoint coverPoint, GameObject visitor)
		{
			this.onEnter.Invoke();
			if (Quaternion.LookRotation(base.transform.forward).eulerAngles.AngleFormOtherEuler(coverPoint.transform.eulerAngles).y.IsInSideRange(-this.angleMargin, this.angleMargin))
			{
				this.onEnterForward.Invoke();
				return;
			}
			if (Quaternion.LookRotation(-base.transform.forward).eulerAngles.AngleFormOtherEuler(coverPoint.transform.eulerAngles).y.IsInSideRange(-this.angleMargin, this.angleMargin))
			{
				this.onEnterBackward.Invoke();
				return;
			}
			if (Quaternion.LookRotation(base.transform.right).eulerAngles.AngleFormOtherEuler(coverPoint.transform.eulerAngles).y.IsInSideRange(-this.angleMargin, this.angleMargin))
			{
				this.onEnterRight.Invoke();
				return;
			}
			if (Quaternion.LookRotation(-base.transform.right).eulerAngles.AngleFormOtherEuler(coverPoint.transform.eulerAngles).y.IsInSideRange(-this.angleMargin, this.angleMargin))
			{
				this.onEnterLeft.Invoke();
				return;
			}
		}

		// Token: 0x04001C70 RID: 7280
		public float angleMargin = 15f;

		// Token: 0x04001C71 RID: 7281
		public UnityEvent onEnter;

		// Token: 0x04001C72 RID: 7282
		[vSeparator("Events by Transform side", "")]
		public UnityEvent onEnterForward;

		// Token: 0x04001C73 RID: 7283
		public UnityEvent onEnterBackward;

		// Token: 0x04001C74 RID: 7284
		public UnityEvent onEnterRight;

		// Token: 0x04001C75 RID: 7285
		public UnityEvent onEnterLeft;
	}
}
