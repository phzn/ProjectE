using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000223 RID: 547
	[CreateAssetMenu(menuName = "MoreMountains/Sequencer/MMSequence")]
	public class MMSequence : ScriptableObject
	{
		// Token: 0x06000EFE RID: 3838 RVA: 0x00042F24 File Offset: 0x00041124
		private static int SortByTimestamp(MMSequenceNote p1, MMSequenceNote p2)
		{
			return p1.Timestamp.CompareTo(p2.Timestamp);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00042F37 File Offset: 0x00041137
		public virtual void SortOriginalSequence()
		{
			this.OriginalSequence.Line.Sort(new Comparison<MMSequenceNote>(MMSequence.SortByTimestamp));
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00042F55 File Offset: 0x00041155
		public virtual void QuantizeOriginalSequence()
		{
			this.ComputeLength();
			this.QuantizeSequenceToBPM(this.OriginalSequence.Line);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00042F6E File Offset: 0x0004116E
		public virtual void ComputeLength()
		{
			this.Length = this.OriginalSequence.Line[this.OriginalSequence.Line.Count - 1].Timestamp + this.EndSilenceDuration;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00042FA4 File Offset: 0x000411A4
		public virtual void QuantizeSequenceToBPM(List<MMSequenceNote> baseSequence)
		{
			float length = this.Length;
			float num = 60f / (float)this.TargetBPM;
			int num2 = (int)(length / num);
			this.QuantizedSequence = new List<MMSequenceList>();
			this._deleteList = new List<MMSequenceNote>();
			this._deleteList.Clear();
			this._quantizedBeats = new float[num2];
			for (int i = 0; i < num2; i++)
			{
				this._quantizedBeats[i] = (float)i * num;
			}
			for (int j = 0; j < this.SequenceTracks.Count; j++)
			{
				this.QuantizedSequence.Add(new MMSequenceList());
				this.QuantizedSequence[j].Line = new List<MMSequenceNote>();
				for (int k = 0; k < num2; k++)
				{
					MMSequenceNote mmsequenceNote = new MMSequenceNote();
					mmsequenceNote.ID = -1;
					mmsequenceNote.Timestamp = this._quantizedBeats[k];
					this.QuantizedSequence[j].Line.Add(mmsequenceNote);
					foreach (MMSequenceNote mmsequenceNote2 in baseSequence)
					{
						if (MMSequence.RoundFloatToArray(mmsequenceNote2.Timestamp, this._quantizedBeats) == this._quantizedBeats[k] && mmsequenceNote2.ID == this.SequenceTracks[j].ID)
						{
							this.QuantizedSequence[j].Line[k].ID = mmsequenceNote2.ID;
						}
					}
				}
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00043134 File Offset: 0x00041334
		protected virtual void OnValidate()
		{
			for (int i = 0; i < this.SequenceTracks.Count; i++)
			{
				this.SequenceTracks[i].SetDefaults(i);
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0004316C File Offset: 0x0004136C
		protected virtual void RandomizeTrackColors()
		{
			foreach (MMSequenceTrack mmsequenceTrack in this.SequenceTracks)
			{
				mmsequenceTrack.TrackColor = MMSequence.RandomSequenceColor();
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000431C4 File Offset: 0x000413C4
		public static Color RandomSequenceColor()
		{
			switch (Random.Range(0, 32))
			{
			case 0:
				return new Color32(240, 248, byte.MaxValue, byte.MaxValue);
			case 1:
				return new Color32(127, byte.MaxValue, 212, byte.MaxValue);
			case 2:
				return new Color32(245, 245, 220, byte.MaxValue);
			case 3:
				return new Color32(95, 158, 160, byte.MaxValue);
			case 4:
				return new Color32(byte.MaxValue, 127, 80, byte.MaxValue);
			case 5:
				return new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			case 6:
				return new Color32(byte.MaxValue, 215, 0, byte.MaxValue);
			case 7:
				return new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);
			case 8:
				return new Color32(50, 128, 120, byte.MaxValue);
			case 9:
				return new Color32(173, byte.MaxValue, 47, byte.MaxValue);
			case 10:
				return new Color32(byte.MaxValue, 105, 180, byte.MaxValue);
			case 11:
				return new Color32(75, 0, 130, byte.MaxValue);
			case 12:
				return new Color32(byte.MaxValue, byte.MaxValue, 240, byte.MaxValue);
			case 13:
				return new Color32(124, 252, 0, byte.MaxValue);
			case 14:
				return new Color32(byte.MaxValue, 160, 122, byte.MaxValue);
			case 15:
				return new Color32(0, byte.MaxValue, 0, byte.MaxValue);
			case 16:
				return new Color32(245, byte.MaxValue, 250, byte.MaxValue);
			case 17:
				return new Color32(byte.MaxValue, 228, 225, byte.MaxValue);
			case 18:
				return new Color32(218, 112, 214, byte.MaxValue);
			case 19:
				return new Color32(byte.MaxValue, 192, 203, byte.MaxValue);
			case 20:
				return new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			case 21:
				return new Color32(196, 112, byte.MaxValue, byte.MaxValue);
			case 22:
				return new Color32(250, 128, 114, byte.MaxValue);
			case 23:
				return new Color32(46, 139, 87, byte.MaxValue);
			case 24:
				return new Color32(192, 192, 192, byte.MaxValue);
			case 25:
				return new Color32(135, 206, 235, byte.MaxValue);
			case 26:
				return new Color32(0, byte.MaxValue, 127, byte.MaxValue);
			case 27:
				return new Color32(210, 180, 140, byte.MaxValue);
			case 28:
				return new Color32(0, 128, 128, byte.MaxValue);
			case 29:
				return new Color32(byte.MaxValue, 99, 71, byte.MaxValue);
			case 30:
				return new Color32(64, 224, 208, byte.MaxValue);
			case 31:
				return new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
			case 32:
				return new Color32(154, 205, 50, byte.MaxValue);
			default:
				return new Color32(240, 248, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00043618 File Offset: 0x00041818
		public static float RoundFloatToArray(float value, float[] array)
		{
			int num = 0;
			if (array[num] >= value)
			{
				return array[num];
			}
			int num2 = array.Length - 1;
			if (array[num2] <= value)
			{
				return array[num2];
			}
			while (num2 - num > 1)
			{
				int num3 = (num2 + num) / 2;
				if (array[num3] == value)
				{
					return array[num3];
				}
				if (array[num3] < value)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			if (array[num2] - value <= value - array[num])
			{
				return array[num2];
			}
			return array[num];
		}

		// Token: 0x04001196 RID: 4502
		[Header("Sequence")]
		[Tooltip("the length (in seconds) of the sequence")]
		[MMFReadOnly]
		public float Length;

		// Token: 0x04001197 RID: 4503
		[Tooltip("the original sequence (as outputted by the input sequence recorder)")]
		public MMSequenceList OriginalSequence;

		// Token: 0x04001198 RID: 4504
		[Tooltip("the duration in seconds to apply after the last input")]
		public float EndSilenceDuration;

		// Token: 0x04001199 RID: 4505
		[Header("Sequence Contents")]
		[Tooltip("the list of tracks for this sequence")]
		public List<MMSequenceTrack> SequenceTracks;

		// Token: 0x0400119A RID: 4506
		[Header("Quantizing")]
		[Tooltip("whether this sequence should be used in quantized form or not")]
		public bool Quantized;

		// Token: 0x0400119B RID: 4507
		[Tooltip("the target BPM for this sequence")]
		public int TargetBPM = 120;

		// Token: 0x0400119C RID: 4508
		[Tooltip("the contents of the quantized sequence")]
		public List<MMSequenceList> QuantizedSequence;

		// Token: 0x0400119D RID: 4509
		[Space]
		[Header("Controls")]
		[MMFInspectorButton("RandomizeTrackColors")]
		public bool RandomizeTrackColorsButton;

		// Token: 0x0400119E RID: 4510
		protected float[] _quantizedBeats;

		// Token: 0x0400119F RID: 4511
		protected List<MMSequenceNote> _deleteList;
	}
}
