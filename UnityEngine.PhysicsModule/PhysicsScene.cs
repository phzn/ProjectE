using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000037 RID: 55
	[NativeHeader("Modules/Physics/Public/PhysicsSceneHandle.h")]
	public struct PhysicsScene : IEquatable<PhysicsScene>
	{
		// Token: 0x06000469 RID: 1129 RVA: 0x00006134 File Offset: 0x00004334
		public override string ToString()
		{
			return UnityString.Format("({0})", new object[] { this.m_Handle });
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00006164 File Offset: 0x00004364
		public static bool operator ==(PhysicsScene lhs, PhysicsScene rhs)
		{
			return lhs.m_Handle == rhs.m_Handle;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00006184 File Offset: 0x00004384
		public static bool operator !=(PhysicsScene lhs, PhysicsScene rhs)
		{
			return lhs.m_Handle != rhs.m_Handle;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000061A8 File Offset: 0x000043A8
		public override int GetHashCode()
		{
			return this.m_Handle;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000061C0 File Offset: 0x000043C0
		public override bool Equals(object other)
		{
			bool flag = !(other is PhysicsScene);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				PhysicsScene physicsScene = (PhysicsScene)other;
				flag2 = this.m_Handle == physicsScene.m_Handle;
			}
			return flag2;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000061FC File Offset: 0x000043FC
		public bool Equals(PhysicsScene other)
		{
			return this.m_Handle == other.m_Handle;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000621C File Offset: 0x0000441C
		public bool IsValid()
		{
			return PhysicsScene.IsValid_Internal(this);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00006239 File Offset: 0x00004439
		[NativeMethod("IsPhysicsSceneValid")]
		[StaticAccessor("GetPhysicsManager()", StaticAccessorType.Dot)]
		private static bool IsValid_Internal(PhysicsScene physicsScene)
		{
			return PhysicsScene.IsValid_Internal_Injected(ref physicsScene);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00006244 File Offset: 0x00004444
		public bool IsEmpty()
		{
			bool flag = this.IsValid();
			if (flag)
			{
				return PhysicsScene.IsEmpty_Internal(this);
			}
			throw new InvalidOperationException("Cannot check if physics scene is empty as it is invalid.");
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00006276 File Offset: 0x00004476
		[NativeMethod("IsPhysicsWorldEmpty")]
		[StaticAccessor("GetPhysicsManager()", StaticAccessorType.Dot)]
		private static bool IsEmpty_Internal(PhysicsScene physicsScene)
		{
			return PhysicsScene.IsEmpty_Internal_Injected(ref physicsScene);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00006280 File Offset: 0x00004480
		public void Simulate(float step)
		{
			bool flag = this.IsValid();
			if (flag)
			{
				bool flag2 = this == Physics.defaultPhysicsScene && Physics.simulationMode != SimulationMode.Script;
				if (flag2)
				{
					Debug.LogWarning("PhysicsScene.Simulate(...) was called but simulation mode is not set to Script. You should set simulation mode to Script first before calling this function therefore the simulation was not run.");
				}
				else
				{
					Physics.Simulate_Internal(this, step);
				}
				return;
			}
			throw new InvalidOperationException("Cannot simulate the physics scene as it is invalid.");
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000062E8 File Offset: 0x000044E8
		public void InterpolateBodies()
		{
			bool flag = !this.IsValid();
			if (flag)
			{
				throw new InvalidOperationException("Cannot interpolate the physics scene as it is invalid.");
			}
			bool flag2 = this == Physics.defaultPhysicsScene;
			if (flag2)
			{
				Debug.LogWarning("PhysicsScene.InterpolateBodies() was called on the default Physics Scene. This is done automatically and the call will be ignored");
			}
			else
			{
				Physics.InterpolateBodies_Internal(this);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000633C File Offset: 0x0000453C
		public void ResetInterpolationPoses()
		{
			bool flag = !this.IsValid();
			if (flag)
			{
				throw new InvalidOperationException("Cannot reset poses of the physics scene as it is invalid.");
			}
			bool flag2 = this == Physics.defaultPhysicsScene;
			if (flag2)
			{
				Debug.LogWarning("PhysicsScene.ResetInterpolationPoses() was called on the default Physics Scene. This is done automatically and the call will be ignored");
			}
			else
			{
				Physics.ResetInterpolationPoses_Internal(this);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00006390 File Offset: 0x00004590
		public bool Raycast(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("Physics.DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			bool flag2;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				Ray ray = new Ray(origin, vector);
				flag2 = PhysicsScene.Internal_RaycastTest(this, ray, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000063E0 File Offset: 0x000045E0
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("RaycastTest")]
		private static bool Internal_RaycastTest(PhysicsScene physicsScene, Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Internal_RaycastTest_Injected(ref physicsScene, ref ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000063F0 File Offset: 0x000045F0
		public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("Physics.DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			hitInfo = default(RaycastHit);
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			bool flag2;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				Ray ray = new Ray(origin, vector);
				flag2 = PhysicsScene.Internal_Raycast(this, ray, maxDistance, ref hitInfo, layerMask, queryTriggerInteraction);
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00006449 File Offset: 0x00004649
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("Raycast")]
		private static bool Internal_Raycast(PhysicsScene physicsScene, Ray ray, float maxDistance, ref RaycastHit hit, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Internal_Raycast_Injected(ref physicsScene, ref ray, maxDistance, ref hit, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000645C File Offset: 0x0000465C
		public int Raycast(Vector3 origin, Vector3 direction, RaycastHit[] raycastHits, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("Physics.DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			int num;
			if (flag)
			{
				Ray ray = new Ray(origin, direction.normalized);
				num = PhysicsScene.Internal_RaycastNonAlloc(this, ray, raycastHits, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000064A9 File Offset: 0x000046A9
		[NativeName("RaycastNonAlloc")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static int Internal_RaycastNonAlloc(PhysicsScene physicsScene, Ray ray, [Unmarshalled] RaycastHit[] raycastHits, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Internal_RaycastNonAlloc_Injected(ref physicsScene, ref ray, raycastHits, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000064BC File Offset: 0x000046BC
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("CapsuleCast")]
		private static bool Query_CapsuleCast(PhysicsScene physicsScene, Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, ref RaycastHit hitInfo, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Query_CapsuleCast_Injected(ref physicsScene, ref point1, ref point2, radius, ref direction, maxDistance, ref hitInfo, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000064E0 File Offset: 0x000046E0
		private static bool Internal_CapsuleCast(PhysicsScene physicsScene, Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			hitInfo = default(RaycastHit);
			bool flag = magnitude > float.Epsilon;
			bool flag2;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				flag2 = PhysicsScene.Query_CapsuleCast(physicsScene, point1, point2, radius, vector, maxDistance, ref hitInfo, layerMask, queryTriggerInteraction);
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00006530 File Offset: 0x00004730
		public bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return PhysicsScene.Internal_CapsuleCast(this, point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000655C File Offset: 0x0000475C
		[NativeName("CapsuleCastNonAlloc")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static int Internal_CapsuleCastNonAlloc(PhysicsScene physicsScene, Vector3 p0, Vector3 p1, float radius, Vector3 direction, [Unmarshalled] RaycastHit[] raycastHits, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Internal_CapsuleCastNonAlloc_Injected(ref physicsScene, ref p0, ref p1, radius, ref direction, raycastHits, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00006580 File Offset: 0x00004780
		public int CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			int num;
			if (flag)
			{
				num = PhysicsScene.Internal_CapsuleCastNonAlloc(this, point1, point2, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000065C4 File Offset: 0x000047C4
		[NativeName("OverlapCapsuleNonAlloc")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		private static int OverlapCapsuleNonAlloc_Internal(PhysicsScene physicsScene, Vector3 point0, Vector3 point1, float radius, [Unmarshalled] Collider[] results, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.OverlapCapsuleNonAlloc_Internal_Injected(ref physicsScene, ref point0, ref point1, radius, results, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000065D8 File Offset: 0x000047D8
		public int OverlapCapsule(Vector3 point0, Vector3 point1, float radius, Collider[] results, [DefaultValue("AllLayers")] int layerMask = -1, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return PhysicsScene.OverlapCapsuleNonAlloc_Internal(this, point0, point1, radius, results, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000065FE File Offset: 0x000047FE
		[NativeName("SphereCast")]
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		private static bool Query_SphereCast(PhysicsScene physicsScene, Vector3 origin, float radius, Vector3 direction, float maxDistance, ref RaycastHit hitInfo, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Query_SphereCast_Injected(ref physicsScene, ref origin, radius, ref direction, maxDistance, ref hitInfo, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00006614 File Offset: 0x00004814
		private static bool Internal_SphereCast(PhysicsScene physicsScene, Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			hitInfo = default(RaycastHit);
			bool flag = magnitude > float.Epsilon;
			bool flag2;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				flag2 = PhysicsScene.Query_SphereCast(physicsScene, origin, radius, vector, maxDistance, ref hitInfo, layerMask, queryTriggerInteraction);
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00006660 File Offset: 0x00004860
		public bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return PhysicsScene.Internal_SphereCast(this, origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00006688 File Offset: 0x00004888
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("SphereCastNonAlloc")]
		private static int Internal_SphereCastNonAlloc(PhysicsScene physicsScene, Vector3 origin, float radius, Vector3 direction, [Unmarshalled] RaycastHit[] raycastHits, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Internal_SphereCastNonAlloc_Injected(ref physicsScene, ref origin, radius, ref direction, raycastHits, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000066A0 File Offset: 0x000048A0
		public int SphereCast(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			int num;
			if (flag)
			{
				num = PhysicsScene.Internal_SphereCastNonAlloc(this, origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000066E2 File Offset: 0x000048E2
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("OverlapSphereNonAlloc")]
		private static int OverlapSphereNonAlloc_Internal(PhysicsScene physicsScene, Vector3 position, float radius, [Unmarshalled] Collider[] results, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.OverlapSphereNonAlloc_Internal_Injected(ref physicsScene, ref position, radius, results, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000066F4 File Offset: 0x000048F4
		public int OverlapSphere(Vector3 position, float radius, Collider[] results, [DefaultValue("AllLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.OverlapSphereNonAlloc_Internal(this, position, radius, results, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00006718 File Offset: 0x00004918
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()", StaticAccessorType.Dot)]
		[NativeName("BoxCast")]
		private static bool Query_BoxCast(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, ref RaycastHit outHit, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Query_BoxCast_Injected(ref physicsScene, ref center, ref halfExtents, ref direction, ref orientation, maxDistance, ref outHit, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000673C File Offset: 0x0000493C
		private static bool Internal_BoxCast(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Quaternion orientation, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			hitInfo = default(RaycastHit);
			bool flag = magnitude > float.Epsilon;
			bool flag2;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				flag2 = PhysicsScene.Query_BoxCast(physicsScene, center, halfExtents, vector, orientation, maxDistance, ref hitInfo, layerMask, queryTriggerInteraction);
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000678C File Offset: 0x0000498C
		public bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return PhysicsScene.Internal_BoxCast(this, center, halfExtents, orientation, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000067B8 File Offset: 0x000049B8
		[ExcludeFromDocs]
		public bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo)
		{
			return PhysicsScene.Internal_BoxCast(this, center, halfExtents, Quaternion.identity, direction, out hitInfo, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000067E7 File Offset: 0x000049E7
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("OverlapBoxNonAlloc")]
		private static int OverlapBoxNonAlloc_Internal(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, [Unmarshalled] Collider[] results, Quaternion orientation, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.OverlapBoxNonAlloc_Internal_Injected(ref physicsScene, ref center, ref halfExtents, results, ref orientation, mask, queryTriggerInteraction);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000067FC File Offset: 0x000049FC
		public int OverlapBox(Vector3 center, Vector3 halfExtents, Collider[] results, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return PhysicsScene.OverlapBoxNonAlloc_Internal(this, center, halfExtents, results, orientation, layerMask, queryTriggerInteraction);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00006824 File Offset: 0x00004A24
		[ExcludeFromDocs]
		public int OverlapBox(Vector3 center, Vector3 halfExtents, Collider[] results)
		{
			return PhysicsScene.OverlapBoxNonAlloc_Internal(this, center, halfExtents, results, Quaternion.identity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000684C File Offset: 0x00004A4C
		[StaticAccessor("GetPhysicsManager().GetPhysicsQuery()")]
		[NativeName("BoxCastNonAlloc")]
		private static int Internal_BoxCastNonAlloc(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Vector3 direction, [Unmarshalled] RaycastHit[] raycastHits, Quaternion orientation, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return PhysicsScene.Internal_BoxCastNonAlloc_Injected(ref physicsScene, ref center, ref halfExtents, ref direction, raycastHits, ref orientation, maxDistance, mask, queryTriggerInteraction);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00006870 File Offset: 0x00004A70
		public int BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results, [DefaultValue("Quaternion.identity")] Quaternion orientation, [DefaultValue("Mathf.Infinity")] float maxDistance = float.PositiveInfinity, [DefaultValue("DefaultRaycastLayers")] int layerMask = -5, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			int num;
			if (flag)
			{
				num = PhysicsScene.Internal_BoxCastNonAlloc(this, center, halfExtents, direction, results, orientation, maxDistance, layerMask, queryTriggerInteraction);
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000068B4 File Offset: 0x00004AB4
		[ExcludeFromDocs]
		public int BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results)
		{
			return this.BoxCast(center, halfExtents, direction, results, Quaternion.identity, float.PositiveInfinity, -5, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x06000494 RID: 1172
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValid_Internal_Injected(ref PhysicsScene physicsScene);

		// Token: 0x06000495 RID: 1173
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEmpty_Internal_Injected(ref PhysicsScene physicsScene);

		// Token: 0x06000496 RID: 1174
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_RaycastTest_Injected(ref PhysicsScene physicsScene, ref Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x06000497 RID: 1175
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_Raycast_Injected(ref PhysicsScene physicsScene, ref Ray ray, float maxDistance, ref RaycastHit hit, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x06000498 RID: 1176
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_RaycastNonAlloc_Injected(ref PhysicsScene physicsScene, ref Ray ray, RaycastHit[] raycastHits, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x06000499 RID: 1177
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Query_CapsuleCast_Injected(ref PhysicsScene physicsScene, ref Vector3 point1, ref Vector3 point2, float radius, ref Vector3 direction, float maxDistance, ref RaycastHit hitInfo, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600049A RID: 1178
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_CapsuleCastNonAlloc_Injected(ref PhysicsScene physicsScene, ref Vector3 p0, ref Vector3 p1, float radius, ref Vector3 direction, RaycastHit[] raycastHits, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600049B RID: 1179
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int OverlapCapsuleNonAlloc_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 point0, ref Vector3 point1, float radius, Collider[] results, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600049C RID: 1180
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Query_SphereCast_Injected(ref PhysicsScene physicsScene, ref Vector3 origin, float radius, ref Vector3 direction, float maxDistance, ref RaycastHit hitInfo, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600049D RID: 1181
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_SphereCastNonAlloc_Injected(ref PhysicsScene physicsScene, ref Vector3 origin, float radius, ref Vector3 direction, RaycastHit[] raycastHits, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600049E RID: 1182
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int OverlapSphereNonAlloc_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 position, float radius, Collider[] results, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x0600049F RID: 1183
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Query_BoxCast_Injected(ref PhysicsScene physicsScene, ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, ref Quaternion orientation, float maxDistance, ref RaycastHit outHit, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x060004A0 RID: 1184
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int OverlapBoxNonAlloc_Internal_Injected(ref PhysicsScene physicsScene, ref Vector3 center, ref Vector3 halfExtents, Collider[] results, ref Quaternion orientation, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x060004A1 RID: 1185
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_BoxCastNonAlloc_Injected(ref PhysicsScene physicsScene, ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, RaycastHit[] raycastHits, ref Quaternion orientation, float maxDistance, int mask, QueryTriggerInteraction queryTriggerInteraction);

		// Token: 0x040000C7 RID: 199
		private int m_Handle;
	}
}
