using System;
using System.Collections.Generic;
using Features.CielOS.WindowManager;
using UnityEngine;

namespace Features.CielOS.Desktop
{
	// Token: 0x020013E6 RID: 5094
	public class CielOSDesktop : MonoBehaviour
	{
		// Token: 0x06009D30 RID: 40240 RVA: 0x00409199 File Offset: 0x00407399
		private void Start()
		{
			this.BuildIcons();
		}

		// Token: 0x06009D31 RID: 40241 RVA: 0x004091A4 File Offset: 0x004073A4
		public void RefreshAvailability()
		{
			foreach (CielOSDesktopIcon cielOSDesktopIcon in this._icons)
			{
				if (!(cielOSDesktopIcon == null))
				{
					cielOSDesktopIcon.gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(cielOSDesktopIcon.gameObject);
					}
					else
					{
						Object.DestroyImmediate(cielOSDesktopIcon.gameObject);
					}
				}
			}
			this._icons.Clear();
			this._selected = null;
			this._built = false;
			this.BuildIcons();
		}

		// Token: 0x06009D32 RID: 40242 RVA: 0x00409244 File Offset: 0x00407444
		private void BuildIcons()
		{
			if (this._built)
			{
				return;
			}
			if (this.catalog == null || this.iconLayer == null || this.iconPrefab == null)
			{
				Debug.LogWarning("[CielOSDesktop] Missing catalog, iconLayer, or iconPrefab.", this);
				return;
			}
			this._built = true;
			int num = 0;
			foreach (CielOSAppDefinition cielOSAppDefinition in this.catalog.GetDesktopApps())
			{
				CielOSDesktopIcon cielOSDesktopIcon = Object.Instantiate<CielOSDesktopIcon>(this.iconPrefab, this.iconLayer);
				cielOSDesktopIcon.Bind(this, cielOSAppDefinition.appId, cielOSAppDefinition.displayName, cielOSAppDefinition.icon, cielOSAppDefinition.isCustomApp, !cielOSAppDefinition.IsPlatformSupported);
				Vector2 vector2;
				Vector2 vector = (CielOSDesktop.TryLoadPosition(cielOSAppDefinition.appId, out vector2) ? vector2 : this.DefaultGridPosition(num));
				this._icons.Add(cielOSDesktopIcon);
				cielOSDesktopIcon.Rect.anchoredPosition = this.SnapToFreeCell(cielOSDesktopIcon, vector);
				num++;
			}
		}

		// Token: 0x06009D33 RID: 40243 RVA: 0x00409358 File Offset: 0x00407558
		private Vector2 DefaultGridPosition(int index)
		{
			int num = index / this.iconsPerColumn;
			int num2 = index % this.iconsPerColumn;
			return this.CellToPos(num, num2);
		}

		// Token: 0x06009D34 RID: 40244 RVA: 0x00409380 File Offset: 0x00407580
		public void OnIconClicked(CielOSDesktopIcon icon)
		{
			if (this._selected != null && this._selected != icon)
			{
				this._selected.SetSelected(false);
			}
			this._selected = icon;
			if (this._selected != null)
			{
				this._selected.SetSelected(true);
			}
		}

		// Token: 0x06009D35 RID: 40245 RVA: 0x004093D6 File Offset: 0x004075D6
		public void LaunchIcon(CielOSDesktopIcon icon)
		{
			if (icon == null || this.windowManager == null)
			{
				return;
			}
			this.windowManager.LaunchApp(icon.AppId);
		}

		// Token: 0x06009D36 RID: 40246 RVA: 0x00409404 File Offset: 0x00407604
		public void OnIconMoved(CielOSDesktopIcon icon, Vector2 anchoredPos)
		{
			if (icon == null)
			{
				return;
			}
			Vector2 vector = this.SnapToFreeCell(icon, anchoredPos);
			icon.Rect.anchoredPosition = vector;
			CielOSDesktop.SavePosition(icon.AppId, vector);
		}

		// Token: 0x06009D37 RID: 40247 RVA: 0x0040943C File Offset: 0x0040763C
		private Vector2 CellToPos(int col, int row)
		{
			return new Vector2(this.gridOrigin.x + (float)col * this.gridSpacing.x, this.gridOrigin.y - (float)row * this.gridSpacing.y);
		}

		// Token: 0x06009D38 RID: 40248 RVA: 0x00409478 File Offset: 0x00407678
		private void PosToCell(Vector2 pos, out int col, out int row)
		{
			col = Mathf.Max(0, Mathf.RoundToInt((pos.x - this.gridOrigin.x) / this.gridSpacing.x));
			row = Mathf.Max(0, Mathf.RoundToInt((this.gridOrigin.y - pos.y) / this.gridSpacing.y));
		}

		// Token: 0x06009D39 RID: 40249 RVA: 0x004094DC File Offset: 0x004076DC
		private Vector2 SnapToFreeCell(CielOSDesktopIcon moving, Vector2 pos)
		{
			int num;
			int num2;
			this.PosToCell(pos, out num, out num2);
			if (this.IsCellFree(moving, num, num2))
			{
				return this.CellToPos(num, num2);
			}
			for (int i = 1; i < 32; i++)
			{
				for (int j = -i; j <= i; j++)
				{
					for (int k = -i; k <= i; k++)
					{
						if (Mathf.Abs(j) == i || Mathf.Abs(k) == i)
						{
							int num3 = num + j;
							int num4 = num2 + k;
							if (num3 >= 0 && num4 >= 0 && this.IsCellFree(moving, num3, num4))
							{
								return this.CellToPos(num3, num4);
							}
						}
					}
				}
			}
			return this.CellToPos(num, num2);
		}

		// Token: 0x06009D3A RID: 40250 RVA: 0x0040957C File Offset: 0x0040777C
		private bool IsCellFree(CielOSDesktopIcon moving, int col, int row)
		{
			Vector2 vector = this.CellToPos(col, row);
			foreach (CielOSDesktopIcon cielOSDesktopIcon in this._icons)
			{
				if (!(cielOSDesktopIcon == null) && !(cielOSDesktopIcon == moving) && (cielOSDesktopIcon.Rect.anchoredPosition - vector).sqrMagnitude < 1f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009D3B RID: 40251 RVA: 0x0040960C File Offset: 0x0040780C
		private static void SavePosition(string appId, Vector2 pos)
		{
			PlayerPrefs.SetFloat("CielOS.Desktop.Icon." + appId + ".x", pos.x);
			PlayerPrefs.SetFloat("CielOS.Desktop.Icon." + appId + ".y", pos.y);
		}

		// Token: 0x06009D3C RID: 40252 RVA: 0x00409644 File Offset: 0x00407844
		private static bool TryLoadPosition(string appId, out Vector2 pos)
		{
			string text = "CielOS.Desktop.Icon." + appId + ".x";
			string text2 = "CielOS.Desktop.Icon." + appId + ".y";
			if (PlayerPrefs.HasKey(text) && PlayerPrefs.HasKey(text2))
			{
				pos = new Vector2(PlayerPrefs.GetFloat(text), PlayerPrefs.GetFloat(text2));
				return true;
			}
			pos = Vector2.zero;
			return false;
		}

		// Token: 0x0400A1B1 RID: 41393
		[Header("References")]
		[SerializeField]
		private CielOSAppCatalogSO catalog;

		// Token: 0x0400A1B2 RID: 41394
		[SerializeField]
		private CielOSWindowManager windowManager;

		// Token: 0x0400A1B3 RID: 41395
		[SerializeField]
		private RectTransform iconLayer;

		// Token: 0x0400A1B4 RID: 41396
		[SerializeField]
		private CielOSDesktopIcon iconPrefab;

		// Token: 0x0400A1B5 RID: 41397
		[Header("Default Grid")]
		[Tooltip("Top-left anchored position of the first icon.")]
		[SerializeField]
		private Vector2 gridOrigin = new Vector2(40f, -60f);

		// Token: 0x0400A1B6 RID: 41398
		[Tooltip("Spacing between icon cells (x right, y down as positive).")]
		[SerializeField]
		private Vector2 gridSpacing = new Vector2(110f, 120f);

		// Token: 0x0400A1B7 RID: 41399
		[Tooltip("Icons per column before wrapping to the next column.")]
		[SerializeField]
		private int iconsPerColumn = 4;

		// Token: 0x0400A1B8 RID: 41400
		private const string PrefKeyPrefix = "CielOS.Desktop.Icon.";

		// Token: 0x0400A1B9 RID: 41401
		private readonly List<CielOSDesktopIcon> _icons = new List<CielOSDesktopIcon>();

		// Token: 0x0400A1BA RID: 41402
		private CielOSDesktopIcon _selected;

		// Token: 0x0400A1BB RID: 41403
		private bool _built;
	}
}
