using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Utils
{
	// Token: 0x02000609 RID: 1545
	[vClassHeader("Events With Delay", true, "icon_v2", false, "")]
	public class vEventWithDelay : vMonoBehaviour
	{
		// Token: 0x0600239C RID: 9116 RVA: 0x000CA115 File Offset: 0x000C8315
		private void OnEnable()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.triggerOnEnable)
			{
				if (this.all)
				{
					this.DoEvents();
					return;
				}
				this.DoEvent(this.eventIndex);
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000CA143 File Offset: 0x000C8343
		protected virtual void Start()
		{
			if (this.triggerOnStart)
			{
				if (this.all)
				{
					this.DoEvents();
					return;
				}
				this.DoEvent(this.eventIndex);
			}
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000CA168 File Offset: 0x000C8368
		private void OnDisable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000CA170 File Offset: 0x000C8370
		public virtual void DoEvents()
		{
			if (!base.enabled)
			{
				return;
			}
			for (int i = 0; i < this.events.Length; i++)
			{
				base.StartCoroutine(this.DoEventWithDelay(this.events[i]));
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000CA1AE File Offset: 0x000C83AE
		public virtual void DoEvent(int index)
		{
			if (!base.enabled)
			{
				return;
			}
			if (index < this.events.Length && this.events.Length != 0)
			{
				base.StartCoroutine(this.DoEventWithDelay(this.events[index]));
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000CA1E4 File Offset: 0x000C83E4
		public virtual void DoEvent(string name)
		{
			if (!base.enabled)
			{
				return;
			}
			vEventWithDelay.vEventWithDelayObject vEventWithDelayObject = Array.Find<vEventWithDelay.vEventWithDelayObject>(this.events, (vEventWithDelay.vEventWithDelayObject e) => e.name.Equals(name));
			if (vEventWithDelayObject != null)
			{
				base.StartCoroutine(this.DoEventWithDelay(vEventWithDelayObject));
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000CA230 File Offset: 0x000C8430
		private IEnumerator DoEventWithDelay(vEventWithDelay.vEventWithDelayObject _event)
		{
			yield return new WaitForSeconds(_event.delay);
			_event.onDoEvent.Invoke();
			yield break;
		}

		// Token: 0x040023A5 RID: 9125
		public bool triggerOnStart;

		// Token: 0x040023A6 RID: 9126
		public bool triggerOnEnable;

		// Token: 0x040023A7 RID: 9127
		[vHideInInspector("triggerOnStart", false)]
		public bool all;

		// Token: 0x040023A8 RID: 9128
		[vHideInInspector("triggerOnStart", false)]
		public int eventIndex;

		// Token: 0x040023A9 RID: 9129
		[SerializeField]
		private vEventWithDelay.vEventWithDelayObject[] events = new vEventWithDelay.vEventWithDelayObject[0];

		// Token: 0x0200060A RID: 1546
		[Serializable]
		public class vEventWithDelayObject
		{
			// Token: 0x040023AA RID: 9130
			public string name = "EventName";

			// Token: 0x040023AB RID: 9131
			public float delay;

			// Token: 0x040023AC RID: 9132
			public UnityEvent onDoEvent;
		}
	}
}
