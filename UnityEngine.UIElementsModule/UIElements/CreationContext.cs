using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003E8 RID: 1000
	public struct CreationContext : IEquatable<CreationContext>
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060020AE RID: 8366 RVA: 0x0007BF8F File Offset: 0x0007A18F
		// (set) Token: 0x060020AF RID: 8367 RVA: 0x0007BF97 File Offset: 0x0007A197
		public VisualElement target { readonly get; private set; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x0007BFA0 File Offset: 0x0007A1A0
		// (set) Token: 0x060020B1 RID: 8369 RVA: 0x0007BFA8 File Offset: 0x0007A1A8
		public VisualTreeAsset visualTreeAsset { readonly get; private set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060020B2 RID: 8370 RVA: 0x0007BFB1 File Offset: 0x0007A1B1
		// (set) Token: 0x060020B3 RID: 8371 RVA: 0x0007BFB9 File Offset: 0x0007A1B9
		public Dictionary<string, VisualElement> slotInsertionPoints { readonly get; private set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x0007BFC2 File Offset: 0x0007A1C2
		// (set) Token: 0x060020B5 RID: 8373 RVA: 0x0007BFCA File Offset: 0x0007A1CA
		internal List<TemplateAsset.AttributeOverride> attributeOverrides { readonly get; private set; }

		// Token: 0x060020B6 RID: 8374 RVA: 0x0007BFD3 File Offset: 0x0007A1D3
		internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, VisualTreeAsset vta, VisualElement target)
		{
			this = new CreationContext(slotInsertionPoints, null, vta, target);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x0007BFE1 File Offset: 0x0007A1E1
		internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, List<TemplateAsset.AttributeOverride> attributeOverrides, VisualTreeAsset vta, VisualElement target)
		{
			this.target = target;
			this.slotInsertionPoints = slotInsertionPoints;
			this.attributeOverrides = attributeOverrides;
			this.visualTreeAsset = vta;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x0007C008 File Offset: 0x0007A208
		public override bool Equals(object obj)
		{
			return obj is CreationContext && this.Equals((CreationContext)obj);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x0007C034 File Offset: 0x0007A234
		public bool Equals(CreationContext other)
		{
			return EqualityComparer<VisualElement>.Default.Equals(this.target, other.target) && EqualityComparer<VisualTreeAsset>.Default.Equals(this.visualTreeAsset, other.visualTreeAsset) && EqualityComparer<Dictionary<string, VisualElement>>.Default.Equals(this.slotInsertionPoints, other.slotInsertionPoints);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x0007C094 File Offset: 0x0007A294
		public override int GetHashCode()
		{
			int num = -2123482148;
			num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(this.target);
			num = num * -1521134295 + EqualityComparer<VisualTreeAsset>.Default.GetHashCode(this.visualTreeAsset);
			return num * -1521134295 + EqualityComparer<Dictionary<string, VisualElement>>.Default.GetHashCode(this.slotInsertionPoints);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x0007C0F8 File Offset: 0x0007A2F8
		public static bool operator ==(CreationContext context1, CreationContext context2)
		{
			return context1.Equals(context2);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x0007C114 File Offset: 0x0007A314
		public static bool operator !=(CreationContext context1, CreationContext context2)
		{
			return !(context1 == context2);
		}

		// Token: 0x04000D89 RID: 3465
		public static readonly CreationContext Default = default(CreationContext);
	}
}
