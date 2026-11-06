using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Entities.Internal;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001BE RID: 446
	public static class IJobEntityExtensions
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x0003268C File Offset: 0x0003088C
		[MustUseReturnValue]
		public static JobHandle Schedule<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x000326A0 File Offset: 0x000308A0
		[MustUseReturnValue]
		public static JobHandle ScheduleByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x000326B4 File Offset: 0x000308B4
		[MustUseReturnValue]
		public static JobHandle Schedule<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x000326C8 File Offset: 0x000308C8
		[MustUseReturnValue]
		public static JobHandle ScheduleByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x000326DA File Offset: 0x000308DA
		public static void Schedule<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000326DA File Offset: 0x000308DA
		public static void ScheduleByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000326DA File Offset: 0x000308DA
		public static void Schedule<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000326DA File Offset: 0x000308DA
		public static void ScheduleByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000326E4 File Offset: 0x000308E4
		[MustUseReturnValue]
		public static JobHandle ScheduleParallel<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000326F8 File Offset: 0x000308F8
		[MustUseReturnValue]
		public static JobHandle ScheduleParallelByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0003270C File Offset: 0x0003090C
		[MustUseReturnValue]
		public static JobHandle ScheduleParallel<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00032720 File Offset: 0x00030920
		[MustUseReturnValue]
		public static JobHandle ScheduleParallelByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000326DA File Offset: 0x000308DA
		public static void ScheduleParallel<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x000326DA File Offset: 0x000308DA
		public static void ScheduleParallelByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000326DA File Offset: 0x000308DA
		public static void ScheduleParallel<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000326DA File Offset: 0x000308DA
		public static void ScheduleParallelByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this T jobData, EntityQuery query) where T : struct, ValueType, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000326DA File Offset: 0x000308DA
		public static void Run<T>(this T jobData) where T : struct, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000326DA File Offset: 0x000308DA
		public static void RunByRef<T>(this T jobData) where T : struct, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000326DA File Offset: 0x000308DA
		public static void Run<T>(this T jobData, EntityQuery query) where T : struct, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000326DA File Offset: 0x000308DA
		public static void RunByRef<T>(this T jobData, EntityQuery query) where T : struct, IJobEntity
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
