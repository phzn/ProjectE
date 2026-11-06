using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200004E RID: 78
	[AddComponentMenu("More Mountains/Tools/GUI/MMHealthBar")]
	public class MMHealthBar : MonoBehaviour
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00007F2F File Offset: 0x0000612F
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007F37 File Offset: 0x00006137
		protected void OnEnable()
		{
			this._finalHideStarted = false;
			this.SetInitialActiveState();
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007F46 File Offset: 0x00006146
		public virtual void SetInitialActiveState()
		{
			if (!this.AlwaysVisible && this._progressBar != null)
			{
				this.ShowBar(false);
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007F65 File Offset: 0x00006165
		public virtual void ShowBar(bool state)
		{
			this._progressBar.gameObject.SetActive(state);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007F78 File Offset: 0x00006178
		public virtual bool BarIsShown()
		{
			return this._progressBar.gameObject.activeInHierarchy;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007F8C File Offset: 0x0000618C
		public virtual void Initialization()
		{
			this._finalHideStarted = false;
			if (this._progressBar != null)
			{
				this.ShowBar(this.AlwaysVisible);
				return;
			}
			switch (this.HealthBarType)
			{
			case MMHealthBar.HealthBarTypes.Prefab:
				if (this.HealthBarPrefab == null)
				{
					Debug.LogWarning(base.name + " : the HealthBar has no prefab associated to it, nothing will be displayed.");
					return;
				}
				this._progressBar = Object.Instantiate<MMProgressBar>(this.HealthBarPrefab, base.transform.position + this.HealthBarOffset, base.transform.rotation);
				SceneManager.MoveGameObjectToScene(this._progressBar.gameObject, base.gameObject.scene);
				this._progressBar.transform.SetParent(base.transform);
				this._progressBar.gameObject.name = "HealthBar";
				break;
			case MMHealthBar.HealthBarTypes.Drawn:
				this.DrawHealthBar();
				this.UpdateDrawnColors();
				break;
			case MMHealthBar.HealthBarTypes.Existing:
				this._progressBar = this.TargetProgressBar;
				break;
			}
			if (!this.AlwaysVisible)
			{
				this.ShowBar(false);
			}
			if (this._progressBar != null)
			{
				this._progressBar.SetBar(100f, 0f, 100f);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000080C8 File Offset: 0x000062C8
		protected virtual void DrawHealthBar()
		{
			GameObject gameObject = new GameObject();
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			gameObject.name = "HealthBar|" + base.gameObject.name;
			if (this.NestDrawnHealthBar)
			{
				gameObject.transform.SetParent(base.transform);
			}
			this._progressBar = gameObject.AddComponent<MMProgressBar>();
			this._followTransform = gameObject.AddComponent<MMFollowTarget>();
			this._followTransform.Offset = this.HealthBarOffset;
			this._followTransform.Target = base.transform;
			this._followTransform.FollowRotation = this.FollowRotation;
			this._followTransform.FollowScale = this.FollowScale;
			this._followTransform.InterpolatePosition = false;
			this._followTransform.InterpolateRotation = false;
			this._followTransform.UpdateMode = this.FollowTargetMode;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;
			canvas.transform.localScale = Vector3.one;
			canvas.GetComponent<RectTransform>().sizeDelta = this.Size;
			if (!string.IsNullOrEmpty(this.SortingLayerName))
			{
				canvas.sortingLayerName = this.SortingLayerName;
			}
			GameObject gameObject2 = new GameObject();
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.name = "MMProgressBarContainer";
			gameObject2.transform.localScale = Vector3.one;
			GameObject gameObject3 = new GameObject();
			gameObject3.transform.SetParent(gameObject2.transform);
			gameObject3.name = "HealthBar Border";
			this._borderImage = gameObject3.AddComponent<Image>();
			this._borderImage.transform.position = Vector3.zero;
			this._borderImage.transform.localScale = Vector3.one;
			this._borderImage.GetComponent<RectTransform>().sizeDelta = this.Size;
			this._borderImage.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			GameObject gameObject4 = new GameObject();
			gameObject4.transform.SetParent(gameObject2.transform);
			gameObject4.name = "HealthBar Background";
			this._backgroundImage = gameObject4.AddComponent<Image>();
			this._backgroundImage.transform.position = Vector3.zero;
			this._backgroundImage.transform.localScale = Vector3.one;
			this._backgroundImage.GetComponent<RectTransform>().sizeDelta = this.Size - this.BackgroundPadding * 2f;
			this._backgroundImage.GetComponent<RectTransform>().anchoredPosition = -this._backgroundImage.GetComponent<RectTransform>().sizeDelta / 2f;
			this._backgroundImage.GetComponent<RectTransform>().pivot = Vector2.zero;
			GameObject gameObject5 = new GameObject();
			gameObject5.transform.SetParent(gameObject2.transform);
			gameObject5.name = "HealthBar Delayed Foreground";
			this._delayedImage = gameObject5.AddComponent<Image>();
			this._delayedImage.transform.position = Vector3.zero;
			this._delayedImage.transform.localScale = Vector3.one;
			this._delayedImage.GetComponent<RectTransform>().sizeDelta = this.Size - this.BackgroundPadding * 2f;
			this._delayedImage.GetComponent<RectTransform>().anchoredPosition = -this._delayedImage.GetComponent<RectTransform>().sizeDelta / 2f;
			this._delayedImage.GetComponent<RectTransform>().pivot = Vector2.zero;
			GameObject gameObject6 = new GameObject();
			gameObject6.transform.SetParent(gameObject2.transform);
			gameObject6.name = "HealthBar Foreground";
			this._foregroundImage = gameObject6.AddComponent<Image>();
			this._foregroundImage.transform.position = Vector3.zero;
			this._foregroundImage.transform.localScale = Vector3.one;
			this._foregroundImage.color = this.ForegroundColor.Evaluate(1f);
			this._foregroundImage.GetComponent<RectTransform>().sizeDelta = this.Size - this.BackgroundPadding * 2f;
			this._foregroundImage.GetComponent<RectTransform>().anchoredPosition = -this._foregroundImage.GetComponent<RectTransform>().sizeDelta / 2f;
			this._foregroundImage.GetComponent<RectTransform>().pivot = Vector2.zero;
			if (this.Billboard)
			{
				this._progressBar.gameObject.AddComponent<MMBillboard>().NestObject = !this.NestDrawnHealthBar;
			}
			this._progressBar.LerpDecreasingDelayedBar = this.LerpDelayedBar;
			this._progressBar.LerpForegroundBar = this.LerpFrontBar;
			this._progressBar.LerpDecreasingDelayedBarSpeed = this.LerpDelayedBarSpeed;
			this._progressBar.LerpForegroundBarSpeedIncreasing = this.LerpFrontBarSpeed;
			this._progressBar.ForegroundBar = this._foregroundImage.transform;
			this._progressBar.DelayedBarDecreasing = this._delayedImage.transform;
			this._progressBar.DecreasingDelay = this.Delay;
			this._progressBar.BumpScaleOnChange = this.BumpScaleOnChange;
			this._progressBar.BumpDuration = this.BumpDuration;
			this._progressBar.BumpScaleAnimationCurve = this.BumpAnimationCurve;
			this._progressBar.TimeScale = ((this.TimeScale == MMHealthBar.TimeScales.Time) ? MMProgressBar.TimeScales.Time : MMProgressBar.TimeScales.UnscaledTime);
			gameObject2.transform.localEulerAngles = this.InitialRotationAngles;
			this._progressBar.Initialization();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008634 File Offset: 0x00006834
		protected virtual void Update()
		{
			if (this._progressBar == null)
			{
				return;
			}
			if (this._finalHideStarted)
			{
				return;
			}
			this.UpdateDrawnColors();
			if (this.AlwaysVisible)
			{
				return;
			}
			if (this._showBar)
			{
				this.ShowBar(true);
				if (((this.TimeScale == MMHealthBar.TimeScales.UnscaledTime) ? Time.unscaledTime : Time.time) - this._lastShowTimestamp > this.DisplayDurationOnHit)
				{
					this._showBar = false;
					return;
				}
			}
			else if (this.BarIsShown())
			{
				this.ShowBar(false);
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000086B1 File Offset: 0x000068B1
		protected virtual IEnumerator FinalHideBar()
		{
			this._finalHideStarted = true;
			if (this.InstantiatedOnDeath != null)
			{
				SceneManager.MoveGameObjectToScene(Object.Instantiate<GameObject>(this.InstantiatedOnDeath, base.transform.position + this.HealthBarOffset, base.transform.rotation).gameObject, base.gameObject.scene);
			}
			if (this.HideBarAtZeroDelay == 0f)
			{
				this._showBar = false;
				this.ShowBar(false);
				yield return null;
			}
			else
			{
				this._progressBar.HideBar(this.HideBarAtZeroDelay);
			}
			yield break;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000086C0 File Offset: 0x000068C0
		protected virtual void UpdateDrawnColors()
		{
			if (this.HealthBarType != MMHealthBar.HealthBarTypes.Drawn)
			{
				return;
			}
			if (this._progressBar.Bumping)
			{
				return;
			}
			if (this._borderImage != null)
			{
				this._borderImage.color = this.BorderColor.Evaluate(this._progressBar.BarProgress);
			}
			if (this._backgroundImage != null)
			{
				this._backgroundImage.color = this.BackgroundColor.Evaluate(this._progressBar.BarProgress);
			}
			if (this._delayedImage != null)
			{
				this._delayedImage.color = this.DelayedColor.Evaluate(this._progressBar.BarProgress);
			}
			if (this._foregroundImage != null)
			{
				this._foregroundImage.color = this.ForegroundColor.Evaluate(this._progressBar.BarProgress);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000087A4 File Offset: 0x000069A4
		public virtual void UpdateBar(float currentHealth, float minHealth, float maxHealth, bool show)
		{
			if (!this.AlwaysVisible && show)
			{
				this._showBar = true;
				this._lastShowTimestamp = ((this.TimeScale == MMHealthBar.TimeScales.UnscaledTime) ? Time.unscaledTime : Time.time);
			}
			if (this._progressBar != null)
			{
				this._progressBar.UpdateBar(currentHealth, minHealth, maxHealth);
				if (this.HideBarAtZero && this._progressBar.BarTarget <= 0f)
				{
					base.StartCoroutine(this.FinalHideBar());
				}
				if (this.BumpScaleOnChange)
				{
					this._progressBar.Bump();
				}
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008836 File Offset: 0x00006A36
		protected virtual void TestUpdateHealth()
		{
			this.UpdateBar(this.TestCurrentHealth, this.TestMinHealth, this.TestMaxHealth, true);
		}

		// Token: 0x04000233 RID: 563
		[MMInformation("Add this component to an object and it'll add a healthbar next to it to reflect its health level in real time. You can decide here whether the health bar should be drawn automatically or use a prefab.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("whether the healthbar uses a prefab or is drawn automatically")]
		public MMHealthBar.HealthBarTypes HealthBarType = MMHealthBar.HealthBarTypes.Drawn;

		// Token: 0x04000234 RID: 564
		[Tooltip("defines whether the bar will work on scaled or unscaled time (whether or not it'll keep moving if time is slowed down for example)")]
		public MMHealthBar.TimeScales TimeScale;

		// Token: 0x04000235 RID: 565
		[Header("Select a Prefab")]
		[MMInformation("Select a prefab with a progress bar script on it. There is one example of such a prefab in Common/Prefabs/GUI.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the prefab to use as the health bar")]
		public MMProgressBar HealthBarPrefab;

		// Token: 0x04000236 RID: 566
		[Header("Existing MMProgressBar")]
		[Tooltip("the MMProgressBar this health bar should update")]
		public MMProgressBar TargetProgressBar;

		// Token: 0x04000237 RID: 567
		[Header("Drawn Healthbar Settings ")]
		[MMInformation("Set the size (in world units), padding, back and front colors of the healthbar.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("if the healthbar is drawn, its size in world units")]
		public Vector2 Size = new Vector2(1f, 0.2f);

		// Token: 0x04000238 RID: 568
		[Tooltip("if the healthbar is drawn, the padding to apply to the foreground, in world units")]
		public Vector2 BackgroundPadding = new Vector2(0.01f, 0.01f);

		// Token: 0x04000239 RID: 569
		[Tooltip("the rotation to apply to the MMHealthBarContainer when drawing it")]
		public Vector3 InitialRotationAngles;

		// Token: 0x0400023A RID: 570
		[Tooltip("if the healthbar is drawn, the color of its foreground")]
		public Gradient ForegroundColor = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(MMColors.BestRed, 0f),
				new GradientColorKey(MMColors.BestRed, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 1f)
			}
		};

		// Token: 0x0400023B RID: 571
		[Tooltip("if the healthbar is drawn, the color of its delayed bar")]
		public Gradient DelayedColor = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(MMColors.Orange, 0f),
				new GradientColorKey(MMColors.Orange, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 1f)
			}
		};

		// Token: 0x0400023C RID: 572
		[Tooltip("if the healthbar is drawn, the color of its border")]
		public Gradient BorderColor = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(MMColors.AntiqueWhite, 0f),
				new GradientColorKey(MMColors.AntiqueWhite, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 1f)
			}
		};

		// Token: 0x0400023D RID: 573
		[Tooltip("if the healthbar is drawn, the color of its background")]
		public Gradient BackgroundColor = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(MMColors.Black, 0f),
				new GradientColorKey(MMColors.Black, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 1f)
			}
		};

		// Token: 0x0400023E RID: 574
		[Tooltip("the name of the sorting layer to put this health bar on")]
		public string SortingLayerName = "UI";

		// Token: 0x0400023F RID: 575
		[Tooltip("the delay to apply to the delayed bar if drawn")]
		public float Delay = 0.5f;

		// Token: 0x04000240 RID: 576
		[Tooltip("whether or not the front bar should lerp")]
		public bool LerpFrontBar = true;

		// Token: 0x04000241 RID: 577
		[Tooltip("the speed at which the front bar lerps")]
		public float LerpFrontBarSpeed = 15f;

		// Token: 0x04000242 RID: 578
		[Tooltip("whether or not the delayed bar should lerp")]
		public bool LerpDelayedBar = true;

		// Token: 0x04000243 RID: 579
		[Tooltip("the speed at which the delayed bar lerps")]
		public float LerpDelayedBarSpeed = 15f;

		// Token: 0x04000244 RID: 580
		[Tooltip("if this is true, bumps the scale of the healthbar when its value changes")]
		public bool BumpScaleOnChange = true;

		// Token: 0x04000245 RID: 581
		[Tooltip("the duration of the bump animation")]
		public float BumpDuration = 0.2f;

		// Token: 0x04000246 RID: 582
		[Tooltip("the animation curve to map the bump animation on")]
		public AnimationCurve BumpAnimationCurve = AnimationCurve.Constant(0f, 1f, 1f);

		// Token: 0x04000247 RID: 583
		[Tooltip("the mode the bar should follow the target in")]
		public MMFollowTarget.UpdateModes FollowTargetMode = MMFollowTarget.UpdateModes.LateUpdate;

		// Token: 0x04000248 RID: 584
		[Tooltip("if this is true, the drawn health bar will adapt its rotation to match the one of its target")]
		public bool FollowRotation;

		// Token: 0x04000249 RID: 585
		[Tooltip("if this is true, the drawn health bar will adapt its scale to match the one of its target")]
		public bool FollowScale = true;

		// Token: 0x0400024A RID: 586
		[Tooltip("if this is true, the drawn health bar will be nested below the MMHealthBar")]
		public bool NestDrawnHealthBar;

		// Token: 0x0400024B RID: 587
		[Tooltip("if this is true, a MMBillboard component will be added to the progress bar to make sure it always looks towards the camera")]
		public bool Billboard;

		// Token: 0x0400024C RID: 588
		[Header("Death")]
		[Tooltip("a gameobject (usually a particle system) to instantiate when the healthbar reaches zero")]
		public GameObject InstantiatedOnDeath;

		// Token: 0x0400024D RID: 589
		[Header("Offset")]
		[MMInformation("Set the offset (in world units), relative to the object's center, to which the health bar will be displayed.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the offset to apply to the healthbar compared to the object's center")]
		public Vector3 HealthBarOffset = new Vector3(0f, 1f, 0f);

		// Token: 0x0400024E RID: 590
		[Header("Display")]
		[MMInformation("Here you can define whether or not the healthbar should always be visible. If not, you can set here how long after a hit it'll remain visible.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("whether or not the bar should be permanently displayed")]
		public bool AlwaysVisible = true;

		// Token: 0x0400024F RID: 591
		[Tooltip("the duration (in seconds) during which to display the bar")]
		public float DisplayDurationOnHit = 1f;

		// Token: 0x04000250 RID: 592
		[Tooltip("if this is set to true the bar will hide itself when it reaches zero")]
		public bool HideBarAtZero = true;

		// Token: 0x04000251 RID: 593
		[Tooltip("the delay (in seconds) after which to hide the bar")]
		public float HideBarAtZeroDelay = 1f;

		// Token: 0x04000252 RID: 594
		[Header("Test")]
		[Tooltip("a test value to use when pressing the TestUpdateHealth button")]
		public float TestMinHealth;

		// Token: 0x04000253 RID: 595
		[Tooltip("a test value to use when pressing the TestUpdateHealth button")]
		public float TestMaxHealth = 100f;

		// Token: 0x04000254 RID: 596
		[Tooltip("a test value to use when pressing the TestUpdateHealth button")]
		public float TestCurrentHealth = 25f;

		// Token: 0x04000255 RID: 597
		[MMInspectorButton("TestUpdateHealth")]
		public bool TestUpdateHealthButton;

		// Token: 0x04000256 RID: 598
		protected MMProgressBar _progressBar;

		// Token: 0x04000257 RID: 599
		protected MMFollowTarget _followTransform;

		// Token: 0x04000258 RID: 600
		protected float _lastShowTimestamp;

		// Token: 0x04000259 RID: 601
		protected bool _showBar;

		// Token: 0x0400025A RID: 602
		protected Image _backgroundImage;

		// Token: 0x0400025B RID: 603
		protected Image _borderImage;

		// Token: 0x0400025C RID: 604
		protected Image _foregroundImage;

		// Token: 0x0400025D RID: 605
		protected Image _delayedImage;

		// Token: 0x0400025E RID: 606
		protected bool _finalHideStarted;

		// Token: 0x020003C2 RID: 962
		public enum HealthBarTypes
		{
			// Token: 0x04001DD0 RID: 7632
			Prefab,
			// Token: 0x04001DD1 RID: 7633
			Drawn,
			// Token: 0x04001DD2 RID: 7634
			Existing
		}

		// Token: 0x020003C3 RID: 963
		public enum TimeScales
		{
			// Token: 0x04001DD4 RID: 7636
			UnscaledTime,
			// Token: 0x04001DD5 RID: 7637
			Time
		}
	}
}
