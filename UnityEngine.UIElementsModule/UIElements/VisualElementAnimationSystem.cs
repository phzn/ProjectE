using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine.UIElements.Experimental;

namespace UnityEngine.UIElements
{
	// Token: 0x0200040F RID: 1039
	internal class VisualElementAnimationSystem : BaseVisualTreeUpdater
	{
		// Token: 0x0600212F RID: 8495 RVA: 0x0007D8D0 File Offset: 0x0007BAD0
		private long CurrentTimeMs()
		{
			return Panel.TimeSinceStartupMs();
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x0007D8E7 File Offset: 0x0007BAE7
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return VisualElementAnimationSystem.s_ProfilerMarker;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x0007D8EE File Offset: 0x0007BAEE
		private static ProfilerMarker stylePropertyAnimationProfilerMarker
		{
			get
			{
				return VisualElementAnimationSystem.s_StylePropertyAnimationProfilerMarker;
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x0007D8F5 File Offset: 0x0007BAF5
		public void UnregisterAnimation(IValueAnimationUpdate anim)
		{
			this.m_Animations.Remove(anim);
			this.m_IterationListDirty = true;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0007D90C File Offset: 0x0007BB0C
		public void UnregisterAnimations(List<IValueAnimationUpdate> anims)
		{
			foreach (IValueAnimationUpdate valueAnimationUpdate in anims)
			{
				this.m_Animations.Remove(valueAnimationUpdate);
			}
			this.m_IterationListDirty = true;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0007D96C File Offset: 0x0007BB6C
		public void RegisterAnimation(IValueAnimationUpdate anim)
		{
			this.m_Animations.Add(anim);
			this.m_HasNewAnimations = true;
			this.m_IterationListDirty = true;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0007D98C File Offset: 0x0007BB8C
		public void RegisterAnimations(List<IValueAnimationUpdate> anims)
		{
			foreach (IValueAnimationUpdate valueAnimationUpdate in anims)
			{
				this.m_Animations.Add(valueAnimationUpdate);
			}
			this.m_HasNewAnimations = true;
			this.m_IterationListDirty = true;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x0007D9F4 File Offset: 0x0007BBF4
		public override void Update()
		{
			long num = Panel.TimeSinceStartupMs();
			bool iterationListDirty = this.m_IterationListDirty;
			if (iterationListDirty)
			{
				this.m_IterationList = this.m_Animations.ToList<IValueAnimationUpdate>();
				this.m_IterationListDirty = false;
			}
			bool flag = this.m_HasNewAnimations || this.lastUpdate != num;
			if (flag)
			{
				foreach (IValueAnimationUpdate valueAnimationUpdate in this.m_IterationList)
				{
					valueAnimationUpdate.Tick(num);
				}
				this.m_HasNewAnimations = false;
				this.lastUpdate = num;
			}
			IStylePropertyAnimationSystem styleAnimationSystem = base.panel.styleAnimationSystem;
			using (VisualElementAnimationSystem.stylePropertyAnimationProfilerMarker.Auto())
			{
				styleAnimationSystem.Update();
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
		}

		// Token: 0x04000E06 RID: 3590
		private HashSet<IValueAnimationUpdate> m_Animations = new HashSet<IValueAnimationUpdate>();

		// Token: 0x04000E07 RID: 3591
		private List<IValueAnimationUpdate> m_IterationList = new List<IValueAnimationUpdate>();

		// Token: 0x04000E08 RID: 3592
		private bool m_HasNewAnimations = false;

		// Token: 0x04000E09 RID: 3593
		private bool m_IterationListDirty = false;

		// Token: 0x04000E0A RID: 3594
		private static readonly string s_Description = "Animation Update";

		// Token: 0x04000E0B RID: 3595
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(VisualElementAnimationSystem.s_Description);

		// Token: 0x04000E0C RID: 3596
		private static readonly string s_StylePropertyAnimationDescription = "StylePropertyAnimation Update";

		// Token: 0x04000E0D RID: 3597
		private static readonly ProfilerMarker s_StylePropertyAnimationProfilerMarker = new ProfilerMarker(VisualElementAnimationSystem.s_StylePropertyAnimationDescription);

		// Token: 0x04000E0E RID: 3598
		private long lastUpdate;
	}
}
