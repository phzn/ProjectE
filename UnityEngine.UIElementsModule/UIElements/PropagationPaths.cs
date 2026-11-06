using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200022E RID: 558
	internal class PropagationPaths
	{
		// Token: 0x06001022 RID: 4130 RVA: 0x0003B353 File Offset: 0x00039553
		public PropagationPaths()
		{
			this.trickleDownPath = new List<VisualElement>(16);
			this.targetElements = new List<VisualElement>(4);
			this.bubbleUpPath = new List<VisualElement>(16);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003B383 File Offset: 0x00039583
		public PropagationPaths(PropagationPaths paths)
		{
			this.trickleDownPath = new List<VisualElement>(paths.trickleDownPath);
			this.targetElements = new List<VisualElement>(paths.targetElements);
			this.bubbleUpPath = new List<VisualElement>(paths.bubbleUpPath);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0003B3C0 File Offset: 0x000395C0
		internal static PropagationPaths Copy(PropagationPaths paths)
		{
			PropagationPaths propagationPaths = PropagationPaths.s_Pool.Get();
			propagationPaths.trickleDownPath.AddRange(paths.trickleDownPath);
			propagationPaths.targetElements.AddRange(paths.targetElements);
			propagationPaths.bubbleUpPath.AddRange(paths.bubbleUpPath);
			return propagationPaths;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0003B414 File Offset: 0x00039614
		public static PropagationPaths Build(VisualElement elem, EventBase evt)
		{
			PropagationPaths propagationPaths = PropagationPaths.s_Pool.Get();
			EventCategory eventCategory = evt.eventCategory;
			bool flag = elem.HasEventCallbacksOrDefaultActions(eventCategory);
			if (flag)
			{
				propagationPaths.targetElements.Add(elem);
			}
			for (VisualElement visualElement = elem.nextParentWithEventCallback; visualElement != null; visualElement = visualElement.nextParentWithEventCallback)
			{
				bool flag2 = visualElement.isCompositeRoot && !evt.ignoreCompositeRoots;
				if (flag2)
				{
					bool flag3 = visualElement.HasEventCallbacksOrDefaultActions(eventCategory);
					if (flag3)
					{
						propagationPaths.targetElements.Add(visualElement);
					}
				}
				else
				{
					bool flag4 = visualElement.HasEventCallbacks(eventCategory);
					if (flag4)
					{
						bool flag5 = evt.tricklesDown && visualElement.HasTrickleDownHandlers();
						if (flag5)
						{
							propagationPaths.trickleDownPath.Add(visualElement);
						}
						bool flag6 = evt.bubbles && visualElement.HasBubbleUpHandlers();
						if (flag6)
						{
							propagationPaths.bubbleUpPath.Add(visualElement);
						}
					}
				}
			}
			return propagationPaths;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003B508 File Offset: 0x00039708
		public void Release()
		{
			this.bubbleUpPath.Clear();
			this.targetElements.Clear();
			this.trickleDownPath.Clear();
			PropagationPaths.s_Pool.Release(this);
		}

		// Token: 0x04000727 RID: 1831
		private static readonly ObjectPool<PropagationPaths> s_Pool = new ObjectPool<PropagationPaths>(() => new PropagationPaths(), 100);

		// Token: 0x04000728 RID: 1832
		public readonly List<VisualElement> trickleDownPath;

		// Token: 0x04000729 RID: 1833
		public readonly List<VisualElement> targetElements;

		// Token: 0x0400072A RID: 1834
		public readonly List<VisualElement> bubbleUpPath;

		// Token: 0x0400072B RID: 1835
		private const int k_DefaultPropagationDepth = 16;

		// Token: 0x0400072C RID: 1836
		private const int k_DefaultTargetCount = 4;

		// Token: 0x0200022F RID: 559
		[Flags]
		public enum Type
		{
			// Token: 0x0400072E RID: 1838
			None = 0,
			// Token: 0x0400072F RID: 1839
			TrickleDown = 1,
			// Token: 0x04000730 RID: 1840
			BubbleUp = 2
		}
	}
}
