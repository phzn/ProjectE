using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C5 RID: 709
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you reveal words, lines, or characters in a target TMP, one at a time")]
	[FeedbackPath("TextMesh Pro/TMP Text Reveal")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPTextReveal : MMF_Feedback
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x000566AC File Offset: 0x000548AC
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x000566AF File Offset: 0x000548AF
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x000566C0 File Offset: 0x000548C0
		// (set) Token: 0x06001415 RID: 5141 RVA: 0x000568A4 File Offset: 0x00054AA4
		public override float FeedbackDuration
		{
			get
			{
				if (this.DurationMode == MMF_TMPTextReveal.DurationModes.TotalDuration)
				{
					return this.RevealDuration;
				}
				if (this.TargetTMPText == null)
				{
					return 0f;
				}
				if (this.TargetTMPText.textInfo == null)
				{
					bool activeSelf = this.TargetTMPText.gameObject.activeSelf;
					this.TargetTMPText.gameObject.SetActive(true);
					this.TargetTMPText.ForceMeshUpdate(true, false);
					this.TargetTMPText.gameObject.SetActive(activeSelf);
				}
				if (this.AllowHierarchyActivationForDurationComputation)
				{
					List<Transform> list = (from p in this.TargetTMPText.transform.MMEnumerateAllParents(true)
						where !p.gameObject.activeSelf
						select p).ToList<Transform>();
					list.ForEach(delegate(Transform p)
					{
						p.gameObject.SetActive(true);
					});
					this.TargetTMPText.ForceMeshUpdate(true, false);
					list.ForEach(delegate(Transform p)
					{
						p.gameObject.SetActive(false);
					});
				}
				if (this.TargetTMPText.textInfo == null)
				{
					return 0f;
				}
				float num = 0f;
				if (this.ReplaceText)
				{
					this._originalText = this.TargetTMPText.text;
					this.TargetTMPText.text = this.NewText;
				}
				switch (this.RevealMode)
				{
				case MMF_TMPTextReveal.RevealModes.Character:
					num = (float)this.RichTextLength(this.TargetTMPText.text) * this.IntervalBetweenReveals;
					break;
				case MMF_TMPTextReveal.RevealModes.Lines:
					num = (float)this.TargetTMPText.textInfo.lineCount * this.IntervalBetweenReveals;
					break;
				case MMF_TMPTextReveal.RevealModes.Words:
					num = (float)this.TargetTMPText.textInfo.wordCount * this.IntervalBetweenReveals;
					break;
				}
				if (this.ReplaceText)
				{
					this.TargetTMPText.text = this._originalText;
				}
				return num;
			}
			set
			{
				if (this.DurationMode == MMF_TMPTextReveal.DurationModes.TotalDuration)
				{
					this.RevealDuration = value;
					return;
				}
				if (this.TargetTMPText != null)
				{
					if (this.ReplaceText)
					{
						this._originalText = this.TargetTMPText.text;
						this.TargetTMPText.text = this.NewText;
					}
					switch (this.RevealMode)
					{
					case MMF_TMPTextReveal.RevealModes.Character:
						this.IntervalBetweenReveals = value / (float)this.RichTextLength(this.TargetTMPText.text);
						break;
					case MMF_TMPTextReveal.RevealModes.Lines:
						this.IntervalBetweenReveals = value / (float)this.TargetTMPText.textInfo.lineCount;
						break;
					case MMF_TMPTextReveal.RevealModes.Words:
						this.IntervalBetweenReveals = value / (float)this.TargetTMPText.textInfo.wordCount;
						break;
					}
					if (this.ReplaceText)
					{
						this.TargetTMPText.text = this._originalText;
					}
				}
			}
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00056984 File Offset: 0x00054B84
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPTextReveal.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			this._initialText = this.TargetTMPText.text;
			this._textInfo = this.TargetTMPText.textInfo;
			if (this.ReplaceText)
			{
				this.TargetTMPText.text = this.NewText;
				this.TargetTMPText.ForceMeshUpdate(false, false);
			}
			this._richTextLength = this.RichTextLength(this.TargetTMPText.text);
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			switch (this.RevealMode)
			{
			case MMF_TMPTextReveal.RevealModes.Character:
				this._delay = ((this.DurationMode == MMF_TMPTextReveal.DurationModes.Interval) ? this.IntervalBetweenReveals : (this.RevealDuration / (float)this._richTextLength));
				this.TargetTMPText.maxVisibleCharacters = 0;
				this._coroutine = this.Owner.StartCoroutine(this.RevealCharacters());
				return;
			case MMF_TMPTextReveal.RevealModes.Lines:
				this._delay = ((this.DurationMode == MMF_TMPTextReveal.DurationModes.Interval) ? this.IntervalBetweenReveals : (this.RevealDuration / (float)this.TargetTMPText.textInfo.lineCount));
				this.TargetTMPText.maxVisibleLines = 0;
				this._coroutine = this.Owner.StartCoroutine(this.RevealLines());
				return;
			case MMF_TMPTextReveal.RevealModes.Words:
				this._delay = ((this.DurationMode == MMF_TMPTextReveal.DurationModes.Interval) ? this.IntervalBetweenReveals : (this.RevealDuration / (float)this.TargetTMPText.textInfo.wordCount));
				this.TargetTMPText.maxVisibleWords = 0;
				this._coroutine = this.Owner.StartCoroutine(this.RevealWords());
				return;
			default:
				return;
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00056B2D File Offset: 0x00054D2D
		protected virtual IEnumerator RevealCharacters()
		{
			float startTime = this.FeedbackTime;
			this._totalCharacters = this._richTextLength;
			int visibleCharacters = 0;
			float lastCharAt = 0f;
			this.IsPlaying = true;
			while (visibleCharacters <= this._totalCharacters && !this.Owner.SkippingToTheEnd)
			{
				float time = this.FeedbackTime;
				if (time - lastCharAt < this.IntervalBetweenReveals)
				{
					yield return null;
				}
				this.TargetTMPText.maxVisibleCharacters = visibleCharacters;
				this.InvokeRevealEvents();
				int num = visibleCharacters;
				visibleCharacters = num + 1;
				lastCharAt = time;
				float num2;
				if (this.DurationMode == MMF_TMPTextReveal.DurationModes.Interval)
				{
					this._delay = Mathf.Max(this.IntervalBetweenReveals, this.FeedbackDeltaTime);
					num2 = this._delay - this.FeedbackDeltaTime;
				}
				else
				{
					int num3 = this._totalCharacters - visibleCharacters;
					float num4 = time - startTime;
					if (num3 != 0)
					{
						this._delay = (this.RevealDuration - num4) / (float)num3;
					}
					num2 = this._delay - this.FeedbackDeltaTime;
				}
				yield return this.WaitFor(num2);
			}
			this.TargetTMPText.maxVisibleCharacters = this._richTextLength;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00056B3C File Offset: 0x00054D3C
		protected virtual IEnumerator RevealLines()
		{
			this._totalLines = this.TargetTMPText.textInfo.lineCount;
			int visibleLines = 0;
			this.IsPlaying = true;
			while (visibleLines <= this._totalLines && !this.Owner.SkippingToTheEnd)
			{
				this.TargetTMPText.maxVisibleLines = visibleLines;
				this.InvokeRevealEvents();
				int num = visibleLines;
				visibleLines = num + 1;
				yield return this.WaitFor(this._delay);
			}
			this.TargetTMPText.maxVisibleLines = this._totalLines;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00056B4B File Offset: 0x00054D4B
		protected virtual IEnumerator RevealWords()
		{
			this._totalWords = this.TargetTMPText.textInfo.wordCount;
			int visibleWords = 0;
			this.IsPlaying = true;
			while (visibleWords <= this._totalWords && !this.Owner.SkippingToTheEnd)
			{
				this.TargetTMPText.maxVisibleWords = visibleWords;
				this.InvokeRevealEvents();
				int num = visibleWords;
				visibleWords = num + 1;
				yield return this.WaitFor(this._delay);
			}
			this.TargetTMPText.maxVisibleWords = this._totalWords;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00056B5C File Offset: 0x00054D5C
		protected virtual void InvokeRevealEvents()
		{
			if ((this.RevealMode == MMF_TMPTextReveal.RevealModes.Character && this.TargetTMPText.maxVisibleCharacters == 0) || (this.RevealMode == MMF_TMPTextReveal.RevealModes.Character && !this.IsNewVisibleCharacter()) || (this.RevealMode == MMF_TMPTextReveal.RevealModes.Lines && this.TargetTMPText.maxVisibleLines == 0) || (this.RevealMode == MMF_TMPTextReveal.RevealModes.Words && this.TargetTMPText.maxVisibleWords == 0))
			{
				return;
			}
			UnityEvent onReveal = this.OnReveal;
			if (onReveal == null)
			{
				return;
			}
			onReveal.Invoke();
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00056BCC File Offset: 0x00054DCC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPTextReveal.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00056C18 File Offset: 0x00054E18
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.IsPlaying)
			{
				return;
			}
			switch (this.RevealMode)
			{
			case MMF_TMPTextReveal.RevealModes.Character:
				this.TargetTMPText.maxVisibleCharacters = this._totalCharacters;
				return;
			case MMF_TMPTextReveal.RevealModes.Lines:
				this.TargetTMPText.maxVisibleLines = this._totalLines;
				return;
			case MMF_TMPTextReveal.RevealModes.Words:
				this.TargetTMPText.maxVisibleWords = this._totalWords;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00056C80 File Offset: 0x00054E80
		protected int RichTextLength(string richText)
		{
			int num = 0;
			bool flag = false;
			richText = richText.Replace("<br>", "-");
			foreach (char c in richText)
			{
				if (c == '<')
				{
					flag = true;
				}
				else if (c == '>')
				{
					flag = false;
				}
				else if (!flag)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00056CDC File Offset: 0x00054EDC
		protected virtual bool IsNewVisibleCharacter()
		{
			int num = -1;
			this._textInfo = this.TargetTMPText.GetTextInfo(this.TargetTMPText.text);
			for (int i = 0; i < this._textInfo.characterCount; i++)
			{
				if (this._textInfo.characterInfo[i].isVisible)
				{
					num = i;
				}
			}
			if (num < 0 || num > this.TargetTMPText.text.Length || num == this._indexLastTime)
			{
				return false;
			}
			this._indexLastTime = num;
			return char.IsLetterOrDigit(this._textInfo.characterInfo[num].character);
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00056D7B File Offset: 0x00054F7B
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPTextReveal.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.text = this._initialText;
		}

		// Token: 0x0400171A RID: 5914
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400171B RID: 5915
		protected string _originalText;

		// Token: 0x0400171C RID: 5916
		protected TMP_TextInfo _textInfo;

		// Token: 0x0400171D RID: 5917
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the target TMP_Text component we want to change the text on")]
		public TMP_Text TargetTMPText;

		// Token: 0x0400171E RID: 5918
		[MMFInspectorGroup("Change Text", true, 13, false, false)]
		[Tooltip("whether or not to replace the current TMP target's text on play")]
		public bool ReplaceText;

		// Token: 0x0400171F RID: 5919
		[Tooltip("the new text to replace the old one with")]
		[TextArea]
		public string NewText = "Hello World";

		// Token: 0x04001720 RID: 5920
		[MMFInspectorGroup("Reveal", true, 14, false, false)]
		[Tooltip("the selected way to reveal the text (character by character, word by word, or line by line)")]
		public MMF_TMPTextReveal.RevealModes RevealMode;

		// Token: 0x04001721 RID: 5921
		[Tooltip("whether to define duration by the time interval between two unit reveals, or by the total duration the reveal should take")]
		public MMF_TMPTextReveal.DurationModes DurationMode;

		// Token: 0x04001722 RID: 5922
		[Tooltip("the interval (in seconds) between two reveals")]
		[MMFEnumCondition("DurationMode", new int[] { 0 })]
		public float IntervalBetweenReveals = 0.05f;

		// Token: 0x04001723 RID: 5923
		[Tooltip("the total duration of the text reveal, in seconds")]
		[MMFEnumCondition("DurationMode", new int[] { 1 })]
		public float RevealDuration = 1f;

		// Token: 0x04001724 RID: 5924
		[Tooltip("a UnityEvent to invoke every time a reveal happens (word, line or character)")]
		public UnityEvent OnReveal;

		// Token: 0x04001725 RID: 5925
		[Tooltip("alright so that one will be weird : for reasons, TextMeshPro won't let you read the length of a disabled text, so to do so, we need to enable it, even if it's just to disable it again right after. If you're targeting a disabled text, or a text that is part of a disabled hierarchy, you'll probably want to set this to true so that the system can proceed with accurate duration computation. If you don't, and your target transform is disabled, duration won't be computed correctly.")]
		public bool AllowHierarchyActivationForDurationComputation;

		// Token: 0x04001726 RID: 5926
		protected float _delay;

		// Token: 0x04001727 RID: 5927
		protected Coroutine _coroutine;

		// Token: 0x04001728 RID: 5928
		protected int _richTextLength;

		// Token: 0x04001729 RID: 5929
		protected int _totalCharacters;

		// Token: 0x0400172A RID: 5930
		protected int _totalLines;

		// Token: 0x0400172B RID: 5931
		protected int _totalWords;

		// Token: 0x0400172C RID: 5932
		protected string _initialText;

		// Token: 0x0400172D RID: 5933
		protected int _indexLastTime = -1;

		// Token: 0x02000552 RID: 1362
		public enum RevealModes
		{
			// Token: 0x0400246A RID: 9322
			Character,
			// Token: 0x0400246B RID: 9323
			Lines,
			// Token: 0x0400246C RID: 9324
			Words
		}

		// Token: 0x02000553 RID: 1363
		public enum DurationModes
		{
			// Token: 0x0400246E RID: 9326
			Interval,
			// Token: 0x0400246F RID: 9327
			TotalDuration
		}
	}
}
