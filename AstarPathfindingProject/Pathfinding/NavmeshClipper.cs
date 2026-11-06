using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200012B RID: 299
	[ExecuteAlways]
	public abstract class NavmeshClipper : VersionedMonoBehaviour
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x00034710 File Offset: 0x00032910
		internal static void RefreshEnabledList()
		{
			NavmeshClipper[] array = UnityCompatibility.FindObjectsByTypeUnsorted<NavmeshClipper>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].enabled && array[i].listIndex == -1)
				{
					array[i].enabled = false;
					array[i].enabled = true;
				}
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00034758 File Offset: 0x00032958
		public static void AddEnableCallback(Action<NavmeshClipper> onEnable, Action<NavmeshClipper> onDisable)
		{
			NavmeshClipper.OnEnableCallback = (Action<NavmeshClipper>)Delegate.Combine(NavmeshClipper.OnEnableCallback, onEnable);
			NavmeshClipper.OnDisableCallback = (Action<NavmeshClipper>)Delegate.Combine(NavmeshClipper.OnDisableCallback, onDisable);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00034784 File Offset: 0x00032984
		public static void RemoveEnableCallback(Action<NavmeshClipper> onEnable, Action<NavmeshClipper> onDisable)
		{
			NavmeshClipper.OnEnableCallback = (Action<NavmeshClipper>)Delegate.Remove(NavmeshClipper.OnEnableCallback, onEnable);
			NavmeshClipper.OnDisableCallback = (Action<NavmeshClipper>)Delegate.Remove(NavmeshClipper.OnDisableCallback, onDisable);
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x000347B0 File Offset: 0x000329B0
		public static List<NavmeshClipper> allEnabled
		{
			get
			{
				return NavmeshClipper.all;
			}
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x000347B7 File Offset: 0x000329B7
		protected virtual void OnEnable()
		{
			if (this.listIndex != -1)
			{
				return;
			}
			if (NavmeshClipper.OnEnableCallback != null)
			{
				NavmeshClipper.OnEnableCallback(this);
			}
			this.listIndex = NavmeshClipper.all.Count;
			NavmeshClipper.all.Add(this);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000347F0 File Offset: 0x000329F0
		protected virtual void OnDisable()
		{
			if (this.listIndex == -1)
			{
				return;
			}
			NavmeshClipper.all[this.listIndex] = NavmeshClipper.all[NavmeshClipper.all.Count - 1];
			NavmeshClipper.all[this.listIndex].listIndex = this.listIndex;
			NavmeshClipper.all.RemoveAt(NavmeshClipper.all.Count - 1);
			this.listIndex = -1;
			if (NavmeshClipper.OnDisableCallback != null)
			{
				NavmeshClipper.OnDisableCallback(this);
			}
		}

		// Token: 0x060009F7 RID: 2551
		public abstract void NotifyUpdated(GridLookup<NavmeshClipper>.Root previousState);

		// Token: 0x060009F8 RID: 2552
		public abstract Bounds GetBounds(GraphTransform transform, float radiusMargin);

		// Token: 0x060009F9 RID: 2553
		public abstract bool RequiresUpdate(GridLookup<NavmeshClipper>.Root previousState);

		// Token: 0x060009FA RID: 2554
		public abstract void ForceUpdate();

		// Token: 0x0400067E RID: 1662
		private static Action<NavmeshClipper> OnEnableCallback;

		// Token: 0x0400067F RID: 1663
		private static Action<NavmeshClipper> OnDisableCallback;

		// Token: 0x04000680 RID: 1664
		private static readonly List<NavmeshClipper> all = new List<NavmeshClipper>();

		// Token: 0x04000681 RID: 1665
		private int listIndex = -1;

		// Token: 0x04000682 RID: 1666
		public GraphMask graphMask = GraphMask.everything;
	}
}
