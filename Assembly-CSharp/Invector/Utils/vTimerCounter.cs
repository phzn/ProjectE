using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.Utils
{
	// Token: 0x02000617 RID: 1559
	public class vTimerCounter : MonoBehaviour
	{
		// Token: 0x060023E1 RID: 9185 RVA: 0x000CA9CD File Offset: 0x000C8BCD
		public void Start()
		{
			if (this.startTimerOnStart)
			{
				this.StartTimer();
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000CA9E0 File Offset: 0x000C8BE0
		public virtual void StartTimer()
		{
			if (this.timerRoutine != null)
			{
				base.StopCoroutine(this.timerRoutine);
			}
			this.currentTime = 0f;
			this.timerResult = 0f;
			this.onTimerUpdated.Invoke(0f);
			this.timerRoutine = base.StartCoroutine(this.TimerRoutiner());
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000CAA39 File Offset: 0x000C8C39
		public void StopTimer()
		{
			this.PauseTimer();
			this.currentTime = 0f;
			this.onStop.Invoke();
			this.timerResult = 0f;
			this.onTimerUpdated.Invoke(0f);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000CAA72 File Offset: 0x000C8C72
		public void PauseTimer()
		{
			if (this.timerRoutine != null)
			{
				base.StopCoroutine(this.timerRoutine);
			}
			this.timerRoutine = null;
			this.onPause.Invoke();
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000CAA9A File Offset: 0x000C8C9A
		private IEnumerator TimerRoutiner()
		{
			this.onStart.Invoke();
			while (this.currentTime < this.targetTime)
			{
				this.currentTime += Time.deltaTime;
				this.timerResult = (this.normalizeResult ? (this.currentTime / this.targetTime) : this.currentTime);
				this.onTimerUpdated.Invoke(this.timerResult);
				yield return null;
			}
			this.timerRoutine = null;
			this.timerResult = (this.normalizeResult ? 1f : this.targetTime);
			this.onTimerUpdated.Invoke(this.timerResult);
			this.onFinish.Invoke();
			yield break;
		}

		// Token: 0x040023D8 RID: 9176
		public float targetTime;

		// Token: 0x040023D9 RID: 9177
		public bool normalizeResult;

		// Token: 0x040023DA RID: 9178
		[SerializeField]
		[vReadOnly(true)]
		protected float timerResult;

		// Token: 0x040023DB RID: 9179
		public bool startTimerOnStart;

		// Token: 0x040023DC RID: 9180
		public UnityEvent onStart;

		// Token: 0x040023DD RID: 9181
		public UnityEvent onPause;

		// Token: 0x040023DE RID: 9182
		public UnityEvent onStop;

		// Token: 0x040023DF RID: 9183
		public UnityEvent onFinish;

		// Token: 0x040023E0 RID: 9184
		public Slider.SliderEvent onTimerUpdated;

		// Token: 0x040023E1 RID: 9185
		protected float currentTime;

		// Token: 0x040023E2 RID: 9186
		protected Coroutine timerRoutine;
	}
}
