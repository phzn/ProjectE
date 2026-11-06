using System;
using Unity.Entities.CodeGeneratedJobForEach;

namespace Unity.Entities
{
	// Token: 0x0200023A RID: 570
	public static class LambdaSingleJobDescriptionConstructionMethods
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x00042D47 File Offset: 0x00040F47
		public static TDescription WithCode<TDescription>(this TDescription description, [AllowDynamicValue] LambdaSingleJobDescriptionConstructionMethods.WithCodeAction code) where TDescription : ISingleJobDescription
		{
			return description;
		}

		// Token: 0x0200023B RID: 571
		// (Invoke) Token: 0x06000E08 RID: 3592
		public delegate void WithCodeAction();
	}
}
