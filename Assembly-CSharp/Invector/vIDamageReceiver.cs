using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000481 RID: 1153
	public interface vIDamageReceiver
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001C55 RID: 7253
		OnReceiveDamage onStartReceiveDamage { get; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001C56 RID: 7254
		OnReceiveDamage onReceiveDamage { get; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001C57 RID: 7255
		Transform transform { get; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001C58 RID: 7256
		GameObject gameObject { get; }

		// Token: 0x06001C59 RID: 7257
		void TakeDamage(vDamage damage);
	}
}
