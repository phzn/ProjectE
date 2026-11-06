using System;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000238 RID: 568
	public static class LambdaJobDescriptionExecutionMethods
	{
		// Token: 0x06000DFC RID: 3580 RVA: 0x00042D4A File Offset: 0x00040F4A
		public static JobHandle Schedule<TDescription>(this TDescription description, [AllowDynamicValue] JobHandle dependency) where TDescription : ILambdaJobExecutionDescription
		{
			return LambdaJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00042D4A File Offset: 0x00040F4A
		public static JobHandle ScheduleParallel<TDescription>(this TDescription description, [AllowDynamicValue] JobHandle dependency) where TDescription : ILambdaJobExecutionDescription
		{
			return LambdaJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00042D51 File Offset: 0x00040F51
		public static void Schedule<TDescription>(this TDescription description) where TDescription : ILambdaJobExecutionDescription
		{
			LambdaJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00042D51 File Offset: 0x00040F51
		public static void ScheduleParallel<TDescription>(this TDescription description) where TDescription : ILambdaJobExecutionDescription
		{
			LambdaJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00042D51 File Offset: 0x00040F51
		public static void Run<TDescription>(this TDescription description) where TDescription : ILambdaJobExecutionDescription
		{
			LambdaJobDescriptionExecutionMethods.ThrowCodeGenException();
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00042D5C File Offset: 0x00040F5C
		private static JobHandle ThrowCodeGenException()
		{
			throw new Exception("This SystemBase method should have been replaced by codegen");
		}
	}
}
