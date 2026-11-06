using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Throw
{
	// Token: 0x020004EB RID: 1259
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rigidbody))]
	[vClassHeader("Throwable Object", true, "icon_v2", false, "", openClose = false)]
	public class vThrowableObject : vMonoBehaviour
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000B78F0 File Offset: 0x000B5AF0
		public virtual Rigidbody selfRigidbody
		{
			get
			{
				if (this._rigidbody == null)
				{
					Rigidbody rigidbody;
					if (base.TryGetComponent<Rigidbody>(out rigidbody))
					{
						this._rigidbody = rigidbody;
					}
					else
					{
						this._rigidbody = base.gameObject.AddComponent<Rigidbody>();
					}
				}
				return this._rigidbody;
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000B7935 File Offset: 0x000B5B35
		public static implicit operator Rigidbody(vThrowableObject throwable)
		{
			return throwable.selfRigidbody;
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x000B793D File Offset: 0x000B5B3D
		// (set) Token: 0x06001E6A RID: 7786 RVA: 0x000B794A File Offset: 0x000B5B4A
		public virtual bool isKinematic
		{
			get
			{
				return this.selfRigidbody.isKinematic;
			}
			set
			{
				this.selfRigidbody.isKinematic = value;
			}
		}

		// Token: 0x04001F41 RID: 8001
		protected Rigidbody _rigidbody;

		// Token: 0x04001F42 RID: 8002
		public vThrowSettings throwSettings;

		// Token: 0x04001F43 RID: 8003
		public vThrowVisualSettings throwVisualSettings;

		// Token: 0x04001F44 RID: 8004
		public float indicatorRangeMultiplier = 1f;

		// Token: 0x04001F45 RID: 8005
		public vThrowableObject.ThrowSenderEvent onThrow;

		// Token: 0x020004EC RID: 1260
		[Serializable]
		public class ThrowSenderEvent : UnityEvent<Transform>
		{
		}
	}
}
