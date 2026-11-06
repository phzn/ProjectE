using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x020009F4 RID: 2548
	[ExecuteAlways]
	[SelectionBase]
	public class CoverWallSetup : MonoBehaviour, ICoverStanceOverride
	{
		// Token: 0x0600487E RID: 18558 RVA: 0x001B9C3B File Offset: 0x001B7E3B
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.CoverWallSetup_interactionConfig)
			{
				return this.interactionConfig;
			}
			throw new InvalidOperationException("Unsupported CoverWallSetup POI binding: " + field.ToString());
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x001B9C65 File Offset: 0x001B7E65
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.CoverWallSetup_interactionConfig)
			{
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported CoverWallSetup POI binding: " + field.ToString());
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x001B9C95 File Offset: 0x001B7E95
		public float Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004881 RID: 18561 RVA: 0x001B9C9D File Offset: 0x001B7E9D
		public float Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004882 RID: 18562 RVA: 0x001B9CA5 File Offset: 0x001B7EA5
		public float Thickness
		{
			get
			{
				return this.thickness;
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x001B9CAD File Offset: 0x001B7EAD
		public bool IsCrouched
		{
			get
			{
				return this.stance == CoverWallSetup.Stance.Crouched;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004884 RID: 18564 RVA: 0x001B9CB8 File Offset: 0x001B7EB8
		public float MinAllowedHeight
		{
			get
			{
				if (!this.IsCrouched)
				{
					return 1.6f;
				}
				return 0.3f;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x001B9CCD File Offset: 0x001B7ECD
		public float MaxAllowedHeight
		{
			get
			{
				if (!this.IsCrouched)
				{
					return float.MaxValue;
				}
				return 1.3f;
			}
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x001B9CE2 File Offset: 0x001B7EE2
		public float ClampHeight(float h)
		{
			return Mathf.Clamp(h, this.MinAllowedHeight, this.MaxAllowedHeight);
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x001B9CF6 File Offset: 0x001B7EF6
		public bool TryGetForcedCrouch(out bool crouch)
		{
			crouch = (this.allowCrouchToggle ? (this.IsCrouched ^ CoverCrouchToggle.StanceInverted) : this.IsCrouched);
			return true;
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004888 RID: 18568 RVA: 0x001B9D17 File Offset: 0x001B7F17
		public bool HasLeftPeek
		{
			get
			{
				return this.peekMode == CoverWallSetup.PeekMode.Left || this.peekMode == CoverWallSetup.PeekMode.Both;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x001B9D2D File Offset: 0x001B7F2D
		public bool HasRightPeek
		{
			get
			{
				return this.peekMode == CoverWallSetup.PeekMode.Right || this.peekMode == CoverWallSetup.PeekMode.Both;
			}
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x001B9D43 File Offset: 0x001B7F43
		private void Awake()
		{
			this.ApplyTechnicalRendererMode();
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x001B9D43 File Offset: 0x001B7F43
		private void OnEnable()
		{
			this.ApplyTechnicalRendererMode();
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x001B9D4B File Offset: 0x001B7F4B
		private void Start()
		{
			if (Application.isPlaying)
			{
				this.Rebuild();
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x001B9D5C File Offset: 0x001B7F5C
		public int ActiveCoverPointCount
		{
			get
			{
				if (this.coverLine == null || this.coverLine.CPTransform == null)
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.coverLine.CPTransform.childCount; i++)
				{
					Transform child = this.coverLine.CPTransform.GetChild(i);
					if (child.gameObject.activeSelf && child.GetComponent<vCoverPoint>() != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x001B9DDC File Offset: 0x001B7FDC
		public bool TryGetVisualBounds(out CoverWallSetup.WallBounds bounds)
		{
			bounds = default(CoverWallSetup.WallBounds);
			if (this.wallVisual == null)
			{
				this.wallVisual = base.transform.Find("Wall_Visual");
			}
			if (this.wallVisual == null)
			{
				return false;
			}
			MeshFilter[] componentsInChildren = this.wallVisual.GetComponentsInChildren<MeshFilter>(true);
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			bool flag = false;
			Vector3 vector = Vector3.positiveInfinity;
			Vector3 vector2 = Vector3.negativeInfinity;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Mesh sharedMesh = componentsInChildren[i].sharedMesh;
				if (!(sharedMesh == null))
				{
					Matrix4x4 matrix4x = worldToLocalMatrix * componentsInChildren[i].transform.localToWorldMatrix;
					Bounds bounds2 = sharedMesh.bounds;
					for (int j = 0; j < 8; j++)
					{
						Vector3 vector3 = new Vector3(((j & 1) == 0) ? bounds2.min.x : bounds2.max.x, ((j & 2) == 0) ? bounds2.min.y : bounds2.max.y, ((j & 4) == 0) ? bounds2.min.z : bounds2.max.z);
						Vector3 vector4 = matrix4x.MultiplyPoint3x4(vector3);
						vector = Vector3.Min(vector, vector4);
						vector2 = Vector3.Max(vector2, vector4);
					}
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			bounds.min = vector;
			bounds.max = vector2;
			return bounds.IsValid;
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x001B9F54 File Offset: 0x001B8154
		public void SetVisualBounds(CoverWallSetup.WallBounds target)
		{
			CoverWallSetup.WallBounds wallBounds;
			if (!this.TryGetVisualBounds(out wallBounds))
			{
				return;
			}
			Vector3 vector = new Vector3(Mathf.Max(0.3f, target.Length), this.ClampHeight(target.Height), Mathf.Max(0.02f, target.Thickness));
			Vector3 size = wallBounds.Size;
			Vector3 localScale = this.wallVisual.localScale;
			Vector3 vector2 = new Vector3((size.x > 0.0001f) ? (vector.x / size.x) : 1f, (size.y > 0.0001f) ? (vector.y / size.y) : 1f, (size.z > 0.0001f) ? (vector.z / size.z) : 1f);
			Vector3 vector3 = this.MapRootAxesToVisualAxes(vector2);
			this.wallVisual.localScale = Vector3.Scale(localScale, vector3);
			CoverWallSetup.WallBounds wallBounds2;
			if (this.TryGetVisualBounds(out wallBounds2))
			{
				Vector3 vector4 = new Vector3((target.min.x + target.max.x) * 0.5f, target.min.y + vector.y * 0.5f, (target.min.z + target.max.z) * 0.5f) - wallBounds2.Center;
				this.wallVisual.position += base.transform.TransformVector(vector4);
			}
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x001BA0D4 File Offset: 0x001B82D4
		private Vector3 MapRootAxesToVisualAxes(Vector3 rootAxisRatio)
		{
			Quaternion quaternion = Quaternion.Inverse(base.transform.rotation) * this.wallVisual.rotation;
			Vector3 vector = quaternion * Vector3.right;
			Vector3 vector2 = quaternion * Vector3.up;
			Vector3 vector3 = quaternion * Vector3.forward;
			return new Vector3(CoverWallSetup.Pick(vector, rootAxisRatio), CoverWallSetup.Pick(vector2, rootAxisRatio), CoverWallSetup.Pick(vector3, rootAxisRatio));
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x001BA140 File Offset: 0x001B8340
		private static float Pick(Vector3 dir, Vector3 ratio)
		{
			float num = Mathf.Abs(dir.x);
			float num2 = Mathf.Abs(dir.y);
			float num3 = Mathf.Abs(dir.z);
			if (num >= num2 && num >= num3)
			{
				return ratio.x;
			}
			if (num2 >= num3)
			{
				return ratio.y;
			}
			return ratio.z;
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x001BA194 File Offset: 0x001B8394
		public void SetDimensions(float newLength, float newHeight, float newThickness)
		{
			CoverWallSetup.WallBounds wallBounds;
			if (!this.TryGetVisualBounds(out wallBounds))
			{
				return;
			}
			Vector3 center = wallBounds.Center;
			newLength = Mathf.Max(0.3f, newLength);
			newHeight = this.ClampHeight(newHeight);
			newThickness = Mathf.Max(0.02f, newThickness);
			CoverWallSetup.WallBounds wallBounds2 = new CoverWallSetup.WallBounds
			{
				min = new Vector3(center.x - newLength * 0.5f, wallBounds.min.y, center.z - newThickness * 0.5f),
				max = new Vector3(center.x + newLength * 0.5f, wallBounds.min.y + newHeight, center.z + newThickness * 0.5f)
			};
			this.SetVisualBounds(wallBounds2);
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x001BA250 File Offset: 0x001B8450
		public void SetEdgeX(bool minEdge, float newLocalX)
		{
			CoverWallSetup.WallBounds wallBounds;
			if (!this.TryGetVisualBounds(out wallBounds))
			{
				return;
			}
			if (minEdge)
			{
				wallBounds.min.x = Mathf.Min(newLocalX, wallBounds.max.x - 0.3f);
			}
			else
			{
				wallBounds.max.x = Mathf.Max(newLocalX, wallBounds.min.x + 0.3f);
			}
			this.SetVisualBounds(wallBounds);
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x001BA2BC File Offset: 0x001B84BC
		public void SetTopY(float newLocalY)
		{
			CoverWallSetup.WallBounds wallBounds;
			if (!this.TryGetVisualBounds(out wallBounds))
			{
				return;
			}
			wallBounds.max.y = wallBounds.min.y + this.ClampHeight(newLocalY - wallBounds.min.y);
			this.SetVisualBounds(wallBounds);
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x001BA308 File Offset: 0x001B8508
		public bool EnsureStanceInitialized()
		{
			CoverWallSetup.WallBounds wallBounds;
			return !this.stanceInitialized && this.TryGetVisualBounds(out wallBounds) && this.EnsureStanceInitialized(wallBounds);
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x001BA332 File Offset: 0x001B8532
		private bool EnsureStanceInitialized(CoverWallSetup.WallBounds bounds)
		{
			if (this.stanceInitialized)
			{
				return false;
			}
			this.stance = ((bounds.Height < 1.5f) ? CoverWallSetup.Stance.Crouched : CoverWallSetup.Stance.Standing);
			this.stanceInitialized = true;
			return true;
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x001BA360 File Offset: 0x001B8560
		[ContextMenu("Rebuild Cover Wall")]
		public void Rebuild()
		{
			this.ClampSettings();
			this.EnsureWallVisual();
			CoverWallSetup.WallBounds wallBounds;
			if (!this.TryGetVisualBounds(out wallBounds))
			{
				Debug.LogWarning("[CoverWallSetup] Wall_Visual has no mesh: cannot rebuild.", this);
				return;
			}
			this.EnsureStanceInitialized(wallBounds);
			float num = this.ClampHeight(wallBounds.Height);
			if (!Mathf.Approximately(num, wallBounds.Height))
			{
				wallBounds.max.y = wallBounds.min.y + num;
				this.SetVisualBounds(wallBounds);
				this.TryGetVisualBounds(out wallBounds);
			}
			this.length = wallBounds.Length;
			this.height = wallBounds.Height;
			this.thickness = wallBounds.Thickness;
			this.EnsureCoverLine();
			this.EnsureCoverLineHierarchy();
			this.ApplyCoverLineSettings();
			this.RebuildControlPoints(wallBounds);
			List<vCoverPoint> list = this.RebuildCoverPoints(wallBounds);
			this.ApplyPeekRanges(list, wallBounds);
			this.EnsureInteractable();
			this.RecordEditorChanges();
			if (Application.isPlaying)
			{
				Physics.SyncTransforms();
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x001BA445 File Offset: 0x001B8645
		public int CalculateCoverPointCount()
		{
			return this.CalculateCoverPointCount(this.length);
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x001BA454 File Offset: 0x001B8654
		public int CalculateCoverPointCount(float wallLength)
		{
			float num = Mathf.Max(0.1f, this.coverPointWidth);
			float num2 = Mathf.Max(0.3f, wallLength) / num;
			int num3 = Mathf.FloorToInt(num2);
			if (num2 - (float)num3 > 0.5f)
			{
				num3++;
			}
			return Mathf.Max(1, num3);
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x001BA49C File Offset: 0x001B869C
		private void ClampSettings()
		{
			this.coverPointWidth = Mathf.Clamp(this.coverPointWidth, 0.1f, 1f);
			this.coverOffsetZ = Mathf.Clamp(this.coverOffsetZ, 0f, 1f);
			this.coverDepth = Mathf.Clamp(this.coverDepth, 0.2f, 1f);
			this.leftPeekRange = Mathf.Max(0.1f, this.leftPeekRange);
			this.rightPeekRange = Mathf.Max(0.1f, this.rightPeekRange);
			this.peekCornerMargin = Mathf.Max(0f, this.peekCornerMargin);
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x001BA53C File Offset: 0x001B873C
		private void EnsureWallVisual()
		{
			if (this.wallVisual == null)
			{
				this.wallVisual = base.transform.Find("Wall_Visual");
			}
			if (this.wallVisual == null)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				CoverWallSetup.RegisterCreatedObject(gameObject, "Create Wall Cover visual");
				gameObject.name = "Wall_Visual";
				gameObject.transform.SetParent(base.transform, false);
				gameObject.transform.localPosition = new Vector3(0f, 1f, 0f);
				gameObject.transform.localScale = new Vector3(4f, 2f, 0.3f);
				this.wallVisual = gameObject.transform;
				MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			this.wallVisual.gameObject.SetActive(true);
			if (this.wallVisual.GetComponent<MeshFilter>() != null && this.wallVisual.GetComponent<Collider>() == null)
			{
				BoxCollider boxCollider = CoverWallSetup.AddComponentWithUndo<BoxCollider>(this.wallVisual.gameObject);
				boxCollider.center = Vector3.zero;
				boxCollider.size = Vector3.one;
			}
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x001BA66C File Offset: 0x001B886C
		private void ApplyTechnicalRendererMode()
		{
			if (this.wallVisual == null)
			{
				this.wallVisual = base.transform.Find("Wall_Visual");
			}
			MeshRenderer meshRenderer;
			if (this.wallVisual != null && this.wallVisual.TryGetComponent<MeshRenderer>(out meshRenderer))
			{
				meshRenderer.forceRenderingOff = Application.isPlaying;
			}
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x001BA6C8 File Offset: 0x001B88C8
		private void EnsureCoverLine()
		{
			if (this.coverLine == null)
			{
				Transform transform = base.transform.Find("Cover Line");
				if (transform != null)
				{
					this.coverLine = transform.GetComponent<vCoverLine>();
				}
			}
			if (this.coverLine == null)
			{
				GameObject gameObject = CoverWallSetup.CreateChild("Cover Line", base.transform);
				this.coverLine = CoverWallSetup.AddComponentWithUndo<vCoverLine>(gameObject);
			}
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x001BA734 File Offset: 0x001B8934
		private void EnsureCoverLineHierarchy()
		{
			Transform transform = this.coverLine.CtrlPTransform;
			if (transform == null)
			{
				transform = this.coverLine.transform.Find("ControlPoints");
			}
			if (transform == null)
			{
				transform = CoverWallSetup.CreateChild("ControlPoints", this.coverLine.transform).transform;
			}
			Transform transform2 = this.coverLine.CPTransform;
			if (transform2 == null)
			{
				transform2 = this.coverLine.transform.Find("CoverPoints");
			}
			if (transform2 == null)
			{
				transform2 = CoverWallSetup.CreateChild("CoverPoints", this.coverLine.transform).transform;
			}
			this.coverLine.CtrlPTransform = transform;
			this.coverLine.CPTransform = transform2;
			CoverWallSetup.ResetLocalTransform(transform);
			CoverWallSetup.ResetLocalTransform(transform2);
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x001BA804 File Offset: 0x001B8A04
		private void ApplyCoverLineSettings()
		{
			this.coverLine.width = this.coverPointWidth;
			this.coverLine.height = this.height;
			this.coverLine.depht = this.coverDepth;
			this.coverLine.offsetZ = this.coverOffsetZ;
			this.coverLine.close = false;
			this.coverLine.coverPointTag = "CoverPoint";
			this.coverLine.coverPointLayer = "CoverPoint";
			CoverWallSetup.ResetLocalTransform(this.coverLine.transform);
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x001BA894 File Offset: 0x001B8A94
		private void RebuildControlPoints(CoverWallSetup.WallBounds b)
		{
			List<vCoverLinePoint> directChildren = CoverWallSetup.GetDirectChildren<vCoverLinePoint>(this.coverLine.CtrlPTransform);
			while (directChildren.Count < 2)
			{
				GameObject gameObject = CoverWallSetup.CreateChild("P-" + (directChildren.Count + 1).ToString(), this.coverLine.CtrlPTransform);
				directChildren.Add(CoverWallSetup.AddComponentWithUndo<vCoverLinePoint>(gameObject));
			}
			for (int i = 0; i < directChildren.Count; i++)
			{
				directChildren[i].gameObject.SetActive(i < 2);
			}
			vCoverLinePoint vCoverLinePoint = directChildren[0];
			vCoverLinePoint vCoverLinePoint2 = directChildren[1];
			float z = b.max.z;
			CoverWallSetup.ConfigureControlPoint(vCoverLinePoint, "P-1", new Vector3(b.min.x, b.min.y, z));
			CoverWallSetup.ConfigureControlPoint(vCoverLinePoint2, "P-2", new Vector3(b.max.x, b.min.y, z));
			float num = b.Length * 0.5f;
			vCoverLinePoint.tangentLeft = Vector3.one;
			vCoverLinePoint.tangentRight = Vector3.right * num;
			vCoverLinePoint.conectedLeft = null;
			vCoverLinePoint.conectedRight = vCoverLinePoint2;
			vCoverLinePoint2.tangentLeft = Vector3.left * num;
			vCoverLinePoint2.tangentRight = -Vector3.one;
			vCoverLinePoint2.conectedLeft = vCoverLinePoint;
			vCoverLinePoint2.conectedRight = null;
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x001BA9F8 File Offset: 0x001B8BF8
		private static void ConfigureControlPoint(vCoverLinePoint point, string pointName, Vector3 localPosition)
		{
			point.gameObject.name = pointName;
			point.transform.localPosition = localPosition;
			point.transform.localRotation = Quaternion.identity;
			point.transform.localScale = Vector3.one;
			point.curveType = vCoverLine.CurveType.Linear;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x001BAA44 File Offset: 0x001B8C44
		private List<vCoverPoint> RebuildCoverPoints(CoverWallSetup.WallBounds b)
		{
			int num = this.CalculateCoverPointCount(b.Length);
			List<vCoverPoint> directChildren = CoverWallSetup.GetDirectChildren<vCoverPoint>(this.coverLine.CPTransform);
			while (directChildren.Count < num)
			{
				GameObject gameObject = CoverWallSetup.CreateChild("CP-" + (directChildren.Count + 1).ToString("00"), this.coverLine.CPTransform);
				directChildren.Add(CoverWallSetup.AddComponentWithUndo<vCoverPoint>(gameObject));
			}
			List<vCoverPoint> list = new List<vCoverPoint>(num);
			float num2 = b.Length / (float)num;
			float num3 = b.max.z + this.coverOffsetZ;
			int num4 = LayerMask.NameToLayer(this.coverLine.coverPointLayer);
			for (int i = 0; i < directChildren.Count; i++)
			{
				vCoverPoint vCoverPoint = directChildren[i];
				bool flag = i < num;
				vCoverPoint.gameObject.SetActive(flag);
				if (!flag)
				{
					CoverWallSetup.ClearPoint(vCoverPoint);
				}
				else
				{
					vCoverPoint.gameObject.name = "CP-" + (i + 1).ToString("00");
					if (!string.IsNullOrEmpty(this.coverLine.coverPointTag))
					{
						vCoverPoint.gameObject.tag = this.coverLine.coverPointTag;
					}
					if (num4 >= 0)
					{
						vCoverPoint.gameObject.layer = num4;
					}
					float num5 = b.min.x + num2 * ((float)i + 0.5f);
					vCoverPoint.transform.localPosition = new Vector3(num5, b.min.y, num3);
					vCoverPoint.transform.localRotation = Quaternion.identity;
					vCoverPoint.transform.localScale = Vector3.one;
					BoxCollider boxCollider = vCoverPoint.GetComponent<BoxCollider>();
					if (boxCollider == null)
					{
						boxCollider = CoverWallSetup.AddComponentWithUndo<BoxCollider>(vCoverPoint.gameObject);
					}
					boxCollider.enabled = true;
					boxCollider.isTrigger = true;
					boxCollider.size = new Vector3(num2, b.Height, this.coverDepth);
					boxCollider.center = new Vector3(0f, b.Height * 0.5f, this.coverDepth * 0.5f);
					if (vCoverPoint.onEnterCover == null)
					{
						vCoverPoint.onEnterCover = new vCoverPoint.CoverEvent();
					}
					vCoverPoint.manualConnections = true;
					vCoverPoint.autoDetectCorner = false;
					vCoverPoint.mask = 1 | ((num4 >= 0) ? (1 << num4) : 0);
					vCoverPoint.corner = (vCoverPoint.Corner)0;
					vCoverPoint.offsetPosePositionX = 0f;
					vCoverPoint.rayCastNeighborOffsetX = 0.15f;
					vCoverPoint.leftCorner = null;
					vCoverPoint.rightCorner = null;
					vCoverPoint.isValid = true;
					if (!Application.isPlaying)
					{
						vCoverPoint.isOccuped = false;
					}
					list.Add(vCoverPoint);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				vCoverPoint vCoverPoint2 = list[j];
				vCoverPoint2.left = ((j < list.Count - 1) ? list[j + 1] : null);
				vCoverPoint2.right = ((j > 0) ? list[j - 1] : null);
			}
			return list;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x001BAD58 File Offset: 0x001B8F58
		private void ApplyPeekRanges(List<vCoverPoint> activePoints, CoverWallSetup.WallBounds b)
		{
			List<vCoverPoint> directChildren = CoverWallSetup.GetDirectChildren<vCoverPoint>(this.coverLine.CPTransform);
			for (int i = 0; i < directChildren.Count; i++)
			{
				this.ClearPeekRanges(directChildren[i]);
			}
			if (activePoints.Count == 0)
			{
				return;
			}
			vCoverPoint vCoverPoint = activePoints[0];
			vCoverPoint vCoverPoint2 = activePoints[activePoints.Count - 1];
			float num = b.Length / (float)activePoints.Count;
			if (this.HasLeftPeek)
			{
				vCoverPoint2.corner |= vCoverPoint.Corner.Left;
				float num2 = b.max.x - vCoverPoint2.transform.localPosition.x;
				float num3 = num2 - this.peekCornerMargin;
				float num4 = Mathf.Max(-num * 0.5f, num2 - this.leftPeekRange);
				this.SetupPeek(vCoverPoint2, true, num4, num3);
			}
			if (this.HasRightPeek)
			{
				vCoverPoint.corner |= vCoverPoint.Corner.Right;
				float num5 = b.min.x - vCoverPoint.transform.localPosition.x;
				float num6 = num5 + this.peekCornerMargin;
				float num7 = Mathf.Min(num * 0.5f, num5 + this.rightPeekRange);
				this.SetupPeek(vCoverPoint, false, num7, num6);
			}
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x001BAE90 File Offset: 0x001B9090
		private void ClearPeekRanges(vCoverPoint point)
		{
			point.peekLeftRangeStart = null;
			point.peekLeftRangeEnd = null;
			point.peekRightRangeStart = null;
			point.peekRightRangeEnd = null;
			CoverWallSetup.SetNamedChildActive(point.transform, "PeekLeftStart", false);
			CoverWallSetup.SetNamedChildActive(point.transform, "PeekLeftEnd", false);
			CoverWallSetup.SetNamedChildActive(point.transform, "PeekRightStart", false);
			CoverWallSetup.SetNamedChildActive(point.transform, "PeekRightEnd", false);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x001BAF00 File Offset: 0x001B9100
		private void SetupPeek(vCoverPoint point, bool isLeftPeek, float startLocalX, float endLocalX)
		{
			string text = (isLeftPeek ? "PeekLeftStart" : "PeekRightStart");
			string text2 = (isLeftPeek ? "PeekLeftEnd" : "PeekRightEnd");
			Transform transform = CoverWallSetup.FindOrCreateChild(point.transform, text);
			Transform transform2 = CoverWallSetup.FindOrCreateChild(point.transform, text2);
			transform.gameObject.SetActive(true);
			transform2.gameObject.SetActive(true);
			transform.localPosition = new Vector3(startLocalX, 0f, 0f);
			transform2.localPosition = new Vector3(endLocalX, 0f, 0f);
			transform.localRotation = Quaternion.identity;
			transform2.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			transform2.localScale = Vector3.one;
			if (isLeftPeek)
			{
				point.peekLeftRangeStart = transform;
				point.peekLeftRangeEnd = transform2;
				return;
			}
			point.peekRightRangeStart = transform;
			point.peekRightRangeEnd = transform2;
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x001BAFD8 File Offset: 0x001B91D8
		private void EnsureInteractable()
		{
			if (this.interactable == null)
			{
				this.interactable = base.GetComponent<CoverWallInteractable>();
			}
			if (this.interactable == null)
			{
				this.interactable = CoverWallSetup.AddComponentWithUndo<CoverWallInteractable>(base.gameObject);
			}
			if (Application.isPlaying)
			{
				this.interactable.SetInteractionConfig(this.interactionConfig);
				return;
			}
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x001BB038 File Offset: 0x001B9238
		private static void ClearPoint(vCoverPoint point)
		{
			point.autoDetectCorner = false;
			point.left = null;
			point.right = null;
			point.leftCorner = null;
			point.rightCorner = null;
			point.corner = (vCoverPoint.Corner)0;
			point.peekLeftRangeStart = null;
			point.peekLeftRangeEnd = null;
			point.peekRightRangeStart = null;
			point.peekRightRangeEnd = null;
			BoxCollider component = point.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x001BB0A4 File Offset: 0x001B92A4
		private static List<T> GetDirectChildren<T>(Transform parent) where T : Component
		{
			List<T> list = new List<T>();
			if (parent == null)
			{
				return list;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				T component = parent.GetChild(i).GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
			}
			return list;
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x001BB0F6 File Offset: 0x001B92F6
		private static void ResetLocalTransform(Transform target)
		{
			target.localPosition = Vector3.zero;
			target.localRotation = Quaternion.identity;
			target.localScale = Vector3.one;
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x001BB11C File Offset: 0x001B931C
		private static void SetNamedChildActive(Transform parent, string childName, bool active)
		{
			Transform transform = parent.Find(childName);
			if (transform != null)
			{
				transform.gameObject.SetActive(active);
			}
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x001BB148 File Offset: 0x001B9348
		private static Transform FindOrCreateChild(Transform parent, string childName)
		{
			Transform transform = parent.Find(childName);
			if (transform != null)
			{
				return transform;
			}
			return CoverWallSetup.CreateChild(childName, parent).transform;
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x001BB174 File Offset: 0x001B9374
		private static GameObject CreateChild(string childName, Transform parent)
		{
			GameObject gameObject = new GameObject(childName);
			CoverWallSetup.RegisterCreatedObject(gameObject, "Create Wall Cover child");
			gameObject.transform.SetParent(parent, false);
			return gameObject;
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x001BB194 File Offset: 0x001B9394
		private static T AddComponentWithUndo<T>(GameObject target) where T : Component
		{
			return target.AddComponent<T>();
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x00004F31 File Offset: 0x00003131
		private static void RegisterCreatedObject(Object target, string undoName)
		{
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00004F31 File Offset: 0x00003131
		private void RecordEditorChanges()
		{
		}

		// Token: 0x04004875 RID: 18549
		public const float MinLength = 0.3f;

		// Token: 0x04004876 RID: 18550
		public const float MinHeight = 0.3f;

		// Token: 0x04004877 RID: 18551
		public const float MinThickness = 0.02f;

		// Token: 0x04004878 RID: 18552
		public const float CrouchMaxHeight = 1.3f;

		// Token: 0x04004879 RID: 18553
		public const float StandingMinHeight = 1.6f;

		// Token: 0x0400487A RID: 18554
		[Header("Dimensions (derived from Wall_Visual mesh, read-only)")]
		[SerializeField]
		private float length = 4f;

		// Token: 0x0400487B RID: 18555
		[SerializeField]
		private float height = 2f;

		// Token: 0x0400487C RID: 18556
		[SerializeField]
		private float thickness = 0.3f;

		// Token: 0x0400487D RID: 18557
		[Header("Stance")]
		[Tooltip("Standing = wall at least 1.6 m (Ciel stays up). Crouched = wall at most 1.3 m (Ciel ducks behind it). The height is clamped accordingly.")]
		public CoverWallSetup.Stance stance;

		// Token: 0x0400487E RID: 18558
		[Tooltip("Let Ciel toggle between standing and crouched with C while in cover, in both directions.")]
		public bool allowCrouchToggle = true;

		// Token: 0x0400487F RID: 18559
		[SerializeField]
		[HideInInspector]
		private bool stanceInitialized;

		// Token: 0x04004880 RID: 18560
		[Header("Cover")]
		[Tooltip("Approximate width of every CoverPoint along the wall.")]
		[Range(0.1f, 1f)]
		public float coverPointWidth = 1f;

		// Token: 0x04004881 RID: 18561
		[Tooltip("Safety gap from the player-facing +Z wall surface to the CoverPoint line.")]
		[Range(0f, 1f)]
		public float coverOffsetZ = 0.08f;

		// Token: 0x04004882 RID: 18562
		[Tooltip("Depth of the CoverPoint trigger volume, towards the player.")]
		[Range(0.2f, 1f)]
		public float coverDepth = 0.5f;

		// Token: 0x04004883 RID: 18563
		[Header("Peek")]
		public CoverWallSetup.PeekMode peekMode = CoverWallSetup.PeekMode.Both;

		// Token: 0x04004884 RID: 18564
		[Tooltip("Length (from the corner towards the wall center) of the LEFT peek range.")]
		[Min(0.1f)]
		public float leftPeekRange = 0.8f;

		// Token: 0x04004885 RID: 18565
		[Tooltip("Length (from the corner towards the wall center) of the RIGHT peek range.")]
		[Min(0.1f)]
		public float rightPeekRange = 0.8f;

		// Token: 0x04004886 RID: 18566
		[Tooltip("Small margin between the corner and the end of the peek range.")]
		[Min(0f)]
		public float peekCornerMargin = 0.05f;

		// Token: 0x04004887 RID: 18567
		[Header("Slide")]
		[Tooltip("Max speed (m/s) Ciel slides along this cover. 0 = unlimited (Invector default: motor + friction).")]
		[Min(0f)]
		public float maxSlideSpeed;

		// Token: 0x04004888 RID: 18568
		[Header("Interaction")]
		[Tooltip("Actions shown in the HUD while the player is in cover.")]
		public InteractionConfigSO interactionConfig;

		// Token: 0x04004889 RID: 18569
		[Header("References (auto-managed)")]
		public Transform wallVisual;

		// Token: 0x0400488A RID: 18570
		public vCoverLine coverLine;

		// Token: 0x0400488B RID: 18571
		public CoverWallInteractable interactable;

		// Token: 0x0400488C RID: 18572
		public const string WallVisualName = "Wall_Visual";

		// Token: 0x0400488D RID: 18573
		private const string CoverLineName = "Cover Line";

		// Token: 0x0400488E RID: 18574
		private const string ControlPointsName = "ControlPoints";

		// Token: 0x0400488F RID: 18575
		private const string CoverPointsName = "CoverPoints";

		// Token: 0x04004890 RID: 18576
		private const string PeekLeftStartName = "PeekLeftStart";

		// Token: 0x04004891 RID: 18577
		private const string PeekLeftEndName = "PeekLeftEnd";

		// Token: 0x04004892 RID: 18578
		private const string PeekRightStartName = "PeekRightStart";

		// Token: 0x04004893 RID: 18579
		private const string PeekRightEndName = "PeekRightEnd";

		// Token: 0x020009F5 RID: 2549
		public enum PeekMode
		{
			// Token: 0x04004895 RID: 18581
			None,
			// Token: 0x04004896 RID: 18582
			Left,
			// Token: 0x04004897 RID: 18583
			Right,
			// Token: 0x04004898 RID: 18584
			Both
		}

		// Token: 0x020009F6 RID: 2550
		public enum Stance
		{
			// Token: 0x0400489A RID: 18586
			Standing,
			// Token: 0x0400489B RID: 18587
			Crouched
		}

		// Token: 0x020009F7 RID: 2551
		public struct WallBounds
		{
			// Token: 0x17000C4E RID: 3150
			// (get) Token: 0x060048B1 RID: 18609 RVA: 0x001BB220 File Offset: 0x001B9420
			public Vector3 Center
			{
				get
				{
					return (this.min + this.max) * 0.5f;
				}
			}

			// Token: 0x17000C4F RID: 3151
			// (get) Token: 0x060048B2 RID: 18610 RVA: 0x001BB23D File Offset: 0x001B943D
			public Vector3 Size
			{
				get
				{
					return this.max - this.min;
				}
			}

			// Token: 0x17000C50 RID: 3152
			// (get) Token: 0x060048B3 RID: 18611 RVA: 0x001BB250 File Offset: 0x001B9450
			public float Length
			{
				get
				{
					return this.max.x - this.min.x;
				}
			}

			// Token: 0x17000C51 RID: 3153
			// (get) Token: 0x060048B4 RID: 18612 RVA: 0x001BB269 File Offset: 0x001B9469
			public float Height
			{
				get
				{
					return this.max.y - this.min.y;
				}
			}

			// Token: 0x17000C52 RID: 3154
			// (get) Token: 0x060048B5 RID: 18613 RVA: 0x001BB282 File Offset: 0x001B9482
			public float Thickness
			{
				get
				{
					return this.max.z - this.min.z;
				}
			}

			// Token: 0x17000C53 RID: 3155
			// (get) Token: 0x060048B6 RID: 18614 RVA: 0x001BB29B File Offset: 0x001B949B
			public bool IsValid
			{
				get
				{
					return this.Length > 0.001f && this.Height > 0.001f;
				}
			}

			// Token: 0x0400489C RID: 18588
			public Vector3 min;

			// Token: 0x0400489D RID: 18589
			public Vector3 max;
		}
	}
}
