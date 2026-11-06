using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000238 RID: 568
	[EventCategory(EventCategory.StyleTransition)]
	public abstract class TransitionEventBase<T> : EventBase<T>, ITransitionEvent where T : TransitionEventBase<T>, new()
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0003B7B7 File Offset: 0x000399B7
		public StylePropertyNameCollection stylePropertyNames { get; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0003B7BF File Offset: 0x000399BF
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x0003B7C7 File Offset: 0x000399C7
		public double elapsedTime { get; protected set; }

		// Token: 0x0600104D RID: 4173 RVA: 0x0003B7D0 File Offset: 0x000399D0
		protected TransitionEventBase()
		{
			this.stylePropertyNames = new StylePropertyNameCollection(new List<StylePropertyName>());
			this.LocalInit();
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0003B7F1 File Offset: 0x000399F1
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0003B802 File Offset: 0x00039A02
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles;
			base.propagateToIMGUI = false;
			this.stylePropertyNames.propertiesList.Clear();
			this.elapsedTime = 0.0;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0003B838 File Offset: 0x00039A38
		public static T GetPooled(StylePropertyName stylePropertyName, double elapsedTime)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.stylePropertyNames.propertiesList.Add(stylePropertyName);
			pooled.elapsedTime = elapsedTime;
			return pooled;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003B878 File Offset: 0x00039A78
		public bool AffectsProperty(StylePropertyName stylePropertyName)
		{
			return this.stylePropertyNames.Contains(stylePropertyName);
		}
	}
}
