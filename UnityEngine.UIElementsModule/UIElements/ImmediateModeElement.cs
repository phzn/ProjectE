using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x0200026B RID: 619
	public abstract class ImmediateModeElement : VisualElement
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00040798 File Offset: 0x0003E998
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x000407B0 File Offset: 0x0003E9B0
		public bool cullingEnabled
		{
			get
			{
				return this.m_CullingEnabled;
			}
			set
			{
				this.m_CullingEnabled = value;
				base.IncrementVersion(VersionChangeType.Repaint);
			}
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000407C8 File Offset: 0x0003E9C8
		public ImmediateModeElement()
		{
			base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(this.OnGenerateVisualContent));
			Type type = base.GetType();
			bool flag = !ImmediateModeElement.s_Markers.TryGetValue(type, out this.m_ImmediateRepaintMarker);
			if (flag)
			{
				this.m_ImmediateRepaintMarker = new ProfilerMarker(base.typeName + ".ImmediateRepaint");
				ImmediateModeElement.s_Markers[type] = this.m_ImmediateRepaintMarker;
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00040855 File Offset: 0x0003EA55
		private void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			mgc.painter.DrawImmediate(new Action(this.CallImmediateRepaint), this.cullingEnabled);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00040878 File Offset: 0x0003EA78
		private void CallImmediateRepaint()
		{
			using (this.m_ImmediateRepaintMarker.Auto())
			{
				this.ImmediateRepaint();
			}
		}

		// Token: 0x060011AE RID: 4526
		protected abstract void ImmediateRepaint();

		// Token: 0x040007CF RID: 1999
		private static readonly Dictionary<Type, ProfilerMarker> s_Markers = new Dictionary<Type, ProfilerMarker>();

		// Token: 0x040007D0 RID: 2000
		private readonly ProfilerMarker m_ImmediateRepaintMarker;

		// Token: 0x040007D1 RID: 2001
		private bool m_CullingEnabled = false;
	}
}
