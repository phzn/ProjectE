using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.UIElements
{
	// Token: 0x020003F9 RID: 1017
	internal static class EventInterestReflectionUtils
	{
		// Token: 0x060020DB RID: 8411 RVA: 0x0007C550 File Offset: 0x0007A750
		internal static void GetDefaultEventInterests(Type elementType, out int defaultActionCategories, out int defaultActionAtTargetCategories)
		{
			EventInterestReflectionUtils.DefaultEventInterests defaultEventInterests;
			bool flag = !EventInterestReflectionUtils.s_DefaultEventInterests.TryGetValue(elementType, out defaultEventInterests);
			if (flag)
			{
				Type baseType = elementType.BaseType;
				bool flag2 = baseType != null;
				if (flag2)
				{
					EventInterestReflectionUtils.GetDefaultEventInterests(baseType, out defaultEventInterests.DefaultActionCategories, out defaultEventInterests.DefaultActionAtTargetCategories);
				}
				defaultEventInterests.DefaultActionCategories |= EventInterestReflectionUtils.ComputeDefaultEventInterests(elementType, "ExecuteDefaultAction") | EventInterestReflectionUtils.ComputeDefaultEventInterests(elementType, "ExecuteDefaultActionDisabled");
				defaultEventInterests.DefaultActionAtTargetCategories |= EventInterestReflectionUtils.ComputeDefaultEventInterests(elementType, "ExecuteDefaultActionAtTarget") | EventInterestReflectionUtils.ComputeDefaultEventInterests(elementType, "ExecuteDefaultActionDisabledAtTarget");
				EventInterestReflectionUtils.s_DefaultEventInterests.Add(elementType, defaultEventInterests);
			}
			defaultActionCategories = defaultEventInterests.DefaultActionCategories;
			defaultActionAtTargetCategories = defaultEventInterests.DefaultActionAtTargetCategories;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x0007C600 File Offset: 0x0007A800
		private static int ComputeDefaultEventInterests(Type elementType, string methodName)
		{
			MethodInfo method = elementType.GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag = method == null;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				bool flag2 = false;
				int num2 = 0;
				object[] customAttributes = method.GetCustomAttributes(typeof(EventInterestAttribute), false);
				foreach (EventInterestAttribute eventInterestAttribute in customAttributes)
				{
					flag2 = true;
					bool flag3 = eventInterestAttribute.eventTypes != null;
					if (flag3)
					{
						foreach (Type type in eventInterestAttribute.eventTypes)
						{
							num2 |= 1 << (int)EventInterestReflectionUtils.GetEventCategory(type);
						}
					}
					num2 |= (int)eventInterestAttribute.categoryFlags;
				}
				num = (flag2 ? num2 : (-1));
			}
			return num;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x0007C6C8 File Offset: 0x0007A8C8
		internal static EventCategory GetEventCategory(Type eventType)
		{
			EventCategory category;
			bool flag = EventInterestReflectionUtils.s_EventCategories.TryGetValue(eventType, out category);
			EventCategory eventCategory;
			if (flag)
			{
				eventCategory = category;
			}
			else
			{
				object[] customAttributes = eventType.GetCustomAttributes(typeof(EventCategoryAttribute), true);
				object[] array = customAttributes;
				int num = 0;
				if (num >= array.Length)
				{
					throw new ArgumentOutOfRangeException("eventType", "Type must derive from EventBase<T>");
				}
				EventCategoryAttribute eventCategoryAttribute = (EventCategoryAttribute)array[num];
				category = eventCategoryAttribute.category;
				EventInterestReflectionUtils.s_EventCategories.Add(eventType, category);
				eventCategory = category;
			}
			return eventCategory;
		}

		// Token: 0x04000DC7 RID: 3527
		private static readonly Dictionary<Type, EventInterestReflectionUtils.DefaultEventInterests> s_DefaultEventInterests = new Dictionary<Type, EventInterestReflectionUtils.DefaultEventInterests>();

		// Token: 0x04000DC8 RID: 3528
		private static readonly Dictionary<Type, EventCategory> s_EventCategories = new Dictionary<Type, EventCategory>();

		// Token: 0x020003FA RID: 1018
		private struct DefaultEventInterests
		{
			// Token: 0x04000DC9 RID: 3529
			public int DefaultActionCategories;

			// Token: 0x04000DCA RID: 3530
			public int DefaultActionAtTargetCategories;
		}
	}
}
