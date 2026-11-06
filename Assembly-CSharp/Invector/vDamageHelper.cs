using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000482 RID: 1154
	public static class vDamageHelper
	{
		// Token: 0x06001C5A RID: 7258 RVA: 0x000B06F4 File Offset: 0x000AE8F4
		public static void ApplyDamage(this GameObject receiver, vDamage damage)
		{
			vIDamageReceiver[] components = receiver.GetComponents<vIDamageReceiver>();
			if (components != null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					components[i].TakeDamage(damage);
				}
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000B0722 File Offset: 0x000AE922
		public static bool CanReceiveDamage(this GameObject receiver)
		{
			return receiver.GetComponent<vIDamageReceiver>() != null;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000B0730 File Offset: 0x000AE930
		public static float HitAngle(this Transform transform, Vector3 hitpoint, bool normalized = true)
		{
			Vector3 vector = transform.InverseTransformPoint(hitpoint);
			int num = (int)(Mathf.Atan2(vector.x, vector.z) * 57.29578f);
			if (!normalized)
			{
				return (float)num;
			}
			if (num <= 45 && num >= -45)
			{
				num = 0;
			}
			else if (num > 45 && num < 135)
			{
				num = 90;
			}
			else if (num >= 135 || num <= -135)
			{
				num = 180;
			}
			else if (num < -45 && num > -135)
			{
				num = -90;
			}
			return (float)num;
		}
	}
}
