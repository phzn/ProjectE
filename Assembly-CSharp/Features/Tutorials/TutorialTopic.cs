using System;
using System.Collections.Generic;
using Features.CielOS.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BD0 RID: 3024
	public sealed class TutorialTopic
	{
		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x060056F6 RID: 22262 RVA: 0x0021D4F4 File Offset: 0x0021B6F4
		public string Id { get; }

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x060056F7 RID: 22263 RVA: 0x0021D4FC File Offset: 0x0021B6FC
		public string CategoryId { get; }

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x060056F8 RID: 22264 RVA: 0x0021D504 File Offset: 0x0021B704
		public string Title { get; }

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x0021D50C File Offset: 0x0021B70C
		public string Kicker { get; }

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x060056FA RID: 22266 RVA: 0x0021D514 File Offset: 0x0021B714
		public string Summary { get; }

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x060056FB RID: 22267 RVA: 0x0021D51C File Offset: 0x0021B71C
		public string Description { get; }

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x060056FC RID: 22268 RVA: 0x0021D524 File Offset: 0x0021B724
		public string Glyph { get; }

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x060056FD RID: 22269 RVA: 0x0021D52C File Offset: 0x0021B72C
		public string VisualResource { get; }

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x060056FE RID: 22270 RVA: 0x0021D534 File Offset: 0x0021B734
		public TutorialVisualKind VisualKind { get; }

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x060056FF RID: 22271 RVA: 0x0021D53C File Offset: 0x0021B73C
		public CielOSIconId IconId { get; }

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005700 RID: 22272 RVA: 0x0021D544 File Offset: 0x0021B744
		public string PlayerAction { get; }

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005701 RID: 22273 RVA: 0x0021D54C File Offset: 0x0021B74C
		public string ExpectedFeedback { get; }

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005702 RID: 22274 RVA: 0x0021D554 File Offset: 0x0021B754
		public IReadOnlyList<string> Bindings { get; }

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005703 RID: 22275 RVA: 0x0021D55C File Offset: 0x0021B75C
		public IReadOnlyList<string> Tips { get; }

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005704 RID: 22276 RVA: 0x0021D564 File Offset: 0x0021B764
		public IReadOnlyList<string> VisualCaptions { get; }

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x0021D56C File Offset: 0x0021B76C
		public IReadOnlyList<string> VisualResources { get; }

		// Token: 0x06005706 RID: 22278 RVA: 0x0021D574 File Offset: 0x0021B774
		public TutorialTopic(string id, string categoryId, string title, string kicker, string summary, string description, string glyph, string[] bindings, string[] tips, TutorialVisualKind visualKind, string visualResource, CielOSIconId iconId, string playerAction, string expectedFeedback, string[] visualCaptions, string[] visualResources = null)
		{
			this.Id = id;
			this.CategoryId = categoryId;
			this.Title = title;
			this.Kicker = kicker;
			this.Summary = summary;
			this.Description = description;
			this.Glyph = glyph;
			this.VisualResource = visualResource;
			this.VisualKind = visualKind;
			this.IconId = iconId;
			this.PlayerAction = playerAction;
			this.ExpectedFeedback = expectedFeedback;
			this.Bindings = bindings ?? Array.Empty<string>();
			this.Tips = tips ?? Array.Empty<string>();
			this.VisualCaptions = visualCaptions ?? Array.Empty<string>();
			this.VisualResources = visualResources ?? Array.Empty<string>();
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x0021D628 File Offset: 0x0021B828
		public string GetVisualCaption(int index)
		{
			if (index < 0 || index >= this.VisualCaptions.Count)
			{
				return "IN-GAME EXAMPLE";
			}
			return this.VisualCaptions[index];
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x0021D650 File Offset: 0x0021B850
		public string GetVisualResource(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (this.VisualResources.Count <= 0)
			{
				return "Tutorials/Guide/" + this.Id + ((index == 0) ? string.Empty : ("_" + (index + 1).ToString()));
			}
			if (index >= this.VisualResources.Count)
			{
				return null;
			}
			return this.VisualResources[index];
		}
	}
}
