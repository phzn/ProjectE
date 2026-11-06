using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025C RID: 604
	internal static class GroupBoxUtility
	{
		// Token: 0x0600114E RID: 4430 RVA: 0x0003ED58 File Offset: 0x0003CF58
		public static void RegisterGroupBoxOption<T>(this T option) where T : VisualElement, IGroupBoxOption
		{
			VisualElement visualElement = option;
			IGroupBox groupBox = null;
			for (VisualElement visualElement2 = visualElement.hierarchy.parent; visualElement2 != null; visualElement2 = visualElement2.hierarchy.parent)
			{
				IGroupBox groupBox2 = visualElement2 as IGroupBox;
				bool flag = groupBox2 != null;
				if (flag)
				{
					groupBox = groupBox2;
					break;
				}
			}
			IGroupBox groupBox3 = groupBox ?? visualElement.elementPanel;
			IGroupManager groupManager = GroupBoxUtility.FindOrCreateGroupManager(groupBox3);
			groupManager.RegisterOption(option);
			GroupBoxUtility.s_GroupOptionManagerCache[option] = groupManager;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0003EDEC File Offset: 0x0003CFEC
		public static void UnregisterGroupBoxOption<T>(this T option) where T : VisualElement, IGroupBoxOption
		{
			bool flag = !GroupBoxUtility.s_GroupOptionManagerCache.ContainsKey(option);
			if (!flag)
			{
				GroupBoxUtility.s_GroupOptionManagerCache[option].UnregisterOption(option);
				GroupBoxUtility.s_GroupOptionManagerCache.Remove(option);
			}
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0003EE40 File Offset: 0x0003D040
		public static void OnOptionSelected<T>(this T selectedOption) where T : VisualElement, IGroupBoxOption
		{
			bool flag = !GroupBoxUtility.s_GroupOptionManagerCache.ContainsKey(selectedOption);
			if (!flag)
			{
				GroupBoxUtility.s_GroupOptionManagerCache[selectedOption].OnOptionSelectionChanged(selectedOption);
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0003EE84 File Offset: 0x0003D084
		public static IGroupBoxOption GetSelectedOption(this IGroupBox groupBox)
		{
			return (!GroupBoxUtility.s_GroupManagers.ContainsKey(groupBox)) ? null : GroupBoxUtility.s_GroupManagers[groupBox].GetSelectedOption();
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0003EEB8 File Offset: 0x0003D0B8
		public static IGroupManager GetGroupManager(this IGroupBox groupBox)
		{
			return GroupBoxUtility.s_GroupManagers.ContainsKey(groupBox) ? GroupBoxUtility.s_GroupManagers[groupBox] : null;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0003EEE8 File Offset: 0x0003D0E8
		private static IGroupManager FindOrCreateGroupManager(IGroupBox groupBox)
		{
			bool flag = GroupBoxUtility.s_GroupManagers.ContainsKey(groupBox);
			IGroupManager groupManager;
			if (flag)
			{
				groupManager = GroupBoxUtility.s_GroupManagers[groupBox];
			}
			else
			{
				Type type = null;
				foreach (Type type2 in groupBox.GetType().GetInterfaces())
				{
					bool flag2 = type2.IsGenericType && GroupBoxUtility.k_GenericGroupBoxType.IsAssignableFrom(type2.GetGenericTypeDefinition());
					if (flag2)
					{
						type = type2.GetGenericArguments()[0];
						break;
					}
				}
				IGroupManager groupManager3;
				if (!(type != null))
				{
					IGroupManager groupManager2 = new DefaultGroupManager();
					groupManager3 = groupManager2;
				}
				else
				{
					groupManager3 = (IGroupManager)Activator.CreateInstance(type);
				}
				IGroupManager groupManager4 = groupManager3;
				groupManager4.Init(groupBox);
				BaseVisualElementPanel baseVisualElementPanel = groupBox as BaseVisualElementPanel;
				bool flag3 = baseVisualElementPanel != null;
				if (flag3)
				{
					baseVisualElementPanel.panelDisposed += GroupBoxUtility.OnPanelDestroyed;
				}
				else
				{
					VisualElement visualElement = groupBox as VisualElement;
					bool flag4 = visualElement != null;
					if (flag4)
					{
						visualElement.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(GroupBoxUtility.OnGroupBoxDetachedFromPanel), TrickleDown.NoTrickleDown);
					}
				}
				GroupBoxUtility.s_GroupManagers[groupBox] = groupManager4;
				groupManager = groupManager4;
			}
			return groupManager;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0003F002 File Offset: 0x0003D202
		private static void OnGroupBoxDetachedFromPanel(DetachFromPanelEvent evt)
		{
			GroupBoxUtility.s_GroupManagers.Remove(evt.currentTarget as IGroupBox);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0003F01B File Offset: 0x0003D21B
		private static void OnPanelDestroyed(BaseVisualElementPanel panel)
		{
			GroupBoxUtility.s_GroupManagers.Remove(panel);
			panel.panelDisposed -= GroupBoxUtility.OnPanelDestroyed;
		}

		// Token: 0x040007A4 RID: 1956
		private static Dictionary<IGroupBox, IGroupManager> s_GroupManagers = new Dictionary<IGroupBox, IGroupManager>();

		// Token: 0x040007A5 RID: 1957
		private static Dictionary<IGroupBoxOption, IGroupManager> s_GroupOptionManagerCache = new Dictionary<IGroupBoxOption, IGroupManager>();

		// Token: 0x040007A6 RID: 1958
		private static readonly Type k_GenericGroupBoxType = typeof(IGroupBox<>);
	}
}
