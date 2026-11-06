using System;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004C8 RID: 1224
	[vClassHeader("Effect Receiver", "Use with the EffectSender component to trigger a Effect.")]
	public class vEffectReceiver : vMonoBehaviour
	{
		// Token: 0x06001D99 RID: 7577 RVA: 0x000B5AE0 File Offset: 0x000B3CE0
		public virtual void OnReceiveEffect<T>(T effect) where T : vIEffect
		{
			this.onReceiveEffect.Invoke(effect);
			for (int i = 0; i < this.effectsByName.Length; i++)
			{
				if (this.effectsByName[i].effectName.Equals(effect.EffectName))
				{
					this.effectsByName[i].onReceiveEffect.Invoke(effect);
				}
			}
		}

		// Token: 0x04001EBE RID: 7870
		public vEffectReceiver.vEffectEvent onReceiveEffect;

		// Token: 0x04001EBF RID: 7871
		public vEffectReceiver.vEffectByName[] effectsByName;

		// Token: 0x020004C9 RID: 1225
		[Serializable]
		public class vEffectByName
		{
			// Token: 0x04001EC0 RID: 7872
			public string effectName;

			// Token: 0x04001EC1 RID: 7873
			public vEffectReceiver.vEffectEvent onReceiveEffect;
		}

		// Token: 0x020004CA RID: 1226
		[Serializable]
		public class vEffectEvent : UnityEvent<vIEffect>
		{
		}
	}
}
