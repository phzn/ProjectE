using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoreMountains.Feel
{
	// Token: 0x0200037D RID: 893
	[AddComponentMenu("")]
	public class Snake : MonoBehaviour
	{
		// Token: 0x06001874 RID: 6260 RVA: 0x00067DD4 File Offset: 0x00065FD4
		protected void Awake()
		{
			this._speed = this.Speed;
			this.SnakePoints = 0;
			this._recorder = base.gameObject.GetComponent<MMPositionRecorder>();
			this.PointsCounter.text = "0";
			this._snakeBodyParts = new List<SnakeBodyPart>();
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00067E20 File Offset: 0x00066020
		protected virtual void Update()
		{
			this.HandleInput();
			this.HandleMovement();
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00067E2E File Offset: 0x0006602E
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Turn();
			}
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00067E40 File Offset: 0x00066040
		protected virtual void HandleMovement()
		{
			this._speedMultiplier = ((Time.time - this._lastFoodEatenAt < this.BoostDuration) ? this.BoostMultiplier : this.NormalSpeedMultiplier);
			this._speed = MMMaths.Lerp(this._speed, this.Speed * this._speedMultiplier, this.SpeedChangeRate, Time.deltaTime);
			this._newPosition = this._speed * Time.deltaTime * this.Direction;
			base.transform.position += this._newPosition;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00067ED8 File Offset: 0x000660D8
		public virtual void Turn()
		{
			MMFeedbacks turnFeedback = this.TurnFeedback;
			if (turnFeedback != null)
			{
				turnFeedback.PlayFeedbacks();
			}
			this.Direction = MMMaths.RotateVector2(this.Direction, 90f);
			base.transform.Rotate(new Vector3(0f, 0f, 90f));
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00067F35 File Offset: 0x00066135
		public virtual void Teleport()
		{
			base.StartCoroutine(this.TeleportCo());
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00067F44 File Offset: 0x00066144
		protected virtual IEnumerator TeleportCo()
		{
			MMFeedbacks teleportFeedback = this.TeleportFeedback;
			if (teleportFeedback != null)
			{
				teleportFeedback.PlayFeedbacks();
			}
			MMFeedbacks teleportOnceFeedback = this.TeleportOnceFeedback;
			if (teleportOnceFeedback != null)
			{
				teleportOnceFeedback.PlayFeedbacks();
			}
			yield return MMCoroutine.WaitForFrames(this.BodyPartsOffset);
			int total = this._snakeBodyParts.Count;
			float part = 1f / (float)total;
			int num2;
			for (int i = 0; i < total; i = num2 + 1)
			{
				yield return MMCoroutine.WaitForFrames(this.BodyPartsOffset / 2);
				float num = 1f - (float)i * part;
				MMFeedbacks teleportFeedback2 = this.TeleportFeedback;
				if (teleportFeedback2 != null)
				{
					teleportFeedback2.PlayFeedbacks(base.transform.position, num, false);
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00067F54 File Offset: 0x00066154
		public virtual void Eat()
		{
			this.EatEffect();
			MMFeedbacks eatFeedback = this.EatFeedback;
			if (eatFeedback != null)
			{
				eatFeedback.PlayFeedbacks();
			}
			this.SnakePoints++;
			this.PointsCounter.text = this.SnakePoints.ToString();
			base.StartCoroutine(this.EatCo());
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00067FA9 File Offset: 0x000661A9
		protected virtual IEnumerator EatCo()
		{
			int total = this._snakeBodyParts.Count;
			float part = 1f / (float)total;
			int num2;
			for (int i = 0; i < total; i = num2 + 1)
			{
				if (i >= this._snakeBodyParts.Count)
				{
					yield break;
				}
				yield return MMCoroutine.WaitForFrames(this.BodyPartsOffset / 2);
				float num = 1f - (float)i * part;
				if (i == total - 1)
				{
					if (i < this.MaxAmountOfBodyParts - 1 && this._snakeBodyParts.Count > i && this._snakeBodyParts[i] != null)
					{
						this._snakeBodyParts[i].New();
					}
				}
				else
				{
					this._snakeBodyParts[i].Eat(num);
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00067FB8 File Offset: 0x000661B8
		public virtual void EatEffect()
		{
			this._lastFoodEatenAt = Time.time;
			if (this.SnakePoints < this.MaxAmountOfBodyParts - 1)
			{
				SnakeBodyPart snakeBodyPart = Object.Instantiate<SnakeBodyPart>(this.BodyPartPrefab);
				SceneManager.MoveGameObjectToScene(snakeBodyPart.gameObject, base.gameObject.scene);
				snakeBodyPart.transform.position = base.transform.position;
				snakeBodyPart.TargetRecorder = this._recorder;
				snakeBodyPart.Offset = this.SnakePoints * this.BodyPartsOffset + this.BodyPartsOffset + 1;
				snakeBodyPart.Index = this._snakeBodyParts.Count;
				snakeBodyPart.name = "SnakeBodyPart_" + snakeBodyPart.Index.ToString();
				this._snakeBodyParts.Add(snakeBodyPart);
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0006807C File Offset: 0x0006627C
		public virtual void Lose(SnakeBodyPart part)
		{
			if (Time.time - this._lastLostPart < this.MinTimeBetweenLostParts)
			{
				return;
			}
			this._lastLostPart = Time.time;
			MMFeedbacks loseFeedback = this.LoseFeedback;
			if (loseFeedback != null)
			{
				loseFeedback.PlayFeedbacks(part.transform.position, 1f, false);
			}
			Object.Destroy(this._snakeBodyParts[this._snakeBodyParts.Count - 1].gameObject);
			this._snakeBodyParts.RemoveAt(this._snakeBodyParts.Count - 1);
			this.SnakePoints--;
			this.PointsCounter.text = this.SnakePoints.ToString();
		}

		// Token: 0x04001C6F RID: 7279
		[Header("Movement")]
		public float Speed = 5f;

		// Token: 0x04001C70 RID: 7280
		public float NormalSpeedMultiplier = 1f;

		// Token: 0x04001C71 RID: 7281
		public float SpeedChangeRate = 0.2f;

		// Token: 0x04001C72 RID: 7282
		public Vector3 Direction = Vector2.right;

		// Token: 0x04001C73 RID: 7283
		[Header("Boost")]
		public float BoostMultiplier = 2f;

		// Token: 0x04001C74 RID: 7284
		public float BoostDuration = 2f;

		// Token: 0x04001C75 RID: 7285
		[Header("BodyParts")]
		public SnakeBodyPart BodyPartPrefab;

		// Token: 0x04001C76 RID: 7286
		public int BodyPartsOffset = 7;

		// Token: 0x04001C77 RID: 7287
		public int MaxAmountOfBodyParts = 10;

		// Token: 0x04001C78 RID: 7288
		public float MinTimeBetweenLostParts = 2f;

		// Token: 0x04001C79 RID: 7289
		[Header("Bindings")]
		public Text PointsCounter;

		// Token: 0x04001C7A RID: 7290
		[Header("Feedbacks")]
		public MMFeedbacks TurnFeedback;

		// Token: 0x04001C7B RID: 7291
		public MMFeedbacks TeleportFeedback;

		// Token: 0x04001C7C RID: 7292
		public MMFeedbacks TeleportOnceFeedback;

		// Token: 0x04001C7D RID: 7293
		public MMFeedbacks EatFeedback;

		// Token: 0x04001C7E RID: 7294
		public MMFeedbacks LoseFeedback;

		// Token: 0x04001C7F RID: 7295
		[Header("Debug")]
		[MMReadOnly]
		public int SnakePoints;

		// Token: 0x04001C80 RID: 7296
		[MMReadOnly]
		public float _speed;

		// Token: 0x04001C81 RID: 7297
		[MMReadOnly]
		public float _speedMultiplier;

		// Token: 0x04001C82 RID: 7298
		[MMReadOnly]
		public float _lastFoodEatenAt = -100f;

		// Token: 0x04001C83 RID: 7299
		protected Vector3 _newPosition;

		// Token: 0x04001C84 RID: 7300
		protected MMPositionRecorder _recorder;

		// Token: 0x04001C85 RID: 7301
		public List<SnakeBodyPart> _snakeBodyParts;

		// Token: 0x04001C86 RID: 7302
		protected float _lastLostPart;
	}
}
