using System;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Entities.UniversalDelegates;

namespace Unity.Entities
{
	// Token: 0x0200023C RID: 572
	public static class LambdaForEachDescriptionConstructionMethods
	{
		// Token: 0x06000E0B RID: 3595 RVA: 0x00042D9C File Offset: 0x00040F9C
		public static TDescription ThrowCodeGenException<TDescription>()
		{
			throw new Exception("This method should have been replaced by codegen");
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription>(this TDescription description, [AllowDynamicValue] Empty codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0>(this TDescription description, [AllowDynamicValue] R<T0> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0>(this TDescription description, [AllowDynamicValue] I<T0> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0>(this TDescription description, [AllowDynamicValue] V<T0> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1>(this TDescription description, [AllowDynamicValue] RI<T0, T1> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1>(this TDescription description, [AllowDynamicValue] RR<T0, T1> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1>(this TDescription description, [AllowDynamicValue] II<T0, T1> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1>(this TDescription description, [AllowDynamicValue] VI<T0, T1> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1>(this TDescription description, [AllowDynamicValue] VR<T0, T1> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1>(this TDescription description, [AllowDynamicValue] VV<T0, T1> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] RII<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] RRI<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] RRR<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] III<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] VII<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] VRI<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] VRR<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] VVI<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] VVR<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2>(this TDescription description, [AllowDynamicValue] VVV<T0, T1, T2> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] RIII<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] RRII<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] RRRI<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] RRRR<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] IIII<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VIII<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VRII<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VRRI<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VRRR<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VVII<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VVRI<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VVRR<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VVVI<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VVVR<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3>(this TDescription description, [AllowDynamicValue] VVVV<T0, T1, T2, T3> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] RIIII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] RRIII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] RRRII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] RRRRI<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] RRRRR<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] IIIII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VIIII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VRIII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VRRII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VRRRI<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VRRRR<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVIII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVRII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVRRI<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVRRR<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVVII<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVVRI<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVVRR<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVVVI<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVVVR<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4>(this TDescription description, [AllowDynamicValue] VVVVV<T0, T1, T2, T3, T4> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] RIIIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] RRIIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] RRRIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] RRRRII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] RRRRRI<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] RRRRRR<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] IIIIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VIIIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VRIIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VRRIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VRRRII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VRRRRI<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VRRRRR<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVIIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVRIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVRRII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVRRRI<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVRRRR<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVIII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVRII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVRRI<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVRRR<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVVII<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVVRI<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVVRR<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVVVI<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVVVR<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5>(this TDescription description, [AllowDynamicValue] VVVVVV<T0, T1, T2, T3, T4, T5> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RIIIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RRIIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RRRIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RRRRIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RRRRRII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RRRRRRI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] RRRRRRR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] IIIIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VIIIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VRIIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VRRIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VRRRIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VRRRRII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VRRRRRI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VRRRRRR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVIIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVRIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVRRIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVRRRII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVRRRRI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVRRRRR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVIIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVRIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVRRII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVRRRI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVRRRR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVIII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVRII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVRRI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVRRR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVVII<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVVRI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVVRR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVVVI<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVVVR<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6>(this TDescription description, [AllowDynamicValue] VVVVVVV<T0, T1, T2, T3, T4, T5, T6> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RIIIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRIIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRRIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRRRIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRRRRIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRRRRRII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRRRRRRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] RRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] IIIIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VIIIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRIIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRRIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRRRIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRRRRIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRRRRRII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRRRRRRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVIIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVRIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVRRIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVRRRIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVRRRRII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVRRRRRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVIIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVRIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVRRIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVRRRII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVRRRRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVRRRRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVIIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVRIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVRRII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVRRRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVRRRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVIII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVRII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVRRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVRRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVVII<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVVRI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVVRR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVVVI<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVVVR<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00042DB3 File Offset: 0x00040FB3
		public static TDescription ForEach<TDescription, T0, T1, T2, T3, T4, T5, T6, T7>(this TDescription description, [AllowDynamicValue] VVVVVVVV<T0, T1, T2, T3, T4, T5, T6, T7> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate
		{
			return LambdaForEachDescriptionConstructionMethods.ThrowCodeGenException<TDescription>();
		}
	}
}
