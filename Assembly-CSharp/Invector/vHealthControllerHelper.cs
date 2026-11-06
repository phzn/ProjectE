using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000485 RID: 1157
	public static class vHealthControllerHelper
	{
		// Token: 0x06001C68 RID: 7272 RVA: 0x000B07B6 File Offset: 0x000AE9B6
		private static vIHealthController GetHealthController(this GameObject gameObject)
		{
			return gameObject.GetComponent<vIHealthController>();
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000B07C0 File Offset: 0x000AE9C0
		public static void AddHealth(this GameObject receiver, int health)
		{
			vIHealthController healthController = receiver.GetHealthController();
			if (healthController != null)
			{
				healthController.AddHealth(health);
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000B07E0 File Offset: 0x000AE9E0
		public static void ChangeHealth(this GameObject receiver, int health)
		{
			vIHealthController healthController = receiver.GetHealthController();
			if (healthController != null)
			{
				healthController.ChangeHealth(health);
			}
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000B0800 File Offset: 0x000AEA00
		public static void ChangeMaxHealth(this GameObject receiver, int health)
		{
			vIHealthController healthController = receiver.GetHealthController();
			if (healthController != null)
			{
				healthController.ChangeMaxHealth(health);
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000B081E File Offset: 0x000AEA1E
		public static bool HasHealth(this GameObject gameObject)
		{
			return gameObject.GetHealthController() != null;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000B082C File Offset: 0x000AEA2C
		public static bool IsDead(this GameObject gameObject)
		{
			vIHealthController healthController = gameObject.GetHealthController();
			return healthController == null || healthController.isDead;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000B084C File Offset: 0x000AEA4C
		public static void ResetHealth(this GameObject receiver, float health)
		{
			vIHealthController healthController = receiver.GetHealthController();
			if (healthController != null)
			{
				healthController.ResetHealth(health);
			}
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000B086C File Offset: 0x000AEA6C
		public static void ResetHealth(this GameObject receiver)
		{
			vIHealthController healthController = receiver.GetHealthController();
			if (healthController != null)
			{
				healthController.ResetHealth();
			}
		}
	}
}
