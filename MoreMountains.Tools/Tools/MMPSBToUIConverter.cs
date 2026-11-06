using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000052 RID: 82
	[AddComponentMenu("More Mountains/Tools/GUI/MMPSBToUIConverter")]
	public class MMPSBToUIConverter : MonoBehaviour
	{
		// Token: 0x060001CE RID: 462 RVA: 0x00008EB4 File Offset: 0x000070B4
		public virtual void ConvertToCanvas()
		{
			Screen.SetResolution((int)this.TargetWidth, (int)this.TargetHeight, true);
			this._sortingOrders = new Dictionary<Transform, int>();
			foreach (object obj in this.TargetCanvas.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name == base.name)
				{
					transform.MMDestroyAllChildren();
					Object.DestroyImmediate(transform.gameObject);
				}
			}
			CanvasScaler component = this.TargetCanvas.GetComponent<CanvasScaler>();
			if (component != null)
			{
				component.referenceResolution = new Vector2(this.TargetWidth, this.TargetHeight);
			}
			GameObject gameObject = new GameObject(base.name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(this.TargetCanvas.transform);
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			this.SetupForStretch(component2);
			this._topLevel = gameObject.transform;
			this.CreateImageForChildren(base.transform, gameObject.transform);
			foreach (KeyValuePair<Transform, int> keyValuePair in this._sortingOrders)
			{
				keyValuePair.Key.SetSiblingIndex(keyValuePair.Value);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009034 File Offset: 0x00007234
		protected virtual void CreateImageForChildren(Transform root, Transform parent)
		{
			foreach (object obj in root)
			{
				Transform transform = (Transform)obj;
				GameObject gameObject = new GameObject(transform.name, new Type[] { typeof(RectTransform) });
				gameObject.transform.localPosition = this.ScaleFactor * transform.transform.localPosition;
				if (this.ReplicateNesting)
				{
					gameObject.transform.SetParent(parent);
				}
				else
				{
					gameObject.transform.SetParent(this._topLevel);
					Vector3 localPosition = gameObject.transform.localPosition;
					localPosition.x += this.TargetWidth / 2f;
					gameObject.transform.localPosition = localPosition;
				}
				SpriteRenderer component = transform.gameObject.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					Image image = gameObject.AddComponent<Image>();
					image.sprite = component.sprite;
					this._sortingOrders.Add(image.transform, component.sortingOrder);
					image.SetNativeSize();
					RectTransform component2 = gameObject.GetComponent<RectTransform>();
					Vector3 vector = component2.localPosition;
					vector += this.ChildImageOffset;
					vector.z = 0f;
					component2.localPosition = vector;
				}
				else
				{
					GameObject gameObject2 = gameObject;
					gameObject2.name += " - NODE";
					RectTransform component3 = gameObject.GetComponent<RectTransform>();
					component3.sizeDelta = new Vector2(this.TargetWidth, this.TargetHeight);
					component3.localPosition = Vector3.zero;
				}
				gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
				this.CreateImageForChildren(transform, gameObject.transform);
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009208 File Offset: 0x00007408
		protected virtual void SetupForStretch(RectTransform rect)
		{
			rect.localPosition = Vector3.zero;
			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			rect.localScale = Vector3.one;
		}

		// Token: 0x04000276 RID: 630
		[Header("Target")]
		public Canvas TargetCanvas;

		// Token: 0x04000277 RID: 631
		public float ScaleFactor = 100f;

		// Token: 0x04000278 RID: 632
		public bool ReplicateNesting;

		// Token: 0x04000279 RID: 633
		[Header("Size")]
		public float TargetWidth = 2048f;

		// Token: 0x0400027A RID: 634
		public float TargetHeight = 1152f;

		// Token: 0x0400027B RID: 635
		[Header("Conversion")]
		[MMInspectorButton("ConvertToCanvas")]
		public bool ConvertToCanvasButton;

		// Token: 0x0400027C RID: 636
		public Vector3 ChildImageOffset = new Vector3(-1024f, -576f, 0f);

		// Token: 0x0400027D RID: 637
		protected Transform _topLevel;

		// Token: 0x0400027E RID: 638
		protected Dictionary<Transform, int> _sortingOrders;
	}
}
