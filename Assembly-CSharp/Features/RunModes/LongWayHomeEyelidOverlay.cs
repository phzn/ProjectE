using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.RunModes
{
	// Token: 0x02000CB4 RID: 3252
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(20010)]
	public sealed class LongWayHomeEyelidOverlay : MonoBehaviour
	{
		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06005E60 RID: 24160 RVA: 0x0025560F File Offset: 0x0025380F
		// (set) Token: 0x06005E61 RID: 24161 RVA: 0x00255617 File Offset: 0x00253817
		public bool Visible { get; set; }

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06005E62 RID: 24162 RVA: 0x00255620 File Offset: 0x00253820
		public float Closed
		{
			get
			{
				return this._closed;
			}
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x00255628 File Offset: 0x00253828
		public void Bind(SkinnedMeshRenderer face)
		{
			this._face = face;
			this._mesh = null;
			this.ResolveShapes();
		}

		// Token: 0x06005E64 RID: 24164 RVA: 0x00255640 File Offset: 0x00253840
		private void Awake()
		{
			GameObject gameObject = new GameObject("Long Way Home - Eyelids");
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.AddComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = 900;
			this._canvas.enabled = false;
			this._gradient = LongWayHomeEyelidOverlay.BuildGradient();
			this._top = this.CreateLid("Top Lid", gameObject.transform, true);
			this._bottom = this.CreateLid("Bottom Lid", gameObject.transform, false);
		}

		// Token: 0x06005E65 RID: 24165 RVA: 0x002556DC File Offset: 0x002538DC
		private RectTransform CreateLid(string lidName, Transform parent, bool top)
		{
			GameObject gameObject = new GameObject(lidName, new Type[] { typeof(RectTransform) });
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMin = new Vector2(0f, top ? 1f : 0f);
			rectTransform.anchorMax = new Vector2(1f, top ? 1f : 0f);
			rectTransform.pivot = new Vector2(0.5f, top ? 1f : 0f);
			rectTransform.sizeDelta = Vector2.zero;
			RawImage rawImage = gameObject.AddComponent<RawImage>();
			rawImage.texture = this._gradient;
			rawImage.color = Color.black;
			rawImage.raycastTarget = false;
			rawImage.uvRect = (top ? new Rect(0f, 1f, 1f, -1f) : new Rect(0f, 0f, 1f, 1f));
			return rectTransform;
		}

		// Token: 0x06005E66 RID: 24166 RVA: 0x002557E0 File Offset: 0x002539E0
		private static Texture2D BuildGradient()
		{
			Texture2D texture2D = new Texture2D(1, 64, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.HideAndDontSave
			};
			for (int i = 0; i < 64; i++)
			{
				float num = (float)i / 63f;
				float num2 = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.72f, 1f, num));
				texture2D.SetPixel(0, i, new Color(1f, 1f, 1f, num2));
			}
			texture2D.Apply(false, true);
			return texture2D;
		}

		// Token: 0x06005E67 RID: 24167 RVA: 0x00255870 File Offset: 0x00253A70
		private void ResolveShapes()
		{
			Mesh mesh = ((this._face != null) ? this._face.sharedMesh : null);
			if (mesh == this._mesh)
			{
				return;
			}
			this._mesh = mesh;
			this._left = ((mesh != null) ? mesh.GetBlendShapeIndex("eye_close_sleep_L") : (-1));
			this._right = ((mesh != null) ? mesh.GetBlendShapeIndex("eye_close_sleep_R") : (-1));
		}

		// Token: 0x06005E68 RID: 24168 RVA: 0x002558EC File Offset: 0x00253AEC
		private void LateUpdate()
		{
			this.ResolveShapes();
			float num = 0f;
			if (this._face != null)
			{
				float num2 = ((this._left >= 0) ? this._face.GetBlendShapeWeight(this._left) : 0f);
				float num3 = ((this._right >= 0) ? this._face.GetBlendShapeWeight(this._right) : 0f);
				num = Mathf.Clamp01(Mathf.Max(num2, num3) / 100f);
			}
			this._closed = Mathf.MoveTowards(this._closed, num, Time.unscaledDeltaTime * 9f);
			bool flag = this.Visible && this._closed > 0.003f;
			if (this._canvas.enabled != flag)
			{
				this._canvas.enabled = flag;
			}
			if (!flag)
			{
				return;
			}
			float num4 = Mathf.SmoothStep(0f, 1f, this._closed) * 0.62f;
			Vector2 size = ((RectTransform)this._canvas.transform).rect.size;
			this._top.sizeDelta = new Vector2(0f, size.y * num4);
			this._bottom.sizeDelta = new Vector2(0f, size.y * num4);
		}

		// Token: 0x06005E69 RID: 24169 RVA: 0x00255A33 File Offset: 0x00253C33
		private void OnDisable()
		{
			if (this._canvas != null)
			{
				this._canvas.enabled = false;
			}
		}

		// Token: 0x06005E6A RID: 24170 RVA: 0x00255A4F File Offset: 0x00253C4F
		private void OnDestroy()
		{
			if (this._gradient != null)
			{
				Object.Destroy(this._gradient);
			}
		}

		// Token: 0x04005F6C RID: 24428
		private const string LeftLid = "eye_close_sleep_L";

		// Token: 0x04005F6D RID: 24429
		private const string RightLid = "eye_close_sleep_R";

		// Token: 0x04005F6E RID: 24430
		private const int SortingOrder = 900;

		// Token: 0x04005F6F RID: 24431
		private SkinnedMeshRenderer _face;

		// Token: 0x04005F70 RID: 24432
		private int _left = -1;

		// Token: 0x04005F71 RID: 24433
		private int _right = -1;

		// Token: 0x04005F72 RID: 24434
		private Mesh _mesh;

		// Token: 0x04005F73 RID: 24435
		private Canvas _canvas;

		// Token: 0x04005F74 RID: 24436
		private RectTransform _top;

		// Token: 0x04005F75 RID: 24437
		private RectTransform _bottom;

		// Token: 0x04005F76 RID: 24438
		private Texture2D _gradient;

		// Token: 0x04005F77 RID: 24439
		private float _closed;
	}
}
