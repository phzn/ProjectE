using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000049 RID: 73
	[NativeHeader("Modules/Physics/BatchCommands/OverlapBoxCommand.h")]
	public struct OverlapBoxCommand
	{
		// Token: 0x06000535 RID: 1333 RVA: 0x00007883 File Offset: 0x00005A83
		public OverlapBoxCommand(Vector3 center, Vector3 halfExtents, Quaternion orientation, QueryParameters queryParameters)
		{
			this.center = center;
			this.halfExtents = halfExtents;
			this.orientation = orientation;
			this.queryParameters = queryParameters;
			this.physicsScene = Physics.defaultPhysicsScene;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000078B2 File Offset: 0x00005AB2
		public OverlapBoxCommand(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Quaternion orientation, QueryParameters queryParameters)
		{
			this.physicsScene = physicsScene;
			this.center = center;
			this.halfExtents = halfExtents;
			this.orientation = orientation;
			this.queryParameters = queryParameters;
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x000078DE File Offset: 0x00005ADE
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x000078E6 File Offset: 0x00005AE6
		public Vector3 center { readonly get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000078EF File Offset: 0x00005AEF
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x000078F7 File Offset: 0x00005AF7
		public Vector3 halfExtents { readonly get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00007900 File Offset: 0x00005B00
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00007908 File Offset: 0x00005B08
		public Quaternion orientation { readonly get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00007911 File Offset: 0x00005B11
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x00007919 File Offset: 0x00005B19
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x0600053F RID: 1343 RVA: 0x00007924 File Offset: 0x00005B24
		public static JobHandle ScheduleBatch(NativeArray<OverlapBoxCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
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
					BatchQueryJob<OverlapBoxCommand, ColliderHit> batchQueryJob = new BatchQueryJob<OverlapBoxCommand, ColliderHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<OverlapBoxCommand, ColliderHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<OverlapBoxCommand, ColliderHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = OverlapBoxCommand.ScheduleOverlapBoxBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<OverlapBoxCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<ColliderHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000079CC File Offset: 0x00005BCC
		[FreeFunction("ScheduleOverlapBoxCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleOverlapBoxBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			OverlapBoxCommand.ScheduleOverlapBoxBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000541 RID: 1345
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleOverlapBoxBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x0400012D RID: 301
		public QueryParameters queryParameters;
	}
}
