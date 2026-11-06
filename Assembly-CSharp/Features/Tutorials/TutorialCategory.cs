using System;

namespace Features.Tutorials
{
	// Token: 0x02000BCF RID: 3023
	public sealed class TutorialCategory
	{
		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x060056F1 RID: 22257 RVA: 0x0021D4AF File Offset: 0x0021B6AF
		public string Id { get; }

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x060056F2 RID: 22258 RVA: 0x0021D4B7 File Offset: 0x0021B6B7
		public string Title { get; }

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x060056F3 RID: 22259 RVA: 0x0021D4BF File Offset: 0x0021B6BF
		public string Kicker { get; }

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x060056F4 RID: 22260 RVA: 0x0021D4C7 File Offset: 0x0021B6C7
		public string Glyph { get; }

		// Token: 0x060056F5 RID: 22261 RVA: 0x0021D4CF File Offset: 0x0021B6CF
		public TutorialCategory(string id, string title, string kicker, string glyph)
		{
			this.Id = id;
			this.Title = title;
			this.Kicker = kicker;
			this.Glyph = glyph;
		}
	}
}
