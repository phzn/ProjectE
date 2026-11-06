using System;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000239 RID: 569
	public static class LambdaSingleJobDescriptionExecutionMethods
	{
		// Token: 0x06000E02 RID: 3586 RVA: 0x00042D73 File Offset: 0x00040F73
		public static JobHandle Schedule<TDescription>(this TDescription description, [AllowDynamicValue] JobHandle dependency) where TDescription : ILambdaSingleJobExecutionDescription
		{
			return LambdaSingleJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00042D7A File Offset: 0x00040F7A
		public static void Schedule<TDescription>(this TDescription description) where TDescription : ILambdaSingleJobExecutionDescription
		{
			LambdaSingleJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00042D7A File Offset: 0x00040F7A
		public static void Run<TDescription>(this TDescription description) where TDescription : ILambdaSingleJobExecutionDescription
		{
			LambdaSingleJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00042D84 File Offset: 0x00040F84
		private static JobHandle ThrowCodeGenException()
		{
			throw new Exception("This SystemBase method should have been replaced by codegen");
		}
	}
}
