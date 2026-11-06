using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x0200069F RID: 1695
	public class vAnimatorMoveSender : MonoBehaviour
	{
		// Token: 0x06002844 RID: 10308 RVA: 0x000E5708 File Offset: 0x000E3908
		protected virtual void Awake()
		{
			base.hideFlags = HideFlags.HideInInspector;
			vIAnimatorMoveReceiver[] components = base.GetComponents<vIAnimatorMoveReceiver>();
			for (int i = 0; i < components.Length; i++)
			{
				vIAnimatorMoveReceiver receiver = components[i];
				this.animatorMoveEvent = (Action)Delegate.Combine(this.animatorMoveEvent, new Action(delegate
				{
					if (receiver.enabled)
					{
						receiver.OnAnimatorMoveEvent();
					}
				}));
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000E5762 File Offset: 0x000E3962
		protected virtual void OnAnimatorMove()
		{
			Action action = this.animatorMoveEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040028AE RID: 10414
		public Action animatorMoveEvent;
	}
}
