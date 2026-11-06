using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000365 RID: 869
	public class TemplateContainer : BindableElement
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000703B6 File Offset: 0x0006E5B6
		// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x000703BE File Offset: 0x0006E5BE
		public string templateId { get; private set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000703C7 File Offset: 0x0006E5C7
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000703CF File Offset: 0x0006E5CF
		public VisualTreeAsset templateSource
		{
			get
			{
				return this.m_TemplateSource;
			}
			internal set
			{
				this.m_TemplateSource = value;
			}
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x000703D8 File Offset: 0x0006E5D8
		public TemplateContainer()
			: this(null)
		{
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x000703E3 File Offset: 0x0006E5E3
		public TemplateContainer(string templateId)
		{
			this.templateId = templateId;
			this.m_ContentContainer = this;
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000703FC File Offset: 0x0006E5FC
		public override VisualElement contentContainer
		{
			get
			{
				return this.m_ContentContainer;
			}
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x00070414 File Offset: 0x0006E614
		internal void SetContentContainer(VisualElement content)
		{
			this.m_ContentContainer = content;
		}

		// Token: 0x04000C2E RID: 3118
		private VisualElement m_ContentContainer;

		// Token: 0x04000C2F RID: 3119
		private VisualTreeAsset m_TemplateSource;

		// Token: 0x02000366 RID: 870
		public new class UxmlFactory : UxmlFactory<TemplateContainer, TemplateContainer.UxmlTraits>
		{
			// Token: 0x170006BD RID: 1725
			// (get) Token: 0x06001CEB RID: 7403 RVA: 0x0007041E File Offset: 0x0006E61E
			public override string uxmlName
			{
				get
				{
					return "Instance";
				}
			}

			// Token: 0x170006BE RID: 1726
			// (get) Token: 0x06001CEC RID: 7404 RVA: 0x00070425 File Offset: 0x0006E625
			public override string uxmlQualifiedName
			{
				get
				{
					return this.uxmlNamespace + "." + this.uxmlName;
				}
			}

			// Token: 0x04000C30 RID: 3120
			internal const string k_ElementName = "Instance";
		}

		// Token: 0x02000367 RID: 871
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x170006BF RID: 1727
			// (get) Token: 0x06001CEE RID: 7406 RVA: 0x00070448 File Offset: 0x0006E648
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x06001CEF RID: 7407 RVA: 0x00070468 File Offset: 0x0006E668
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				TemplateContainer templateContainer = (TemplateContainer)ve;
				templateContainer.templateId = this.m_Template.GetValueFromBag(bag, cc);
				VisualTreeAsset visualTreeAsset = cc.visualTreeAsset;
				VisualTreeAsset visualTreeAsset2 = ((visualTreeAsset != null) ? visualTreeAsset.ResolveTemplate(templateContainer.templateId) : null);
				bool flag = visualTreeAsset2 == null;
				if (flag)
				{
					templateContainer.Add(new Label(string.Format("Unknown Template: '{0}'", templateContainer.templateId)));
				}
				else
				{
					TemplateAsset templateAsset = bag as TemplateAsset;
					List<TemplateAsset.AttributeOverride> list = ((templateAsset != null) ? templateAsset.attributeOverrides : null);
					List<TemplateAsset.AttributeOverride> attributeOverrides = cc.attributeOverrides;
					List<TemplateAsset.AttributeOverride> list2 = null;
					bool flag2 = list != null || attributeOverrides != null;
					if (flag2)
					{
						list2 = new List<TemplateAsset.AttributeOverride>();
						bool flag3 = attributeOverrides != null;
						if (flag3)
						{
							list2.AddRange(attributeOverrides);
						}
						bool flag4 = list != null;
						if (flag4)
						{
							list2.AddRange(list);
						}
					}
					visualTreeAsset2.CloneTree(ve, cc.slotInsertionPoints, list2);
				}
				bool flag5 = visualTreeAsset2 == null;
				if (flag5)
				{
					Debug.LogErrorFormat("Could not resolve template with name '{0}'", new object[] { templateContainer.templateId });
				}
			}

			// Token: 0x04000C31 RID: 3121
			internal const string k_TemplateAttributeName = "template";

			// Token: 0x04000C32 RID: 3122
			private UxmlStringAttributeDescription m_Template = new UxmlStringAttributeDescription
			{
				name = "template",
				use = UxmlAttributeDescription.Use.Required
			};
		}
	}
}
