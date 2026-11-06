using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CompassNavigatorPro
{
	// Token: 0x02000286 RID: 646
	[HelpURL("https://kronnect.com/guides-category/compass-navigator-pro-2/")]
	[ExecuteAlways]
	[DefaultExecutionOrder(100)]
	public class CompassPro : MonoBehaviour
	{
		// Token: 0x060010BB RID: 4283 RVA: 0x0006F9B8 File Offset: 0x0006DBB8
		public static GameObject Create()
		{
			GameObject gameObject = Resources.Load("CNPro/Prefabs/CompassNavigatorPro") as GameObject;
			if (gameObject != null)
			{
				gameObject = Object.Instantiate<GameObject>(gameObject);
				gameObject.name = "CompassNavigatorPro";
				return gameObject;
			}
			return null;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0006F9F3 File Offset: 0x0006DBF3
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x0006FA15 File Offset: 0x0006DC15
		public Camera cameraMain
		{
			get
			{
				if (this._cameraMain == null)
				{
					this._cameraMain = this.FindSuitableCamera();
				}
				return this._cameraMain;
			}
			set
			{
				if (this._cameraMain != value)
				{
					this._cameraMain = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0006FA32 File Offset: 0x0006DC32
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x0006FA3A File Offset: 0x0006DC3A
		public Transform follow
		{
			get
			{
				return this._follow;
			}
			set
			{
				if (this._follow != value)
				{
					this._follow = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0006FA57 File Offset: 0x0006DC57
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x0006FA5F File Offset: 0x0006DC5F
		public UpdateMode updateMode
		{
			get
			{
				return this._updateMode;
			}
			set
			{
				if (value != this._updateMode)
				{
					this._updateMode = value;
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0006FA71 File Offset: 0x0006DC71
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x0006FA79 File Offset: 0x0006DC79
		public int updateIntervalFrameCount
		{
			get
			{
				return this._updateIntervalFrameCount;
			}
			set
			{
				if (value != this._updateIntervalFrameCount)
				{
					this._updateIntervalFrameCount = value;
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0006FA8B File Offset: 0x0006DC8B
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x0006FA93 File Offset: 0x0006DC93
		public float updateIntervalTime
		{
			get
			{
				return this._updateIntervalTime;
			}
			set
			{
				if (value != this._updateIntervalTime)
				{
					this._updateIntervalTime = value;
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0006FAA5 File Offset: 0x0006DCA5
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x0006FAAD File Offset: 0x0006DCAD
		public int indicatorUpdateIntervalFrames
		{
			get
			{
				return this._indicatorUpdateIntervalFrames;
			}
			set
			{
				if (value != this._indicatorUpdateIntervalFrames)
				{
					this._indicatorUpdateIntervalFrames = Mathf.Max(1, value);
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0006FAC5 File Offset: 0x0006DCC5
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x0006FACD File Offset: 0x0006DCCD
		public float distanceTextUpdateThreshold
		{
			get
			{
				return this._distanceTextUpdateThreshold;
			}
			set
			{
				if (value != this._distanceTextUpdateThreshold)
				{
					this._distanceTextUpdateThreshold = Mathf.Max(0f, value);
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x0006FAE9 File Offset: 0x0006DCE9
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x0006FAF1 File Offset: 0x0006DCF1
		public bool enableAreaOfInterest
		{
			get
			{
				return this._enableAreaOfInterest;
			}
			set
			{
				if (value != this._enableAreaOfInterest)
				{
					this._enableAreaOfInterest = value;
				}
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x0006FB03 File Offset: 0x0006DD03
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x0006FB0B File Offset: 0x0006DD0B
		public float areaOfInterestMaxDistance
		{
			get
			{
				return this._areaOfInterestMaxDistance;
			}
			set
			{
				if (value != this._areaOfInterestMaxDistance)
				{
					this._areaOfInterestMaxDistance = value;
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x0006FB1D File Offset: 0x0006DD1D
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x0006FB25 File Offset: 0x0006DD25
		public float areaOfInterestUpdateDistance
		{
			get
			{
				return this._areaOfInterestUpdateDistance;
			}
			set
			{
				if (value != this._areaOfInterestUpdateDistance)
				{
					this._areaOfInterestUpdateDistance = value;
				}
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0006FB37 File Offset: 0x0006DD37
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x0006FB3F File Offset: 0x0006DD3F
		public bool hideIfNoCamera
		{
			get
			{
				return this._hideIfNoCamera;
			}
			set
			{
				if (value != this._hideIfNoCamera)
				{
					this._hideIfNoCamera = value;
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x0006FB51 File Offset: 0x0006DD51
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x0006FB59 File Offset: 0x0006DD59
		public bool showCompassBar
		{
			get
			{
				return this._showCompassBar;
			}
			set
			{
				if (value != this._showCompassBar)
				{
					this._showCompassBar = value;
					this.UpdateCompassBarAppearance();
					this.UpdateCompassBarAlpha();
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x0006FB7E File Offset: 0x0006DD7E
		// (set) Token: 0x060010D5 RID: 4309 RVA: 0x0006FB86 File Offset: 0x0006DD86
		public CompassStyle style
		{
			get
			{
				return this._style;
			}
			set
			{
				if (value != this._style)
				{
					this._style = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0006FB9E File Offset: 0x0006DD9E
		// (set) Token: 0x060010D7 RID: 4311 RVA: 0x0006FBA6 File Offset: 0x0006DDA6
		public CompassBarOrientation compassBarOrientation
		{
			get
			{
				return this._compassBarOrientation;
			}
			set
			{
				if (value != this._compassBarOrientation)
				{
					this._compassBarOrientation = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0006FBBF File Offset: 0x0006DDBF
		// (set) Token: 0x060010D9 RID: 4313 RVA: 0x0006FBC7 File Offset: 0x0006DDC7
		public Sprite compassBackSprite
		{
			get
			{
				return this._compassBackSprite;
			}
			set
			{
				if (value != this._compassBackSprite)
				{
					this._compassBackSprite = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x0006FBE4 File Offset: 0x0006DDE4
		// (set) Token: 0x060010DB RID: 4315 RVA: 0x0006FBEC File Offset: 0x0006DDEC
		public bool compassBackSpriteScrollable
		{
			get
			{
				return this._compassBackSpriteScrollable;
			}
			set
			{
				if (value != this._compassBackSpriteScrollable)
				{
					this._compassBackSpriteScrollable = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x0006FC04 File Offset: 0x0006DE04
		// (set) Token: 0x060010DD RID: 4317 RVA: 0x0006FC0C File Offset: 0x0006DE0C
		public float compassBackSpriteScrollOffset
		{
			get
			{
				return this._compassBackSpriteScrollOffset;
			}
			set
			{
				if (value != this._compassBackSpriteScrollOffset)
				{
					this._compassBackSpriteScrollOffset = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0006FC24 File Offset: 0x0006DE24
		// (set) Token: 0x060010DF RID: 4319 RVA: 0x0006FC2C File Offset: 0x0006DE2C
		public Color compassTintColor
		{
			get
			{
				return this._compassTintColor;
			}
			set
			{
				if (value != this._compassTintColor)
				{
					this._compassTintColor = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0006FC49 File Offset: 0x0006DE49
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x0006FC51 File Offset: 0x0006DE51
		public float northDegrees
		{
			get
			{
				return this._northDegrees;
			}
			set
			{
				if (value != this._northDegrees)
				{
					this._northDegrees = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0006FC6A File Offset: 0x0006DE6A
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0006FC72 File Offset: 0x0006DE72
		public float visibleMaxDistance
		{
			get
			{
				return this._visibleMaxDistance;
			}
			set
			{
				if (value != this._visibleMaxDistance)
				{
					this._visibleMaxDistance = value;
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0006FC84 File Offset: 0x0006DE84
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x0006FC8C File Offset: 0x0006DE8C
		public float visibleMinDistance
		{
			get
			{
				return this._visibleMinDistance;
			}
			set
			{
				if (value != this._visibleMinDistance)
				{
					this._visibleMinDistance = value;
				}
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x0006FC9E File Offset: 0x0006DE9E
		// (set) Token: 0x060010E7 RID: 4327 RVA: 0x0006FCA6 File Offset: 0x0006DEA6
		public float titleMinPOIDistance
		{
			get
			{
				return this._titleMinPOIDistance;
			}
			set
			{
				if (value != this._titleMinPOIDistance)
				{
					this._titleMinPOIDistance = value;
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x0006FCB8 File Offset: 0x0006DEB8
		// (set) Token: 0x060010E9 RID: 4329 RVA: 0x0006FCC0 File Offset: 0x0006DEC0
		public float nearDistance
		{
			get
			{
				return this._nearDistance;
			}
			set
			{
				if (value != this._nearDistance)
				{
					this._nearDistance = value;
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x0006FCD2 File Offset: 0x0006DED2
		// (set) Token: 0x060010EB RID: 4331 RVA: 0x0006FCDA File Offset: 0x0006DEDA
		public float visitedDistance
		{
			get
			{
				return this._visitedDistance;
			}
			set
			{
				if (value != this._visitedDistance)
				{
					this._visitedDistance = value;
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x0006FCEC File Offset: 0x0006DEEC
		// (set) Token: 0x060010ED RID: 4333 RVA: 0x0006FCF4 File Offset: 0x0006DEF4
		public bool showOnScreenIndicators
		{
			get
			{
				return this._showOnScreenIndicators;
			}
			set
			{
				if (value != this._showOnScreenIndicators)
				{
					this._showOnScreenIndicators = value;
					this.UpdateSettings();
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0006FD0C File Offset: 0x0006DF0C
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x0006FD14 File Offset: 0x0006DF14
		public GameObject compassIconPrefab
		{
			get
			{
				return this._compassIconPrefab;
			}
			set
			{
				if (value != this._compassIconPrefab)
				{
					this._compassIconPrefab = value;
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0006FD2B File Offset: 0x0006DF2B
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x0006FD33 File Offset: 0x0006DF33
		public GameObject onScreenIndicatorPrefab
		{
			get
			{
				return this._onScreenIndicatorPrefab;
			}
			set
			{
				if (value != this._onScreenIndicatorPrefab)
				{
					this._onScreenIndicatorPrefab = value;
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0006FD4A File Offset: 0x0006DF4A
		// (set) Token: 0x060010F3 RID: 4339 RVA: 0x0006FD52 File Offset: 0x0006DF52
		public float onScreenIndicatorAlpha
		{
			get
			{
				return this._onScreenIndicatorAlpha;
			}
			set
			{
				if (value != this._onScreenIndicatorAlpha)
				{
					this._onScreenIndicatorAlpha = value;
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0006FD64 File Offset: 0x0006DF64
		// (set) Token: 0x060010F5 RID: 4341 RVA: 0x0006FD6C File Offset: 0x0006DF6C
		public float onScreenIndicatorScale
		{
			get
			{
				return this._onScreenIndicatorScale;
			}
			set
			{
				if (value != this._onScreenIndicatorScale)
				{
					this._onScreenIndicatorScale = value;
				}
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0006FD7E File Offset: 0x0006DF7E
		// (set) Token: 0x060010F7 RID: 4343 RVA: 0x0006FD86 File Offset: 0x0006DF86
		public float onScreenIndicatorScaleSpeed
		{
			get
			{
				return this._onScreenIndicatorScaleSpeed;
			}
			set
			{
				if (value != this._onScreenIndicatorScaleSpeed)
				{
					this._onScreenIndicatorScaleSpeed = value;
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0006FD98 File Offset: 0x0006DF98
		// (set) Token: 0x060010F9 RID: 4345 RVA: 0x0006FDA0 File Offset: 0x0006DFA0
		public float onScreenIndicatorFarDistance
		{
			get
			{
				return this._onScreenIndicatorFarDistance;
			}
			set
			{
				if (value != this._onScreenIndicatorFarDistance)
				{
					this._onScreenIndicatorFarDistance = value;
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0006FDB2 File Offset: 0x0006DFB2
		// (set) Token: 0x060010FB RID: 4347 RVA: 0x0006FDBA File Offset: 0x0006DFBA
		public float onScreenIndicatorFarFadeDistance
		{
			get
			{
				return this._onScreenIndicatorFarFadeDistance;
			}
			set
			{
				if (value != this._onScreenIndicatorFarFadeDistance)
				{
					this._onScreenIndicatorFarFadeDistance = value;
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0006FDCC File Offset: 0x0006DFCC
		// (set) Token: 0x060010FD RID: 4349 RVA: 0x0006FDD4 File Offset: 0x0006DFD4
		public float onScreenIndicatorNearFadeDistance
		{
			get
			{
				return this._onScreenIndicatorNearFadeDistance;
			}
			set
			{
				if (value != this._onScreenIndicatorNearFadeDistance)
				{
					this._onScreenIndicatorNearFadeDistance = value;
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x0006FDE6 File Offset: 0x0006DFE6
		// (set) Token: 0x060010FF RID: 4351 RVA: 0x0006FDEE File Offset: 0x0006DFEE
		public float onScreenIndicatorNearFadeMin
		{
			get
			{
				return this._onScreenIndicatorNearFadeMin;
			}
			set
			{
				if (value != this._onScreenIndicatorNearFadeMin)
				{
					this._onScreenIndicatorNearFadeMin = value;
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x0006FE00 File Offset: 0x0006E000
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x0006FE08 File Offset: 0x0006E008
		public bool onScreenIndicatorShowDistance
		{
			get
			{
				return this._onScreenIndicatorShowDistance;
			}
			set
			{
				if (value != this._onScreenIndicatorShowDistance)
				{
					this._onScreenIndicatorShowDistance = value;
				}
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x0006FE1A File Offset: 0x0006E01A
		// (set) Token: 0x06001103 RID: 4355 RVA: 0x0006FE22 File Offset: 0x0006E022
		public string onScreenIndicatorShowDistanceFormat
		{
			get
			{
				return this._onScreenIndicatorShowDistanceFormat;
			}
			set
			{
				if (value != this._onScreenIndicatorShowDistanceFormat)
				{
					this._onScreenIndicatorShowDistanceFormat = value;
				}
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x0006FE39 File Offset: 0x0006E039
		// (set) Token: 0x06001105 RID: 4357 RVA: 0x0006FE41 File Offset: 0x0006E041
		public bool onScreenIndicatorShowTitle
		{
			get
			{
				return this._onScreenIndicatorShowTitle;
			}
			set
			{
				if (value != this._onScreenIndicatorShowTitle)
				{
					this._onScreenIndicatorShowTitle = value;
				}
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0006FE53 File Offset: 0x0006E053
		// (set) Token: 0x06001107 RID: 4359 RVA: 0x0006FE5B File Offset: 0x0006E05B
		public bool showOffScreenIndicators
		{
			get
			{
				return this._showOffScreenIndicators;
			}
			set
			{
				if (value != this._showOffScreenIndicators)
				{
					this._showOffScreenIndicators = value;
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x0006FE6D File Offset: 0x0006E06D
		// (set) Token: 0x06001109 RID: 4361 RVA: 0x0006FE75 File Offset: 0x0006E075
		public float offScreenIndicatorScale
		{
			get
			{
				return this._offScreenIndicatorScale;
			}
			set
			{
				if (value != this._offScreenIndicatorScale)
				{
					this._offScreenIndicatorScale = value;
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x0006FE87 File Offset: 0x0006E087
		// (set) Token: 0x0600110B RID: 4363 RVA: 0x0006FE8F File Offset: 0x0006E08F
		public float offScreenIndicatorMargin
		{
			get
			{
				return this._offScreenIndicatorMargin;
			}
			set
			{
				if (value != this._offScreenIndicatorMargin)
				{
					this._offScreenIndicatorMargin = value;
				}
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x0006FEA1 File Offset: 0x0006E0A1
		// (set) Token: 0x0600110D RID: 4365 RVA: 0x0006FEA9 File Offset: 0x0006E0A9
		public Rect offScreenIndicatorRect
		{
			get
			{
				return this._offScreenIndicatorRect;
			}
			set
			{
				if (value != this._offScreenIndicatorRect)
				{
					this._offScreenIndicatorRect = value;
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x0006FEC0 File Offset: 0x0006E0C0
		// (set) Token: 0x0600110F RID: 4367 RVA: 0x0006FEC8 File Offset: 0x0006E0C8
		public bool offScreenIndicatorAvoidOverlap
		{
			get
			{
				return this._offScreenIndicatorAvoidOverlap;
			}
			set
			{
				if (value != this._offScreenIndicatorAvoidOverlap)
				{
					this._offScreenIndicatorAvoidOverlap = value;
				}
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x0006FEDA File Offset: 0x0006E0DA
		// (set) Token: 0x06001111 RID: 4369 RVA: 0x0006FEE2 File Offset: 0x0006E0E2
		public float offScreenIndicatorOverlapDistance
		{
			get
			{
				return this._offScreenIndicatorOverlapDistance;
			}
			set
			{
				if (value != this._offScreenIndicatorOverlapDistance)
				{
					this._offScreenIndicatorOverlapDistance = value;
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x0006FEF4 File Offset: 0x0006E0F4
		// (set) Token: 0x06001113 RID: 4371 RVA: 0x0006FEFC File Offset: 0x0006E0FC
		public int offScreenIndicatorOverlapMaxPOIs
		{
			get
			{
				return this._offScreenIndicatorOverlapMaxPOIs;
			}
			set
			{
				if (value != this._offScreenIndicatorOverlapMaxPOIs)
				{
					this._offScreenIndicatorOverlapMaxPOIs = Mathf.Max(1, value);
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x0006FF14 File Offset: 0x0006E114
		// (set) Token: 0x06001115 RID: 4373 RVA: 0x0006FF1C File Offset: 0x0006E11C
		public float offScreenIndicatorAlpha
		{
			get
			{
				return this._offScreenIndicatorAlpha;
			}
			set
			{
				if (value != this._offScreenIndicatorAlpha)
				{
					this._offScreenIndicatorAlpha = value;
				}
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x0006FF2E File Offset: 0x0006E12E
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x0006FF36 File Offset: 0x0006E136
		public float alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (value != this._alpha)
				{
					this._alpha = value;
					this.UpdateCompassBarAlpha();
				}
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x0006FF4E File Offset: 0x0006E14E
		// (set) Token: 0x06001119 RID: 4377 RVA: 0x0006FF56 File Offset: 0x0006E156
		public bool autoHide
		{
			get
			{
				return this._autoHide;
			}
			set
			{
				if (value != this._autoHide)
				{
					this._autoHide = value;
				}
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x0006FF68 File Offset: 0x0006E168
		// (set) Token: 0x0600111B RID: 4379 RVA: 0x0006FF70 File Offset: 0x0006E170
		public float fadeDuration
		{
			get
			{
				return this._fadeDuration;
			}
			set
			{
				if (value != this._fadeDuration)
				{
					this._fadeDuration = value;
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x0006FF82 File Offset: 0x0006E182
		// (set) Token: 0x0600111D RID: 4381 RVA: 0x0006FF8A File Offset: 0x0006E18A
		public bool alwaysVisibleInEditMode
		{
			get
			{
				return this._alwaysVisibleInEditMode;
			}
			set
			{
				if (value != this._alwaysVisibleInEditMode)
				{
					this._alwaysVisibleInEditMode = value;
					this.UpdateCompassBarAlpha();
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x0006FFA2 File Offset: 0x0006E1A2
		// (set) Token: 0x0600111F RID: 4383 RVA: 0x0006FFAA File Offset: 0x0006E1AA
		public float verticalPosition
		{
			get
			{
				return this._verticalPosition;
			}
			set
			{
				if (value != this._verticalPosition)
				{
					this._verticalPosition = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x0006FFC2 File Offset: 0x0006E1C2
		// (set) Token: 0x06001121 RID: 4385 RVA: 0x0006FFCA File Offset: 0x0006E1CA
		public float horizontalPosition
		{
			get
			{
				return this._horizontalPosition;
			}
			set
			{
				if (value != this._horizontalPosition)
				{
					this._horizontalPosition = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x0006FFE2 File Offset: 0x0006E1E2
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x0006FFEA File Offset: 0x0006E1EA
		public float bendAmount
		{
			get
			{
				return this._bendAmount;
			}
			set
			{
				if (value != this._bendAmount)
				{
					this._bendAmount = value;
					if (this._bendAmount == 0f)
					{
						this._verticalPosition = 0.94f;
					}
					this.UpdateCompassBarAppearance();
					this.UpdateTitleAppearance();
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x00070020 File Offset: 0x0006E220
		// (set) Token: 0x06001125 RID: 4389 RVA: 0x00070028 File Offset: 0x0006E228
		public float width
		{
			get
			{
				return this._width;
			}
			set
			{
				if (value != this._width)
				{
					this._width = value;
					this.UpdateCompassBarAppearance();
					this.UpdateHalfWindsAppearance();
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00070046 File Offset: 0x0006E246
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x0007004E File Offset: 0x0006E24E
		public float height
		{
			get
			{
				return this._height;
			}
			set
			{
				if (value != this._height)
				{
					this._height = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00070066 File Offset: 0x0006E266
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x0007006E File Offset: 0x0006E26E
		public bool edgeFadeOut
		{
			get
			{
				return this._edgeFadeOut;
			}
			set
			{
				if (value != this._edgeFadeOut)
				{
					this._edgeFadeOut = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00070086 File Offset: 0x0006E286
		// (set) Token: 0x0600112B RID: 4395 RVA: 0x0007008E File Offset: 0x0006E28E
		public bool edgeFadeOutText
		{
			get
			{
				return this._edgeFadeOutText;
			}
			set
			{
				if (value != this._edgeFadeOutText)
				{
					this._edgeFadeOutText = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x000700A6 File Offset: 0x0006E2A6
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x000700AE File Offset: 0x0006E2AE
		public float edgeFadeOutWidth
		{
			get
			{
				return this._edgeFadeOutWidth;
			}
			set
			{
				if (value != this._edgeFadeOutWidth)
				{
					this._edgeFadeOutWidth = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x000700C6 File Offset: 0x0006E2C6
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x000700CE File Offset: 0x0006E2CE
		public float edgeFadeOutStart
		{
			get
			{
				return this._edgeFadeOutStart;
			}
			set
			{
				if (value != this._edgeFadeOutStart)
				{
					this._edgeFadeOutStart = value;
					this.UpdateCompassBarAppearance();
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x000700E6 File Offset: 0x0006E2E6
		// (set) Token: 0x06001131 RID: 4401 RVA: 0x000700EE File Offset: 0x0006E2EE
		public float endCapsWidth
		{
			get
			{
				return this._endCapsWidth;
			}
			set
			{
				if (value != this._endCapsWidth)
				{
					this._endCapsWidth = value;
					this.UpdateCompassBarAppearance();
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x0007010D File Offset: 0x0006E30D
		// (set) Token: 0x06001133 RID: 4403 RVA: 0x00070115 File Offset: 0x0006E315
		public bool showCardinalPoints
		{
			get
			{
				return this._showCardinalPoints;
			}
			set
			{
				if (value != this._showCardinalPoints)
				{
					this._showCardinalPoints = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0007012E File Offset: 0x0006E32E
		// (set) Token: 0x06001135 RID: 4405 RVA: 0x00070136 File Offset: 0x0006E336
		public bool showOrdinalPoints
		{
			get
			{
				return this._showOrdinalPoints;
			}
			set
			{
				if (value != this._showOrdinalPoints)
				{
					this._showOrdinalPoints = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x0007014F File Offset: 0x0006E34F
		// (set) Token: 0x06001137 RID: 4407 RVA: 0x00070157 File Offset: 0x0006E357
		public float cardinalScale
		{
			get
			{
				return this._cardinalScale;
			}
			set
			{
				if (value != this._cardinalScale)
				{
					this._cardinalScale = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00070170 File Offset: 0x0006E370
		// (set) Token: 0x06001139 RID: 4409 RVA: 0x00070178 File Offset: 0x0006E378
		public float ordinalScale
		{
			get
			{
				return this._ordinalScale;
			}
			set
			{
				if (value != this._ordinalScale)
				{
					this._ordinalScale = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x00070191 File Offset: 0x0006E391
		// (set) Token: 0x0600113B RID: 4411 RVA: 0x00070199 File Offset: 0x0006E399
		public float cardinalPointsVerticalOffset
		{
			get
			{
				return this._cardinalPointsVerticalOffset;
			}
			set
			{
				if (value != this._cardinalPointsVerticalOffset)
				{
					this._cardinalPointsVerticalOffset = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x000701B2 File Offset: 0x0006E3B2
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x000701BA File Offset: 0x0006E3BA
		public bool showHalfWinds
		{
			get
			{
				return this._showHalfWinds;
			}
			set
			{
				if (value != this._showHalfWinds)
				{
					this._showHalfWinds = value;
					this.UpdateHalfWindsAppearance();
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x000701D9 File Offset: 0x0006E3D9
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x000701E1 File Offset: 0x0006E3E1
		public float halfWindsHeight
		{
			get
			{
				return this._halfWindsHeight;
			}
			set
			{
				if (value != this._halfWindsHeight)
				{
					this._halfWindsHeight = value;
					this.UpdateHalfWindsAppearance();
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x000701F9 File Offset: 0x0006E3F9
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x00070201 File Offset: 0x0006E401
		public float halfWindsWidth
		{
			get
			{
				return this._halfWindsWidth;
			}
			set
			{
				if (value != this._halfWindsWidth)
				{
					this._halfWindsWidth = value;
					this.UpdateHalfWindsAppearance();
				}
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x00070219 File Offset: 0x0006E419
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x00070221 File Offset: 0x0006E421
		public float halfWindsInterval
		{
			get
			{
				return this._halfWindsInterval;
			}
			set
			{
				if (value != this._halfWindsInterval)
				{
					this._halfWindsInterval = value;
					this.UpdateHalfWindsAppearance();
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00070239 File Offset: 0x0006E439
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x00070241 File Offset: 0x0006E441
		public Color halfWindsTintColor
		{
			get
			{
				return this._halfWindsTintColor;
			}
			set
			{
				if (value != this._halfWindsTintColor)
				{
					this._halfWindsTintColor = value;
					this.UpdateHalfWindsAppearance();
				}
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0007025E File Offset: 0x0006E45E
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x00070266 File Offset: 0x0006E466
		public float labelHotZone
		{
			get
			{
				return this._labelHotZone;
			}
			set
			{
				if (value != this._labelHotZone)
				{
					this._labelHotZone = value;
				}
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x00070278 File Offset: 0x0006E478
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x00070280 File Offset: 0x0006E480
		public float maxIconSize
		{
			get
			{
				return this._maxIconSize;
			}
			set
			{
				if (value != this._maxIconSize)
				{
					this._maxIconSize = value;
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00070292 File Offset: 0x0006E492
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x0007029A File Offset: 0x0006E49A
		public float minIconSize
		{
			get
			{
				return this._minIconSize;
			}
			set
			{
				if (value != this._minIconSize)
				{
					this._minIconSize = value;
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x000702AC File Offset: 0x0006E4AC
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x000702B4 File Offset: 0x0006E4B4
		public float scaleInDuration
		{
			get
			{
				return this._scaleInDuration;
			}
			set
			{
				if (value != this._scaleInDuration)
				{
					this._scaleInDuration = value;
				}
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x000702C6 File Offset: 0x0006E4C6
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x000702CE File Offset: 0x0006E4CE
		public CompassProPOI focusedPOI
		{
			get
			{
				return this._focusedPOI;
			}
			set
			{
				if (value != this._focusedPOI)
				{
					this._focusedPOI = value;
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x000702EC File Offset: 0x0006E4EC
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x000702F4 File Offset: 0x0006E4F4
		public WorldMappingMode worldMappingMode
		{
			get
			{
				return this._worldMappingMode;
			}
			set
			{
				if (value != this._worldMappingMode)
				{
					this._worldMappingMode = value;
					this.UpdateHalfWindsAppearance();
					this.UpdateCompassBarAppearance();
					this.needUpdateCompassBarIcons = true;
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x00070319 File Offset: 0x0006E519
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x00070321 File Offset: 0x0006E521
		public float textVerticalPosition
		{
			get
			{
				return this._textVerticalPosition;
			}
			set
			{
				if (value != this._textVerticalPosition)
				{
					this._textVerticalPosition = value;
					this.UpdateTextAppearanceEditMode();
				}
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x00070339 File Offset: 0x0006E539
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x00070341 File Offset: 0x0006E541
		public float textScale
		{
			get
			{
				return this._textScale;
			}
			set
			{
				if (value != this._textScale)
				{
					this._textScale = value;
					this.UpdateTextAppearanceEditMode();
				}
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x00070359 File Offset: 0x0006E559
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x00070361 File Offset: 0x0006E561
		public float textLetterSpacing
		{
			get
			{
				return this._textLetterSpacing;
			}
			set
			{
				if (value != this._textLetterSpacing)
				{
					this._textLetterSpacing = value;
				}
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x00070373 File Offset: 0x0006E573
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x0007037B File Offset: 0x0006E57B
		public bool textRevealEnabled
		{
			get
			{
				return this._textRevealEnabled;
			}
			set
			{
				if (value != this._textRevealEnabled)
				{
					this._textRevealEnabled = value;
				}
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x0007038D File Offset: 0x0006E58D
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x00070395 File Offset: 0x0006E595
		public float textRevealDuration
		{
			get
			{
				return this._textRevealDuration;
			}
			set
			{
				if (value != this._textRevealDuration)
				{
					this._textRevealDuration = value;
				}
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x000703A7 File Offset: 0x0006E5A7
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x000703AF File Offset: 0x0006E5AF
		public float textRevealLetterDelay
		{
			get
			{
				return this._textRevealLetterDelay;
			}
			set
			{
				if (value != this._textRevealLetterDelay)
				{
					this._textRevealLetterDelay = value;
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x000703C1 File Offset: 0x0006E5C1
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x000703C9 File Offset: 0x0006E5C9
		public float textDuration
		{
			get
			{
				return this._textDuration;
			}
			set
			{
				if (value != this._textDuration)
				{
					this._textDuration = value;
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x000703DB File Offset: 0x0006E5DB
		// (set) Token: 0x06001161 RID: 4449 RVA: 0x000703E3 File Offset: 0x0006E5E3
		public float textFadeOutDuration
		{
			get
			{
				return this._textFadeOutDuration;
			}
			set
			{
				if (value != this._textFadeOutDuration)
				{
					this._textFadeOutDuration = value;
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x000703F5 File Offset: 0x0006E5F5
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x000703FD File Offset: 0x0006E5FD
		public bool textShadowEnabled
		{
			get
			{
				return this._textShadowEnabled;
			}
			set
			{
				if (value != this._textShadowEnabled)
				{
					this._textShadowEnabled = value;
					if (!Application.isPlaying)
					{
						this.UpdateTextAppearanceEditMode();
					}
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0007041C File Offset: 0x0006E61C
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x00070442 File Offset: 0x0006E642
		public Font textFont
		{
			get
			{
				if (this._textFont == null)
				{
					this._textFont = Resources.Load<Font>("CNPro/Fonts/Vollkorn-Regular");
				}
				return this._textFont;
			}
			set
			{
				if (value != this._textFont)
				{
					this._textFont = value;
					this.UpdateTextAppearanceEditMode();
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0007045F File Offset: 0x0006E65F
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x00070467 File Offset: 0x0006E667
		public float titleVerticalPosition
		{
			get
			{
				return this._titleVerticalPosition;
			}
			set
			{
				if (value != this._titleVerticalPosition)
				{
					this._titleVerticalPosition = value;
					if (Application.isPlaying)
					{
						this.UpdateTitleAppearance();
						return;
					}
					this.UpdateTitleAppearanceEditMode();
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x0007048D File Offset: 0x0006E68D
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x00070495 File Offset: 0x0006E695
		public float titleScale
		{
			get
			{
				return this._titleScale;
			}
			set
			{
				if (value != this._titleScale)
				{
					this._titleScale = value;
					if (Application.isPlaying)
					{
						this.UpdateTitleAppearance();
						return;
					}
					this.UpdateTitleAppearanceEditMode();
				}
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x000704BB File Offset: 0x0006E6BB
		// (set) Token: 0x0600116B RID: 4459 RVA: 0x000704C3 File Offset: 0x0006E6C3
		public Font titleFont
		{
			get
			{
				return this._titleFont;
			}
			set
			{
				if (value != this._titleFont)
				{
					this._titleFont = value;
					if (Application.isPlaying)
					{
						this.UpdateTitleAppearance();
						return;
					}
					this.UpdateTitleAppearanceEditMode();
				}
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x000704EE File Offset: 0x0006E6EE
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x000704F6 File Offset: 0x0006E6F6
		public bool titleShadowEnabled
		{
			get
			{
				return this._titleShadowEnabled;
			}
			set
			{
				if (value != this._titleShadowEnabled)
				{
					this._titleShadowEnabled = value;
					if (Application.isPlaying)
					{
						this.UpdateTitleAppearance();
						return;
					}
					this.UpdateTitleAppearanceEditMode();
				}
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x0007051C File Offset: 0x0006E71C
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x00070524 File Offset: 0x0006E724
		public TMP_FontAsset titleFontTMP
		{
			get
			{
				return this._titleFontTMP;
			}
			set
			{
				if (value != this._titleFontTMP)
				{
					this._titleFontTMP = value;
					if (Application.isPlaying)
					{
						this.UpdateTitleAppearance();
						return;
					}
					this.UpdateTitleAppearanceEditMode();
				}
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x0007054F File Offset: 0x0006E74F
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x00070557 File Offset: 0x0006E757
		public bool titleShowDistance
		{
			get
			{
				return this._titleShowDistance;
			}
			set
			{
				if (value != this._titleShowDistance)
				{
					this._titleShowDistance = value;
				}
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00070569 File Offset: 0x0006E769
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x00070571 File Offset: 0x0006E771
		public string titleShowDistanceFormat
		{
			get
			{
				return this._titleShowDistanceFormat;
			}
			set
			{
				if (value != this._titleShowDistanceFormat)
				{
					this._titleShowDistanceFormat = value;
				}
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00070588 File Offset: 0x0006E788
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00070590 File Offset: 0x0006E790
		public bool use3Ddistance
		{
			get
			{
				return this._use3Ddistance;
			}
			set
			{
				if (value != this._use3Ddistance)
				{
					this._use3Ddistance = value;
				}
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x000705A2 File Offset: 0x0006E7A2
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x000705AA File Offset: 0x0006E7AA
		public float sameAltitudeThreshold
		{
			get
			{
				return this._sameAltitudeThreshold;
			}
			set
			{
				if (value != this._sameAltitudeThreshold)
				{
					this._sameAltitudeThreshold = value;
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x000705BC File Offset: 0x0006E7BC
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x000705C4 File Offset: 0x0006E7C4
		public bool showDistance
		{
			get
			{
				return this._showDistance;
			}
			set
			{
				if (value != this._showDistance)
				{
					this._showDistance = value;
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x000705D6 File Offset: 0x0006E7D6
		// (set) Token: 0x0600117B RID: 4475 RVA: 0x000705DE File Offset: 0x0006E7DE
		public float showDistanceHeight
		{
			get
			{
				return this._showDistanceHeight;
			}
			set
			{
				if (value != this._showDistanceHeight)
				{
					this._showDistanceHeight = value;
				}
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x000705F0 File Offset: 0x0006E7F0
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x000705F8 File Offset: 0x0006E7F8
		public string showDistanceFormat
		{
			get
			{
				return this._showDistanceFormat;
			}
			set
			{
				if (value != this._showDistanceFormat)
				{
					this._showDistanceFormat = value;
				}
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x0007060F File Offset: 0x0006E80F
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x00070617 File Offset: 0x0006E817
		public AudioClip visitedDefaultAudioClip
		{
			get
			{
				return this._visitedDefaultAudioClip;
			}
			set
			{
				if (value != this._visitedDefaultAudioClip)
				{
					this._visitedDefaultAudioClip = value;
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x0007062E File Offset: 0x0006E82E
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x00070636 File Offset: 0x0006E836
		public AudioClip beaconDefaultAudioClip
		{
			get
			{
				return this._beaconDefaultAudioClip;
			}
			set
			{
				if (value != this._beaconDefaultAudioClip)
				{
					this._beaconDefaultAudioClip = value;
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x0007064D File Offset: 0x0006E84D
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x00070655 File Offset: 0x0006E855
		public AudioClip heartbeatDefaultAudioClip
		{
			get
			{
				return this._heartbeatDefaultAudioClip;
			}
			set
			{
				if (value != this._heartbeatDefaultAudioClip)
				{
					this._heartbeatDefaultAudioClip = value;
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x0007066C File Offset: 0x0006E86C
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x00070674 File Offset: 0x0006E874
		public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			set
			{
				if (value != this._dontDestroyOnLoad)
				{
					this._dontDestroyOnLoad = value;
				}
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00070686 File Offset: 0x0006E886
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x0007068E File Offset: 0x0006E88E
		public bool offScreenIndicatorShowDistance
		{
			get
			{
				return this._offScreenIndicatorShowDistance;
			}
			set
			{
				if (value != this._offScreenIndicatorShowDistance)
				{
					this._offScreenIndicatorShowDistance = value;
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x000706A0 File Offset: 0x0006E8A0
		// (set) Token: 0x06001189 RID: 4489 RVA: 0x000706A8 File Offset: 0x0006E8A8
		public string offScreenIndicatorShowDistanceFormat
		{
			get
			{
				return this._offScreenIndicatorShowDistanceFormat;
			}
			set
			{
				if (value != this._offScreenIndicatorShowDistanceFormat)
				{
					this._offScreenIndicatorShowDistanceFormat = value;
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x000706BF File Offset: 0x0006E8BF
		public static CompassPro instance
		{
			get
			{
				if (CompassPro._instance == null)
				{
					CompassPro._instance = Misc.FindObjectOfType<CompassPro>(false);
				}
				return CompassPro._instance;
			}
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000706DE File Offset: 0x0006E8DE
		public void Refresh()
		{
			this.RequestPOIsOfInterestUpdate();
			this.HideMiniMapIcons();
			this.needUpdateMiniMapIcons = true;
			this.needUpdateCompassBarIcons = true;
			this.needUpdateIndicators = true;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00070704 File Offset: 0x0006E904
		public bool POIRegister(CompassProPOI newPOI)
		{
			bool flag = false;
			foreach (CompassPro compassPro in CompassPro.compasses)
			{
				if (compassPro != null)
				{
					compassPro.Refresh();
					if (!compassPro.POIisRegistered(newPOI))
					{
						flag = compassPro.POIRegister_internal(newPOI);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00070774 File Offset: 0x0006E974
		private bool POIRegister_internal(CompassProPOI newPOI)
		{
			newPOI.compass = this;
			this.pois.Add(newPOI);
			this.RequestPOIsOfInterestUpdate();
			this.needsIconSorting = true;
			this.OnPOIRegister.Invoke(newPOI);
			return true;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x000707A4 File Offset: 0x0006E9A4
		public bool POIisRegistered(CompassProPOI poi)
		{
			int count = this.pois.Count;
			for (int i = 0; i < count; i++)
			{
				CompassProPOI compassProPOI = this.pois[i];
				if (!(compassProPOI == null) && compassProPOI.id == poi.id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000707F0 File Offset: 0x0006E9F0
		public void POIResort()
		{
			foreach (CompassPro compassPro in CompassPro.compasses)
			{
				if (compassPro != null)
				{
					compassPro.needsIconSorting = true;
				}
			}
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0007084C File Offset: 0x0006EA4C
		public void POIUnregister(CompassProPOI newPOI)
		{
			foreach (CompassPro compassPro in CompassPro.compasses)
			{
				if (compassPro != null)
				{
					compassPro.Refresh();
					this.POIUnregister_internal(newPOI);
				}
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x000708B0 File Offset: 0x0006EAB0
		private void POIUnregister_internal(CompassProPOI poi)
		{
			int count = this.pois.Count;
			for (int i = 0; i < count; i++)
			{
				CompassProPOI compassProPOI = this.pois[i];
				if (!(compassProPOI == null) && compassProPOI.id == poi.id)
				{
					this.OnPOIUnregister.Invoke(poi);
					compassProPOI.Release();
					this.pois[i] = null;
					return;
				}
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0007091C File Offset: 0x0006EB1C
		public void POIFocus(CompassProPOI poi)
		{
			poi.showOnScreenIndicator = true;
			foreach (CompassPro compassPro in CompassPro.compasses)
			{
				if (compassPro != null)
				{
					compassPro.focusedPOI = poi;
				}
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00070980 File Offset: 0x0006EB80
		public void POIBlur()
		{
			foreach (CompassPro compassPro in CompassPro.compasses)
			{
				if (compassPro != null && compassPro.focusedPOI != null)
				{
					compassPro.focusedPOI.showOnScreenIndicator = false;
					compassPro.focusedPOI = null;
				}
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x000709F8 File Offset: 0x0006EBF8
		public void POIStartCircleAnimation(CompassProPOI poi)
		{
			if (poi != null)
			{
				poi.StartCircleAnimation();
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00070A09 File Offset: 0x0006EC09
		public GameObject POIShowBeacon(CompassProPOI existingPOI, float duration, float horizontalScale = 1f, bool playSound = true)
		{
			return this.POIShowBeacon(existingPOI, duration, horizontalScale, 1f, Color.white, playSound);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00070A20 File Offset: 0x0006EC20
		public GameObject POIShowBeacon(CompassProPOI existingPOI, float duration, float horizontalScale, float intensity, Color tintColor, bool playSound = true)
		{
			Transform transform = existingPOI.transform.Find("POIBeacon");
			if (transform != null)
			{
				return transform.gameObject;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("CNPro/Prefabs/POIBeacon"));
			gameObject.name = "POIBeacon";
			transform = gameObject.transform;
			transform.localScale = new Vector3(transform.localScale.x * horizontalScale, transform.localScale.y, transform.localScale.z * horizontalScale);
			transform.position = existingPOI.transform.position + new Vector3(0f, transform.localScale.y * 0.5f, 0f);
			BeaconAnimator component = transform.GetComponent<BeaconAnimator>();
			component.duration = duration;
			component.tintColor = tintColor;
			component.intensity = intensity;
			component.target = existingPOI.transform;
			if (playSound && this.audioSource != null)
			{
				if (existingPOI.beaconAudioClip != null)
				{
					this.audioSource.PlayOneShot(existingPOI.beaconAudioClip);
					return gameObject;
				}
				if (this._beaconDefaultAudioClip != null)
				{
					this.audioSource.PlayOneShot(this._beaconDefaultAudioClip);
				}
			}
			return gameObject;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00070B4E File Offset: 0x0006ED4E
		public GameObject POIShowBeacon(Vector3 position, float duration, float horizontalScale = 1f, bool playSound = true)
		{
			return this.POIShowBeacon(position, duration, horizontalScale, 1f, Color.white, playSound);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00070B68 File Offset: 0x0006ED68
		public GameObject POIShowBeacon(Vector3 position, float duration, float horizontalScale, float intensity, Color tintColor, bool playSound = true)
		{
			string text = "POIBeacon ";
			Vector3 vector = position;
			string text2 = text + vector.ToString();
			if (GameObject.Find(text2) != null)
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("CNPro/Prefabs/POIBeacon"));
			gameObject.name = text2;
			Transform transform = gameObject.transform;
			transform.localScale = new Vector3(transform.localScale.x * horizontalScale, transform.localScale.y, transform.localScale.z);
			transform.position = position + Misc.Vector3up * transform.transform.localScale.y * 0.5f;
			BeaconAnimator component = transform.gameObject.GetComponent<BeaconAnimator>();
			component.duration = duration;
			component.tintColor = tintColor;
			component.intensity = intensity;
			if (playSound && this.audioSource != null && this._beaconDefaultAudioClip != null)
			{
				this.audioSource.PlayOneShot(this._beaconDefaultAudioClip);
			}
			return gameObject;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00070C6C File Offset: 0x0006EE6C
		public void POIShowBeacon(float duration, float horizontalScale = 1f, bool playSound = true)
		{
			this.POIShowBeacon(duration, horizontalScale, 1f, Color.white, playSound);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00070C84 File Offset: 0x0006EE84
		public void POIShowBeacon(float duration, float horizontalScale, float intensity, Color tintColor, bool playSound = true)
		{
			int count = this.pois.Count;
			for (int i = 0; i < count; i++)
			{
				CompassProPOI compassProPOI = this.pois[i];
				if (!(compassProPOI == null) && !compassProPOI.isVisited && (compassProPOI.isVisible || !this.showCompassBar))
				{
					this.POIShowBeacon(compassProPOI, duration, horizontalScale, intensity, tintColor, playSound);
				}
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00070CE6 File Offset: 0x0006EEE6
		public void FadeIn(float duration)
		{
			this.fadeDuration = duration;
			this.fadeStartTime = Time.time;
			this.prevAlpha = this.canvasGroup.alpha;
			this.alpha = 1f;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00070D16 File Offset: 0x0006EF16
		public void FadeOut(float duration)
		{
			this.fadeDuration = duration;
			this.fadeStartTime = Time.time;
			this.prevAlpha = this.canvasGroup.alpha;
			this.alpha = 0f;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00070D46 File Offset: 0x0006EF46
		public void ShowAnimatedText(string text)
		{
			base.StartCoroutine(this.AnimateDiscoverText(text));
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x00070D56 File Offset: 0x0006EF56
		public Canvas canvas
		{
			get
			{
				return this._canvas;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00070D60 File Offset: 0x0006EF60
		public float degrees
		{
			get
			{
				float num;
				if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
				{
					num = this._follow.eulerAngles.y;
				}
				else
				{
					num = this._cameraMain.transform.eulerAngles.y;
				}
				return (num + 360f - this._northDegrees) % 360f;
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00070DC4 File Offset: 0x0006EFC4
		public void POIGetVisited(List<CompassProPOI> pois)
		{
			pois.Clear();
			foreach (CompassProPOI compassProPOI in this.pois)
			{
				if (compassProPOI != null && compassProPOI.isVisited)
				{
					pois.Add(compassProPOI);
				}
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00070E30 File Offset: 0x0006F030
		public void POIGetUnvisited(List<CompassProPOI> pois)
		{
			pois.Clear();
			foreach (CompassProPOI compassProPOI in this.pois)
			{
				if (compassProPOI != null && !compassProPOI.isVisited)
				{
					pois.Add(compassProPOI);
				}
			}
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00070E9C File Offset: 0x0006F09C
		public void POIGetAll(List<CompassProPOI> pois)
		{
			pois.Clear();
			foreach (CompassProPOI compassProPOI in this.pois)
			{
				if (compassProPOI != null)
				{
					pois.Add(compassProPOI);
				}
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00070F00 File Offset: 0x0006F100
		public void POISetVisited(CompassProPOI poi, bool visited)
		{
			if (poi == null)
			{
				return;
			}
			if (visited && !poi.isVisited)
			{
				poi.isVisited = true;
				if (poi.hideWhenVisited)
				{
					poi.enabled = false;
				}
				this.OnPOIVisited.Invoke(poi);
				if (poi.playAudioClipWhenVisited && this.audioSource != null)
				{
					if (poi.visitedAudioClipOverride != null)
					{
						this.audioSource.PlayOneShot(poi.visitedAudioClipOverride);
					}
					else if (this._visitedDefaultAudioClip != null)
					{
						this.audioSource.PlayOneShot(this._visitedDefaultAudioClip);
					}
				}
				this.ShowPOIDiscoveredText(poi);
				return;
			}
			if (!visited && poi.isVisited)
			{
				poi.isVisited = false;
				if (poi.hideWhenVisited)
				{
					poi.enabled = true;
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x00070FC9 File Offset: 0x0006F1C9
		// (set) Token: 0x060011A5 RID: 4517 RVA: 0x00070FD1 File Offset: 0x0006F1D1
		public bool fogOfWarEnabled
		{
			get
			{
				return this._fogOfWarEnabled;
			}
			set
			{
				if (value != this._fogOfWarEnabled)
				{
					this._fogOfWarEnabled = value;
					this.SetupMiniMap(false);
					this.UpdateFogOfWar();
				}
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00070FF0 File Offset: 0x0006F1F0
		// (set) Token: 0x060011A7 RID: 4519 RVA: 0x00070FF8 File Offset: 0x0006F1F8
		public Vector3 fogOfWarCenter
		{
			get
			{
				return this._fogOfWarCenter;
			}
			set
			{
				if (value != this._fogOfWarCenter)
				{
					this._fogOfWarCenter = value;
					this.UpdateFogOfWar();
				}
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x00071015 File Offset: 0x0006F215
		// (set) Token: 0x060011A9 RID: 4521 RVA: 0x0007101D File Offset: 0x0006F21D
		public Vector3 fogOfWarSize
		{
			get
			{
				return this._fogOfWarSize;
			}
			set
			{
				if (value != this._fogOfWarSize && value.x > 0f && value.z > 0f)
				{
					this._fogOfWarSize = value;
					this.UpdateFogOfWar();
				}
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x00071054 File Offset: 0x0006F254
		// (set) Token: 0x060011AB RID: 4523 RVA: 0x0007105C File Offset: 0x0006F25C
		public int fogOfWarTextureSize
		{
			get
			{
				return this._fogOfWarTextureSize;
			}
			set
			{
				if (value != this._fogOfWarTextureSize && value > 16)
				{
					this._fogOfWarTextureSize = value;
					this.UpdateFogOfWar();
				}
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00071079 File Offset: 0x0006F279
		// (set) Token: 0x060011AD RID: 4525 RVA: 0x00071081 File Offset: 0x0006F281
		public Color fogOfWarColor
		{
			get
			{
				return this._fogOfWarColor;
			}
			set
			{
				if (value != this._fogOfWarColor)
				{
					this._fogOfWarColor = value;
					this.UpdateFogOfWar();
				}
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x0007109E File Offset: 0x0006F29E
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x000710A6 File Offset: 0x0006F2A6
		public bool fogOfWarAutoClear
		{
			get
			{
				return this._fogOfWarAutoClear;
			}
			set
			{
				if (value != this._fogOfWarAutoClear)
				{
					this._fogOfWarAutoClear = value;
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x000710B8 File Offset: 0x0006F2B8
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x000710C0 File Offset: 0x0006F2C0
		public float fogOfWarAutoClearRadius
		{
			get
			{
				return this._fogOfWarAutoClearRadius;
			}
			set
			{
				if (value != this._fogOfWarAutoClearRadius)
				{
					this._fogOfWarAutoClearRadius = value;
				}
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x000710D2 File Offset: 0x0006F2D2
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x000710DA File Offset: 0x0006F2DA
		public float fogOfWarDefaultAlpha
		{
			get
			{
				return this._fogOfWarDefaultAlpha;
			}
			set
			{
				if (value != this._fogOfWarDefaultAlpha)
				{
					this._fogOfWarDefaultAlpha = value;
					this.UpdateFogOfWar();
				}
			}
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000710F2 File Offset: 0x0006F2F2
		public void UpdateFogOfWar()
		{
			if (!this.currentMiniMapUsesFogOfWar)
			{
				return;
			}
			if (Application.isPlaying)
			{
				this.needFogOfWarUpdate = true;
				return;
			}
			this.UpdateFogOfWarTexture();
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00071112 File Offset: 0x0006F312
		// (set) Token: 0x060011B6 RID: 4534 RVA: 0x0007111C File Offset: 0x0006F31C
		public Color32[] fogOfWarTextureData
		{
			get
			{
				return this.fogOfWarColorBuffer;
			}
			set
			{
				this.fogOfWarEnabled = true;
				this.fogOfWarColorBuffer = value;
				if (value == null || this.fogOfWarTexture == null)
				{
					return;
				}
				if (value.Length != this.fogOfWarTexture.width * this.fogOfWarTexture.height)
				{
					return;
				}
				this.fogOfWarTexture.SetPixels32(this.fogOfWarColorBuffer);
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00071184 File Offset: 0x0006F384
		public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha, float border)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			float num = (worldPosition.x - this._fogOfWarCenter.x) / this._fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return;
			}
			float num2 = (worldPosition.z - this._fogOfWarCenter.z) / this._fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return;
			}
			int width = this.fogOfWarTexture.width;
			int height = this.fogOfWarTexture.height;
			int num3 = Mathf.Clamp((int)(num * (float)width), 0, width - 1);
			int num4 = Mathf.Clamp((int)(num2 * (float)height), 0, height - 1);
			int num5 = num4 * width + num3;
			byte b = (byte)(fogNewAlpha * 255f);
			float num6 = radius / this._fogOfWarSize.z;
			int num7 = (int)((float)height * num6);
			int num8 = num7 * num7;
			for (int i = num4 - num7; i <= num4 + num7; i++)
			{
				if (i >= 0 && i < height)
				{
					for (int j = num3 - num7; j <= num3 + num7; j++)
					{
						if (j >= 0 && j < width)
						{
							int num9 = (num4 - i) * (num4 - i) + (num3 - j) * (num3 - j);
							if (num9 <= num8)
							{
								num5 = i * width + j;
								Color32 color = this.fogOfWarColorBuffer[num5];
								float num10 = (float)num9 * border / (float)num8;
								if (num10 > 1f)
								{
									num10 = 1f;
								}
								byte b2 = (byte)((double)b * (1.0 - (double)num10) + (double)((float)color.a * num10));
								color.a = b2;
								this.fogOfWarColorBuffer[num5] = color;
								this.needFogOfWarTextureUpdate = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00071364 File Offset: 0x0006F564
		public void SetFogOfWarAlpha(Bounds bounds, float fogNewAlpha, float border)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			Vector3 center = bounds.center;
			float num = (center.x - this._fogOfWarCenter.x) / this._fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return;
			}
			float num2 = (center.z - this._fogOfWarCenter.z) / this._fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return;
			}
			int width = this.fogOfWarTexture.width;
			int height = this.fogOfWarTexture.height;
			int num3 = Mathf.Clamp((int)(num * (float)width), 0, width - 1);
			int num4 = Mathf.Clamp((int)(num2 * (float)height), 0, height - 1);
			byte b = (byte)(fogNewAlpha * 255f);
			float num5 = bounds.extents.x / this._fogOfWarSize.x;
			float num6 = bounds.extents.z / this._fogOfWarSize.z;
			int num7 = (int)((float)width * num5);
			int num8 = (int)((float)height * num6);
			for (int i = num4 - num8; i <= num4 + num8; i++)
			{
				if (i >= 0 && i < height)
				{
					int num9 = num4 - i;
					if (num9 < 0)
					{
						num9 = -num9;
					}
					if (num9 <= num8)
					{
						float num10 = (float)(num8 - num9 + 1) / ((float)num8 * border + 0.0001f);
						for (int j = num3 - num7; j <= num3 + num7; j++)
						{
							if (j >= 0 && j < width)
							{
								int num11 = num3 - j;
								if (num11 < 0)
								{
									num11 = -num11;
								}
								if (num11 <= num7)
								{
									int num12 = i * width + j;
									Color32 color = this.fogOfWarColorBuffer[num12];
									float num13 = (float)(num7 - num11 + 1) / ((float)num7 * border + 0.0001f) * num10;
									if (num13 > 1f)
									{
										num13 = 1f;
									}
									byte b2 = (byte)((float)color.a * (1f - num13) + (float)b * num13);
									color.a = b2;
									this.fogOfWarColorBuffer[num12] = color;
									this.needFogOfWarTextureUpdate = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x000715A0 File Offset: 0x0006F7A0
		public void ResetFogOfWar(float alpha = 1f)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			int height = this.fogOfWarTexture.height;
			int width = this.fogOfWarTexture.width;
			int num = height * width;
			if (this.fogOfWarColorBuffer == null || this.fogOfWarColorBuffer.Length != num)
			{
				this.fogOfWarColorBuffer = new Color32[num];
			}
			byte b = (byte)(alpha * 255f);
			Color32 color = new Color32(b, b, b, b);
			for (int i = 0; i < num; i++)
			{
				this.fogOfWarColorBuffer[i] = color;
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00071628 File Offset: 0x0006F828
		public float GetFogOfWarAlpha(Vector3 worldPosition)
		{
			if (this.fogOfWarColorBuffer == null)
			{
				return 1f;
			}
			float num = (worldPosition.x - this._fogOfWarCenter.x) / this._fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return 1f;
			}
			float num2 = (worldPosition.z - this._fogOfWarCenter.z) / this._fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return 1f;
			}
			int width = this.fogOfWarTexture.width;
			int height = this.fogOfWarTexture.height;
			int num3 = (int)(num * (float)width);
			int num4 = (int)(num2 * (float)height) * width + num3;
			if (num4 < 0 || num4 >= this.fogOfWarColorBuffer.Length)
			{
				return 1f;
			}
			return (float)this.fogOfWarColorBuffer[num4].a / 255f;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00071714 File Offset: 0x0006F914
		public void SetFogOfWar(List<Vector3> points, float stepSize = 1f, float alpha = 1f)
		{
			if (points == null)
			{
				return;
			}
			int count = points.Count;
			if (count < 2)
			{
				return;
			}
			for (int i = 0; i < count - 1; i++)
			{
				Vector3 vector = points[i];
				Vector3 vector2 = points[i + 1];
				vector.y = (vector2.y = 0f);
				int num = Mathf.CeilToInt(Vector3.Distance(vector, vector2) / stepSize);
				for (int j = 0; j <= num; j++)
				{
					float num2 = (float)j / (float)num;
					Vector3 vector3 = Vector3.Lerp(vector, vector2, num2);
					this.SetFogOfWarAlpha(vector3, 1f, alpha, 0f);
				}
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x000717B1 File Offset: 0x0006F9B1
		// (set) Token: 0x060011BD RID: 4541 RVA: 0x000717B9 File Offset: 0x0006F9B9
		public bool showMiniMap
		{
			get
			{
				return this._showMiniMap;
			}
			set
			{
				if (value != this._showMiniMap)
				{
					this._showMiniMap = value;
					this.miniMapFullScreenState = false;
					this.SetupMiniMap(false);
					this.UpdateCompassBarAlpha();
				}
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x000717DF File Offset: 0x0006F9DF
		// (set) Token: 0x060011BF RID: 4543 RVA: 0x000717E7 File Offset: 0x0006F9E7
		public MiniMapPositionAndScaleMode miniMapPositionAndSize
		{
			get
			{
				return this._miniMapPositionAndSize;
			}
			set
			{
				if (value != this._miniMapPositionAndSize)
				{
					this._miniMapPositionAndSize = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00071800 File Offset: 0x0006FA00
		// (set) Token: 0x060011C1 RID: 4545 RVA: 0x00071808 File Offset: 0x0006FA08
		public MiniMapPosition miniMapLocation
		{
			get
			{
				return this._miniMapLocation;
			}
			set
			{
				if (value != this._miniMapLocation)
				{
					this._miniMapLocation = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x00071821 File Offset: 0x0006FA21
		// (set) Token: 0x060011C3 RID: 4547 RVA: 0x00071829 File Offset: 0x0006FA29
		public float miniMapVisibleMaxDistance
		{
			get
			{
				return this._miniMapVisibleMaxDistance;
			}
			set
			{
				if (value != this._miniMapVisibleMaxDistance)
				{
					this._miniMapVisibleMaxDistance = value;
				}
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0007183B File Offset: 0x0006FA3B
		// (set) Token: 0x060011C5 RID: 4549 RVA: 0x00071843 File Offset: 0x0006FA43
		public Vector2 miniMapLocationOffset
		{
			get
			{
				return this._miniMapScreenPositionOffset;
			}
			set
			{
				if (value != this._miniMapScreenPositionOffset)
				{
					this._miniMapScreenPositionOffset = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x00071861 File Offset: 0x0006FA61
		// (set) Token: 0x060011C7 RID: 4551 RVA: 0x00071869 File Offset: 0x0006FA69
		public bool miniMapKeepStraight
		{
			get
			{
				return this._miniMapKeepStraight;
			}
			set
			{
				if (value != this._miniMapKeepStraight)
				{
					this._miniMapKeepStraight = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00071882 File Offset: 0x0006FA82
		// (set) Token: 0x060011C9 RID: 4553 RVA: 0x0007188A File Offset: 0x0006FA8A
		public MiniMapOrientation miniMapOrientation
		{
			get
			{
				return this._miniMapOrientation;
			}
			set
			{
				if (value != this._miniMapOrientation)
				{
					this._miniMapOrientation = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x000718A3 File Offset: 0x0006FAA3
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x000718AB File Offset: 0x0006FAAB
		public float miniMapCameraTilt
		{
			get
			{
				return this._miniMapCameraTilt;
			}
			set
			{
				if (value != this._miniMapCameraTilt)
				{
					this._miniMapCameraTilt = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x000718C4 File Offset: 0x0006FAC4
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x000718CC File Offset: 0x0006FACC
		public float miniMapSize
		{
			get
			{
				return this._miniMapSize;
			}
			set
			{
				if (value != this._miniMapSize)
				{
					this._miniMapSize = Mathf.Max(value, 0.001f);
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x000718EF File Offset: 0x0006FAEF
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x000718F7 File Offset: 0x0006FAF7
		public Vector3 miniMapFollowOffset
		{
			get
			{
				return this._miniMapFollowOffset;
			}
			set
			{
				if (this._miniMapFollowOffset != value)
				{
					this._miniMapFollowOffset = value;
					this.ClampDragOffset();
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x0007191B File Offset: 0x0006FB1B
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x00071923 File Offset: 0x0006FB23
		public Sprite miniMapMaskSprite
		{
			get
			{
				return this._miniMapMaskSprite;
			}
			set
			{
				if (value != this._miniMapMaskSprite)
				{
					this._miniMapMaskSprite = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00071941 File Offset: 0x0006FB41
		// (set) Token: 0x060011D3 RID: 4563 RVA: 0x00071949 File Offset: 0x0006FB49
		public Texture2D miniMapBorderTexture
		{
			get
			{
				return this._miniMapBorderTexture;
			}
			set
			{
				if (value != this._miniMapBorderTexture)
				{
					this._miniMapBorderTexture = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x00071967 File Offset: 0x0006FB67
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x0007196F File Offset: 0x0006FB6F
		public MiniMapStyle miniMapStyle
		{
			get
			{
				return this._miniMapStyle;
			}
			set
			{
				if (value != this._miniMapStyle)
				{
					this._miniMapStyle = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x00071988 File Offset: 0x0006FB88
		// (set) Token: 0x060011D7 RID: 4567 RVA: 0x00071990 File Offset: 0x0006FB90
		public Color miniMapBackgroundColor
		{
			get
			{
				return this._miniMapBackgroundColor;
			}
			set
			{
				if (value != this._miniMapBackgroundColor)
				{
					this._miniMapBackgroundColor = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x000719AE File Offset: 0x0006FBAE
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x000719B6 File Offset: 0x0006FBB6
		public bool miniMapBackgroundOpaque
		{
			get
			{
				return this._miniMapBackgroundOpaque;
			}
			set
			{
				if (value != this._miniMapBackgroundOpaque)
				{
					this._miniMapBackgroundOpaque = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x000719CE File Offset: 0x0006FBCE
		// (set) Token: 0x060011DB RID: 4571 RVA: 0x000719D6 File Offset: 0x0006FBD6
		public MiniMapContents miniMapContents
		{
			get
			{
				return this._miniMapContents;
			}
			set
			{
				if (value != this._miniMapContents)
				{
					this._miniMapContents = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x000719EF File Offset: 0x0006FBEF
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x000719F7 File Offset: 0x0006FBF7
		public Texture miniMapContentsTexture
		{
			get
			{
				return this._miniMapContentsTexture;
			}
			set
			{
				if (value != this._miniMapContentsTexture)
				{
					this._miniMapContentsTexture = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x00071A14 File Offset: 0x0006FC14
		// (set) Token: 0x060011DF RID: 4575 RVA: 0x00071A1C File Offset: 0x0006FC1C
		public bool miniMapContentsTextureAllowRotation
		{
			get
			{
				return this._miniMapContentsTextureAllowRotation;
			}
			set
			{
				if (value != this._miniMapContentsTextureAllowRotation)
				{
					this._miniMapContentsTextureAllowRotation = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x00071A34 File Offset: 0x0006FC34
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x00071A3C File Offset: 0x0006FC3C
		public Sprite miniMapMaskSpriteFullScreenMode
		{
			get
			{
				return this._miniMapMaskSpriteFullScreenMode;
			}
			set
			{
				if (value != this._miniMapMaskSpriteFullScreenMode)
				{
					this._miniMapMaskSpriteFullScreenMode = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00071A5A File Offset: 0x0006FC5A
		// (set) Token: 0x060011E3 RID: 4579 RVA: 0x00071A62 File Offset: 0x0006FC62
		public Texture2D miniMapBorderTextureFullScreenMode
		{
			get
			{
				return this._miniMapBorderTextureFullScreenMode;
			}
			set
			{
				if (value != this._miniMapBorderTextureFullScreenMode)
				{
					this._miniMapBorderTextureFullScreenMode = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00071A80 File Offset: 0x0006FC80
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x00071A88 File Offset: 0x0006FC88
		public MiniMapStyle miniMapFullScreenStyle
		{
			get
			{
				return this._miniMapFullScreenStyle;
			}
			set
			{
				if (value != this._miniMapFullScreenStyle)
				{
					this._miniMapFullScreenStyle = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00071AA1 File Offset: 0x0006FCA1
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00071AA9 File Offset: 0x0006FCA9
		public MiniMapResolution miniMapResolution
		{
			get
			{
				return this._miniMapResolution;
			}
			set
			{
				if (value != this._miniMapResolution)
				{
					this._miniMapResolution = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00071AC2 File Offset: 0x0006FCC2
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00071ACA File Offset: 0x0006FCCA
		public MiniMapResolution miniMapFullScreenResolution
		{
			get
			{
				return this._miniMapFullScreenResolution;
			}
			set
			{
				if (value != this._miniMapFullScreenResolution)
				{
					this._miniMapFullScreenResolution = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00071AE3 File Offset: 0x0006FCE3
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x00071AEB File Offset: 0x0006FCEB
		public float miniMapFullScreenZoomLevel
		{
			get
			{
				return this._miniMapFullScreenZoomLevel;
			}
			set
			{
				value = Mathf.Clamp(value, this._miniMapZoomMin, this._miniMapZoomMax);
				if (value != this._miniMapFullScreenZoomLevel)
				{
					this._miniMapFullScreenZoomLevel = value;
					this.lastViewConeCameraAspect = 0f;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00071B23 File Offset: 0x0006FD23
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x00071B2B File Offset: 0x0006FD2B
		public RectTransform miniMapFullScreenPlaceholder
		{
			get
			{
				return this._miniMapFullScreenPlaceholder;
			}
			set
			{
				if (value != this._miniMapFullScreenPlaceholder)
				{
					this._miniMapFullScreenPlaceholder = value;
					if (this._miniMapFullScreenState)
					{
						this.MiniMapZoomToggle(false);
						return;
					}
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x00071B59 File Offset: 0x0006FD59
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x00071B61 File Offset: 0x0006FD61
		public float miniMapFullScreenClampBorder
		{
			get
			{
				return this._miniMapFullScreenClampBorder;
			}
			set
			{
				if (value != this._miniMapFullScreenClampBorder)
				{
					this._miniMapFullScreenClampBorder = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x00071B7A File Offset: 0x0006FD7A
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x00071B82 File Offset: 0x0006FD82
		public bool miniMapFullScreenClampBorderCircular
		{
			get
			{
				return this._miniMapFullScreenClampBorderCircular;
			}
			set
			{
				if (value != this._miniMapFullScreenClampBorderCircular)
				{
					this._miniMapFullScreenClampBorderCircular = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x00071B9B File Offset: 0x0006FD9B
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00071BA3 File Offset: 0x0006FDA3
		public float miniMapFullScreenSize
		{
			get
			{
				return this._miniMapFullScreenSize;
			}
			set
			{
				if (value != this._miniMapFullScreenSize)
				{
					this._miniMapFullScreenSize = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00071BBC File Offset: 0x0006FDBC
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00071BC4 File Offset: 0x0006FDC4
		public MiniMapContents miniMapFullScreenContents
		{
			get
			{
				return this._miniMapFullScreenContents;
			}
			set
			{
				if (value != this._miniMapFullScreenContents)
				{
					this._miniMapFullScreenContents = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00071BDD File Offset: 0x0006FDDD
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00071BE5 File Offset: 0x0006FDE5
		public Texture miniMapFullScreenContentsTexture
		{
			get
			{
				return this._miniMapFullScreenContentsTexture;
			}
			set
			{
				if (value != this._miniMapFullScreenContentsTexture)
				{
					this._miniMapFullScreenContentsTexture = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00071C03 File Offset: 0x0006FE03
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00071C0B File Offset: 0x0006FE0B
		public bool miniMapKeepAspectRatio
		{
			get
			{
				return this._miniMapKeepAspectRatio;
			}
			set
			{
				if (value != this._miniMapKeepAspectRatio)
				{
					this._miniMapKeepAspectRatio = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00071C24 File Offset: 0x0006FE24
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x00071C2C File Offset: 0x0006FE2C
		public bool miniMapAllowUserDrag
		{
			get
			{
				return this._miniMapAllowUserDrag;
			}
			set
			{
				if (value != this._miniMapAllowUserDrag)
				{
					this._miniMapAllowUserDrag = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x00071C45 File Offset: 0x0006FE45
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x00071C4D File Offset: 0x0006FE4D
		public bool miniMapFullScreenAllowUserDrag
		{
			get
			{
				return this._miniMapFullScreenAllowUserDrag;
			}
			set
			{
				if (value != this._miniMapFullScreenAllowUserDrag)
				{
					this._miniMapFullScreenAllowUserDrag = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x00071C66 File Offset: 0x0006FE66
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x00071C6E File Offset: 0x0006FE6E
		public bool miniMapAutoResetDrag
		{
			get
			{
				return this._miniMapAutoResetDrag;
			}
			set
			{
				if (value != this._miniMapAutoResetDrag)
				{
					this._miniMapAutoResetDrag = value;
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x00071C80 File Offset: 0x0006FE80
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x00071C88 File Offset: 0x0006FE88
		public bool miniMapFullScreenAutoResetDrag
		{
			get
			{
				return this._miniMapFullScreenAutoResetDrag;
			}
			set
			{
				if (value != this._miniMapFullScreenAutoResetDrag)
				{
					this._miniMapFullScreenAutoResetDrag = value;
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x00071C9A File Offset: 0x0006FE9A
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x00071CA2 File Offset: 0x0006FEA2
		public float miniMapDragMaxDistance
		{
			get
			{
				return this._miniMapDragMaxDistance;
			}
			set
			{
				if (value != this._miniMapDragMaxDistance)
				{
					this._miniMapDragMaxDistance = value;
					this.ClampDragOffset();
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x00071CBA File Offset: 0x0006FEBA
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x00071CC2 File Offset: 0x0006FEC2
		public float miniMapFullScreenDragMaxDistance
		{
			get
			{
				return this._miniMapFullScreenDragMaxDistance;
			}
			set
			{
				if (value != this._miniMapFullScreenDragMaxDistance)
				{
					this._miniMapFullScreenDragMaxDistance = value;
					this.ClampDragOffset();
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x00071CDA File Offset: 0x0006FEDA
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x00071CE2 File Offset: 0x0006FEE2
		public MiniMapCameraMode miniMapCameraMode
		{
			get
			{
				return this._miniMapCameraMode;
			}
			set
			{
				if (value != this._miniMapCameraMode)
				{
					this._miniMapCameraMode = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00071CFB File Offset: 0x0006FEFB
		// (set) Token: 0x06001209 RID: 4617 RVA: 0x00071D03 File Offset: 0x0006FF03
		public MiniMapCameraSnapshotFrequency miniMapCameraSnapshotFrequency
		{
			get
			{
				return this._miniMapCameraSnapshotFrequency;
			}
			set
			{
				if (value != this._miniMapCameraSnapshotFrequency)
				{
					this._miniMapCameraSnapshotFrequency = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x00071D1C File Offset: 0x0006FF1C
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x00071D24 File Offset: 0x0006FF24
		public float miniMapCaptureSize
		{
			get
			{
				return this._miniMapCaptureSize;
			}
			set
			{
				if (value != this._miniMapCaptureSize)
				{
					this._miniMapCaptureSize = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x00071D3D File Offset: 0x0006FF3D
		// (set) Token: 0x0600120D RID: 4621 RVA: 0x00071D45 File Offset: 0x0006FF45
		public float miniMapSnapshotInterval
		{
			get
			{
				return this._miniMapSnapshotInterval;
			}
			set
			{
				if (value != this._miniMapSnapshotInterval)
				{
					this._miniMapSnapshotInterval = value;
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x00071D57 File Offset: 0x0006FF57
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x00071D5F File Offset: 0x0006FF5F
		public float miniMapSnapshotDistance
		{
			get
			{
				return this._miniMapSnapshotDistance;
			}
			set
			{
				if (value != this._miniMapSnapshotDistance)
				{
					this._miniMapSnapshotDistance = value;
				}
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x00071D71 File Offset: 0x0006FF71
		// (set) Token: 0x06001211 RID: 4625 RVA: 0x00071D79 File Offset: 0x0006FF79
		public float miniMapContrast
		{
			get
			{
				return this._miniMapContrast;
			}
			set
			{
				if (value != this._miniMapContrast)
				{
					this._miniMapContrast = value;
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x00071D8B File Offset: 0x0006FF8B
		// (set) Token: 0x06001213 RID: 4627 RVA: 0x00071D93 File Offset: 0x0006FF93
		public float miniMapBrightness
		{
			get
			{
				return this._miniMapBrightness;
			}
			set
			{
				if (value != this._miniMapBrightness)
				{
					this._miniMapBrightness = value;
				}
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x00071DA5 File Offset: 0x0006FFA5
		// (set) Token: 0x06001215 RID: 4629 RVA: 0x00071DAD File Offset: 0x0006FFAD
		public Color miniMapTintColor
		{
			get
			{
				return this._miniMapTintColor;
			}
			set
			{
				if (value != this._miniMapTintColor)
				{
					this._miniMapTintColor = value;
				}
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x00071DC4 File Offset: 0x0006FFC4
		// (set) Token: 0x06001217 RID: 4631 RVA: 0x00071DCC File Offset: 0x0006FFCC
		public bool miniMapEnableShadows
		{
			get
			{
				return this._miniMapEnableShadows;
			}
			set
			{
				if (value != this._miniMapEnableShadows)
				{
					this._miniMapEnableShadows = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x00071DE5 File Offset: 0x0006FFE5
		// (set) Token: 0x06001219 RID: 4633 RVA: 0x00071DED File Offset: 0x0006FFED
		public float miniMapZoomMin
		{
			get
			{
				return this._miniMapZoomMin;
			}
			set
			{
				if (value != this._miniMapZoomMin)
				{
					this._miniMapZoomMin = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x00071E06 File Offset: 0x00070006
		// (set) Token: 0x0600121B RID: 4635 RVA: 0x00071E0E File Offset: 0x0007000E
		public float miniMapZoomMax
		{
			get
			{
				return this._miniMapZoomMax;
			}
			set
			{
				if (value != this._miniMapZoomMax)
				{
					this._miniMapZoomMax = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x00071E27 File Offset: 0x00070027
		// (set) Token: 0x0600121D RID: 4637 RVA: 0x00071E2F File Offset: 0x0007002F
		public GameObject miniMapIconPrefab
		{
			get
			{
				return this._miniMapIconPrefab;
			}
			set
			{
				if (value != this._miniMapIconPrefab)
				{
					this._miniMapIconPrefab = value;
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x00071E46 File Offset: 0x00070046
		// (set) Token: 0x0600121F RID: 4639 RVA: 0x00071E50 File Offset: 0x00070050
		public Vector2 miniMapIconPositionShift
		{
			get
			{
				return this._miniMapIconPositionShift;
			}
			set
			{
				if (value != this._miniMapIconPositionShift)
				{
					this._miniMapIconPositionShift = value;
					this._miniMapIconPositionShift.x = Mathf.Clamp(this._miniMapIconPositionShift.x, -1f, 1f);
					this._miniMapIconPositionShift.y = Mathf.Clamp(this._miniMapIconPositionShift.y, -1f, 1f);
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x00071EC3 File Offset: 0x000700C3
		// (set) Token: 0x06001221 RID: 4641 RVA: 0x00071ECC File Offset: 0x000700CC
		public float miniMapZoomLevel
		{
			get
			{
				return this._miniMapZoomLevel;
			}
			set
			{
				float num = Mathf.Clamp(value, this._miniMapZoomMin, this._miniMapZoomMax);
				if (num != this._miniMapZoomLevel)
				{
					this._miniMapZoomLevel = num;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x00071F03 File Offset: 0x00070103
		// (set) Token: 0x06001223 RID: 4643 RVA: 0x00071F0B File Offset: 0x0007010B
		public float miniMapCameraMinAltitude
		{
			get
			{
				return this._miniMapCameraMinAltitude;
			}
			set
			{
				if (value != this._miniMapCameraMinAltitude)
				{
					this._miniMapCameraMinAltitude = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x00071F24 File Offset: 0x00070124
		// (set) Token: 0x06001225 RID: 4645 RVA: 0x00071F2C File Offset: 0x0007012C
		public float miniMapCameraMaxAltitude
		{
			get
			{
				return this._miniMapCameraMaxAltitude;
			}
			set
			{
				value = Mathf.Max(this._miniMapCameraMinAltitude, value);
				if (value != this._miniMapCameraMaxAltitude)
				{
					this._miniMapCameraMaxAltitude = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x00071F53 File Offset: 0x00070153
		// (set) Token: 0x06001227 RID: 4647 RVA: 0x00071F5B File Offset: 0x0007015B
		public float miniMapCameraHeightVSFollow
		{
			get
			{
				return this._miniMapCameraHeightVSFollow;
			}
			set
			{
				if (value != this._miniMapCameraHeightVSFollow)
				{
					this._miniMapCameraHeightVSFollow = value;
					this.UpdateMiniMapContents(1);
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00071F74 File Offset: 0x00070174
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x00071F7C File Offset: 0x0007017C
		public float miniMapCameraDepth
		{
			get
			{
				return this._miniMapCameraDepth;
			}
			set
			{
				if (value != this._miniMapCameraDepth)
				{
					this._miniMapCameraDepth = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x00071F95 File Offset: 0x00070195
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x00071F9D File Offset: 0x0007019D
		public LayerMask miniMapLayerMask
		{
			get
			{
				return this._miniMapLayerMask;
			}
			set
			{
				if (value != this._miniMapLayerMask)
				{
					this._miniMapLayerMask = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x00071FC0 File Offset: 0x000701C0
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x00071FC8 File Offset: 0x000701C8
		public float miniMapIconSize
		{
			get
			{
				return this._miniMapIconSize;
			}
			set
			{
				if (value != this._miniMapIconSize)
				{
					this._miniMapIconSize = value;
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x00071FDA File Offset: 0x000701DA
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x00071FE2 File Offset: 0x000701E2
		public bool miniMapShowPlayerIcon
		{
			get
			{
				return this._miniMapShowPlayerIcon;
			}
			set
			{
				if (value != this._miniMapShowPlayerIcon)
				{
					this._miniMapShowPlayerIcon = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00071FFB File Offset: 0x000701FB
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x00072003 File Offset: 0x00070203
		public float miniMapPlayerIconSize
		{
			get
			{
				return this._miniMapPlayerIconSize;
			}
			set
			{
				if (value != this._miniMapPlayerIconSize)
				{
					this._miniMapPlayerIconSize = value;
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00072015 File Offset: 0x00070215
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x0007201D File Offset: 0x0007021D
		public Sprite miniMapPlayerIconSprite
		{
			get
			{
				return this._miniMapPlayerIconSprite;
			}
			set
			{
				if (value != this._miniMapPlayerIconSprite)
				{
					this._miniMapPlayerIconSprite = value;
				}
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00072034 File Offset: 0x00070234
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x0007203C File Offset: 0x0007023C
		public Color miniMapPlayerIconColor
		{
			get
			{
				return this._miniMapPlayerIconColor;
			}
			set
			{
				if (value != this._miniMapPlayerIconColor)
				{
					this._miniMapPlayerIconColor = value;
				}
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x00072053 File Offset: 0x00070253
		// (set) Token: 0x06001237 RID: 4663 RVA: 0x0007205B File Offset: 0x0007025B
		public bool miniMapShowNorth
		{
			get
			{
				return this._miniMapShowCardinals;
			}
			set
			{
				if (value != this._miniMapShowCardinals)
				{
					this._miniMapShowCardinals = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x00072074 File Offset: 0x00070274
		// (set) Token: 0x06001239 RID: 4665 RVA: 0x0007207C File Offset: 0x0007027C
		public float miniMapCardinalsSize
		{
			get
			{
				return this._miniMapCardinalsSize;
			}
			set
			{
				if (value != this._miniMapCardinalsSize)
				{
					this._miniMapCardinalsSize = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x00072095 File Offset: 0x00070295
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x0007209D File Offset: 0x0007029D
		public Sprite miniMapCardinalsSprite
		{
			get
			{
				return this._miniMapCardinalsSprite;
			}
			set
			{
				if (value != this._miniMapCardinalsSprite)
				{
					this._miniMapCardinalsSprite = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x000720BB File Offset: 0x000702BB
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x000720C3 File Offset: 0x000702C3
		public Color miniMapCardinalsColor
		{
			get
			{
				return this._miniMapCardinalsColor;
			}
			set
			{
				if (value != this._miniMapCardinalsColor)
				{
					this._miniMapCardinalsColor = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x000720E1 File Offset: 0x000702E1
		// (set) Token: 0x0600123F RID: 4671 RVA: 0x000720E9 File Offset: 0x000702E9
		public bool miniMapShowViewCone
		{
			get
			{
				return this._miniMapShowViewCone;
			}
			set
			{
				if (value != this._miniMapShowViewCone)
				{
					this._miniMapShowViewCone = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x00072102 File Offset: 0x00070302
		// (set) Token: 0x06001241 RID: 4673 RVA: 0x0007210A File Offset: 0x0007030A
		public Color miniMapViewConeColor
		{
			get
			{
				return this._miniMapViewConeColor;
			}
			set
			{
				if (value != this._miniMapViewConeColor)
				{
					this._miniMapViewConeColor = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x00072128 File Offset: 0x00070328
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x00072130 File Offset: 0x00070330
		public MiniMapViewConeFovSource miniMapViewConeFoVSource
		{
			get
			{
				return this._miniMapViewConeFoVSource;
			}
			set
			{
				if (value != this._miniMapViewConeFoVSource)
				{
					this._miniMapViewConeFoVSource = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x00072148 File Offset: 0x00070348
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x00072150 File Offset: 0x00070350
		public float miniMapViewConeFoV
		{
			get
			{
				return this._miniMapViewConeFoV;
			}
			set
			{
				if (value != this._miniMapViewConeFoV)
				{
					this._miniMapViewConeFoV = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x00072168 File Offset: 0x00070368
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x00072170 File Offset: 0x00070370
		public float miniMapViewConeDistance
		{
			get
			{
				return this._miniMapViewConeDistance;
			}
			set
			{
				if (value != this._miniMapViewConeDistance)
				{
					this._miniMapViewConeDistance = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x00072188 File Offset: 0x00070388
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x00072190 File Offset: 0x00070390
		public float miniMapViewConeFallOff
		{
			get
			{
				return this._miniMapViewConeFallOff;
			}
			set
			{
				if (value != this._miniMapViewConeFallOff)
				{
					this._miniMapViewConeFallOff = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x000721A8 File Offset: 0x000703A8
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x000721B0 File Offset: 0x000703B0
		public bool miniMapShowViewConeOutline
		{
			get
			{
				return this._miniMapShowViewConeOutline;
			}
			set
			{
				if (value != this._miniMapShowViewConeOutline)
				{
					this._miniMapShowViewConeOutline = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x000721C9 File Offset: 0x000703C9
		// (set) Token: 0x0600124D RID: 4685 RVA: 0x000721D1 File Offset: 0x000703D1
		public Color miniMapViewConeOutlineColor
		{
			get
			{
				return this._miniMapViewConeOutlineColor;
			}
			set
			{
				if (value != this._miniMapViewConeOutlineColor)
				{
					this._miniMapViewConeOutlineColor = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x000721EF File Offset: 0x000703EF
		// (set) Token: 0x0600124F RID: 4687 RVA: 0x000721F7 File Offset: 0x000703F7
		public float miniMapClampBorder
		{
			get
			{
				return this._miniMapClampBorder;
			}
			set
			{
				if (value != this._miniMapClampBorder)
				{
					this._miniMapClampBorder = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001250 RID: 4688 RVA: 0x00072210 File Offset: 0x00070410
		// (set) Token: 0x06001251 RID: 4689 RVA: 0x00072218 File Offset: 0x00070418
		public bool miniMapClampBorderCircular
		{
			get
			{
				return this._miniMapClampBorderCircular;
			}
			set
			{
				if (value != this._miniMapClampBorderCircular)
				{
					this._miniMapClampBorderCircular = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x00072231 File Offset: 0x00070431
		// (set) Token: 0x06001253 RID: 4691 RVA: 0x00072239 File Offset: 0x00070439
		public bool miniMapVignette
		{
			get
			{
				return this._miniMapVignette;
			}
			set
			{
				if (value != this._miniMapVignette)
				{
					this._miniMapVignette = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x00072252 File Offset: 0x00070452
		// (set) Token: 0x06001255 RID: 4693 RVA: 0x0007225A File Offset: 0x0007045A
		public float miniMapAlpha
		{
			get
			{
				return this._miniMapAlpha;
			}
			set
			{
				if (value != this._miniMapAlpha)
				{
					this._miniMapAlpha = value;
				}
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x0007226C File Offset: 0x0007046C
		// (set) Token: 0x06001257 RID: 4695 RVA: 0x00072274 File Offset: 0x00070474
		public bool miniMapShowZoomInOutButtons
		{
			get
			{
				return this._miniMapShowZoomInOutButtons;
			}
			set
			{
				if (value != this._miniMapShowZoomInOutButtons)
				{
					this._miniMapShowZoomInOutButtons = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x0007228D File Offset: 0x0007048D
		// (set) Token: 0x06001259 RID: 4697 RVA: 0x00072295 File Offset: 0x00070495
		public float miniMapIconCircleAnimationDuration
		{
			get
			{
				return this._miniMapIconCircleAnimationDuration;
			}
			set
			{
				if (value != this._miniMapIconCircleAnimationDuration)
				{
					this._miniMapIconCircleAnimationDuration = value;
				}
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x000722A7 File Offset: 0x000704A7
		// (set) Token: 0x0600125B RID: 4699 RVA: 0x000722AF File Offset: 0x000704AF
		public bool miniMapShowMaximizeButton
		{
			get
			{
				return this._miniMapShowMaximizeButton;
			}
			set
			{
				if (value != this._miniMapShowMaximizeButton)
				{
					this._miniMapShowMaximizeButton = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x000722C8 File Offset: 0x000704C8
		// (set) Token: 0x0600125D RID: 4701 RVA: 0x000722D0 File Offset: 0x000704D0
		public float miniMapButtonsScale
		{
			get
			{
				return this._miniMapButtonsScale;
			}
			set
			{
				if (value != this._miniMapButtonsScale)
				{
					this._miniMapButtonsScale = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x000722E9 File Offset: 0x000704E9
		// (set) Token: 0x0600125F RID: 4703 RVA: 0x000722F1 File Offset: 0x000704F1
		public bool miniMapIconEvents
		{
			get
			{
				return this._miniMapIconEvents;
			}
			set
			{
				if (this._miniMapIconEvents != value)
				{
					this._miniMapIconEvents = value;
				}
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00072303 File Offset: 0x00070503
		// (set) Token: 0x06001261 RID: 4705 RVA: 0x0007230B File Offset: 0x0007050B
		public MiniMapRadarInfoType miniMapRadarInfoDisplay
		{
			get
			{
				return this._miniMapRadarInfoDisplay;
			}
			set
			{
				if (this._miniMapRadarInfoDisplay != value)
				{
					this._miniMapRadarInfoDisplay = value;
				}
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0007231D File Offset: 0x0007051D
		// (set) Token: 0x06001263 RID: 4707 RVA: 0x00072325 File Offset: 0x00070525
		public MiniMapRadarGraphicsMethod miniMapRadarGraphicsMethod
		{
			get
			{
				return this._miniMapRadarGraphicsMethod;
			}
			set
			{
				if (this._miniMapRadarGraphicsMethod != value)
				{
					this._miniMapRadarGraphicsMethod = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0007233E File Offset: 0x0007053E
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x00072346 File Offset: 0x00070546
		public Color miniMapRadarRingsColor
		{
			get
			{
				return this._miniMapRadarRingsColor;
			}
			set
			{
				if (value != this._miniMapRadarRingsColor)
				{
					this._miniMapRadarRingsColor = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x00072364 File Offset: 0x00070564
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x0007236C File Offset: 0x0007056C
		public float miniMapRadarRingsDistance
		{
			get
			{
				return this._miniMapRadarRingsDistance;
			}
			set
			{
				if (value != this._miniMapRadarRingsDistance)
				{
					this._miniMapRadarRingsDistance = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00072385 File Offset: 0x00070585
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x0007238D File Offset: 0x0007058D
		public float miniMapRadarRingsWidth
		{
			get
			{
				return this._miniMapRadarRingsWidth;
			}
			set
			{
				if (value != this._miniMapRadarRingsWidth)
				{
					this._miniMapRadarRingsWidth = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x000723A6 File Offset: 0x000705A6
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x000723AE File Offset: 0x000705AE
		public bool miniMapRadarPulseEnabled
		{
			get
			{
				return this._miniMapRadarPulseEnabled;
			}
			set
			{
				if (value != this._miniMapRadarPulseEnabled)
				{
					this._miniMapRadarPulseEnabled = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x000723C7 File Offset: 0x000705C7
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x000723CF File Offset: 0x000705CF
		public MiniMapPulsePreset miniMapRadarPulseAnimationPreset
		{
			get
			{
				return this._miniMapRadarPulseAnimationPreset;
			}
			set
			{
				if (value != this._miniMapRadarPulseAnimationPreset)
				{
					this._miniMapRadarPulseAnimationPreset = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x000723E8 File Offset: 0x000705E8
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x000723F0 File Offset: 0x000705F0
		public float miniMapRadarPulseOpacity
		{
			get
			{
				return this._miniMapRadarPulseOpacity;
			}
			set
			{
				if (value != this._miniMapRadarPulseOpacity)
				{
					this._miniMapRadarPulseOpacity = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x00072408 File Offset: 0x00070608
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x00072410 File Offset: 0x00070610
		public float miniMapRadarPulseFrequency
		{
			get
			{
				return this._miniMapRadarPulseFrequency;
			}
			set
			{
				if (value != this._miniMapRadarPulseFrequency)
				{
					this._miniMapRadarPulseFrequency = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00072428 File Offset: 0x00070628
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x00072430 File Offset: 0x00070630
		public float miniMapRadarPulseFallOff
		{
			get
			{
				return this._miniMapRadarPulseFallOff;
			}
			set
			{
				if (value != this._miniMapRadarPulseFallOff)
				{
					this._miniMapRadarPulseFallOff = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x00072448 File Offset: 0x00070648
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x00072450 File Offset: 0x00070650
		public float miniMapRadarPulseSpeed
		{
			get
			{
				return this._miniMapRadarPulseSpeed;
			}
			set
			{
				if (value != this._miniMapRadarPulseSpeed)
				{
					this._miniMapRadarPulseSpeed = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00072468 File Offset: 0x00070668
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x00072470 File Offset: 0x00070670
		public bool miniMapRadarFadePOIs
		{
			get
			{
				return this._miniMapRadarFadePOIs;
			}
			set
			{
				if (value != this._miniMapRadarFadePOIs)
				{
					this._miniMapRadarFadePOIs = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00072488 File Offset: 0x00070688
		public void MiniMapZoomIn(float speed = 1f)
		{
			float num = Time.deltaTime * speed;
			if (this.miniMapFullScreenState)
			{
				this.miniMapFullScreenZoomLevel += num * this._miniMapFullScreenZoomLevel;
				return;
			}
			this.miniMapZoomLevel += num * this._miniMapZoomLevel;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000724D0 File Offset: 0x000706D0
		public void MiniMapZoomOut(float speed = 1f)
		{
			float num = Time.deltaTime * speed;
			if (this.miniMapFullScreenState)
			{
				this.miniMapFullScreenZoomLevel -= num * this._miniMapFullScreenZoomLevel;
				return;
			}
			this.miniMapZoomLevel -= num * this._miniMapZoomLevel;
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x00072518 File Offset: 0x00070718
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x00072520 File Offset: 0x00070720
		public bool miniMapFullScreenState
		{
			get
			{
				return this._miniMapFullScreenState;
			}
			set
			{
				if (this._miniMapFullScreenState != value)
				{
					if (!this._showMiniMap)
					{
						this.showMiniMap = true;
					}
					UnityEvent<bool> onMiniMapChangeFullScreenState = this.OnMiniMapChangeFullScreenState;
					if (onMiniMapChangeFullScreenState != null)
					{
						onMiniMapChangeFullScreenState.Invoke(value);
					}
					this.MiniMapZoomToggle(value);
				}
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00072553 File Offset: 0x00070753
		public void UpdateMiniMapContents(int numberOfFramesToRefresh = 1)
		{
			if (this.needMiniMapShot == 0)
			{
				this.needMiniMapShot += numberOfFramesToRefresh;
			}
			this.lastViewConeCameraAspect = 0f;
			this.needUpdateMiniMapIcons = true;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0007257D File Offset: 0x0007077D
		public bool IsMouseOverMiniMap()
		{
			return !(this.miniMapUIRootRT == null) && RectTransformUtility.RectangleContainsScreenPoint(this.miniMapUIRootRT, Input.mousePosition);
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x000725A4 File Offset: 0x000707A4
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x000725AC File Offset: 0x000707AC
		[Tooltip("Center of the world map in full screen mode")]
		public Vector3 miniMapFullScreenWorldCenter
		{
			get
			{
				return this._miniMapFullScreenWorldCenter;
			}
			set
			{
				if (this._miniMapFullScreenWorldCenter != value)
				{
					this._miniMapFullScreenWorldCenter = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x000725CA File Offset: 0x000707CA
		// (set) Token: 0x06001281 RID: 4737 RVA: 0x000725D2 File Offset: 0x000707D2
		[Tooltip("Size of the world map")]
		public Vector3 miniMapFullScreenWorldSize
		{
			get
			{
				return this._miniMapFullScreenWorldSize;
			}
			set
			{
				if (this._miniMapFullScreenWorldSize != value)
				{
					this._miniMapFullScreenWorldSize = value;
					this.needUpdateMiniMapIcons = true;
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x000725F0 File Offset: 0x000707F0
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x000725F8 File Offset: 0x000707F8
		public Vector3 miniMapWorldCenter
		{
			get
			{
				return this._miniMapWorldCenter;
			}
			set
			{
				if (this._miniMapWorldCenter != value)
				{
					this._miniMapWorldCenter = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00072615 File Offset: 0x00070815
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0007261D File Offset: 0x0007081D
		public Vector3 miniMapWorldSize
		{
			get
			{
				return this._miniMapWorldSize;
			}
			set
			{
				if (this._miniMapWorldSize != value)
				{
					this._miniMapWorldSize = value;
					this.UpdateMiniMap();
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0007263A File Offset: 0x0007083A
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x00072642 File Offset: 0x00070842
		public float miniMapLutIntensity
		{
			get
			{
				return this._miniMapLutIntensity;
			}
			set
			{
				if (this._miniMapLutIntensity != value)
				{
					this._miniMapLutIntensity = value;
				}
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x00072654 File Offset: 0x00070854
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0007265C File Offset: 0x0007085C
		public Texture2D miniMapLutTexture
		{
			get
			{
				return this._miniMapLutTexture;
			}
			set
			{
				if (this._miniMapLutTexture != value)
				{
					this._miniMapLutTexture = value;
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x00072673 File Offset: 0x00070873
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x0007267B File Offset: 0x0007087B
		public Color miniMapVignetteColor
		{
			get
			{
				return this._miniMapVignetteColor;
			}
			set
			{
				if (value != this._miniMapVignetteColor)
				{
					this._miniMapVignetteColor = value;
					this.SetupMiniMap(false);
				}
			}
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0007269C File Offset: 0x0007089C
		public float GetAltitudeUnderMiniMapCamera()
		{
			if (this.miniMapCamera == null)
			{
				return 0f;
			}
			RaycastHit raycastHit;
			Physics.Raycast(this.miniMapCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out raycastHit);
			return this.miniMapCamera.transform.position.y - raycastHit.distance;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00072701 File Offset: 0x00070901
		public void ResetDragOffset()
		{
			this.miniMapFollowOffset = Misc.Vector3zero;
			this.needUpdateMiniMapIcons = true;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00072715 File Offset: 0x00070915
		public ScanEffect Scan(bool includeDisabledPOIs = true)
		{
			return CompassPro.Scan(this.scanProfile, this.OnScanHit, this.OnScanEnd, null);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0007272F File Offset: 0x0007092F
		public ScanEffect Scan(List<CompassProPOI> pois)
		{
			return CompassPro.Scan(this.scanProfile, this.OnScanHit, this.OnScanEnd, pois);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0007274C File Offset: 0x0007094C
		public static ScanEffect Scan(ScanProfile profile, UnityEvent<ScanEffect, CompassProPOI, Transform> onScanHit = null, UnityEvent<ScanEffect> onScanEnd = null, List<CompassProPOI> pois = null)
		{
			if (profile == null)
			{
				return CompassPro.Scan(default(Color), default(Vector3), 800f, 2f, 2f, 1f, -1, 100f, false, default(Color), 0.1f, 150f, 100f, false, default(Color), true, true, true, default(Color), 0.01f, 0.1f, 2f, 1f, null, null, onScanHit, onScanEnd, pois, true);
			}
			return CompassPro.Scan(profile.color, profile.center, profile.maxRange, profile.duration, profile.fadeOutStart, profile.fadeOutDuration, profile.hitLayerMask, profile.width, profile.grid, profile.gridColor, profile.gridSize, profile.gridDepthWidthControl, profile.gridDepthStart, profile.filled, profile.colorBlend, profile.scanLines, profile.noise, profile.edgeOutline, profile.edgeColor, profile.edgeDepthThreshold, profile.edgeColorThreshold, profile.edgeStrength, profile.edgeWidth, profile.audioClipStart, profile.audioClipHit, onScanHit, onScanEnd, pois, profile.detectDisabledPOIs);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00072890 File Offset: 0x00070A90
		public static ScanEffect Scan(Color color, Vector3 center = default(Vector3), float maxRange = 800f, float duration = 2f, float fadeOutStart = 2f, float fadeOutDuration = 1f, int scanLayerMask = -1, float width = 100f, bool grid = false, Color gridColor = default(Color), float gridSize = 0.1f, float gridDepthWidthControl = 150f, float gridDepthStart = 100f, bool filled = false, Color colorBlend = default(Color), bool scanLines = true, bool noise = true, bool edgeOutline = true, Color edgeColor = default(Color), float edgeDepthThreshold = 0.01f, float edgeColorThreshold = 0.1f, float edgeStrength = 2f, float edgeWidth = 1f, AudioClip audioClipStart = null, AudioClip audioClipHit = null, UnityEvent<ScanEffect, CompassProPOI, Transform> onScanHit = null, UnityEvent<ScanEffect> onScanEnd = null, List<CompassProPOI> pois = null, bool detectDisabledPOIs = true)
		{
			ScanEffect instanceFromPool = ScanEffect.GetInstanceFromPool();
			if (instanceFromPool == null)
			{
				return null;
			}
			Camera main = Camera.main;
			if (main != null)
			{
				main.depthTextureMode = DepthTextureMode.Depth;
				if (center == default(Vector3))
				{
					center = main.transform.position;
				}
			}
			if (color == default(Color))
			{
				color = new Color(0f, 0.5f, 1f, 0.2f);
			}
			if (gridColor == default(Color))
			{
				gridColor = new Color(1f, 1f, 0f);
			}
			if (edgeColor == default(Color))
			{
				edgeColor = Color.white;
			}
			instanceFromPool.color = color;
			instanceFromPool.center = center;
			instanceFromPool.maxRange = maxRange;
			instanceFromPool.duration = duration;
			instanceFromPool.fadeOutStart = fadeOutStart;
			instanceFromPool.fadeOutDuration = fadeOutDuration;
			instanceFromPool.scanLayerMask = scanLayerMask;
			instanceFromPool.width = width;
			instanceFromPool.grid = grid;
			instanceFromPool.gridColor = gridColor;
			instanceFromPool.gridSize = gridSize;
			instanceFromPool.gridDepthWidthControl = gridDepthWidthControl;
			instanceFromPool.gridDepthStart = gridDepthStart;
			instanceFromPool.filled = filled;
			instanceFromPool.scanLines = scanLines;
			instanceFromPool.noise = noise;
			instanceFromPool.edgeOutline = edgeOutline;
			instanceFromPool.edgeColor = edgeColor;
			instanceFromPool.edgeDepthThreshold = edgeDepthThreshold;
			instanceFromPool.edgeColorThreshold = edgeColorThreshold;
			instanceFromPool.edgeStrength = edgeStrength;
			instanceFromPool.edgeWidth = edgeWidth;
			instanceFromPool.colorBlend = colorBlend;
			instanceFromPool.audioClipStart = audioClipStart;
			instanceFromPool.audioClipHit = audioClipHit;
			if (pois != null)
			{
				instanceFromPool.pois.AddRange(pois);
			}
			else if (detectDisabledPOIs)
			{
				instanceFromPool.pois.AddRange(Misc.FindObjectsOfType<CompassProPOI>(true));
				int num = instanceFromPool.pois.Count;
				for (int i = 0; i < num; i++)
				{
					CompassProPOI compassProPOI = instanceFromPool.pois[i];
					if (((1 << compassProPOI.gameObject.layer) & scanLayerMask) == 0)
					{
						instanceFromPool.pois.RemoveAt(i);
						num--;
						i--;
					}
				}
			}
			else
			{
				CompassPro instance = CompassPro.instance;
				if (instance == null)
				{
					instanceFromPool.pois.AddRange(Misc.FindObjectsOfType<CompassProPOI>(false));
				}
				else
				{
					instance.POIGetAll(instanceFromPool.pois);
					int num2 = instanceFromPool.pois.Count;
					for (int j = 0; j < num2; j++)
					{
						CompassProPOI compassProPOI2 = instanceFromPool.pois[j];
						if (((1 << compassProPOI2.gameObject.layer) & scanLayerMask) == 0)
						{
							instanceFromPool.pois.RemoveAt(j);
							num2--;
							j--;
						}
					}
				}
			}
			instanceFromPool.pois.Sort(delegate(CompassProPOI a, CompassProPOI b)
			{
				float sqrMagnitude = (a.transform.position - center).sqrMagnitude;
				float sqrMagnitude2 = (b.transform.position - center).sqrMagnitude;
				return sqrMagnitude.CompareTo(sqrMagnitude2);
			});
			instanceFromPool.ScanStart();
			if (onScanHit != null)
			{
				instanceFromPool.OnScanHit.AddListener(new UnityAction<ScanEffect, CompassProPOI, Transform>(onScanHit.Invoke));
			}
			if (onScanEnd != null)
			{
				instanceFromPool.OnScanEnd.AddListener(new UnityAction<ScanEffect>(onScanEnd.Invoke));
			}
			return instanceFromPool;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00072BA8 File Offset: 0x00070DA8
		public void OnEnable()
		{
			this.activePOIs = this.pois;
			this.RequestPOIsOfInterestUpdate();
			if (!CompassPro.compasses.Contains(this))
			{
				CompassPro.compasses.Add(this);
			}
			if (this._follow == null && this.cameraMain != null)
			{
				this._follow = this._cameraMain.transform;
			}
			if (this.compassPoints == null || this.compassPoints.Length == 0)
			{
				this.Init();
			}
			if (Application.isPlaying && this.currentMiniMapUsesEvents && Misc.FindObjectOfType<EventSystem>(false) == null)
			{
				new GameObject("EventSystem", new Type[] { typeof(EventSystem) }).AddComponent<StandaloneInputModule>();
			}
			this.EnableCompass();
			this.SetupTextPool();
			this.SetupMiniMap(false);
			if (this.dontDestroyOnLoad && Application.isPlaying)
			{
				if (Misc.FindObjectsOfType(base.GetType(), false).Length > 1)
				{
					Object.Destroy(base.gameObject);
					return;
				}
				Object.DontDestroyOnLoad(this);
				SceneManager.sceneLoaded += this.UpdateFogOfWarOnLoadScene;
			}
			foreach (CompassProPOI compassProPOI in Misc.FindObjectsOfType<CompassProPOI>(false))
			{
				if (!this.POIisRegistered(compassProPOI))
				{
					compassProPOI.RegisterPOI();
				}
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00072CE1 File Offset: 0x00070EE1
		private void Start()
		{
			this.needsSetupMiniMap = true;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00072CE1 File Offset: 0x00070EE1
		private void OnRectTransformDimensionsChange()
		{
			this.needsSetupMiniMap = true;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00072CEA File Offset: 0x00070EEA
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.UpdateFogOfWarOnLoadScene;
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00072CFD File Offset: 0x00070EFD
		private void OnDrawGizmosSelected()
		{
			if (this.currentMiniMapContents != MiniMapContents.WorldMappedTexture || !this._showMiniMap)
			{
				return;
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(this.currentMiniMapWorldCenter, this.currentMiniMapWorldSize);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00072D2C File Offset: 0x00070F2C
		private void OnDestroy()
		{
			this.DisableCompass();
			this.DisableMiniMap();
			this.DisableIndicators();
			foreach (CompassProPOI compassProPOI in this.pois)
			{
				if (compassProPOI != null)
				{
					compassProPOI.Release();
				}
			}
			this.pois.Clear();
			if (this.curvedMat != null)
			{
				Object.DestroyImmediate(this.curvedMat);
			}
			if (this.compassBarMat != null)
			{
				Object.DestroyImmediate(this.compassBarMat);
			}
			if (this.defaultUICurvedMatForCardinals != null)
			{
				Object.DestroyImmediate(this.defaultUICurvedMatForCardinals);
			}
			if (this.defaultUICurvedMatForText != null)
			{
				Object.DestroyImmediate(this.defaultUICurvedMatForText);
			}
			this.MiniMapDispose();
			if (this.textPool != null)
			{
				for (int i = 0; i < this.textPool.Length; i++)
				{
					this.ReleaseTextPoolEntry(i);
				}
			}
			if (this.canvasTextPool != null)
			{
				Object.Destroy(this.canvasTextPool.gameObject);
				this.canvasTextPool = null;
			}
			if (CompassPro.compasses.Contains(this))
			{
				CompassPro.compasses.Remove(this);
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00072E70 File Offset: 0x00071070
		private void OnValidate()
		{
			this._miniMapSize = Mathf.Max(0.001f, this._miniMapSize);
			this._miniMapIconSize = Mathf.Max(0f, this._miniMapIconSize);
			this._miniMapViewConeDistance = Mathf.Max(this._miniMapViewConeDistance, 0f);
			this._visitedDistance = Mathf.Max(this._visitedDistance, 1f);
			this._nearDistance = Mathf.Max(10f, this._nearDistance);
			this._miniMapRadarRingsDistance = Mathf.Max(1f, this._miniMapRadarRingsDistance);
			this._onScreenIndicatorScale = Mathf.Max(0.001f, this._onScreenIndicatorScale);
			this._visibleMaxDistance = Mathf.Max(this._visibleMaxDistance, 0f);
			this._visibleMinDistance = Mathf.Max(this._visibleMinDistance, 0f);
			this._miniMapCaptureSize = Mathf.Max(this._miniMapCaptureSize, 2f);
			this._miniMapRadarPulseFallOff = Mathf.Max(0f, this._miniMapRadarPulseFallOff);
			this._miniMapRadarPulseFrequency = Mathf.Max(0f, this._miniMapRadarPulseFrequency);
			this._onScreenIndicatorNearFadeDistance = Mathf.Max(0f, this._onScreenIndicatorNearFadeDistance);
			this._onScreenIndicatorNearFadeMin = Mathf.Max(0f, this._onScreenIndicatorNearFadeMin);
			this._onScreenIndicatorNearFadeMin = Mathf.Min(this._onScreenIndicatorNearFadeMin, this._onScreenIndicatorNearFadeDistance);
			this._onScreenIndicatorFarDistance = Mathf.Max(0f, this._onScreenIndicatorFarDistance);
			this._onScreenIndicatorFarFadeDistance = Mathf.Max(0f, this._onScreenIndicatorFarFadeDistance);
			this._onScreenIndicatorFarFadeDistance = Mathf.Min(this._onScreenIndicatorFarFadeDistance, this._onScreenIndicatorFarDistance);
			this._onScreenIndicatorScaleSpeed = Mathf.Max(1f, this._onScreenIndicatorScaleSpeed);
			this._indicatorUpdateIntervalFrames = Mathf.Max(1, this._indicatorUpdateIntervalFrames);
			this._distanceTextUpdateThreshold = Mathf.Max(0f, this._distanceTextUpdateThreshold);
			this._offScreenIndicatorOverlapMaxPOIs = Mathf.Max(1, this._offScreenIndicatorOverlapMaxPOIs);
			this.needsUpdateSettings = true;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00073062 File Offset: 0x00071262
		private void Init()
		{
			Canvas.ForceUpdateCanvases();
			base.Invoke("CanvasRefresh", 0f);
			this.InitDelayed();
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00073080 File Offset: 0x00071280
		private Camera FindSuitableCamera()
		{
			Camera camera = Camera.main;
			if (camera == null)
			{
				foreach (Camera camera2 in Misc.FindObjectsOfType<Camera>(true))
				{
					if ((!(camera2.targetTexture != null) || camera2.targetTexture.format != RenderTextureFormat.Default) && !(camera2.GetComponentInParent<CompassPro>(true) != null))
					{
						camera = camera2;
						break;
					}
				}
			}
			if (this.hideIfNoCamera)
			{
				base.gameObject.SetActive(camera != null);
			}
			return camera;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x000730FF File Offset: 0x000712FF
		private void CanvasRefresh()
		{
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00073108 File Offset: 0x00071308
		private void InitDelayed()
		{
			this._canvas = base.GetComponent<Canvas>();
			this._canvas.pixelPerfect = false;
			this.pois.Clear();
			this.audioSource = base.GetComponent<AudioSource>();
			if (this.compassIconPrefab == null)
			{
				this.compassIconPrefab = Resources.Load<GameObject>("CNPro/Prefabs/CompassBarIcon");
			}
			if (this.miniMapIconPrefab == null)
			{
				this.miniMapIconPrefab = Resources.Load<GameObject>("CNPro/Prefabs/MiniMapIcon");
			}
			if (this._titleFont == null)
			{
				this._titleFont = Resources.Load<Font>("CNPro/Fonts/Actor-Regular");
			}
			if (this._titleFontTMP == null)
			{
				this._titleFontTMP = Resources.Load<TMP_FontAsset>("CNPro/Fonts/Title Font SDF");
			}
			GameObject gameObject = base.transform.Find("CompassBack").gameObject;
			this.compassBackRT = gameObject.GetComponent<RectTransform>();
			this.compassBackImage = gameObject.GetComponent<Image>();
			this.canvasGroup = this.GetMiniMapCanvasGroup(this.compassBackRT);
			this.text = this.compassBackRT.transform.Find("Text").GetComponent<Text>();
			this.textShadow = this.compassBackRT.transform.Find("TextShadow").GetComponent<Text>();
			this.text.text = (this.textShadow.text = "");
			this.titleRT = this.compassBackRT.transform.Find("Title").GetComponent<RectTransform>();
			this.titleRTDefaultPosition = this.titleRT.position;
			this.title = this.titleRT.GetComponent<Text>();
			this.titleShadowRT = this.compassBackRT.transform.Find("TitleShadow").GetComponent<RectTransform>();
			this.titleShadowRTDefaultPosition = this.titleShadowRT.position;
			this.titleShadow = this.titleShadowRT.GetComponent<Text>();
			this.title.text = (this.titleShadow.text = "");
			this.titleTMPRT = this.compassBackRT.transform.Find("TitleTMP").GetComponent<RectTransform>();
			this.titleTMPRTDefaultPosition = this.titleTMPRT.position;
			this.titleTMP = this.titleTMPRT.GetComponent<TextMeshProUGUI>();
			this.titleTMP.text = "";
			this.canvasGroup.alpha = 0f;
			this.prevAlpha = 0f;
			this.fadeStartTime = Time.time;
			this.lastNearestPOIDistanceText = "";
			this.lastNearestPOIDistance = float.MinValue;
			this.compassPoints = new CompassPro.CompassPointPOI[8];
			this.compassPoints[2].text = this.compassBackRT.Find("CardinalN").GetComponent<Text>();
			this.compassPoints[4].text = this.compassBackRT.Find("CardinalW").GetComponent<Text>();
			this.compassPoints[6].text = this.compassBackRT.Find("CardinalS").GetComponent<Text>();
			this.compassPoints[0].text = this.compassBackRT.Find("CardinalE").GetComponent<Text>();
			this.compassPoints[3].text = this.compassBackRT.Find("InterCardinalNW").GetComponent<Text>();
			this.compassPoints[1].text = this.compassBackRT.Find("InterCardinalNE").GetComponent<Text>();
			this.compassPoints[5].text = this.compassBackRT.Find("InterCardinalSW").GetComponent<Text>();
			this.compassPoints[7].text = this.compassBackRT.Find("InterCardinalSE").GetComponent<Text>();
			this.usedNorthDegrees = -1f;
			MiniMapIconElements[] componentsInChildren = base.GetComponentsInChildren<MiniMapIconElements>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren[i].gameObject);
			}
			CompassPOIElements[] componentsInChildren2 = base.GetComponentsInChildren<CompassPOIElements>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren2[i].gameObject);
			}
			Transform transform = base.transform.Find("OnScreen Indicators Root");
			if (transform != null)
			{
				Object.DestroyImmediate(transform.gameObject);
			}
			MeshRenderer[] componentsInChildren3 = base.GetComponentsInChildren<MeshRenderer>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren3[i].gameObject);
			}
			this.lastUpdateFrameCount = int.MinValue;
			this.lastUpdateTime = float.MinValue;
			this.InitIndicators();
			this.UpdatePOIs();
			this.ComputeCompassPointsPositions();
			this.UpdateCompassBarAppearance();
			this.UpdateHalfWindsAppearance();
			this.UpdateCompassBarAlpha();
			this.SetupMiniMap(false);
			this.UpdateFogOfWarTexture();
			this.Refresh();
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000735C4 File Offset: 0x000717C4
		private void LateUpdate()
		{
			if (this.needsUpdateSettings)
			{
				this.needsUpdateSettings = false;
				this.UpdateSettings();
			}
			if (this._canvas == null)
			{
				return;
			}
			this.UpdatePOIs();
			if (this._showCompassBar || this._showMiniMap)
			{
				if (this._showCompassBar)
				{
					this.UpdateCompassBarIcons();
					this.UpdateCompassBarAlpha();
				}
				if (this._showMiniMap)
				{
					this.UpdateFogOfWarPosition();
					this.UpdateMiniMap();
					this.UpdateMiniMapIcons();
				}
			}
			if ((this._showOnScreenIndicators || this._showOffScreenIndicators) && Application.isPlaying)
			{
				int frameCount = Time.frameCount;
				if (this.needUpdateIndicators || frameCount - this.lastIndicatorsUpdateFrame >= this._indicatorUpdateIntervalFrames)
				{
					this.lastIndicatorsUpdateFrame = frameCount;
					this.UpdateIndicators();
				}
			}
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0007367B File Offset: 0x0007187B
		internal void BubbleEvent<T>(UnityEvent<T> a, T t)
		{
			if (a != null && t != null)
			{
				a.Invoke(t);
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0007368F File Offset: 0x0007188F
		internal void BubbleEvent<T, Q>(UnityEvent<T, Q> a, T t, Q q)
		{
			if (t != null && q != null)
			{
				a.Invoke(t, q);
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000736AC File Offset: 0x000718AC
		public void UpdateSettings()
		{
			if (this._canvas == null)
			{
				this.InitDelayed();
			}
			if (!this.hideIfNoCamera)
			{
				base.gameObject.SetActive(true);
			}
			this.SetupMiniMap(false);
			this.UpdateCompassBarAppearance();
			this.UpdateHalfWindsAppearance();
			this.UpdateCompassBarAlpha();
			this.UpdateFogOfWarTexture();
			this.UpdateTitleAppearance();
			this.UpdateTextAppearance();
			this.InitIndicators();
			if (this.scanProfile != null)
			{
				this.scanProfile.ValidateSettings();
			}
			this.Refresh();
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00073731 File Offset: 0x00071931
		private void EnableCompass()
		{
			if (this.compassBackRT != null)
			{
				this.compassBackRT.gameObject.SetActive(true);
			}
			this.needUpdateCompassBarIcons = true;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00073759 File Offset: 0x00071959
		private void DisableCompass()
		{
			if (this.compassBackRT != null)
			{
				this.compassBackRT.gameObject.SetActive(false);
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0007377C File Offset: 0x0007197C
		private void UpdatePOIs()
		{
			this.activePOIs = this.pois;
			if (this._follow != null)
			{
				this.followPos = this._follow.position;
			}
			if (this.cameraMain == null)
			{
				return;
			}
			Transform transform = this._cameraMain.transform;
			this.currentCamPos = transform.position;
			this.currentCamRot = transform.rotation;
			bool flag = false;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null && this.lastFollowRot != this._follow.rotation)
			{
				this.lastFollowRot = this._follow.rotation;
				flag = true;
			}
			if (this.lastCamPos != this.currentCamPos || this.lastCamRot != this.currentCamRot || !Application.isPlaying || flag)
			{
				this.lastCamPos = this.currentCamPos;
				this.lastCamRot = this.currentCamRot;
				this.RequestPOIIconsUpdate();
			}
			else
			{
				switch (this._updateMode)
				{
				case UpdateMode.NumberOfFrames:
				{
					int frameCount = Time.frameCount;
					if (frameCount - this.lastUpdateFrameCount >= this._updateIntervalFrameCount)
					{
						this.lastUpdateFrameCount = frameCount;
						this.RequestPOIIconsUpdate();
					}
					break;
				}
				case UpdateMode.Time:
				{
					float time = Time.time;
					if (time - this.lastUpdateTime >= this._updateIntervalTime)
					{
						this.lastUpdateTime = time;
						this.RequestPOIIconsUpdate();
					}
					break;
				}
				case UpdateMode.Continuous:
					this.RequestPOIIconsUpdate();
					break;
				}
			}
			Matrix4x4 projectionMatrix = this.cameraMain.projectionMatrix;
			Matrix4x4 matrix4x;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
			{
				matrix4x = this._follow.worldToLocalMatrix;
			}
			else
			{
				matrix4x = transform.worldToLocalMatrix;
			}
			this.currentVP = projectionMatrix * matrix4x;
			if (this.enableAreaOfInterest)
			{
				this.activePOIs = this.poisOfInterest;
				float num = this.followPos.x - this.lastFollowPos.x;
				float num2 = this.followPos.z - this.lastFollowPos.z;
				if (num * num + num2 * num2 >= this.areaOfInterestUpdateDistance * this.areaOfInterestUpdateDistance)
				{
					this.lastFollowPos = this.followPos;
					this.UpdatePOIsOfInterest();
				}
			}
			this.CheckPOIListConsistency();
			if (this.needsIconSorting)
			{
				this.needsIconSorting = false;
				this.activePOIs.Sort(new Comparison<CompassProPOI>(this.IconPriorityComparer));
			}
			if (!Application.isPlaying)
			{
				return;
			}
			Vector3 vector = this.lastVisitedDistanceFollowPos;
			vector.x -= this.followPos.x;
			vector.z -= this.followPos.z;
			if (vector.x * vector.x + vector.z * vector.z > 1f)
			{
				this.lastVisitedDistanceFollowPos = this.followPos;
				for (int i = 0; i < this.activePOIs.Count; i++)
				{
					CompassProPOI compassProPOI = this.activePOIs[i];
					if (compassProPOI.miniMapShowCircle)
					{
						if (compassProPOI.distanceToFollow < 0.02f)
						{
							if (compassProPOI.insideCircle >= 0)
							{
								compassProPOI.insideCircle = -1;
								this.OnPOIEnterCircle.Invoke(compassProPOI);
							}
						}
						else if (compassProPOI.insideCircle <= 0)
						{
							compassProPOI.insideCircle = 1;
							this.OnPOIExitCircle.Invoke(compassProPOI);
						}
					}
					if (!(compassProPOI == null) && !compassProPOI.isVisited)
					{
						float num3 = ((compassProPOI.visitedDistanceOverride > 0f) ? compassProPOI.visitedDistanceOverride : this._visitedDistance);
						if (compassProPOI.canBeVisited && !compassProPOI.isVisited && compassProPOI.distanceToFollow > 0f && compassProPOI.distanceToFollow < num3)
						{
							compassProPOI.isVisited = true;
							if (compassProPOI.hideWhenVisited)
							{
								compassProPOI.enabled = false;
							}
							this.OnPOIVisited.Invoke(compassProPOI);
							if (compassProPOI == null)
							{
								goto IL_047A;
							}
							if (compassProPOI.playAudioClipWhenVisited && this.audioSource != null)
							{
								if (compassProPOI.visitedAudioClipOverride != null)
								{
									this.audioSource.PlayOneShot(compassProPOI.visitedAudioClipOverride);
								}
								else if (this._visitedDefaultAudioClip != null)
								{
									this.audioSource.PlayOneShot(this._visitedDefaultAudioClip);
								}
							}
							this.ShowPOIDiscoveredText(compassProPOI);
						}
						if (compassProPOI.heartbeatEnabled)
						{
							bool flag2 = compassProPOI.distanceToFollow < compassProPOI.heartbeatDistance;
							if (!compassProPOI.heartbeatIsActive && flag2)
							{
								compassProPOI.StartHeartbeat();
							}
							else if (compassProPOI.heartbeatIsActive && !flag2)
							{
								compassProPOI.StopHeartbeat();
							}
							compassProPOI == null;
						}
					}
					IL_047A:;
				}
				this.CheckPOIListConsistency();
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00073C24 File Offset: 0x00071E24
		private bool ShouldUpdateDistanceText(float previousDistance, float currentDistance)
		{
			if (previousDistance < 0f)
			{
				return true;
			}
			float num = currentDistance - previousDistance;
			if (num < 0f)
			{
				num = -num;
			}
			return num >= this._distanceTextUpdateThreshold;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00073C56 File Offset: 0x00071E56
		private void RequestPOIIconsUpdate()
		{
			this.needUpdateMiniMapIcons = true;
			this.needUpdateCompassBarIcons = true;
			this.needUpdateIndicators = true;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00073C6D File Offset: 0x00071E6D
		private void RequestPOIsOfInterestUpdate()
		{
			this.lastFollowPos.x = float.MaxValue;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00073C80 File Offset: 0x00071E80
		private void UpdatePOIsOfInterest()
		{
			float num = this.areaOfInterestMaxDistance * this.areaOfInterestMaxDistance;
			this.poisOfInterest.Clear();
			int num2 = this.pois.Count;
			for (int i = 0; i < num2; i++)
			{
				CompassProPOI compassProPOI = this.pois[i];
				if (compassProPOI == null)
				{
					this.pois.RemoveAt(i);
					i--;
					num2--;
				}
				else if (compassProPOI.ignoreAreaOfInterest)
				{
					this.poisOfInterest.Add(compassProPOI);
				}
				else
				{
					Vector3 position = compassProPOI.transform.position;
					position.x += compassProPOI.positionOffset.x;
					position.z += compassProPOI.positionOffset.z;
					float num3 = position.x - this.followPos.x;
					float num4 = position.z - this.followPos.z;
					if (num3 * num3 + num4 * num4 - compassProPOI.radius * compassProPOI.radius <= num)
					{
						this.poisOfInterest.Add(compassProPOI);
					}
					else
					{
						if (compassProPOI.isVisible)
						{
							compassProPOI.ToggleCompassBarIconVisibility(false);
						}
						if (compassProPOI.miniMapIsVisible)
						{
							compassProPOI.ToggleMiniMapIconVisibility(false);
						}
						if (compassProPOI.indicatorIsVisible)
						{
							compassProPOI.ToggleIndicatorVisibility(false);
						}
						if (compassProPOI.circleIsVisible)
						{
							compassProPOI.ToggleMiniMapCircleVisibility(false);
						}
					}
				}
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00073DD4 File Offset: 0x00071FD4
		private void CheckPOIListConsistency()
		{
			int num = this.activePOIs.Count;
			for (int i = 0; i < num; i++)
			{
				if (this.activePOIs[i] == null)
				{
					this.activePOIs.RemoveAt(i);
					i--;
					num--;
				}
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00073E24 File Offset: 0x00072024
		private void ComputePOIViewportPos(CompassProPOI poi)
		{
			Vector3 position = poi.transform.position;
			position.x += poi.positionOffset.x;
			position.y += poi.positionOffset.y;
			position.z += poi.positionOffset.z;
			float num = position.x - this.followPos.x;
			float num2 = position.z - this.followPos.z;
			float num3 = num * num + num2 * num2;
			if (this._use3Ddistance)
			{
				float num4 = position.y - this.followPos.y;
				num3 += num4 * num4;
			}
			num3 = Mathf.Sqrt(num3);
			num3 -= poi.radius;
			if (num3 <= 0f)
			{
				num3 = 0.01f;
			}
			poi.positionWS = position;
			poi.distanceToFollow = num3;
			if (this._compassBarOrientation == CompassBarOrientation.Follow)
			{
				poi.viewportPos = this._cameraMain.WorldToViewportPoint(poi.positionWS);
				return;
			}
			poi.viewportPos = this.GetViewportPos(position);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00073F28 File Offset: 0x00072128
		private Vector3 GetViewportPos(Vector3 position)
		{
			if (this._cameraMain != null && this._cameraMain.orthographic)
			{
				return this._cameraMain.WorldToViewportPoint(position);
			}
			Vector4 vector;
			vector.x = position.x;
			vector.y = position.y;
			vector.z = position.z;
			vector.w = 1f;
			vector = this.currentVP * vector;
			if (vector.w > 0f)
			{
				if (vector.w < 1E-05f)
				{
					vector.w = 1E-05f;
				}
			}
			else if (vector.w > -1E-05f)
			{
				vector.w = -1E-05f;
			}
			vector.w = -vector.w;
			vector.x /= vector.w;
			vector.y /= vector.w;
			Vector3 vector2;
			vector2.x = vector.x / 2f + 0.5f;
			vector2.y = vector.y / 2f + 0.5f;
			vector2.z = vector.w;
			return vector2;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00074050 File Offset: 0x00072250
		private void UpdateCompassBarIcons()
		{
			if (!this.needUpdateCompassBarIcons || this._cameraMain == null)
			{
				return;
			}
			this.needUpdateCompassBarIcons = false;
			float time = Time.time;
			float num = this._width * 0.5f - this._endCapsWidth / (float)this._cameraMain.pixelWidth;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
			{
				this.forward = this._follow.forward;
			}
			else
			{
				this.forward = this._cameraMain.transform.forward;
			}
			this.forward.y = 0f;
			this.ComputeCompassPointsPositions();
			this.UpdateCardinalPoints(num);
			this.UpdateOrdinalPoints(num);
			this.UpdateHalfWinds(num);
			this.poiVisibleCount = 0;
			this.nearestPOIDistance = float.MaxValue;
			this.nearestPOI = null;
			Scene activeScene = SceneManager.GetActiveScene();
			int frameCount = Time.frameCount;
			Vector2 renderingDisplaySize = this.canvas.renderingDisplaySize;
			float num2 = this._bendAmount * -0.5f * renderingDisplaySize.y / this.canvas.transform.localScale.y;
			int count = this.activePOIs.Count;
			for (int i = 0; i < count; i++)
			{
				CompassProPOI compassProPOI = this.activePOIs[i];
				if (!compassProPOI.isActiveAndEnabled)
				{
					compassProPOI.ToggleCompassBarIconVisibility(false);
				}
				else
				{
					if (frameCount != compassProPOI.viewportPosFrameCount)
					{
						compassProPOI.viewportPosFrameCount = frameCount;
						this.ComputePOIViewportPos(compassProPOI);
					}
					bool flag = false;
					float num3 = compassProPOI.distanceToFollow / this._nearDistance;
					POIVisibility visibility = compassProPOI.visibility;
					float num4 = compassProPOI.iconScale;
					if (!compassProPOI.iconScaleIsFixed)
					{
						num4 *= Mathf.Lerp(this._maxIconSize, this._minIconSize, num3);
					}
					compassProPOI.compassCurrentIconScale = num4;
					bool isVisible = compassProPOI.isVisible;
					float num5 = ((compassProPOI.visibleDistanceOverride > 0f) ? compassProPOI.visibleDistanceOverride : this._visibleMaxDistance);
					float num6 = ((compassProPOI.visibleMinDistanceOverride > 0f) ? compassProPOI.visibleMinDistanceOverride : this._visibleMinDistance);
					bool flag2 = compassProPOI.distanceToFollow >= num6 && compassProPOI.distanceToFollow < num5;
					compassProPOI.isVisible = (flag2 && compassProPOI.visibility == POIVisibility.WhenInRange) || compassProPOI.visibility == POIVisibility.AlwaysVisible;
					if (compassProPOI.isVisible && compassProPOI.dontDestroyOnLoad && compassProPOI.scene != activeScene)
					{
						compassProPOI.isVisible = false;
					}
					if (compassProPOI.isVisited && compassProPOI.hideWhenVisited)
					{
						compassProPOI.isVisible = false;
					}
					if (Application.isPlaying && isVisible != compassProPOI.isVisible)
					{
						if (compassProPOI.isVisible)
						{
							this.OnPOIVisible.Invoke(compassProPOI);
						}
						else if (!compassProPOI.isVisible)
						{
							this.OnPOIHide.Invoke(compassProPOI);
						}
					}
					if (!compassProPOI.isVisible)
					{
						compassProPOI.ToggleCompassBarIconVisibility(false);
					}
					else
					{
						if (compassProPOI.compassIconRT == null)
						{
							if (this.compassIconPrefab == null)
							{
								Debug.LogError("Compass icon prefab couldn't be loaded. This prefab should be located at CompassNavigatorPro/Resources/CNPro/Prefabs/CompassBarIcon");
								goto IL_0CFA;
							}
							GameObject gameObject = Object.Instantiate<GameObject>(this.compassIconPrefab, this.compassBackRT, false);
							CompassPOIElements component = gameObject.GetComponent<CompassPOIElements>();
							if (component == null)
							{
								Debug.LogError("Compass POI prefab missing Compass POI elements component.");
								this.DestroySafe(gameObject);
								goto IL_0CFA;
							}
							gameObject.name = "CompassIcon " + compassProPOI.gameObject.name;
							compassProPOI.compassIconRT = gameObject.GetComponent<RectTransform>();
							compassProPOI.compassIconImage = component.iconImage;
							compassProPOI.compassIconImage.material = null;
							compassProPOI.compassIconDistanceText = component.distanceText;
							if (compassProPOI.compassIconDistanceText != null)
							{
								compassProPOI.compassIconDistanceTextRT = compassProPOI.compassIconDistanceText.GetComponent<RectTransform>();
							}
							compassProPOI.curvedMaterialSet = false;
							compassProPOI.visibleTime = time;
						}
						if (compassProPOI.curvedMaterialSet)
						{
							if (this._bendAmount == 0f && !this._edgeFadeOut)
							{
								compassProPOI.compassIconImage.material = null;
							}
						}
						else if (this._bendAmount != 0f || this._edgeFadeOut)
						{
							compassProPOI.compassIconImage.material = this.curvedMat;
						}
						Vector3 screenPos = this.GetScreenPos(compassProPOI);
						float num7 = screenPos.x;
						if (compassProPOI.clampPosition || this._focusedPOI == compassProPOI)
						{
							if (screenPos.z < 0f)
							{
								num7 = num * -Mathf.Sign(screenPos.x - 0.5f);
								if (compassProPOI.compassCurrentIconScale > 1f)
								{
									compassProPOI.compassCurrentIconScale = 1f;
								}
							}
							else if (num7 < -num)
							{
								num7 = -num;
								if (compassProPOI.compassCurrentIconScale > 1f)
								{
									compassProPOI.compassCurrentIconScale = 1f;
								}
							}
							else if (num7 > num)
							{
								num7 = num;
								if (compassProPOI.compassCurrentIconScale > 1f)
								{
									compassProPOI.compassCurrentIconScale = 1f;
								}
							}
							screenPos.z = 0f;
						}
						float num8 = Mathf.Abs(num7);
						if (num8 > num || screenPos.z < 0f)
						{
							compassProPOI.ToggleCompassBarIconVisibility(false);
						}
						else
						{
							if (compassProPOI.ToggleCompassBarIconVisibility(true))
							{
								compassProPOI.visibleTime = time;
							}
							RectTransform compassIconRT = compassProPOI.compassIconRT;
							RectTransform compassIconRT2 = compassProPOI.compassIconRT;
							Vector2 vector = new Vector2(0.5f + num7 / this._width, 0.5f);
							compassIconRT2.anchorMax = vector;
							compassIconRT.anchorMin = vector;
							flag = true;
						}
						if (flag)
						{
							this.poiVisibleCount++;
							if (compassProPOI.isVisited)
							{
								if (compassProPOI.compassIconImage.sprite != compassProPOI.iconVisited)
								{
									compassProPOI.compassIconImage.sprite = compassProPOI.iconVisited;
								}
							}
							else if (compassProPOI.compassIconImage.sprite != compassProPOI.iconNonVisited)
							{
								compassProPOI.compassIconImage.sprite = compassProPOI.iconNonVisited;
							}
							float num9 = 1f;
							if (this._scaleInDuration > 0f)
							{
								float num10 = (time - compassProPOI.visibleTime) / this._scaleInDuration;
								if (num10 < 1f)
								{
									this.needUpdateCompassBarIcons = true;
									compassProPOI.compassCurrentIconScale *= num10;
								}
								else
								{
									num10 = 1f;
								}
								num9 = num10;
							}
							Transform transform = compassProPOI.compassIconImage.transform;
							if (compassProPOI.compassCurrentIconScale != transform.localScale.x)
							{
								transform.localScale = new Vector3(compassProPOI.compassCurrentIconScale, compassProPOI.compassCurrentIconScale, 1f);
							}
							if (compassProPOI.visibility != POIVisibility.AlwaysVisible)
							{
								float num11 = (this._visibleMaxDistance - compassProPOI.distanceToFollow) / 4f;
								num11 = Mathf.Clamp01(num11);
								num9 *= num11;
							}
							Color tintColor = compassProPOI.tintColor;
							tintColor.a *= num9;
							compassProPOI.compassIconImage.color = tintColor;
							if (this._focusedPOI == compassProPOI || (num8 < this._labelHotZone && compassProPOI.distanceToFollow < this.nearestPOIDistance))
							{
								this.nearestPOI = compassProPOI;
								this.nearestPOIDistance = compassProPOI.distanceToFollow;
								this.nearestPOIAlpha = num9;
							}
							if (compassProPOI.compassIconDistanceText != null)
							{
								bool flag3;
								if (this._showDistance && compassProPOI.iconShowDistance && compassProPOI.distanceToFollow > 0.1f)
								{
									flag3 = true;
									if (string.IsNullOrEmpty(compassProPOI.lastCompassIconDistanceText) || this.ShouldUpdateDistanceText(compassProPOI.lastCompassIconDistance, compassProPOI.distanceToFollow))
									{
										compassProPOI.lastCompassIconDistance = compassProPOI.distanceToFollow;
										compassProPOI.lastCompassIconDistanceText = compassProPOI.distanceToFollow.ToString(this._showDistanceFormat);
									}
									compassProPOI.compassIconDistanceText.text = compassProPOI.lastCompassIconDistanceText;
									float num12 = Mathf.Abs(screenPos.x);
									float num13 = (this._width - num12 * 2f + 0.001f - this._edgeFadeOutStart) / (this._edgeFadeOutWidth + 0.0001f);
									if (num13 < 0f)
									{
										num13 = 0f;
									}
									else if (num13 > 1f)
									{
										num13 = 1f;
									}
									compassProPOI.compassIconDistanceText.color = new Color(1f, 1f, 1f, num9 * num13);
									RectTransform compassIconDistanceTextRT = compassProPOI.compassIconDistanceTextRT;
									if (this._bendAmount != 0f)
									{
										float num14 = Mathf.Sin(compassIconDistanceTextRT.position.x / renderingDisplaySize.x * 3.1415927f) * num2;
										compassIconDistanceTextRT.localPosition = new Vector3(0f, num14, 0f);
									}
									else
									{
										compassIconDistanceTextRT.localPosition = Misc.Vector3zero;
									}
									compassIconDistanceTextRT.sizeDelta = new Vector2(200f, this._showDistanceHeight * (1f + Mathf.Max(0f, compassProPOI.compassCurrentIconScale - 1f) * 0.52f));
								}
								else
								{
									flag3 = false;
								}
								compassProPOI.compassIconDistanceText.enabled = flag3;
							}
						}
					}
					if (this.nearestPOIDistance < 3.4028235E+38f && ((this.nearestPOI.titleVisibility != TitleVisibility.Never) & (this.nearestPOI.isVisited || this.nearestPOI.titleVisibility == TitleVisibility.Always)))
					{
						if (this.nearestPOI != this.lastNearestPOI || this.titleLocale != GameLocalization.CurrentCode)
						{
							this.lastNearestPOIDistance = -1f;
							this.titleLocale = GameLocalization.CurrentCode;
						}
						if (this.lastNearestPOIDistance != this.nearestPOIDistance)
						{
							this.lastNearestPOIDistance = this.nearestPOIDistance;
							if (this.titleText.Length > 0)
							{
								this.titleText.Length = 0;
							}
							this.titleText.Append(NavigationLocalization.PlaceName(this.nearestPOI.title));
							float num15 = ((this.nearestPOI.titleMinPOIDistanceOverride > 0f) ? this.nearestPOI.titleMinPOIDistanceOverride : this._titleMinPOIDistance);
							if (this.lastNearestPOIDistance >= num15)
							{
								bool flag4 = false;
								if (this.nearestPOI.positionWS.y > this.lastCamPos.y + this._sameAltitudeThreshold)
								{
									if (this.titleText.Length > 0)
									{
										this.titleText.Append(" ");
									}
									this.titleText.Append("(").Append(GameLocalization.GetSource("portal_ui", "Above"));
									flag4 = true;
								}
								else if (this.nearestPOI.positionWS.y < this.lastCamPos.y - this._sameAltitudeThreshold)
								{
									if (this.titleText.Length > 0)
									{
										this.titleText.Append(" ");
									}
									this.titleText.Append("(").Append(GameLocalization.GetSource("portal_ui", "Below"));
									flag4 = true;
								}
								if (this._titleShowDistance)
								{
									if (flag4)
									{
										this.titleText.Append(", ");
									}
									else
									{
										if (this.titleText.Length > 0)
										{
											this.titleText.Append(" ");
										}
										this.titleText.Append("(");
									}
									this.titleText.Append(this.lastNearestPOIDistance.ToString(this._titleShowDistanceFormat));
									this.titleText.Append(")");
								}
								else if (flag4)
								{
									this.titleText.Append(")");
								}
							}
							int length = this.titleText.Length;
							int length2 = this.lastNearestPOIDistanceText.Length;
							bool flag5 = length != length2;
							if (!flag5)
							{
								for (int j = 0; j < length; j++)
								{
									if (this.titleText[j] != this.lastNearestPOIDistanceText[j])
									{
										flag5 = true;
										break;
									}
								}
							}
							if (flag5)
							{
								this.lastNearestPOIDistanceText = this.titleText.ToString();
								this.UpdateTitleText(this.lastNearestPOIDistanceText);
								this.UpdateTitleAppearance();
							}
						}
						this.UpdateTitleAlpha(this.nearestPOIAlpha);
						Vector3 position = this.nearestPOI.compassIconRT.position;
						if (this._bendAmount != 0f)
						{
							Transform transform2 = this.titleRT;
							Transform transform3 = this.titleShadowRT;
							Vector3 vector2 = new Vector3(position.x, this.titleRT.position.y, 0f);
							transform3.position = vector2;
							transform2.position = vector2;
						}
						else
						{
							this.titleTMPRT.position = new Vector3(position.x, this.titleTMPRT.position.y, 0f);
						}
					}
					else
					{
						if (this._bendAmount != 0f)
						{
							if (this.title.text.Length > 0)
							{
								this.title.text = (this.titleShadow.text = "");
								this.titleRT.position = this.titleRTDefaultPosition;
								this.titleShadowRT.position = this.titleShadowRTDefaultPosition;
							}
						}
						else if (this.titleTMP.text.Length > 0)
						{
							this.titleTMP.text = "";
							this.titleTMPRT.position = this.titleTMPRTDefaultPosition;
						}
						this.lastNearestPOIDistanceText = "";
					}
				}
				IL_0CFA:;
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00074D66 File Offset: 0x00072F66
		private int IconPriorityComparer(CompassProPOI p1, CompassProPOI p2)
		{
			if (p1.priority < p2.priority)
			{
				return -1;
			}
			if (p1.priority > p2.priority)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00074D8C File Offset: 0x00072F8C
		private Vector3 GetScreenPos(CompassProPOI poi)
		{
			Vector3 vector = Misc.Vector3zero;
			Vector3 vector2 = this.lastCamPos;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
			{
				vector2 = this.followPos;
			}
			switch (this._worldMappingMode)
			{
			case WorldMappingMode.LimitedToBarWidth:
				vector = poi.viewportPos;
				goto IL_018B;
			case WorldMappingMode.Full180Degrees:
			{
				Vector3 vector3 = poi.positionWS - vector2;
				float num = (Quaternion.FromToRotation(this.forward, vector3).eulerAngles.y + 180f) / 180f;
				vector.x = 0.5f + (num % 2f - 1f) * (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
				goto IL_018B;
			}
			case WorldMappingMode.Full360Degrees:
			{
				Vector3 vector4 = poi.positionWS - vector2;
				float num2 = (Quaternion.FromToRotation(this.forward, vector4).eulerAngles.y + 180f) / 180f;
				vector.x = 0.5f + (num2 % 2f - 1f) * 0.5f * (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
				goto IL_018B;
			}
			}
			vector = poi.viewportPos;
			vector.x = 0.5f + (vector.x - 0.5f) * (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
			IL_018B:
			vector.x -= 0.5f;
			return vector;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00074F38 File Offset: 0x00073138
		private Vector3 GetScreenPos(Vector3 position)
		{
			Vector3 vector = Misc.Vector3zero;
			Vector3 vector2 = this.lastCamPos;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
			{
				vector2 = this.followPos;
			}
			switch (this._worldMappingMode)
			{
			case WorldMappingMode.LimitedToBarWidth:
				vector = this.GetViewportPos(position);
				goto IL_0183;
			case WorldMappingMode.Full180Degrees:
			{
				Vector3 vector3 = position - vector2;
				float num = (Quaternion.FromToRotation(this.forward, vector3).eulerAngles.y + 180f) / 180f;
				vector.x = 0.5f + (num % 2f - 1f) * (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
				goto IL_0183;
			}
			case WorldMappingMode.Full360Degrees:
			{
				Vector3 vector4 = position - vector2;
				float num2 = (Quaternion.FromToRotation(this.forward, vector4).eulerAngles.y + 180f) / 180f;
				vector.x = 0.5f + (num2 % 2f - 1f) * 0.5f * (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
				goto IL_0183;
			}
			}
			vector = this.GetViewportPos(position);
			vector.x = 0.5f + (vector.x - 0.5f) * (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
			IL_0183:
			vector.x -= 0.5f;
			return vector;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000750D9 File Offset: 0x000732D9
		public Sprite GetCompassBarSprite()
		{
			if (this.compassBackImage == null)
			{
				return null;
			}
			return this.compassBackImage.sprite;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000750F8 File Offset: 0x000732F8
		private void ComputeCompassPointsPositions()
		{
			if (this._cameraMain == null || this.compassPoints == null)
			{
				return;
			}
			int num = this.compassPoints.Length;
			if (this._northDegrees != this.usedNorthDegrees)
			{
				this.usedNorthDegrees = this._northDegrees;
				for (int i = 0; i < num; i++)
				{
					float num2 = 6.2831855f * (float)i / (float)num - this._northDegrees * 0.017453292f;
					this.compassPoints[i].cos = Mathf.Cos(num2);
					this.compassPoints[i].sin = Mathf.Sin(num2);
				}
			}
			Vector3 vector = this.lastCamPos;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
			{
				vector = this.followPos;
			}
			for (int j = 0; j < num; j++)
			{
				Vector3 vector2 = vector;
				vector2.x += this.compassPoints[j].cos;
				vector2.z += this.compassPoints[j].sin;
				this.compassPoints[j].position = vector2;
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0007521C File Offset: 0x0007341C
		private void UpdateCardinalPoints(float barMax)
		{
			float num = ((this._worldMappingMode == WorldMappingMode.Full180Degrees || this._worldMappingMode == WorldMappingMode.Full360Degrees) ? 0f : 0.001f);
			int num2 = this.cardinals.Length;
			for (int i = 0; i < num2; i++)
			{
				int num3 = this.cardinals[i];
				if (!this._showCardinalPoints || this._style.HasDegreesOrTicks())
				{
					if (this.compassPoints[num3].text.enabled)
					{
						this.compassPoints[num3].text.enabled = false;
					}
				}
				else
				{
					Vector3 screenPos = this.GetScreenPos(this.compassPoints[num3].position);
					float x = screenPos.x;
					if (Mathf.Abs(x) > barMax || screenPos.z < num)
					{
						if (this.compassPoints[num3].text.enabled)
						{
							this.compassPoints[num3].text.enabled = false;
						}
					}
					else
					{
						if (!this.compassPoints[num3].text.enabled)
						{
							this.compassPoints[num3].text.enabled = true;
						}
						RectTransform rectTransform = this.compassPoints[num3].text.rectTransform;
						RectTransform rectTransform2 = rectTransform;
						RectTransform rectTransform3 = rectTransform;
						Vector2 vector = new Vector2(0.5f + x / this._width, 0.5f + this._cardinalPointsVerticalOffset / rectTransform.sizeDelta.y);
						rectTransform3.anchorMax = vector;
						rectTransform2.anchorMin = vector;
						rectTransform.localScale = new Vector3(0.12f * this._cardinalScale, 0.12f * this._cardinalScale, 1f);
					}
				}
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000753D8 File Offset: 0x000735D8
		private void UpdateOrdinalPoints(float barMax)
		{
			float num = ((this._worldMappingMode == WorldMappingMode.Full180Degrees || this._worldMappingMode == WorldMappingMode.Full360Degrees) ? 0f : 0.001f);
			for (int i = 0; i < this.ordinals.Length; i++)
			{
				int num2 = this.ordinals[i];
				if (!(this.compassPoints[num2].text == null))
				{
					if (!this._showOrdinalPoints || this._style.HasDegreesOrTicks())
					{
						if (this.compassPoints[num2].text.enabled)
						{
							this.compassPoints[num2].text.enabled = false;
						}
					}
					else
					{
						Vector3 screenPos = this.GetScreenPos(this.compassPoints[num2].position);
						float x = screenPos.x;
						if (Mathf.Abs(x) > barMax || screenPos.z < num)
						{
							if (this.compassPoints[num2].text.enabled)
							{
								this.compassPoints[num2].text.enabled = false;
							}
						}
						else
						{
							if (!this.compassPoints[num2].text.enabled)
							{
								this.compassPoints[num2].text.enabled = true;
							}
							RectTransform rectTransform = this.compassPoints[num2].text.rectTransform;
							RectTransform rectTransform2 = rectTransform;
							RectTransform rectTransform3 = rectTransform;
							Vector2 vector = new Vector2(0.5f + x / this._width, 0.5f + this._cardinalPointsVerticalOffset / rectTransform.sizeDelta.y);
							rectTransform3.anchorMax = vector;
							rectTransform2.anchorMin = vector;
							rectTransform.localScale = new Vector3(0.12f * this._ordinalScale, 0.12f * this._ordinalScale, 1f);
						}
					}
				}
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000755AC File Offset: 0x000737AC
		private void UpdateCompassBarAlpha()
		{
			if (!this._showCompassBar)
			{
				return;
			}
			if (this._alwaysVisibleInEditMode && !Application.isPlaying)
			{
				this.thisAlpha = Mathf.Max(0.2f, this._alpha);
			}
			else if (this._autoHide)
			{
				if (!this.autoHiding)
				{
					if (this.poiVisibleCount == 0)
					{
						if (this.thisAlpha > 0f)
						{
							this.autoHiding = true;
							this.fadeStartTime = Time.time;
							this.prevAlpha = this.canvasGroup.alpha;
							this.thisAlpha = 0f;
						}
					}
					else if (this.poiVisibleCount > 0 && this.thisAlpha == 0f)
					{
						this.thisAlpha = this._alpha;
						this.autoHiding = true;
						this.fadeStartTime = Time.time;
						this.prevAlpha = this.canvasGroup.alpha;
					}
				}
			}
			else
			{
				this.thisAlpha = this._alpha;
			}
			if (this._miniMapFullScreenState && this._showMiniMap)
			{
				this.thisAlpha = 0f;
			}
			if (this.thisAlpha != this.canvasGroup.alpha)
			{
				float num = (Application.isPlaying ? ((Time.time - this.fadeStartTime) / this._fadeDuration) : 1f);
				this.canvasGroup.alpha = Mathf.Lerp(this.prevAlpha, this.thisAlpha, num);
				if (num >= 1f)
				{
					this.prevAlpha = this.canvasGroup.alpha;
				}
				this.canvasGroup.gameObject.SetActive(this.canvasGroup.alpha > 0f);
				return;
			}
			if (this.autoHiding)
			{
				this.autoHiding = false;
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00075758 File Offset: 0x00073958
		private void UpdateCompassBarAppearance()
		{
			if (this.compassBackImage.isActiveAndEnabled != this._showCompassBar)
			{
				this.compassBackRT.gameObject.SetActive(this._showCompassBar);
			}
			float num = (1f - this._width) * 0.5f;
			float num2 = 1f - num;
			this.compassBackRT.anchorMin = new Vector2(num + this._horizontalPosition, this._verticalPosition);
			this.compassBackRT.anchorMax = new Vector2(num2 + this._horizontalPosition, this._verticalPosition);
			this.compassBackRT.sizeDelta = new Vector2(this.compassBackRT.sizeDelta.x, 25f * this._height);
			if (this.compassBarMat == null)
			{
				this.compassBarMat = Object.Instantiate<Material>(Resources.Load<Material>("CNPro/Materials/CompassBar"));
			}
			this.compassBarMat.DisableKeyword("SCROLLABLE");
			this.compassBarMat.DisableKeyword("SCROLLABLE_180");
			this.compassBarMat.DisableKeyword("SCROLLABLE_360");
			CompassStyle style = this._style;
			Sprite sprite;
			switch (style)
			{
			case CompassStyle.Rounded:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar2");
				goto IL_0348;
			case CompassStyle.Celtic_White:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar3-White");
				goto IL_0348;
			case CompassStyle.Celtic_Black:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar3-Black");
				goto IL_0348;
			case (CompassStyle)4:
			case (CompassStyle)5:
			case (CompassStyle)6:
			case (CompassStyle)7:
			case (CompassStyle)8:
			case (CompassStyle)9:
			case (CompassStyle)14:
			case (CompassStyle)15:
			case (CompassStyle)16:
			case (CompassStyle)17:
			case (CompassStyle)18:
			case (CompassStyle)19:
			case (CompassStyle)25:
			case (CompassStyle)26:
				break;
			case CompassStyle.Fantasy1:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar4");
				goto IL_0348;
			case CompassStyle.Fantasy2:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar5");
				goto IL_0348;
			case CompassStyle.Fantasy3:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar8");
				goto IL_0348;
			case CompassStyle.Fantasy4:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar9");
				goto IL_0348;
			case CompassStyle.SciFi1:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar6");
				goto IL_0348;
			case CompassStyle.SciFi2:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar7");
				goto IL_0348;
			case CompassStyle.SciFi3:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar10");
				goto IL_0348;
			case CompassStyle.SciFi4:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar11");
				goto IL_0348;
			case CompassStyle.SciFi5:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar12");
				goto IL_0348;
			case CompassStyle.SciFi6:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar13");
				goto IL_0348;
			case CompassStyle.Clean:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar14");
				goto IL_0348;
			case CompassStyle.CleanWithIntegratedDegrees:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar15");
				this.worldMappingMode = WorldMappingMode.Full180Degrees;
				this.SetCompassScrollableProperties(0.5f);
				goto IL_0348;
			case CompassStyle.CleanWithIntegratedDegreesAndTicks1:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar16");
				this.worldMappingMode = WorldMappingMode.Full180Degrees;
				this.SetCompassScrollableProperties(0.5f);
				goto IL_0348;
			case CompassStyle.CleanWithIntegratedDegreesAndTicks2:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar17");
				this.worldMappingMode = WorldMappingMode.Full180Degrees;
				this.SetCompassScrollableProperties(0.5f);
				goto IL_0348;
			case CompassStyle.CleanWithIntegratedDegreesAndTicks3:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar18");
				this.worldMappingMode = WorldMappingMode.Full180Degrees;
				this.SetCompassScrollableProperties(0.5f);
				goto IL_0348;
			case CompassStyle.CleanWithIntegratedDegreesAndTicks4:
				sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar19");
				this.worldMappingMode = WorldMappingMode.Full180Degrees;
				this.SetCompassScrollableProperties(0.5f);
				goto IL_0348;
			default:
				if (style == CompassStyle.Custom)
				{
					sprite = this._compassBackSprite;
					if (this._compassBackSpriteScrollable)
					{
						this.SetCompassScrollableProperties(this._compassBackSpriteScrollOffset);
						goto IL_0348;
					}
					goto IL_0348;
				}
				break;
			}
			sprite = Resources.Load<Sprite>("CNPro/Sprites/Bar1");
			IL_0348:
			if (sprite != null && this.compassBackImage.sprite != sprite)
			{
				this.compassBackImage.sprite = sprite;
			}
			this.ToggleCurvedCompass();
			if (this.cameraMain != null)
			{
				float num3 = (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
				Vector4 vector = new Vector4(num3, this._horizontalPosition + 0.5f, this._width * 0.5f - this._endCapsWidth / (float)this._cameraMain.pixelWidth);
				if (this._worldMappingMode == WorldMappingMode.LimitedToBarWidth)
				{
					vector.x = 1f;
				}
				if (this.compassBarMat != null)
				{
					this.compassBarMat.SetVector(ShaderParams.CompassData, vector);
				}
				if (this.defaultUICurvedMatForCardinals != null)
				{
					this.defaultUICurvedMatForCardinals.SetVector(ShaderParams.CompassData, vector);
				}
				if (this.defaultUICurvedMatForText != null)
				{
					this.defaultUICurvedMatForText.SetVector(ShaderParams.CompassData, vector);
				}
				if (this.curvedMat != null)
				{
					this.curvedMat.SetVector(ShaderParams.CompassData, vector);
				}
			}
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00075BD8 File Offset: 0x00073DD8
		private void SetCompassScrollableProperties(float offset)
		{
			WorldMappingMode worldMappingMode = this._worldMappingMode;
			if (worldMappingMode != WorldMappingMode.Full180Degrees)
			{
				if (worldMappingMode == WorldMappingMode.Full360Degrees)
				{
					this.compassBarMat.EnableKeyword("SCROLLABLE_360");
				}
				else
				{
					this.compassBarMat.EnableKeyword("SCROLLABLE");
				}
			}
			else
			{
				this.compassBarMat.EnableKeyword("SCROLLABLE_180");
			}
			this.compassBarMat.SetFloat(ShaderParams.ScrollOffset, offset);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00075C3C File Offset: 0x00073E3C
		private void UpdateHalfWinds(float barMax)
		{
			if (this.compassBarMat == null)
			{
				return;
			}
			float num;
			if (this._compassBarOrientation == CompassBarOrientation.Follow && this._follow != null)
			{
				num = this._follow.eulerAngles.y;
			}
			else
			{
				num = this.currentCamRot.eulerAngles.y;
			}
			this.compassBarMat.SetFloat(ShaderParams.CompassAngle, num - this._northDegrees);
			if (this._worldMappingMode == WorldMappingMode.LimitedToBarWidth || this._worldMappingMode == WorldMappingMode.CameraFrustum)
			{
				this.compassBarMat.SetMatrix(ShaderParams.CompassIP, this._cameraMain.projectionMatrix.inverse);
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00075CE0 File Offset: 0x00073EE0
		private void UpdateHalfWindsAppearance()
		{
			this.compassBarMat.DisableKeyword("TICKS");
			this.compassBarMat.DisableKeyword("TICKS_180");
			this.compassBarMat.DisableKeyword("TICKS_360");
			if (!this._showHalfWinds || this.cameraMain == null || this._style.HasDegreesOrTicks())
			{
				return;
			}
			float num = (this._width - this._endCapsWidth / (float)this._cameraMain.pixelWidth) * 0.9f;
			Vector4 vector = new Vector4(num, this._horizontalPosition + 0.5f, this._width * 0.5f - this._endCapsWidth / (float)this._cameraMain.pixelWidth);
			switch (this._worldMappingMode)
			{
			case WorldMappingMode.LimitedToBarWidth:
				vector.x = 1f;
				this.compassBarMat.EnableKeyword("TICKS");
				break;
			case WorldMappingMode.CameraFrustum:
				this.compassBarMat.EnableKeyword("TICKS");
				break;
			case WorldMappingMode.Full180Degrees:
				this.compassBarMat.EnableKeyword("TICKS_180");
				break;
			case WorldMappingMode.Full360Degrees:
				this.compassBarMat.EnableKeyword("TICKS_360");
				break;
			}
			this.compassBarMat.SetVector(ShaderParams.CompassData, vector);
			this.compassBarMat.SetColor(ShaderParams.TicksColor, this._halfWindsTintColor);
			this.compassBarMat.SetVector(ShaderParams.TicksSize, new Vector4(this._halfWindsWidth, this._halfWindsHeight, this._halfWindsInterval, 0f));
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00075E5C File Offset: 0x0007405C
		public void UpdateTextAppearanceEditMode()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.text.gameObject.SetActive(this._textRevealEnabled);
			this.textShadow.gameObject.SetActive(this._textRevealEnabled);
			this.text.text = (this.textShadow.text = "SAMPLE TEXT");
			this.UpdateTextAlpha(1f);
			this.UpdateTextAppearance();
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00075ED4 File Offset: 0x000740D4
		private void UpdateTextAppearance()
		{
			this.text.alignment = TextAnchor.MiddleCenter;
			Vector3 vector = new Vector3(this._textScale, this._textScale, 1f);
			RectTransform component = this.text.GetComponent<RectTransform>();
			component.pivot = new Vector2(0.5f, 0.5f);
			component.anchoredPosition3D = new Vector3(0f, this._textVerticalPosition, 0f);
			this.text.transform.localScale = vector;
			this.text.font = this._textFont;
			this.textShadow.enabled = this._textShadowEnabled;
			this.textShadow.alignment = TextAnchor.MiddleCenter;
			RectTransform component2 = this.textShadow.GetComponent<RectTransform>();
			component2.pivot = new Vector2(0.5f, 0.5f);
			component2.anchoredPosition3D = new Vector3(1f, this._textVerticalPosition - 1f, 0f);
			this.textShadow.transform.localScale = vector;
			this.textShadow.font = this._textFont;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00075FE0 File Offset: 0x000741E0
		private void UpdateTextAlpha(float t)
		{
			this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, t);
			this.textShadow.color = new Color(0f, 0f, 0f, t);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0007604E File Offset: 0x0007424E
		public void UpdateTitleAppearanceEditMode()
		{
			if (!base.gameObject.activeInHierarchy || Application.isPlaying)
			{
				return;
			}
			this.UpdateTitleText("SAMPLE TITLE");
			this.UpdateTitleAlpha(1f);
			this.UpdateTitleAppearance();
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00076084 File Offset: 0x00074284
		private void UpdateTitleText(string text)
		{
			if (this._bendAmount != 0f)
			{
				Text text2 = this.title;
				this.titleShadow.text = text;
				text2.text = text;
				return;
			}
			this.titleTMP.text = text;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000760C8 File Offset: 0x000742C8
		private void UpdateTitleAppearance()
		{
			if (this._bendAmount != 0f)
			{
				this.titleRT.anchoredPosition3D = new Vector3(0f, this._titleVerticalPosition, 0f);
				Vector3 vector = new Vector3(this._titleScale, this._titleScale, 1f);
				this.titleRT.localScale = vector;
				TMP_FontAsset font = GameLocalization.GetFont(null);
				this.title.font = ((font != null && font.sourceFontFile != null) ? font.sourceFontFile : this._titleFont);
				this.titleShadow.enabled = this._titleShadowEnabled;
				this.titleShadowRT.anchoredPosition3D = new Vector3(1f, this._titleVerticalPosition - 1f, 0f);
				this.titleShadow.transform.localScale = vector;
				this.titleShadow.font = this.title.font;
				this.titleRT.gameObject.SetActive(true);
				this.titleShadowRT.gameObject.SetActive(true);
				this.titleTMPRT.gameObject.SetActive(false);
				return;
			}
			this.titleTMPRT.anchoredPosition3D = new Vector3(0f, this._titleVerticalPosition, 0f);
			Vector3 vector2 = new Vector3(this._titleScale, this._titleScale, 1f);
			this.titleTMPRT.localScale = vector2;
			this.titleTMP.font = GameLocalization.GetFont(this._titleFontTMP);
			this.titleRT.gameObject.SetActive(false);
			this.titleShadowRT.gameObject.SetActive(false);
			this.titleTMPRT.gameObject.SetActive(true);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00076280 File Offset: 0x00074480
		private void UpdateTitleAlpha(float t)
		{
			if (this._bendAmount != 0f)
			{
				if (this.title.color.a != t)
				{
					this.title.color = new Color(this.title.color.r, this.title.color.g, this.title.color.b, t);
					this.titleShadow.color = new Color(0f, 0f, 0f, t);
					return;
				}
			}
			else if (this.titleTMP.color.a != t)
			{
				this.titleTMP.color = new Color(this.titleTMP.color.r, this.titleTMP.color.g, this.titleTMP.color.b, t);
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00076368 File Offset: 0x00074568
		private void SetupTextPool()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.text.text = (this.textShadow.text = "");
			this.UpdateTextAppearance();
			if (this.textPool == null || this.textPool.Length != 256)
			{
				this.textPool = new LetterAnimator[256];
				this.textPoolLetters = new GameObject[256];
				this.textPoolShadows = new GameObject[256];
			}
			if (this.canvasTextPool == null)
			{
				GameObject gameObject = new GameObject("CompassProTextPool");
				this.canvasTextPool = gameObject.transform;
				if (this.dontDestroyOnLoad)
				{
					Object.DontDestroyOnLoad(gameObject);
				}
				else if (base.gameObject.scene.IsValid() && gameObject.scene != base.gameObject.scene)
				{
					SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
				}
			}
			for (int i = 0; i < 256; i++)
			{
				LetterAnimator letterAnimator = this.textPool[i];
				if (!(letterAnimator != null) || !(letterAnimator.text != null) || !(letterAnimator.textShadow != null))
				{
					this.ReleaseTextPoolEntry(i);
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.textShadow.gameObject, this.canvasTextPool, false);
					gameObject2.name = "TextShadowPool";
					Text component = gameObject2.GetComponent<Text>();
					GameObject gameObject3 = Object.Instantiate<GameObject>(this.text.gameObject, this.canvasTextPool, false);
					gameObject3.name = "TextPool";
					Text component2 = gameObject3.GetComponent<Text>();
					LetterAnimator letterAnimator2 = component.gameObject.AddComponent<LetterAnimator>();
					letterAnimator2.poolIndex = i;
					letterAnimator2.text = component2;
					letterAnimator2.textShadow = component;
					LetterAnimator letterAnimator3 = letterAnimator2;
					letterAnimator3.OnAnimationEnds = (OnAnimationEndDelegate)Delegate.Combine(letterAnimator3.OnAnimationEnds, new OnAnimationEndDelegate(this.PushTextToPool));
					letterAnimator2.used = false;
					this.textPool[i] = letterAnimator2;
					this.textPoolLetters[i] = gameObject3;
					this.textPoolShadows[i] = gameObject2;
					this.textPoolOriginalLocalPosition = gameObject3.transform.localPosition;
					this.textPoolOriginalShadowLocalPosition = gameObject2.transform.localPosition;
				}
			}
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x000765A0 File Offset: 0x000747A0
		private void ReleaseTextPoolEntry(int index)
		{
			LetterAnimator letterAnimator = this.textPool[index];
			if (letterAnimator == null && this.textPoolShadows[index] != null)
			{
				letterAnimator = this.textPoolShadows[index].GetComponent<LetterAnimator>();
			}
			if (letterAnimator != null)
			{
				LetterAnimator letterAnimator2 = letterAnimator;
				letterAnimator2.OnAnimationEnds = (OnAnimationEndDelegate)Delegate.Remove(letterAnimator2.OnAnimationEnds, new OnAnimationEndDelegate(this.PushTextToPool));
				if (letterAnimator != null)
				{
					letterAnimator.enabled = false;
				}
			}
			if (this.textPoolLetters[index] != null)
			{
				Object.Destroy(this.textPoolLetters[index]);
			}
			if (this.textPoolShadows[index] != null)
			{
				Object.Destroy(this.textPoolShadows[index]);
			}
			this.textPool[index] = null;
			this.textPoolLetters[index] = null;
			this.textPoolShadows[index] = null;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00076664 File Offset: 0x00074864
		private void FetchTextFromPool(out Text lt, out Text lts)
		{
			if (this.canvasTextPool == null)
			{
				this.SetupTextPool();
			}
			for (int i = 0; i < 256; i++)
			{
				this.poolIndex++;
				if (this.poolIndex >= 256)
				{
					this.poolIndex = 0;
				}
				LetterAnimator letterAnimator = this.textPool[this.poolIndex];
				if (letterAnimator == null || letterAnimator.text == null || letterAnimator.textShadow == null)
				{
					this.SetupTextPool();
				}
				if (!this.textPool[this.poolIndex].used)
				{
					break;
				}
			}
			lts = this.textPool[this.poolIndex].textShadow;
			lts.transform.SetParent(this.compassBackRT, false);
			this.SetCurvedTextMaterial(lts);
			lt = this.textPool[this.poolIndex].text;
			lt.transform.SetParent(this.compassBackRT, false);
			this.SetCurvedTextMaterial(lt);
			this.textPool[this.poolIndex].used = true;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00076778 File Offset: 0x00074978
		private void PushTextToPool(int index)
		{
			LetterAnimator letterAnimator = this.textPool[index];
			if (this.canvasTextPool == null || letterAnimator == null || letterAnimator.text == null || letterAnimator.textShadow == null)
			{
				this.SetupTextPool();
			}
			Transform transform = this.textPool[index].text.transform;
			transform.SetParent(this.canvasTextPool);
			transform.localPosition = this.textPoolOriginalLocalPosition;
			Transform transform2 = this.textPool[index].textShadow.transform;
			transform2.SetParent(this.canvasTextPool);
			transform2.localPosition = this.textPoolOriginalShadowLocalPosition;
			this.textPool[index].used = false;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00076827 File Offset: 0x00074A27
		private void ShowPOIDiscoveredText(CompassProPOI poi)
		{
			if (!this._textRevealEnabled || !this._showCompassBar || string.IsNullOrEmpty(poi.visitedText))
			{
				return;
			}
			base.StartCoroutine(this.AnimateDiscoverText(poi.visitedText));
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0007685A File Offset: 0x00074A5A
		private IEnumerator AnimateDiscoverText(string discoverText)
		{
			int len = discoverText.Length;
			if (this._cameraMain == null || this.textPool == null || this.textPool.Length != 256)
			{
				yield break;
			}
			while (Time.time < this.endTimeOfCurrentTextReveal)
			{
				yield return Misc.WaitForOneSecond;
			}
			float time = Time.time;
			this.endTimeOfCurrentTextReveal = time + this._textRevealDuration + this._textDuration + this._textFadeOutDuration * 0.5f;
			this.text.text = (this.textShadow.text = "");
			this.UpdateTextAppearance();
			TextGenerationSettings generationSettings = this.text.GetGenerationSettings(Misc.Vector2zero);
			generationSettings.scaleFactor = 1f;
			TextGenerator cachedTextGenerator = this.text.cachedTextGenerator;
			float num = this._textScale * this._textLetterSpacing;
			string text = discoverText.Replace(" ", "A");
			float num2 = cachedTextGenerator.GetPreferredWidth(text, generationSettings) * -0.5f * num;
			float num3 = cachedTextGenerator.GetPreferredWidth("A", generationSettings) * num;
			float num4 = 0f;
			for (int i = 0; i < len; i++)
			{
				string text2 = discoverText.Substring(i, 1);
				Text text3;
				Text text4;
				this.FetchTextFromPool(out text3, out text4);
				text4.text = text2;
				text3.text = text2;
				float num5;
				if (" ".Equals(text2))
				{
					num5 = num3;
				}
				else
				{
					num5 = cachedTextGenerator.GetPreferredWidth(text2, generationSettings) * num;
				}
				RectTransform component = text3.GetComponent<RectTransform>();
				component.anchoredPosition3D = new Vector3(num2 + num4 + num5 * 0.5f, component.anchoredPosition3D.y, 0f);
				RectTransform component2 = text4.GetComponent<RectTransform>();
				component2.anchoredPosition3D = new Vector3(num2 + num4 + num5 * 0.5f + 1f, component2.anchoredPosition3D.y, 0f);
				num4 += num5;
				LetterAnimator letterAnimator = this.textPool[this.poolIndex];
				letterAnimator.startTime = time + (float)i * this._textRevealLetterDelay;
				letterAnimator.revealDuration = this._textRevealDuration;
				letterAnimator.startFadeTime = time + this._textRevealDuration + this._textDuration;
				letterAnimator.fadeDuration = this._textFadeOutDuration;
				letterAnimator.enabled = true;
				letterAnimator.Play();
			}
			yield break;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00076870 File Offset: 0x00074A70
		private void ToggleCurvedCompass()
		{
			if (this.compassBackRT == null)
			{
				return;
			}
			if (this.compassBackImage.GetComponent<CompassBarMeshModifier>() == null)
			{
				this.compassBackImage.gameObject.AddComponent<CompassBarMeshModifier>();
			}
			Vector4 vector = new Vector4(this._bendAmount, this._width, this._edgeFadeOutWidth, this._edgeFadeOutStart);
			if (!this._edgeFadeOut)
			{
				vector.z = (vector.w = 0f);
			}
			this.compassBarMat.SetVector(ShaderParams.FXData, vector);
			this.compassBarMat.SetColor(ShaderParams.TintColor, this._compassTintColor);
			this.compassBackImage.material = this.compassBarMat;
			if (this.curvedMat == null)
			{
				this.curvedMat = Object.Instantiate<Material>(Resources.Load<Material>("CNPro/Materials/SpriteCurved"));
				this.curvedMat.DisableKeyword("TICKS");
				this.curvedMat.DisableKeyword("TICKS_180");
				this.curvedMat.DisableKeyword("TICKS_360");
			}
			if (this.defaultUICurvedMatForCardinals == null)
			{
				this.defaultUICurvedMatForCardinals = Object.Instantiate<Material>(Resources.Load<Material>("CNPro/Materials/UIDefaultCurved"));
				this.defaultUICurvedMatForText = Object.Instantiate<Material>(this.defaultUICurvedMatForCardinals);
			}
			this.curvedMat.SetVector(ShaderParams.FXData, vector);
			this.defaultUICurvedMatForCardinals.SetVector(ShaderParams.FXData, vector);
			vector.z = (vector.w = 0f);
			this.defaultUICurvedMatForText.SetVector(ShaderParams.FXData, vector);
			Image[] componentsInChildren = this.compassBackRT.GetComponentsInChildren<Image>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != this.compassBackImage)
				{
					componentsInChildren[i].material = this.curvedMat;
				}
			}
			Text[] componentsInChildren2 = this.compassBackRT.GetComponentsInChildren<Text>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren2[j].name.Contains("Cardinal"))
				{
					componentsInChildren2[j].material = this.defaultUICurvedMatForCardinals;
				}
				else
				{
					componentsInChildren2[j].material = this.defaultUICurvedMatForText;
				}
			}
			RawImage[] componentsInChildren3 = this.compassBackRT.GetComponentsInChildren<RawImage>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].material = this.defaultUICurvedMatForCardinals;
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00076ABA File Offset: 0x00074CBA
		private void SetCurvedTextMaterial(Text text)
		{
			if (this._bendAmount != 0f)
			{
				text.material = this.defaultUICurvedMatForText;
				return;
			}
			text.material = null;
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00076ADD File Offset: 0x00074CDD
		private bool currentMiniMapUsesFogOfWar
		{
			get
			{
				return this._fogOfWarEnabled && (this.currentMiniMapContents == MiniMapContents.TopDownWorldView || this.currentMiniMapContents == MiniMapContents.WorldMappedTexture);
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00076AFC File Offset: 0x00074CFC
		private void UpdateFogOfWarOnLoadScene(Scene scene, LoadSceneMode loadMode)
		{
			if (loadMode == LoadSceneMode.Single)
			{
				this.UpdateFogOfWar();
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00076B08 File Offset: 0x00074D08
		private void UpdateFogOfWarTexture()
		{
			if (this.miniMapCamera == null)
			{
				return;
			}
			if (!this.currentMiniMapUsesFogOfWar)
			{
				return;
			}
			if (this.fogOfWarTexture == null || this.fogOfWarTexture.width != this._fogOfWarTextureSize || this.fogOfWarTexture.height != this._fogOfWarTextureSize)
			{
				this.fogOfWarTexture = new Texture2D(this._fogOfWarTextureSize, this._fogOfWarTextureSize, TextureFormat.Alpha8, false);
				this.fogOfWarTexture.filterMode = FilterMode.Bilinear;
				this.fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
				this.ResetFogOfWar(this._fogOfWarDefaultAlpha);
			}
			else if (this.fogOfWarColorBuffer == null || this.fogOfWarColorBuffer.Length != this.fogOfWarTexture.width * this.fogOfWarTexture.height || !Application.isPlaying)
			{
				this.ResetFogOfWar(this._fogOfWarDefaultAlpha);
			}
			CompassProFogVolume[] array = Misc.FindObjectsOfType<CompassProFogVolume>(false);
			Array.Sort<CompassProFogVolume>(array, new Comparison<CompassProFogVolume>(this.VolumeComparer));
			for (int i = 0; i < array.Length; i++)
			{
				Collider component = array[i].GetComponent<Collider>();
				if (component != null && component.gameObject.activeInHierarchy)
				{
					this.SetFogOfWarAlpha(component.bounds, array[i].alpha, array[i].border);
				}
			}
			this.needFogOfWarTextureUpdate = true;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00076C48 File Offset: 0x00074E48
		private void UpdateFogOfWarPosition()
		{
			if (!this.currentMiniMapUsesFogOfWar)
			{
				return;
			}
			if (this.needFogOfWarUpdate)
			{
				this.needFogOfWarUpdate = false;
				this.UpdateFogOfWarTexture();
			}
			if (this._fogOfWarAutoClear)
			{
				int num = (int)this.followPos.x;
				int num2 = (int)this.followPos.z;
				if (num != this.fogOfWarAutoClearLastPosX || num2 != this.fogOfWarAutoClearLastPosZ)
				{
					this.fogOfWarAutoClearLastPosX = num;
					this.fogOfWarAutoClearLastPosZ = num2;
					this.SetFogOfWarAlpha(this.followPos, this._fogOfWarAutoClearRadius, 0f, 1f);
				}
			}
			if (this.needFogOfWarTextureUpdate)
			{
				this.needFogOfWarTextureUpdate = false;
				if (this.fogOfWarTexture != null)
				{
					this.fogOfWarTexture.SetPixels32(this.fogOfWarColorBuffer);
					this.fogOfWarTexture.Apply();
				}
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00076D0A File Offset: 0x00074F0A
		private int VolumeComparer(CompassProFogVolume v1, CompassProFogVolume v2)
		{
			if (v1.order < v2.order)
			{
				return -1;
			}
			if (v1.order > v2.order)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00076D30 File Offset: 0x00074F30
		private void InitIndicators()
		{
			if (this._onScreenIndicatorPrefab == null)
			{
				this._onScreenIndicatorPrefab = Resources.Load<GameObject>("CNPro/Prefabs/POIGizmo");
			}
			if (this.indicatorsRoot == null)
			{
				this.indicatorsRoot = base.transform.Find("OnScreen Indicators Root");
				if (this.indicatorsRoot == null)
				{
					GameObject gameObject = Resources.Load<GameObject>("CNPro/Prefabs/OnScreenIndicatorsRoot");
					if (gameObject != null)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, base.transform, false);
						gameObject2.name = "OnScreen Indicators Root";
						this.indicatorsRoot = gameObject2.transform;
					}
				}
			}
			this.indicatorsRoot.gameObject.SetActive(this._showOnScreenIndicators || this._showOffScreenIndicators);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00076DE7 File Offset: 0x00074FE7
		private void DisableIndicators()
		{
			if (this.indicatorsRoot != null)
			{
				this.indicatorsRoot.gameObject.SetActive(false);
			}
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00076E08 File Offset: 0x00075008
		private void UpdateIndicators()
		{
			float aspect = this._cameraMain.aspect;
			bool orthographic = this._cameraMain.orthographic;
			float num = 1f;
			float num2 = this._offScreenIndicatorOverlapDistance * 0.9f;
			float width = this._offScreenIndicatorRect.width;
			float height = this._offScreenIndicatorRect.height;
			float num3 = (0.5f - this._offScreenIndicatorMargin) * width;
			float num4 = (0.5f - this._offScreenIndicatorMargin * aspect) * height;
			float num5 = this._offScreenIndicatorRect.xMin + this._offScreenIndicatorMargin;
			float num6 = this._offScreenIndicatorRect.xMax - this._offScreenIndicatorMargin;
			float num7 = this._offScreenIndicatorRect.yMin + this._offScreenIndicatorMargin * aspect;
			float num8 = this._offScreenIndicatorRect.yMax - this._offScreenIndicatorMargin * aspect;
			float x = this._offScreenIndicatorRect.center.x;
			float y = this._offScreenIndicatorRect.center.y;
			Vector3 vector3one = Misc.Vector3one;
			float num9 = Time.deltaTime * this._onScreenIndicatorScaleSpeed;
			int frameCount = Time.frameCount;
			int num10 = 0;
			int count = this.activePOIs.Count;
			bool flag = this._offScreenIndicatorAvoidOverlap && count <= this._offScreenIndicatorOverlapMaxPOIs;
			if (this.needUpdateIndicators)
			{
				this.needUpdateIndicators = false;
				for (int i = 0; i < count; i++)
				{
					this.activePOIs[i].lastIndicatorViewportPos.x = -1f;
				}
			}
			for (int j = 0; j < count; j++)
			{
				CompassProPOI compassProPOI = this.activePOIs[j];
				if (!compassProPOI.isActiveAndEnabled)
				{
					compassProPOI.ToggleIndicatorVisibility(false);
				}
				else
				{
					bool flag2 = !this._miniMapFullScreenState || !this._showMiniMap;
					if (compassProPOI.isVisited && compassProPOI.hideWhenVisited)
					{
						flag2 = false;
					}
					if (!flag2)
					{
						compassProPOI.ToggleIndicatorVisibility(false);
					}
					else
					{
						if (frameCount != compassProPOI.viewportPosFrameCount)
						{
							compassProPOI.viewportPosFrameCount = frameCount;
							this.ComputePOIViewportPos(compassProPOI);
						}
						float num11 = ((compassProPOI.onScreenIndicatorFarDistance > 0f) ? compassProPOI.onScreenIndicatorFarDistance : this._onScreenIndicatorFarDistance);
						if (compassProPOI.distanceToFollow > num11)
						{
							compassProPOI.ToggleIndicatorVisibility(false);
						}
						else
						{
							Vector3 vector = compassProPOI.viewportPos;
							bool flag3 = vector.z > 0f && vector.x >= num5 && vector.x < num6 && vector.y >= num7 && vector.y < num8;
							float num12 = 0f;
							float num13 = 1f;
							if (flag3)
							{
								flag2 = this._showOnScreenIndicators && compassProPOI.showOnScreenIndicator;
								if (flag2 && compassProPOI.isOnScreen >= 0)
								{
									compassProPOI.isOnScreen = -1;
									this.OnPOIOnScreen.Invoke(compassProPOI);
								}
								num13 = this._onScreenIndicatorScale * compassProPOI.onScreenIndicatorScale;
							}
							else
							{
								flag2 = this._showOffScreenIndicators && compassProPOI.showOffScreenIndicator;
								if (flag2)
								{
									if (compassProPOI.isOnScreen <= 0)
									{
										compassProPOI.isOnScreen = 1;
										this.OnPOIOffScreen.Invoke(compassProPOI);
									}
									num13 = this._offScreenIndicatorScale * compassProPOI.onScreenIndicatorScale;
									vector.x -= 0.5f;
									vector.y -= 0.5f;
									if (vector.z < 0f)
									{
										if (!orthographic)
										{
											vector *= -1f;
										}
										else
										{
											vector.z = -vector.z;
										}
										if (vector.y > 0f)
										{
											vector.y = -vector.y;
										}
									}
									num12 = Mathf.Atan2(vector.y, vector.x);
									float num14 = Mathf.Tan(num12);
									float num15;
									float num16;
									if (compassProPOI.offScreenIndicatorMarginOverride != 0f)
									{
										num15 = (0.5f - compassProPOI.offScreenIndicatorMarginOverride) * width;
										num16 = (0.5f - compassProPOI.offScreenIndicatorMarginOverride * aspect) * height;
									}
									else
									{
										num15 = num3;
										num16 = num4;
									}
									if (vector.x > 0f)
									{
										vector.x = num15;
										vector.y = num15 * num14;
									}
									else
									{
										vector.x = -num15;
										vector.y = -num15 * num14;
									}
									if (vector.y > num16)
									{
										vector.x = num16 / num14;
										vector.y = num16;
									}
									else if (vector.y < -num16)
									{
										vector.x = -num16 / num14;
										vector.y = -num16;
									}
									if (flag && num10 < 100)
									{
										float num17 = 0f;
										bool flag4 = vector.x * vector.x > vector.y * vector.y;
										int num18 = Mathf.Min(num10, 100);
										if (flag4)
										{
											for (int k = 0; k < num18; k++)
											{
												float num19 = this.lastVPos[k].x - vector.x;
												if (num19 < 0f)
												{
													num19 = -num19;
												}
												float num20 = this.lastVPos[k].y - vector.y;
												if (num20 < 0f)
												{
													num20 = -num20;
												}
												if (num19 < num2 && num20 < num2)
												{
													if (num17 <= 0f)
													{
														vector = this.lastVPos[k];
														num17 = this._offScreenIndicatorOverlapDistance * num;
													}
													vector.y += num17;
													if (vector.y < -0.4f || vector.y > 0.4f)
													{
														break;
													}
													k = -1;
												}
											}
										}
										else
										{
											for (int l = 0; l < num18; l++)
											{
												float num21 = this.lastVPos[l].x - vector.x;
												if (num21 < 0f)
												{
													num21 = -num21;
												}
												float num22 = this.lastVPos[l].y - vector.y;
												if (num22 < 0f)
												{
													num22 = -num22;
												}
												if (num21 < num2 && num22 < num2)
												{
													if (num17 <= 0f)
													{
														vector = this.lastVPos[l];
														num17 = this._offScreenIndicatorOverlapDistance * num;
													}
													vector.x += num17;
													if (vector.x < -0.4f || vector.x > 0.4f)
													{
														break;
													}
													l = -1;
												}
											}
										}
										num = -num;
										this.lastVPos[num10++] = vector;
									}
									vector.x += x;
									vector.y += y;
								}
							}
							if (compassProPOI.indicatorImage != null)
							{
								compassProPOI.ToggleIndicatorVisibility(flag2);
								if (!flag2)
								{
									goto IL_0B20;
								}
							}
							else
							{
								if (!flag2)
								{
									goto IL_0B20;
								}
								GameObject indicator = this.GetIndicator();
								compassProPOI.indicatorRT = indicator.GetComponent<RectTransform>();
								compassProPOI.indicatorCanvasGroup = indicator.GetComponent<CanvasGroup>();
								GizmoElements componentInChildren = indicator.GetComponentInChildren<GizmoElements>();
								if (componentInChildren == null)
								{
									Debug.LogError("Gizmo prefab missing GizmoElements component.");
									Object.DestroyImmediate(indicator);
									goto IL_0B20;
								}
								compassProPOI.indicatorImage = componentInChildren.iconImage;
								compassProPOI.indicatorDistanceText = componentInChildren.distanceText;
								compassProPOI.indicatorTitleText = componentInChildren.titleText;
								compassProPOI.indicatorArrowRT = componentInChildren.arrowPivot;
								compassProPOI.indicatorRT.localScale = Misc.Vector3zero;
							}
							RectTransform indicatorRT = compassProPOI.indicatorRT;
							vector3one.x = (vector3one.y = num13);
							Vector3 vector2 = Vector3.Lerp(indicatorRT.localScale, vector3one, num9);
							indicatorRT.localScale = vector2;
							if (!(compassProPOI.lastIndicatorViewportPos == vector))
							{
								compassProPOI.lastIndicatorViewportPos = vector;
								compassProPOI.indicatorRT.anchorMin = (compassProPOI.indicatorRT.anchorMax = vector);
								compassProPOI.indicatorImage.sprite = ((compassProPOI.isVisited && compassProPOI.iconVisited != null) ? compassProPOI.iconVisited : compassProPOI.iconNonVisited);
								bool flag5 = (flag3 && compassProPOI.onScreenIndicatorShowDistance && this._onScreenIndicatorShowDistance) || (!flag3 && compassProPOI.offScreenIndicatorShowDistance && this._offScreenIndicatorShowDistance);
								if (compassProPOI.indicatorDistanceText.isActiveAndEnabled != flag5)
								{
									compassProPOI.indicatorDistanceText.gameObject.SetActive(flag5);
								}
								bool flag6 = flag3 && compassProPOI.onScreenIndicatorShowTitle && this._onScreenIndicatorShowTitle;
								if (compassProPOI.indicatorTitleText.isActiveAndEnabled != flag6)
								{
									compassProPOI.indicatorTitleText.gameObject.SetActive(flag6);
								}
								float num29;
								if (flag3)
								{
									float num23 = ((compassProPOI.onScreenIndicatorNearFadeMin > 0f) ? compassProPOI.onScreenIndicatorNearFadeMin : this._onScreenIndicatorNearFadeMin);
									float num24 = ((compassProPOI.onScreenIndicatorNearFadeDistance > 0f) ? compassProPOI.onScreenIndicatorNearFadeDistance : this._onScreenIndicatorNearFadeDistance);
									float num25 = ((compassProPOI.onScreenIndicatorFarFadeDistance > 0f) ? compassProPOI.onScreenIndicatorFarFadeDistance : this._onScreenIndicatorFarFadeDistance);
									float num26 = ((num24 <= num23) ? 1f : Mathf.Clamp01((compassProPOI.distanceToFollow - num23) / (num24 - num23)));
									float num27 = ((num25 >= num11) ? 1f : Mathf.Clamp01((num11 - compassProPOI.distanceToFollow) / (num11 - num25)));
									float num28 = num26 * num27;
									num29 = this._onScreenIndicatorAlpha * num28;
									if (compassProPOI.onScreenIndicatorShowDistance && this._onScreenIndicatorShowDistance && (string.IsNullOrEmpty(compassProPOI.lastIndicatorDistanceText) || this.ShouldUpdateDistanceText(compassProPOI.prevIndicatorDistance, compassProPOI.distanceToFollow)))
									{
										compassProPOI.prevIndicatorDistance = compassProPOI.distanceToFollow;
										compassProPOI.lastIndicatorDistanceText = compassProPOI.distanceToFollow.ToString(this._onScreenIndicatorShowDistanceFormat);
										compassProPOI.indicatorDistanceText.text = compassProPOI.lastIndicatorDistanceText;
									}
									if (compassProPOI.onScreenIndicatorShowTitle && this._onScreenIndicatorShowTitle)
									{
										if (!compassProPOI.indicatorTitleText.enabled)
										{
											compassProPOI.indicatorTitleText.enabled = true;
										}
										compassProPOI.indicatorTitleText.text = NavigationLocalization.PlaceName(compassProPOI.title);
										if (compassProPOI.indicatorTitleText.GetComponent<LocalizedGameText>() == null)
										{
											LocalizedGameText.BindFontOnly(compassProPOI.indicatorTitleText);
										}
										if (vector.x > 0.85f)
										{
											compassProPOI.indicatorTitleText.alignment = TextAlignmentOptions.MidlineRight;
										}
										else if (vector.x < 0.15f)
										{
											compassProPOI.indicatorTitleText.alignment = TextAlignmentOptions.MidlineLeft;
										}
										else
										{
											compassProPOI.indicatorTitleText.alignment = TextAlignmentOptions.Midline;
										}
									}
								}
								else
								{
									num29 = this._offScreenIndicatorAlpha;
									compassProPOI.indicatorArrowRT.localRotation = Quaternion.Euler(0f, 0f, num12 * 57.29578f);
									if (compassProPOI.offScreenIndicatorShowDistance && this._offScreenIndicatorShowDistance && (string.IsNullOrEmpty(compassProPOI.lastIndicatorDistanceText) || this.ShouldUpdateDistanceText(compassProPOI.prevIndicatorDistance, compassProPOI.distanceToFollow)))
									{
										compassProPOI.prevIndicatorDistance = compassProPOI.distanceToFollow;
										compassProPOI.lastIndicatorDistanceText = compassProPOI.distanceToFollow.ToString(this._offScreenIndicatorShowDistanceFormat);
										compassProPOI.indicatorDistanceText.text = compassProPOI.lastIndicatorDistanceText;
									}
								}
								compassProPOI.indicatorImage.color = compassProPOI.tintColor;
								compassProPOI.indicatorCanvasGroup.alpha = num29;
								compassProPOI.indicatorArrowRT.gameObject.SetActive(!flag3);
							}
						}
					}
				}
				IL_0B20:;
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00077944 File Offset: 0x00075B44
		private GameObject GetIndicator()
		{
			return Object.Instantiate<GameObject>(this._onScreenIndicatorPrefab, this.indicatorsRoot, false);
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0006FA32 File Offset: 0x0006DC32
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x00077958 File Offset: 0x00075B58
		[Obsolete("miniMapFollow is obsolete. Please use 'follow'.")]
		public Transform miniMapFollow
		{
			get
			{
				return this._follow;
			}
			set
			{
				this.follow = value;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x00072518 File Offset: 0x00070718
		// (set) Token: 0x060012D3 RID: 4819 RVA: 0x00077961 File Offset: 0x00075B61
		[Obsolete("miniMapZoomState is obsolete. Please use 'miniMapFullScreenState'.")]
		public bool miniMapZoomState
		{
			get
			{
				return this._miniMapFullScreenState;
			}
			set
			{
				this.miniMapFullScreenState = value;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x0007796A File Offset: 0x00075B6A
		public bool currentMiniMapAllowsUserDrag
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapAllowUserDrag;
				}
				return this._miniMapFullScreenAllowUserDrag;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x00077981 File Offset: 0x00075B81
		private float currentMiniMapClampBorder
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapClampBorder;
				}
				return this._miniMapFullScreenClampBorder;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x00077998 File Offset: 0x00075B98
		private bool currentMiniMapIsCircular
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapClampBorderCircular || this._miniMapContents == MiniMapContents.Radar;
				}
				return this._miniMapFullScreenClampBorderCircular || this._miniMapFullScreenContents == MiniMapContents.Radar;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x000779C9 File Offset: 0x00075BC9
		private bool currentMiniMapUsesEvents
		{
			get
			{
				return this._miniMapShowZoomInOutButtons || this._miniMapShowMaximizeButton || this._miniMapIconEvents || this.currentMiniMapAllowsUserDrag;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x000779EB File Offset: 0x00075BEB
		private MiniMapContents currentMiniMapContents
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapContents;
				}
				return this._miniMapFullScreenContents;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x00077A02 File Offset: 0x00075C02
		private float currentMiniMapZoomLevel
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapZoomLevel;
				}
				return this._miniMapFullScreenZoomLevel;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x00077A19 File Offset: 0x00075C19
		private Vector3 currentMiniMapWorldCenter
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapWorldCenter;
				}
				return this._miniMapFullScreenWorldCenter;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00077A30 File Offset: 0x00075C30
		private Vector3 currentMiniMapWorldSize
		{
			get
			{
				if (!this._miniMapFullScreenState)
				{
					return this._miniMapWorldSize;
				}
				return this._miniMapFullScreenWorldSize;
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00077A47 File Offset: 0x00075C47
		private void MiniMapDispose()
		{
			this.MiniMapReleaseRenderTexture();
			if (this.miniMapOverlayMat != null)
			{
				Object.DestroyImmediate(this.miniMapOverlayMat);
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00077A68 File Offset: 0x00075C68
		private void SetupMiniMap(bool force = false)
		{
			if (this._canvas == null)
			{
				return;
			}
			this.ResetDragOffset();
			if (this._miniMapFullScreenState && !force)
			{
				this.MiniMapZoomToggle(true);
				return;
			}
			if (this.miniMapUIRootRT == null)
			{
				Transform transform = base.transform.Find("MiniMap Root");
				if (transform != null)
				{
					this.miniMapUIRootRT = transform.GetComponent<RectTransform>();
				}
			}
			if (this.miniMapUIRootRT != null)
			{
				this.miniMapUIRootRT.gameObject.SetActive(this._showMiniMap);
			}
			this.HideMiniMapIcons();
			if (!this._showMiniMap)
			{
				return;
			}
			if (this.miniMapUIRootRT == null)
			{
				Debug.LogError("Mini Map element not found in the hierarchy and could not be intialized.");
				this._showMiniMap = false;
				return;
			}
			this.lastRadarInfoDistance = 0f;
			Transform transform2 = this.miniMapUIRootRT.Find("MiniMap");
			if (this.miniMapUI == null)
			{
				this.miniMapUI = transform2.GetComponent<RectTransform>();
			}
			if (this.miniMapUI != null)
			{
				MiniMapInteraction miniMapInteraction = this.miniMapUI.GetComponent<MiniMapInteraction>();
				if (miniMapInteraction == null)
				{
					miniMapInteraction = this.miniMapUI.gameObject.AddComponent<MiniMapInteraction>();
				}
				miniMapInteraction.compass = this;
			}
			if (this.miniMapMaskUI == null)
			{
				this.miniMapMaskUI = this.miniMapUI.Find("MiniMapMask");
			}
			if (this.miniMapButtonsPanel == null)
			{
				this.miniMapButtonsPanel = this.miniMapUIRootRT.Find("Buttons");
			}
			if (this.miniMapButtonsPanel != null)
			{
				this.miniMapButtonsPanel.transform.localScale = new Vector3(this._miniMapButtonsScale, this._miniMapButtonsScale, 1f);
				this.ToggleButtonEventHandler("ZoomIn", delegate
				{
					this.MiniMapZoomIn(1f);
					EventSystem.current.SetSelectedGameObject(null);
				}, true, this._miniMapShowZoomInOutButtons);
				this.ToggleButtonEventHandler("ZoomOut", delegate
				{
					this.MiniMapZoomOut(1f);
					EventSystem.current.SetSelectedGameObject(null);
				}, true, this._miniMapShowZoomInOutButtons);
				this.ToggleButtonEventHandler("ToggleFull", delegate
				{
					this.miniMapFullScreenState = !this.miniMapFullScreenState;
					EventSystem.current.SetSelectedGameObject(null);
				}, false, this._miniMapShowMaximizeButton);
			}
			if (this.miniMapCamera == null)
			{
				this.miniMapCamera = base.transform.GetComponentInChildren<Camera>(true);
			}
			if (this.miniMapCamera != null)
			{
				this.miniMapCamera.enabled = false;
				if (CNP2URPCameraSetup.usesURP)
				{
					CNP2URPCameraSetup.SetupURPCamera(this.miniMapCamera, this._miniMapEnableShadows);
				}
				else if (CNP2HDRPCameraSetup.usesHDRP)
				{
					CNP2HDRPCameraSetup.SetupHDRPCamera(this.miniMapCamera, this._miniMapEnableShadows, this._miniMapBackgroundColor);
				}
			}
			if (this._miniMapPositionAndSize == MiniMapPositionAndScaleMode.ControlledByCompassNavigatorPro)
			{
				switch (this._miniMapLocation)
				{
				case MiniMapPosition.TopLeft:
					this.miniMapUIRootRT.anchorMin = new Vector2(0f, 1f);
					this.miniMapUIRootRT.anchorMax = new Vector2(0f, 1f);
					this.miniMapUIRootRT.pivot = new Vector2(0f, 1f);
					break;
				case MiniMapPosition.TopCenter:
					this.miniMapUIRootRT.anchorMin = new Vector2(0.5f, 1f);
					this.miniMapUIRootRT.anchorMax = new Vector2(0.5f, 1f);
					this.miniMapUIRootRT.pivot = new Vector2(0.5f, 1f);
					break;
				case MiniMapPosition.TopRight:
					this.miniMapUIRootRT.anchorMin = new Vector2(1f, 1f);
					this.miniMapUIRootRT.anchorMax = new Vector2(1f, 1f);
					this.miniMapUIRootRT.pivot = new Vector2(1f, 1f);
					break;
				case MiniMapPosition.MiddleLeft:
					this.miniMapUIRootRT.anchorMin = new Vector2(0f, 0.5f);
					this.miniMapUIRootRT.anchorMax = new Vector2(0f, 0.5f);
					this.miniMapUIRootRT.pivot = new Vector2(0f, 0.5f);
					break;
				case MiniMapPosition.MiddleCenter:
					this.miniMapUIRootRT.anchorMin = new Vector2(0.5f, 0.5f);
					this.miniMapUIRootRT.anchorMax = new Vector2(0.5f, 0.5f);
					this.miniMapUIRootRT.pivot = new Vector2(0.5f, 0.5f);
					break;
				case MiniMapPosition.MiddleRight:
					this.miniMapUIRootRT.anchorMin = new Vector2(1f, 0.5f);
					this.miniMapUIRootRT.anchorMax = new Vector2(1f, 0.5f);
					this.miniMapUIRootRT.pivot = new Vector2(1f, 0.5f);
					break;
				case MiniMapPosition.BottomLeft:
					this.miniMapUIRootRT.anchorMin = new Vector2(0f, 0f);
					this.miniMapUIRootRT.anchorMax = new Vector2(0f, 0f);
					this.miniMapUIRootRT.pivot = new Vector2(0f, 0f);
					break;
				case MiniMapPosition.BottomCenter:
					this.miniMapUIRootRT.anchorMin = new Vector2(0.5f, 0f);
					this.miniMapUIRootRT.anchorMax = new Vector2(0.5f, 0f);
					this.miniMapUIRootRT.pivot = new Vector2(0.5f, 0f);
					break;
				case MiniMapPosition.BottomRight:
					this.miniMapUIRootRT.anchorMin = new Vector2(1f, 0f);
					this.miniMapUIRootRT.anchorMax = new Vector2(1f, 0f);
					this.miniMapUIRootRT.pivot = new Vector2(1f, 0f);
					break;
				}
				this.miniMapUIRootRT.anchoredPosition = this._miniMapScreenPositionOffset;
			}
			if (this._miniMapPositionAndSize == MiniMapPositionAndScaleMode.ControlledByCompassNavigatorPro)
			{
				float num = ((this._cameraMain != null) ? ((float)this._cameraMain.pixelHeight * this._miniMapSize) : ((float)Screen.height * this._miniMapSize));
				this.miniMapUIRootRT.sizeDelta = new Vector2(num / this._canvas.scaleFactor, num / this._canvas.scaleFactor);
			}
			if (this.miniMapOverlayMat == null)
			{
				this.miniMapOverlayMat = Object.Instantiate<Material>(Resources.Load<Material>("CNPro/Materials/MiniMapOverlayUnlit"));
			}
			this.miniMapOverlayMat.DisableKeyword("COMPASS_ROTATE_BORDER");
			MiniMapContents currentMiniMapContents = this.currentMiniMapContents;
			Texture2D texture2D;
			Sprite sprite;
			if (currentMiniMapContents == MiniMapContents.Radar)
			{
				if (this._miniMapRadarGraphicsMethod == MiniMapRadarGraphicsMethod.Texture)
				{
					if (this._miniMapFullScreenState)
					{
						texture2D = this._miniMapBorderTextureFullScreenMode;
						sprite = this._miniMapMaskSpriteFullScreenMode;
					}
					else
					{
						texture2D = this._miniMapBorderTexture;
						sprite = this._miniMapMaskSprite;
					}
					this.miniMapOverlayMat.EnableKeyword("COMPASS_ROTATE_BORDER");
				}
				else
				{
					texture2D = null;
					sprite = null;
				}
			}
			else
			{
				MiniMapStyle miniMapStyle = (this._miniMapFullScreenState ? this._miniMapFullScreenStyle : this._miniMapStyle);
				switch (miniMapStyle)
				{
				case MiniMapStyle.TornPaper:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapMask");
					goto IL_088A;
				case MiniMapStyle.SolidBox:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorderSolidBox");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapMaskSolidBox");
					goto IL_088A;
				case MiniMapStyle.SolidCircle:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorderSolidCircle");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapMaskSolidCircle");
					goto IL_088A;
				case (MiniMapStyle)3:
				case (MiniMapStyle)4:
				case (MiniMapStyle)5:
				case (MiniMapStyle)6:
				case (MiniMapStyle)7:
				case (MiniMapStyle)8:
				case (MiniMapStyle)9:
				case (MiniMapStyle)16:
				case (MiniMapStyle)17:
				case (MiniMapStyle)18:
				case (MiniMapStyle)19:
					break;
				case MiniMapStyle.Fantasy1:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_Fantasy1");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_Fantasy1_Mask");
					goto IL_088A;
				case MiniMapStyle.Fantasy2:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_Fantasy2");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_Fantasy2_Mask");
					goto IL_088A;
				case MiniMapStyle.Fantasy3:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_Fantasy3");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_Fantasy3_Mask");
					goto IL_088A;
				case MiniMapStyle.Fantasy4:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_Fantasy4");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_Fantasy4_Mask");
					goto IL_088A;
				case MiniMapStyle.Fantasy5:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_Fantasy5");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_Fantasy5_Mask");
					goto IL_088A;
				case MiniMapStyle.Fantasy6:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_Fantasy6");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_Fantasy6_Mask");
					goto IL_088A;
				case MiniMapStyle.SciFi1:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_SciFi1");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_SciFi1_Mask");
					goto IL_088A;
				case MiniMapStyle.SciFi2:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_SciFi2");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_SciFi2_Mask");
					goto IL_088A;
				case MiniMapStyle.SciFi3:
					texture2D = Resources.Load<Texture2D>("CNPro/Textures/MiniMapBorder_SciFi3");
					sprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapBorder_SciFi3_Mask");
					goto IL_088A;
				default:
					if (miniMapStyle == MiniMapStyle.None)
					{
						texture2D = null;
						sprite = null;
						goto IL_088A;
					}
					break;
				}
				if (this._miniMapFullScreenState)
				{
					texture2D = this._miniMapBorderTextureFullScreenMode;
					sprite = this._miniMapMaskSpriteFullScreenMode;
				}
				else
				{
					texture2D = this._miniMapBorderTexture;
					sprite = this._miniMapMaskSprite;
				}
			}
			IL_088A:
			if (this._miniMapZoomMin < 0.001f)
			{
				this._miniMapZoomMin = 0.001f;
			}
			if (this._miniMapZoomMax < this._miniMapZoomMin)
			{
				this._miniMapZoomMax = this._miniMapZoomMin;
			}
			this._miniMapZoomLevel = Mathf.Clamp(this._miniMapZoomLevel, this._miniMapZoomMin, this._miniMapZoomMax);
			this._miniMapFullScreenZoomLevel = Mathf.Clamp(this._miniMapFullScreenZoomLevel, this._miniMapZoomMin, this._miniMapZoomMax);
			if (this._miniMapCameraMaxAltitude < this._miniMapCameraMinAltitude)
			{
				this._miniMapCameraMaxAltitude = this._miniMapCameraMinAltitude;
			}
			if (this.miniMapUI != null)
			{
				this.miniMapImage = this.miniMapUI.GetComponent<Image>();
				if (this.miniMapImage != null)
				{
					this.miniMapImage.sprite = null;
					this.miniMapImage.material = this.miniMapOverlayMat;
					Material materialForRendering = this.miniMapImage.materialForRendering;
					Texture texture = ((sprite != null) ? sprite.texture : null);
					this.miniMapOverlayMat.SetTexture(ShaderParams.MaskTex, texture);
					this.miniMapOverlayMat.SetTexture(ShaderParams.BorderTex, texture2D);
					materialForRendering.SetTexture(ShaderParams.MaskTex, texture);
					materialForRendering.SetTexture(ShaderParams.BorderTex, texture2D);
					materialForRendering.DisableKeyword("COMPASS_FOG_OF_WAR");
					materialForRendering.DisableKeyword("COMPASS_RADAR");
					if (currentMiniMapContents == MiniMapContents.Radar && this._miniMapRadarGraphicsMethod == MiniMapRadarGraphicsMethod.ProceduralRings)
					{
						materialForRendering.EnableKeyword("COMPASS_RADAR");
						materialForRendering.SetColor(ShaderParams.RingsColor, this._miniMapRadarRingsColor);
					}
					else if (this._miniMapCameraMode == MiniMapCameraMode.Orthographic && this.currentMiniMapUsesFogOfWar)
					{
						materialForRendering.EnableKeyword("COMPASS_FOG_OF_WAR");
					}
					if (this._miniMapKeepStraight)
					{
						materialForRendering.DisableKeyword("COMPASS_ROTATED");
					}
					else
					{
						materialForRendering.EnableKeyword("COMPASS_ROTATED");
					}
					if (this._miniMapShowViewCone && !this._miniMapFullScreenState)
					{
						materialForRendering.SetColor(ShaderParams.ViewConeColor, this._miniMapViewConeColor);
						if (this._miniMapShowViewConeOutline)
						{
							materialForRendering.DisableKeyword("COMPASS_VIEW_CONE");
							materialForRendering.EnableKeyword("COMPASS_VIEW_CONE_OUTLINE");
							materialForRendering.SetColor(ShaderParams.ViewConeOutlineColor, this._miniMapViewConeOutlineColor);
						}
						else
						{
							materialForRendering.DisableKeyword("COMPASS_VIEW_CONE_OUTLINE");
							materialForRendering.EnableKeyword("COMPASS_VIEW_CONE");
						}
						this.lastViewConeCameraAspect = 0f;
					}
					else
					{
						materialForRendering.DisableKeyword("COMPASS_VIEW_CONE_OUTLINE");
						materialForRendering.DisableKeyword("COMPASS_VIEW_CONE");
					}
				}
				if (this.miniMapMaskUI != null)
				{
					this.miniMapMaskImage = this.miniMapMaskUI.GetComponent<Image>();
					if (this.miniMapMaskImage != null)
					{
						this.miniMapMaskImage.sprite = sprite;
					}
				}
			}
			if (this.miniMapCamera != null)
			{
				this.miniMapCamera.allowHDR = false;
				this.miniMapCamera.allowMSAA = false;
				this.miniMapCamera.clearFlags = CameraClearFlags.Color;
				this.miniMapCamera.backgroundColor = this._miniMapBackgroundColor;
				this.miniMapCamera.orthographic = this._miniMapCameraMode == MiniMapCameraMode.Orthographic || currentMiniMapContents == MiniMapContents.Radar;
				this.miniMapCamera.cullingMask = this._miniMapLayerMask & -33;
				this.miniMapCamera.farClipPlane = this._miniMapCameraDepth;
			}
			if (currentMiniMapContents != MiniMapContents.TopDownWorldView)
			{
				this.MiniMapReleaseRenderTexture();
			}
			if (this.playerIcon == null)
			{
				this.playerIcon = this.miniMapUIRootRT.Find("CameraCompass");
			}
			if (this.playerIcon != null)
			{
				this.playerIcon.localEulerAngles = Misc.Vector3zero;
				this.playerIconRT = this.playerIcon.GetComponent<RectTransform>();
				this.playerIconImage = this.playerIcon.GetComponent<Image>();
				this.playerIcon.gameObject.SetActive(this._miniMapShowPlayerIcon);
				if (this.playerIconImage != null)
				{
					if (this._miniMapPlayerIconSprite == null)
					{
						this._miniMapPlayerIconSprite = Resources.Load<Sprite>("CNPro/Sprites/player-icon");
					}
					if (this._miniMapPlayerIconSprite != null)
					{
						this.playerIconImage.sprite = this._miniMapPlayerIconSprite;
					}
					this.playerIconImage.color = this._miniMapPlayerIconColor;
				}
			}
			if (this.miniMapCardinalsRT == null)
			{
				this.miniMapCardinalsRT = this.miniMapUI.Find("Cardinals");
			}
			if (this.miniMapCardinalsRT != null)
			{
				this.miniMapCardinalsImage = this.miniMapCardinalsRT.GetComponent<Image>();
				this.miniMapCardinalsRT.gameObject.SetActive(this._miniMapShowCardinals && !this._miniMapFullScreenState);
				this.miniMapCardinalsRT.localScale = new Vector3(this._miniMapCardinalsSize, this._miniMapCardinalsSize, 1f);
				if (this.miniMapCardinalsImage != null)
				{
					if (this._miniMapCardinalsSprite == null)
					{
						this._miniMapCardinalsSprite = Resources.Load<Sprite>("CNPro/Sprites/MiniMapCardinals");
					}
					if (this._miniMapCardinalsSprite != null)
					{
						this.miniMapCardinalsImage.sprite = this._miniMapCardinalsSprite;
					}
					this.miniMapCardinalsImage.color = this._miniMapCardinalsColor;
				}
			}
			if (this.ringsDistanceText == null)
			{
				Transform transform3 = this.miniMapUI.Find("RingsDistance");
				this.ringsDistanceText = transform3.GetComponent<TextMeshProUGUI>();
			}
			if (this.ringsDistanceText != null)
			{
				this.ringsDistanceText.gameObject.SetActive(this._miniMapRadarInfoDisplay != MiniMapRadarInfoType.Nothing && currentMiniMapContents == MiniMapContents.Radar);
			}
			if (this.miniMapCanvasGroup == null)
			{
				this.miniMapCanvasGroup = this.GetMiniMapCanvasGroup(this.miniMapUIRootRT);
			}
			this.miniMapCanvasGroup.interactable = (this.miniMapCanvasGroup.blocksRaycasts = this.currentMiniMapUsesEvents);
			this.needMiniMapShot = 2;
			this.needUpdateMiniMapIcons = true;
			this.miniMapLastBoundsCheckPos = ((this.miniMapCamera != null) ? this.miniMapCamera.transform.position : Vector3.zero);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x000788A8 File Offset: 0x00076AA8
		public Texture2D GetMiniMapMaterialBorderTexture()
		{
			return this.miniMapImage.materialForRendering.GetTexture(ShaderParams.BorderTex) as Texture2D;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x000788C4 File Offset: 0x00076AC4
		public Texture2D GetMiniMapMaterialMaskTexture()
		{
			return this.miniMapImage.materialForRendering.GetTexture(ShaderParams.MaskTex) as Texture2D;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x000788E0 File Offset: 0x00076AE0
		private void MiniMapReleaseRenderTexture()
		{
			if (this.miniMapTex != null)
			{
				this.miniMapTex.Release();
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000788FC File Offset: 0x00076AFC
		private void MiniMapResizeRenderTexture(int width, int height)
		{
			if (this.miniMapCamera == null)
			{
				return;
			}
			if (this.miniMapTex == null || this.miniMapTex.width != width || this.miniMapTex.height != height)
			{
				if (this.miniMapTex != null)
				{
					this.miniMapTex.Release();
				}
				this.miniMapTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
				this.miniMapTex.filterMode = FilterMode.Bilinear;
			}
			this.miniMapCamera.targetTexture = this.miniMapTex;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00078988 File Offset: 0x00076B88
		private void DisableMiniMap()
		{
			if (this.miniMapUIRootRT != null && this.miniMapUIRootRT.gameObject.activeSelf)
			{
				this.miniMapUIRootRT.gameObject.SetActive(false);
			}
			if (this.miniMapCamera != null)
			{
				this.miniMapCamera.enabled = false;
			}
			if (this.miniMapTex != null)
			{
				this.miniMapTex.Release();
				Object.DestroyImmediate(this.miniMapTex);
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00078A04 File Offset: 0x00076C04
		private void UpdateMiniMap()
		{
			if (!this._showMiniMap || this.miniMapCamera == null || this.cameraMain == null)
			{
				return;
			}
			if (this.needsSetupMiniMap)
			{
				this.needsSetupMiniMap = false;
				this.SetupMiniMap(false);
			}
			MiniMapContents currentMiniMapContents = this.currentMiniMapContents;
			Transform transform = this.miniMapCamera.transform;
			Transform transform2 = ((this._miniMapOrientation == MiniMapOrientation.Follow && this._follow != null) ? this._follow : this._cameraMain.transform);
			bool flag = currentMiniMapContents != MiniMapContents.WorldMappedTexture && !currentMiniMapContents.usesTexture() && this._miniMapCameraMode == MiniMapCameraMode.Perspective && this._miniMapCameraTilt > 0f && this._miniMapCameraSnapshotFrequency != MiniMapCameraSnapshotFrequency.Continuous && this.miniMapLastSnapshotTime > 0f;
			if (!flag)
			{
				if (this._miniMapFullScreenState)
				{
					if (this.miniMapFullScreenFreezeCamera)
					{
						this.currentCamRot = (transform2.rotation = this.miniMapFullScreenFixedCameraRotation);
						this.currentCamPos = (transform2.position = this.miniMapFullScreenFixedCameraPosition);
						if (this._follow != null)
						{
							this._follow.rotation = this.miniMapFullScreenFixedFollowRotation;
							this._follow.position = this.miniMapFullScreenFixedFollowPosition;
						}
					}
					if (this.miniMapFullScreenWorldCenterFollows)
					{
						this.miniMapCenter = this.GetMiniMapFollowPos();
					}
					else
					{
						this.miniMapCenter = this.GetMiniMapFullScreenWorldCenter();
					}
					if (this.miniMapFullScreenClampToWorldEdges)
					{
						this.miniMapCenter = this.ClampMiniMapCenterToWorldSize(this.miniMapCenter);
					}
					float num = Mathf.Max(this.miniMapFullScreenWorldSize.x, this.miniMapFullScreenWorldSize.z) * 0.5f;
					if (this._miniMapCameraMode == MiniMapCameraMode.Orthographic)
					{
						transform.position = new Vector3(this.miniMapCenter.x, this.miniMapCenter.y + this._miniMapCameraHeightVSFollow + 0.1f, this.miniMapCenter.z);
						this.miniMapCamera.orthographicSize = num;
					}
					else
					{
						float num2 = this.miniMapCamera.fieldOfView * 0.017453292f;
						float num3 = num / Mathf.Tan(num2 * 0.5f);
						transform.position = new Vector3(this.miniMapCenter.x, this.miniMapCenter.y + num3, this.miniMapCenter.z);
					}
				}
				else
				{
					this.miniMapCenter = this.GetMiniMapFollowPos();
					if (currentMiniMapContents == MiniMapContents.WorldMappedTexture && this.miniMapClampToWorldEdges)
					{
						this.miniMapCenter = this.ClampMiniMapCenterToWorldSize(this.miniMapCenter);
					}
					if (this._miniMapCameraMode == MiniMapCameraMode.Orthographic)
					{
						transform.position = new Vector3(this.miniMapCenter.x, this.miniMapCenter.y + this._miniMapCameraHeightVSFollow + 0.1f, this.miniMapCenter.z);
						this.miniMapCamera.orthographicSize = this._miniMapCaptureSize * 0.5f;
					}
					else
					{
						float num4 = this._miniMapCameraMinAltitude + (this._miniMapCameraMaxAltitude - this._miniMapCameraMinAltitude) * this._miniMapZoomLevel;
						transform.position = new Vector3(this.miniMapCenter.x, this.miniMapCenter.y + num4, this.miniMapCenter.z);
					}
				}
			}
			else
			{
				if (this._miniMapFullScreenState)
				{
					this.miniMapCenter = (this.miniMapFullScreenWorldCenterFollows ? this.GetMiniMapFollowPos() : this.GetMiniMapFullScreenWorldCenter());
					if (this.miniMapFullScreenClampToWorldEdges)
					{
						this.miniMapCenter = this.ClampMiniMapCenterToWorldSize(this.miniMapCenter);
					}
				}
				else
				{
					this.miniMapCenter = this.GetMiniMapFollowPos();
					if (currentMiniMapContents == MiniMapContents.WorldMappedTexture && this.miniMapClampToWorldEdges)
					{
						this.miniMapCenter = this.ClampMiniMapCenterToWorldSize(this.miniMapCenter);
					}
				}
				transform.position = this.miniMapLastSnapshotLocation;
				if (this.miniMapLastSnapshotRotation != Quaternion.identity)
				{
					transform.rotation = this.miniMapLastSnapshotRotation;
				}
			}
			if (this.miniMapCanvasGroup.alpha != this._miniMapAlpha)
			{
				this.miniMapCanvasGroup.alpha = this._miniMapAlpha;
			}
			if (currentMiniMapContents == MiniMapContents.WorldMappedTexture)
			{
				float num5 = (this._miniMapFullScreenState ? this._miniMapFullScreenWorldSize.x : this._miniMapWorldSize.x);
				this.miniMapCamera.orthographicSize = num5 * 0.5f;
			}
			if (!flag)
			{
				if (this._miniMapKeepStraight || this._miniMapFullScreenState)
				{
					transform.eulerAngles = new Vector3(90f, 0f, 0f);
					if (this.playerIcon != null)
					{
						Vector3 eulerAngles = transform2.eulerAngles;
						eulerAngles.z = -eulerAngles.y;
						eulerAngles.x = (eulerAngles.y = 0f);
						this.playerIcon.localEulerAngles = eulerAngles;
					}
				}
				else
				{
					Vector3 vector = ((this._miniMapOrientation == MiniMapOrientation.Follow && this._follow != null) ? this._follow.forward : transform2.forward);
					vector.y = this.miniMapCenter.y;
					transform.LookAt(this.miniMapCenter, vector);
				}
			}
			else if (!this._miniMapKeepStraight && !this._miniMapFullScreenState && this.miniMapLastSnapshotRotation != Quaternion.identity)
			{
				transform.rotation = this.miniMapLastSnapshotRotation;
			}
			float num6;
			if (this._miniMapCameraTilt > 0f && !this._miniMapKeepStraight && this._miniMapCameraMode == MiniMapCameraMode.Perspective && this._miniMapCameraSnapshotFrequency == MiniMapCameraSnapshotFrequency.Continuous)
			{
				num6 = 0f;
			}
			else
			{
				num6 = (flag ? this.miniMapLastSnapshotRotationY : transform2.eulerAngles.y);
			}
			Material materialForRendering = this.miniMapImage.materialForRendering;
			if (this._miniMapShowViewCone || (this._miniMapShowPlayerIcon && this.playerIconRT != null))
			{
				float currentMiniMapClampBorder = this.currentMiniMapClampBorder;
				Vector2 vector2;
				if (flag)
				{
					vector2 = Misc.Vector2half;
				}
				else
				{
					vector2 = this.GetMiniMapScreenPos(this.followPos);
				}
				if (this.currentMiniMapIsCircular)
				{
					float num7 = vector2.x - 0.5f;
					float num8 = vector2.y - 0.5f;
					float num9 = Mathf.Sqrt(num7 * num7 + num8 * num8) * 2f;
					if (num9 < 0.0001f)
					{
						vector2 = Misc.Vector2half;
					}
					else
					{
						float num10 = Mathf.Min(num9, 1f - currentMiniMapClampBorder);
						vector2.x = 0.5f + num7 * num10 / num9;
						vector2.y = 0.5f + num8 * num10 / num9;
					}
				}
				else
				{
					vector2.x = Mathf.Clamp(vector2.x, currentMiniMapClampBorder, 1f - currentMiniMapClampBorder);
					vector2.y = Mathf.Clamp(vector2.y, currentMiniMapClampBorder, 1f - currentMiniMapClampBorder);
				}
				if (this.playerIconRT != null)
				{
					this.playerIconRT.anchorMin = (this.playerIconRT.anchorMax = vector2);
					this.playerIconRT.localScale = new Vector3(this._miniMapPlayerIconSize, this._miniMapPlayerIconSize, 1f);
				}
				materialForRendering.SetVector(ShaderParams.FollowPos, new Vector4(vector2.x, vector2.y, 0f, 0f));
			}
			if (this.playerIcon != null && !this._miniMapFullScreenState)
			{
				if (!this._miniMapKeepStraight)
				{
					if (this.playerIcon.localEulerAngles != Vector3.zero)
					{
						this.playerIcon.localEulerAngles = Vector3.zero;
					}
				}
				else
				{
					Vector3 eulerAngles2 = transform2.eulerAngles;
					eulerAngles2.z = -eulerAngles2.y;
					eulerAngles2.x = (eulerAngles2.y = 0f);
					this.playerIcon.localEulerAngles = eulerAngles2;
				}
			}
			if (this._miniMapShowCardinals && !this._miniMapFullScreenState && this.miniMapCardinalsRT != null)
			{
				float y = transform2.eulerAngles.y;
				float num11 = (this._miniMapKeepStraight ? 0f : y);
				this.miniMapCardinalsRT.localRotation = Quaternion.Euler(0f, 0f, num11 - this._northDegrees);
			}
			if (currentMiniMapContents == MiniMapContents.WorldMappedTexture)
			{
				materialForRendering.SetTexture(ShaderParams.MiniMapTex, this._miniMapFullScreenState ? this._miniMapFullScreenContentsTexture : this._miniMapContentsTexture);
				float currentMiniMapZoomLevel = this.currentMiniMapZoomLevel;
				Vector3 position = transform.position;
				Vector4 vector3;
				float num12;
				if (this._miniMapFullScreenState)
				{
					vector3 = this._miniMapFullScreenWorldCenter - position;
					num12 = this._miniMapFullScreenWorldSize.x;
				}
				else
				{
					vector3 = this._miniMapWorldCenter - position;
					num12 = this._miniMapWorldSize.x;
				}
				vector3.x /= num12;
				vector3.y = vector3.z / num12;
				vector3.z = currentMiniMapZoomLevel;
				vector3.w = this.miniMapUIRootRT.rect.size.y / this.miniMapUIRootRT.rect.size.x;
				Vector4 vector4 = this._fogOfWarCenter - position;
				vector4.x = vector4.x / num12 / currentMiniMapZoomLevel;
				vector4.y = vector4.z / num12 / currentMiniMapZoomLevel;
				vector4.z = currentMiniMapZoomLevel * num12 / this._fogOfWarSize.x;
				vector4.w = currentMiniMapZoomLevel * num12 / this._fogOfWarSize.z;
				materialForRendering.SetVector(ShaderParams.UVOffset, vector3);
				materialForRendering.SetVector(ShaderParams.UVFogOffset, vector4);
				materialForRendering.SetTexture(ShaderParams.FoWTexture, this.fogOfWarTexture);
				materialForRendering.SetColor(ShaderParams.FoWTintColor, this._fogOfWarColor);
				if (this._miniMapKeepStraight || this._miniMapFullScreenState)
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, 0f);
					materialForRendering.SetFloat(ShaderParams.ConeRotation, -num6 * 0.017453292f);
				}
				else
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, num6 * 0.017453292f);
					materialForRendering.SetFloat(ShaderParams.ConeRotation, 0f);
				}
			}
			else if (currentMiniMapContents.usesTexture())
			{
				if (this.miniMapCamera != null)
				{
					this.miniMapCamera.aspect = 1f;
				}
				materialForRendering.SetTexture(ShaderParams.MiniMapTex, this._miniMapFullScreenState ? this._miniMapFullScreenContentsTexture : this._miniMapContentsTexture);
				materialForRendering.SetVector(ShaderParams.UVOffset, new Vector4(0f, 0f, 1f, 1f));
				if (this._miniMapFullScreenState)
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, 0f);
				}
				else
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, this._miniMapContentsTextureAllowRotation ? (num6 * 0.017453292f) : 0f);
				}
				if (this._miniMapKeepStraight || this._miniMapFullScreenState)
				{
					materialForRendering.SetFloat(ShaderParams.ConeRotation, -num6 * 0.017453292f);
				}
				else
				{
					materialForRendering.SetFloat(ShaderParams.ConeRotation, 0f);
				}
			}
			else
			{
				switch (this._miniMapCameraSnapshotFrequency)
				{
				case MiniMapCameraSnapshotFrequency.Continuous:
					this.needMiniMapShot = 1;
					break;
				case MiniMapCameraSnapshotFrequency.TimeInterval:
					if (Time.time - this.miniMapLastSnapshotTime > this._miniMapSnapshotInterval)
					{
						this.needMiniMapShot = 1;
					}
					break;
				case MiniMapCameraSnapshotFrequency.DistanceTravelled:
				{
					Vector3 vector5 = ((this.miniMapLastSnapshotCenter == Vector3.zero) ? this.miniMapCenter : this.miniMapLastSnapshotCenter);
					Vector3 vector6 = this.miniMapCenter - vector5;
					vector6.y = 0f;
					if (vector6.sqrMagnitude > this._miniMapSnapshotDistance * this._miniMapSnapshotDistance)
					{
						this.needMiniMapShot = 1;
					}
					break;
				}
				}
				if (this._miniMapCameraSnapshotFrequency != MiniMapCameraSnapshotFrequency.Continuous && this.miniMapTex != null && this.miniMapLastSnapshotTime > 0f)
				{
					Vector3 vector7 = transform.position - this.miniMapLastBoundsCheckPos;
					vector7.y = 0f;
					if (vector7.sqrMagnitude >= 25f || this.miniMapLastBoundsCheckPos == Vector3.zero)
					{
						Vector3 vector8 = transform.position - this.miniMapLastSnapshotLocation;
						float miniMapAspectRatio = this.GetMiniMapAspectRatio();
						float currentMiniMapZoomLevel2 = this.currentMiniMapZoomLevel;
						float num13;
						if (this.miniMapCamera.orthographic)
						{
							num13 = this.miniMapCamera.orthographicSize * 2f;
						}
						else
						{
							float num14 = Mathf.Abs(transform.position.y - this.miniMapCenter.y);
							float num15 = this.miniMapCamera.fieldOfView * 0.017453292f;
							num13 = 2f * num14 * Mathf.Tan(num15 * 0.5f);
						}
						float num16 = num13 * 0.5f / currentMiniMapZoomLevel2;
						float num17 = num16 * miniMapAspectRatio;
						float num18;
						if (this.miniMapCamera.orthographic)
						{
							num18 = this._miniMapCaptureSize;
						}
						else
						{
							float num19 = this.miniMapLastSnapshotFoV * 0.017453292f;
							num18 = 2f * this.miniMapLastSnapshotAltitude * Mathf.Tan(num19 * 0.5f);
						}
						float num20 = num18 * 0.5f;
						float num21 = 5f;
						num20 = ((num20 > num21) ? (num20 - num21) : 0f);
						float num22 = (flag ? this.miniMapLastSnapshotRotationY : num6);
						float num23 = ((this._miniMapKeepStraight || this._miniMapFullScreenState) ? 0f : (num22 * 0.017453292f));
						float num24 = Mathf.Cos(num23);
						float num25 = Mathf.Sin(num23);
						float num26 = Mathf.Abs(num17 * num24) + Mathf.Abs(num16 * num25);
						float num27 = Mathf.Abs(num17 * num25) + Mathf.Abs(num16 * num24);
						if (Mathf.Abs(vector8.x) + num26 > num20 || Mathf.Abs(vector8.z) + num27 > num20)
						{
							this.needMiniMapShot = 1;
						}
						this.miniMapLastBoundsCheckPos = transform.position;
					}
				}
				if (this.needMiniMapShot > 0)
				{
					this.needMiniMapShot--;
					if (this.needMiniMapShot <= 0)
					{
						this.needMiniMapShot = 0;
						Quaternion rotation = transform.rotation;
						bool flag2 = this._miniMapCameraTilt > 0f && this._miniMapCameraMode == MiniMapCameraMode.Perspective;
						if (flag2)
						{
							if (this._miniMapCameraSnapshotFrequency != MiniMapCameraSnapshotFrequency.Continuous)
							{
								float num28 = ((this._miniMapKeepStraight || this._miniMapFullScreenState) ? 0f : transform2.eulerAngles.y);
								transform.eulerAngles = new Vector3(90f, num28, 0f);
							}
							transform.Rotate(-this._miniMapCameraTilt, 0f, 0f, Space.Self);
							float num29 = Vector3.Distance(this.miniMapCenter, transform.position);
							transform.position = this.miniMapCenter - transform.forward * num29;
						}
						else
						{
							transform.eulerAngles = new Vector3(90f, 0f, 0f);
						}
						if (this._miniMapFullScreenState)
						{
							this.MiniMapResizeRenderTexture((int)this._miniMapFullScreenResolution, (int)this._miniMapFullScreenResolution);
						}
						else
						{
							this.MiniMapResizeRenderTexture((int)this._miniMapResolution, (int)this._miniMapResolution);
						}
						if (CNP2HDRPCameraSetup.usesHDRP)
						{
							this._canvas.enabled = false;
						}
						this.OnMiniMapBeforeCapture.Invoke();
						Vector3 position2 = transform.position;
						Vector3 vector9 = position2;
						if (this._miniMapCameraSnapshotFrequency == MiniMapCameraSnapshotFrequency.Continuous && this.miniMapCamera.orthographic && this.miniMapTex != null)
						{
							float num30 = this.miniMapCamera.orthographicSize * 2f / (float)this.miniMapTex.width;
							vector9.x = Mathf.Round(vector9.x / num30) * num30;
							vector9.z = Mathf.Round(vector9.z / num30) * num30;
							transform.position = vector9;
						}
						if (!this.miniMapCamera.orthographic)
						{
							float num31 = Mathf.Abs(transform.position.y - this.miniMapCenter.y) + 1f;
							if (this.miniMapCamera.farClipPlane < num31)
							{
								this.miniMapCamera.farClipPlane = num31;
							}
						}
						bool fog = RenderSettings.fog;
						RenderSettings.fog = false;
						if (!this._miniMapEnableShadows && Application.isPlaying)
						{
							ShadowQuality shadows = QualitySettings.shadows;
							QualitySettings.shadows = ShadowQuality.Disable;
							this.miniMapCamera.Render();
							QualitySettings.shadows = shadows;
						}
						else
						{
							this.miniMapCamera.Render();
						}
						RenderSettings.fog = fog;
						this.OnMiniMapAfterCapture.Invoke();
						if (this._miniMapCameraSnapshotFrequency == MiniMapCameraSnapshotFrequency.Continuous && this.miniMapCamera.orthographic && this.miniMapTex != null)
						{
							transform.position = position2;
						}
						if (CNP2HDRPCameraSetup.usesHDRP)
						{
							this._canvas.enabled = true;
						}
						if (!this._miniMapKeepStraight && !flag2)
						{
							transform.rotation = rotation;
						}
						this.miniMapLastSnapshotTime = Time.time;
						this.miniMapLastSnapshotLocation = ((this._miniMapCameraSnapshotFrequency == MiniMapCameraSnapshotFrequency.Continuous && this.miniMapCamera.orthographic && this.miniMapTex != null) ? vector9 : transform.position);
						this.miniMapLastSnapshotCenter = this.miniMapCenter;
						this.miniMapLastSnapshotAltitude = Mathf.Abs(transform.position.y - this.miniMapCenter.y);
						this.miniMapLastSnapshotFoV = this.miniMapCamera.fieldOfView;
						this.miniMapLastSnapshotRotationY = num6;
						this.miniMapLastSnapshotRotation = transform.rotation;
						this.needUpdateMiniMapIcons = true;
					}
				}
				materialForRendering.SetTexture(ShaderParams.MiniMapTex, this.miniMapTex);
				float currentMiniMapZoomLevel3 = this.currentMiniMapZoomLevel;
				Vector3 position3 = transform.position;
				bool flag3 = this._miniMapCameraMode == MiniMapCameraMode.Perspective && this._miniMapCameraTilt > 0f && this._miniMapCameraSnapshotFrequency != MiniMapCameraSnapshotFrequency.Continuous && this.miniMapLastSnapshotTime > 0f;
				Vector4 vector10;
				float num33;
				if (flag3)
				{
					vector10 = Vector4.zero;
					float num32 = this.miniMapLastSnapshotFoV * 0.017453292f;
					num33 = 2f * this.miniMapLastSnapshotAltitude * Mathf.Tan(num32 * 0.5f);
				}
				else
				{
					vector10 = this.miniMapLastSnapshotLocation - position3;
					if (this.miniMapCamera.orthographic)
					{
						num33 = this.miniMapCamera.orthographicSize * 2f;
					}
					else
					{
						float num34 = Mathf.Abs(transform.position.y - this.miniMapCenter.y);
						float num35 = this.miniMapCamera.fieldOfView * 0.017453292f;
						num33 = 2f * num34 * Mathf.Tan(num35 * 0.5f);
					}
				}
				vector10.x /= num33;
				vector10.y = vector10.z / num33;
				vector10.z = currentMiniMapZoomLevel3;
				float miniMapAspectRatio2 = this.GetMiniMapAspectRatio();
				vector10.w = 1f / miniMapAspectRatio2;
				Vector3 vector11 = (flag3 ? this.miniMapLastSnapshotLocation : position3);
				Vector4 vector12 = this._fogOfWarCenter - vector11;
				vector12.x = vector12.x / num33 / currentMiniMapZoomLevel3;
				vector12.y = vector12.z / num33 / currentMiniMapZoomLevel3;
				vector12.y *= miniMapAspectRatio2;
				vector12.z = currentMiniMapZoomLevel3 * num33 / this._fogOfWarSize.x;
				vector12.w = currentMiniMapZoomLevel3 * num33 / this._fogOfWarSize.z;
				materialForRendering.SetVector(ShaderParams.UVOffset, vector10);
				materialForRendering.SetVector(ShaderParams.UVFogOffset, vector12);
				materialForRendering.SetTexture(ShaderParams.FoWTexture, this.fogOfWarTexture);
				materialForRendering.SetColor(ShaderParams.FoWTintColor, this._fogOfWarColor);
				float num36 = ((flag3 && this.miniMapLastSnapshotTime > 0f) ? this.miniMapLastSnapshotRotationY : num6);
				float y2 = transform2.eulerAngles.y;
				if (flag3)
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, 0f);
					float num37 = (this._miniMapKeepStraight ? (-y2 * 0.017453292f) : 0f);
					materialForRendering.SetFloat(ShaderParams.ConeRotation, num37);
				}
				else if (this._miniMapKeepStraight || this._miniMapFullScreenState)
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, 0f);
					materialForRendering.SetFloat(ShaderParams.ConeRotation, -num6 * 0.017453292f);
				}
				else
				{
					materialForRendering.SetFloat(ShaderParams.Rotation, num36 * 0.017453292f);
					materialForRendering.SetFloat(ShaderParams.ConeRotation, 0f);
				}
			}
			if (this._miniMapShowViewCone)
			{
				float aspect = this._cameraMain.aspect;
				float fieldOfView = this._cameraMain.fieldOfView;
				if (this.lastViewConeCameraAspect == 0f || aspect != this.lastViewConeCameraAspect || fieldOfView != this.lastViewConeFoV)
				{
					this.lastViewConeCameraAspect = aspect;
					this.lastViewConeFoV = fieldOfView;
					float num38;
					if (this._miniMapViewConeFoVSource == MiniMapViewConeFovSource.FromCamera)
					{
						num38 = Mathf.Atan(Mathf.Tan(this._cameraMain.fieldOfView * 0.017453292f / 2f) * aspect);
					}
					else
					{
						num38 = this._miniMapViewConeFoV * 0.017453292f * 0.5f;
					}
					Vector2 miniMapScreenPosNoShift = this.GetMiniMapScreenPosNoShift(this.GetMiniMapFollowPos() + new Vector3(0f, 0f, this._miniMapViewConeDistance));
					miniMapScreenPosNoShift.x -= 0.5f;
					miniMapScreenPosNoShift.y -= 0.5f;
					float magnitude = miniMapScreenPosNoShift.magnitude;
					this.viewConeData.x = num38;
					this.viewConeData.y = magnitude * magnitude;
					this.viewConeData.z = this.viewConeData.y / (this._miniMapViewConeFallOff * this._miniMapViewConeFallOff);
					materialForRendering.SetVector(ShaderParams.ViewConeData, this.viewConeData);
				}
			}
			materialForRendering.SetVector(ShaderParams.Effects, new Vector4(this._miniMapBrightness, this._miniMapContrast, this._miniMapLutIntensity, (this._miniMapVignette && this.currentMiniMapContents != MiniMapContents.Radar && !this._miniMapFullScreenState) ? (this._miniMapVignetteColor.a * 48f) : 0f));
			materialForRendering.SetColor(ShaderParams.BackgroundColor, this._miniMapBackgroundColor);
			materialForRendering.SetInt(ShaderParams.BackgroundOpaque, this._miniMapBackgroundOpaque ? 1 : 0);
			materialForRendering.SetColor(ShaderParams.TintColor, this._miniMapTintColor);
			materialForRendering.SetColor(ShaderParams.VignetteColor, this._miniMapVignetteColor);
			if (this._miniMapLutTexture != null && this._miniMapLutIntensity > 0f)
			{
				materialForRendering.SetTexture(ShaderParams.LUTTexture, this._miniMapLutTexture);
				materialForRendering.EnableKeyword("COMPASS_LUT");
			}
			else
			{
				materialForRendering.DisableKeyword("COMPASS_LUT");
			}
			if (currentMiniMapContents == MiniMapContents.Radar && this._miniMapRadarGraphicsMethod == MiniMapRadarGraphicsMethod.ProceduralRings)
			{
				float num39 = ((this._miniMapRadarInfoDisplay == MiniMapRadarInfoType.RadarRange) ? (this._miniMapCaptureSize * 0.5f) : this._miniMapRadarRingsDistance);
				if (num39 != this.lastRadarInfoDistance)
				{
					float num40 = 0f;
					for (int i = 0; i < 10; i++)
					{
						Vector3 vector13 = this.GetMiniMapScreenPosNoShift(this.miniMapCenter + new Vector3(0f, 0f, this._miniMapRadarRingsDistance));
						vector13.x -= 0.5f;
						vector13.y -= 0.5f;
						num40 = Mathf.Sqrt(vector13.x * vector13.x + vector13.y * vector13.y) * 2f;
						if (num40 > 0.1f)
						{
							break;
						}
					}
					if (num39 != this.lastRadarInfoDistance && this._miniMapRadarInfoDisplay != MiniMapRadarInfoType.Nothing && this.ringsDistanceText != null)
					{
						this.lastRadarInfoDistance = num39;
						this.ringsDistanceText.text = ((int)num39).ToString() + "m";
					}
					materialForRendering.SetVector(ShaderParams.RingsData, new Vector4(num40, 10f / this._miniMapRadarRingsWidth, 0f, 0f));
				}
				float num41 = (this._miniMapRadarPulseEnabled ? this._miniMapRadarPulseOpacity : 0f);
				switch (this._miniMapRadarPulseAnimationPreset)
				{
				case MiniMapPulsePreset.Default:
					materialForRendering.SetVector(ShaderParams.RingsPulseData, new Vector4(5f, 50f, 0.1f, num41));
					return;
				case MiniMapPulsePreset.LongSweep:
					materialForRendering.SetVector(ShaderParams.RingsPulseData, new Vector4(4f, 15f, 0.25f, num41));
					return;
				case MiniMapPulsePreset.Scanning:
					materialForRendering.SetVector(ShaderParams.RingsPulseData, new Vector4(30f, 4f, 3f, num41));
					return;
				default:
					materialForRendering.SetVector(ShaderParams.RingsPulseData, new Vector4(this._miniMapRadarPulseSpeed, this._miniMapRadarPulseFallOff, this._miniMapRadarPulseFrequency, num41));
					break;
				}
			}
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0007A1D4 File Offset: 0x000783D4
		private Vector3 ClampMiniMapCenterToWorldSize(Vector3 miniMapCenter)
		{
			float currentMiniMapZoomLevel = this.currentMiniMapZoomLevel;
			Vector3 vector;
			float num;
			if (this._miniMapFullScreenState)
			{
				vector = this._miniMapFullScreenWorldCenter;
				num = this._miniMapFullScreenWorldSize.x * 0.5f;
			}
			else
			{
				vector = this._miniMapWorldCenter;
				num = this._miniMapWorldSize.x * 0.5f;
			}
			float num2 = num * currentMiniMapZoomLevel;
			float num3 = Mathf.Clamp(miniMapCenter.x, vector.x - num + num2, vector.x + num - num2);
			float num4 = Mathf.Clamp(miniMapCenter.z, vector.z - num + num2, vector.z + num - num2);
			this._miniMapFollowOffset.x = this._miniMapFollowOffset.x + (num3 - miniMapCenter.x);
			this._miniMapFollowOffset.z = this._miniMapFollowOffset.z + (num4 - miniMapCenter.z);
			miniMapCenter.x = num3;
			miniMapCenter.z = num4;
			return miniMapCenter;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0007A2B0 File Offset: 0x000784B0
		private void ClampDragOffset()
		{
			float num = Mathf.Sqrt(this._miniMapFollowOffset.x * this._miniMapFollowOffset.x + this._miniMapFollowOffset.z * this._miniMapFollowOffset.z);
			if (this._miniMapFullScreenState)
			{
				if (num > this._miniMapFullScreenDragMaxDistance)
				{
					this._miniMapFollowOffset.y = 0f;
					this._miniMapFollowOffset = this._miniMapFollowOffset.normalized * this._miniMapFullScreenDragMaxDistance;
					return;
				}
			}
			else if (num > this._miniMapDragMaxDistance)
			{
				this._miniMapFollowOffset.y = 0f;
				this._miniMapFollowOffset = this._miniMapFollowOffset.normalized * this._miniMapDragMaxDistance;
			}
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0007A368 File Offset: 0x00078568
		private void UpdateMiniMapIcons()
		{
			if (!this.needUpdateMiniMapIcons)
			{
				return;
			}
			if (this.miniMapCamera == null)
			{
				return;
			}
			this.needUpdateMiniMapIcons = false;
			float time = Time.time;
			int frameCount = Time.frameCount;
			float miniMapAspectRatio = this.GetMiniMapAspectRatio();
			float num = (Application.isPlaying ? (Time.deltaTime * 10f) : 1f);
			Vector3 one = Vector3.one;
			Quaternion quaternion = Quaternion.Inverse(this.currentCamRot);
			MiniMapContents currentMiniMapContents = this.currentMiniMapContents;
			int count = this.activePOIs.Count;
			int i = 0;
			while (i < count)
			{
				CompassProPOI compassProPOI = this.activePOIs[i];
				if (compassProPOI.miniMapType == POIMiniMapType.Any)
				{
					goto IL_00B6;
				}
				if (currentMiniMapContents == MiniMapContents.Radar)
				{
					if (compassProPOI.miniMapType == POIMiniMapType.RadarOnly)
					{
						goto IL_00B6;
					}
				}
				else if (compassProPOI.miniMapType == POIMiniMapType.MiniMapOnly)
				{
					goto IL_00B6;
				}
				IL_07AA:
				i++;
				continue;
				IL_00B6:
				bool flag = true;
				if (compassProPOI.isVisited && compassProPOI.hideWhenVisited)
				{
					flag = false;
				}
				if (!compassProPOI.isActiveAndEnabled || !flag)
				{
					compassProPOI.ToggleMiniMapIconVisibility(false);
					compassProPOI.ToggleMiniMapCircleVisibility(false);
					goto IL_07AA;
				}
				if (frameCount != compassProPOI.viewportPosFrameCount)
				{
					compassProPOI.viewportPosFrameCount = frameCount;
					this.ComputePOIViewportPos(compassProPOI);
				}
				Vector3 position = compassProPOI.transform.position;
				float num2 = ((compassProPOI.miniMapVisibleDistanceOverride > 0f) ? compassProPOI.miniMapVisibleDistanceOverride : this._miniMapVisibleMaxDistance);
				bool flag2 = compassProPOI.distanceToFollow < num2;
				flag = compassProPOI.miniMapVisibility == POIVisibility.AlwaysVisible || (compassProPOI.miniMapVisibility == POIVisibility.WhenInRange && flag2);
				if (flag)
				{
					Vector3 miniMapScreenPos = this.GetMiniMapScreenPos(position, miniMapAspectRatio);
					if (compassProPOI.miniMapIconRT == null)
					{
						GameObject gameObject = Object.Instantiate<GameObject>((compassProPOI.miniMapIconPrefabOverride != null) ? compassProPOI.miniMapIconPrefabOverride : this.miniMapIconPrefab);
						gameObject.name = "MiniMap Icon " + compassProPOI.name;
						gameObject.transform.SetParent(this.miniMapMaskUI.transform, false);
						compassProPOI.miniMapIconRT = gameObject.GetComponent<RectTransform>();
						MiniMapIconElements component = gameObject.GetComponent<MiniMapIconElements>();
						if (component == null)
						{
							Debug.LogError("MiniMap icon prefab missing MiniMapIconElements component.");
							goto IL_07AA;
						}
						compassProPOI.miniMapIconImage = component.iconImage;
						if (compassProPOI.miniMapIconImage != null)
						{
							compassProPOI.miniMapIconImageRT = compassProPOI.miniMapIconImage.GetComponent<RectTransform>();
							if (this._miniMapIconEvents)
							{
								compassProPOI.miniMapIconImage.raycastTarget = true;
								CompassIconEventHandler compassIconEventHandler = gameObject.AddComponent<CompassIconEventHandler>();
								compassIconEventHandler.poi = compassProPOI;
								compassIconEventHandler.compass = this;
							}
						}
						compassProPOI.miniMapCircleRT = component.circle;
						if (compassProPOI.miniMapCircleRT != null)
						{
							compassProPOI.miniMapCircleImage = compassProPOI.miniMapCircleRT.GetComponent<Image>();
							if (compassProPOI.miniMapCircleImage.material == null)
							{
								compassProPOI.miniMapCircleImage.material = Resources.Load<Material>("CNPro/Materials/MiniMapCircle");
							}
						}
					}
					flag = miniMapScreenPos.x >= 0f && miniMapScreenPos.x < 1f && miniMapScreenPos.y >= 0f && miniMapScreenPos.y < 1f;
					float num3 = 1f;
					float num4 = 1f;
					float num5 = miniMapScreenPos.x - 0.5f;
					float num6 = miniMapScreenPos.y - 0.5f;
					float num7 = Mathf.Sqrt(num5 * num5 + num6 * num6) * 2f + 1E-06f;
					if (this.currentMiniMapContents == MiniMapContents.Radar && this._miniMapRadarFadePOIs && !compassProPOI.miniMapClampPosition)
					{
						num4 = 1f - Mathf.Clamp01((num7 - 0.7f) / 0.3f);
					}
					if (compassProPOI.miniMapClampPosition)
					{
						flag = true;
						float currentMiniMapClampBorder = this.currentMiniMapClampBorder;
						bool flag3 = false;
						if (this.currentMiniMapIsCircular)
						{
							float num8 = Mathf.Min(num7, 1f - currentMiniMapClampBorder);
							miniMapScreenPos.x = 0.5f + num5 * num8 / num7;
							miniMapScreenPos.y = 0.5f + num6 * num8 / num7;
							flag3 = num7 > 1f - currentMiniMapClampBorder;
						}
						else
						{
							if (miniMapScreenPos.x < currentMiniMapClampBorder)
							{
								miniMapScreenPos.x = currentMiniMapClampBorder;
								flag3 = true;
							}
							else if (miniMapScreenPos.x > 1f - currentMiniMapClampBorder)
							{
								miniMapScreenPos.x = 1f - currentMiniMapClampBorder;
								flag3 = true;
							}
							if (miniMapScreenPos.y < currentMiniMapClampBorder)
							{
								miniMapScreenPos.y = currentMiniMapClampBorder;
								flag3 = true;
							}
							else if (miniMapScreenPos.y > 1f - currentMiniMapClampBorder)
							{
								miniMapScreenPos.y = 1f - currentMiniMapClampBorder;
								flag3 = true;
							}
						}
						if (flag3)
						{
							num3 = compassProPOI.miniMapClampedScaleMultiplier;
						}
					}
					float num9 = ((compassProPOI.miniMapCircleRadius > 0f) ? compassProPOI.miniMapCircleRadius : compassProPOI.radius);
					if (num9 > 0f && (compassProPOI.miniMapShowCircle || (compassProPOI.circleVisibleTime <= 0f && compassProPOI.miniMapCircleAnimationWhenAppears)))
					{
						flag = true;
						if (compassProPOI.lastCircleRadius != num9 || compassProPOI.lastCircleHeight != this.miniMapUI.rect.height || compassProPOI.lastCircleZoomLevel != this.currentMiniMapZoomLevel)
						{
							compassProPOI.lastCircleRadius = num9;
							compassProPOI.lastCircleHeight = this.miniMapUI.rect.height;
							compassProPOI.lastCircleZoomLevel = this.currentMiniMapZoomLevel;
							this.ComputeCircleScale(compassProPOI, miniMapAspectRatio);
						}
						Material materialForRendering = compassProPOI.miniMapCircleImage.materialForRendering;
						compassProPOI.miniMapCircleImage.color = compassProPOI.miniMapCircleColor;
						materialForRendering.SetColor(ShaderParams.CircleInnerColor, compassProPOI.miniMapCircleInnerColor);
						materialForRendering.SetFloat(ShaderParams.CircleStartRadius, compassProPOI.miniMapCircleStartRadius);
						compassProPOI.ToggleMiniMapCircleVisibility(true);
					}
					else if (!compassProPOI.miniMapCircleAnimationWhenAppears)
					{
						compassProPOI.ToggleMiniMapCircleVisibility(false);
					}
					if (compassProPOI.circleVisibleTime <= 0f)
					{
						compassProPOI.circleVisibleTime = time;
						if (compassProPOI.miniMapCircleAnimationWhenAppears && Application.isPlaying)
						{
							base.StartCoroutine(this.AnimateCircle(compassProPOI, time));
						}
					}
					if (flag)
					{
						compassProPOI.miniMapIconRT.anchorMin = (compassProPOI.miniMapIconRT.anchorMax = miniMapScreenPos);
						if (compassProPOI.isVisited)
						{
							if (compassProPOI.miniMapIconImage.sprite != compassProPOI.iconVisited)
							{
								compassProPOI.miniMapIconImage.sprite = compassProPOI.iconVisited;
							}
						}
						else if (compassProPOI.miniMapIconImage.sprite != compassProPOI.iconNonVisited)
						{
							compassProPOI.miniMapIconImage.sprite = compassProPOI.iconNonVisited;
						}
						Color tintColor = compassProPOI.tintColor;
						tintColor.a *= num4;
						compassProPOI.miniMapIconImage.color = tintColor;
						compassProPOI.miniMapCurrentIconScale = this._miniMapIconSize * compassProPOI.miniMapIconScale * num3;
						Vector3 localScale = compassProPOI.miniMapIconImageRT.localScale;
						if (this.SignificantlyDifferents(compassProPOI.miniMapCurrentIconScale, localScale.x))
						{
							this.needUpdateMiniMapIcons = true;
							float num10 = Mathf.Lerp(localScale.x, compassProPOI.miniMapCurrentIconScale, num);
							one.x = (one.y = num10);
							compassProPOI.miniMapIconImageRT.localScale = one;
						}
						if (compassProPOI.miniMapShowRotation)
						{
							float num11;
							if (this._miniMapKeepStraight)
							{
								num11 = compassProPOI.transform.localEulerAngles.y;
							}
							else
							{
								num11 = (quaternion * compassProPOI.transform.localRotation).eulerAngles.y;
							}
							Vector3 vector = new Vector3(0f, 0f, compassProPOI.miniMapRotationAngleOffset - num11);
							compassProPOI.miniMapIconRT.localEulerAngles = vector;
						}
					}
				}
				if (compassProPOI.miniMapIsVisible && !flag)
				{
					this.OnPOIVisibleInMiniMap.Invoke(compassProPOI);
				}
				else if (flag && !compassProPOI.miniMapIsVisible)
				{
					this.OnPOIHidesInMiniMap.Invoke(compassProPOI);
				}
				compassProPOI.ToggleMiniMapIconVisibility(flag);
				goto IL_07AA;
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0007AB30 File Offset: 0x00078D30
		private bool SignificantlyDifferents(float a, float b)
		{
			float num = a - b;
			return num < -0.001f || num > 0.001f;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0007AB54 File Offset: 0x00078D54
		private void HideMiniMapIcons()
		{
			int count = this.activePOIs.Count;
			for (int i = 0; i < count; i++)
			{
				CompassProPOI compassProPOI = this.activePOIs[i];
				if (!(compassProPOI == null) && (compassProPOI.miniMapType == POIMiniMapType.Any || (compassProPOI.miniMapType == POIMiniMapType.RadarOnly && this._miniMapContents == MiniMapContents.Radar) || (compassProPOI.miniMapType == POIMiniMapType.MiniMapOnly && this._miniMapContents != MiniMapContents.Radar)))
				{
					compassProPOI.ToggleMiniMapIconVisibility(false);
					compassProPOI.ToggleMiniMapCircleVisibility(false);
				}
			}
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0007ABD8 File Offset: 0x00078DD8
		private void ComputeCircleScale(CompassProPOI poi, float miniMapAspectRatio)
		{
			float num = ((poi.miniMapCircleRadius > 0f) ? poi.miniMapCircleRadius : poi.radius);
			Vector3 miniMapScreenPos = this.GetMiniMapScreenPos(this.miniMapCenter + new Vector3(0f, 0f, num), miniMapAspectRatio);
			float num2 = miniMapScreenPos.x - 0.5f;
			float num3 = miniMapScreenPos.y - 0.5f;
			float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3) * 2f;
			poi.circleScale = num4 * this.miniMapUI.rect.height / poi.miniMapCircleRT.rect.height;
			if (poi.miniMapCircleMaterial == null)
			{
				poi.miniMapCircleMaterial = Object.Instantiate<Material>(poi.miniMapCircleImage.material);
			}
			poi.miniMapCircleRT.localScale = new Vector3(poi.circleScale, poi.circleScale, 1f);
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0007ACC5 File Offset: 0x00078EC5
		private IEnumerator AnimateCircle(CompassProPOI poi, float startTime)
		{
			if (poi == null || poi.miniMapCircleRT == null)
			{
				yield break;
			}
			poi.ToggleMiniMapCircleVisibility(true);
			int repetitions = ((this._focusedPOI == poi) ? int.MaxValue : poi.miniMapCircleAnimationRepetitions);
			int i = 0;
			IL_01CF:
			while (i < repetitions)
			{
				Vector3 scale = Misc.Vector3one;
				Color color = poi.miniMapCircleColor;
				while (!(poi == null) && poi.isActiveAndEnabled && !(poi.miniMapCircleRT == null))
				{
					float t = (Time.time - startTime) / this._miniMapIconCircleAnimationDuration;
					if (t < 1f)
					{
						scale.x = (scale.y = t * poi.circleScale);
						poi.miniMapCircleRT.localScale = scale;
						if (!poi.miniMapShowCircle)
						{
							color.a = 1f - t;
						}
						poi.miniMapCircleImage.color = color;
					}
					yield return null;
					if (t >= 1f)
					{
						startTime = Time.time;
						scale = default(Vector3);
						color = default(Color);
						int num = i;
						i = num + 1;
						goto IL_01CF;
					}
				}
				yield break;
			}
			if (poi != null && !poi.miniMapShowCircle)
			{
				poi.ToggleMiniMapCircleVisibility(false);
			}
			yield break;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0007ACE4 File Offset: 0x00078EE4
		private CanvasGroup GetMiniMapCanvasGroup(Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				canvasGroup = transform.gameObject.AddComponent<CanvasGroup>();
				canvasGroup.blocksRaycasts = false;
			}
			return canvasGroup;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0007AD20 File Offset: 0x00078F20
		private void ToggleButtonEventHandler(string buttonName, UnityAction handler, bool continuous, bool isVisible)
		{
			if (this.miniMapButtonsPanel == null)
			{
				return;
			}
			Transform transform = this.miniMapButtonsPanel.Find(buttonName);
			if (transform == null)
			{
				return;
			}
			if (!isVisible)
			{
				transform.gameObject.SetActive(false);
				return;
			}
			transform.gameObject.SetActive(true);
			Button component = transform.GetComponent<Button>();
			if (component == null)
			{
				return;
			}
			if (continuous)
			{
				CompassButtonHandler compassButtonHandler = component.GetComponent<CompassButtonHandler>();
				if (compassButtonHandler == null)
				{
					compassButtonHandler = component.gameObject.AddComponent<CompassButtonHandler>();
				}
				compassButtonHandler.actionHandler = handler;
				return;
			}
			component.onClick.RemoveListener(handler);
			component.onClick.AddListener(handler);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0007ADC0 File Offset: 0x00078FC0
		private void MiniMapZoomToggle(bool state)
		{
			if (this.miniMapUIRootRT == null)
			{
				this.SetupMiniMap(true);
			}
			if (this.cameraMain == null || this.miniMapCamera == null)
			{
				return;
			}
			this._miniMapFullScreenState = state;
			RectTransform rectTransform = this.miniMapUIRootRT;
			if (state)
			{
				this.miniMapRegularZoomLevel = this._miniMapZoomLevel;
				this._miniMapZoomLevel = this._miniMapFullScreenZoomLevel;
				Transform transform = this.cameraMain.transform;
				this.miniMapFullScreenFixedCameraRotation = transform.rotation;
				this.miniMapFullScreenFixedCameraPosition = transform.position;
				if (this._follow != null)
				{
					this.miniMapFullScreenFixedFollowRotation = this._follow.rotation;
					this.miniMapFullScreenFixedFollowPosition = this._follow.position;
				}
				this.miniMapAnchorMin = rectTransform.anchorMin;
				this.miniMapAnchorMax = rectTransform.anchorMax;
				this.miniMapPivot = rectTransform.pivot;
				this.miniMapSizeDelta = rectTransform.sizeDelta;
				this.miniMapCameraAspect = this.miniMapCamera.aspect;
				this.SetupMiniMap(true);
				float num = (1f - this._miniMapFullScreenSize) * 0.5f;
				float num2;
				float num3;
				float num5;
				float num6;
				if (this._miniMapFullScreenPlaceholder != null)
				{
					this._miniMapFullScreenPlaceholder.gameObject.SetActive(false);
					Rect viewportRect = this._miniMapFullScreenPlaceholder.GetViewportRect(this._cameraMain);
					num2 = viewportRect.yMin + num;
					num3 = viewportRect.yMax - num;
					float num4 = viewportRect.width / viewportRect.height;
					num5 = viewportRect.xMin + num * num4;
					num6 = viewportRect.xMax - num * num4;
				}
				else
				{
					num2 = num;
					num3 = 1f - num;
					if (this._miniMapKeepAspectRatio)
					{
						float num7 = (1f - this._miniMapFullScreenSize / this.cameraMain.aspect) * 0.5f;
						num5 = num7;
						num6 = 1f - num7;
					}
					else
					{
						num5 = num2;
						num6 = num3;
					}
				}
				rectTransform.anchorMin = new Vector3(num5, num2);
				rectTransform.anchorMax = new Vector3(num6, num3);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.sizeDelta = new Vector2(0f, 0f);
			}
			else
			{
				this._miniMapZoomLevel = this.miniMapRegularZoomLevel;
				rectTransform.anchorMin = this.miniMapAnchorMin;
				rectTransform.anchorMax = this.miniMapAnchorMax;
				rectTransform.pivot = this.miniMapPivot;
				rectTransform.sizeDelta = this.miniMapSizeDelta;
				this.SetupMiniMap(true);
				this.miniMapCamera.aspect = this.miniMapCameraAspect;
			}
			this.UpdateMiniMap();
			this.UpdateMiniMapIcons();
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0007B058 File Offset: 0x00079258
		public Vector3 GetMiniMapFollowPos()
		{
			return this.followPos + this._miniMapFollowOffset;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0007B06B File Offset: 0x0007926B
		public Vector3 GetMiniMapFullScreenWorldCenter()
		{
			return this._miniMapFullScreenWorldCenter + this._miniMapFollowOffset;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0007B080 File Offset: 0x00079280
		private Vector2 GetMiniMapScreenPos(Vector3 poiPosition)
		{
			float miniMapAspectRatio = this.GetMiniMapAspectRatio();
			return this.GetMiniMapScreenPos(poiPosition, miniMapAspectRatio);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0007B0A4 File Offset: 0x000792A4
		private Vector2 GetMiniMapScreenPosNoShift(Vector3 poiPosition)
		{
			float miniMapAspectRatio = this.GetMiniMapAspectRatio();
			return this.GetMiniMapScreenPosNoShift(poiPosition, miniMapAspectRatio);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0007B0C8 File Offset: 0x000792C8
		private float GetMiniMapAspectRatio()
		{
			Vector2 size = this.miniMapUIRootRT.rect.size;
			if (size.y <= 0f)
			{
				return 1f;
			}
			return size.x / size.y;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0007B10C File Offset: 0x0007930C
		private Vector3 GetMiniMapScreenPos(Vector3 poiPosition, float aspectRatio)
		{
			Vector3 miniMapScreenPosNoShift = this.GetMiniMapScreenPosNoShift(poiPosition, aspectRatio);
			miniMapScreenPosNoShift.x += this._miniMapIconPositionShift.x;
			miniMapScreenPosNoShift.y += this._miniMapIconPositionShift.y;
			return miniMapScreenPosNoShift;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0007B150 File Offset: 0x00079350
		private Vector3 GetMiniMapScreenPosNoShift(Vector3 poiPosition, float aspectRatio)
		{
			float currentMiniMapZoomLevel = this.currentMiniMapZoomLevel;
			Vector3 vector = this.miniMapCamera.WorldToViewportPoint(poiPosition);
			vector.x = (vector.x - 0.5f) / currentMiniMapZoomLevel + 0.5f;
			vector.y = (vector.y - 0.5f) / currentMiniMapZoomLevel + 0.5f;
			vector.y = (vector.y - 0.5f) * aspectRatio + 0.5f;
			return vector;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0007B1C4 File Offset: 0x000793C4
		public Vector3 GetWorldPositionFromPointerEvent(Vector2 position)
		{
			Rect screenRect = this.miniMapUI.GetScreenRect();
			Vector2 vector;
			vector.x = (position.x - screenRect.xMin) / screenRect.width;
			vector.y = (position.y - screenRect.yMin) / screenRect.height;
			return this.GetMiniMapWorldPositionFromUV(vector);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0007B220 File Offset: 0x00079420
		public Vector3 GetMiniMapWorldPositionFromUV(Vector2 uv)
		{
			if (this.miniMapCamera == null)
			{
				return Vector3.zero;
			}
			float currentMiniMapZoomLevel = this.currentMiniMapZoomLevel;
			uv.x = (uv.x - 0.5f) * currentMiniMapZoomLevel + 0.5f;
			uv.y = (uv.y - 0.5f) * currentMiniMapZoomLevel + 0.5f;
			Vector2 size = this.miniMapUIRootRT.rect.size;
			float num = size.y / size.x;
			uv.y = (uv.y - 0.5f) * num + 0.5f;
			float num2 = Mathf.Abs(this.followPos.y - this.miniMapCamera.transform.position.y);
			Vector3 vector = this.miniMapCamera.ViewportToWorldPoint(new Vector3(uv.x, uv.y, num2));
			vector.y = 0f;
			return vector;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0007B310 File Offset: 0x00079510
		private void DestroySafe(Object o)
		{
			Object.Destroy(o);
		}

		// Token: 0x0400124E RID: 4686
		[Tooltip("Camera used for computing the indicators and POI screen positions")]
		[SerializeField]
		private Camera _cameraMain;

		// Token: 0x0400124F RID: 4687
		[Tooltip("The pivot used to compute distances. In a third person setup, the follow could be the root of the player game object which is different than the orbiting camera.")]
		[SerializeField]
		private Transform _follow;

		// Token: 0x04001250 RID: 4688
		[Tooltip("Contents are always updated if camera moves or rotates. If not, this property specifies the interval between POI change checks")]
		[SerializeField]
		private UpdateMode _updateMode;

		// Token: 0x04001251 RID: 4689
		[Tooltip("Frames between compass bar updates.")]
		[SerializeField]
		private int _updateIntervalFrameCount = 60;

		// Token: 0x04001252 RID: 4690
		[Tooltip("Seconds between compass bar updates")]
		[SerializeField]
		private float _updateIntervalTime = 0.2f;

		// Token: 0x04001253 RID: 4691
		[Tooltip("Frames between on/off-screen indicator updates. Increase to reduce CPU load.")]
		[SerializeField]
		[Min(1f)]
		private int _indicatorUpdateIntervalFrames = 2;

		// Token: 0x04001254 RID: 4692
		[Tooltip("Minimum distance delta (meters) required to refresh distance text labels.")]
		[SerializeField]
		[Min(0f)]
		private float _distanceTextUpdateThreshold = 1f;

		// Token: 0x04001255 RID: 4693
		[Tooltip("Only manage POIs that are within the area of interest")]
		[SerializeField]
		private bool _enableAreaOfInterest;

		// Token: 0x04001256 RID: 4694
		[Tooltip("Maximum distance to a POI to be considered as part of the area of interest")]
		[SerializeField]
		private float _areaOfInterestMaxDistance = 50f;

		// Token: 0x04001257 RID: 4695
		[Tooltip("Updates POIs of interest list when player moves this distance")]
		[SerializeField]
		private float _areaOfInterestUpdateDistance = 5f;

		// Token: 0x04001258 RID: 4696
		[Tooltip("Hides UI if no camera is found")]
		[SerializeField]
		private bool _hideIfNoCamera;

		// Token: 0x04001259 RID: 4697
		[Tooltip("Shows the compass bar")]
		[SerializeField]
		private bool _showCompassBar = true;

		// Token: 0x0400125A RID: 4698
		[Tooltip("Compass bar style")]
		[SerializeField]
		private CompassStyle _style = CompassStyle.Celtic_White;

		// Token: 0x0400125B RID: 4699
		[Tooltip("Which transform to use for orientation source")]
		[SerializeField]
		private CompassBarOrientation _compassBarOrientation;

		// Token: 0x0400125C RID: 4700
		[Tooltip("Custom sprite for the compass bar. Check the documentation for configuring a sprite.")]
		[SerializeField]
		private Sprite _compassBackSprite;

		// Token: 0x0400125D RID: 4701
		[Tooltip("If the sprite can scroll horizontally")]
		[SerializeField]
		private bool _compassBackSpriteScrollable;

		// Token: 0x0400125E RID: 4702
		[Tooltip("If the sprite can scroll horizontally")]
		[SerializeField]
		[Range(-1f, 1f)]
		private float _compassBackSpriteScrollOffset;

		// Token: 0x0400125F RID: 4703
		[SerializeField]
		private Color _compassTintColor = Color.white;

		// Token: 0x04001260 RID: 4704
		[Tooltip("The position of the North in degrees (0-360)")]
		[Range(-180f, 180f)]
		[SerializeField]
		private float _northDegrees;

		// Token: 0x04001261 RID: 4705
		[Tooltip("POIs beyond visible distance (meters) will not be shown in the compass bar")]
		[SerializeField]
		private float _visibleMaxDistance = 500f;

		// Token: 0x04001262 RID: 4706
		[Tooltip("POIs nearer than this distance (meters) will not be shown in the compass bar")]
		[SerializeField]
		private float _visibleMinDistance;

		// Token: 0x04001263 RID: 4707
		[Tooltip("Minimum POI distance to display its title. Distance text won't be shown for objects within this distance")]
		[SerializeField]
		private float _titleMinPOIDistance = 10f;

		// Token: 0x04001264 RID: 4708
		[Tooltip("Distance to a POI where the icon will start to grow as player approaches")]
		[SerializeField]
		private float _nearDistance = 75f;

		// Token: 0x04001265 RID: 4709
		[Tooltip("Minimum distance to a POI to be considered as explored/visited")]
		[SerializeField]
		private float _visitedDistance = 25f;

		// Token: 0x04001266 RID: 4710
		[Tooltip("Shows on-screen indicators in the scene during playmode for the POIs (can be enabled/disabled per POI)")]
		[SerializeField]
		private bool _showOnScreenIndicators = true;

		// Token: 0x04001267 RID: 4711
		[SerializeField]
		private GameObject _compassIconPrefab;

		// Token: 0x04001268 RID: 4712
		[Tooltip("Indicator prefab. Used for both on-screen and off-screen modes.")]
		[SerializeField]
		private GameObject _onScreenIndicatorPrefab;

		// Token: 0x04001269 RID: 4713
		[Tooltip("Transparency level of on-screen indicators")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _onScreenIndicatorAlpha = 0.85f;

		// Token: 0x0400126A RID: 4714
		[Tooltip("Scaling applied to indicators shown during playmode")]
		[SerializeField]
		private float _onScreenIndicatorScale = 1f;

		// Token: 0x0400126B RID: 4715
		[Tooltip("Speed for the scale animation of the indicators")]
		[SerializeField]
		private float _onScreenIndicatorScaleSpeed = 10f;

		// Token: 0x0400126C RID: 4716
		[Tooltip("Distance at which the indicator will not be visible")]
		[SerializeField]
		private float _onScreenIndicatorFarDistance = 10000f;

		// Token: 0x0400126D RID: 4717
		[Tooltip("Distance at which the on-screen indicator will start to fade out before reaching max visible distance")]
		[SerializeField]
		private float _onScreenIndicatorFarFadeDistance = 8000f;

		// Token: 0x0400126E RID: 4718
		[Tooltip("Distance at which the on-screen indicator will start to fade when it approaches camera")]
		[SerializeField]
		private float _onScreenIndicatorNearFadeDistance = 10f;

		// Token: 0x0400126F RID: 4719
		[Tooltip("Minimum distance at which the on-screen indicator disappear")]
		[SerializeField]
		private float _onScreenIndicatorNearFadeMin = 1f;

		// Token: 0x04001270 RID: 4720
		[Tooltip("Whether the distance in meters should be shown under the indicator")]
		[SerializeField]
		private bool _onScreenIndicatorShowDistance = true;

		// Token: 0x04001271 RID: 4721
		[Tooltip("The string format for displaying the distance on the indicators. The syntax for this string format corresponds with the available options for ToString(format) method of C#")]
		[SerializeField]
		private string _onScreenIndicatorShowDistanceFormat = "0m";

		// Token: 0x04001272 RID: 4722
		[Tooltip("Whether the title of the POI should also be displayed")]
		[SerializeField]
		private bool _onScreenIndicatorShowTitle;

		// Token: 0x04001273 RID: 4723
		[Tooltip("Show indicators on the edges of screen during playmode for POIs not visible in the screen (can be enabled/disabled per POI)")]
		[SerializeField]
		private bool _showOffScreenIndicators = true;

		// Token: 0x04001274 RID: 4724
		[Tooltip("Scaling applied to offscreen indicators shown during playmode.")]
		[SerializeField]
		private float _offScreenIndicatorScale = 1f;

		// Token: 0x04001275 RID: 4725
		[Tooltip("Margin between the indicator and screen edge")]
		[Range(0f, 0.4f)]
		[SerializeField]
		private float _offScreenIndicatorMargin = 0.05f;

		// Token: 0x04001276 RID: 4726
		[Tooltip("Screen rect where indicators are displayed")]
		[SerializeField]
		private Rect _offScreenIndicatorRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001277 RID: 4727
		[Tooltip("Enable to avoid off-screen icons to overlap.")]
		[SerializeField]
		private bool _offScreenIndicatorAvoidOverlap = true;

		// Token: 0x04001278 RID: 4728
		[Tooltip("Overlap distance")]
		[Range(0f, 0.1f)]
		[SerializeField]
		private float _offScreenIndicatorOverlapDistance = 0.04f;

		// Token: 0x04001279 RID: 4729
		[Tooltip("Disables overlap resolution automatically when active POIs exceed this amount. Prevents O(N^2) cost spikes.")]
		[SerializeField]
		[Min(1f)]
		private int _offScreenIndicatorOverlapMaxPOIs = 24;

		// Token: 0x0400127A RID: 4730
		[Tooltip("Transparency level of offscreen indicators")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _offScreenIndicatorAlpha = 0.85f;

		// Token: 0x0400127B RID: 4731
		[Tooltip("Transparency of the compass bar")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _alpha = 1f;

		// Token: 0x0400127C RID: 4732
		[Tooltip("Hides the compass bar if no POIs are below visible distance")]
		[SerializeField]
		private bool _autoHide;

		// Token: 0x0400127D RID: 4733
		[Tooltip("Duration of alpha changes in seconds")]
		[Range(0f, 8f)]
		[SerializeField]
		private float _fadeDuration = 2f;

		// Token: 0x0400127E RID: 4734
		[Tooltip("Makes the bar always visible (ignores alpha property) while in Edit Mode")]
		[SerializeField]
		private bool _alwaysVisibleInEditMode = true;

		// Token: 0x0400127F RID: 4735
		[Tooltip("Distance from the bottom of the screen in %")]
		[Range(-0.2f, 1.2f)]
		[SerializeField]
		private float _verticalPosition = 0.97f;

		// Token: 0x04001280 RID: 4736
		[Tooltip("Distance from the center of the screen in %")]
		[Range(-0.5f, 0.5f)]
		[SerializeField]
		private float _horizontalPosition;

		// Token: 0x04001281 RID: 4737
		[Tooltip("Bending amount. Set this to zero to disable bending effect")]
		[Range(-1f, 1f)]
		[SerializeField]
		private float _bendAmount;

		// Token: 0x04001282 RID: 4738
		[Tooltip("Width of the compass bar in % of the screen width")]
		[Range(0.05f, 1f)]
		[SerializeField]
		private float _width = 0.65f;

		// Token: 0x04001283 RID: 4739
		[Tooltip("Vertical scale of the compass bar.")]
		[Range(0.05f, 5f)]
		[SerializeField]
		private float _height = 1f;

		// Token: 0x04001284 RID: 4740
		[Tooltip("Enables edge fade out effect")]
		[SerializeField]
		private bool _edgeFadeOut;

		// Token: 0x04001285 RID: 4741
		[Tooltip("If edge fade out affects title and text below compass bar")]
		[SerializeField]
		private bool _edgeFadeOutText = true;

		// Token: 0x04001286 RID: 4742
		[Tooltip("Width of the edge fade out")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _edgeFadeOutWidth = 0.1f;

		// Token: 0x04001287 RID: 4743
		[Tooltip("Start of the edge fade out.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _edgeFadeOutStart;

		// Token: 0x04001288 RID: 4744
		[Tooltip("Width of the end caps of the compass bar. This setting limits the usable horizontal range of the bar in the screen to prevent icons being drawn over the art of the end caps of the bar")]
		[Range(0f, 100f)]
		[SerializeField]
		private float _endCapsWidth = 54f;

		// Token: 0x04001289 RID: 4745
		[Tooltip("Whether N, W, S, E should be visible in the compass bar")]
		[SerializeField]
		private bool _showCardinalPoints = true;

		// Token: 0x0400128A RID: 4746
		[Tooltip("Whether NW, NE, SW, SE should be visible in the compass bar")]
		[SerializeField]
		private bool _showOrdinalPoints = true;

		// Token: 0x0400128B RID: 4747
		[Range(0.1f, 3f)]
		[SerializeField]
		private float _cardinalScale = 1f;

		// Token: 0x0400128C RID: 4748
		[Range(0.1f, 3f)]
		[SerializeField]
		private float _ordinalScale = 1f;

		// Token: 0x0400128D RID: 4749
		[Tooltip("Optional vertical displacement for both cardinal and ordinal points")]
		[SerializeField]
		private float _cardinalPointsVerticalOffset;

		// Token: 0x0400128E RID: 4750
		[Tooltip("Enable vertical interval marks in the compass bar")]
		[SerializeField]
		private bool _showHalfWinds = true;

		// Token: 0x0400128F RID: 4751
		[SerializeField]
		[Range(0.01f, 0.5f)]
		private float _halfWindsHeight = 0.125f;

		// Token: 0x04001290 RID: 4752
		[SerializeField]
		[Range(0.01f, 2f)]
		private float _halfWindsWidth = 0.2f;

		// Token: 0x04001291 RID: 4753
		[SerializeField]
		[Range(1f, 45f)]
		private float _halfWindsInterval = 5f;

		// Token: 0x04001292 RID: 4754
		[SerializeField]
		private Color _halfWindsTintColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04001293 RID: 4755
		[Tooltip("The distance from the center of the compass bar where a POI's label is visible")]
		[Range(0.001f, 0.2f)]
		[SerializeField]
		private float _labelHotZone = 0.015f;

		// Token: 0x04001294 RID: 4756
		[SerializeField]
		private float _maxIconSize = 1.15f;

		// Token: 0x04001295 RID: 4757
		[SerializeField]
		private float _minIconSize = 0.5f;

		// Token: 0x04001296 RID: 4758
		[Tooltip("Duration for the scale animation when the POI appears on the compass bar")]
		[Range(0f, 5f)]
		[SerializeField]
		private float _scaleInDuration = 0.3f;

		// Token: 0x04001297 RID: 4759
		[SerializeField]
		private CompassProPOI _focusedPOI;

		// Token: 0x04001298 RID: 4760
		[Tooltip("How POIs positions are mapped to the bar. 1) Limited To Bar Width = the bar width determines the view angle, 2) Camera Frustum = the entire camera frustum is mapped to the bar width, 3) Full 180 degrees = all POIs in front of the camera will appear in the compass bar. 4) Full 360 degrees = all POIs are visible in the compass bar")]
		[SerializeField]
		private WorldMappingMode _worldMappingMode = WorldMappingMode.CameraFrustum;

		// Token: 0x04001299 RID: 4761
		[Tooltip("Vertical offset in pixels for the text with respect to the compass bar.")]
		[Range(-200f, 200f)]
		[SerializeField]
		private float _textVerticalPosition = -30f;

		// Token: 0x0400129A RID: 4762
		[Tooltip("Scaling applied to the text")]
		[Range(0.02f, 3f)]
		[SerializeField]
		private float _textScale = 0.2f;

		// Token: 0x0400129B RID: 4763
		[Tooltip("Controls the spacing between each letter in the reveal text.")]
		[Range(0.02f, 3f)]
		[SerializeField]
		private float _textLetterSpacing = 1f;

		// Token: 0x0400129C RID: 4764
		[Tooltip("Show a revealing text effect when discovering POIs for the first time")]
		[SerializeField]
		private bool _textRevealEnabled = true;

		// Token: 0x0400129D RID: 4765
		[Tooltip("Text reveal duration in seconds")]
		[Range(0f, 3f)]
		[SerializeField]
		private float _textRevealDuration = 0.5f;

		// Token: 0x0400129E RID: 4766
		[Tooltip("Delay in appearance of each letter during a text reveal")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _textRevealLetterDelay = 0.05f;

		// Token: 0x0400129F RID: 4767
		[Tooltip("Text duration in screen")]
		[Range(0f, 20f)]
		[SerializeField]
		private float _textDuration = 5f;

		// Token: 0x040012A0 RID: 4768
		[Tooltip("Duration of the text fade out.")]
		[Range(0f, 10f)]
		[SerializeField]
		private float _textFadeOutDuration = 2f;

		// Token: 0x040012A1 RID: 4769
		[Tooltip("Enable or disable text shadow")]
		[SerializeField]
		private bool _textShadowEnabled = true;

		// Token: 0x040012A2 RID: 4770
		[SerializeField]
		private Font _textFont;

		// Token: 0x040012A3 RID: 4771
		[Tooltip("Vertical offset in pixels for the title with respect to the compass bar")]
		[Range(-200f, 200f)]
		[SerializeField]
		private float _titleVerticalPosition = 18f;

		// Token: 0x040012A4 RID: 4772
		[Tooltip("Scaling applied to the title")]
		[Range(0.02f, 3f)]
		[SerializeField]
		private float _titleScale = 0.1f;

		// Token: 0x040012A5 RID: 4773
		[SerializeField]
		private Font _titleFont;

		// Token: 0x040012A6 RID: 4774
		[Tooltip("Font used for the title when bending is enabled")]
		[SerializeField]
		private bool _titleShadowEnabled = true;

		// Token: 0x040012A7 RID: 4775
		[Tooltip("Font used for the title when bending is disabled")]
		[SerializeField]
		private TMP_FontAsset _titleFontTMP;

		// Token: 0x040012A8 RID: 4776
		[Tooltip("Whether the distance in meters should be shown in the title")]
		[SerializeField]
		private bool _titleShowDistance;

		// Token: 0x040012A9 RID: 4777
		[Tooltip("The string format for displaying the distance in the title. The syntax for this string format corresponds with the available options for ToString(format) method of C#")]
		[SerializeField]
		private string _titleShowDistanceFormat = "0.0 m";

		// Token: 0x040012AA RID: 4778
		[Tooltip("Whether 3D distance should be computed instead of planar X/Z distance")]
		[SerializeField]
		private bool _use3Ddistance;

		// Token: 0x040012AB RID: 4779
		[Tooltip("Minimum difference in altitude from camera to show 'above' or 'below'")]
		[Range(1f, 50f)]
		[SerializeField]
		private float _sameAltitudeThreshold = 3f;

		// Token: 0x040012AC RID: 4780
		[Tooltip("Whether the distance in meters should be shown in the POI indicator")]
		[SerializeField]
		private bool _showDistance = true;

		// Token: 0x040012AD RID: 4781
		[Tooltip("Height of the distance text in the POI indicator")]
		[SerializeField]
		private float _showDistanceHeight = 45f;

		// Token: 0x040012AE RID: 4782
		[Tooltip("The string format for displaying the distance under the icons on the compass bar. The syntax for this string format corresponds with the available options for ToString(format) method of C#")]
		[SerializeField]
		private string _showDistanceFormat = "0m";

		// Token: 0x040012AF RID: 4783
		[Tooltip("Default audio clip to be played when a POI is visited for the first time. Note that you can specify a different audio clip in the POI script itself")]
		[SerializeField]
		private AudioClip _visitedDefaultAudioClip;

		// Token: 0x040012B0 RID: 4784
		[Tooltip("Default audio clip to be played when a POI beacon is shown. Note that you can specify a different audio clip in the POI script itself")]
		[SerializeField]
		private AudioClip _beaconDefaultAudioClip;

		// Token: 0x040012B1 RID: 4785
		[Tooltip("Default audio clip to play for the heartbeat effect. This effect is enabled on each POI and will play a custom sound with variable speed depending on distance")]
		[SerializeField]
		private AudioClip _heartbeatDefaultAudioClip;

		// Token: 0x040012B2 RID: 4786
		[Tooltip("Preserve compass bar between scene changes.")]
		[SerializeField]
		private bool _dontDestroyOnLoad;

		// Token: 0x040012B3 RID: 4787
		[NonSerialized]
		public CompassProPOI nearestPOI;

		// Token: 0x040012B4 RID: 4788
		[Tooltip("Whether the distance in meters should be shown for off-screen indicators")]
		[SerializeField]
		private bool _offScreenIndicatorShowDistance;

		// Token: 0x040012B5 RID: 4789
		[Tooltip("The string format for displaying the distance for off-screen indicators. The syntax for this string format corresponds with the available options for ToString(format) method of C#")]
		[SerializeField]
		private string _offScreenIndicatorShowDistanceFormat = "0m";

		// Token: 0x040012B6 RID: 4790
		public UnityEvent<CompassProPOI> OnPOIRegister;

		// Token: 0x040012B7 RID: 4791
		public UnityEvent<CompassProPOI> OnPOIUnregister;

		// Token: 0x040012B8 RID: 4792
		public UnityEvent<CompassProPOI> OnPOIVisited;

		// Token: 0x040012B9 RID: 4793
		public UnityEvent<CompassProPOI> OnPOIEnterCircle;

		// Token: 0x040012BA RID: 4794
		public UnityEvent<CompassProPOI> OnPOIExitCircle;

		// Token: 0x040012BB RID: 4795
		public UnityEvent<CompassProPOI> OnPOIVisible;

		// Token: 0x040012BC RID: 4796
		public UnityEvent<CompassProPOI> OnPOIHide;

		// Token: 0x040012BD RID: 4797
		public UnityEvent<CompassProPOI> OnPOIMiniMapIconMouseEnter;

		// Token: 0x040012BE RID: 4798
		public UnityEvent<CompassProPOI> OnPOIMiniMapIconMouseExit;

		// Token: 0x040012BF RID: 4799
		public UnityEvent<CompassProPOI, int> OnPOIMiniMapIconMouseDown;

		// Token: 0x040012C0 RID: 4800
		public UnityEvent<CompassProPOI, int> OnPOIMiniMapIconMouseUp;

		// Token: 0x040012C1 RID: 4801
		public UnityEvent<CompassProPOI, int> OnPOIMiniMapIconMouseClick;

		// Token: 0x040012C2 RID: 4802
		public UnityEvent<CompassProPOI> OnPOIVisibleInMiniMap;

		// Token: 0x040012C3 RID: 4803
		public UnityEvent<CompassProPOI> OnPOIHidesInMiniMap;

		// Token: 0x040012C4 RID: 4804
		public UnityEvent<bool> OnMiniMapChangeFullScreenState;

		// Token: 0x040012C5 RID: 4805
		public UnityEvent<Vector3, int> OnMiniMapMouseClick;

		// Token: 0x040012C6 RID: 4806
		public UnityEvent<Vector2> OnMiniMapMouseEnter;

		// Token: 0x040012C7 RID: 4807
		public UnityEvent<Vector2> OnMiniMapMouseExit;

		// Token: 0x040012C8 RID: 4808
		public UnityEvent OnMiniMapBeforeCapture;

		// Token: 0x040012C9 RID: 4809
		public UnityEvent OnMiniMapAfterCapture;

		// Token: 0x040012CA RID: 4810
		public UnityEvent<CompassProPOI> OnPOIOnScreen;

		// Token: 0x040012CB RID: 4811
		public UnityEvent<CompassProPOI> OnPOIOffScreen;

		// Token: 0x040012CC RID: 4812
		public UnityEvent<ScanEffect, CompassProPOI, Transform> OnScanHit;

		// Token: 0x040012CD RID: 4813
		public UnityEvent<ScanEffect> OnScanEnd;

		// Token: 0x040012CE RID: 4814
		[Tooltip("Enables fog of war system")]
		[SerializeField]
		private bool _fogOfWarEnabled;

		// Token: 0x040012CF RID: 4815
		[Tooltip("Center of the fog of war layer (only X/Z coordinates are used)")]
		[SerializeField]
		private Vector3 _fogOfWarCenter;

		// Token: 0x040012D0 RID: 4816
		[Tooltip("Size of the fog of war layer (only X/Z coordinates are used)")]
		[SerializeField]
		private Vector3 _fogOfWarSize = new Vector3(1024f, 0f, 1024f);

		// Token: 0x040012D1 RID: 4817
		[Tooltip("Resolution for the fog of war texture effect")]
		[SerializeField]
		[Range(32f, 2048f)]
		private int _fogOfWarTextureSize = 256;

		// Token: 0x040012D2 RID: 4818
		[SerializeField]
		private Color _fogOfWarColor = new Color(0.18431373f, 0.18431373f, 0.18431373f);

		// Token: 0x040012D3 RID: 4819
		[Tooltip("Clears fog automatically as player crosses it")]
		[SerializeField]
		private bool _fogOfWarAutoClear;

		// Token: 0x040012D4 RID: 4820
		[SerializeField]
		private float _fogOfWarAutoClearRadius = 20f;

		// Token: 0x040012D5 RID: 4821
		[Tooltip("Default alpha value of fog of war in the scene")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _fogOfWarDefaultAlpha = 1f;

		// Token: 0x040012D6 RID: 4822
		[Tooltip("Shows the minimap")]
		[SerializeField]
		private bool _showMiniMap;

		// Token: 0x040012D7 RID: 4823
		[SerializeField]
		private MiniMapPositionAndScaleMode _miniMapPositionAndSize;

		// Token: 0x040012D8 RID: 4824
		[SerializeField]
		private MiniMapPosition _miniMapLocation = MiniMapPosition.BottomRight;

		// Token: 0x040012D9 RID: 4825
		[Tooltip("POIs beyond visible distance (meters) will not be shown in the compass bar")]
		[SerializeField]
		private float _miniMapVisibleMaxDistance = 10000f;

		// Token: 0x040012DA RID: 4826
		[SerializeField]
		private Vector2 _miniMapScreenPositionOffset = new Vector2(-5f, 5f);

		// Token: 0x040012DB RID: 4827
		[Tooltip("Keeps the mini-map oriented to North")]
		[SerializeField]
		private bool _miniMapKeepStraight;

		// Token: 0x040012DC RID: 4828
		[Tooltip("Orientation of the mini-map")]
		[SerializeField]
		private MiniMapOrientation _miniMapOrientation;

		// Token: 0x040012DD RID: 4829
		[Tooltip("Rotation of the mini-map camera")]
		[SerializeField]
		[Range(0f, 90f)]
		private float _miniMapCameraTilt;

		// Token: 0x040012DE RID: 4830
		[Tooltip("Screen size of mini-map in % of screen height")]
		[SerializeField]
		private float _miniMapSize = 0.35f;

		// Token: 0x040012DF RID: 4831
		private Vector3 _miniMapFollowOffset;

		// Token: 0x040012E0 RID: 4832
		[Tooltip("Mask for the border of the mini map")]
		[SerializeField]
		private Sprite _miniMapMaskSprite;

		// Token: 0x040012E1 RID: 4833
		[Tooltip("Texture for the border of the mini map")]
		[SerializeField]
		private Texture2D _miniMapBorderTexture;

		// Token: 0x040012E2 RID: 4834
		[Tooltip("Mini-map style")]
		[SerializeField]
		private MiniMapStyle _miniMapStyle;

		// Token: 0x040012E3 RID: 4835
		[SerializeField]
		private Color _miniMapBackgroundColor = Color.black;

		// Token: 0x040012E4 RID: 4836
		[SerializeField]
		private bool _miniMapBackgroundOpaque;

		// Token: 0x040012E5 RID: 4837
		[Tooltip("What to show when minimap is non full screen mode")]
		[SerializeField]
		private MiniMapContents _miniMapContents;

		// Token: 0x040012E6 RID: 4838
		[Tooltip("The texture to be used as background for the minimap in non full screen mode")]
		[SerializeField]
		private Texture _miniMapContentsTexture;

		// Token: 0x040012E7 RID: 4839
		[Tooltip("Allows rotation of the UI texture")]
		[SerializeField]
		private bool _miniMapContentsTextureAllowRotation;

		// Token: 0x040012E8 RID: 4840
		[Tooltip("Mask for the border of the mini map in full screen mode")]
		[SerializeField]
		private Sprite _miniMapMaskSpriteFullScreenMode;

		// Token: 0x040012E9 RID: 4841
		[Tooltip("Texture for the border of the mini map in full screen mode")]
		[SerializeField]
		private Texture2D _miniMapBorderTextureFullScreenMode;

		// Token: 0x040012EA RID: 4842
		[Tooltip("Mini-map style in full screen mode")]
		[SerializeField]
		private MiniMapStyle _miniMapFullScreenStyle = MiniMapStyle.SolidBox;

		// Token: 0x040012EB RID: 4843
		[Tooltip("Size of the mini-map render texture in non-full screen mode")]
		[SerializeField]
		private MiniMapResolution _miniMapResolution = MiniMapResolution._512;

		// Token: 0x040012EC RID: 4844
		[Tooltip("Size of the render texture in full screen mode")]
		[SerializeField]
		private MiniMapResolution _miniMapFullScreenResolution = MiniMapResolution._1024;

		// Token: 0x040012ED RID: 4845
		[Tooltip("The zoom level used for the mini-map in full screen mode")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _miniMapFullScreenZoomLevel = 1f;

		// Token: 0x040012EE RID: 4846
		[Tooltip("Optional UI element which serves as placeholder for exact positioning of the mini-map in fullscreen mode")]
		[SerializeField]
		private RectTransform _miniMapFullScreenPlaceholder;

		// Token: 0x040012EF RID: 4847
		[Tooltip("The distance of clamped icons to the edge of the mini-map in full screen mode")]
		[SerializeField]
		private float _miniMapFullScreenClampBorder = 0.02f;

		// Token: 0x040012F0 RID: 4848
		[Tooltip("Enable this option if the minimap uses a circular shape in full screen mode")]
		[SerializeField]
		private bool _miniMapFullScreenClampBorderCircular;

		// Token: 0x040012F1 RID: 4849
		[Tooltip("Percentage of screen size if full-screen mode. Image resolution will increase according to screen resolution")]
		[Range(0.5f, 1f)]
		[SerializeField]
		private float _miniMapFullScreenSize = 0.9f;

		// Token: 0x040012F2 RID: 4850
		[Tooltip("What to show when minimap is in full screen mode")]
		[SerializeField]
		private MiniMapContents _miniMapFullScreenContents;

		// Token: 0x040012F3 RID: 4851
		[Tooltip("The texture to be used as background for the minimap in full screen mode.")]
		[SerializeField]
		private Texture _miniMapFullScreenContentsTexture;

		// Token: 0x040012F4 RID: 4852
		[Tooltip("Keep aspect ration in full screen mode")]
		[SerializeField]
		private bool _miniMapKeepAspectRatio = true;

		// Token: 0x040012F5 RID: 4853
		[Tooltip("Allow user to drag the map around")]
		[SerializeField]
		private bool _miniMapAllowUserDrag;

		// Token: 0x040012F6 RID: 4854
		[Tooltip("Allow user to drag the map around in full screen mode")]
		[SerializeField]
		private bool _miniMapFullScreenAllowUserDrag;

		// Token: 0x040012F7 RID: 4855
		[Tooltip("Reset drag offset when user ends dragging")]
		[SerializeField]
		private bool _miniMapAutoResetDrag;

		// Token: 0x040012F8 RID: 4856
		[Tooltip("Reset drag offset when user ends dragging in full screen mode")]
		[SerializeField]
		private bool _miniMapFullScreenAutoResetDrag;

		// Token: 0x040012F9 RID: 4857
		[SerializeField]
		[Tooltip("Maximum allowed drag distance")]
		private float _miniMapDragMaxDistance = 1000f;

		// Token: 0x040012FA RID: 4858
		[SerializeField]
		[Tooltip("Maximum allowed drag distance in full screen mode")]
		private float _miniMapFullScreenDragMaxDistance = 1000f;

		// Token: 0x040012FB RID: 4859
		[Tooltip("Orthographic or perspective mode for the mini-map camera")]
		[SerializeField]
		private MiniMapCameraMode _miniMapCameraMode = MiniMapCameraMode.Orthographic;

		// Token: 0x040012FC RID: 4860
		[Tooltip("Frequency of camera capture")]
		[SerializeField]
		private MiniMapCameraSnapshotFrequency _miniMapCameraSnapshotFrequency = MiniMapCameraSnapshotFrequency.DistanceTravelled;

		// Token: 0x040012FD RID: 4861
		[Tooltip("The orthographic size of the mini-map camera")]
		[SerializeField]
		private float _miniMapCaptureSize = 256f;

		// Token: 0x040012FE RID: 4862
		[SerializeField]
		private float _miniMapSnapshotInterval = 10f;

		// Token: 0x040012FF RID: 4863
		[Tooltip("Distance in meters")]
		[SerializeField]
		private float _miniMapSnapshotDistance = 10f;

		// Token: 0x04001300 RID: 4864
		[Tooltip("Contrast of the mini-map image")]
		[Range(0f, 2f)]
		[SerializeField]
		private float _miniMapContrast = 1.02f;

		// Token: 0x04001301 RID: 4865
		[Tooltip("Brightness of the mini-map image")]
		[Range(0f, 2f)]
		[SerializeField]
		private float _miniMapBrightness = 1.05f;

		// Token: 0x04001302 RID: 4866
		[Tooltip("Tint color for the mini-map. Alpha controls the intensity.")]
		[SerializeField]
		private Color _miniMapTintColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04001303 RID: 4867
		[Tooltip("Enable to render shadows in mini-map")]
		[SerializeField]
		private bool _miniMapEnableShadows;

		// Token: 0x04001304 RID: 4868
		[SerializeField]
		[Range(0f, 1f)]
		private float _miniMapZoomMin = 0.01f;

		// Token: 0x04001305 RID: 4869
		[SerializeField]
		[Range(0f, 1f)]
		private float _miniMapZoomMax = 1f;

		// Token: 0x04001306 RID: 4870
		[SerializeField]
		private GameObject _miniMapIconPrefab;

		// Token: 0x04001307 RID: 4871
		[Tooltip("Optional displacement for the icons in the mini-map")]
		[SerializeField]
		private Vector2 _miniMapIconPositionShift;

		// Token: 0x04001308 RID: 4872
		[Tooltip("The current zoom for the mini-map based on the minimum / maximum ranges")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _miniMapZoomLevel = 0.5f;

		// Token: 0x04001309 RID: 4873
		[Tooltip("The minimum altitude of the mini-map camera respect with the follow target")]
		[SerializeField]
		private float _miniMapCameraMinAltitude = 10f;

		// Token: 0x0400130A RID: 4874
		[Tooltip("The maximum altitude of the mini-map camera respect with the follow target")]
		[SerializeField]
		private float _miniMapCameraMaxAltitude = 100f;

		// Token: 0x0400130B RID: 4875
		[Tooltip("The altitude of the mini-map camera relative to the main camera or followed gameobject")]
		[SerializeField]
		private float _miniMapCameraHeightVSFollow = 200f;

		// Token: 0x0400130C RID: 4876
		[Tooltip("How far will capture the mini-map camera from the top-down position (this is the far clip plane of the mini-map camera).")]
		[SerializeField]
		private float _miniMapCameraDepth = 1000f;

		// Token: 0x0400130D RID: 4877
		[Tooltip("Which objects will be visible in the mini-map")]
		[SerializeField]
		private LayerMask _miniMapLayerMask = -1;

		// Token: 0x0400130E RID: 4878
		[Tooltip("The size for the icons on the mini-map")]
		[SerializeField]
		private float _miniMapIconSize = 0.5f;

		// Token: 0x0400130F RID: 4879
		[Tooltip("Enables player/compass icon on the mini-map")]
		[SerializeField]
		private bool _miniMapShowPlayerIcon = true;

		// Token: 0x04001310 RID: 4880
		[Tooltip("The size for the player icon on the mini-map")]
		[SerializeField]
		private float _miniMapPlayerIconSize = 1f;

		// Token: 0x04001311 RID: 4881
		[Tooltip("The sprite for the player icon")]
		[SerializeField]
		private Sprite _miniMapPlayerIconSprite;

		// Token: 0x04001312 RID: 4882
		[Tooltip("The color for the player icon")]
		[SerializeField]
		private Color _miniMapPlayerIconColor = Color.white;

		// Token: 0x04001313 RID: 4883
		[Tooltip("Enables North icon on the mini-map")]
		[SerializeField]
		private bool _miniMapShowCardinals = true;

		// Token: 0x04001314 RID: 4884
		[Tooltip("The size for the North icon on the mini-map")]
		[SerializeField]
		private float _miniMapCardinalsSize = 1f;

		// Token: 0x04001315 RID: 4885
		[Tooltip("The sprite for the mini-map cardinals")]
		[SerializeField]
		private Sprite _miniMapCardinalsSprite;

		// Token: 0x04001316 RID: 4886
		[Tooltip("The tint color for the mini-map cardinals")]
		[SerializeField]
		private Color _miniMapCardinalsColor = Color.white;

		// Token: 0x04001317 RID: 4887
		[Tooltip("Enables view cone on the mini-map")]
		[SerializeField]
		private bool _miniMapShowViewCone = true;

		// Token: 0x04001318 RID: 4888
		[Tooltip("Color of the view cone")]
		[SerializeField]
		private Color _miniMapViewConeColor = new Color(1f, 1f, 1f, 0.25f);

		// Token: 0x04001319 RID: 4889
		[SerializeField]
		private MiniMapViewConeFovSource _miniMapViewConeFoVSource;

		// Token: 0x0400131A RID: 4890
		[Tooltip("The field of view for the cone when FoV source is user-defined.")]
		[SerializeField]
		[Range(0f, 360f)]
		private float _miniMapViewConeFoV = 60f;

		// Token: 0x0400131B RID: 4891
		[Tooltip("Distance of the view cone")]
		[SerializeField]
		private float _miniMapViewConeDistance = 150f;

		// Token: 0x0400131C RID: 4892
		[Tooltip("Fall-off/gradient for the view cone effect")]
		[SerializeField]
		[Range(0.0001f, 1f)]
		private float _miniMapViewConeFallOff = 0.75f;

		// Token: 0x0400131D RID: 4893
		[Tooltip("Enables view cone outline")]
		[SerializeField]
		private bool _miniMapShowViewConeOutline;

		// Token: 0x0400131E RID: 4894
		[Tooltip("Color of the view cone outline")]
		[SerializeField]
		private Color _miniMapViewConeOutlineColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x0400131F RID: 4895
		[Tooltip("The distance of clamped icons to the edge of the mini-map")]
		[SerializeField]
		private float _miniMapClampBorder = 0.02f;

		// Token: 0x04001320 RID: 4896
		[Tooltip("Enable this option if the minimap uses a circular shape")]
		[SerializeField]
		private bool _miniMapClampBorderCircular;

		// Token: 0x04001321 RID: 4897
		[Tooltip("Enable this option if the minimap uses a circular shape and want to darken the inner border.")]
		[SerializeField]
		private bool _miniMapVignette;

		// Token: 0x04001322 RID: 4898
		[Tooltip("Transparency of the mini-map")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _miniMapAlpha = 1f;

		// Token: 0x04001323 RID: 4899
		[Tooltip("Show the zoom in/out minimap button")]
		[SerializeField]
		private bool _miniMapShowZoomInOutButtons;

		// Token: 0x04001324 RID: 4900
		[SerializeField]
		[Range(0.001f, 10f)]
		private float _miniMapIconCircleAnimationDuration = 1f;

		// Token: 0x04001325 RID: 4901
		[Tooltip("Show the maximize minimap button.")]
		[SerializeField]
		private bool _miniMapShowMaximizeButton;

		// Token: 0x04001326 RID: 4902
		[Range(0.01f, 2f)]
		[SerializeField]
		private float _miniMapButtonsScale = 1f;

		// Token: 0x04001327 RID: 4903
		[Tooltip("Raise pointer click, down, up, enter and exit events on icons")]
		[SerializeField]
		private bool _miniMapIconEvents;

		// Token: 0x04001328 RID: 4904
		[Tooltip("Displays the distance to each ring")]
		[SerializeField]
		private MiniMapRadarInfoType _miniMapRadarInfoDisplay = MiniMapRadarInfoType.RingIntervalDistance;

		// Token: 0x04001329 RID: 4905
		[Tooltip("Method used to render radar graphics")]
		[SerializeField]
		private MiniMapRadarGraphicsMethod _miniMapRadarGraphicsMethod;

		// Token: 0x0400132A RID: 4906
		[SerializeField]
		private Color _miniMapRadarRingsColor = Color.white;

		// Token: 0x0400132B RID: 4907
		[SerializeField]
		private float _miniMapRadarRingsDistance = 10f;

		// Token: 0x0400132C RID: 4908
		[Range(0.01f, 4f)]
		[SerializeField]
		private float _miniMapRadarRingsWidth = 1f;

		// Token: 0x0400132D RID: 4909
		[SerializeField]
		private bool _miniMapRadarPulseEnabled = true;

		// Token: 0x0400132E RID: 4910
		[SerializeField]
		private MiniMapPulsePreset _miniMapRadarPulseAnimationPreset;

		// Token: 0x0400132F RID: 4911
		[Range(0f, 1f)]
		[SerializeField]
		private float _miniMapRadarPulseOpacity = 0.25f;

		// Token: 0x04001330 RID: 4912
		[SerializeField]
		private float _miniMapRadarPulseFrequency = 0.1f;

		// Token: 0x04001331 RID: 4913
		[SerializeField]
		private float _miniMapRadarPulseFallOff = 50f;

		// Token: 0x04001332 RID: 4914
		[SerializeField]
		private float _miniMapRadarPulseSpeed = 50f;

		// Token: 0x04001333 RID: 4915
		[SerializeField]
		private bool _miniMapRadarFadePOIs = true;

		// Token: 0x04001334 RID: 4916
		[SerializeField]
		private bool _miniMapFullScreenState;

		// Token: 0x04001335 RID: 4917
		[SerializeField]
		private Vector3 _miniMapFullScreenWorldCenter;

		// Token: 0x04001336 RID: 4918
		[SerializeField]
		private Vector3 _miniMapFullScreenWorldSize = new Vector3(1000f, 0f, 1000f);

		// Token: 0x04001337 RID: 4919
		[Tooltip("Forces center of the world map to be the same position of the followed object")]
		public bool miniMapFullScreenWorldCenterFollows = true;

		// Token: 0x04001338 RID: 4920
		[Tooltip("Prevents user move or rotate camera while in maximized mode")]
		public bool miniMapFullScreenFreezeCamera = true;

		// Token: 0x04001339 RID: 4921
		[Tooltip("Ensures the map doesn't scroll beyond world size in maximized mode")]
		public bool miniMapFullScreenClampToWorldEdges = true;

		// Token: 0x0400133A RID: 4922
		[Tooltip("Ensures the map doesn't scroll beyond world size in non-maximized mode")]
		public bool miniMapClampToWorldEdges;

		// Token: 0x0400133B RID: 4923
		[Tooltip("Center of the world map")]
		[SerializeField]
		private Vector3 _miniMapWorldCenter;

		// Token: 0x0400133C RID: 4924
		[Tooltip("Size of the world map")]
		[SerializeField]
		private Vector3 _miniMapWorldSize = new Vector3(1000f, 0f, 1000f);

		// Token: 0x0400133D RID: 4925
		[SerializeField]
		[Range(0f, 1f)]
		private float _miniMapLutIntensity = 1f;

		// Token: 0x0400133E RID: 4926
		[SerializeField]
		private Texture2D _miniMapLutTexture;

		// Token: 0x0400133F RID: 4927
		[SerializeField]
		private Color _miniMapVignetteColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04001340 RID: 4928
		public ScanProfile scanProfile;

		// Token: 0x04001341 RID: 4929
		private readonly int[] cardinals = new int[] { 0, 2, 4, 6 };

		// Token: 0x04001342 RID: 4930
		private readonly int[] ordinals = new int[] { 1, 3, 5, 7 };

		// Token: 0x04001343 RID: 4931
		private const string SAMPLE_TITLE_TEXT = "SAMPLE TITLE";

		// Token: 0x04001344 RID: 4932
		private const int TEXT_POOL_SIZE = 256;

		// Token: 0x04001345 RID: 4933
		private const string TEXT_POOL_OBJECT_NAME = "CompassProTextPool";

		// Token: 0x04001346 RID: 4934
		[NonSerialized]
		public readonly List<CompassProPOI> pois = new List<CompassProPOI>();

		// Token: 0x04001347 RID: 4935
		private readonly List<CompassProPOI> poisOfInterest = new List<CompassProPOI>();

		// Token: 0x04001348 RID: 4936
		[NonSerialized]
		public List<CompassProPOI> activePOIs;

		// Token: 0x04001349 RID: 4937
		private static readonly List<CompassPro> compasses = new List<CompassPro>();

		// Token: 0x0400134A RID: 4938
		private static CompassPro _instance;

		// Token: 0x0400134B RID: 4939
		private float fadeStartTime;

		// Token: 0x0400134C RID: 4940
		private float prevAlpha;

		// Token: 0x0400134D RID: 4941
		private CanvasGroup canvasGroup;

		// Token: 0x0400134E RID: 4942
		private RectTransform compassBackRT;

		// Token: 0x0400134F RID: 4943
		private Image compassBackImage;

		// Token: 0x04001350 RID: 4944
		private Text text;

		// Token: 0x04001351 RID: 4945
		private Text textShadow;

		// Token: 0x04001352 RID: 4946
		private Text title;

		// Token: 0x04001353 RID: 4947
		private Text titleShadow;

		// Token: 0x04001354 RID: 4948
		private TextMeshProUGUI titleTMP;

		// Token: 0x04001355 RID: 4949
		private float endTimeOfCurrentTextReveal;

		// Token: 0x04001356 RID: 4950
		private Vector3 lastCamPos;

		// Token: 0x04001357 RID: 4951
		private Quaternion lastCamRot;

		// Token: 0x04001358 RID: 4952
		private Quaternion lastFollowRot;

		// Token: 0x04001359 RID: 4953
		private int lastUpdateFrameCount;

		// Token: 0x0400135A RID: 4954
		private float lastUpdateTime;

		// Token: 0x0400135B RID: 4955
		private readonly StringBuilder titleText = new StringBuilder();

		// Token: 0x0400135C RID: 4956
		private RectTransform titleRT;

		// Token: 0x0400135D RID: 4957
		private RectTransform titleShadowRT;

		// Token: 0x0400135E RID: 4958
		private RectTransform titleTMPRT;

		// Token: 0x0400135F RID: 4959
		private Vector3 titleRTDefaultPosition;

		// Token: 0x04001360 RID: 4960
		private Vector3 titleShadowRTDefaultPosition;

		// Token: 0x04001361 RID: 4961
		private Vector3 titleTMPRTDefaultPosition;

		// Token: 0x04001362 RID: 4962
		private AudioSource audioSource;

		// Token: 0x04001363 RID: 4963
		private int poiVisibleCount;

		// Token: 0x04001364 RID: 4964
		private bool autoHiding;

		// Token: 0x04001365 RID: 4965
		private float thisAlpha;

		// Token: 0x04001366 RID: 4966
		private bool needUpdateCompassBarIcons;

		// Token: 0x04001367 RID: 4967
		private CompassPro.CompassPointPOI[] compassPoints;

		// Token: 0x04001368 RID: 4968
		private float usedNorthDegrees;

		// Token: 0x04001369 RID: 4969
		private LetterAnimator[] textPool;

		// Token: 0x0400136A RID: 4970
		private GameObject[] textPoolLetters;

		// Token: 0x0400136B RID: 4971
		private GameObject[] textPoolShadows;

		// Token: 0x0400136C RID: 4972
		private Vector3 textPoolOriginalLocalPosition;

		// Token: 0x0400136D RID: 4973
		private Vector3 textPoolOriginalShadowLocalPosition;

		// Token: 0x0400136E RID: 4974
		private int poolIndex;

		// Token: 0x0400136F RID: 4975
		private Transform canvasTextPool;

		// Token: 0x04001370 RID: 4976
		private Canvas _canvas;

		// Token: 0x04001371 RID: 4977
		private CompassProPOI lastNearestPOI;

		// Token: 0x04001372 RID: 4978
		private string lastNearestPOIDistanceText;

		// Token: 0x04001373 RID: 4979
		private float lastNearestPOIDistance;

		// Token: 0x04001374 RID: 4980
		private float nearestPOIDistance;

		// Token: 0x04001375 RID: 4981
		private float nearestPOIAlpha;

		// Token: 0x04001376 RID: 4982
		private Vector3 currentCamPos;

		// Token: 0x04001377 RID: 4983
		private Quaternion currentCamRot;

		// Token: 0x04001378 RID: 4984
		private Matrix4x4 currentVP;

		// Token: 0x04001379 RID: 4985
		private Vector3 followPos;

		// Token: 0x0400137A RID: 4986
		private Vector3 lastFollowPos;

		// Token: 0x0400137B RID: 4987
		private bool needsUpdateSettings;

		// Token: 0x0400137C RID: 4988
		private Vector3 lastVisitedDistanceFollowPos;

		// Token: 0x0400137D RID: 4989
		private Vector3 forward;

		// Token: 0x0400137E RID: 4990
		private int lastIndicatorsUpdateFrame = int.MinValue;

		// Token: 0x0400137F RID: 4991
		private Transform indicatorsRoot;

		// Token: 0x04001380 RID: 4992
		private Material compassBarMat;

		// Token: 0x04001381 RID: 4993
		private Material curvedMat;

		// Token: 0x04001382 RID: 4994
		private Material defaultUICurvedMatForCardinals;

		// Token: 0x04001383 RID: 4995
		private Material defaultUICurvedMatForText;

		// Token: 0x04001384 RID: 4996
		private string titleLocale;

		// Token: 0x04001385 RID: 4997
		[NonSerialized]
		public bool needFogOfWarUpdate;

		// Token: 0x04001386 RID: 4998
		[NonSerialized]
		public bool needFogOfWarTextureUpdate;

		// Token: 0x04001387 RID: 4999
		private Texture2D fogOfWarTexture;

		// Token: 0x04001388 RID: 5000
		private Color32[] fogOfWarColorBuffer;

		// Token: 0x04001389 RID: 5001
		private Material fogOfWarMaterial;

		// Token: 0x0400138A RID: 5002
		private int fogOfWarAutoClearLastPosX = int.MaxValue;

		// Token: 0x0400138B RID: 5003
		private int fogOfWarAutoClearLastPosZ;

		// Token: 0x0400138C RID: 5004
		private const string INDICATORS_ROOT_NAME = "OnScreen Indicators Root";

		// Token: 0x0400138D RID: 5005
		private const int MAX_STORED_VPOS = 100;

		// Token: 0x0400138E RID: 5006
		private readonly Vector3[] lastVPos = new Vector3[100];

		// Token: 0x0400138F RID: 5007
		private bool needUpdateIndicators;

		// Token: 0x04001390 RID: 5008
		[NonSerialized]
		public Camera miniMapCamera;

		// Token: 0x04001391 RID: 5009
		private bool needUpdateMiniMapIcons;

		// Token: 0x04001392 RID: 5010
		private Transform miniMapMaskUI;

		// Token: 0x04001393 RID: 5011
		private Transform miniMapButtonsPanel;

		// Token: 0x04001394 RID: 5012
		private RectTransform miniMapUI;

		// Token: 0x04001395 RID: 5013
		private RectTransform miniMapUIRootRT;

		// Token: 0x04001396 RID: 5014
		private Transform playerIcon;

		// Token: 0x04001397 RID: 5015
		private Transform miniMapCardinalsRT;

		// Token: 0x04001398 RID: 5016
		private RectTransform playerIconRT;

		// Token: 0x04001399 RID: 5017
		private Image playerIconImage;

		// Token: 0x0400139A RID: 5018
		private Image miniMapCardinalsImage;

		// Token: 0x0400139B RID: 5019
		private RenderTexture miniMapTex;

		// Token: 0x0400139C RID: 5020
		private CanvasGroup miniMapCanvasGroup;

		// Token: 0x0400139D RID: 5021
		private Material miniMapOverlayMat;

		// Token: 0x0400139E RID: 5022
		private Vector2 miniMapAnchorMin;

		// Token: 0x0400139F RID: 5023
		private Vector2 miniMapAnchorMax;

		// Token: 0x040013A0 RID: 5024
		private Vector2 miniMapPivot;

		// Token: 0x040013A1 RID: 5025
		private Vector2 miniMapSizeDelta;

		// Token: 0x040013A2 RID: 5026
		private float miniMapCameraAspect;

		// Token: 0x040013A3 RID: 5027
		private float miniMapLastSnapshotTime;

		// Token: 0x040013A4 RID: 5028
		private Vector3 miniMapLastSnapshotLocation;

		// Token: 0x040013A5 RID: 5029
		private Vector3 miniMapLastSnapshotCenter;

		// Token: 0x040013A6 RID: 5030
		private float miniMapLastSnapshotAltitude;

		// Token: 0x040013A7 RID: 5031
		private float miniMapLastSnapshotFoV;

		// Token: 0x040013A8 RID: 5032
		private float miniMapLastSnapshotRotationY;

		// Token: 0x040013A9 RID: 5033
		private Quaternion miniMapLastSnapshotRotation;

		// Token: 0x040013AA RID: 5034
		private Vector3 miniMapLastBoundsCheckPos;

		// Token: 0x040013AB RID: 5035
		private int needMiniMapShot;

		// Token: 0x040013AC RID: 5036
		private Image miniMapImage;

		// Token: 0x040013AD RID: 5037
		private Image miniMapMaskImage;

		// Token: 0x040013AE RID: 5038
		private Quaternion miniMapFullScreenFixedCameraRotation;

		// Token: 0x040013AF RID: 5039
		private Vector3 miniMapFullScreenFixedCameraPosition;

		// Token: 0x040013B0 RID: 5040
		private Quaternion miniMapFullScreenFixedFollowRotation;

		// Token: 0x040013B1 RID: 5041
		private Vector3 miniMapFullScreenFixedFollowPosition;

		// Token: 0x040013B2 RID: 5042
		private bool needsSetupMiniMap;

		// Token: 0x040013B3 RID: 5043
		private bool needsIconSorting;

		// Token: 0x040013B4 RID: 5044
		private float miniMapRegularZoomLevel;

		// Token: 0x040013B5 RID: 5045
		private float lastViewConeCameraAspect;

		// Token: 0x040013B6 RID: 5046
		private float lastViewConeFoV;

		// Token: 0x040013B7 RID: 5047
		private Vector4 viewConeData;

		// Token: 0x040013B8 RID: 5048
		private TextMeshProUGUI ringsDistanceText;

		// Token: 0x040013B9 RID: 5049
		private float lastRadarInfoDistance;

		// Token: 0x040013BA RID: 5050
		private Vector3 miniMapCenter;

		// Token: 0x02000287 RID: 647
		private enum CompassPoint
		{
			// Token: 0x040013BC RID: 5052
			CardinalEast,
			// Token: 0x040013BD RID: 5053
			OrdinalNorthEast,
			// Token: 0x040013BE RID: 5054
			CardinalNorth,
			// Token: 0x040013BF RID: 5055
			OrdinalNorthWest,
			// Token: 0x040013C0 RID: 5056
			CardinalWest,
			// Token: 0x040013C1 RID: 5057
			OrdinalSouthWest,
			// Token: 0x040013C2 RID: 5058
			CardinalSouth,
			// Token: 0x040013C3 RID: 5059
			OrdinalSouthEast
		}

		// Token: 0x02000288 RID: 648
		private struct CompassPointPOI
		{
			// Token: 0x040013C4 RID: 5060
			public Vector3 position;

			// Token: 0x040013C5 RID: 5061
			public float cos;

			// Token: 0x040013C6 RID: 5062
			public float sin;

			// Token: 0x040013C7 RID: 5063
			public Text text;
		}
	}
}
