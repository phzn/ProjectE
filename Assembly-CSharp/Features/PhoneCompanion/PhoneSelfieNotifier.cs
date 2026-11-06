using System;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E4E RID: 3662
	public class PhoneSelfieNotifier : MonoBehaviour
	{
		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06006D62 RID: 28002 RVA: 0x002BF138 File Offset: 0x002BD338
		// (remove) Token: 0x06006D63 RID: 28003 RVA: 0x002BF16C File Offset: 0x002BD36C
		public static event Action<bool> SelfieStateChanged;

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06006D64 RID: 28004 RVA: 0x002BF19F File Offset: 0x002BD39F
		// (set) Token: 0x06006D65 RID: 28005 RVA: 0x002BF1A6 File Offset: 0x002BD3A6
		public static PhoneSelfieNotifier Active { get; private set; }

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06006D66 RID: 28006 RVA: 0x002BF1AE File Offset: 0x002BD3AE
		public bool IsLogicallyOpen
		{
			get
			{
				return base.gameObject != null && base.gameObject.activeSelf;
			}
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x002BF1CB File Offset: 0x002BD3CB
		private void OnEnable()
		{
			this.ReportOpen();
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x002BF1D3 File Offset: 0x002BD3D3
		private void OnDisable()
		{
			if (base.gameObject != null && base.gameObject.activeSelf)
			{
				return;
			}
			this.ReportClosed();
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x002BF1F7 File Offset: 0x002BD3F7
		private void OnDestroy()
		{
			this.ReportClosed();
		}

		// Token: 0x06006D6A RID: 28010 RVA: 0x002BF1FF File Offset: 0x002BD3FF
		public void ReconcileLogicalState()
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				this.ReportClosed();
				return;
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.ReportOpen();
			}
		}

		// Token: 0x06006D6B RID: 28011 RVA: 0x002BF237 File Offset: 0x002BD437
		private void ReportOpen()
		{
			PhoneSelfieNotifier.Active = this;
			if (this._reportedOpen)
			{
				return;
			}
			this._reportedOpen = true;
			Action<bool> selfieStateChanged = PhoneSelfieNotifier.SelfieStateChanged;
			if (selfieStateChanged == null)
			{
				return;
			}
			selfieStateChanged(true);
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x002BF25F File Offset: 0x002BD45F
		private void ReportClosed()
		{
			if (PhoneSelfieNotifier.Active == this)
			{
				PhoneSelfieNotifier.Active = null;
			}
			if (!this._reportedOpen)
			{
				return;
			}
			this._reportedOpen = false;
			Action<bool> selfieStateChanged = PhoneSelfieNotifier.SelfieStateChanged;
			if (selfieStateChanged == null)
			{
				return;
			}
			selfieStateChanged(false);
		}

		// Token: 0x0400700D RID: 28685
		private bool _reportedOpen;
	}
}
