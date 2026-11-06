using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000047 RID: 71
	[NativeHeader("Modules/Physics/BatchCommands/ClosestPointCommand.h")]
	[NativeHeader("Runtime/Jobs/ScriptBindings/JobsBindingsTypes.h")]
	public struct ClosestPointCommand
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x00007618 File Offset: 0x00005818
		public ClosestPointCommand(Vector3 point, int colliderInstanceID, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.point = point;
			this.colliderInstanceID = colliderInstanceID;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00007645 File Offset: 0x00005845
		public ClosestPointCommand(Vector3 point, Collider collider, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.point = point;
			this.colliderInstanceID = collider.GetInstanceID();
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00007677 File Offset: 0x00005877
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x0000767F File Offset: 0x0000587F
		public Vector3 point { readonly get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00007688 File Offset: 0x00005888
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x00007690 File Offset: 0x00005890
		public int colliderInstanceID { readonly get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00007699 File Offset: 0x00005899
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x000076A1 File Offset: 0x000058A1
		public Vector3 position { readonly get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x000076AA File Offset: 0x000058AA
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x000076B2 File Offset: 0x000058B2
		public Quaternion rotation { readonly get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x000076BB File Offset: 0x000058BB
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x000076C3 File Offset: 0x000058C3
		public Vector3 scale { readonly get; set; }

		// Token: 0x06000527 RID: 1319 RVA: 0x000076CC File Offset: 0x000058CC
		public static JobHandle ScheduleBatch(NativeArray<ClosestPointCommand> commands, NativeArray<Vector3> results, int minCommandsPerJob, JobHandle dependsOn = default(JobHandle))
		{
			BatchQueryJob<ClosestPointCommand, Vector3> batchQueryJob = new BatchQueryJob<ClosestPointCommand, Vector3>(commands, results);
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<ClosestPointCommand, Vector3>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<ClosestPointCommand, Vector3>>.Initialize(), dependsOn, ScheduleMode.Batched);
			return ClosestPointCommand.ScheduleClosestPointCommandBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<ClosestPointCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<Vector3>(results), results.Length, minCommandsPerJob);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00007720 File Offset: 0x00005920
		[FreeFunction("ScheduleClosestPointCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleClosestPointCommandBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob)
		{
			JobHandle jobHandle;
			ClosestPointCommand.ScheduleClosestPointCommandBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000529 RID: 1321
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleClosestPointCommandBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, out JobHandle ret);
	}
}
