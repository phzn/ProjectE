using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000004 RID: 4
	[NativeHeader("Physics2DScriptingClasses.h")]
	[StaticAccessor("GetPhysicsManager2D()", StaticAccessorType.Arrow)]
	[NativeHeader("Physics2DScriptingClasses.h")]
	[NativeHeader("Modules/Physics2D/PhysicsManager2D.h")]
	public class Physics2D
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002F50 File Offset: 0x00001150
		public static PhysicsScene2D defaultPhysicsScene
		{
			get
			{
				return default(PhysicsScene2D);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600008C RID: 140
		// (set) Token: 0x0600008D RID: 141
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern int velocityIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600008E RID: 142
		// (set) Token: 0x0600008F RID: 143
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern int positionIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002F6C File Offset: 0x0000116C
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00002F81 File Offset: 0x00001181
		[StaticAccessor("GetPhysics2DSettings()")]
		public static Vector2 gravity
		{
			get
			{
				Vector2 vector;
				Physics2D.get_gravity_Injected(out vector);
				return vector;
			}
			set
			{
				Physics2D.set_gravity_Injected(ref value);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000092 RID: 146
		// (set) Token: 0x06000093 RID: 147
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool queriesHitTriggers
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000094 RID: 148
		// (set) Token: 0x06000095 RID: 149
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool queriesStartInColliders
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000096 RID: 150
		// (set) Token: 0x06000097 RID: 151
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool callbacksOnDisable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000098 RID: 152
		// (set) Token: 0x06000099 RID: 153
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool reuseCollisionCallbacks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600009A RID: 154
		// (set) Token: 0x0600009B RID: 155
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool autoSyncTransforms
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600009C RID: 156
		// (set) Token: 0x0600009D RID: 157
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern SimulationMode2D simulationMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00002F8C File Offset: 0x0000118C
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00002FA1 File Offset: 0x000011A1
		[StaticAccessor("GetPhysics2DSettings()")]
		public static PhysicsJobOptions2D jobOptions
		{
			get
			{
				PhysicsJobOptions2D physicsJobOptions2D;
				Physics2D.get_jobOptions_Injected(out physicsJobOptions2D);
				return physicsJobOptions2D;
			}
			set
			{
				Physics2D.set_jobOptions_Injected(ref value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000A0 RID: 160
		// (set) Token: 0x060000A1 RID: 161
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float velocityThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000A2 RID: 162
		// (set) Token: 0x060000A3 RID: 163
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxLinearCorrection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A4 RID: 164
		// (set) Token: 0x060000A5 RID: 165
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxAngularCorrection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000A6 RID: 166
		// (set) Token: 0x060000A7 RID: 167
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxTranslationSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A8 RID: 168
		// (set) Token: 0x060000A9 RID: 169
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxRotationSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000AA RID: 170
		// (set) Token: 0x060000AB RID: 171
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float defaultContactOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000AC RID: 172
		// (set) Token: 0x060000AD RID: 173
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float baumgarteScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000AE RID: 174
		// (set) Token: 0x060000AF RID: 175
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float baumgarteTOIScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B0 RID: 176
		// (set) Token: 0x060000B1 RID: 177
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float timeToSleep
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B2 RID: 178
		// (set) Token: 0x060000B3 RID: 179
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float linearSleepTolerance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B4 RID: 180
		// (set) Token: 0x060000B5 RID: 181
		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float angularSleepTolerance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002FAC File Offset: 0x000011AC
		public static bool Simulate(float step)
		{
			return Physics2D.Simulate_Internal(Physics2D.defaultPhysicsScene, step);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00002FC9 File Offset: 0x000011C9
		[NativeMethod("Simulate_Binding")]
		internal static bool Simulate_Internal(PhysicsScene2D physicsScene, float step)
		{
			return Physics2D.Simulate_Internal_Injected(ref physicsScene, step);
		}

		// Token: 0x060000B8 RID: 184
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SyncTransforms();

		// Token: 0x060000B9 RID: 185 RVA: 0x00002FD3 File Offset: 0x000011D3
		[ExcludeFromDocs]
		public static void IgnoreCollision([Writable] Collider2D collider1, [Writable] Collider2D collider2)
		{
			Physics2D.IgnoreCollision(collider1, collider2, true);
		}

		// Token: 0x060000BA RID: 186
		[NativeMethod("IgnoreCollision_Binding")]
		[StaticAccessor("PhysicsScene2D", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IgnoreCollision([NotNull("ArgumentNullException")] [Writable] Collider2D collider1, [Writable] [NotNull("ArgumentNullException")] Collider2D collider2, [DefaultValue("true")] bool ignore);

		// Token: 0x060000BB RID: 187
		[NativeMethod("GetIgnoreCollision_Binding")]
		[StaticAccessor("PhysicsScene2D", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIgnoreCollision([Writable] [NotNull("ArgumentNullException")] Collider2D collider1, [Writable] [NotNull("ArgumentNullException")] Collider2D collider2);

		// Token: 0x060000BC RID: 188 RVA: 0x00002FDF File Offset: 0x000011DF
		[ExcludeFromDocs]
		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			Physics2D.IgnoreLayerCollision(layer1, layer2, true);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002FEC File Offset: 0x000011EC
		public static void IgnoreLayerCollision(int layer1, int layer2, bool ignore)
		{
			bool flag = layer1 < 0 || layer1 > 31;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			bool flag2 = layer2 < 0 || layer2 > 31;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("layer2 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			Physics2D.IgnoreLayerCollision_Internal(layer1, layer2, ignore);
		}

		// Token: 0x060000BE RID: 190
		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("IgnoreLayerCollision")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void IgnoreLayerCollision_Internal(int layer1, int layer2, bool ignore);

		// Token: 0x060000BF RID: 191 RVA: 0x0000303C File Offset: 0x0000123C
		public static bool GetIgnoreLayerCollision(int layer1, int layer2)
		{
			bool flag = layer1 < 0 || layer1 > 31;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			bool flag2 = layer2 < 0 || layer2 > 31;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("layer2 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			return Physics2D.GetIgnoreLayerCollision_Internal(layer1, layer2);
		}

		// Token: 0x060000C0 RID: 192
		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("GetIgnoreLayerCollision")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIgnoreLayerCollision_Internal(int layer1, int layer2);

		// Token: 0x060000C1 RID: 193 RVA: 0x0000308C File Offset: 0x0000128C
		public static void SetLayerCollisionMask(int layer, int layerMask)
		{
			bool flag = layer < 0 || layer > 31;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			Physics2D.SetLayerCollisionMask_Internal(layer, layerMask);
		}

		// Token: 0x060000C2 RID: 194
		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("SetLayerCollisionMask")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLayerCollisionMask_Internal(int layer, int layerMask);

		// Token: 0x060000C3 RID: 195 RVA: 0x000030C0 File Offset: 0x000012C0
		public static int GetLayerCollisionMask(int layer)
		{
			bool flag = layer < 0 || layer > 31;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			return Physics2D.GetLayerCollisionMask_Internal(layer);
		}

		// Token: 0x060000C4 RID: 196
		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("GetLayerCollisionMask")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLayerCollisionMask_Internal(int layer);

		// Token: 0x060000C5 RID: 197
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTouching([NotNull("ArgumentNullException")] [Writable] Collider2D collider1, [NotNull("ArgumentNullException")] [Writable] Collider2D collider2);

		// Token: 0x060000C6 RID: 198 RVA: 0x000030F4 File Offset: 0x000012F4
		public static bool IsTouching([Writable] Collider2D collider1, [Writable] Collider2D collider2, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_TwoCollidersWithFilter(collider1, collider2, contactFilter);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000310E File Offset: 0x0000130E
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("IsTouching")]
		private static bool IsTouching_TwoCollidersWithFilter([NotNull("ArgumentNullException")] [Writable] Collider2D collider1, [NotNull("ArgumentNullException")] [Writable] Collider2D collider2, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_TwoCollidersWithFilter_Injected(collider1, collider2, ref contactFilter);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000311C File Offset: 0x0000131C
		public static bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_SingleColliderWithFilter(collider, contactFilter);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003135 File Offset: 0x00001335
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("IsTouching")]
		private static bool IsTouching_SingleColliderWithFilter([Writable] [NotNull("ArgumentNullException")] Collider2D collider, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_SingleColliderWithFilter_Injected(collider, ref contactFilter);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003140 File Offset: 0x00001340
		[ExcludeFromDocs]
		public static bool IsTouchingLayers([Writable] Collider2D collider)
		{
			return Physics2D.IsTouchingLayers(collider, -1);
		}

		// Token: 0x060000CB RID: 203
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTouchingLayers([NotNull("ArgumentNullException")] [Writable] Collider2D collider, [DefaultValue("Physics2D.AllLayers")] int layerMask);

		// Token: 0x060000CC RID: 204 RVA: 0x0000315C File Offset: 0x0000135C
		public static ColliderDistance2D Distance([Writable] Collider2D colliderA, [Writable] Collider2D colliderB)
		{
			bool flag = colliderA == null;
			if (flag)
			{
				throw new ArgumentNullException("ColliderA cannot be NULL.");
			}
			bool flag2 = colliderB == null;
			if (flag2)
			{
				throw new ArgumentNullException("ColliderB cannot be NULL.");
			}
			bool flag3 = colliderA == colliderB;
			if (flag3)
			{
				throw new ArgumentException("Cannot calculate the distance between the same collider.");
			}
			return Physics2D.Distance_Internal(colliderA, colliderB);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000031B8 File Offset: 0x000013B8
		[NativeMethod("Distance")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static ColliderDistance2D Distance_Internal([NotNull("ArgumentNullException")] [Writable] Collider2D colliderA, [Writable] [NotNull("ArgumentNullException")] Collider2D colliderB)
		{
			ColliderDistance2D colliderDistance2D;
			Physics2D.Distance_Internal_Injected(colliderA, colliderB, out colliderDistance2D);
			return colliderDistance2D;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000031D0 File Offset: 0x000013D0
		public static Vector2 ClosestPoint(Vector2 position, Collider2D collider)
		{
			bool flag = collider == null;
			if (flag)
			{
				throw new ArgumentNullException("Collider cannot be NULL.");
			}
			return Physics2D.ClosestPoint_Collider(position, collider);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003200 File Offset: 0x00001400
		public static Vector2 ClosestPoint(Vector2 position, Rigidbody2D rigidbody)
		{
			bool flag = rigidbody == null;
			if (flag)
			{
				throw new ArgumentNullException("Rigidbody cannot be NULL.");
			}
			return Physics2D.ClosestPoint_Rigidbody(position, rigidbody);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003230 File Offset: 0x00001430
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("ClosestPoint")]
		private static Vector2 ClosestPoint_Collider(Vector2 position, [NotNull("ArgumentNullException")] Collider2D collider)
		{
			Vector2 vector;
			Physics2D.ClosestPoint_Collider_Injected(ref position, collider, out vector);
			return vector;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003248 File Offset: 0x00001448
		[NativeMethod("ClosestPoint")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static Vector2 ClosestPoint_Rigidbody(Vector2 position, [NotNull("ArgumentNullException")] Rigidbody2D rigidbody)
		{
			Vector2 vector;
			Physics2D.ClosestPoint_Rigidbody_Injected(ref position, rigidbody, out vector);
			return vector;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003260 File Offset: 0x00001460
		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end)
		{
			return Physics2D.defaultPhysicsScene.Linecast(start, end, -5);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003284 File Offset: 0x00001484
		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter2D);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000032B8 File Offset: 0x000014B8
		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter2D);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000032E8 File Offset: 0x000014E8
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter2D);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003314 File Offset: 0x00001514
		public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter, results);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003338 File Offset: 0x00001538
		public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, List<RaycastHit2D> results)
		{
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter, results);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000335C File Offset: 0x0000155C
		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.LinecastAll_Internal(Physics2D.defaultPhysicsScene, start, end, contactFilter2D);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003390 File Offset: 0x00001590
		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.LinecastAll_Internal(Physics2D.defaultPhysicsScene, start, end, contactFilter2D);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000033C0 File Offset: 0x000015C0
		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.LinecastAll_Internal(Physics2D.defaultPhysicsScene, start, end, contactFilter2D);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000033EC File Offset: 0x000015EC
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.LinecastAll_Internal(Physics2D.defaultPhysicsScene, start, end, contactFilter2D);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003415 File Offset: 0x00001615
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("LinecastAll_Binding")]
		private static RaycastHit2D[] LinecastAll_Internal(PhysicsScene2D physicsScene, Vector2 start, Vector2 end, ContactFilter2D contactFilter)
		{
			return Physics2D.LinecastAll_Internal_Injected(ref physicsScene, ref start, ref end, ref contactFilter);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003424 File Offset: 0x00001624
		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.Linecast(start, end, results, -5);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003448 File Offset: 0x00001648
		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter2D, results);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000347C File Offset: 0x0000167C
		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter2D, results);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000034B0 File Offset: 0x000016B0
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.Linecast(start, end, contactFilter2D, results);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000034E0 File Offset: 0x000016E0
		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity, -5);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003508 File Offset: 0x00001708
		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, -5);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000352C File Offset: 0x0000172C
		[RequiredByNativeCode]
		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003560 File Offset: 0x00001760
		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00003594 File Offset: 0x00001794
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000035C4 File Offset: 0x000017C4
		[ExcludeFromDocs]
		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity, contactFilter, results);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000035EC File Offset: 0x000017EC
		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00003614 File Offset: 0x00001814
		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000363C File Offset: 0x0000183C
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity, results, -5);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003668 File Offset: 0x00001868
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, results, -5);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003690 File Offset: 0x00001890
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter2D, results);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000036C8 File Offset: 0x000018C8
		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter2D, results);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000036FC File Offset: 0x000018FC
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter2D, results);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000372C File Offset: 0x0000192C
		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.RaycastAll_Internal(Physics2D.defaultPhysicsScene, origin, direction, float.PositiveInfinity, contactFilter2D);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003764 File Offset: 0x00001964
		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.RaycastAll_Internal(Physics2D.defaultPhysicsScene, origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003798 File Offset: 0x00001998
		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.RaycastAll_Internal(Physics2D.defaultPhysicsScene, origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000037CC File Offset: 0x000019CC
		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.RaycastAll_Internal(Physics2D.defaultPhysicsScene, origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000037FC File Offset: 0x000019FC
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.RaycastAll_Internal(Physics2D.defaultPhysicsScene, origin, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003827 File Offset: 0x00001A27
		[NativeMethod("RaycastAll_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static RaycastHit2D[] RaycastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.RaycastAll_Internal_Injected(ref physicsScene, ref origin, ref direction, distance, ref contactFilter);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003838 File Offset: 0x00001A38
		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, float.PositiveInfinity, -5);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00003864 File Offset: 0x00001A64
		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, -5);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000388C File Offset: 0x00001A8C
		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000038C4 File Offset: 0x00001AC4
		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000038F8 File Offset: 0x00001AF8
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00003928 File Offset: 0x00001B28
		[ExcludeFromDocs]
		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, float.PositiveInfinity, contactFilter, results);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00003954 File Offset: 0x00001B54
		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000397C File Offset: 0x00001B7C
		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000039A4 File Offset: 0x00001BA4
		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.CircleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, radius, direction, float.PositiveInfinity, contactFilter2D);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000039DC File Offset: 0x00001BDC
		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.CircleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003A10 File Offset: 0x00001C10
		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.CircleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003A44 File Offset: 0x00001C44
		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.CircleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003A74 File Offset: 0x00001C74
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.CircleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, radius, direction, distance, contactFilter2D);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003AA1 File Offset: 0x00001CA1
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("CircleCastAll_Binding")]
		private static RaycastHit2D[] CircleCastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.CircleCastAll_Internal_Injected(ref physicsScene, ref origin, radius, ref direction, distance, ref contactFilter);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003AB4 File Offset: 0x00001CB4
		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, float.PositiveInfinity, results, -5);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00003AE0 File Offset: 0x00001CE0
		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, results, -5);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00003B08 File Offset: 0x00001D08
		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003B40 File Offset: 0x00001D40
		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003B78 File Offset: 0x00001D78
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003BAC File Offset: 0x00001DAC
		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, float.PositiveInfinity, -5);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003BD8 File Offset: 0x00001DD8
		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, -5);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003C00 File Offset: 0x00001E00
		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003C38 File Offset: 0x00001E38
		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003C70 File Offset: 0x00001E70
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("Physics2D.AllLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00003CA4 File Offset: 0x00001EA4
		[ExcludeFromDocs]
		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, float.PositiveInfinity, contactFilter, results);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00003CFC File Offset: 0x00001EFC
		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003D28 File Offset: 0x00001F28
		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.BoxCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, angle, direction, float.PositiveInfinity, contactFilter2D);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003D60 File Offset: 0x00001F60
		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.BoxCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003D98 File Offset: 0x00001F98
		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.BoxCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003DD0 File Offset: 0x00001FD0
		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.BoxCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00003E04 File Offset: 0x00002004
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.BoxCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00003E33 File Offset: 0x00002033
		[NativeMethod("BoxCastAll_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static RaycastHit2D[] BoxCastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.BoxCastAll_Internal_Injected(ref physicsScene, ref origin, ref size, angle, ref direction, distance, ref contactFilter);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003E48 File Offset: 0x00002048
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, float.PositiveInfinity, results, -5);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00003E74 File Offset: 0x00002074
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, results, -5);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00003EA0 File Offset: 0x000020A0
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003EDC File Offset: 0x000020DC
		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003F14 File Offset: 0x00002114
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter2D, results);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00003F48 File Offset: 0x00002148
		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, -5);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003F74 File Offset: 0x00002174
		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, -5);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00003FA0 File Offset: 0x000021A0
		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00003FDC File Offset: 0x000021DC
		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004014 File Offset: 0x00002214
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004048 File Offset: 0x00002248
		[ExcludeFromDocs]
		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, contactFilter, results);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004078 File Offset: 0x00002278
		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000040A4 File Offset: 0x000022A4
		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000040D0 File Offset: 0x000022D0
		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.CapsuleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, contactFilter2D);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000410C File Offset: 0x0000230C
		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.CapsuleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004143 File Offset: 0x00002343
		[NativeMethod("CapsuleCastAll_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static RaycastHit2D[] CapsuleCastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.CapsuleCastAll_Internal_Injected(ref physicsScene, ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000415C File Offset: 0x0000235C
		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.CapsuleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004194 File Offset: 0x00002394
		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.CapsuleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000041C8 File Offset: 0x000023C8
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.CapsuleCastAll_Internal(Physics2D.defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter2D);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000041FC File Offset: 0x000023FC
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, results, -5);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000422C File Offset: 0x0000242C
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, results, -5);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004258 File Offset: 0x00002458
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter2D, results);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004294 File Offset: 0x00002494
		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter2D, results);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000042D0 File Offset: 0x000024D0
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter2D, results);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004308 File Offset: 0x00002508
		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray)
		{
			return Physics2D.defaultPhysicsScene.GetRayIntersection(ray, float.PositiveInfinity, -5);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004330 File Offset: 0x00002530
		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray, float distance)
		{
			return Physics2D.defaultPhysicsScene.GetRayIntersection(ray, distance, -5);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004354 File Offset: 0x00002554
		public static RaycastHit2D GetRayIntersection(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return Physics2D.defaultPhysicsScene.GetRayIntersection(ray, distance, layerMask);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004378 File Offset: 0x00002578
		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray)
		{
			return Physics2D.GetRayIntersectionAll_Internal(Physics2D.defaultPhysicsScene, ray.origin, ray.direction, float.PositiveInfinity, -5);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000043AC File Offset: 0x000025AC
		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, float distance)
		{
			return Physics2D.GetRayIntersectionAll_Internal(Physics2D.defaultPhysicsScene, ray.origin, ray.direction, distance, -5);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000043DC File Offset: 0x000025DC
		[RequiredByNativeCode]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return Physics2D.GetRayIntersectionAll_Internal(Physics2D.defaultPhysicsScene, ray.origin, ray.direction, distance, layerMask);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004408 File Offset: 0x00002608
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetRayIntersectionAll_Binding")]
		private static RaycastHit2D[] GetRayIntersectionAll_Internal(PhysicsScene2D physicsScene, Vector3 origin, Vector3 direction, float distance, int layerMask)
		{
			return Physics2D.GetRayIntersectionAll_Internal_Injected(ref physicsScene, ref origin, ref direction, distance, layerMask);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004418 File Offset: 0x00002618
		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results)
		{
			return Physics2D.defaultPhysicsScene.GetRayIntersection(ray, float.PositiveInfinity, results, -5);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00004440 File Offset: 0x00002640
		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, float distance)
		{
			return Physics2D.defaultPhysicsScene.GetRayIntersection(ray, distance, results, -5);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00004464 File Offset: 0x00002664
		[RequiredByNativeCode]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return Physics2D.defaultPhysicsScene.GetRayIntersection(ray, distance, results, layerMask);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004488 File Offset: 0x00002688
		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point)
		{
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, -5);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000044AC File Offset: 0x000026AC
		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter2D);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000044E0 File Offset: 0x000026E0
		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter2D);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004510 File Offset: 0x00002710
		public static Collider2D OverlapPoint(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter2D);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000453C File Offset: 0x0000273C
		public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, [Unmarshalled] Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004560 File Offset: 0x00002760
		public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004584 File Offset: 0x00002784
		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapPointAll_Internal(Physics2D.defaultPhysicsScene, point, contactFilter2D);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000045B4 File Offset: 0x000027B4
		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapPointAll_Internal(Physics2D.defaultPhysicsScene, point, contactFilter2D);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000045E4 File Offset: 0x000027E4
		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.OverlapPointAll_Internal(Physics2D.defaultPhysicsScene, point, contactFilter2D);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004610 File Offset: 0x00002810
		public static Collider2D[] OverlapPointAll(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.OverlapPointAll_Internal(Physics2D.defaultPhysicsScene, point, contactFilter2D);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00004637 File Offset: 0x00002837
		[NativeMethod("OverlapPointAll_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static Collider2D[] OverlapPointAll_Internal(PhysicsScene2D physicsScene, Vector2 point, ContactFilter2D contactFilter)
		{
			return Physics2D.OverlapPointAll_Internal_Injected(ref physicsScene, ref point, ref contactFilter);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004644 File Offset: 0x00002844
		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, results, -5);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004668 File Offset: 0x00002868
		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter2D, results);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000469C File Offset: 0x0000289C
		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter2D, results);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000046CC File Offset: 0x000028CC
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapPoint(point, contactFilter2D, results);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000046F8 File Offset: 0x000028F8
		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius)
		{
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, -5);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000471C File Offset: 0x0000291C
		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter2D);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00004750 File Offset: 0x00002950
		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter2D);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004780 File Offset: 0x00002980
		public static Collider2D OverlapCircle(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter2D);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000047AC File Offset: 0x000029AC
		public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000047D0 File Offset: 0x000029D0
		public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000047F4 File Offset: 0x000029F4
		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapCircleAll_Internal(Physics2D.defaultPhysicsScene, point, radius, contactFilter2D);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004828 File Offset: 0x00002A28
		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapCircleAll_Internal(Physics2D.defaultPhysicsScene, point, radius, contactFilter2D);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004858 File Offset: 0x00002A58
		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.OverlapCircleAll_Internal(Physics2D.defaultPhysicsScene, point, radius, contactFilter2D);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004884 File Offset: 0x00002A84
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.OverlapCircleAll_Internal(Physics2D.defaultPhysicsScene, point, radius, contactFilter2D);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000048AD File Offset: 0x00002AAD
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("OverlapCircleAll_Binding")]
		private static Collider2D[] OverlapCircleAll_Internal(PhysicsScene2D physicsScene, Vector2 point, float radius, ContactFilter2D contactFilter)
		{
			return Physics2D.OverlapCircleAll_Internal_Injected(ref physicsScene, ref point, radius, ref contactFilter);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000048BC File Offset: 0x00002ABC
		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, results, -5);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000048E0 File Offset: 0x00002AE0
		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter2D, results);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00004914 File Offset: 0x00002B14
		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter2D, results);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00004948 File Offset: 0x00002B48
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapCircle(point, radius, contactFilter2D, results);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004978 File Offset: 0x00002B78
		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle)
		{
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, -5);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000499C File Offset: 0x00002B9C
		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter2D);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000049D0 File Offset: 0x00002BD0
		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter2D);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004A04 File Offset: 0x00002C04
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter2D);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004A34 File Offset: 0x00002C34
		public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004A5C File Offset: 0x00002C5C
		public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004A84 File Offset: 0x00002C84
		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapBoxAll_Internal(Physics2D.defaultPhysicsScene, point, size, angle, contactFilter2D);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004AB8 File Offset: 0x00002CB8
		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapBoxAll_Internal(Physics2D.defaultPhysicsScene, point, size, angle, contactFilter2D);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004AEC File Offset: 0x00002CEC
		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.OverlapBoxAll_Internal(Physics2D.defaultPhysicsScene, point, size, angle, contactFilter2D);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004B1C File Offset: 0x00002D1C
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.OverlapBoxAll_Internal(Physics2D.defaultPhysicsScene, point, size, angle, contactFilter2D);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004B47 File Offset: 0x00002D47
		[NativeMethod("OverlapBoxAll_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static Collider2D[] OverlapBoxAll_Internal(PhysicsScene2D physicsScene, Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter)
		{
			return Physics2D.OverlapBoxAll_Internal_Injected(ref physicsScene, ref point, ref size, angle, ref contactFilter);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004B58 File Offset: 0x00002D58
		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, results, -5);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004B80 File Offset: 0x00002D80
		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter2D, results);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004BB8 File Offset: 0x00002DB8
		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter2D, results);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00004BEC File Offset: 0x00002DEC
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter2D, results);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004C1C File Offset: 0x00002E1C
		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB)
		{
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, -5);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00004C40 File Offset: 0x00002E40
		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter2D);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00004C74 File Offset: 0x00002E74
		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter2D);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter2D);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004D18 File Offset: 0x00002F18
		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB)
		{
			return Physics2D.OverlapAreaAllToBox_Internal(pointA, pointB, -5, float.NegativeInfinity, float.PositiveInfinity);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004D40 File Offset: 0x00002F40
		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			return Physics2D.OverlapAreaAllToBox_Internal(pointA, pointB, layerMask, float.NegativeInfinity, float.PositiveInfinity);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004D64 File Offset: 0x00002F64
		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			return Physics2D.OverlapAreaAllToBox_Internal(pointA, pointB, layerMask, minDepth, float.PositiveInfinity);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00004D84 File Offset: 0x00002F84
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.OverlapAreaAllToBox_Internal(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00004DA4 File Offset: 0x00002FA4
		private static Collider2D[] OverlapAreaAllToBox_Internal(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth, float maxDepth)
		{
			Vector2 vector = (pointA + pointB) * 0.5f;
			Vector2 vector2 = new Vector2(Mathf.Abs(pointA.x - pointB.x), Math.Abs(pointA.y - pointB.y));
			return Physics2D.OverlapBoxAll(vector, vector2, 0f, layerMask, minDepth, maxDepth);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004E04 File Offset: 0x00003004
		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, results, -5);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00004E28 File Offset: 0x00003028
		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter2D, results);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00004E5C File Offset: 0x0000305C
		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter2D, results);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00004E90 File Offset: 0x00003090
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter2D, results);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00004EC0 File Offset: 0x000030C0
		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
		{
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, -5);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00004EE8 File Offset: 0x000030E8
		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00004F20 File Offset: 0x00003120
		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004F54 File Offset: 0x00003154
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00004F84 File Offset: 0x00003184
		public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00004FAC File Offset: 0x000031AC
		public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00004FD4 File Offset: 0x000031D4
		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapCapsuleAll_Internal(Physics2D.defaultPhysicsScene, point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00005008 File Offset: 0x00003208
		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.OverlapCapsuleAll_Internal(Physics2D.defaultPhysicsScene, point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000503C File Offset: 0x0000323C
		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.OverlapCapsuleAll_Internal(Physics2D.defaultPhysicsScene, point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000506C File Offset: 0x0000326C
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.OverlapCapsuleAll_Internal(Physics2D.defaultPhysicsScene, point, size, direction, angle, contactFilter2D);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005099 File Offset: 0x00003299
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("OverlapCapsuleAll_Binding")]
		private static Collider2D[] OverlapCapsuleAll_Internal(PhysicsScene2D physicsScene, Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter)
		{
			return Physics2D.OverlapCapsuleAll_Internal_Injected(ref physicsScene, ref point, ref size, direction, angle, ref contactFilter);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000050AC File Offset: 0x000032AC
		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results)
		{
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, results, -5);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000050D4 File Offset: 0x000032D4
		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter2D, results);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000510C File Offset: 0x0000330C
		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter2D, results);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005144 File Offset: 0x00003344
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter2D, results);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00005178 File Offset: 0x00003378
		public static int OverlapCollider(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return PhysicsScene2D.OverlapCollider(collider, contactFilter, results);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005194 File Offset: 0x00003394
		public static int OverlapCollider(Collider2D collider, ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return PhysicsScene2D.OverlapCollider(collider, contactFilter, results);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000051B0 File Offset: 0x000033B0
		public static int GetContacts(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetColliderColliderContactsArray(collider1, collider2, contactFilter, contacts);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000051CC File Offset: 0x000033CC
		public static int GetContacts(Collider2D collider, ContactPoint2D[] contacts)
		{
			return Physics2D.GetColliderContactsArray(collider, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000051F4 File Offset: 0x000033F4
		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetColliderContactsArray(collider, contactFilter, contacts);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005210 File Offset: 0x00003410
		public static int GetContacts(Collider2D collider, Collider2D[] colliders)
		{
			return Physics2D.GetColliderContactsCollidersOnlyArray(collider, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00005238 File Offset: 0x00003438
		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetColliderContactsCollidersOnlyArray(collider, contactFilter, colliders);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00005254 File Offset: 0x00003454
		public static int GetContacts(Rigidbody2D rigidbody, ContactPoint2D[] contacts)
		{
			return Physics2D.GetRigidbodyContactsArray(rigidbody, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000527C File Offset: 0x0000347C
		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetRigidbodyContactsArray(rigidbody, contactFilter, contacts);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005298 File Offset: 0x00003498
		public static int GetContacts(Rigidbody2D rigidbody, Collider2D[] colliders)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnlyArray(rigidbody, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000052C0 File Offset: 0x000034C0
		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnlyArray(rigidbody, contactFilter, colliders);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000052DA File Offset: 0x000034DA
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetColliderContactsArray_Binding")]
		private static int GetColliderContactsArray([NotNull("ArgumentNullException")] Collider2D collider, ContactFilter2D contactFilter, [Unmarshalled] [NotNull("ArgumentNullException")] ContactPoint2D[] results)
		{
			return Physics2D.GetColliderContactsArray_Injected(collider, ref contactFilter, results);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000052E5 File Offset: 0x000034E5
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetColliderColliderContactsArray_Binding")]
		private static int GetColliderColliderContactsArray([NotNull("ArgumentNullException")] Collider2D collider1, [NotNull("ArgumentNullException")] Collider2D collider2, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] [Unmarshalled] ContactPoint2D[] results)
		{
			return Physics2D.GetColliderColliderContactsArray_Injected(collider1, collider2, ref contactFilter, results);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000052F1 File Offset: 0x000034F1
		[NativeMethod("GetRigidbodyContactsArray_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static int GetRigidbodyContactsArray([NotNull("ArgumentNullException")] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] [Unmarshalled] ContactPoint2D[] results)
		{
			return Physics2D.GetRigidbodyContactsArray_Injected(rigidbody, ref contactFilter, results);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000052FC File Offset: 0x000034FC
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetColliderContactsCollidersOnlyArray_Binding")]
		private static int GetColliderContactsCollidersOnlyArray([NotNull("ArgumentNullException")] Collider2D collider, ContactFilter2D contactFilter, [Unmarshalled] [NotNull("ArgumentNullException")] Collider2D[] results)
		{
			return Physics2D.GetColliderContactsCollidersOnlyArray_Injected(collider, ref contactFilter, results);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00005307 File Offset: 0x00003507
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetRigidbodyContactsCollidersOnlyArray_Binding")]
		private static int GetRigidbodyContactsCollidersOnlyArray([NotNull("ArgumentNullException")] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [Unmarshalled] [NotNull("ArgumentNullException")] Collider2D[] results)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnlyArray_Injected(rigidbody, ref contactFilter, results);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00005314 File Offset: 0x00003514
		public static int GetContacts(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetColliderColliderContactsList(collider1, collider2, contactFilter, contacts);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00005330 File Offset: 0x00003530
		public static int GetContacts(Collider2D collider, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetColliderContactsList(collider, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00005358 File Offset: 0x00003558
		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetColliderContactsList(collider, contactFilter, contacts);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00005374 File Offset: 0x00003574
		public static int GetContacts(Collider2D collider, List<Collider2D> colliders)
		{
			return Physics2D.GetColliderContactsCollidersOnlyList(collider, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000539C File Offset: 0x0000359C
		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, List<Collider2D> colliders)
		{
			return Physics2D.GetColliderContactsCollidersOnlyList(collider, contactFilter, colliders);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000053B8 File Offset: 0x000035B8
		public static int GetContacts(Rigidbody2D rigidbody, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetRigidbodyContactsList(rigidbody, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000053E0 File Offset: 0x000035E0
		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetRigidbodyContactsList(rigidbody, contactFilter, contacts);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000053FC File Offset: 0x000035FC
		public static int GetContacts(Rigidbody2D rigidbody, List<Collider2D> colliders)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnlyList(rigidbody, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005424 File Offset: 0x00003624
		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, List<Collider2D> colliders)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnlyList(rigidbody, contactFilter, colliders);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000543E File Offset: 0x0000363E
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetColliderContactsList_Binding")]
		private static int GetColliderContactsList([NotNull("ArgumentNullException")] Collider2D collider, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<ContactPoint2D> results)
		{
			return Physics2D.GetColliderContactsList_Injected(collider, ref contactFilter, results);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005449 File Offset: 0x00003649
		[NativeMethod("GetColliderColliderContactsList_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static int GetColliderColliderContactsList([NotNull("ArgumentNullException")] Collider2D collider1, [NotNull("ArgumentNullException")] Collider2D collider2, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<ContactPoint2D> results)
		{
			return Physics2D.GetColliderColliderContactsList_Injected(collider1, collider2, ref contactFilter, results);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00005455 File Offset: 0x00003655
		[NativeMethod("GetRigidbodyContactsList_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static int GetRigidbodyContactsList([NotNull("ArgumentNullException")] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<ContactPoint2D> results)
		{
			return Physics2D.GetRigidbodyContactsList_Injected(rigidbody, ref contactFilter, results);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00005460 File Offset: 0x00003660
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		[NativeMethod("GetColliderContactsCollidersOnlyList_Binding")]
		private static int GetColliderContactsCollidersOnlyList([NotNull("ArgumentNullException")] Collider2D collider, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<Collider2D> results)
		{
			return Physics2D.GetColliderContactsCollidersOnlyList_Injected(collider, ref contactFilter, results);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000546B File Offset: 0x0000366B
		[NativeMethod("GetRigidbodyContactsCollidersOnlyList_Binding")]
		[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
		private static int GetRigidbodyContactsCollidersOnlyList([NotNull("ArgumentNullException")] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<Collider2D> results)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnlyList_Injected(rigidbody, ref contactFilter, results);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005478 File Offset: 0x00003678
		internal static void SetEditorDragMovement(bool dragging, GameObject[] objs)
		{
			foreach (Rigidbody2D rigidbody2D in Physics2D.m_LastDisabledRigidbody2D)
			{
				bool flag = rigidbody2D != null;
				if (flag)
				{
					rigidbody2D.SetDragBehaviour(false);
				}
			}
			Physics2D.m_LastDisabledRigidbody2D.Clear();
			bool flag2 = !dragging;
			if (!flag2)
			{
				foreach (GameObject gameObject in objs)
				{
					Rigidbody2D[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody2D>(false);
					foreach (Rigidbody2D rigidbody2D2 in componentsInChildren)
					{
						Physics2D.m_LastDisabledRigidbody2D.Add(rigidbody2D2);
						rigidbody2D2.SetDragBehaviour(true);
					}
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00005554 File Offset: 0x00003754
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00005567 File Offset: 0x00003767
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Physics2D.raycastsHitTriggers is deprecated. Use Physics2D.queriesHitTriggers instead. (UnityUpgradable) -> queriesHitTriggers", true)]
		public static bool raycastsHitTriggers
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000556C File Offset: 0x0000376C
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00005567 File Offset: 0x00003767
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Physics2D.raycastsStartInColliders is deprecated. Use Physics2D.queriesStartInColliders instead. (UnityUpgradable) -> queriesStartInColliders", true)]
		public static bool raycastsStartInColliders
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00005580 File Offset: 0x00003780
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.deleteStopsCallbacks is deprecated.(UnityUpgradable) -> changeStopsCallbacks", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool deleteStopsCallbacks
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005594 File Offset: 0x00003794
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.changeStopsCallbacks is deprecated and will always return false.", false)]
		public static bool changeStopsCallbacks
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000055A8 File Offset: 0x000037A8
		// (set) Token: 0x060001AA RID: 426 RVA: 0x000055BF File Offset: 0x000037BF
		[Obsolete("Physics2D.minPenetrationForPenalty is deprecated. Use Physics2D.defaultContactOffset instead. (UnityUpgradable) -> defaultContactOffset", false)]
		public static float minPenetrationForPenalty
		{
			get
			{
				return Physics2D.defaultContactOffset;
			}
			set
			{
				Physics2D.defaultContactOffset = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000055CC File Offset: 0x000037CC
		// (set) Token: 0x060001AC RID: 428 RVA: 0x000055E9 File Offset: 0x000037E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Physics2D.autoSimulation is deprecated. Use Physics2D.simulationMode instead.", false)]
		public static bool autoSimulation
		{
			get
			{
				return Physics2D.simulationMode != SimulationMode2D.Script;
			}
			set
			{
				Physics2D.simulationMode = (value ? SimulationMode2D.FixedUpdate : SimulationMode2D.Script);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000055FC File Offset: 0x000037FC
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.colliderAwakeColor is deprecated. This options has been moved to 2D Preferences.", true)]
		public static Color colliderAwakeColor
		{
			get
			{
				return Color.magenta;
			}
			set
			{
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00005614 File Offset: 0x00003814
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.colliderAsleepColor is deprecated. This options has been moved to 2D Preferences.", true)]
		public static Color colliderAsleepColor
		{
			get
			{
				return Color.magenta;
			}
			set
			{
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000562C File Offset: 0x0000382C
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.colliderContactColor is deprecated. This options has been moved to 2D Preferences.", true)]
		public static Color colliderContactColor
		{
			get
			{
				return Color.magenta;
			}
			set
			{
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00005644 File Offset: 0x00003844
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.colliderAABBColor is deprecated. All Physics 2D colors moved to Preferences. This is now known as 'Collider Bounds Color'.", true)]
		public static Color colliderAABBColor
		{
			get
			{
				return Color.magenta;
			}
			set
			{
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000565C File Offset: 0x0000385C
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.contactArrowScale is deprecated. This options has been moved to 2D Preferences.", true)]
		public static float contactArrowScale
		{
			get
			{
				return 0.2f;
			}
			set
			{
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00005673 File Offset: 0x00003873
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000567A File Offset: 0x0000387A
		[Obsolete("Physics2D.alwaysShowColliders is deprecated. It is no longer available in the Editor or Builds.", true)]
		public static bool alwaysShowColliders { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00005682 File Offset: 0x00003882
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00005689 File Offset: 0x00003889
		[Obsolete("Physics2D.showCollidersFilled is deprecated. It is no longer available in the Editor or Builds.", true)]
		public static bool showCollidersFilled { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00005691 File Offset: 0x00003891
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00005698 File Offset: 0x00003898
		[Obsolete("Physics2D.showColliderSleep is deprecated. It is no longer available in the Editor or Builds.", true)]
		public static bool showColliderSleep { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000056A0 File Offset: 0x000038A0
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000056A7 File Offset: 0x000038A7
		[Obsolete("Physics2D.showColliderContacts is deprecated. It is no longer available in the Editor or Builds.", true)]
		public static bool showColliderContacts { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000056B0 File Offset: 0x000038B0
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("Physics2D.showColliderAABB is deprecated. It is no longer available in the Editor or Builds.", true)]
		public static bool showColliderAABB
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x060001C3 RID: 451
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_gravity_Injected(out Vector2 ret);

		// Token: 0x060001C4 RID: 452
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_gravity_Injected(ref Vector2 value);

		// Token: 0x060001C5 RID: 453
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_jobOptions_Injected(out PhysicsJobOptions2D ret);

		// Token: 0x060001C6 RID: 454
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_jobOptions_Injected(ref PhysicsJobOptions2D value);

		// Token: 0x060001C7 RID: 455
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Simulate_Internal_Injected(ref PhysicsScene2D physicsScene, float step);

		// Token: 0x060001C8 RID: 456
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsTouching_TwoCollidersWithFilter_Injected([Writable] Collider2D collider1, [Writable] Collider2D collider2, ref ContactFilter2D contactFilter);

		// Token: 0x060001C9 RID: 457
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsTouching_SingleColliderWithFilter_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

		// Token: 0x060001CA RID: 458
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Distance_Internal_Injected([Writable] Collider2D colliderA, [Writable] Collider2D colliderB, out ColliderDistance2D ret);

		// Token: 0x060001CB RID: 459
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClosestPoint_Collider_Injected(ref Vector2 position, Collider2D collider, out Vector2 ret);

		// Token: 0x060001CC RID: 460
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClosestPoint_Rigidbody_Injected(ref Vector2 position, Rigidbody2D rigidbody, out Vector2 ret);

		// Token: 0x060001CD RID: 461
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] LinecastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter);

		// Token: 0x060001CE RID: 462
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] RaycastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		// Token: 0x060001CF RID: 463
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] CircleCastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		// Token: 0x060001D0 RID: 464
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] BoxCastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		// Token: 0x060001D1 RID: 465
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] CapsuleCastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		// Token: 0x060001D2 RID: 466
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] GetRayIntersectionAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector3 origin, ref Vector3 direction, float distance, int layerMask);

		// Token: 0x060001D3 RID: 467
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] OverlapPointAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, ref ContactFilter2D contactFilter);

		// Token: 0x060001D4 RID: 468
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] OverlapCircleAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, float radius, ref ContactFilter2D contactFilter);

		// Token: 0x060001D5 RID: 469
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] OverlapBoxAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter);

		// Token: 0x060001D6 RID: 470
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] OverlapCapsuleAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter);

		// Token: 0x060001D7 RID: 471
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetColliderContactsArray_Injected(Collider2D collider, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		// Token: 0x060001D8 RID: 472
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetColliderColliderContactsArray_Injected(Collider2D collider1, Collider2D collider2, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		// Token: 0x060001D9 RID: 473
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRigidbodyContactsArray_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		// Token: 0x060001DA RID: 474
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetColliderContactsCollidersOnlyArray_Injected(Collider2D collider, ref ContactFilter2D contactFilter, Collider2D[] results);

		// Token: 0x060001DB RID: 475
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRigidbodyContactsCollidersOnlyArray_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, Collider2D[] results);

		// Token: 0x060001DC RID: 476
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetColliderContactsList_Injected(Collider2D collider, ref ContactFilter2D contactFilter, List<ContactPoint2D> results);

		// Token: 0x060001DD RID: 477
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetColliderColliderContactsList_Injected(Collider2D collider1, Collider2D collider2, ref ContactFilter2D contactFilter, List<ContactPoint2D> results);

		// Token: 0x060001DE RID: 478
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRigidbodyContactsList_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, List<ContactPoint2D> results);

		// Token: 0x060001DF RID: 479
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetColliderContactsCollidersOnlyList_Injected(Collider2D collider, ref ContactFilter2D contactFilter, List<Collider2D> results);

		// Token: 0x060001E0 RID: 480
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRigidbodyContactsCollidersOnlyList_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, List<Collider2D> results);

		// Token: 0x04000002 RID: 2
		public const int IgnoreRaycastLayer = 4;

		// Token: 0x04000003 RID: 3
		public const int DefaultRaycastLayers = -5;

		// Token: 0x04000004 RID: 4
		public const int AllLayers = -1;

		// Token: 0x04000005 RID: 5
		public const int MaxPolygonShapeVertices = 8;

		// Token: 0x04000006 RID: 6
		private static List<Rigidbody2D> m_LastDisabledRigidbody2D = new List<Rigidbody2D>();

		// Token: 0x02000005 RID: 5
		[Flags]
		internal enum GizmoOptions
		{
			// Token: 0x0400000C RID: 12
			AllColliders = 1,
			// Token: 0x0400000D RID: 13
			CollidersOutlined = 2,
			// Token: 0x0400000E RID: 14
			CollidersFilled = 4,
			// Token: 0x0400000F RID: 15
			CollidersSleeping = 8,
			// Token: 0x04000010 RID: 16
			ColliderContacts = 16,
			// Token: 0x04000011 RID: 17
			ColliderBounds = 32
		}
	}
}
