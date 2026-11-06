using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feel
{
	// Token: 0x02000391 RID: 913
	[AddComponentMenu("")]
	public class Strike : MonoBehaviour
	{
		// Token: 0x060018E0 RID: 6368 RVA: 0x00069B19 File Offset: 0x00067D19
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00069B24 File Offset: 0x00067D24
		protected virtual void Initialization()
		{
			Physics.gravity = this.Gravity;
			Physics.bounceThreshold = 2f;
			Physics.sleepThreshold = 0.005f;
			Physics.defaultContactOffset = 0.01f;
			Physics.defaultSolverIterations = 6;
			Physics.defaultSolverVelocityIterations = 1;
			Physics.queriesHitTriggers = true;
			this.ConsecutiveStrikes = 0;
			this.LastScore = 0;
			this.TotalPoints = 0;
			this.ConsecutiveStrikesText.text = "0";
			this.LastScoreText.text = "0";
			this.TotalScoreText.text = "0";
			this.SetStrikeElements(false);
			this._initialBallPosition = this.BowlingBallRb.transform.position;
			this._initialBallRotation = this.BowlingBallRb.transform.localRotation;
			this._strikePins = new List<StrikePin>();
			this._pinColliders = new List<Collider>();
			foreach (Rigidbody rigidbody in this.Pins)
			{
				StrikePin strikePin = default(StrikePin);
				strikePin.Rb = rigidbody;
				strikePin.InitialPosition = rigidbody.transform.position;
				strikePin.InitialRotation = rigidbody.transform.rotation;
				this._strikePins.Add(strikePin);
				this._pinColliders.Add(strikePin.Rb.gameObject.GetComponent<Collider>());
			}
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00069C94 File Offset: 0x00067E94
		protected virtual void SetStrikeElements(bool status)
		{
			foreach (GameObject gameObject in this.StrikeElements)
			{
				gameObject.SetActive(status);
			}
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00069CE8 File Offset: 0x00067EE8
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00069CF0 File Offset: 0x00067EF0
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.StartBall();
			}
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00069CFF File Offset: 0x00067EFF
		protected virtual void StartBall()
		{
			if (!this._ballThrown)
			{
				MMFeedbacks throwBallFeedback = this.ThrowBallFeedback;
				if (throwBallFeedback != null)
				{
					throwBallFeedback.PlayFeedbacks();
				}
				this.BowlingBallLauncherWiggler.RotationActive = false;
				this._ballThrown = true;
			}
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00069D30 File Offset: 0x00067F30
		public virtual void ThrowBall()
		{
			if (this.BowlingBallRb != null)
			{
				this.BowlingBallRb.AddRelativeForce(this.ThrowingForce, ForceMode.Impulse);
				this.BowlingBallRb.AddTorque(this.ThrowingForce, ForceMode.Impulse);
				this._resetCoroutine = base.StartCoroutine(this.ResetCountdown());
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00069D81 File Offset: 0x00067F81
		protected void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.MMGetComponentNoAlloc<StrikeBall>() != null)
			{
				base.StartCoroutine(this.ResetSceneCo());
				if (this._resetCoroutine != null)
				{
					base.StopCoroutine(this._resetCoroutine);
				}
			}
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00069DB7 File Offset: 0x00067FB7
		protected virtual IEnumerator ResetCountdown()
		{
			yield return MMCoroutine.WaitFor(this.MaxDurationBeforeReset);
			base.StartCoroutine(this.ResetSceneCo());
			yield break;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00069DC6 File Offset: 0x00067FC6
		protected virtual IEnumerator ResetSceneCo()
		{
			yield return MMCoroutine.WaitFor(this.DelayBeforeReset);
			this.CountPoints();
			yield return MMCoroutine.WaitFor(this.DelayForPoints);
			MMFeedbacks resetFeedback = this.ResetFeedback;
			if (resetFeedback != null)
			{
				resetFeedback.PlayFeedbacks();
			}
			yield return MMCoroutine.WaitFor(0.1f);
			this.BowlingBallRb.MovePosition(this._initialBallPosition);
			this.BowlingBallRb.transform.localRotation = this._initialBallRotation;
			this.BowlingBallRb.velocity = Vector3.zero;
			this.BowlingBallRb.angularVelocity = Vector3.zero;
			yield return MMCoroutine.WaitForFrames(1);
			this.BowlingBallRb.transform.position = this._initialBallPosition;
			this.BowlingBallLauncherWiggler.RotationActive = true;
			foreach (StrikePin strikePin in this._strikePins)
			{
				strikePin.ResetPin();
			}
			this._ballThrown = false;
			yield break;
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00069DD8 File Offset: 0x00067FD8
		protected virtual void CountPoints()
		{
			int num = 10;
			using (List<Collider>.Enumerator enumerator = this._pinColliders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.bounds.Intersects(this.PointsCollider.bounds))
					{
						num--;
					}
				}
			}
			this.LastScore = num;
			this.ConsecutiveStrikes = ((num == 10) ? (this.ConsecutiveStrikes + 1) : 0);
			if (num == 10)
			{
				MMFeedbacks strikeFeedback = this.StrikeFeedback;
				if (strikeFeedback != null)
				{
					strikeFeedback.PlayFeedbacks();
				}
				this.SetStrikeElements(true);
			}
			else
			{
				MMFeedbacks noStrikeFeedback = this.NoStrikeFeedback;
				if (noStrikeFeedback != null)
				{
					noStrikeFeedback.PlayFeedbacks();
				}
				this.SetStrikeElements(false);
			}
			this.TotalPoints += num;
			this.ConsecutiveStrikesText.text = this.ConsecutiveStrikes.ToString();
			this.LastScoreText.text = this.LastScore.ToString();
			this.TotalScoreText.text = this.TotalPoints.ToString();
		}

		// Token: 0x04001D0A RID: 7434
		[Header("Input")]
		[Tooltip("a key to use to throw the ball")]
		public KeyCode ActionKey = KeyCode.Space;

		// Token: 0x04001D0B RID: 7435
		[Tooltip("a secondary key to use to throw the ball")]
		public KeyCode ActionKeyAlt = KeyCode.Joystick1Button0;

		// Token: 0x04001D0C RID: 7436
		[Header("Bindings")]
		[Tooltip("the rigidbody of the bowling ball")]
		public Rigidbody BowlingBallRb;

		// Token: 0x04001D0D RID: 7437
		[Tooltip("a collider used to count points (still standing pins will overlap with it)")]
		public Collider PointsCollider;

		// Token: 0x04001D0E RID: 7438
		[Tooltip("the rigidbody of the pins")]
		public List<Rigidbody> Pins;

		// Token: 0x04001D0F RID: 7439
		[Tooltip("the wiggler that makes the launcher rotate")]
		public MMWiggle BowlingBallLauncherWiggler;

		// Token: 0x04001D10 RID: 7440
		[Tooltip("the text component used to display the current last score")]
		public Text LastScoreText;

		// Token: 0x04001D11 RID: 7441
		[Tooltip("the text component used to display the total score")]
		public Text TotalScoreText;

		// Token: 0x04001D12 RID: 7442
		[Tooltip("the text component used to display the number of consecutive strikes")]
		public Text ConsecutiveStrikesText;

		// Token: 0x04001D13 RID: 7443
		[Tooltip("a list of elements to turn on/off in case of strike")]
		public List<GameObject> StrikeElements;

		// Token: 0x04001D14 RID: 7444
		[Header("Settings")]
		[Tooltip("the force to apply when throwing the ball")]
		public Vector3 ThrowingForce = new Vector3(0f, 0f, 10f);

		// Token: 0x04001D15 RID: 7445
		[Tooltip("the gravity to apply")]
		public Vector3 Gravity = new Vector3(0f, -9.81f, 0f);

		// Token: 0x04001D16 RID: 7446
		[Tooltip("the max duration before a reset")]
		public float MaxDurationBeforeReset = 4f;

		// Token: 0x04001D17 RID: 7447
		[Tooltip("the delay to wait for (in seconds) before resetting the scene")]
		public float DelayBeforeReset = 1f;

		// Token: 0x04001D18 RID: 7448
		[Tooltip("the delay to wait for (in seconds) while counting/displaying points")]
		public float DelayForPoints = 1f;

		// Token: 0x04001D19 RID: 7449
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when throwing the ball")]
		public MMFeedbacks ThrowBallFeedback;

		// Token: 0x04001D1A RID: 7450
		[Tooltip("a feedback to call when resetting the scene")]
		public MMFeedbacks ResetFeedback;

		// Token: 0x04001D1B RID: 7451
		[Tooltip("a feedback played when hitting a strike")]
		public MMFeedbacks StrikeFeedback;

		// Token: 0x04001D1C RID: 7452
		[Tooltip("a feedback played when missing a strike")]
		public MMFeedbacks NoStrikeFeedback;

		// Token: 0x04001D1D RID: 7453
		[Header("Scores")]
		[Tooltip("the last score you hit")]
		[MMReadOnly]
		public int LastScore;

		// Token: 0x04001D1E RID: 7454
		[Tooltip("The total amount of points since the start")]
		[MMReadOnly]
		public int TotalPoints;

		// Token: 0x04001D1F RID: 7455
		[Tooltip("the amount of consecutive strikes")]
		[MMReadOnly]
		public int ConsecutiveStrikes;

		// Token: 0x04001D20 RID: 7456
		protected bool _ballThrown;

		// Token: 0x04001D21 RID: 7457
		protected Vector3 _initialBallPosition;

		// Token: 0x04001D22 RID: 7458
		protected Quaternion _initialBallRotation;

		// Token: 0x04001D23 RID: 7459
		protected List<StrikePin> _strikePins;

		// Token: 0x04001D24 RID: 7460
		protected List<Collider> _pinColliders;

		// Token: 0x04001D25 RID: 7461
		protected Coroutine _resetCoroutine;
	}
}
