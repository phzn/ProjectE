using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200001F RID: 31
	[StaticAccessor("GetPhysicsManager()", StaticAccessorType.Dot)]
	[NativeHeader("Modules/Physics/PhysicsManager.h")]
	public class Physics
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000125 RID: 293 RVA: 0x00003114 File Offset: 0x00001314
		// (remove) Token: 0x06000126 RID: 294 RVA: 0x00003148 File Offset: 0x00001348
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<PhysicsScene, NativeArray<ModifiableContactPair>> ContactModifyEvent;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000127 RID: 295 RVA: 0x0000317C File Offset: 0x0000137C
		// (remove) Token: 0x06000128 RID: 296 RVA: 0x000031B0 File Offset: 0x000013B0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<PhysicsScene, NativeArray<ModifiableContactPair>> ContactModifyEventCCD;

		// Token: 0x06000129 RID: 297 RVA: 0x000031E4 File Offset: 0x000013E4
		[RequiredByNativeCode]
		private static void OnSceneContactModify(PhysicsScene scene, IntPtr buffer, int count, bool isCCD)
		{
			NativeArray<ModifiableContactPair> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ModifiableContactPair>(buffer.ToPointer(), count, Allocator.None);
			bool flag = !isCCD;
			if (flag)
			{
				Action<PhysicsScene, NativeArray<ModifiableContactPair>> contactModifyEvent = Physics.ContactModifyEvent;
				if (contactModifyEvent != null)
				{
					contactModifyEvent(scene, nativeArray);
				}
			}
			else
			{
				Action<PhysicsScene, NativeArray<ModifiableContactPair>> contactModifyEventCCD = Physics.ContactModifyEventCCD;
				if (contactModifyEventCCD != null)
				{
					contactModifyEventCCD(scene, nativeArray);
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003234 File Offset: 0x00001434
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00003249 File Offset: 0x00001449
		public static Vector3 gravity
		{
			[ThreadSafe]
			get
			{
				Vector3 vector;
				Physics.get_gravity_Injected(out vector);
				return vector;
			}
			set
			{
				Physics.set_gravity_Injected(ref value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600012C RID: 300
		// (set) Token: 0x0600012D RID: 301
		public static extern float defaultContactOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600012E RID: 302
		// (set) Token: 0x0600012F RID: 303
		public static extern float sleepThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000130 RID: 304
		// (set) Token: 0x06000131 RID: 305
		public static extern bool queriesHitTriggers
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000132 RID: 306
		// (set) Token: 0x06000133 RID: 307
		public static extern bool queriesHitBackfaces
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000134 RID: 308
		// (set) Token: 0x06000135 RID: 309
		public static extern float bounceThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000136 RID: 310
		// (set) Token: 0x06000137 RID: 311
		public static extern float defaultMaxDepenetrationVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000138 RID: 312
		// (set) Token: 0x06000139 RID: 313
		public static extern int defaultSolverIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600013A RID: 314
		// (set) Token: 0x0600013B RID: 315
		public static extern int defaultSolverVelocityIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600013C RID: 316
		// (set) Token: 0x0600013D RID: 317
		public static extern SimulationMode simulationMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600013E RID: 318
		// (set) Token: 0x0600013F RID: 319
		public static extern float defaultMaxAngularSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000140 RID: 320
		// (set) Token: 0x06000141 RID: 321
		public static extern bool improvedPatchFriction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000142 RID: 322
		// (set) Token: 0x06000143 RID: 323
		public static extern bool invokeCollisionCallbacks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00003254 File Offset: 0x00001454
		[NativeProperty("DefaultPhysicsSceneHandle", true, TargetType.Function, true)]
		public static PhysicsScene defaultPhysicsScene
		{
			get
			{
				PhysicsScene physicsScene;
				Physics.get_defaultPhysicsScene_Injected(out physicsScene);
				return physicsScene;
			}
		}

		// Token: 0x06000145 RID: 325
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IgnoreCollision([NotNull("NullExceptionObject")] Collider collider1, [NotNull("NullExceptionObject")] Collider collider2, [DefaultValue("true")] bool ignore);

		// Token: 0x06000146 RID: 326 RVA: 0x00003269 File Offset: 0x00001469
		[ExcludeFromDocs]
		public static void IgnoreCollision(Collider collider1, Collider collider2)
		{
			Physics.IgnoreCollision(collider1, collider2, true);
		}

		// Token: 0x06000147 RID: 327
		[NativeName("IgnoreCollision")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IgnoreLayerCollision(int layer1, int layer2, [DefaultValue("true")] bool ignore);

		// Token: 0x06000148 RID: 328 RVA: 0x00003275 File Offset: 0x00001475
		[ExcludeFromDocs]
		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			Physics.IgnoreLayerCollision(layer1, layer2, true);
		}

		// Token: 0x06000149 RID: 329
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIgnoreLayerCollision(int layer1, int layer2);

		// Token: 0x0600014A RID: 330
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIgnoreCollision([NotNull("NullExceptionObject")] Collider collider1, [NotNull("NullExceptionObject")] Collider collider2);

		// Token: 0x0600014B RID: 331 RVA: 0x00003284 File Offset: 0x00001484
		public static bool Raycast(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000032AC File Offset: 0x000014AC
		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000032D0 File Offset: 0x000014D0
		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000032F8 File Offset: 0x000014F8
		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00003324 File Offset: 0x00001524
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000334C File Offset: 0x0000154C
		[RequiredByNativeCode]
		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00003374 File Offset: 0x00001574
		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000339C File Offset: 0x0000159C
		[ExcludeFromDocs]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000033C8 File Offset: 0x000015C8
		public static bool Raycast(Ray ray, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000033F8 File Offset: 0x000015F8
		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, float maxDistance, int layerMask)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00003428 File Offset: 0x00001628
		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, float maxDistance)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000345C File Offset: 0x0000165C
		[ExcludeFromDocs]
		public static bool Raycast(Ray ray)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003494 File Offset: 0x00001694
		public static bool Raycast(Ray ray, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000034C8 File Offset: 0x000016C8
		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			return Physics.Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000034F4 File Offset: 0x000016F4
		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003528 File Offset: 0x00001728
		[ExcludeFromDocs]
		public static bool Raycast(Ray ray, out RaycastHit hitInfo)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00003560 File Offset: 0x00001760
		public static bool Linecast(Vector3 start, Vector3 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			Vector3 vector = end - start;
			return Physics.defaultPhysicsScene.Raycast(start, vector, vector.magnitude, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00003594 File Offset: 0x00001794
		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end, int layerMask)
		{
			return Physics.Linecast(start, end, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000035B0 File Offset: 0x000017B0
		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end)
		{
			return Physics.Linecast(start, end, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000035CC File Offset: 0x000017CC
		public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			Vector3 vector = end - start;
			return Physics.defaultPhysicsScene.Raycast(start, vector, out hitInfo, vector.magnitude, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00003600 File Offset: 0x00001800
		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask)
		{
			return Physics.Linecast(start, end, out hitInfo, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000361C File Offset: 0x0000181C
		[ExcludeFromDocs]
		public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo)
		{
			return Physics.Linecast(start, end, out hitInfo, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000363C File Offset: 0x0000183C
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			RaycastHit raycastHit;
			return Physics.defaultPhysicsScene.CapsuleCast(point1, point2, radius, direction, out raycastHit, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003668 File Offset: 0x00001868
		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, int layerMask)
		{
			return Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003688 File Offset: 0x00001888
		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
		{
			return Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000036A8 File Offset: 0x000018A8
		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction)
		{
			return Physics.CapsuleCast(point1, point2, radius, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000036CC File Offset: 0x000018CC
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000036F8 File Offset: 0x000018F8
		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			return Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000371C File Offset: 0x0000191C
		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			return Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003740 File Offset: 0x00001940
		[ExcludeFromDocs]
		public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo)
		{
			return Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003768 File Offset: 0x00001968
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003794 File Offset: 0x00001994
		[ExcludeFromDocs]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			return Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000037B4 File Offset: 0x000019B4
		[ExcludeFromDocs]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			return Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000037D4 File Offset: 0x000019D4
		[ExcludeFromDocs]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo)
		{
			return Physics.SphereCast(origin, radius, direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000037F8 File Offset: 0x000019F8
		public static bool SphereCast(Ray ray, float radius, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			RaycastHit raycastHit;
			return Physics.SphereCast(ray.origin, radius, ray.direction, out raycastHit, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003824 File Offset: 0x00001A24
		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, float maxDistance, int layerMask)
		{
			return Physics.SphereCast(ray, radius, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00003840 File Offset: 0x00001A40
		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, float maxDistance)
		{
			return Physics.SphereCast(ray, radius, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00003860 File Offset: 0x00001A60
		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius)
		{
			return Physics.SphereCast(ray, radius, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00003884 File Offset: 0x00001A84
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.SphereCast(ray.origin, radius, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000038B0 File Offset: 0x00001AB0
		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance, int layerMask)
		{
			return Physics.SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000038D0 File Offset: 0x00001AD0
		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance)
		{
			return Physics.SphereCast(ray, radius, out hitInfo, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000038F0 File Offset: 0x00001AF0
		[ExcludeFromDocs]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo)
		{
			return Physics.SphereCast(ray, radius, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00003914 File Offset: 0x00001B14
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			RaycastHit raycastHit;
			return Physics.defaultPhysicsScene.BoxCast(center, halfExtents, direction, out raycastHit, orientation, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00003940 File Offset: 0x00001B40
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, int layerMask)
		{
			return Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00003960 File Offset: 0x00001B60
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
		{
			return Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00003980 File Offset: 0x00001B80
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation)
		{
			return Physics.BoxCast(center, halfExtents, direction, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000039A4 File Offset: 0x00001BA4
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction)
		{
			return Physics.BoxCast(center, halfExtents, direction, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000039CC File Offset: 0x00001BCC
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000039F8 File Offset: 0x00001BF8
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation, float maxDistance, int layerMask)
		{
			return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00003A1C File Offset: 0x00001C1C
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation, float maxDistance)
		{
			return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00003A40 File Offset: 0x00001C40
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation)
		{
			return Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00003A68 File Offset: 0x00001C68
		[ExcludeFromDocs]
		public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo)
		{
			return Physics.BoxCast(center, halfExtents, direction, out hitInfo, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00003A90 File Offset: 0x00001C90
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("RaycastAll")]
		private static RaycastHit[] Internal_RaycastAll(PhysicsScene physicsScene, Ray ray, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.Internal_RaycastAll_Injected(ref physicsScene, ref ray, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00003AA0 File Offset: 0x00001CA0
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			RaycastHit[] array;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				Ray ray = new Ray(origin, vector);
				array = Physics.Internal_RaycastAll(Physics.defaultPhysicsScene, ray, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				array = new RaycastHit[0];
			}
			return array;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00003AF4 File Offset: 0x00001CF4
		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
		{
			return Physics.RaycastAll(origin, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00003B10 File Offset: 0x00001D10
		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance)
		{
			return Physics.RaycastAll(origin, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00003B30 File Offset: 0x00001D30
		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction)
		{
			return Physics.RaycastAll(origin, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00003B54 File Offset: 0x00001D54
		public static RaycastHit[] RaycastAll(Ray ray, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00003B7C File Offset: 0x00001D7C
		[RequiredByNativeCode]
		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask)
		{
			return Physics.RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00003BA4 File Offset: 0x00001DA4
		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Ray ray, float maxDistance)
		{
			return Physics.RaycastAll(ray.origin, ray.direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00003BD0 File Offset: 0x00001DD0
		[ExcludeFromDocs]
		public static RaycastHit[] RaycastAll(Ray ray)
		{
			return Physics.RaycastAll(ray.origin, ray.direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00003C00 File Offset: 0x00001E00
		public static int RaycastNonAlloc(Ray ray, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00003C34 File Offset: 0x00001E34
		[RequiredByNativeCode]
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Ray ray, RaycastHit[] results, float maxDistance, int layerMask)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00003C68 File Offset: 0x00001E68
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Ray ray, RaycastHit[] results, float maxDistance)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00003C9C File Offset: 0x00001E9C
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Ray ray, RaycastHit[] results)
		{
			return Physics.defaultPhysicsScene.Raycast(ray.origin, ray.direction, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00003CD4 File Offset: 0x00001ED4
		public static int RaycastNonAlloc(Vector3 origin, Vector3 direction, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, results, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00003CFC File Offset: 0x00001EFC
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector3 origin, Vector3 direction, RaycastHit[] results, float maxDistance, int layerMask)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00003D24 File Offset: 0x00001F24
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector3 origin, Vector3 direction, RaycastHit[] results, float maxDistance)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00003D4C File Offset: 0x00001F4C
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector3 origin, Vector3 direction, RaycastHit[] results)
		{
			return Physics.defaultPhysicsScene.Raycast(origin, direction, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00003D76 File Offset: 0x00001F76
		[NativeName("CapsuleCastAll")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		private static RaycastHit[] Query_CapsuleCastAll(PhysicsScene physicsScene, Vector3 p0, Vector3 p1, float radius, Vector3 direction, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.Query_CapsuleCastAll_Injected(ref physicsScene, ref p0, ref p1, radius, ref direction, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00003D8C File Offset: 0x00001F8C
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			RaycastHit[] array;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				array = Physics.Query_CapsuleCastAll(Physics.defaultPhysicsScene, point1, point2, radius, vector, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				array = new RaycastHit[0];
			}
			return array;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00003DDC File Offset: 0x00001FDC
		[ExcludeFromDocs]
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, int layerMask)
		{
			return Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00003DFC File Offset: 0x00001FFC
		[ExcludeFromDocs]
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance)
		{
			return Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00003E1C File Offset: 0x0000201C
		[ExcludeFromDocs]
		public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction)
		{
			return Physics.CapsuleCastAll(point1, point2, radius, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00003E3F File Offset: 0x0000203F
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("SphereCastAll")]
		private static RaycastHit[] Query_SphereCastAll(PhysicsScene physicsScene, Vector3 origin, float radius, Vector3 direction, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.Query_SphereCastAll_Injected(ref physicsScene, ref origin, radius, ref direction, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00003E54 File Offset: 0x00002054
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			RaycastHit[] array;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				array = Physics.Query_SphereCastAll(Physics.defaultPhysicsScene, origin, radius, vector, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				array = new RaycastHit[0];
			}
			return array;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00003EA0 File Offset: 0x000020A0
		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance, int layerMask)
		{
			return Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00003EC0 File Offset: 0x000020C0
		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance)
		{
			return Physics.SphereCastAll(origin, radius, direction, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00003EE0 File Offset: 0x000020E0
		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction)
		{
			return Physics.SphereCastAll(origin, radius, direction, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00003F04 File Offset: 0x00002104
		public static RaycastHit[] SphereCastAll(Ray ray, float radius, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.SphereCastAll(ray.origin, radius, ray.direction, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00003F30 File Offset: 0x00002130
		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Ray ray, float radius, float maxDistance, int layerMask)
		{
			return Physics.SphereCastAll(ray, radius, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00003F4C File Offset: 0x0000214C
		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Ray ray, float radius, float maxDistance)
		{
			return Physics.SphereCastAll(ray, radius, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00003F6C File Offset: 0x0000216C
		[ExcludeFromDocs]
		public static RaycastHit[] SphereCastAll(Ray ray, float radius)
		{
			return Physics.SphereCastAll(ray, radius, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00003F8D File Offset: 0x0000218D
		[NativeName("OverlapCapsule")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		private static Collider[] OverlapCapsule_Internal(PhysicsScene physicsScene, Vector3 point0, Vector3 point1, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.OverlapCapsule_Internal_Injected(ref physicsScene, ref point0, ref point1, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00003FA0 File Offset: 0x000021A0
		public static Collider[] OverlapCapsule(Vector3 point0, Vector3 point1, float radius, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.OverlapCapsule_Internal(Physics.defaultPhysicsScene, point0, point1, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00003FC4 File Offset: 0x000021C4
		[ExcludeFromDocs]
		public static Collider[] OverlapCapsule(Vector3 point0, Vector3 point1, float radius, int layerMask)
		{
			return Physics.OverlapCapsule(point0, point1, radius, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00003FE0 File Offset: 0x000021E0
		[ExcludeFromDocs]
		public static Collider[] OverlapCapsule(Vector3 point0, Vector3 point1, float radius)
		{
			return Physics.OverlapCapsule(point0, point1, radius, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00003FFC File Offset: 0x000021FC
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("OverlapSphere")]
		private static Collider[] OverlapSphere_Internal(PhysicsScene physicsScene, Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.OverlapSphere_Internal_Injected(ref physicsScene, ref position, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000400C File Offset: 0x0000220C
		public static Collider[] OverlapSphere(Vector3 position, float radius, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.OverlapSphere_Internal(Physics.defaultPhysicsScene, position, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000402C File Offset: 0x0000222C
		[ExcludeFromDocs]
		public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask)
		{
			return Physics.OverlapSphere(position, radius, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004048 File Offset: 0x00002248
		[ExcludeFromDocs]
		public static Collider[] OverlapSphere(Vector3 position, float radius)
		{
			return Physics.OverlapSphere(position, radius, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00004063 File Offset: 0x00002263
		[NativeName("Simulate")]
		internal static void Simulate_Internal(PhysicsScene physicsScene, float step)
		{
			Physics.Simulate_Internal_Injected(ref physicsScene, step);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00004070 File Offset: 0x00002270
		public static void Simulate(float step)
		{
			bool flag = Physics.simulationMode != SimulationMode.Script;
			if (flag)
			{
				Debug.LogWarning("Physics.Simulate(...) was called but simulation mode is not set to Script. You should set simulation mode to Script first before calling this function therefore the simulation was not run.");
			}
			else
			{
				Physics.Simulate_Internal(Physics.defaultPhysicsScene, step);
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000040A7 File Offset: 0x000022A7
		[NativeName("InterpolateBodies")]
		internal static void InterpolateBodies_Internal(PhysicsScene physicsScene)
		{
			Physics.InterpolateBodies_Internal_Injected(ref physicsScene);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000040B0 File Offset: 0x000022B0
		[NativeName("ResetInterpolatedTransformPosition")]
		internal static void ResetInterpolationPoses_Internal(PhysicsScene physicsScene)
		{
			Physics.ResetInterpolationPoses_Internal_Injected(ref physicsScene);
		}

		// Token: 0x060001AA RID: 426
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SyncTransforms();

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AB RID: 427
		// (set) Token: 0x060001AC RID: 428
		public static extern bool autoSyncTransforms
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001AD RID: 429
		// (set) Token: 0x060001AE RID: 430
		public static extern bool reuseCollisionCallbacks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000040B9 File Offset: 0x000022B9
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("ComputePenetration")]
		private static bool Query_ComputePenetration([NotNull("ArgumentNullException")] Collider colliderA, Vector3 positionA, Quaternion rotationA, [NotNull("ArgumentNullException")] Collider colliderB, Vector3 positionB, Quaternion rotationB, ref Vector3 direction, ref float distance)
		{
			return Physics.Query_ComputePenetration_Injected(colliderA, ref positionA, ref rotationA, colliderB, ref positionB, ref rotationB, ref direction, ref distance);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000040D0 File Offset: 0x000022D0
		public static bool ComputePenetration(Collider colliderA, Vector3 positionA, Quaternion rotationA, Collider colliderB, Vector3 positionB, Quaternion rotationB, out Vector3 direction, out float distance)
		{
			direction = Vector3.zero;
			distance = 0f;
			return Physics.Query_ComputePenetration(colliderA, positionA, rotationA, colliderB, positionB, rotationB, ref direction, ref distance);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00004108 File Offset: 0x00002308
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("ClosestPoint")]
		private static Vector3 Query_ClosestPoint([NotNull("ArgumentNullException")] Collider collider, Vector3 position, Quaternion rotation, Vector3 point)
		{
			Vector3 vector;
			Physics.Query_ClosestPoint_Injected(collider, ref position, ref rotation, ref point, out vector);
			return vector;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00004124 File Offset: 0x00002324
		public static Vector3 ClosestPoint(Vector3 point, Collider collider, Vector3 position, Quaternion rotation)
		{
			return Physics.Query_ClosestPoint(collider, position, rotation, point);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001B3 RID: 435
		// (set) Token: 0x060001B4 RID: 436
		[StaticAccessor("GetPhysicsManager()")]
		public static extern float interCollisionDistance
		{
			[NativeName("GetClothInterCollisionDistance")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("SetClothInterCollisionDistance")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B5 RID: 437
		// (set) Token: 0x060001B6 RID: 438
		[StaticAccessor("GetPhysicsManager()")]
		public static extern float interCollisionStiffness
		{
			[NativeName("GetClothInterCollisionStiffness")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("SetClothInterCollisionStiffness")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001B7 RID: 439
		// (set) Token: 0x060001B8 RID: 440
		[StaticAccessor("GetPhysicsManager()")]
		public static extern bool interCollisionSettingsToggle
		{
			[NativeName("GetClothInterCollisionSettingsToggle")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("SetClothInterCollisionSettingsToggle")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00004140 File Offset: 0x00002340
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00004155 File Offset: 0x00002355
		public static Vector3 clothGravity
		{
			[ThreadSafe]
			get
			{
				Vector3 vector;
				Physics.get_clothGravity_Injected(out vector);
				return vector;
			}
			set
			{
				Physics.set_clothGravity_Injected(ref value);
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00004160 File Offset: 0x00002360
		public static int OverlapSphereNonAlloc(Vector3 position, float radius, Collider[] results, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.OverlapSphere(position, radius, results, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00004188 File Offset: 0x00002388
		[ExcludeFromDocs]
		public static int OverlapSphereNonAlloc(Vector3 position, float radius, Collider[] results, int layerMask)
		{
			return Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000041A4 File Offset: 0x000023A4
		[ExcludeFromDocs]
		public static int OverlapSphereNonAlloc(Vector3 position, float radius, Collider[] results)
		{
			return Physics.OverlapSphereNonAlloc(position, radius, results, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000041C0 File Offset: 0x000023C0
		[NativeName("SphereTest")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static bool CheckSphere_Internal(PhysicsScene physicsScene, Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.CheckSphere_Internal_Injected(ref physicsScene, ref position, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000041D0 File Offset: 0x000023D0
		public static bool CheckSphere(Vector3 position, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.CheckSphere_Internal(Physics.defaultPhysicsScene, position, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000041F0 File Offset: 0x000023F0
		[ExcludeFromDocs]
		public static bool CheckSphere(Vector3 position, float radius, int layerMask)
		{
			return Physics.CheckSphere(position, radius, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000420C File Offset: 0x0000240C
		[ExcludeFromDocs]
		public static bool CheckSphere(Vector3 position, float radius)
		{
			return Physics.CheckSphere(position, radius, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00004228 File Offset: 0x00002428
		public static int CapsuleCastNonAlloc(Vector3 point1, Vector3 point2, float radius, Vector3 direction, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.CapsuleCast(point1, point2, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00004254 File Offset: 0x00002454
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector3 point1, Vector3 point2, float radius, Vector3 direction, RaycastHit[] results, float maxDistance, int layerMask)
		{
			return Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00004278 File Offset: 0x00002478
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector3 point1, Vector3 point2, float radius, Vector3 direction, RaycastHit[] results, float maxDistance)
		{
			return Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000429C File Offset: 0x0000249C
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector3 point1, Vector3 point2, float radius, Vector3 direction, RaycastHit[] results)
		{
			return Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000042C4 File Offset: 0x000024C4
		public static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.SphereCast(origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000042F0 File Offset: 0x000024F0
		[ExcludeFromDocs]
		public static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, float maxDistance, int layerMask)
		{
			return Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00004310 File Offset: 0x00002510
		[ExcludeFromDocs]
		public static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, float maxDistance)
		{
			return Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00004330 File Offset: 0x00002530
		[ExcludeFromDocs]
		public static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results)
		{
			return Physics.SphereCastNonAlloc(origin, radius, direction, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00004354 File Offset: 0x00002554
		public static int SphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.SphereCastNonAlloc(ray.origin, radius, ray.direction, results, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00004380 File Offset: 0x00002580
		[ExcludeFromDocs]
		public static int SphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results, float maxDistance, int layerMask)
		{
			return Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000043A0 File Offset: 0x000025A0
		[ExcludeFromDocs]
		public static int SphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results, float maxDistance)
		{
			return Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000043C0 File Offset: 0x000025C0
		[ExcludeFromDocs]
		public static int SphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results)
		{
			return Physics.SphereCastNonAlloc(ray, radius, results, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000043E2 File Offset: 0x000025E2
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("CapsuleTest")]
		private static bool CheckCapsule_Internal(PhysicsScene physicsScene, Vector3 start, Vector3 end, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.CheckCapsule_Internal_Injected(ref physicsScene, ref start, ref end, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000043F4 File Offset: 0x000025F4
		public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.CheckCapsule_Internal(Physics.defaultPhysicsScene, start, end, radius, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00004418 File Offset: 0x00002618
		[ExcludeFromDocs]
		public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, int layerMask)
		{
			return Physics.CheckCapsule(start, end, radius, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00004434 File Offset: 0x00002634
		[ExcludeFromDocs]
		public static bool CheckCapsule(Vector3 start, Vector3 end, float radius)
		{
			return Physics.CheckCapsule(start, end, radius, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00004451 File Offset: 0x00002651
		[NativeName("BoxTest")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static bool CheckBox_Internal(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Quaternion orientation, int layermask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.CheckBox_Internal_Injected(ref physicsScene, ref center, ref halfExtents, ref orientation, layermask, queryTriggerInteraction);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00004464 File Offset: 0x00002664
		public static bool CheckBox(Vector3 center, Vector3 halfExtents, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("DefaultRaycastLayers")] int layermask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.CheckBox_Internal(Physics.defaultPhysicsScene, center, halfExtents, orientation, layermask, queryTriggerInteraction);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00004488 File Offset: 0x00002688
		[ExcludeFromDocs]
		public static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask)
		{
			return Physics.CheckBox(center, halfExtents, orientation, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000044A4 File Offset: 0x000026A4
		[ExcludeFromDocs]
		public static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
		{
			return Physics.CheckBox(center, halfExtents, orientation, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000044C4 File Offset: 0x000026C4
		[ExcludeFromDocs]
		public static bool CheckBox(Vector3 center, Vector3 halfExtents)
		{
			return Physics.CheckBox(center, halfExtents, Quaternion.identity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000044E5 File Offset: 0x000026E5
		[NativeName("OverlapBox")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static Collider[] OverlapBox_Internal(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.OverlapBox_Internal_Injected(ref physicsScene, ref center, ref halfExtents, ref orientation, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000044F8 File Offset: 0x000026F8
		public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.OverlapBox_Internal(Physics.defaultPhysicsScene, center, halfExtents, orientation, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000451C File Offset: 0x0000271C
		[ExcludeFromDocs]
		public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask)
		{
			return Physics.OverlapBox(center, halfExtents, orientation, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00004538 File Offset: 0x00002738
		[ExcludeFromDocs]
		public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
		{
			return Physics.OverlapBox(center, halfExtents, orientation, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00004554 File Offset: 0x00002754
		[ExcludeFromDocs]
		public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents)
		{
			return Physics.OverlapBox(center, halfExtents, Quaternion.identity, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00004574 File Offset: 0x00002774
		public static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Collider[] results, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("AllLayers")] int mask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.OverlapBox(center, halfExtents, results, orientation, mask, queryTriggerInteraction);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000459C File Offset: 0x0000279C
		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Collider[] results, Quaternion orientation, int mask)
		{
			return Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, mask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000045BC File Offset: 0x000027BC
		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Collider[] results, Quaternion orientation)
		{
			return Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000045DC File Offset: 0x000027DC
		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Collider[] results)
		{
			return Physics.OverlapBoxNonAlloc(center, halfExtents, results, Quaternion.identity, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00004600 File Offset: 0x00002800
		public static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.BoxCast(center, halfExtents, direction, results, orientation, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000462C File Offset: 0x0000282C
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results, Quaternion orientation)
		{
			return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00004654 File Offset: 0x00002854
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results, Quaternion orientation, float maxDistance)
		{
			return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00004678 File Offset: 0x00002878
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results, Quaternion orientation, float maxDistance, int layerMask)
		{
			return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000469C File Offset: 0x0000289C
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results)
		{
			return Physics.BoxCastNonAlloc(center, halfExtents, direction, results, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000046C4 File Offset: 0x000028C4
		[NativeName("BoxCastAll")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static RaycastHit[] Internal_BoxCastAll(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.Internal_BoxCastAll_Injected(ref physicsScene, ref center, ref halfExtents, ref direction, ref orientation, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000046DC File Offset: 0x000028DC
		public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			RaycastHit[] array;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				array = Physics.Internal_BoxCastAll(Physics.defaultPhysicsScene, center, halfExtents, vector, orientation, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				array = new RaycastHit[0];
			}
			return array;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000472C File Offset: 0x0000292C
		[ExcludeFromDocs]
		public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, int layerMask)
		{
			return Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000474C File Offset: 0x0000294C
		[ExcludeFromDocs]
		public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance)
		{
			return Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000476C File Offset: 0x0000296C
		[ExcludeFromDocs]
		public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation)
		{
			return Physics.BoxCastAll(center, halfExtents, direction, orientation, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00004790 File Offset: 0x00002990
		[ExcludeFromDocs]
		public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction)
		{
			return Physics.BoxCastAll(center, halfExtents, direction, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000047B8 File Offset: 0x000029B8
		public static int OverlapCapsuleNonAlloc(Vector3 point0, Vector3 point1, float radius, Collider[] results, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Physics.defaultPhysicsScene.OverlapCapsule(point0, point1, radius, results, layerMask, queryTriggerInteraction);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000047E0 File Offset: 0x000029E0
		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector3 point0, Vector3 point1, float radius, Collider[] results, int layerMask)
		{
			return Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00004800 File Offset: 0x00002A00
		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector3 point0, Vector3 point1, float radius, Collider[] results)
		{
			return Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, -1, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000481D File Offset: 0x00002A1D
		[NativeName("RebuildBroadphaseRegions")]
		[StaticAccessor("GetPhysicsManager()")]
		private static void Internal_RebuildBroadphaseRegions(Bounds bounds, int subdivisions)
		{
			Physics.Internal_RebuildBroadphaseRegions_Injected(ref bounds, subdivisions);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00004828 File Offset: 0x00002A28
		public static void RebuildBroadphaseRegions(Bounds worldBounds, int subdivisions)
		{
			bool flag = subdivisions < 1 || subdivisions > 16;
			if (flag)
			{
				throw new ArgumentException("Physics.RebuildBroadphaseRegions requires the subdivisions to be greater than zero and less than 17.");
			}
			bool flag2 = worldBounds.extents.x <= 0f || worldBounds.extents.y <= 0f || worldBounds.extents.z <= 0f;
			if (flag2)
			{
				throw new ArgumentException("Physics.RebuildBroadphaseRegions requires the world bounds to be non-empty, and have positive extents.");
			}
			Physics.Internal_RebuildBroadphaseRegions(worldBounds, subdivisions);
		}

		// Token: 0x060001F0 RID: 496
		[ThreadSafe]
		[StaticAccessor("GetPhysicsManager()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BakeMesh(int meshID, bool convex, MeshColliderCookingOptions cookingOptions);

		// Token: 0x060001F1 RID: 497 RVA: 0x000048A9 File Offset: 0x00002AA9
		public static void BakeMesh(int meshID, bool convex)
		{
			Physics.BakeMesh(meshID, convex, MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase);
		}

		// Token: 0x060001F2 RID: 498
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Collider ResolveShapeToCollider(IntPtr shapePtr);

		// Token: 0x060001F3 RID: 499
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Component ResolveActorToComponent(IntPtr actorPtr);

		// Token: 0x060001F4 RID: 500
		[ThreadSafe]
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ResolveShapeToInstanceID(IntPtr shapePtr);

		// Token: 0x060001F5 RID: 501
		[ThreadSafe]
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ResolveActorToInstanceID(IntPtr actorPtr);

		// Token: 0x060001F6 RID: 502
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Collider GetColliderByInstanceID(int instanceID);

		// Token: 0x060001F7 RID: 503
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Component GetBodyByInstanceID(int instanceID);

		// Token: 0x060001F8 RID: 504
		[ThreadSafe]
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint TranslateTriangleIndex(IntPtr shapePtr, uint rawIndex);

		// Token: 0x060001F9 RID: 505
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint TranslateTriangleIndexFromID(int instanceID, uint faceIndex);

		// Token: 0x060001FA RID: 506
		[ThreadSafe]
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsShapeTrigger(IntPtr shapePtr);

		// Token: 0x060001FB RID: 507
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SendOnCollisionEnter(Component component, Collision collision);

		// Token: 0x060001FC RID: 508
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SendOnCollisionStay(Component component, Collision collision);

		// Token: 0x060001FD RID: 509
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SendOnCollisionExit(Component component, Collision collision);

		// Token: 0x060001FE RID: 510 RVA: 0x000048B8 File Offset: 0x00002AB8
		[ThreadSafe]
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		internal static Vector3 GetActorLinearVelocity(IntPtr actorPtr)
		{
			Vector3 vector;
			Physics.GetActorLinearVelocity_Injected(actorPtr, out vector);
			return vector;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000048D0 File Offset: 0x00002AD0
		[StaticAccessor("PhysicsManager", StaticAccessorType.DoubleColon)]
		[ThreadSafe]
		internal static Vector3 GetActorAngularVelocity(IntPtr actorPtr)
		{
			Vector3 vector;
			Physics.GetActorAngularVelocity_Injected(actorPtr, out vector);
			return vector;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000048E8 File Offset: 0x00002AE8
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("Use Physics.defaultContactOffset or Collider.contactOffset instead.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static float minPenetrationForPenalty
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00004900 File Offset: 0x00002B00
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00004917 File Offset: 0x00002B17
		[Obsolete("Please use bounceThreshold instead. (UnityUpgradable) -> bounceThreshold")]
		public static float bounceTreshold
		{
			get
			{
				return Physics.bounceThreshold;
			}
			set
			{
				Physics.bounceThreshold = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00004924 File Offset: 0x00002B24
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("The sleepVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static float sleepVelocity
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000493C File Offset: 0x00002B3C
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The sleepAngularVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.", true)]
		public static float sleepAngularVelocity
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00004954 File Offset: 0x00002B54
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Rigidbody.maxAngularVelocity instead.", true)]
		public static float maxAngularVelocity
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000496C File Offset: 0x00002B6C
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00004983 File Offset: 0x00002B83
		[Obsolete("Please use Physics.defaultSolverIterations instead. (UnityUpgradable) -> defaultSolverIterations")]
		public static int solverIterationCount
		{
			get
			{
				return Physics.defaultSolverIterations;
			}
			set
			{
				Physics.defaultSolverIterations = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00004990 File Offset: 0x00002B90
		// (set) Token: 0x0600020D RID: 525 RVA: 0x000049A7 File Offset: 0x00002BA7
		[Obsolete("Please use Physics.defaultSolverVelocityIterations instead. (UnityUpgradable) -> defaultSolverVelocityIterations")]
		public static int solverVelocityIterationCount
		{
			get
			{
				return Physics.defaultSolverVelocityIterations;
			}
			set
			{
				Physics.defaultSolverVelocityIterations = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000049B4 File Offset: 0x00002BB4
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("penetrationPenaltyForce has no effect.", true)]
		public static float penetrationPenaltyForce
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000210 RID: 528 RVA: 0x000049CC File Offset: 0x00002BCC
		// (set) Token: 0x06000211 RID: 529 RVA: 0x000049E9 File Offset: 0x00002BE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Physics.autoSimulation has been replaced by Physics.simulationMode", false)]
		public static bool autoSimulation
		{
			get
			{
				return Physics.simulationMode != SimulationMode.Script;
			}
			set
			{
				Physics.simulationMode = (value ? SimulationMode.FixedUpdate : SimulationMode.Script);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000212 RID: 530 RVA: 0x000049FC File Offset: 0x00002BFC
		// (remove) Token: 0x06000213 RID: 531 RVA: 0x00004A30 File Offset: 0x00002C30
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Physics.ContactEventDelegate ContactEvent;

		// Token: 0x06000214 RID: 532 RVA: 0x00004A64 File Offset: 0x00002C64
		[RequiredByNativeCode]
		private static void OnSceneContact(PhysicsScene scene, IntPtr buffer, int count)
		{
			bool flag = count == 0;
			if (!flag)
			{
				NativeArray<ContactPairHeader> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ContactPairHeader>(buffer.ToPointer(), count, Allocator.None);
				try
				{
					Physics.ContactEventDelegate contactEvent = Physics.ContactEvent;
					if (contactEvent != null)
					{
						contactEvent(scene, nativeArray.AsReadOnly());
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
				finally
				{
					Physics.ReportContacts(nativeArray.AsReadOnly());
				}
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00004AE4 File Offset: 0x00002CE4
		private static void ReportContacts(NativeArray<ContactPairHeader>.ReadOnly array)
		{
			bool flag = !Physics.invokeCollisionCallbacks;
			if (!flag)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ContactPairHeader contactPairHeader = array[i];
					bool hasRemovedBody = contactPairHeader.HasRemovedBody;
					if (!hasRemovedBody)
					{
						int num = 0;
						while ((long)num < (long)((ulong)contactPairHeader.m_NbPairs))
						{
							readonly ref ContactPair contactPair = ref contactPairHeader.GetContactPair(num);
							bool hasRemovedCollider = contactPair.HasRemovedCollider;
							if (!hasRemovedCollider)
							{
								Component body = contactPairHeader.Body;
								Component otherBody = contactPairHeader.OtherBody;
								Component component = ((body != null) ? body : contactPair.Collider);
								Component component2 = ((otherBody != null) ? otherBody : contactPair.OtherCollider);
								bool isCollisionEnter = contactPair.IsCollisionEnter;
								if (isCollisionEnter)
								{
									Physics.SendOnCollisionEnter(component, Physics.GetCollisionToReport(in contactPairHeader, in contactPair, false));
									Physics.SendOnCollisionEnter(component2, Physics.GetCollisionToReport(in contactPairHeader, in contactPair, true));
								}
								bool isCollisionStay = contactPair.IsCollisionStay;
								if (isCollisionStay)
								{
									Physics.SendOnCollisionStay(component, Physics.GetCollisionToReport(in contactPairHeader, in contactPair, false));
									Physics.SendOnCollisionStay(component2, Physics.GetCollisionToReport(in contactPairHeader, in contactPair, true));
								}
								bool isCollisionExit = contactPair.IsCollisionExit;
								if (isCollisionExit)
								{
									Physics.SendOnCollisionExit(component, Physics.GetCollisionToReport(in contactPairHeader, in contactPair, false));
									Physics.SendOnCollisionExit(component2, Physics.GetCollisionToReport(in contactPairHeader, in contactPair, true));
								}
							}
							num++;
						}
					}
				}
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00004C58 File Offset: 0x00002E58
		private static Collision GetCollisionToReport(in ContactPairHeader header, in ContactPair pair, bool flipped)
		{
			bool reuseCollisionCallbacks = Physics.reuseCollisionCallbacks;
			Collision collision;
			if (reuseCollisionCallbacks)
			{
				Physics.s_ReusableCollision.Reuse(in header, in pair);
				Physics.s_ReusableCollision.Flipped = flipped;
				collision = Physics.s_ReusableCollision;
			}
			else
			{
				collision = new Collision(in header, in pair, flipped);
			}
			return collision;
		}

		// Token: 0x06000219 RID: 537
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_gravity_Injected(out Vector3 ret);

		// Token: 0x0600021A RID: 538
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_gravity_Injected(ref Vector3 value);

		// Token: 0x0600021B RID: 539
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_defaultPhysicsScene_Injected(out PhysicsScene ret);

		// Token: 0x0600021C RID: 540
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit[] Internal_RaycastAll_Injected(ref PhysicsScene physicsScene, ref Ray ray, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600021D RID: 541
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit[] Query_CapsuleCastAll_Injected(ref PhysicsScene physicsScene, ref Vector3 p0, ref Vector3 p1, float radius, ref Vector3 direction, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600021E RID: 542
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit[] Query_SphereCastAll_Injected(ref PhysicsScene physicsScene, ref Vector3 origin, float radius, ref Vector3 direction, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600021F RID: 543
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider[] OverlapCapsule_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 point0, ref Vector3 point1, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x06000220 RID: 544
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider[] OverlapSphere_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x06000221 RID: 545
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Simulate_Internal_Injected(ref PhysicsScene physicsScene, float step);

		// Token: 0x06000222 RID: 546
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InterpolateBodies_Internal_Injected(ref PhysicsScene physicsScene);

		// Token: 0x06000223 RID: 547
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetInterpolationPoses_Internal_Injected(ref PhysicsScene physicsScene);

		// Token: 0x06000224 RID: 548
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Query_ComputePenetration_Injected(Collider colliderA, ref Vector3 positionA, ref Quaternion rotationA, Collider colliderB, ref Vector3 positionB, ref Quaternion rotationB, ref Vector3 direction, ref float distance);

		// Token: 0x06000225 RID: 549
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Query_ClosestPoint_Injected(Collider collider, ref Vector3 position, ref Quaternion rotation, ref Vector3 point, out Vector3 ret);

		// Token: 0x06000226 RID: 550
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_clothGravity_Injected(out Vector3 ret);

		// Token: 0x06000227 RID: 551
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_clothGravity_Injected(ref Vector3 value);

		// Token: 0x06000228 RID: 552
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CheckSphere_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x06000229 RID: 553
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CheckCapsule_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 start, ref Vector3 end, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600022A RID: 554
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CheckBox_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 center, ref Vector3 halfExtents, ref Quaternion orientation, int layermask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600022B RID: 555
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider[] OverlapBox_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 center, ref Vector3 halfExtents, ref Quaternion orientation, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600022C RID: 556
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit[] Internal_BoxCastAll_Injected(ref PhysicsScene physicsScene, ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, ref Quaternion orientation, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600022D RID: 557
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RebuildBroadphaseRegions_Injected(ref Bounds bounds, int subdivisions);

		// Token: 0x0600022E RID: 558
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetActorLinearVelocity_Injected(IntPtr actorPtr, out Vector3 ret);

		// Token: 0x0600022F RID: 559
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetActorAngularVelocity_Injected(IntPtr actorPtr, out Vector3 ret);

		// Token: 0x04000085 RID: 133
		internal const float k_MaxFloatMinusEpsilon = 3.4028233E+38f;

		// Token: 0x04000086 RID: 134
		public const int IgnoreRaycastLayer = 4;

		// Token: 0x04000087 RID: 135
		public const int DefaultRaycastLayers = -5;

		// Token: 0x04000088 RID: 136
		public const int AllLayers = -1;

		// Token: 0x04000089 RID: 137
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Please use Physics.IgnoreRaycastLayer instead. (UnityUpgradable) -> IgnoreRaycastLayer", true)]
		public const int kIgnoreRaycastLayer = 4;

		// Token: 0x0400008A RID: 138
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Please use Physics.DefaultRaycastLayers instead. (UnityUpgradable) -> DefaultRaycastLayers", true)]
		public const int kDefaultRaycastLayers = -5;

		// Token: 0x0400008B RID: 139
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Please use Physics.AllLayers instead. (UnityUpgradable) -> AllLayers", true)]
		public const int kAllLayers = -1;

		// Token: 0x0400008D RID: 141
		private static readonly Collision s_ReusableCollision = new Collision();

		// Token: 0x02000020 RID: 32
		// (Invoke) Token: 0x06000231 RID: 561
		public delegate void ContactEventDelegate(PhysicsScene scene, NativeArray<ContactPairHeader>.ReadOnly headerArray);
	}
}
