using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x02000022 RID: 34
	[NativeHeader("Modules/IMGUI/GUILayoutUtility.bindings.h")]
	public class GUILayoutUtility
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000993C File Offset: 0x00007B3C
		private static Rect Internal_GetWindowRect(int windowID)
		{
			Rect rect;
			GUILayoutUtility.Internal_GetWindowRect_Injected(windowID, out rect);
			return rect;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00009952 File Offset: 0x00007B52
		private static void Internal_MoveWindow(int windowID, Rect r)
		{
			GUILayoutUtility.Internal_MoveWindow_Injected(windowID, ref r);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000995C File Offset: 0x00007B5C
		internal static Rect GetWindowsBounds()
		{
			Rect rect;
			GUILayoutUtility.GetWindowsBounds_Injected(out rect);
			return rect;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00009971 File Offset: 0x00007B71
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00009978 File Offset: 0x00007B78
		internal static int unbalancedgroupscount { get; set; }

		// Token: 0x0600025B RID: 603 RVA: 0x00009980 File Offset: 0x00007B80
		internal static void CleanupRoots()
		{
			GUILayoutUtility.s_SpaceStyle = null;
			GUILayoutUtility.s_StoredLayouts.Clear();
			GUILayoutUtility.s_StoredWindows.Clear();
			GUILayoutUtility.current = new GUILayoutUtility.LayoutCache(-1);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000099AC File Offset: 0x00007BAC
		internal static GUILayoutUtility.LayoutCache GetLayoutCache(int instanceID, bool isWindow)
		{
			Dictionary<int, GUILayoutUtility.LayoutCache> dictionary = (isWindow ? GUILayoutUtility.s_StoredWindows : GUILayoutUtility.s_StoredLayouts);
			GUILayoutUtility.LayoutCache layoutCache;
			dictionary.TryGetValue(instanceID, out layoutCache);
			return layoutCache;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000099DC File Offset: 0x00007BDC
		internal static GUILayoutUtility.LayoutCache SelectIDList(int instanceID, bool isWindow)
		{
			Dictionary<int, GUILayoutUtility.LayoutCache> dictionary = (isWindow ? GUILayoutUtility.s_StoredWindows : GUILayoutUtility.s_StoredLayouts);
			GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.GetLayoutCache(instanceID, isWindow);
			bool flag = layoutCache == null;
			if (flag)
			{
				layoutCache = new GUILayoutUtility.LayoutCache(instanceID);
				dictionary[instanceID] = layoutCache;
			}
			GUILayoutUtility.current.topLevel = layoutCache.topLevel;
			GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
			GUILayoutUtility.current.windows = layoutCache.windows;
			return layoutCache;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009A54 File Offset: 0x00007C54
		internal static void RemoveSelectedIdList(int instanceID, bool isWindow)
		{
			Dictionary<int, GUILayoutUtility.LayoutCache> dictionary = (isWindow ? GUILayoutUtility.s_StoredWindows : GUILayoutUtility.s_StoredLayouts);
			bool flag = dictionary.ContainsKey(instanceID);
			if (flag)
			{
				dictionary.Remove(instanceID);
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009A88 File Offset: 0x00007C88
		internal static void Begin(int instanceID)
		{
			GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.SelectIDList(instanceID, false);
			bool flag = Event.current.type == EventType.Layout;
			if (flag)
			{
				GUILayoutUtility.current.topLevel = (layoutCache.topLevel = new GUILayoutGroup());
				GUILayoutUtility.current.layoutGroups.Clear();
				GUILayoutUtility.current.layoutGroups.Push(GUILayoutUtility.current.topLevel);
				GUILayoutUtility.current.windows = (layoutCache.windows = new GUILayoutGroup());
			}
			else
			{
				GUILayoutUtility.current.topLevel = layoutCache.topLevel;
				GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
				GUILayoutUtility.current.windows = layoutCache.windows;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009B40 File Offset: 0x00007D40
		internal static void BeginContainer(GUILayoutUtility.LayoutCache cache)
		{
			bool flag = Event.current.type == EventType.Layout;
			if (flag)
			{
				cache.topLevel = new GUILayoutGroup();
				cache.layoutGroups.Clear();
				cache.layoutGroups.Push(cache.topLevel);
				cache.windows = new GUILayoutGroup();
			}
			GUILayoutUtility.current.topLevel = cache.topLevel;
			GUILayoutUtility.current.layoutGroups = cache.layoutGroups;
			GUILayoutUtility.current.windows = cache.windows;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009BC8 File Offset: 0x00007DC8
		internal static void BeginWindow(int windowID, GUIStyle style, GUILayoutOption[] options)
		{
			GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.SelectIDList(windowID, true);
			bool flag = Event.current.type == EventType.Layout;
			if (flag)
			{
				GUILayoutUtility.current.topLevel = (layoutCache.topLevel = new GUILayoutGroup());
				GUILayoutUtility.current.topLevel.style = style;
				GUILayoutUtility.current.topLevel.windowID = windowID;
				bool flag2 = options != null;
				if (flag2)
				{
					GUILayoutUtility.current.topLevel.ApplyOptions(options);
				}
				GUILayoutUtility.current.layoutGroups.Clear();
				GUILayoutUtility.current.layoutGroups.Push(GUILayoutUtility.current.topLevel);
				GUILayoutUtility.current.windows = (layoutCache.windows = new GUILayoutGroup());
			}
			else
			{
				GUILayoutUtility.current.topLevel = layoutCache.topLevel;
				GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
				GUILayoutUtility.current.windows = layoutCache.windows;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00002221 File Offset: 0x00000421
		[Obsolete("BeginGroup has no effect and will be removed", false)]
		public static void BeginGroup(string GroupName)
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00002221 File Offset: 0x00000421
		[Obsolete("EndGroup has no effect and will be removed", false)]
		public static void EndGroup(string groupName)
		{
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009CBC File Offset: 0x00007EBC
		internal static void Layout()
		{
			bool flag = GUILayoutUtility.current.topLevel.windowID == -1;
			if (flag)
			{
				GUILayoutUtility.current.topLevel.CalcWidth();
				GUILayoutUtility.current.topLevel.SetHorizontal(0f, Mathf.Min((float)Screen.width / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxWidth));
				GUILayoutUtility.current.topLevel.CalcHeight();
				GUILayoutUtility.current.topLevel.SetVertical(0f, Mathf.Min((float)Screen.height / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxHeight));
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
			}
			else
			{
				GUILayoutUtility.LayoutSingleGroup(GUILayoutUtility.current.topLevel);
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009DA4 File Offset: 0x00007FA4
		internal static void LayoutFromEditorWindow()
		{
			bool flag = GUILayoutUtility.current.topLevel != null;
			if (flag)
			{
				GUILayoutUtility.current.topLevel.CalcWidth();
				GUILayoutUtility.current.topLevel.SetHorizontal(0f, (float)Screen.width / GUIUtility.pixelsPerPoint);
				GUILayoutUtility.current.topLevel.CalcHeight();
				GUILayoutUtility.current.topLevel.SetVertical(0f, (float)Screen.height / GUIUtility.pixelsPerPoint);
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
			}
			else
			{
				Debug.LogError("GUILayout state invalid. Verify that all layout begin/end calls match.");
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009E48 File Offset: 0x00008048
		internal static void LayoutFromContainer(float w, float h)
		{
			bool flag = GUILayoutUtility.current.topLevel != null;
			if (flag)
			{
				GUILayoutUtility.current.topLevel.CalcWidth();
				GUILayoutUtility.current.topLevel.SetHorizontal(0f, w);
				GUILayoutUtility.current.topLevel.CalcHeight();
				GUILayoutUtility.current.topLevel.SetVertical(0f, h);
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
			}
			else
			{
				Debug.LogError("GUILayout state invalid. Verify that all layout begin/end calls match.");
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009ED4 File Offset: 0x000080D4
		internal static float LayoutFromInspector(float width)
		{
			bool flag = GUILayoutUtility.current.topLevel != null && GUILayoutUtility.current.topLevel.windowID == -1;
			float num;
			if (flag)
			{
				GUILayoutUtility.current.topLevel.CalcWidth();
				GUILayoutUtility.current.topLevel.SetHorizontal(0f, width);
				GUILayoutUtility.current.topLevel.CalcHeight();
				GUILayoutUtility.current.topLevel.SetVertical(0f, Mathf.Min((float)Screen.height / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxHeight));
				float minHeight = GUILayoutUtility.current.topLevel.minHeight;
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
				num = minHeight;
			}
			else
			{
				bool flag2 = GUILayoutUtility.current.topLevel != null;
				if (flag2)
				{
					GUILayoutUtility.LayoutSingleGroup(GUILayoutUtility.current.topLevel);
				}
				num = 0f;
			}
			return num;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009FC4 File Offset: 0x000081C4
		internal static void LayoutFreeGroup(GUILayoutGroup toplevel)
		{
			foreach (GUILayoutEntry guilayoutEntry in toplevel.entries)
			{
				GUILayoutGroup guilayoutGroup = (GUILayoutGroup)guilayoutEntry;
				GUILayoutUtility.LayoutSingleGroup(guilayoutGroup);
			}
			toplevel.ResetCursor();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A028 File Offset: 0x00008228
		private static void LayoutSingleGroup(GUILayoutGroup i)
		{
			bool flag = !i.isWindow;
			if (flag)
			{
				float minWidth = i.minWidth;
				float maxWidth = i.maxWidth;
				i.CalcWidth();
				i.SetHorizontal(i.rect.x, Mathf.Clamp(i.maxWidth, minWidth, maxWidth));
				float minHeight = i.minHeight;
				float maxHeight = i.maxHeight;
				i.CalcHeight();
				i.SetVertical(i.rect.y, Mathf.Clamp(i.maxHeight, minHeight, maxHeight));
			}
			else
			{
				i.CalcWidth();
				Rect rect = GUILayoutUtility.Internal_GetWindowRect(i.windowID);
				i.SetHorizontal(rect.x, Mathf.Clamp(rect.width, i.minWidth, i.maxWidth));
				i.CalcHeight();
				i.SetVertical(rect.y, Mathf.Clamp(rect.height, i.minHeight, i.maxHeight));
				GUILayoutUtility.Internal_MoveWindow(i.windowID, i.rect);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A12C File Offset: 0x0000832C
		[SecuritySafeCritical]
		private static GUILayoutGroup CreateGUILayoutGroupInstanceOfType(Type LayoutType)
		{
			bool flag = !typeof(GUILayoutGroup).IsAssignableFrom(LayoutType);
			if (flag)
			{
				throw new ArgumentException("LayoutType needs to be of type GUILayoutGroup", "LayoutType");
			}
			return (GUILayoutGroup)Activator.CreateInstance(LayoutType);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A170 File Offset: 0x00008370
		internal static GUILayoutGroup BeginLayoutGroup(GUIStyle style, GUILayoutOption[] options, Type layoutType)
		{
			GUILayoutUtility.unbalancedgroupscount++;
			EventType type = Event.current.type;
			EventType eventType = type;
			GUILayoutGroup guilayoutGroup;
			if (eventType != EventType.Layout && eventType != EventType.Used)
			{
				guilayoutGroup = GUILayoutUtility.current.topLevel.GetNext() as GUILayoutGroup;
				bool flag = guilayoutGroup == null;
				if (flag)
				{
					throw new ExitGUIException("GUILayout: Mismatched LayoutGroup." + Event.current.type.ToString());
				}
				guilayoutGroup.ResetCursor();
			}
			else
			{
				guilayoutGroup = GUILayoutUtility.CreateGUILayoutGroupInstanceOfType(layoutType);
				guilayoutGroup.style = style;
				bool flag2 = options != null;
				if (flag2)
				{
					guilayoutGroup.ApplyOptions(options);
				}
				GUILayoutUtility.current.topLevel.Add(guilayoutGroup);
			}
			GUILayoutUtility.current.layoutGroups.Push(guilayoutGroup);
			GUILayoutUtility.current.topLevel = guilayoutGroup;
			return guilayoutGroup;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000A24C File Offset: 0x0000844C
		internal static void EndLayoutGroup()
		{
			GUILayoutUtility.unbalancedgroupscount--;
			bool flag = GUILayoutUtility.current.layoutGroups.Count == 0;
			if (flag)
			{
				Debug.LogError("EndLayoutGroup: BeginLayoutGroup must be called first.");
			}
			else
			{
				GUILayoutUtility.current.layoutGroups.Pop();
				bool flag2 = 0 < GUILayoutUtility.current.layoutGroups.Count;
				if (flag2)
				{
					GUILayoutUtility.current.topLevel = (GUILayoutGroup)GUILayoutUtility.current.layoutGroups.Peek();
				}
				else
				{
					GUILayoutUtility.current.topLevel = new GUILayoutGroup();
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000A2E0 File Offset: 0x000084E0
		internal static GUILayoutGroup BeginLayoutArea(GUIStyle style, Type layoutType)
		{
			GUILayoutUtility.unbalancedgroupscount++;
			EventType type = Event.current.type;
			EventType eventType = type;
			GUILayoutGroup guilayoutGroup;
			if (eventType != EventType.Layout && eventType != EventType.Used)
			{
				guilayoutGroup = GUILayoutUtility.current.windows.GetNext() as GUILayoutGroup;
				bool flag = guilayoutGroup == null;
				if (flag)
				{
					throw new ExitGUIException("GUILayout: Mismatched LayoutGroup." + Event.current.type.ToString());
				}
				guilayoutGroup.ResetCursor();
			}
			else
			{
				guilayoutGroup = GUILayoutUtility.CreateGUILayoutGroupInstanceOfType(layoutType);
				guilayoutGroup.style = style;
				GUILayoutUtility.current.windows.Add(guilayoutGroup);
			}
			GUILayoutUtility.current.layoutGroups.Push(guilayoutGroup);
			GUILayoutUtility.current.topLevel = guilayoutGroup;
			return guilayoutGroup;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A3A9 File Offset: 0x000085A9
		internal static void EndLayoutArea()
		{
			GUILayoutUtility.unbalancedgroupscount--;
			GUILayoutUtility.current.layoutGroups.Pop();
			GUILayoutUtility.current.topLevel = (GUILayoutGroup)GUILayoutUtility.current.layoutGroups.Peek();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A3E8 File Offset: 0x000085E8
		internal static GUILayoutGroup DoBeginLayoutArea(GUIStyle style, Type layoutType)
		{
			return GUILayoutUtility.BeginLayoutArea(style, layoutType);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000A401 File Offset: 0x00008601
		internal static GUILayoutGroup topLevel
		{
			get
			{
				return GUILayoutUtility.current.topLevel;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A410 File Offset: 0x00008610
		public static Rect GetRect(GUIContent content, GUIStyle style)
		{
			return GUILayoutUtility.DoGetRect(content, style, null);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A42C File Offset: 0x0000862C
		public static Rect GetRect(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(content, style, options);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A448 File Offset: 0x00008648
		private static Rect DoGetRect(GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			GUIUtility.CheckOnGUI();
			EventType type = Event.current.type;
			EventType eventType = type;
			Rect rect;
			if (eventType != EventType.Layout)
			{
				if (eventType != EventType.Used)
				{
					GUILayoutEntry next = GUILayoutUtility.current.topLevel.GetNext();
					rect = next.rect;
				}
				else
				{
					rect = GUILayoutUtility.kDummyRect;
				}
			}
			else
			{
				bool isHeightDependantOnWidth = style.isHeightDependantOnWidth;
				if (isHeightDependantOnWidth)
				{
					GUILayoutUtility.current.topLevel.Add(new GUIWordWrapSizer(style, content, options));
				}
				else
				{
					Vector2 vector = new Vector2(0f, 0f);
					bool flag = options != null;
					if (flag)
					{
						foreach (GUILayoutOption guilayoutOption in options)
						{
							GUILayoutOption.Type type2 = guilayoutOption.type;
							GUILayoutOption.Type type3 = type2;
							if (type3 != GUILayoutOption.Type.maxWidth)
							{
								if (type3 == GUILayoutOption.Type.maxHeight)
								{
									vector.y = (float)guilayoutOption.value;
								}
							}
							else
							{
								vector.x = (float)guilayoutOption.value;
							}
						}
					}
					Vector2 vector2 = style.CalcSizeWithConstraints(content, vector);
					vector2.x = Mathf.Ceil(vector2.x);
					vector2.y = Mathf.Ceil(vector2.y);
					GUILayoutUtility.current.topLevel.Add(new GUILayoutEntry(vector2.x, vector2.x, vector2.y, vector2.y, style, options));
				}
				rect = GUILayoutUtility.kDummyRect;
			}
			return rect;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A5BC File Offset: 0x000087BC
		public static Rect GetRect(float width, float height)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, GUIStyle.none, null);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public static Rect GetRect(float width, float height, GUIStyle style)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, style, null);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A600 File Offset: 0x00008800
		public static Rect GetRect(float width, float height, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, GUIStyle.none, options);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A624 File Offset: 0x00008824
		public static Rect GetRect(float width, float height, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, style, options);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A644 File Offset: 0x00008844
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, null);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A668 File Offset: 0x00008868
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, null);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A688 File Offset: 0x00008888
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A6AC File Offset: 0x000088AC
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A6CC File Offset: 0x000088CC
		private static Rect DoGetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, GUILayoutOption[] options)
		{
			EventType type = Event.current.type;
			EventType eventType = type;
			Rect rect;
			if (eventType != EventType.Layout)
			{
				if (eventType != EventType.Used)
				{
					rect = GUILayoutUtility.current.topLevel.GetNext().rect;
				}
				else
				{
					rect = GUILayoutUtility.kDummyRect;
				}
			}
			else
			{
				GUILayoutUtility.current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style, options));
				rect = new Rect(0f, 0f, maxWidth, maxHeight);
			}
			return rect;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A748 File Offset: 0x00008948
		public static Rect GetLastRect()
		{
			EventType type = Event.current.type;
			EventType eventType = type;
			Rect last;
			if (eventType != EventType.Layout && eventType != EventType.Used)
			{
				last = GUILayoutUtility.current.topLevel.GetLast();
			}
			else
			{
				last = GUILayoutUtility.kDummyRect;
			}
			return last;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A78C File Offset: 0x0000898C
		public static Rect GetAspectRect(float aspect)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, null);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public static Rect GetAspectRect(float aspect, GUIStyle style)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, null);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A7C4 File Offset: 0x000089C4
		public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, options);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A7E0 File Offset: 0x000089E0
		public static Rect GetAspectRect(float aspect, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, options);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A7FC File Offset: 0x000089FC
		private static Rect DoGetAspectRect(float aspect, GUILayoutOption[] options)
		{
			EventType type = Event.current.type;
			EventType eventType = type;
			Rect rect;
			if (eventType != EventType.Layout)
			{
				if (eventType != EventType.Used)
				{
					rect = GUILayoutUtility.current.topLevel.GetNext().rect;
				}
				else
				{
					rect = GUILayoutUtility.kDummyRect;
				}
			}
			else
			{
				GUILayoutUtility.current.topLevel.Add(new GUIAspectSizer(aspect, options));
				rect = GUILayoutUtility.kDummyRect;
			}
			return rect;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000A864 File Offset: 0x00008A64
		internal static GUIStyle spaceStyle
		{
			get
			{
				bool flag = GUILayoutUtility.s_SpaceStyle == null;
				if (flag)
				{
					GUILayoutUtility.s_SpaceStyle = new GUIStyle();
				}
				GUILayoutUtility.s_SpaceStyle.stretchWidth = false;
				return GUILayoutUtility.s_SpaceStyle;
			}
		}

		// Token: 0x06000286 RID: 646
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetWindowRect_Injected(int windowID, out Rect ret);

		// Token: 0x06000287 RID: 647
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_MoveWindow_Injected(int windowID, ref Rect r);

		// Token: 0x06000288 RID: 648
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetWindowsBounds_Injected(out Rect ret);

		// Token: 0x04000094 RID: 148
		private static readonly Dictionary<int, GUILayoutUtility.LayoutCache> s_StoredLayouts = new Dictionary<int, GUILayoutUtility.LayoutCache>();

		// Token: 0x04000095 RID: 149
		private static readonly Dictionary<int, GUILayoutUtility.LayoutCache> s_StoredWindows = new Dictionary<int, GUILayoutUtility.LayoutCache>();

		// Token: 0x04000096 RID: 150
		internal static GUILayoutUtility.LayoutCache current = new GUILayoutUtility.LayoutCache(-1);

		// Token: 0x04000097 RID: 151
		internal static readonly Rect kDummyRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04000099 RID: 153
		private static GUIStyle s_SpaceStyle;

		// Token: 0x02000023 RID: 35
		internal readonly struct LayoutCacheState
		{
			// Token: 0x06000289 RID: 649 RVA: 0x0000A8DC File Offset: 0x00008ADC
			public LayoutCacheState(GUILayoutUtility.LayoutCache cache)
			{
				this.id = cache.id;
				this.topLevel = cache.topLevel;
				this.layoutGroups = cache.layoutGroups;
				this.windows = cache.windows;
			}

			// Token: 0x0400009A RID: 154
			public readonly int id;

			// Token: 0x0400009B RID: 155
			public readonly GUILayoutGroup topLevel;

			// Token: 0x0400009C RID: 156
			public readonly GenericStack layoutGroups;

			// Token: 0x0400009D RID: 157
			public readonly GUILayoutGroup windows;
		}

		// Token: 0x02000024 RID: 36
		[DebuggerDisplay("id={id}, groups={layoutGroups.Count}")]
		internal sealed class LayoutCache
		{
			// Token: 0x17000044 RID: 68
			// (get) Token: 0x0600028A RID: 650 RVA: 0x0000A90F File Offset: 0x00008B0F
			// (set) Token: 0x0600028B RID: 651 RVA: 0x0000A917 File Offset: 0x00008B17
			internal int id { get; private set; }

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x0600028C RID: 652 RVA: 0x0000A920 File Offset: 0x00008B20
			public GUILayoutUtility.LayoutCacheState State
			{
				get
				{
					return new GUILayoutUtility.LayoutCacheState(this);
				}
			}

			// Token: 0x0600028D RID: 653 RVA: 0x0000A928 File Offset: 0x00008B28
			internal LayoutCache(int instanceID = -1)
			{
				this.id = instanceID;
				this.layoutGroups.Push(this.topLevel);
			}

			// Token: 0x0600028E RID: 654 RVA: 0x0000A978 File Offset: 0x00008B78
			internal void CopyState(GUILayoutUtility.LayoutCacheState other)
			{
				this.id = other.id;
				this.topLevel = other.topLevel;
				this.layoutGroups = other.layoutGroups;
				this.windows = other.windows;
			}

			// Token: 0x0600028F RID: 655 RVA: 0x0000A9AC File Offset: 0x00008BAC
			public void ResetCursor()
			{
				this.windows.ResetCursor();
				this.topLevel.ResetCursor();
				foreach (object obj in this.layoutGroups)
				{
					((GUILayoutGroup)obj).ResetCursor();
				}
			}

			// Token: 0x0400009F RID: 159
			internal GUILayoutGroup topLevel = new GUILayoutGroup();

			// Token: 0x040000A0 RID: 160
			internal GenericStack layoutGroups = new GenericStack();

			// Token: 0x040000A1 RID: 161
			internal GUILayoutGroup windows = new GUILayoutGroup();
		}
	}
}
