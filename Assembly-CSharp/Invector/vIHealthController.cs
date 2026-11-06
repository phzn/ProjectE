using System;

namespace Invector
{
	// Token: 0x02000484 RID: 1156
	public interface vIHealthController : vIDamageReceiver
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001C5E RID: 7262
		OnDead onDead { get; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001C5F RID: 7263
		float currentHealth { get; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001C60 RID: 7264
		int MaxHealth { get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001C61 RID: 7265
		// (set) Token: 0x06001C62 RID: 7266
		bool isDead { get; set; }

		// Token: 0x06001C63 RID: 7267
		void AddHealth(int value);

		// Token: 0x06001C64 RID: 7268
		void ChangeHealth(int value);

		// Token: 0x06001C65 RID: 7269
		void ChangeMaxHealth(int value);

		// Token: 0x06001C66 RID: 7270
		void ResetHealth(float health);

		// Token: 0x06001C67 RID: 7271
		void ResetHealth();
	}
}
