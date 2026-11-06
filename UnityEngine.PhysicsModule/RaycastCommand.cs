using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000043 RID: 67
	[NativeHeader("Runtime/Jobs/ScriptBindings/JobsBindingsTypes.h")]
	[NativeHeader("Modules/Physics/BatchCommands/RaycastCommand.h")]
	public struct RaycastCommand
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x00006C0F File Offset: 0x00004E0F
		public RaycastCommand(Vector3 from, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.from = from;
			this.direction = direction;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.distance = distance;
			this.queryParameters = queryParameters;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00006C3E File Offset: 0x00004E3E
		public RaycastCommand(PhysicsScene physicsScene, Vector3 from, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.from = from;
			this.direction = direction;
			this.physicsScene = physicsScene;
			this.distance = distance;
			this.queryParameters = queryParameters;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00006C6A File Offset: 0x00004E6A
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00006C72 File Offset: 0x00004E72
		public Vector3 from { readonly get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00006C7B File Offset: 0x00004E7B
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00006C83 File Offset: 0x00004E83
		public Vector3 direction { readonly get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00006C8C File Offset: 0x00004E8C
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00006C94 File Offset: 0x00004E94
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00006C9D File Offset: 0x00004E9D
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00006CA5 File Offset: 0x00004EA5
		public float distance { readonly get; set; }

		// Token: 0x060004D1 RID: 1233 RVA: 0x00006CB0 File Offset: 0x00004EB0
		public static JobHandle ScheduleBatch(NativeArray<RaycastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
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
					BatchQueryJob<RaycastCommand, RaycastHit> batchQueryJob = new BatchQueryJob<RaycastCommand, RaycastHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<RaycastCommand, RaycastHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<RaycastCommand, RaycastHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = RaycastCommand.ScheduleRaycastBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<RaycastCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<RaycastHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00006D58 File Offset: 0x00004F58
		public static JobHandle ScheduleBatch(NativeArray<RaycastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, JobHandle dependsOn = default(JobHandle))
		{
			return RaycastCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependsOn);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00006D74 File Offset: 0x00004F74
		[FreeFunction("ScheduleRaycastCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleRaycastBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			RaycastCommand.ScheduleRaycastBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00006D93 File Offset: 0x00004F93
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public RaycastCommand(Vector3 from, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5, int maxHits = 1)
		{
			this.from = from;
			this.direction = direction;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = QueryParameters.Default;
			this.distance = distance;
			this.layerMask = layerMask;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00006DCE File Offset: 0x00004FCE
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public RaycastCommand(PhysicsScene physicsScene, Vector3 from, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5, int maxHits = 1)
		{
			this.from = from;
			this.direction = direction;
			this.physicsScene = physicsScene;
			this.queryParameters = QueryParameters.Default;
			this.distance = distance;
			this.layerMask = layerMask;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00006E08 File Offset: 0x00005008
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("maxHits property was moved to be a part of RaycastCommand.ScheduleBatch.", false)]
		public int maxHits
		{
			get
			{
				return 1;
			}
			set
			{
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00006E1C File Offset: 0x0000501C
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00006E39 File Offset: 0x00005039
		[Obsolete("Layer Mask is now a part of QueryParameters struct", false)]
		public int layerMask
		{
			get
			{
				return this.queryParameters.layerMask;
			}
			set
			{
				this.queryParameters.layerMask = value;
			}
		}

		// Token: 0x060004DA RID: 1242
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleRaycastBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x0400010B RID: 267
		public QueryParameters queryParameters;
	}
}
