using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200004A RID: 74
	[NativeHeader("Modules/Physics/BatchCommands/OverlapCapsuleCommand.h")]
	public struct OverlapCapsuleCommand
	{
		// Token: 0x06000542 RID: 1346 RVA: 0x000079EB File Offset: 0x00005BEB
		public OverlapCapsuleCommand(Vector3 point0, Vector3 point1, float radius, QueryParameters queryParameters)
		{
			this.point0 = point0;
			this.point1 = point1;
			this.radius = radius;
			this.queryParameters = queryParameters;
			this.physicsScene = Physics.defaultPhysicsScene;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00007A1A File Offset: 0x00005C1A
		public OverlapCapsuleCommand(PhysicsScene physicsScene, Vector3 point0, Vector3 point1, float radius, QueryParameters queryParameters)
		{
			this.physicsScene = physicsScene;
			this.point0 = point0;
			this.point1 = point1;
			this.radius = radius;
			this.queryParameters = queryParameters;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00007A46 File Offset: 0x00005C46
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00007A4E File Offset: 0x00005C4E
		public Vector3 point0 { readonly get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00007A57 File Offset: 0x00005C57
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00007A5F File Offset: 0x00005C5F
		public Vector3 point1 { readonly get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00007A68 File Offset: 0x00005C68
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00007A70 File Offset: 0x00005C70
		public float radius { readonly get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00007A79 File Offset: 0x00005C79
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00007A81 File Offset: 0x00005C81
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x0600054C RID: 1356 RVA: 0x00007A8C File Offset: 0x00005C8C
		public static JobHandle ScheduleBatch(NativeArray<OverlapCapsuleCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
		{
			bool flag = maxHits < 1;
			JobHandle jobHandle;
			if (flag)
			{
				Debug.LogWarning("maxHits should be greater than 0.");
				jobHandle = default(JobHandle);
			}
			else
			{
				bool flag2 = results.Length < maxHits * commands.Length;
				if (flag2)
				{
					Debug.LogWarning("The supplied results buffer is too small, there should be at least maxHits space per each command in the batch.");
					jobHandle = default(JobHandle);
				}
				else
				{
					BatchQueryJob<OverlapCapsuleCommand, ColliderHit> batchQueryJob = new BatchQueryJob<OverlapCapsuleCommand, ColliderHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<OverlapCapsuleCommand, ColliderHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<OverlapCapsuleCommand, ColliderHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = OverlapCapsuleCommand.ScheduleOverlapCapsuleBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<OverlapCapsuleCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<ColliderHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00007B34 File Offset: 0x00005D34
		[FreeFunction("ScheduleOverlapCapsuleCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleOverlapCapsuleBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			OverlapCapsuleCommand.ScheduleOverlapCapsuleBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x0600054E RID: 1358
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleOverlapCapsuleBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x04000132 RID: 306
		public QueryParameters queryParameters;
	}
}
