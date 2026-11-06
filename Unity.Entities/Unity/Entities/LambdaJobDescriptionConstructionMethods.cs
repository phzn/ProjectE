using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities.CodeGeneratedJobForEach;

namespace Unity.Entities
{
	// Token: 0x02000236 RID: 566
	public static class LambdaJobDescriptionConstructionMethods
	{
		// Token: 0x06000DF2 RID: 3570 RVA: 0x00042D47 File Offset: 0x00040F47
		public static TDescription WithoutBurst<TDescription>(this TDescription description) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00042D47 File Offset: 0x00040F47
		public static TDescription WithBurst<TDescription>(this TDescription description, FloatMode floatMode = FloatMode.Default, FloatPrecision floatPrecision = FloatPrecision.Standard, bool synchronousCompilation = false) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00042D47 File Offset: 0x00040F47
		public static TDescription WithName<TDescription>(this TDescription description, string name) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00042D47 File Offset: 0x00040F47
		public static TDescription WithStructuralChanges<TDescription>(this TDescription description) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00042D47 File Offset: 0x00040F47
		[LambdaJobDescriptionConstructionMethods.AllowMultipleInvocationsAttribute]
		public static TDescription WithReadOnly<TDescription, TCapturedVariableType>(this TDescription description, [AllowDynamicValue] TCapturedVariableType capturedVariable) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00042D47 File Offset: 0x00040F47
		[LambdaJobDescriptionConstructionMethods.AllowMultipleInvocationsAttribute]
		public static TDescription WithDisposeOnCompletion<TDescription, TCapturedVariableType>(this TDescription description, [AllowDynamicValue] TCapturedVariableType capturedVariable) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00042D47 File Offset: 0x00040F47
		[LambdaJobDescriptionConstructionMethods.AllowMultipleInvocationsAttribute]
		public static TDescription WithNativeDisableContainerSafetyRestriction<TDescription, TCapturedVariableType>(this TDescription description, [AllowDynamicValue] TCapturedVariableType capturedVariable) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00042D47 File Offset: 0x00040F47
		[LambdaJobDescriptionConstructionMethods.AllowMultipleInvocationsAttribute]
		public unsafe static TDescription WithNativeDisableUnsafePtrRestriction<TDescription, [global::System.Runtime.CompilerServices.IsUnmanaged] TCapturedVariableType>(this TDescription description, [AllowDynamicValue] TCapturedVariableType* capturedVariable) where TDescription : ILambdaJobDescription where TCapturedVariableType : struct, ValueType
		{
			return description;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00042D47 File Offset: 0x00040F47
		[LambdaJobDescriptionConstructionMethods.AllowMultipleInvocationsAttribute]
		public static TDescription WithNativeDisableParallelForRestriction<TDescription, TCapturedVariableType>(this TDescription description, [AllowDynamicValue] TCapturedVariableType capturedVariable) where TDescription : ILambdaJobDescription
		{
			return description;
		}

		// Token: 0x02000237 RID: 567
		[AttributeUsage(AttributeTargets.Method)]
		internal class AllowMultipleInvocationsAttribute : Attribute
		{
		}
	}
}
