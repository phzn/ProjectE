using System;
using System.Collections;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003DE RID: 990
	[DisallowMultipleComponent]
	public sealed class PhoneAppAutoHide : MonoBehaviour
	{
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x000A0DB8 File Offset: 0x0009EFB8
		public bool Pending
		{
			get
			{
				return this._pending;
			}
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000A0DC0 File Offset: 0x0009EFC0
		public static void Schedule(GameObject target, float delaySeconds)
		{
			if (target == null)
			{
				return;
			}
			PhoneAppAutoHide phoneAppAutoHide = target.GetComponent<PhoneAppAutoHide>();
			if (phoneAppAutoHide == null)
			{
				phoneAppAutoHide = target.AddComponent<PhoneAppAutoHide>();
			}
			phoneAppAutoHide.ScheduleInternal(delaySeconds);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x000A0DF5 File Offset: 0x0009EFF5
		private void ScheduleInternal(float delaySeconds)
		{
			this._pending = true;
			this._dueAt = Time.realtimeSinceStartup + Mathf.Max(0f, delaySeconds);
			this.RestartRoutineIfPossible();
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x000A0E1B File Offset: 0x0009F01B
		private void OnEnable()
		{
			this.RestartRoutineIfPossible();
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x000A0E23 File Offset: 0x0009F023
		private void OnDisable()
		{
			this._routine = null;
			if (!base.gameObject.activeSelf)
			{
				this._pending = false;
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x000A0E40 File Offset: 0x0009F040
		private void RestartRoutineIfPossible()
		{
			if (!this._pending || !base.isActiveAndEnabled || this._routine != null)
			{
				return;
			}
			this._routine = base.StartCoroutine(this.HideWhenDue());
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x000A0E6D File Offset: 0x0009F06D
		private IEnumerator HideWhenDue()
		{
			while (Time.realtimeSinceStartup < this._dueAt)
			{
				yield return null;
			}
			yield return null;
			this._routine = null;
			if (!this._pending || !base.gameObject.activeInHierarchy)
			{
				yield break;
			}
			this._pending = false;
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04001A82 RID: 6786
		private bool _pending;

		// Token: 0x04001A83 RID: 6787
		private float _dueAt;

		// Token: 0x04001A84 RID: 6788
		private Coroutine _routine;
	}
}
