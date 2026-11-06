using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000DB RID: 219
	[ExecuteAlways]
	public static class MMEventManager
	{
		// Token: 0x060004BC RID: 1212 RVA: 0x00016507 File Offset: 0x00014707
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializeStatics()
		{
			MMEventManager._subscribersList = new Dictionary<Type, List<MMEventListenerBase>>();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00016520 File Offset: 0x00014720
		public static void AddListener<MMEvent>(MMEventListener<MMEvent> listener) where MMEvent : struct
		{
			Type typeFromHandle = typeof(MMEvent);
			if (!MMEventManager._subscribersList.ContainsKey(typeFromHandle))
			{
				MMEventManager._subscribersList[typeFromHandle] = new List<MMEventListenerBase>();
			}
			if (!MMEventManager.SubscriptionExists(typeFromHandle, listener))
			{
				MMEventManager._subscribersList[typeFromHandle].Add(listener);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00016570 File Offset: 0x00014770
		public static void RemoveListener<MMEvent>(MMEventListener<MMEvent> listener) where MMEvent : struct
		{
			Type typeFromHandle = typeof(MMEvent);
			if (!MMEventManager._subscribersList.ContainsKey(typeFromHandle))
			{
				return;
			}
			List<MMEventListenerBase> list = MMEventManager._subscribersList[typeFromHandle];
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == listener)
				{
					list.Remove(list[i]);
					if (list.Count == 0)
					{
						MMEventManager._subscribersList.Remove(typeFromHandle);
					}
					return;
				}
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000165E4 File Offset: 0x000147E4
		public static void TriggerEvent<MMEvent>(MMEvent newEvent) where MMEvent : struct
		{
			List<MMEventListenerBase> list;
			if (!MMEventManager._subscribersList.TryGetValue(typeof(MMEvent), out list))
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				(list[i] as MMEventListener<MMEvent>).OnMMEvent(newEvent);
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00016630 File Offset: 0x00014830
		private static bool SubscriptionExists(Type type, MMEventListenerBase receiver)
		{
			List<MMEventListenerBase> list;
			if (!MMEventManager._subscribersList.TryGetValue(type, out list))
			{
				return false;
			}
			bool flag = false;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == receiver)
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x04000635 RID: 1589
		private static Dictionary<Type, List<MMEventListenerBase>> _subscribersList = new Dictionary<Type, List<MMEventListenerBase>>();
	}
}
