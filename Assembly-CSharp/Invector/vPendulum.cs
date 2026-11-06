using System;
using System.Collections;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000486 RID: 1158
	public class vPendulum : MonoBehaviour
	{
		// Token: 0x06001C70 RID: 7280 RVA: 0x000B0889 File Offset: 0x000AEA89
		private IEnumerator Start()
		{
			this.qStart = this.PendulumRotation(this.angle);
			this.qEnd = this.PendulumRotation(-this.angle);
			yield return new WaitForSeconds(this.startDelay);
			this.work = true;
			yield break;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000B0898 File Offset: 0x000AEA98
		private void FixedUpdate()
		{
			if (this.work)
			{
				if (!this.working)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.qEnd, this.speed);
					if (Vector3.Distance(base.transform.rotation.eulerAngles, this.qEnd.eulerAngles) < 0.1f)
					{
						this.working = true;
						return;
					}
				}
				else
				{
					this.startTime += Time.deltaTime;
					base.transform.rotation = Quaternion.Lerp(this.qStart, this.qEnd, (Mathf.Sin(this.startTime * this.speed + 1.5707964f) + 1f) / 2f);
				}
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000B0963 File Offset: 0x000AEB63
		private void resetTimer()
		{
			this.startTime = 0f;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000B0970 File Offset: 0x000AEB70
		private Quaternion PendulumRotation(float _angle)
		{
			Quaternion rotation = base.transform.rotation;
			float num = rotation.eulerAngles.z + _angle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, num);
			return rotation;
		}

		// Token: 0x04001DA7 RID: 7591
		[Range(0f, 360f)]
		public float angle = 90f;

		// Token: 0x04001DA8 RID: 7592
		[Range(0f, 4f)]
		public float speed = 1.5f;

		// Token: 0x04001DA9 RID: 7593
		public float startDelay;

		// Token: 0x04001DAA RID: 7594
		private Quaternion qStart;

		// Token: 0x04001DAB RID: 7595
		private Quaternion qEnd;

		// Token: 0x04001DAC RID: 7596
		private float startTime;

		// Token: 0x04001DAD RID: 7597
		private bool work;

		// Token: 0x04001DAE RID: 7598
		private bool working;
	}
}
