using System;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.CR
{
	// Token: 0x02000003 RID: 3
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/D.A. CornerRounder (CR)")]
	public class CornerRounder : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020BB File Offset: 0x000002BB
		private Vector4 _radii
		{
			get
			{
				return this.GetNormalizedRadii();
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C4 File Offset: 0x000002C4
		private Vector4 GetNormalizedRadii()
		{
			float num = Mathf.Abs(base.GetComponent<RectTransform>().GetWidth());
			float num2 = Mathf.Abs(base.GetComponent<RectTransform>().GetHeight());
			return new Vector4
			{
				x = (float)this.radiiSerialized.x.NormalizeAngleToSize(num, num2),
				y = (float)this.radiiSerialized.y.NormalizeAngleToSize(num, num2),
				z = (float)this.radiiSerialized.z.NormalizeAngleToSize(num, num2),
				w = (float)this.radiiSerialized.w.NormalizeAngleToSize(num, num2)
			};
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002164 File Offset: 0x00000364
		public void SetRadii(Vector4 r)
		{
			if (r.x == r.y && r.x == r.z && r.x == r.w)
			{
				this.independent = false;
			}
			else
			{
				this.independent = true;
			}
			this.radiiSerialized = r;
			this.Refresh();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021B8 File Offset: 0x000003B8
		private void OnValidate()
		{
			this.Validate();
			this.Refresh();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021C6 File Offset: 0x000003C6
		private void OnEnable()
		{
			this.Validate();
			this.Refresh();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021D4 File Offset: 0x000003D4
		private void OnRectTransformDimensionsChange()
		{
			if (base.enabled && this.material != null)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021F2 File Offset: 0x000003F2
		private void OnDestroy()
		{
			this.image.material = null;
			Object.Destroy(this.material);
			this.image = null;
			this.material = null;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000221C File Offset: 0x0000041C
		public void Validate()
		{
			if (this.material == null)
			{
				try
				{
					this.material = new Material(Shader.Find("UI/RoundedCorners/CornerRounder"));
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			if (this.image == null)
			{
				base.TryGetComponent<MaskableGraphic>(out this.image);
			}
			if (this.image != null)
			{
				this.image.material = this.material;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022A0 File Offset: 0x000004A0
		public void Refresh()
		{
			Rect rect = ((RectTransform)base.transform).rect;
			this.RecalculateProps(rect.size);
			if (this.material != null)
			{
				this.material.SetVector(CornerRounder.prop_rect2props, this.rect2props);
				this.material.SetVector(CornerRounder.prop_halfSize, rect.size * 0.5f);
				this.material.SetVector(CornerRounder.prop_radiuses, this._radii);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000232C File Offset: 0x0000052C
		private void RecalculateProps(Vector2 size)
		{
			float num = Vector2.Dot(new Vector2(size.x, -size.y + this._radii.x + this._radii.z), CornerRounder.wNorm) * 0.5f;
			this.rect2props.z = num;
			float num2 = Vector2.Dot(new Vector2(size.x, size.y - this._radii.w - this._radii.y), CornerRounder.hNorm) * 0.5f;
			this.rect2props.w = num2;
			Vector2 vector = new Vector2(size.x - this._radii.x - this._radii.y, 0f);
			Vector2 vector2 = CornerRounder.hNorm * Vector2.Dot(vector, CornerRounder.hNorm);
			Vector2 vector3 = new Vector2(this._radii.x - size.x / 2f, size.y / 2f) + vector2 + CornerRounder.wNorm * num + CornerRounder.hNorm * -num2;
			this.rect2props.x = vector3.x;
			this.rect2props.y = vector3.y;
		}

		// Token: 0x04000001 RID: 1
		private static readonly int prop_halfSize = Shader.PropertyToID("_halfSize");

		// Token: 0x04000002 RID: 2
		private static readonly int prop_radiuses = Shader.PropertyToID("_r");

		// Token: 0x04000003 RID: 3
		private static readonly int prop_rect2props = Shader.PropertyToID("_rect2props");

		// Token: 0x04000004 RID: 4
		private static readonly Vector2 wNorm = new Vector2(0.7071068f, -0.7071068f);

		// Token: 0x04000005 RID: 5
		private static readonly Vector2 hNorm = new Vector2(0.7071068f, 0.7071068f);

		// Token: 0x04000006 RID: 6
		public bool independent;

		// Token: 0x04000007 RID: 7
		public Vector4 radiiSerialized = new Vector4(40f, 40f, 40f, 40f);

		// Token: 0x04000008 RID: 8
		private Material material;

		// Token: 0x04000009 RID: 9
		[HideInInspector]
		[SerializeField]
		private Vector4 rect2props;

		// Token: 0x0400000A RID: 10
		[HideInInspector]
		[SerializeField]
		private MaskableGraphic image;
	}
}
