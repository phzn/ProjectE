using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000014 RID: 20
	public class IntersectionZone : MonoBehaviour
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000744C File Offset: 0x0000564C
		public bool CanEnter(ApproachTrigger approach)
		{
			bool flag;
			switch (this.intersectionType)
			{
			case IntersectionType.TrafficLight:
				flag = this.IsApproachGreen(approach);
				break;
			case IntersectionType.StopSign:
				flag = this.IsApproachFirst(approach) && !this._crossing;
				break;
			case IntersectionType.Yield:
				flag = !this._crossing;
				break;
			default:
				flag = true;
				break;
			}
			return flag;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000074A8 File Offset: 0x000056A8
		public void NotifyCrossing(ApproachTrigger approach)
		{
			if (this.intersectionType != IntersectionType.TrafficLight)
			{
				this._crossing = true;
				if (this._clearingCoroutine != null)
				{
					base.StopCoroutine(this._clearingCoroutine);
				}
				this._clearingCoroutine = base.StartCoroutine(this.ClearAfterDelay());
			}
			if (this.intersectionType == IntersectionType.StopSign && this._waitQueue.Count > 0)
			{
				this._waitQueue.Dequeue();
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000750D File Offset: 0x0000570D
		public void Enqueue(ApproachTrigger approach)
		{
			if (this.intersectionType == IntersectionType.StopSign)
			{
				this._waitQueue.Enqueue(approach);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00007524 File Offset: 0x00005724
		public void Dequeue(ApproachTrigger approach)
		{
			Queue<ApproachTrigger> queue = new Queue<ApproachTrigger>();
			foreach (ApproachTrigger approachTrigger in this._waitQueue)
			{
				if (approachTrigger != approach)
				{
					queue.Enqueue(approachTrigger);
				}
			}
			this._waitQueue = queue;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00007590 File Offset: 0x00005790
		private void Start()
		{
			if (this.intersectionType == IntersectionType.TrafficLight && this.phases.Count > 0)
			{
				this._phaseCoroutine = base.StartCoroutine(this.PhaseCycle());
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000075BC File Offset: 0x000057BC
		private bool IsApproachGreen(ApproachTrigger approach)
		{
			if (this.phases.Count == 0)
			{
				return true;
			}
			IntersectionPhase intersectionPhase = this.phases[this._currentPhase];
			return intersectionPhase.greenApproaches != null && intersectionPhase.greenApproaches.Contains(approach);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007600 File Offset: 0x00005800
		private bool IsApproachFirst(ApproachTrigger approach)
		{
			return this._waitQueue.Count > 0 && this._waitQueue.Peek() == approach;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00007623 File Offset: 0x00005823
		private IEnumerator PhaseCycle()
		{
			for (;;)
			{
				IntersectionPhase phase = this.phases[this._currentPhase];
				if (phase.greenApproaches != null)
				{
					foreach (ApproachTrigger approachTrigger in phase.greenApproaches)
					{
						if (approachTrigger != null)
						{
							approachTrigger.OnPhaseGreen();
						}
					}
				}
				yield return VehicleSimulationPause.WaitForSimulationSeconds(phase.greenDuration);
				if (phase.greenApproaches != null)
				{
					foreach (ApproachTrigger approachTrigger2 in phase.greenApproaches)
					{
						if (approachTrigger2 != null)
						{
							approachTrigger2.OnPhaseYellow();
						}
					}
				}
				yield return VehicleSimulationPause.WaitForSimulationSeconds(phase.yellowDuration);
				this._currentPhase = (this._currentPhase + 1) % this.phases.Count;
				phase = null;
			}
			yield break;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00007632 File Offset: 0x00005832
		private IEnumerator ClearAfterDelay()
		{
			yield return VehicleSimulationPause.WaitForSimulationSeconds(this.clearingTime);
			this._crossing = false;
			yield break;
		}

		// Token: 0x040000C0 RID: 192
		public IntersectionType intersectionType;

		// Token: 0x040000C1 RID: 193
		[Header("Traffic Light — phases cycle in order")]
		public List<IntersectionPhase> phases = new List<IntersectionPhase>();

		// Token: 0x040000C2 RID: 194
		[Header("Stop Sign")]
		[Tooltip("Minimum seconds a vehicle must wait even if the intersection is free.")]
		[Range(0f, 5f)]
		public float stopHoldSeconds = 1.5f;

		// Token: 0x040000C3 RID: 195
		[Tooltip("Seconds a slot is held after a vehicle is granted access (prevents instant double-grant).")]
		[Range(1f, 10f)]
		public float clearingTime = 3f;

		// Token: 0x040000C4 RID: 196
		private int _currentPhase;

		// Token: 0x040000C5 RID: 197
		private bool _crossing;

		// Token: 0x040000C6 RID: 198
		private Queue<ApproachTrigger> _waitQueue = new Queue<ApproachTrigger>();

		// Token: 0x040000C7 RID: 199
		private Coroutine _clearingCoroutine;

		// Token: 0x040000C8 RID: 200
		private Coroutine _phaseCoroutine;
	}
}
