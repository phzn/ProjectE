using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E43 RID: 3651
	[RequireComponent(typeof(GridLayoutGroup))]
	public class PhoneHomeGridReorder : MonoBehaviour
	{
		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06006D16 RID: 27926 RVA: 0x002BC2AC File Offset: 0x002BA4AC
		public float HoldToDrag
		{
			get
			{
				return this.holdToDrag;
			}
		}

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x06006D17 RID: 27927 RVA: 0x002BC2B4 File Offset: 0x002BA4B4
		public float DragScale
		{
			get
			{
				return this.dragScale;
			}
		}

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x06006D18 RID: 27928 RVA: 0x002BC2BC File Offset: 0x002BA4BC
		public float FollowLerp
		{
			get
			{
				return this.followLerp;
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06006D19 RID: 27929 RVA: 0x002BC2C4 File Offset: 0x002BA4C4
		public float ReflowLerp
		{
			get
			{
				return this.reflowLerp;
			}
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06006D1A RID: 27930 RVA: 0x002BC2CC File Offset: 0x002BA4CC
		public GridLayoutGroup Grid
		{
			get
			{
				if (!(this._grid != null))
				{
					return this._grid = base.GetComponent<GridLayoutGroup>();
				}
				return this._grid;
			}
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x002BC2FD File Offset: 0x002BA4FD
		private void Awake()
		{
			this._grid = base.GetComponent<GridLayoutGroup>();
			this._rect = (RectTransform)base.transform;
			this.RefreshChildren();
			if (this.persistOrder)
			{
				this.ApplySavedOrder();
			}
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x002BC330 File Offset: 0x002BA530
		public void EnterEditMode(RectTransform draggedIcon)
		{
			this._editingCount++;
			if (draggedIcon != null)
			{
				this._draggedIcon = draggedIcon;
			}
			if (!this._editing)
			{
				this._editing = true;
				this.Grid.enabled = false;
			}
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x002BC36B File Offset: 0x002BA56B
		public void SetDraggedIcon(RectTransform icon)
		{
			this._draggedIcon = icon;
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x002BC374 File Offset: 0x002BA574
		public void ExitEditMode(RectTransform icon)
		{
			if (icon == this._draggedIcon)
			{
				this._draggedIcon = null;
			}
			this._editingCount = Mathf.Max(0, this._editingCount - 1);
			if (this._editingCount == 0 && this._editing)
			{
				this._editing = false;
				this.Grid.enabled = true;
				this.ResetWiggle();
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06006D1F RID: 27935 RVA: 0x002BC3D3 File Offset: 0x002BA5D3
		public bool IsEditing
		{
			get
			{
				return this._editing;
			}
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x002BC3DC File Offset: 0x002BA5DC
		private void LateUpdate()
		{
			if (!this._editing)
			{
				return;
			}
			float num = ((this.reflowLerp <= 0f) ? 1f : (1f - Mathf.Exp(-this.reflowLerp * Time.unscaledDeltaTime)));
			float unscaledTime = Time.unscaledTime;
			int childCount = base.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != this._draggedIcon)
				{
					Vector2 slotPosition = this.GetSlotPosition(i);
					rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, slotPosition, num);
				}
				if (this.wiggle && rectTransform != this._draggedIcon)
				{
					float num2 = (float)(rectTransform.GetInstanceID() % 628) * 0.01f;
					float num3 = Mathf.Sin(unscaledTime * this.wiggleSpeed + num2) * this.wiggleAngle;
					rectTransform.localRotation = Quaternion.Euler(0f, 0f, num3);
				}
				else if (rectTransform == this._draggedIcon)
				{
					rectTransform.localRotation = Quaternion.identity;
				}
			}
		}

		// Token: 0x06006D21 RID: 27937 RVA: 0x002BC500 File Offset: 0x002BA700
		private void ResetWiggle()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				((RectTransform)base.transform.GetChild(i)).localRotation = Quaternion.identity;
			}
		}

		// Token: 0x06006D22 RID: 27938 RVA: 0x002BC540 File Offset: 0x002BA740
		public Vector2 GetSlotPosition(int index)
		{
			GridLayoutGroup grid = this.Grid;
			int num = Mathf.Max(1, grid.constraintCount);
			float childCount = (float)base.transform.childCount;
			int num2 = index % num;
			int num3 = index / num;
			int num4 = Mathf.CeilToInt(childCount / (float)num);
			float x = grid.cellSize.x;
			float y = grid.cellSize.y;
			float x2 = grid.spacing.x;
			float y2 = grid.spacing.y;
			float width = this._rect.rect.width;
			float num5 = (float)num * x + (float)(num - 1) * x2;
			float num6 = (float)grid.padding.left + (width - (float)grid.padding.left - (float)grid.padding.right - num5) * 0.5f;
			float num7 = (float)grid.padding.top;
			float num8 = num6 + (float)num2 * (x + x2) + x * 0.5f;
			float num9 = -(num7 + (float)num3 * (y + y2) + y * 0.5f);
			return new Vector2(num8, num9);
		}

		// Token: 0x06006D23 RID: 27939 RVA: 0x002BC648 File Offset: 0x002BA848
		public void ApplySavedOrder()
		{
			string @string = PlayerPrefs.GetString(this.saveKey, "");
			if (string.IsNullOrEmpty(@string))
			{
				return;
			}
			string[] array = @string.Split('|', StringSplitOptions.None);
			int num = 0;
			foreach (string text in array)
			{
				Transform transform = base.transform.Find(text);
				if (transform != null)
				{
					transform.SetSiblingIndex(num);
					num++;
				}
			}
		}

		// Token: 0x06006D24 RID: 27940 RVA: 0x002BC6B4 File Offset: 0x002BA8B4
		public void SaveOrder()
		{
			if (!this.persistOrder)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append('|');
				}
				stringBuilder.Append(base.transform.GetChild(i).name);
			}
			PlayerPrefs.SetString(this.saveKey, stringBuilder.ToString());
			PlayerPrefs.Save();
		}

		// Token: 0x06006D25 RID: 27941 RVA: 0x002BC724 File Offset: 0x002BA924
		public void RefreshChildren()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (child.GetComponent<PhoneHomeIconDrag>() == null)
				{
					child.gameObject.AddComponent<PhoneHomeIconDrag>().Init(this);
				}
			}
		}

		// Token: 0x06006D26 RID: 27942 RVA: 0x002BC774 File Offset: 0x002BA974
		public List<RectTransform> GetSlots(RectTransform exclude)
		{
			List<RectTransform> list = new List<RectTransform>(base.transform.childCount);
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != exclude)
				{
					list.Add(rectTransform);
				}
			}
			return list;
		}

		// Token: 0x04006FBE RID: 28606
		[Header("Drag")]
		[Tooltip("Seconds the icon must be held down before it lifts and can be dragged (iOS-style long press).")]
		[SerializeField]
		private float holdToDrag = 1.5f;

		// Token: 0x04006FBF RID: 28607
		[Tooltip("Scale applied to the icon being dragged, for the lifted/iOS feel.")]
		[SerializeField]
		private float dragScale = 1.12f;

		// Token: 0x04006FC0 RID: 28608
		[Tooltip("How quickly the dragged icon follows the pointer (higher = snappier). 0 = instant.")]
		[SerializeField]
		private float followLerp = 25f;

		// Token: 0x04006FC1 RID: 28609
		[Tooltip("How quickly the other icons slide into their new slots while reordering.")]
		[SerializeField]
		private float reflowLerp = 18f;

		// Token: 0x04006FC2 RID: 28610
		[Header("Wiggle (edit mode)")]
		[Tooltip("Enable the iOS jiggle on all icons while in edit mode.")]
		[SerializeField]
		private bool wiggle = true;

		// Token: 0x04006FC3 RID: 28611
		[Tooltip("Max rotation of the wiggle, in degrees.")]
		[SerializeField]
		private float wiggleAngle = 4f;

		// Token: 0x04006FC4 RID: 28612
		[Tooltip("Wiggle speed (oscillations per second-ish).")]
		[SerializeField]
		private float wiggleSpeed = 18f;

		// Token: 0x04006FC5 RID: 28613
		[Header("Persistence")]
		[Tooltip("Persist the icon order across sessions (PlayerPrefs). Uses each icon's GameObject name as a stable id.")]
		[SerializeField]
		private bool persistOrder = true;

		// Token: 0x04006FC6 RID: 28614
		[Tooltip("PlayerPrefs key used to store the icon order. Change it if you have more than one phone grid.")]
		[SerializeField]
		private string saveKey = "PhoneHome.IconOrder";

		// Token: 0x04006FC7 RID: 28615
		private GridLayoutGroup _grid;

		// Token: 0x04006FC8 RID: 28616
		private RectTransform _rect;

		// Token: 0x04006FC9 RID: 28617
		private RectTransform _draggedIcon;

		// Token: 0x04006FCA RID: 28618
		private int _editingCount;

		// Token: 0x04006FCB RID: 28619
		private bool _editing;
	}
}
