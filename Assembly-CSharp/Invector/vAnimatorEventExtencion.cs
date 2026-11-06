using System;
using Invector.vEventSystems;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200045F RID: 1119
	public static class vAnimatorEventExtencion
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x000AF218 File Offset: 0x000AD418
		public static void RegisterEvent(this Animator animator, string eventName, vAnimatorEvent.OnTriggerEvent onTriggerEventAction)
		{
			if (animator)
			{
				vAnimatorEvent[] behaviours = animator.GetBehaviours<vAnimatorEvent>();
				for (int i = 0; i < behaviours.Length; i++)
				{
					if (behaviours[i].HasEvent(eventName))
					{
						behaviours[i].RegisterEvents(eventName, onTriggerEventAction);
					}
				}
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000AF258 File Offset: 0x000AD458
		public static void RemoveEvent(this Animator animator, string eventName, vAnimatorEvent.OnTriggerEvent onTriggerEventAction)
		{
			if (animator)
			{
				vAnimatorEvent[] behaviours = animator.GetBehaviours<vAnimatorEvent>();
				for (int i = 0; i < behaviours.Length; i++)
				{
					if (behaviours[i].HasEvent(eventName))
					{
						behaviours[i].RemoveEvents(eventName, onTriggerEventAction);
					}
				}
			}
		}
	}
}
