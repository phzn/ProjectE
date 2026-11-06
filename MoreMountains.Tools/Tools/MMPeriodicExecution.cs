using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000010 RID: 16
	public class MMPeriodicExecution : MonoBehaviour
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002AB3 File Offset: 0x00000CB3
		protected virtual void Start()
		{
			this.DetermineNewInterval();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002ABB File Offset: 0x00000CBB
		protected virtual void Update()
		{
			if (Time.time - this._lastUpdateAt > this._currentInterval)
			{
				UnityEvent onRandomInterval = this.OnRandomInterval;
				if (onRandomInterval != null)
				{
					onRandomInterval.Invoke();
				}
				this._lastUpdateAt = Time.time;
				this.DetermineNewInterval();
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002AF3 File Offset: 0x00000CF3
		protected virtual void DetermineNewInterval()
		{
			this._currentInterval = Random.Range(this.RandomIntervalDuration.x, this.RandomIntervalDuration.y);
		}

		// Token: 0x04000032 RID: 50
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RandomIntervalDuration = new Vector2(1f, 3f);

		// Token: 0x04000033 RID: 51
		public UnityEvent OnRandomInterval;

		// Token: 0x04000034 RID: 52
		protected float _lastUpdateAt;

		// Token: 0x04000035 RID: 53
		protected float _currentInterval;
	}
}
