using System;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003CF RID: 975
	public class CalculatorManager : MonoBehaviour
	{
		// Token: 0x060018F3 RID: 6387 RVA: 0x0009E8C8 File Offset: 0x0009CAC8
		private void Start()
		{
			this.buttonTapped('c');
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00004F31 File Offset: 0x00003131
		private void Update()
		{
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0009E8D4 File Offset: 0x0009CAD4
		private void clearCalc()
		{
			this.digitLabel.text = "0";
			this.operatorLabel.text = "";
			this.specialAction = (this.displayValid = (this.errorDisplayed = false));
			this.currentVal = (this.result = (this.storedVal = 0.0));
			this.storedOperator = ' ';
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0009E943 File Offset: 0x0009CB43
		private void updateDigitLabel()
		{
			if (!this.errorDisplayed)
			{
				this.digitLabel.text = this.currentVal.ToString();
			}
			this.displayValid = false;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0009E96C File Offset: 0x0009CB6C
		private void calcResult(char activeOp)
		{
			if (activeOp <= '-')
			{
				if (activeOp == '+')
				{
					this.result = this.storedVal + this.currentVal;
					goto IL_00D0;
				}
				if (activeOp == '-')
				{
					this.result = this.storedVal - this.currentVal;
					goto IL_00D0;
				}
			}
			else
			{
				if (activeOp == '=')
				{
					this.result = this.currentVal;
					goto IL_00D0;
				}
				if (activeOp == 'x')
				{
					this.result = this.storedVal * this.currentVal;
					goto IL_00D0;
				}
				if (activeOp == '÷')
				{
					if (this.currentVal != 0.0)
					{
						this.result = this.storedVal / this.currentVal;
						goto IL_00D0;
					}
					this.errorDisplayed = true;
					this.digitLabel.text = "ERROR";
					goto IL_00D0;
				}
			}
			Debug.Log("unknown: " + activeOp.ToString());
			IL_00D0:
			this.currentVal = this.result;
			this.updateDigitLabel();
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0009EA5C File Offset: 0x0009CC5C
		public void buttonTapped(char caption)
		{
			if (this.errorDisplayed)
			{
				this.clearCalc();
			}
			if ((caption >= '0' && caption <= '9') || caption == '.')
			{
				if (this.digitLabel.text.Length < 15 || !this.displayValid)
				{
					if (!this.displayValid)
					{
						this.digitLabel.text = ((caption == '.') ? "0" : "");
					}
					else if (this.digitLabel.text == "0" && caption != '.')
					{
						this.digitLabel.text = "";
					}
					TMP_Text tmp_Text = this.digitLabel;
					tmp_Text.text += caption.ToString();
					this.displayValid = true;
					return;
				}
			}
			else
			{
				if (caption == 'c')
				{
					this.clearCalc();
					return;
				}
				if (caption == '±')
				{
					this.currentVal = -double.Parse(this.digitLabel.text);
					this.updateDigitLabel();
					this.specialAction = true;
					return;
				}
				if (caption == '%')
				{
					this.currentVal = double.Parse(this.digitLabel.text) / 100.0;
					this.updateDigitLabel();
					this.specialAction = true;
					return;
				}
				if (this.displayValid || this.storedOperator == '=' || this.specialAction)
				{
					this.currentVal = double.Parse(this.digitLabel.text);
					this.displayValid = false;
					if (this.storedOperator != ' ')
					{
						this.calcResult(this.storedOperator);
						this.storedOperator = ' ';
					}
					this.operatorLabel.text = caption.ToString();
					this.storedOperator = caption;
					this.storedVal = this.currentVal;
					this.updateDigitLabel();
					this.specialAction = false;
				}
			}
		}

		// Token: 0x040019F0 RID: 6640
		public TMP_Text digitLabel;

		// Token: 0x040019F1 RID: 6641
		public TMP_Text operatorLabel;

		// Token: 0x040019F2 RID: 6642
		private bool errorDisplayed;

		// Token: 0x040019F3 RID: 6643
		private bool displayValid;

		// Token: 0x040019F4 RID: 6644
		private bool specialAction;

		// Token: 0x040019F5 RID: 6645
		private double currentVal;

		// Token: 0x040019F6 RID: 6646
		private double storedVal;

		// Token: 0x040019F7 RID: 6647
		private double result;

		// Token: 0x040019F8 RID: 6648
		private char storedOperator;
	}
}
