using System;
using Features.Detection;
using Features.Inventory.Wearable;
using Features.Rank;
using UnityEngine;
using UnityEngine.Events;

namespace Features.ExposeLingerie
{
	// Token: 0x02001216 RID: 4630
	public class ExposeLingerieFaceController : MonoBehaviour
	{
		// Token: 0x06008F29 RID: 36649 RVA: 0x003A8E70 File Offset: 0x003A7070
		private void Awake()
		{
			if (this.exposeController == null)
			{
				this.exposeController = base.GetComponent<ExposeLingerieController>();
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
		}

		// Token: 0x06008F2A RID: 36650 RVA: 0x003A8ED4 File Offset: 0x003A70D4
		private void OnEnable()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn += this.HandleWornChanged;
				this._wearController.OnItemUnworn += this.HandleWornChanged;
			}
			if (this.exposeController != null)
			{
				this.exposeController.OnGestureStarted.AddListener(new UnityAction(this.HandleGestureChanged));
				this.exposeController.OnGestureEnded.AddListener(new UnityAction(this.HandleGestureChanged));
			}
		}

		// Token: 0x06008F2B RID: 36651 RVA: 0x003A8F64 File Offset: 0x003A7164
		private void OnDisable()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn -= this.HandleWornChanged;
				this._wearController.OnItemUnworn -= this.HandleWornChanged;
			}
			if (this.exposeController != null)
			{
				this.exposeController.OnGestureStarted.RemoveListener(new UnityAction(this.HandleGestureChanged));
				this.exposeController.OnGestureEnded.RemoveListener(new UnityAction(this.HandleGestureChanged));
			}
			this.Release();
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06008F2C RID: 36652 RVA: 0x003A8FF9 File Offset: 0x003A71F9
		private bool AnyWorn
		{
			get
			{
				return this._wearController != null && (this._wearController.IsWorn(this.braClothItem) || this._wearController.IsWorn(this.pantiesClothItem));
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06008F2D RID: 36653 RVA: 0x003A9031 File Offset: 0x003A7231
		private int CurrentExpression
		{
			get
			{
				if (!(this.exposeController != null) || (!this.exposeController.IsPlaying && !this.exposeController.AnyOpen))
				{
					return this.hornyExpressionIndex;
				}
				return this.supremeExpressionIndex;
			}
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x003A9068 File Offset: 0x003A7268
		private void HandleWornChanged(WearableItemSO item)
		{
			if (item != this.braClothItem && item != this.pantiesClothItem)
			{
				return;
			}
			this.Reapply();
		}

		// Token: 0x06008F2F RID: 36655 RVA: 0x003A908D File Offset: 0x003A728D
		private void HandleGestureChanged()
		{
			if (this._holding)
			{
				this.Apply();
			}
		}

		// Token: 0x06008F30 RID: 36656 RVA: 0x003A90A0 File Offset: 0x003A72A0
		private void Update()
		{
			if (Time.time < this._nextCheck)
			{
				return;
			}
			this._nextCheck = Time.time + Mathf.Max(0.1f, this.reassertInterval);
			if (this.AnyWorn)
			{
				PlayerEmotionController instance = PlayerEmotionController.Instance;
				if (instance != null && !instance.IsFacialExpressionOverriddenForAction)
				{
					this._holding = false;
					this.Apply();
					return;
				}
			}
			else if (this._holding)
			{
				this.Release();
			}
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x003A9112 File Offset: 0x003A7312
		private void Reapply()
		{
			if (this.AnyWorn)
			{
				this.Apply();
				return;
			}
			this.Release();
		}

		// Token: 0x06008F32 RID: 36658 RVA: 0x003A912C File Offset: 0x003A732C
		private void Apply()
		{
			if (RankRuntimeController.Current != null)
			{
				if (this._holding)
				{
					this.Release();
				}
				return;
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			if (!this._holding)
			{
				instance.DisableFacialExpressionForAction();
				this._holding = true;
			}
			instance.SetExpression(this.CurrentExpression);
		}

		// Token: 0x06008F33 RID: 36659 RVA: 0x003A9188 File Offset: 0x003A7388
		private void Release()
		{
			if (!this._holding)
			{
				return;
			}
			this._holding = false;
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance != null)
			{
				instance.RestoreFacialExpression();
			}
		}

		// Token: 0x040092BD RID: 37565
		[Header("Items")]
		[SerializeField]
		private WearableItemSO braClothItem;

		// Token: 0x040092BE RID: 37566
		[SerializeField]
		private WearableItemSO pantiesClothItem;

		// Token: 0x040092BF RID: 37567
		[Header("Gesture (auto-found if empty)")]
		[SerializeField]
		private ExposeLingerieController exposeController;

		// Token: 0x040092C0 RID: 37568
		[Header("Face expression indices (Face layer / FaceExpressionIndex)")]
		[Tooltip("Expression while the lingerie is worn. 2 = Horny Face.")]
		[SerializeField]
		private int hornyExpressionIndex = 2;

		// Token: 0x040092C1 RID: 37569
		[Tooltip("Expression while an expose gesture is playing. 3 = HornySupreme.")]
		[SerializeField]
		private int supremeExpressionIndex = 3;

		// Token: 0x040092C2 RID: 37570
		[Tooltip("Seconds between re-acquire checks (covers boot order and other actions releasing the face).")]
		[SerializeField]
		private float reassertInterval = 0.5f;

		// Token: 0x040092C3 RID: 37571
		private WearableUseController _wearController;

		// Token: 0x040092C4 RID: 37572
		private bool _holding;

		// Token: 0x040092C5 RID: 37573
		private float _nextCheck;
	}
}
