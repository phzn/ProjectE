using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000028 RID: 40
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Modules/Physics/Rigidbody.h")]
	public class Rigidbody : Component
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600027E RID: 638 RVA: 0x000053CC File Offset: 0x000035CC
		// (set) Token: 0x0600027F RID: 639 RVA: 0x000053E2 File Offset: 0x000035E2
		public Vector3 velocity
		{
			get
			{
				Vector3 vector;
				this.get_velocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_velocity_Injected(ref value);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000280 RID: 640 RVA: 0x000053EC File Offset: 0x000035EC
		// (set) Token: 0x06000281 RID: 641 RVA: 0x00005402 File Offset: 0x00003602
		public Vector3 angularVelocity
		{
			get
			{
				Vector3 vector;
				this.get_angularVelocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_angularVelocity_Injected(ref value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000282 RID: 642
		// (set) Token: 0x06000283 RID: 643
		public extern float drag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000284 RID: 644
		// (set) Token: 0x06000285 RID: 645
		public extern float angularDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000286 RID: 646
		// (set) Token: 0x06000287 RID: 647
		public extern float mass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000288 RID: 648
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDensity(float density);

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000289 RID: 649
		// (set) Token: 0x0600028A RID: 650
		public extern bool useGravity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600028B RID: 651
		// (set) Token: 0x0600028C RID: 652
		public extern float maxDepenetrationVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600028D RID: 653
		// (set) Token: 0x0600028E RID: 654
		public extern bool isKinematic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600028F RID: 655
		// (set) Token: 0x06000290 RID: 656
		public extern bool freezeRotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000291 RID: 657
		// (set) Token: 0x06000292 RID: 658
		public extern RigidbodyConstraints constraints
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000293 RID: 659
		// (set) Token: 0x06000294 RID: 660
		public extern CollisionDetectionMode collisionDetectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000295 RID: 661
		// (set) Token: 0x06000296 RID: 662
		public extern bool automaticCenterOfMass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000540C File Offset: 0x0000360C
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00005422 File Offset: 0x00003622
		public Vector3 centerOfMass
		{
			get
			{
				Vector3 vector;
				this.get_centerOfMass_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_centerOfMass_Injected(ref value);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000542C File Offset: 0x0000362C
		public Vector3 worldCenterOfMass
		{
			get
			{
				Vector3 vector;
				this.get_worldCenterOfMass_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600029A RID: 666
		// (set) Token: 0x0600029B RID: 667
		public extern bool automaticInertiaTensor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00005444 File Offset: 0x00003644
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000545A File Offset: 0x0000365A
		public Quaternion inertiaTensorRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_inertiaTensorRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_inertiaTensorRotation_Injected(ref value);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00005464 File Offset: 0x00003664
		// (set) Token: 0x0600029F RID: 671 RVA: 0x0000547A File Offset: 0x0000367A
		public Vector3 inertiaTensor
		{
			get
			{
				Vector3 vector;
				this.get_inertiaTensor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_inertiaTensor_Injected(ref value);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002A0 RID: 672
		// (set) Token: 0x060002A1 RID: 673
		public extern bool detectCollisions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00005484 File Offset: 0x00003684
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000549A File Offset: 0x0000369A
		public Vector3 position
		{
			get
			{
				Vector3 vector;
				this.get_position_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_position_Injected(ref value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x000054A4 File Offset: 0x000036A4
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x000054BA File Offset: 0x000036BA
		public Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				this.get_rotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_rotation_Injected(ref value);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002A6 RID: 678
		// (set) Token: 0x060002A7 RID: 679
		public extern RigidbodyInterpolation interpolation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002A8 RID: 680
		// (set) Token: 0x060002A9 RID: 681
		public extern int solverIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002AA RID: 682
		// (set) Token: 0x060002AB RID: 683
		public extern float sleepThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002AC RID: 684
		// (set) Token: 0x060002AD RID: 685
		public extern float maxAngularVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002AE RID: 686
		// (set) Token: 0x060002AF RID: 687
		public extern float maxLinearVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000054C4 File Offset: 0x000036C4
		public void MovePosition(Vector3 position)
		{
			this.MovePosition_Injected(ref position);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000054CE File Offset: 0x000036CE
		public void MoveRotation(Quaternion rot)
		{
			this.MoveRotation_Injected(ref rot);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000054D8 File Offset: 0x000036D8
		public void Move(Vector3 position, Quaternion rotation)
		{
			this.Move_Injected(ref position, ref rotation);
		}

		// Token: 0x060002B3 RID: 691
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Sleep();

		// Token: 0x060002B4 RID: 692
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsSleeping();

		// Token: 0x060002B5 RID: 693
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WakeUp();

		// Token: 0x060002B6 RID: 694
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetCenterOfMass();

		// Token: 0x060002B7 RID: 695
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetInertiaTensor();

		// Token: 0x060002B8 RID: 696 RVA: 0x000054E4 File Offset: 0x000036E4
		public Vector3 GetRelativePointVelocity(Vector3 relativePoint)
		{
			Vector3 vector;
			this.GetRelativePointVelocity_Injected(ref relativePoint, out vector);
			return vector;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000054FC File Offset: 0x000036FC
		public Vector3 GetPointVelocity(Vector3 worldPoint)
		{
			Vector3 vector;
			this.GetPointVelocity_Injected(ref worldPoint, out vector);
			return vector;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002BA RID: 698
		// (set) Token: 0x060002BB RID: 699
		public extern int solverVelocityIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00005514 File Offset: 0x00003714
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000552A File Offset: 0x0000372A
		public LayerMask excludeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_excludeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_excludeLayers_Injected(ref value);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00005534 File Offset: 0x00003734
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000554A File Offset: 0x0000374A
		public LayerMask includeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_includeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_includeLayers_Injected(ref value);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00005554 File Offset: 0x00003754
		public Vector3 GetAccumulatedForce([DefaultValue("Time.fixedDeltaTime")] float step)
		{
			Vector3 vector;
			this.GetAccumulatedForce_Injected(step, out vector);
			return vector;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000556C File Offset: 0x0000376C
		[ExcludeFromDocs]
		public Vector3 GetAccumulatedForce()
		{
			return this.GetAccumulatedForce(Time.fixedDeltaTime);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000558C File Offset: 0x0000378C
		public Vector3 GetAccumulatedTorque([DefaultValue("Time.fixedDeltaTime")] float step)
		{
			Vector3 vector;
			this.GetAccumulatedTorque_Injected(step, out vector);
			return vector;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000055A4 File Offset: 0x000037A4
		[ExcludeFromDocs]
		public Vector3 GetAccumulatedTorque()
		{
			return this.GetAccumulatedTorque(Time.fixedDeltaTime);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000055C1 File Offset: 0x000037C1
		public void AddForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddForce_Injected(ref force, mode);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000055CC File Offset: 0x000037CC
		[ExcludeFromDocs]
		public void AddForce(Vector3 force)
		{
			this.AddForce(force, ForceMode.Force);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000055D8 File Offset: 0x000037D8
		public void AddForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddForce(new Vector3(x, y, z), mode);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000055EC File Offset: 0x000037EC
		[ExcludeFromDocs]
		public void AddForce(float x, float y, float z)
		{
			this.AddForce(new Vector3(x, y, z), ForceMode.Force);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000055FF File Offset: 0x000037FF
		public void AddRelativeForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddRelativeForce_Injected(ref force, mode);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000560A File Offset: 0x0000380A
		[ExcludeFromDocs]
		public void AddRelativeForce(Vector3 force)
		{
			this.AddRelativeForce(force, ForceMode.Force);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00005616 File Offset: 0x00003816
		public void AddRelativeForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddRelativeForce(new Vector3(x, y, z), mode);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000562A File Offset: 0x0000382A
		[ExcludeFromDocs]
		public void AddRelativeForce(float x, float y, float z)
		{
			this.AddRelativeForce(new Vector3(x, y, z), ForceMode.Force);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000563D File Offset: 0x0000383D
		public void AddTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddTorque_Injected(ref torque, mode);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00005648 File Offset: 0x00003848
		[ExcludeFromDocs]
		public void AddTorque(Vector3 torque)
		{
			this.AddTorque(torque, ForceMode.Force);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00005654 File Offset: 0x00003854
		public void AddTorque(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddTorque(new Vector3(x, y, z), mode);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00005668 File Offset: 0x00003868
		[ExcludeFromDocs]
		public void AddTorque(float x, float y, float z)
		{
			this.AddTorque(new Vector3(x, y, z), ForceMode.Force);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000567B File Offset: 0x0000387B
		public void AddRelativeTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddRelativeTorque_Injected(ref torque, mode);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00005686 File Offset: 0x00003886
		[ExcludeFromDocs]
		public void AddRelativeTorque(Vector3 torque)
		{
			this.AddRelativeTorque(torque, ForceMode.Force);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00005692 File Offset: 0x00003892
		public void AddRelativeTorque(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddRelativeTorque(new Vector3(x, y, z), mode);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000056A6 File Offset: 0x000038A6
		[ExcludeFromDocs]
		public void AddRelativeTorque(float x, float y, float z)
		{
			this.AddRelativeTorque(x, y, z, ForceMode.Force);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000056B4 File Offset: 0x000038B4
		public void AddForceAtPosition(Vector3 force, Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddForceAtPosition_Injected(ref force, ref position, mode);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000056C1 File Offset: 0x000038C1
		[ExcludeFromDocs]
		public void AddForceAtPosition(Vector3 force, Vector3 position)
		{
			this.AddForceAtPosition(force, position, ForceMode.Force);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000056CE File Offset: 0x000038CE
		public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, [DefaultValue("0.0f")] float upwardsModifier, [DefaultValue("ForceMode.Force)")] ForceMode mode)
		{
			this.AddExplosionForce_Injected(explosionForce, ref explosionPosition, explosionRadius, upwardsModifier, mode);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000056DE File Offset: 0x000038DE
		[ExcludeFromDocs]
		public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier)
		{
			this.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Force);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000056EE File Offset: 0x000038EE
		[ExcludeFromDocs]
		public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
		{
			this.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 0f, ForceMode.Force);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00005701 File Offset: 0x00003901
		[NativeName("ClosestPointOnBounds")]
		private void Internal_ClosestPointOnBounds(Vector3 point, ref Vector3 outPos, ref float distance)
		{
			this.Internal_ClosestPointOnBounds_Injected(ref point, ref outPos, ref distance);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00005710 File Offset: 0x00003910
		public Vector3 ClosestPointOnBounds(Vector3 position)
		{
			float num = 0f;
			Vector3 zero = Vector3.zero;
			this.Internal_ClosestPointOnBounds(position, ref zero, ref num);
			return zero;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000573C File Offset: 0x0000393C
		private RaycastHit SweepTest(Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction, ref bool hasHit)
		{
			RaycastHit raycastHit;
			this.SweepTest_Injected(ref direction, maxDistance, queryTriggerInteraction, ref hasHit, out raycastHit);
			return raycastHit;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00005758 File Offset: 0x00003958
		public bool SweepTest(Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			bool flag3;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				bool flag2 = false;
				hitInfo = this.SweepTest(vector, maxDistance, queryTriggerInteraction, ref flag2);
				flag3 = flag2;
			}
			else
			{
				hitInfo = default(RaycastHit);
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000057AC File Offset: 0x000039AC
		[ExcludeFromDocs]
		public bool SweepTest(Vector3 direction, out RaycastHit hitInfo, float maxDistance)
		{
			return this.SweepTest(direction, out hitInfo, maxDistance, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000057C8 File Offset: 0x000039C8
		[ExcludeFromDocs]
		public bool SweepTest(Vector3 direction, out RaycastHit hitInfo)
		{
			return this.SweepTest(direction, out hitInfo, float.PositiveInfinity, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000057E8 File Offset: 0x000039E8
		[NativeName("SweepTestAll")]
		private RaycastHit[] Internal_SweepTestAll(Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction)
		{
			return this.Internal_SweepTestAll_Injected(ref direction, maxDistance, queryTriggerInteraction);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000057F4 File Offset: 0x000039F4
		public RaycastHit[] SweepTestAll(Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			float magnitude = direction.magnitude;
			bool flag = magnitude > float.Epsilon;
			RaycastHit[] array;
			if (flag)
			{
				Vector3 vector = direction / magnitude;
				array = this.Internal_SweepTestAll(vector, maxDistance, queryTriggerInteraction);
			}
			else
			{
				array = new RaycastHit[0];
			}
			return array;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00005838 File Offset: 0x00003A38
		[ExcludeFromDocs]
		public RaycastHit[] SweepTestAll(Vector3 direction, float maxDistance)
		{
			return this.SweepTestAll(direction, maxDistance, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00005854 File Offset: 0x00003A54
		[ExcludeFromDocs]
		public RaycastHit[] SweepTestAll(Vector3 direction)
		{
			return this.SweepTestAll(direction, float.PositiveInfinity, QueryTriggerInteraction.UseGlobal);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00005874 File Offset: 0x00003A74
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("The sleepVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float sleepVelocity
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000588C File Offset: 0x00003A8C
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The sleepAngularVelocity is no longer supported. Use sleepThreshold to specify energy.", true)]
		public float sleepAngularVelocity
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000058A3 File Offset: 0x00003AA3
		[Obsolete("Use Rigidbody.maxAngularVelocity instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetMaxAngularVelocity(float a)
		{
			this.maxAngularVelocity = a;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000058B0 File Offset: 0x00003AB0
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00002187 File Offset: 0x00000387
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Cone friction is no longer supported.", true)]
		public bool useConeFriction
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000058C4 File Offset: 0x00003AC4
		// (set) Token: 0x060002EB RID: 747 RVA: 0x000058DC File Offset: 0x00003ADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Please use Rigidbody.solverIterations instead. (UnityUpgradable) -> solverIterations")]
		public int solverIterationCount
		{
			get
			{
				return this.solverIterations;
			}
			set
			{
				this.solverIterations = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002EC RID: 748 RVA: 0x000058E8 File Offset: 0x00003AE8
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00005900 File Offset: 0x00003B00
		[Obsolete("Please use Rigidbody.solverVelocityIterations instead. (UnityUpgradable) -> solverVelocityIterations")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int solverVelocityIterationCount
		{
			get
			{
				return this.solverVelocityIterations;
			}
			set
			{
				this.solverVelocityIterations = value;
			}
		}

		// Token: 0x060002EF RID: 751
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector3 ret);

		// Token: 0x060002F0 RID: 752
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_velocity_Injected(ref Vector3 value);

		// Token: 0x060002F1 RID: 753
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularVelocity_Injected(out Vector3 ret);

		// Token: 0x060002F2 RID: 754
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularVelocity_Injected(ref Vector3 value);

		// Token: 0x060002F3 RID: 755
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_centerOfMass_Injected(out Vector3 ret);

		// Token: 0x060002F4 RID: 756
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_centerOfMass_Injected(ref Vector3 value);

		// Token: 0x060002F5 RID: 757
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldCenterOfMass_Injected(out Vector3 ret);

		// Token: 0x060002F6 RID: 758
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_inertiaTensorRotation_Injected(out Quaternion ret);

		// Token: 0x060002F7 RID: 759
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_inertiaTensorRotation_Injected(ref Quaternion value);

		// Token: 0x060002F8 RID: 760
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_inertiaTensor_Injected(out Vector3 ret);

		// Token: 0x060002F9 RID: 761
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_inertiaTensor_Injected(ref Vector3 value);

		// Token: 0x060002FA RID: 762
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_position_Injected(out Vector3 ret);

		// Token: 0x060002FB RID: 763
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_position_Injected(ref Vector3 value);

		// Token: 0x060002FC RID: 764
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotation_Injected(out Quaternion ret);

		// Token: 0x060002FD RID: 765
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotation_Injected(ref Quaternion value);

		// Token: 0x060002FE RID: 766
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void MovePosition_Injected(ref Vector3 position);

		// Token: 0x060002FF RID: 767
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void MoveRotation_Injected(ref Quaternion rot);

		// Token: 0x06000300 RID: 768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Move_Injected(ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06000301 RID: 769
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetRelativePointVelocity_Injected(ref Vector3 relativePoint, out Vector3 ret);

		// Token: 0x06000302 RID: 770
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPointVelocity_Injected(ref Vector3 worldPoint, out Vector3 ret);

		// Token: 0x06000303 RID: 771
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_excludeLayers_Injected(out LayerMask ret);

		// Token: 0x06000304 RID: 772
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_excludeLayers_Injected(ref LayerMask value);

		// Token: 0x06000305 RID: 773
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_includeLayers_Injected(out LayerMask ret);

		// Token: 0x06000306 RID: 774
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_includeLayers_Injected(ref LayerMask value);

		// Token: 0x06000307 RID: 775
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetAccumulatedForce_Injected([DefaultValue("Time.fixedDeltaTime")] float step, out Vector3 ret);

		// Token: 0x06000308 RID: 776
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetAccumulatedTorque_Injected([DefaultValue("Time.fixedDeltaTime")] float step, out Vector3 ret);

		// Token: 0x06000309 RID: 777
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddForce_Injected(ref Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600030A RID: 778
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddRelativeForce_Injected(ref Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600030B RID: 779
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddTorque_Injected(ref Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600030C RID: 780
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddRelativeTorque_Injected(ref Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600030D RID: 781
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddForceAtPosition_Injected(ref Vector3 force, ref Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600030E RID: 782
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddExplosionForce_Injected(float explosionForce, ref Vector3 explosionPosition, float explosionRadius, [DefaultValue("0.0f")] float upwardsModifier, [DefaultValue("ForceMode.Force)")] ForceMode mode);

		// Token: 0x0600030F RID: 783
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_ClosestPointOnBounds_Injected(ref Vector3 point, ref Vector3 outPos, ref float distance);

		// Token: 0x06000310 RID: 784
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SweepTest_Injected(ref Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction, ref bool hasHit, out RaycastHit ret);

		// Token: 0x06000311 RID: 785
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern RaycastHit[] Internal_SweepTestAll_Injected(ref Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction);
	}
}
