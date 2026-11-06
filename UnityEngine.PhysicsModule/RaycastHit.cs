using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000027 RID: 39
	[NativeHeader("Runtime/Interfaces/IRaycast.h")]
	[NativeHeader("PhysicsScriptingClasses.h")]
	[UsedByNativeCode]
	[NativeHeader("Modules/Physics/RaycastHit.h")]
	public struct RaycastHit
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000050DC File Offset: 0x000032DC
		public Collider collider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Collider) as Collider;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00005100 File Offset: 0x00003300
		public int colliderInstanceID
		{
			get
			{
				return this.m_Collider;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00005118 File Offset: 0x00003318
		// (set) Token: 0x0600026D RID: 621 RVA: 0x00005130 File Offset: 0x00003330
		public Vector3 point
		{
			get
			{
				return this.m_Point;
			}
			set
			{
				this.m_Point = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000513C File Offset: 0x0000333C
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00005154 File Offset: 0x00003354
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
			set
			{
				this.m_Normal = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00005160 File Offset: 0x00003360
		// (set) Token: 0x06000271 RID: 625 RVA: 0x000051AA File Offset: 0x000033AA
		public Vector3 barycentricCoordinate
		{
			get
			{
				return new Vector3(1f - (this.m_UV.y + this.m_UV.x), this.m_UV.x, this.m_UV.y);
			}
			set
			{
				this.m_UV = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000272 RID: 626 RVA: 0x000051BC File Offset: 0x000033BC
		// (set) Token: 0x06000273 RID: 627 RVA: 0x000051D4 File Offset: 0x000033D4
		public float distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				this.m_Distance = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000274 RID: 628 RVA: 0x000051E0 File Offset: 0x000033E0
		public int triangleIndex
		{
			get
			{
				return (int)this.m_FaceID;
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000051F8 File Offset: 0x000033F8
		[NativeMethod("CalculateRaycastTexCoord", true, true)]
		private static Vector2 CalculateRaycastTexCoord(int colliderInstanceID, Vector2 uv, Vector3 pos, uint face, int textcoord)
		{
			Vector2 vector;
			RaycastHit.CalculateRaycastTexCoord_Injected(colliderInstanceID, ref uv, ref pos, face, textcoord, out vector);
			return vector;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00005218 File Offset: 0x00003418
		public Vector2 textureCoord
		{
			get
			{
				return RaycastHit.CalculateRaycastTexCoord(this.m_Collider, this.m_UV, this.m_Point, this.m_FaceID, 0);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00005248 File Offset: 0x00003448
		public Vector2 textureCoord2
		{
			get
			{
				return RaycastHit.CalculateRaycastTexCoord(this.m_Collider, this.m_UV, this.m_Point, this.m_FaceID, 1);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00005278 File Offset: 0x00003478
		public Transform transform
		{
			get
			{
				Rigidbody rigidbody = this.rigidbody;
				bool flag = rigidbody != null;
				Transform transform;
				if (flag)
				{
					transform = rigidbody.transform;
				}
				else
				{
					bool flag2 = this.collider != null;
					if (flag2)
					{
						transform = this.collider.transform;
					}
					else
					{
						transform = null;
					}
				}
				return transform;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000279 RID: 633 RVA: 0x000052C4 File Offset: 0x000034C4
		public Rigidbody rigidbody
		{
			get
			{
				return (this.collider != null) ? this.collider.attachedRigidbody : null;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600027A RID: 634 RVA: 0x000052F4 File Offset: 0x000034F4
		public ArticulationBody articulationBody
		{
			get
			{
				return (this.collider != null) ? this.collider.attachedArticulationBody : null;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00005324 File Offset: 0x00003524
		public Vector2 lightmapCoord
		{
			get
			{
				Vector2 vector = RaycastHit.CalculateRaycastTexCoord(this.m_Collider, this.m_UV, this.m_Point, this.m_FaceID, 1);
				bool flag = this.collider.GetComponent<Renderer>() != null;
				if (flag)
				{
					Vector4 lightmapScaleOffset = this.collider.GetComponent<Renderer>().lightmapScaleOffset;
					vector.x = vector.x * lightmapScaleOffset.x + lightmapScaleOffset.z;
					vector.y = vector.y * lightmapScaleOffset.y + lightmapScaleOffset.w;
				}
				return vector;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600027C RID: 636 RVA: 0x000053B4 File Offset: 0x000035B4
		[Obsolete("Use textureCoord2 instead. (UnityUpgradable) -> textureCoord2")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Vector2 textureCoord1
		{
			get
			{
				return this.textureCoord2;
			}
		}

		// Token: 0x0600027D RID: 637
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalculateRaycastTexCoord_Injected(int colliderInstanceID, ref Vector2 uv, ref Vector3 pos, uint face, int textcoord, out Vector2 ret);

		// Token: 0x040000B8 RID: 184
		[NativeName("point")]
		internal Vector3 m_Point;

		// Token: 0x040000B9 RID: 185
		[NativeName("normal")]
		internal Vector3 m_Normal;

		// Token: 0x040000BA RID: 186
		[NativeName("faceID")]
		internal uint m_FaceID;

		// Token: 0x040000BB RID: 187
		[NativeName("distance")]
		internal float m_Distance;

		// Token: 0x040000BC RID: 188
		[NativeName("uv")]
		internal Vector2 m_UV;

		// Token: 0x040000BD RID: 189
		[NativeName("collider")]
		internal int m_Collider;
	}
}
