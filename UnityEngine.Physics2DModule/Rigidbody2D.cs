using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000021 RID: 33
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Modules/Physics2D/Public/Rigidbody2D.h")]
	public sealed class Rigidbody2D : Component
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00007218 File Offset: 0x00005418
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000722E File Offset: 0x0000542E
		public Vector2 position
		{
			get
			{
				Vector2 vector;
				this.get_position_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_position_Injected(ref value);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000287 RID: 647
		// (set) Token: 0x06000288 RID: 648
		public extern float rotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00007238 File Offset: 0x00005438
		public void SetRotation(float angle)
		{
			this.SetRotation_Angle(angle);
		}

		// Token: 0x0600028A RID: 650
		[NativeMethod("SetRotation")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRotation_Angle(float angle);

		// Token: 0x0600028B RID: 651 RVA: 0x00007243 File Offset: 0x00005443
		public void SetRotation(Quaternion rotation)
		{
			this.SetRotation_Quaternion(rotation);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000724E File Offset: 0x0000544E
		[NativeMethod("SetRotation")]
		private void SetRotation_Quaternion(Quaternion rotation)
		{
			this.SetRotation_Quaternion_Injected(ref rotation);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00007258 File Offset: 0x00005458
		public void MovePosition(Vector2 position)
		{
			this.MovePosition_Injected(ref position);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00007262 File Offset: 0x00005462
		public void MoveRotation(float angle)
		{
			this.MoveRotation_Angle(angle);
		}

		// Token: 0x0600028F RID: 655
		[NativeMethod("MoveRotation")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void MoveRotation_Angle(float angle);

		// Token: 0x06000290 RID: 656 RVA: 0x0000726D File Offset: 0x0000546D
		public void MoveRotation(Quaternion rotation)
		{
			this.MoveRotation_Quaternion(rotation);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00007278 File Offset: 0x00005478
		[NativeMethod("MoveRotation")]
		private void MoveRotation_Quaternion(Quaternion rotation)
		{
			this.MoveRotation_Quaternion_Injected(ref rotation);
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00007284 File Offset: 0x00005484
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000729A File Offset: 0x0000549A
		public Vector2 velocity
		{
			get
			{
				Vector2 vector;
				this.get_velocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_velocity_Injected(ref value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000294 RID: 660
		// (set) Token: 0x06000295 RID: 661
		public extern float angularVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000296 RID: 662
		// (set) Token: 0x06000297 RID: 663
		public extern bool useAutoMass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000298 RID: 664
		// (set) Token: 0x06000299 RID: 665
		public extern float mass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600029A RID: 666
		// (set) Token: 0x0600029B RID: 667
		[NativeMethod("Material")]
		public extern PhysicsMaterial2D sharedMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600029C RID: 668 RVA: 0x000072A4 File Offset: 0x000054A4
		// (set) Token: 0x0600029D RID: 669 RVA: 0x000072BA File Offset: 0x000054BA
		public Vector2 centerOfMass
		{
			get
			{
				Vector2 vector;
				this.get_centerOfMass_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_centerOfMass_Injected(ref value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600029E RID: 670 RVA: 0x000072C4 File Offset: 0x000054C4
		public Vector2 worldCenterOfMass
		{
			get
			{
				Vector2 vector;
				this.get_worldCenterOfMass_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600029F RID: 671
		// (set) Token: 0x060002A0 RID: 672
		public extern float inertia
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002A1 RID: 673
		// (set) Token: 0x060002A2 RID: 674
		public extern float drag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002A3 RID: 675
		// (set) Token: 0x060002A4 RID: 676
		public extern float angularDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002A5 RID: 677
		// (set) Token: 0x060002A6 RID: 678
		public extern float gravityScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002A7 RID: 679
		// (set) Token: 0x060002A8 RID: 680
		public extern RigidbodyType2D bodyType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetBodyType_Binding")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060002A9 RID: 681
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetDragBehaviour(bool dragged);

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002AA RID: 682
		// (set) Token: 0x060002AB RID: 683
		public extern bool useFullKinematicContacts
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002AC RID: 684 RVA: 0x000072DC File Offset: 0x000054DC
		// (set) Token: 0x060002AD RID: 685 RVA: 0x000072F7 File Offset: 0x000054F7
		public bool isKinematic
		{
			get
			{
				return this.bodyType == RigidbodyType2D.Kinematic;
			}
			set
			{
				this.bodyType = (value ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002AE RID: 686
		// (set) Token: 0x060002AF RID: 687
		[Obsolete("'fixedAngle' is no longer supported. Use constraints instead.", false)]
		[NativeMethod("FreezeRotation")]
		public extern bool fixedAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002B0 RID: 688
		// (set) Token: 0x060002B1 RID: 689
		public extern bool freezeRotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002B2 RID: 690
		// (set) Token: 0x060002B3 RID: 691
		public extern RigidbodyConstraints2D constraints
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060002B4 RID: 692
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsSleeping();

		// Token: 0x060002B5 RID: 693
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsAwake();

		// Token: 0x060002B6 RID: 694
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Sleep();

		// Token: 0x060002B7 RID: 695
		[NativeMethod("Wake")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WakeUp();

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002B8 RID: 696
		// (set) Token: 0x060002B9 RID: 697
		public extern bool simulated
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetSimulated_Binding")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002BA RID: 698
		// (set) Token: 0x060002BB RID: 699
		public extern RigidbodyInterpolation2D interpolation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002BC RID: 700
		// (set) Token: 0x060002BD RID: 701
		public extern RigidbodySleepMode2D sleepMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002BE RID: 702
		// (set) Token: 0x060002BF RID: 703
		public extern CollisionDetectionMode2D collisionDetectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002C0 RID: 704
		public extern int attachedColliderCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00007308 File Offset: 0x00005508
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000731E File Offset: 0x0000551E
		public Vector2 totalForce
		{
			get
			{
				Vector2 vector;
				this.get_totalForce_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_totalForce_Injected(ref value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002C3 RID: 707
		// (set) Token: 0x060002C4 RID: 708
		public extern float totalTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00007328 File Offset: 0x00005528
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x0000733E File Offset: 0x0000553E
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00007348 File Offset: 0x00005548
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000735E File Offset: 0x0000555E
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

		// Token: 0x060002C9 RID: 713
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouching([Writable] [NotNull("ArgumentNullException")] Collider2D collider);

		// Token: 0x060002CA RID: 714 RVA: 0x00007368 File Offset: 0x00005568
		public bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return this.IsTouching_OtherColliderWithFilter_Internal(collider, contactFilter);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00007382 File Offset: 0x00005582
		[NativeMethod("IsTouching")]
		private bool IsTouching_OtherColliderWithFilter_Internal([NotNull("ArgumentNullException")] [Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return this.IsTouching_OtherColliderWithFilter_Internal_Injected(collider, ref contactFilter);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00007390 File Offset: 0x00005590
		public bool IsTouching(ContactFilter2D contactFilter)
		{
			return this.IsTouching_AnyColliderWithFilter_Internal(contactFilter);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000073A9 File Offset: 0x000055A9
		[NativeMethod("IsTouching")]
		private bool IsTouching_AnyColliderWithFilter_Internal(ContactFilter2D contactFilter)
		{
			return this.IsTouching_AnyColliderWithFilter_Internal_Injected(ref contactFilter);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000073B4 File Offset: 0x000055B4
		[ExcludeFromDocs]
		public bool IsTouchingLayers()
		{
			return this.IsTouchingLayers(-1);
		}

		// Token: 0x060002CF RID: 719
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

		// Token: 0x060002D0 RID: 720 RVA: 0x000073CD File Offset: 0x000055CD
		public bool OverlapPoint(Vector2 point)
		{
			return this.OverlapPoint_Injected(ref point);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000073D8 File Offset: 0x000055D8
		public ColliderDistance2D Distance([Writable] Collider2D collider)
		{
			bool flag = collider == null;
			if (flag)
			{
				throw new ArgumentNullException("Collider cannot be null.");
			}
			bool flag2 = collider.attachedRigidbody == this;
			if (flag2)
			{
				throw new ArgumentException("The collider cannot be attached to the Rigidbody2D being searched.");
			}
			return this.Distance_Internal(collider);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00007424 File Offset: 0x00005624
		[NativeMethod("Distance")]
		private ColliderDistance2D Distance_Internal([NotNull("ArgumentNullException")] [Writable] Collider2D collider)
		{
			ColliderDistance2D colliderDistance2D;
			this.Distance_Internal_Injected(collider, out colliderDistance2D);
			return colliderDistance2D;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000743C File Offset: 0x0000563C
		public Vector2 ClosestPoint(Vector2 position)
		{
			return Physics2D.ClosestPoint(position, this);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00007455 File Offset: 0x00005655
		[ExcludeFromDocs]
		public void AddForce(Vector2 force)
		{
			this.AddForce(force, ForceMode2D.Force);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00007461 File Offset: 0x00005661
		public void AddForce(Vector2 force, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			this.AddForce_Injected(ref force, mode);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000746C File Offset: 0x0000566C
		[ExcludeFromDocs]
		public void AddRelativeForce(Vector2 relativeForce)
		{
			this.AddRelativeForce(relativeForce, ForceMode2D.Force);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00007478 File Offset: 0x00005678
		public void AddRelativeForce(Vector2 relativeForce, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			this.AddRelativeForce_Injected(ref relativeForce, mode);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00007483 File Offset: 0x00005683
		[ExcludeFromDocs]
		public void AddForceAtPosition(Vector2 force, Vector2 position)
		{
			this.AddForceAtPosition(force, position, ForceMode2D.Force);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00007490 File Offset: 0x00005690
		public void AddForceAtPosition(Vector2 force, Vector2 position, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			this.AddForceAtPosition_Injected(ref force, ref position, mode);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000749D File Offset: 0x0000569D
		[ExcludeFromDocs]
		public void AddTorque(float torque)
		{
			this.AddTorque(torque, ForceMode2D.Force);
		}

		// Token: 0x060002DB RID: 731
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddTorque(float torque, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

		// Token: 0x060002DC RID: 732 RVA: 0x000074AC File Offset: 0x000056AC
		public Vector2 GetPoint(Vector2 point)
		{
			Vector2 vector;
			this.GetPoint_Injected(ref point, out vector);
			return vector;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000074C4 File Offset: 0x000056C4
		public Vector2 GetRelativePoint(Vector2 relativePoint)
		{
			Vector2 vector;
			this.GetRelativePoint_Injected(ref relativePoint, out vector);
			return vector;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000074DC File Offset: 0x000056DC
		public Vector2 GetVector(Vector2 vector)
		{
			Vector2 vector2;
			this.GetVector_Injected(ref vector, out vector2);
			return vector2;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000074F4 File Offset: 0x000056F4
		public Vector2 GetRelativeVector(Vector2 relativeVector)
		{
			Vector2 vector;
			this.GetRelativeVector_Injected(ref relativeVector, out vector);
			return vector;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000750C File Offset: 0x0000570C
		public Vector2 GetPointVelocity(Vector2 point)
		{
			Vector2 vector;
			this.GetPointVelocity_Injected(ref point, out vector);
			return vector;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00007524 File Offset: 0x00005724
		public Vector2 GetRelativePointVelocity(Vector2 relativePoint)
		{
			Vector2 vector;
			this.GetRelativePointVelocity_Injected(ref relativePoint, out vector);
			return vector;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000753C File Offset: 0x0000573C
		public int OverlapCollider(ContactFilter2D contactFilter, [Out] Collider2D[] results)
		{
			return this.OverlapColliderArray_Internal(contactFilter, results);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00007556 File Offset: 0x00005756
		[NativeMethod("OverlapColliderArray_Binding")]
		private int OverlapColliderArray_Internal(ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] [Unmarshalled] Collider2D[] results)
		{
			return this.OverlapColliderArray_Internal_Injected(ref contactFilter, results);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00007564 File Offset: 0x00005764
		public int OverlapCollider(ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return this.OverlapColliderList_Internal(contactFilter, results);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000757E File Offset: 0x0000577E
		[NativeMethod("OverlapColliderList_Binding")]
		private int OverlapColliderList_Internal(ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<Collider2D> results)
		{
			return this.OverlapColliderList_Internal_Injected(ref contactFilter, results);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000758C File Offset: 0x0000578C
		public int GetContacts(ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000075B4 File Offset: 0x000057B4
		public int GetContacts(List<ContactPoint2D> contacts)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000075DC File Offset: 0x000057DC
		public int GetContacts(ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, contactFilter, contacts);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000075F8 File Offset: 0x000057F8
		public int GetContacts(ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetContacts(this, contactFilter, contacts);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00007614 File Offset: 0x00005814
		public int GetContacts(Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000763C File Offset: 0x0000583C
		public int GetContacts(List<Collider2D> colliders)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00007664 File Offset: 0x00005864
		public int GetContacts(ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, contactFilter, colliders);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00007680 File Offset: 0x00005880
		public int GetContacts(ContactFilter2D contactFilter, List<Collider2D> colliders)
		{
			return Physics2D.GetContacts(this, contactFilter, colliders);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000769C File Offset: 0x0000589C
		public int GetAttachedColliders([Out] Collider2D[] results)
		{
			return this.GetAttachedCollidersArray_Internal(results);
		}

		// Token: 0x060002EF RID: 751
		[NativeMethod("GetAttachedCollidersArray_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetAttachedCollidersArray_Internal([Unmarshalled] [NotNull("ArgumentNullException")] Collider2D[] results);

		// Token: 0x060002F0 RID: 752 RVA: 0x000076B8 File Offset: 0x000058B8
		public int GetAttachedColliders(List<Collider2D> results)
		{
			return this.GetAttachedCollidersList_Internal(results);
		}

		// Token: 0x060002F1 RID: 753
		[NativeMethod("GetAttachedCollidersList_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetAttachedCollidersList_Internal([NotNull("ArgumentNullException")] List<Collider2D> results);

		// Token: 0x060002F2 RID: 754 RVA: 0x000076D4 File Offset: 0x000058D4
		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results)
		{
			return this.CastArray_Internal(direction, float.PositiveInfinity, results);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000076F4 File Offset: 0x000058F4
		public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return this.CastArray_Internal(direction, distance, results);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000770F File Offset: 0x0000590F
		[NativeMethod("CastArray_Binding")]
		private int CastArray_Internal(Vector2 direction, float distance, [NotNull("ArgumentNullException")] [Unmarshalled] RaycastHit2D[] results)
		{
			return this.CastArray_Internal_Injected(ref direction, distance, results);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000771C File Offset: 0x0000591C
		public int Cast(Vector2 direction, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
		{
			return this.CastList_Internal(direction, distance, results);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00007737 File Offset: 0x00005937
		[NativeMethod("CastList_Binding")]
		private int CastList_Internal(Vector2 direction, float distance, [NotNull("ArgumentNullException")] List<RaycastHit2D> results)
		{
			return this.CastList_Internal_Injected(ref direction, distance, results);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00007744 File Offset: 0x00005944
		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return this.CastFilteredArray_Internal(direction, float.PositiveInfinity, contactFilter, results);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00007764 File Offset: 0x00005964
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return this.CastFilteredArray_Internal(direction, distance, contactFilter, results);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00007781 File Offset: 0x00005981
		[NativeMethod("CastFilteredArray_Binding")]
		private int CastFilteredArray_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] [Unmarshalled] RaycastHit2D[] results)
		{
			return this.CastFilteredArray_Internal_Injected(ref direction, distance, ref contactFilter, results);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00007790 File Offset: 0x00005990
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return this.CastFilteredList_Internal(direction, distance, contactFilter, results);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000077AD File Offset: 0x000059AD
		[NativeMethod("CastFilteredList_Binding")]
		private int CastFilteredList_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<RaycastHit2D> results)
		{
			return this.CastFilteredList_Internal_Injected(ref direction, distance, ref contactFilter, results);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000077BC File Offset: 0x000059BC
		public int GetShapes(PhysicsShapeGroup2D physicsShapeGroup)
		{
			return this.GetShapes_Internal(ref physicsShapeGroup.m_GroupState);
		}

		// Token: 0x060002FD RID: 765
		[NativeMethod("GetShapes_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetShapes_Internal(ref PhysicsShapeGroup2D.GroupState physicsShapeGroupState);

		// Token: 0x060002FF RID: 767
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_position_Injected(out Vector2 ret);

		// Token: 0x06000300 RID: 768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_position_Injected(ref Vector2 value);

		// Token: 0x06000301 RID: 769
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRotation_Quaternion_Injected(ref Quaternion rotation);

		// Token: 0x06000302 RID: 770
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void MovePosition_Injected(ref Vector2 position);

		// Token: 0x06000303 RID: 771
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void MoveRotation_Quaternion_Injected(ref Quaternion rotation);

		// Token: 0x06000304 RID: 772
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector2 ret);

		// Token: 0x06000305 RID: 773
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_velocity_Injected(ref Vector2 value);

		// Token: 0x06000306 RID: 774
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_centerOfMass_Injected(out Vector2 ret);

		// Token: 0x06000307 RID: 775
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_centerOfMass_Injected(ref Vector2 value);

		// Token: 0x06000308 RID: 776
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldCenterOfMass_Injected(out Vector2 ret);

		// Token: 0x06000309 RID: 777
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_totalForce_Injected(out Vector2 ret);

		// Token: 0x0600030A RID: 778
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_totalForce_Injected(ref Vector2 value);

		// Token: 0x0600030B RID: 779
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_excludeLayers_Injected(out LayerMask ret);

		// Token: 0x0600030C RID: 780
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_excludeLayers_Injected(ref LayerMask value);

		// Token: 0x0600030D RID: 781
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_includeLayers_Injected(out LayerMask ret);

		// Token: 0x0600030E RID: 782
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_includeLayers_Injected(ref LayerMask value);

		// Token: 0x0600030F RID: 783
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsTouching_OtherColliderWithFilter_Internal_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

		// Token: 0x06000310 RID: 784
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsTouching_AnyColliderWithFilter_Internal_Injected(ref ContactFilter2D contactFilter);

		// Token: 0x06000311 RID: 785
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool OverlapPoint_Injected(ref Vector2 point);

		// Token: 0x06000312 RID: 786
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Distance_Internal_Injected([Writable] Collider2D collider, out ColliderDistance2D ret);

		// Token: 0x06000313 RID: 787
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddForce_Injected(ref Vector2 force, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

		// Token: 0x06000314 RID: 788
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddRelativeForce_Injected(ref Vector2 relativeForce, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

		// Token: 0x06000315 RID: 789
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddForceAtPosition_Injected(ref Vector2 force, ref Vector2 position, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

		// Token: 0x06000316 RID: 790
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPoint_Injected(ref Vector2 point, out Vector2 ret);

		// Token: 0x06000317 RID: 791
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetRelativePoint_Injected(ref Vector2 relativePoint, out Vector2 ret);

		// Token: 0x06000318 RID: 792
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector_Injected(ref Vector2 vector, out Vector2 ret);

		// Token: 0x06000319 RID: 793
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetRelativeVector_Injected(ref Vector2 relativeVector, out Vector2 ret);

		// Token: 0x0600031A RID: 794
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPointVelocity_Injected(ref Vector2 point, out Vector2 ret);

		// Token: 0x0600031B RID: 795
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetRelativePointVelocity_Injected(ref Vector2 relativePoint, out Vector2 ret);

		// Token: 0x0600031C RID: 796
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int OverlapColliderArray_Internal_Injected(ref ContactFilter2D contactFilter, Collider2D[] results);

		// Token: 0x0600031D RID: 797
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int OverlapColliderList_Internal_Injected(ref ContactFilter2D contactFilter, List<Collider2D> results);

		// Token: 0x0600031E RID: 798
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CastArray_Internal_Injected(ref Vector2 direction, float distance, RaycastHit2D[] results);

		// Token: 0x0600031F RID: 799
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CastList_Internal_Injected(ref Vector2 direction, float distance, List<RaycastHit2D> results);

		// Token: 0x06000320 RID: 800
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CastFilteredArray_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		// Token: 0x06000321 RID: 801
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CastFilteredList_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, List<RaycastHit2D> results);
	}
}
