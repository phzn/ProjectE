using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200004B RID: 75
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("More Mountains/Tools/Time/MMCountdown")]
	public class MMCountdown : MMMonoBehaviour
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00007A6B File Offset: 0x00005C6B
		private void DebugSetNewCurrentTime()
		{
			this.SetCurrentTime(this.DebugNewCurrentTime);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007A79 File Offset: 0x00005C79
		protected virtual void Start()
		{
			this._text = base.gameObject.GetComponent<Text>();
			this.Initialization();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007A94 File Offset: 0x00005C94
		protected virtual void Initialization()
		{
			this._lastUnitValue = (int)this.CurrentTime;
			this.Direction = ((this.CountdownFrom > this.CountdownTo) ? MMCountdown.MMCountdownDirections.Descending : MMCountdown.MMCountdownDirections.Ascending);
			this.CurrentTime = this.CountdownFrom;
			if (this.AutoStart)
			{
				this.StartCountdown();
			}
			foreach (MMCountdown.MMCountdownFloor mmcountdownFloor in this.Floors)
			{
				mmcountdownFloor.LastChangedAt = this.CountdownFrom;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007B2C File Offset: 0x00005D2C
		protected virtual void Update()
		{
			if (!this._countdowning)
			{
				return;
			}
			this.UpdateTime();
			this.UpdateText();
			this.CheckForFloors();
			this.CheckForEnd();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007B4F File Offset: 0x00005D4F
		protected virtual void UpdateTime()
		{
			if (this.Direction == MMCountdown.MMCountdownDirections.Descending)
			{
				this.CurrentTime -= Time.deltaTime * this.CountdownSpeed;
				return;
			}
			this.CurrentTime += Time.deltaTime * this.CountdownSpeed;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007B90 File Offset: 0x00005D90
		protected virtual void UpdateText()
		{
			if (Time.time - this._lastRefreshAt > this.RefreshFrequency)
			{
				if (this._text != null)
				{
					string text;
					if (this.FormatMethod == MMCountdown.FormatMethods.Explicit)
					{
						if (this.FloorValues)
						{
							text = Mathf.Floor(this.CurrentTime).ToString(this.Format);
						}
						else
						{
							text = this.CurrentTime.ToString(this.Format);
						}
					}
					else
					{
						text = MMTime.FloatToTimeString(this.CurrentTime, this.Hours, this.Minutes, this.Seconds, this.Milliseconds);
					}
					this._text.text = text;
				}
				if (this.CountdownRefreshEvent != null)
				{
					this.CountdownRefreshEvent.Invoke();
				}
				this._lastRefreshAt = Time.time;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007C58 File Offset: 0x00005E58
		protected virtual void CheckForEnd()
		{
			if (this.Infinite)
			{
				return;
			}
			if ((this.Direction == MMCountdown.MMCountdownDirections.Ascending) ? (this.CurrentTime >= this.CountdownTo) : (this.CurrentTime <= this.CountdownTo))
			{
				if (this.CountdownCompleteEvent != null)
				{
					this.CountdownCompleteEvent.Invoke();
				}
				if (this.PingPong)
				{
					this.Direction = ((this.Direction == MMCountdown.MMCountdownDirections.Ascending) ? MMCountdown.MMCountdownDirections.Descending : MMCountdown.MMCountdownDirections.Ascending);
					this._countdowning = true;
					float countdownFrom = this.CountdownFrom;
					this.CountdownFrom = this.CountdownTo;
					this.CountdownTo = countdownFrom;
					return;
				}
				if (this.AutoReset)
				{
					this._countdowning = true;
					this.CurrentTime = this.CountdownFrom;
					return;
				}
				this.CurrentTime = this.CountdownTo;
				this._countdowning = false;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007D1C File Offset: 0x00005F1C
		protected virtual void CheckForFloors()
		{
			foreach (MMCountdown.MMCountdownFloor mmcountdownFloor in this.Floors)
			{
				if (Mathf.Abs(this.CurrentTime - mmcountdownFloor.LastChangedAt) >= mmcountdownFloor.FloorValue)
				{
					if (mmcountdownFloor.FloorEvent != null)
					{
						mmcountdownFloor.FloorEvent.Invoke();
					}
					if (this.Direction == MMCountdown.MMCountdownDirections.Descending)
					{
						if (mmcountdownFloor.LastChangedAt == this.CountdownFrom)
						{
							mmcountdownFloor.LastChangedAt = this.CountdownFrom - mmcountdownFloor.FloorValue;
						}
						else
						{
							mmcountdownFloor.LastChangedAt -= mmcountdownFloor.FloorValue;
						}
					}
					else if (mmcountdownFloor.LastChangedAt == this.CountdownFrom)
					{
						mmcountdownFloor.LastChangedAt = this.CountdownFrom + mmcountdownFloor.FloorValue;
					}
					else
					{
						mmcountdownFloor.LastChangedAt += mmcountdownFloor.FloorValue;
					}
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007E18 File Offset: 0x00006018
		public virtual void StartCountdown()
		{
			this._countdowning = true;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007E21 File Offset: 0x00006021
		public virtual void StopCountdown()
		{
			this._countdowning = false;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007E2A File Offset: 0x0000602A
		public virtual void ResetCountdown()
		{
			this.CurrentTime = this.CountdownFrom;
			this.Initialization();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007E40 File Offset: 0x00006040
		public virtual void ChangeDirection()
		{
			this.Direction = ((this.Direction == MMCountdown.MMCountdownDirections.Descending) ? MMCountdown.MMCountdownDirections.Ascending : MMCountdown.MMCountdownDirections.Descending);
			float countdownTo = this.CountdownTo;
			float countdownFrom = this.CountdownFrom;
			this.CountdownFrom = countdownTo;
			this.CountdownTo = countdownFrom;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007E7C File Offset: 0x0000607C
		public virtual void SetCurrentTime(float newCurrentTime)
		{
			this.CurrentTime = newCurrentTime;
		}

		// Token: 0x04000214 RID: 532
		[MMInspectorGroup("Countdown", true, 18, false)]
		[MMInformation("You can define the bounds of the countdown (how much it should count down from, and to how much, the format it should be displayed in (standard Unity float ToString formatting).", MMInformationAttribute.InformationType.Info, false)]
		public float CountdownFrom = 60f;

		// Token: 0x04000215 RID: 533
		public float CountdownTo;

		// Token: 0x04000216 RID: 534
		public bool Infinite;

		// Token: 0x04000217 RID: 535
		[MMInspectorGroup("Display", true, 19, false)]
		public MMCountdown.FormatMethods FormatMethod = MMCountdown.FormatMethods.Choices;

		// Token: 0x04000218 RID: 536
		[MMEnumCondition("FormatMethod", new int[] { 0 })]
		public bool FloorValues = true;

		// Token: 0x04000219 RID: 537
		[MMEnumCondition("FormatMethod", new int[] { 0 })]
		public string Format = "00.00";

		// Token: 0x0400021A RID: 538
		[MMEnumCondition("FormatMethod", new int[] { 1 })]
		public bool Hours;

		// Token: 0x0400021B RID: 539
		[MMEnumCondition("FormatMethod", new int[] { 1 })]
		public bool Minutes = true;

		// Token: 0x0400021C RID: 540
		[MMEnumCondition("FormatMethod", new int[] { 1 })]
		public bool Seconds = true;

		// Token: 0x0400021D RID: 541
		[MMEnumCondition("FormatMethod", new int[] { 1 })]
		public bool Milliseconds;

		// Token: 0x0400021E RID: 542
		[MMInspectorGroup("Settings", true, 20, false)]
		[MMInformation("You can choose whether or not the countdown should automatically start on its Start, at what frequency (in seconds) it should refresh (0 means every frame), and the countdown's speed multiplier (2 will be twice as fast, 0.5 half normal speed, etc). Floors are used to define and trigger events when certain floors are reached. For each floor, define a floor value (in seconds). Everytime this floor gets reached, the corresponding event will be triggered.Bind events here to trigger them when the countdown reaches its To destination, or every time it gets refreshed.", MMInformationAttribute.InformationType.Info, false)]
		public bool AutoStart = true;

		// Token: 0x0400021F RID: 543
		public bool AutoReset;

		// Token: 0x04000220 RID: 544
		public bool PingPong;

		// Token: 0x04000221 RID: 545
		public float RefreshFrequency = 0.02f;

		// Token: 0x04000222 RID: 546
		public float CountdownSpeed = 1f;

		// Token: 0x04000223 RID: 547
		[MMInspectorGroup("Floors", true, 21, false)]
		public List<MMCountdown.MMCountdownFloor> Floors;

		// Token: 0x04000224 RID: 548
		[MMInspectorGroup("Events", true, 22, false)]
		public UnityEvent CountdownCompleteEvent;

		// Token: 0x04000225 RID: 549
		public UnityEvent CountdownRefreshEvent;

		// Token: 0x04000226 RID: 550
		[MMInspectorGroup("Debug", true, 17, false)]
		[MMReadOnly]
		public float CurrentTime;

		// Token: 0x04000227 RID: 551
		[MMReadOnly]
		public MMCountdown.MMCountdownDirections Direction;

		// Token: 0x04000228 RID: 552
		[MMInspectorButton("StopCountdown")]
		public bool StopCountdownButton;

		// Token: 0x04000229 RID: 553
		[MMInspectorButton("StartCountdown")]
		public bool StartCountdownButton;

		// Token: 0x0400022A RID: 554
		[MMInspectorButton("ResetCountdown")]
		public bool ResetCountdownButton;

		// Token: 0x0400022B RID: 555
		[MMInspectorButton("ChangeDirection")]
		public bool ChangeDirectionButton;

		// Token: 0x0400022C RID: 556
		public float DebugNewCurrentTime = 5f;

		// Token: 0x0400022D RID: 557
		[MMInspectorButton("DebugSetNewCurrentTime")]
		public bool DebugSetNewCurrentTimeButton;

		// Token: 0x0400022E RID: 558
		protected Text _text;

		// Token: 0x0400022F RID: 559
		protected float _lastRefreshAt;

		// Token: 0x04000230 RID: 560
		protected bool _countdowning;

		// Token: 0x04000231 RID: 561
		protected int _lastUnitValue;

		// Token: 0x020003BE RID: 958
		[Serializable]
		public class MMCountdownFloor
		{
			// Token: 0x04001DC3 RID: 7619
			public float FloorValue;

			// Token: 0x04001DC4 RID: 7620
			[MMReadOnly]
			public float LastChangedAt;

			// Token: 0x04001DC5 RID: 7621
			public UnityEvent FloorEvent;
		}

		// Token: 0x020003BF RID: 959
		public enum MMCountdownDirections
		{
			// Token: 0x04001DC7 RID: 7623
			Ascending,
			// Token: 0x04001DC8 RID: 7624
			Descending
		}

		// Token: 0x020003C0 RID: 960
		public enum FormatMethods
		{
			// Token: 0x04001DCA RID: 7626
			Explicit,
			// Token: 0x04001DCB RID: 7627
			Choices
		}
	}
}
