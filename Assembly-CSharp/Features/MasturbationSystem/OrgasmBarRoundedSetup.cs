using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FFB RID: 4091
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	public class OrgasmBarRoundedSetup : MonoBehaviour
	{
		// Token: 0x06007C0F RID: 31759 RVA: 0x0032AFF0 File Offset: 0x003291F0
		private void Awake()
		{
			this._rt = (RectTransform)base.transform;
			if (this.targetImage == null)
			{
				base.TryGetComponent<Image>(out this.targetImage);
			}
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x0032B01E File Offset: 0x0032921E
		private void OnEnable()
		{
			this.EnsureMaterialInstance();
			this.Refresh(true);
		}

		// Token: 0x06007C11 RID: 31761 RVA: 0x0032B02D File Offset: 0x0032922D
		private void OnRectTransformDimensionsChange()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.Refresh(false);
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x0032B03F File Offset: 0x0032923F
		private void OnValidate()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.EnsureMaterialInstance();
			this.Refresh(true);
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x0032B058 File Offset: 0x00329258
		private void EnsureMaterialInstance()
		{
			if (this._rt == null)
			{
				this._rt = (RectTransform)base.transform;
			}
			if (this.targetImage == null)
			{
				base.TryGetComponent<Image>(out this.targetImage);
			}
			if (this.targetImage == null)
			{
				return;
			}
			OrgasmMeterHUD componentInParent = base.GetComponentInParent<OrgasmMeterHUD>(true);
			Material material = ((componentInParent != null) ? componentInParent.GetFillMaterial(this.targetImage) : null);
			if (material != null)
			{
				if (this._materialInstance != material)
				{
					this.ReleaseOwnedMaterial();
					this._materialInstance = material;
					this._lastSize = new Vector2(-1f, -1f);
				}
				return;
			}
			if (this._materialInstance == null || this._materialInstance != this.targetImage.material)
			{
				this.ReleaseOwnedMaterial();
				this._materialInstance = this.targetImage.material;
				if (this._materialInstance != null && !this._materialInstance.name.EndsWith("(Instance)"))
				{
					this._materialInstance = new Material(this._materialInstance)
					{
						name = this._materialInstance.name + " (Instance)"
					};
					this._ownsMaterialInstance = true;
					this.targetImage.material = this._materialInstance;
					if (this.debugMode)
					{
						Debug.Log("[OrgasmBarRoundedSetup] Instanced material to avoid sharing writes.");
					}
				}
				this._lastSize = new Vector2(-1f, -1f);
			}
		}

		// Token: 0x06007C14 RID: 31764 RVA: 0x0032B1D9 File Offset: 0x003293D9
		private void OnDestroy()
		{
			this.ReleaseOwnedMaterial();
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x0032B1E4 File Offset: 0x003293E4
		private void ReleaseOwnedMaterial()
		{
			if (this._ownsMaterialInstance && this._materialInstance != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._materialInstance);
				}
				else
				{
					Object.DestroyImmediate(this._materialInstance);
				}
			}
			this._ownsMaterialInstance = false;
			this._materialInstance = null;
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x0032B234 File Offset: 0x00329434
		private void Refresh(bool force)
		{
			this.EnsureMaterialInstance();
			if (this._materialInstance == null || this._rt == null)
			{
				return;
			}
			Vector2 size = this._rt.rect.size;
			if (!force && size == this._lastSize)
			{
				return;
			}
			this._lastSize = size;
			Vector4 vector = OrgasmBarRoundedSetup.NormalizeRadii(this.radii, Mathf.Abs(size.x), Mathf.Abs(size.y));
			Vector4 vector2 = OrgasmBarRoundedSetup.RecalculateRect2Props(size, vector);
			this._materialInstance.SetVector(OrgasmBarRoundedSetup.prop_radiuses, vector);
			this._materialInstance.SetVector(OrgasmBarRoundedSetup.prop_halfSize, new Vector4(size.x * 0.5f, size.y * 0.5f, 0f, 0f));
			this._materialInstance.SetVector(OrgasmBarRoundedSetup.prop_rect2props, vector2);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[OrgasmBarRoundedSetup] size={0} radii={1} rect2={2}", size, vector, vector2));
			}
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x0032B340 File Offset: 0x00329540
		private static Vector4 NormalizeRadii(Vector4 raw, float width, float height)
		{
			float num = Mathf.Min(width, height) * 0.5f;
			return new Vector4(Mathf.Clamp(raw.x, 0f, num), Mathf.Clamp(raw.y, 0f, num), Mathf.Clamp(raw.z, 0f, num), Mathf.Clamp(raw.w, 0f, num));
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x0032B3A4 File Offset: 0x003295A4
		private static Vector4 RecalculateRect2Props(Vector2 size, Vector4 r)
		{
			Vector4 zero = Vector4.zero;
			float num = Vector2.Dot(new Vector2(size.x, -size.y + r.x + r.z), OrgasmBarRoundedSetup.wNorm) * 0.5f;
			zero.z = num;
			float num2 = Vector2.Dot(new Vector2(size.x, size.y - r.w - r.y), OrgasmBarRoundedSetup.hNorm) * 0.5f;
			zero.w = num2;
			Vector2 vector = new Vector2(size.x - r.x - r.y, 0f);
			Vector2 vector2 = OrgasmBarRoundedSetup.hNorm * Vector2.Dot(vector, OrgasmBarRoundedSetup.hNorm);
			Vector2 vector3 = new Vector2(r.x - size.x / 2f, size.y / 2f) + vector2 + OrgasmBarRoundedSetup.wNorm * num + OrgasmBarRoundedSetup.hNorm * -num2;
			zero.x = vector3.x;
			zero.y = vector3.y;
			return zero;
		}

		// Token: 0x04007FD4 RID: 32724
		[Header("References")]
		[Tooltip("Image il cui materiale ricevera' i parametri SDF. Se null, prende l'Image del GameObject.")]
		[SerializeField]
		private Image targetImage;

		// Token: 0x04007FD5 RID: 32725
		[Tooltip("Raggi dei 4 corner in pixel (top-left, top-right, bottom-right, bottom-left). Match il CornerRounder del Container per coerenza visiva.")]
		[SerializeField]
		private Vector4 radii = new Vector4(40f, 40f, 40f, 40f);

		// Token: 0x04007FD6 RID: 32726
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007FD7 RID: 32727
		private static readonly int prop_halfSize = Shader.PropertyToID("_halfSize");

		// Token: 0x04007FD8 RID: 32728
		private static readonly int prop_radiuses = Shader.PropertyToID("_r");

		// Token: 0x04007FD9 RID: 32729
		private static readonly int prop_rect2props = Shader.PropertyToID("_rect2props");

		// Token: 0x04007FDA RID: 32730
		private static readonly Vector2 wNorm = new Vector2(0.7071068f, -0.7071068f);

		// Token: 0x04007FDB RID: 32731
		private static readonly Vector2 hNorm = new Vector2(0.7071068f, 0.7071068f);

		// Token: 0x04007FDC RID: 32732
		private RectTransform _rt;

		// Token: 0x04007FDD RID: 32733
		private Material _materialInstance;

		// Token: 0x04007FDE RID: 32734
		private bool _ownsMaterialInstance;

		// Token: 0x04007FDF RID: 32735
		private Vector2 _lastSize = new Vector2(-1f, -1f);
	}
}
