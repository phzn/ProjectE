using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vEventSystems
{
	// Token: 0x02000626 RID: 1574
	[vClassHeader("Animator Event Receiver", true, "icon_v2", false, "")]
	public class vAnimatorEventReceiver : vMonoBehaviour
	{
		// Token: 0x0600241C RID: 9244 RVA: 0x000CB3E4 File Offset: 0x000C95E4
		private void Start()
		{
			this.RegisterEvents();
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000CB3EC File Offset: 0x000C95EC
		private void OnDisable()
		{
			this.RemoveEvents();
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000CB3F4 File Offset: 0x000C95F4
		public void OnEnable()
		{
			if (this.hasAnimator && this.hasValidBehaviours)
			{
				this.RemoveEvents();
				this.RegisterEvents();
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000CB3EC File Offset: 0x000C95EC
		private void OnDestroy()
		{
			this.RemoveEvents();
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000CB414 File Offset: 0x000C9614
		public virtual void RegisterEvents()
		{
			if (this.animatorEvents.Count > 0)
			{
				Animator animator = (this.getAnimatorInParent ? base.GetComponentInParent<Animator>() : base.GetComponent<Animator>());
				if (this.getAnimatorInParent && animator.gameObject == base.gameObject && base.transform.parent)
				{
					animator = base.transform.parent.GetComponentInParent<Animator>();
				}
				if (animator)
				{
					this.hasAnimator = true;
					Invector.vEventSystems.vAnimatorEvent[] behaviours = animator.GetBehaviours<Invector.vEventSystems.vAnimatorEvent>();
					for (int i = 0; i < this.animatorEvents.Count; i++)
					{
						bool flag = false;
						for (int j = 0; j < behaviours.Length; j++)
						{
							if (behaviours[j].HasEvent(this.animatorEvents[i].eventName))
							{
								behaviours[j].RegisterEvents(this.animatorEvents[i].eventName, new Invector.vEventSystems.vAnimatorEvent.OnTriggerEvent(this.animatorEvents[i].OnTriggerEvent));
								if (this.animatorEvents[i].debug)
								{
									Debug.Log(string.Concat(new string[]
									{
										"<color=green>",
										base.gameObject.name,
										" Register event : ",
										this.animatorEvents[i].eventName,
										"</color> in the ",
										animator.gameObject.name
									}), base.gameObject);
								}
								this.hasValidBehaviours = true;
								flag = true;
							}
						}
						if (!flag && this.animatorEvents[i].debug)
						{
							Debug.LogWarning(animator.gameObject.name + " Animator doesn't have Event with name: " + this.animatorEvents[i].eventName, base.gameObject);
						}
					}
					return;
				}
				Debug.LogWarning("Can't Find Animator to register Events in " + base.gameObject.name + (this.getAnimatorInParent ? " Parent" : ""), base.gameObject);
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000CB620 File Offset: 0x000C9820
		public virtual void RemoveEvents()
		{
			if (!this.hasAnimator || !this.hasValidBehaviours)
			{
				return;
			}
			if (this.animatorEvents.Count > 0)
			{
				Animator animator = (this.getAnimatorInParent ? base.GetComponentInParent<Animator>() : base.GetComponent<Animator>());
				if (animator)
				{
					Invector.vEventSystems.vAnimatorEvent[] behaviours = animator.GetBehaviours<Invector.vEventSystems.vAnimatorEvent>();
					for (int i = 0; i < this.animatorEvents.Count; i++)
					{
						for (int j = 0; j < behaviours.Length; j++)
						{
							if (behaviours[j].HasEvent(this.animatorEvents[i].eventName))
							{
								behaviours[j].RemoveEvents(this.animatorEvents[i].eventName, new Invector.vEventSystems.vAnimatorEvent.OnTriggerEvent(this.animatorEvents[i].OnTriggerEvent));
								if (this.animatorEvents[i].debug)
								{
									Debug.Log(string.Concat(new string[]
									{
										"<color=red>",
										base.gameObject.name,
										" Remove event : ",
										this.animatorEvents[i].eventName,
										"</color> Of the ",
										animator.gameObject.name
									}), base.gameObject);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04002402 RID: 9218
		[Tooltip("Check this option if the Animator component is on the parent of this GameObject")]
		public bool getAnimatorInParent;

		// Token: 0x04002403 RID: 9219
		[vHelpBox("Use <b>vAnimatorEvent</b> on a AnimatorState to trigger a Event below", vHelpBoxAttribute.MessageType.Info)]
		public List<vAnimatorEventReceiver.vAnimatorEvent> animatorEvents;

		// Token: 0x04002404 RID: 9220
		private bool hasValidBehaviours;

		// Token: 0x04002405 RID: 9221
		private bool hasAnimator;

		// Token: 0x02000627 RID: 1575
		[Serializable]
		public class vAnimatorEvent
		{
			// Token: 0x06002423 RID: 9251 RVA: 0x000CB768 File Offset: 0x000C9968
			public virtual void OnTriggerEvent(string eventName)
			{
				if (this.debug)
				{
					Debug.Log("<color=green><b>Event " + eventName + " was called</b></color>");
				}
				this.onTriggerEvent.Invoke(eventName);
			}

			// Token: 0x04002406 RID: 9222
			public string eventName;

			// Token: 0x04002407 RID: 9223
			public bool debug;

			// Token: 0x04002408 RID: 9224
			public vAnimatorEventReceiver.vAnimatorEvent.StateEvent onTriggerEvent;

			// Token: 0x02000628 RID: 1576
			[Serializable]
			public class StateEvent : UnityEvent<string>
			{
			}
		}
	}
}
