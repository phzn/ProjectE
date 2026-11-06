using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000075 RID: 117
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("More Mountains/Tools/Performance/MMFPSCounter")]
	public class MMFPSCounter : MonoBehaviour
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000CC65 File Offset: 0x0000AE65
		protected virtual void Start()
		{
			if (base.GetComponent<Text>() == null)
			{
				Debug.LogWarning("FPSCounter requires a GUIText component.");
				return;
			}
			this._text = base.GetComponent<Text>();
			this._timeLeft = this.UpdateInterval;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000CC98 File Offset: 0x0000AE98
		protected virtual void Update()
		{
			this._framesDrawnInTheInterval += 1f;
			this._framesAccumulated += Time.timeScale / Time.deltaTime;
			this._timeLeft -= Time.deltaTime;
			if ((double)this._timeLeft <= 0.0)
			{
				this._currentFPS = (int)Mathf.Clamp(this._framesAccumulated / this._framesDrawnInTheInterval, 0f, 300f);
				this._framesDrawnInTheInterval = 0f;
				this._framesAccumulated = 0f;
				this._timeLeft = this.UpdateInterval;
				this._totalFrames++;
				this._average += (this._currentFPS - this._average) / this._totalFrames;
				if (this._currentFPS >= 0 && this._currentFPS <= 300)
				{
					switch (this.Mode)
					{
					case MMFPSCounter.Modes.Instant:
						this._text.text = MMFPSCounter._stringsFrom00To300[this._currentFPS];
						return;
					case MMFPSCounter.Modes.MovingAverage:
						this._text.text = MMFPSCounter._stringsFrom00To300[this._average];
						return;
					case MMFPSCounter.Modes.InstantAndMovingAverage:
						this._text.text = MMFPSCounter._stringsFrom00To300[this._currentFPS] + " / " + MMFPSCounter._stringsFrom00To300[this._average];
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x040003D9 RID: 985
		public float UpdateInterval = 0.3f;

		// Token: 0x040003DA RID: 986
		public MMFPSCounter.Modes Mode;

		// Token: 0x040003DB RID: 987
		protected float _framesAccumulated;

		// Token: 0x040003DC RID: 988
		protected float _framesDrawnInTheInterval;

		// Token: 0x040003DD RID: 989
		protected float _timeLeft;

		// Token: 0x040003DE RID: 990
		protected Text _text;

		// Token: 0x040003DF RID: 991
		protected int _currentFPS;

		// Token: 0x040003E0 RID: 992
		protected int _totalFrames;

		// Token: 0x040003E1 RID: 993
		protected int _average;

		// Token: 0x040003E2 RID: 994
		private static string[] _stringsFrom00To300 = new string[]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
			"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
			"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
			"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
			"90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
			"100", "101", "102", "103", "104", "105", "106", "107", "108", "109",
			"110", "111", "112", "113", "114", "115", "116", "117", "118", "119",
			"120", "121", "122", "123", "124", "125", "126", "127", "128", "129",
			"130", "131", "132", "133", "134", "135", "136", "137", "138", "139",
			"140", "141", "142", "143", "144", "145", "146", "147", "148", "149",
			"150", "151", "152", "153", "154", "155", "156", "157", "158", "159",
			"160", "161", "162", "163", "164", "165", "166", "167", "168", "169",
			"170", "171", "172", "173", "174", "175", "176", "177", "178", "179",
			"180", "181", "182", "183", "184", "185", "186", "187", "188", "189",
			"190", "191", "192", "193", "194", "195", "196", "197", "198", "199",
			"200", "201", "202", "203", "204", "205", "206", "207", "208", "209",
			"210", "211", "212", "213", "214", "215", "216", "217", "218", "219",
			"220", "221", "222", "223", "224", "225", "226", "227", "228", "229",
			"230", "231", "232", "233", "234", "235", "236", "237", "238", "239",
			"240", "241", "242", "243", "244", "245", "246", "247", "248", "249",
			"250", "251", "252", "253", "254", "255", "256", "257", "258", "259",
			"260", "261", "262", "263", "264", "265", "266", "267", "268", "269",
			"270", "271", "272", "273", "274", "275", "276", "277", "278", "279",
			"280", "281", "282", "283", "284", "285", "286", "287", "288", "289",
			"290", "291", "292", "293", "294", "295", "296", "297", "298", "299",
			"300"
		};

		// Token: 0x020003E7 RID: 999
		public enum Modes
		{
			// Token: 0x04001E5B RID: 7771
			Instant,
			// Token: 0x04001E5C RID: 7772
			MovingAverage,
			// Token: 0x04001E5D RID: 7773
			InstantAndMovingAverage
		}
	}
}
