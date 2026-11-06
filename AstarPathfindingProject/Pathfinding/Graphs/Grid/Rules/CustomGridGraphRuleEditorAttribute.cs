using System;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000210 RID: 528
	public class CustomGridGraphRuleEditorAttribute : Attribute
	{
		// Token: 0x06000DF2 RID: 3570 RVA: 0x000557C9 File Offset: 0x000539C9
		public CustomGridGraphRuleEditorAttribute(Type type, string name)
		{
			this.type = type;
			this.name = name;
		}

		// Token: 0x040009F1 RID: 2545
		public Type type;

		// Token: 0x040009F2 RID: 2546
		public string name;
	}
}
