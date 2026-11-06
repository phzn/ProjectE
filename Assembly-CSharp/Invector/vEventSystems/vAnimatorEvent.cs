using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x0200061F RID: 1567
	public class vAnimatorEvent : StateMachineBehaviour
	{
		// Token: 0x06002405 RID: 9221 RVA: 0x000CB07C File Offset: 0x000C927C
		public bool HasEvent(string eventName)
		{
			return this.eventTriggers.Exists((vAnimatorEvent.vAnimatorEventTrigger e) => e.eventName.Equals(eventName));
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000CB0B0 File Offset: 0x000C92B0
		public void RegisterEvents(string eventName, vAnimatorEvent.OnTriggerEvent onTriggerEvent)
		{
			List<vAnimatorEvent.vAnimatorEventTrigger> list = this.eventTriggers.FindAll((vAnimatorEvent.vAnimatorEventTrigger e) => e.eventName.Equals(eventName));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].onTriggerEvent -= onTriggerEvent;
				list[i].onTriggerEvent += onTriggerEvent;
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000CB110 File Offset: 0x000C9310
		public void RemoveEvents(string eventName, vAnimatorEvent.OnTriggerEvent onTriggerEvent)
		{
			List<vAnimatorEvent.vAnimatorEventTrigger> list = this.eventTriggers.FindAll((vAnimatorEvent.vAnimatorEventTrigger e) => e.eventName.Equals(eventName));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].onTriggerEvent -= onTriggerEvent;
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000CB160 File Offset: 0x000C9360
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			for (int i = 0; i < this.eventTriggers.Count; i++)
			{
				if (this.eventTriggers[i].eventTriggerType == vAnimatorEvent.vAnimatorEventTrigger.vAnimatorEventTriggerType.EnterState)
				{
					this.eventTriggers[i].TriggerEvent();
				}
				else if (this.eventTriggers[i].eventTriggerType == vAnimatorEvent.vAnimatorEventTrigger.vAnimatorEventTriggerType.NormalizedTime)
				{
					this.hasNormalizedEvents = true;
					this.eventTriggers[i].Init();
					this.eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
				}
			}
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000CB1F0 File Offset: 0x000C93F0
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if ((!stateInfo.loop && stateInfo.normalizedTime > 1f) || !this.hasNormalizedEvents)
			{
				return;
			}
			for (int i = 0; i < this.eventTriggers.Count; i++)
			{
				if (this.eventTriggers[i].eventTriggerType == vAnimatorEvent.vAnimatorEventTrigger.vAnimatorEventTriggerType.NormalizedTime)
				{
					this.eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
				}
			}
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000CB260 File Offset: 0x000C9460
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			for (int i = 0; i < this.eventTriggers.Count; i++)
			{
				if (this.eventTriggers[i].eventTriggerType == vAnimatorEvent.vAnimatorEventTrigger.vAnimatorEventTriggerType.ExitState)
				{
					this.eventTriggers[i].TriggerEvent();
				}
			}
		}

		// Token: 0x040023F4 RID: 9204
		public List<vAnimatorEvent.vAnimatorEventTrigger> eventTriggers;

		// Token: 0x040023F5 RID: 9205
		protected bool hasNormalizedEvents;

		// Token: 0x02000620 RID: 1568
		[Serializable]
		public class vAnimatorEventTrigger
		{
			// Token: 0x1400003E RID: 62
			// (add) Token: 0x0600240C RID: 9228 RVA: 0x000CB2A8 File Offset: 0x000C94A8
			// (remove) Token: 0x0600240D RID: 9229 RVA: 0x000CB2E0 File Offset: 0x000C94E0
			public event vAnimatorEvent.OnTriggerEvent onTriggerEvent;

			// Token: 0x0600240E RID: 9230 RVA: 0x000CB318 File Offset: 0x000C9518
			public void UpdateEventTrigger(float normalizedTime)
			{
				if (Mathf.Clamp(normalizedTime, 0f, (float)this.loopCount + 1f) >= (float)this.loopCount + this.normalizedTime)
				{
					if (this.onTriggerEvent != null)
					{
						this.onTriggerEvent(this.eventName);
					}
					this.loopCount++;
				}
			}

			// Token: 0x0600240F RID: 9231 RVA: 0x000CB374 File Offset: 0x000C9574
			public void TriggerEvent()
			{
				if (this.onTriggerEvent != null)
				{
					this.onTriggerEvent(this.eventName);
				}
			}

			// Token: 0x06002410 RID: 9232 RVA: 0x000CB38F File Offset: 0x000C958F
			public void Init()
			{
				this.loopCount = 0;
			}

			// Token: 0x040023F6 RID: 9206
			public string eventName = "New Event";

			// Token: 0x040023F7 RID: 9207
			public vAnimatorEvent.vAnimatorEventTrigger.vAnimatorEventTriggerType eventTriggerType;

			// Token: 0x040023F8 RID: 9208
			public float normalizedTime;

			// Token: 0x040023F9 RID: 9209
			private int loopCount;

			// Token: 0x02000621 RID: 1569
			public enum vAnimatorEventTriggerType
			{
				// Token: 0x040023FC RID: 9212
				NormalizedTime,
				// Token: 0x040023FD RID: 9213
				EnterState,
				// Token: 0x040023FE RID: 9214
				ExitState
			}
		}

		// Token: 0x02000622 RID: 1570
		// (Invoke) Token: 0x06002413 RID: 9235
		public delegate void OnTriggerEvent(string eventName);
	}
}
