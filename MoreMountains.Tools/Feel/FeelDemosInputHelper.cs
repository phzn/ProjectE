using System;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200038C RID: 908
	public static class FeelDemosInputHelper
	{
		// Token: 0x060018CA RID: 6346 RVA: 0x00069714 File Offset: 0x00067914
		public static bool CheckMainActionInputPressedThisFrame()
		{
			return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0);
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00069733 File Offset: 0x00067933
		public static bool CheckMainActionInputPressed()
		{
			return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0) || Input.GetMouseButton(0);
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00069752 File Offset: 0x00067952
		public static bool CheckMainActionInputUpThisFrame()
		{
			return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetMouseButtonUp(0);
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00069771 File Offset: 0x00067971
		public static bool CheckEnterPressedThisFrame()
		{
			return Input.GetKeyDown(KeyCode.Return);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0006977A File Offset: 0x0006797A
		public static bool CheckMouseDown()
		{
			return Input.GetMouseButtonUp(0);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00069782 File Offset: 0x00067982
		public static Vector2 MousePosition()
		{
			return Input.mousePosition;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0006978E File Offset: 0x0006798E
		public static Vector2 GetDirectionAxis(ref Vector2 direction)
		{
			direction.x = 0f;
			direction.y = 0f;
			direction.x = Input.GetAxis("Horizontal");
			direction.y = Input.GetAxis("Vertical");
			return direction;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x000697CC File Offset: 0x000679CC
		public static bool CheckAlphaInputPressedThisFrame(int alpha)
		{
			bool flag = false;
			switch (alpha)
			{
			case 1:
				flag = Input.GetKeyDown(KeyCode.Alpha1);
				break;
			case 2:
				flag = Input.GetKeyDown(KeyCode.Alpha2);
				break;
			case 3:
				flag = Input.GetKeyDown(KeyCode.Alpha3);
				break;
			case 4:
				flag = Input.GetKeyDown(KeyCode.Alpha4);
				break;
			}
			return flag;
		}

		// Token: 0x04001CF2 RID: 7410
		private const string _horizontalAxis = "Horizontal";

		// Token: 0x04001CF3 RID: 7411
		private const string _verticalAxis = "Vertical";
	}
}
