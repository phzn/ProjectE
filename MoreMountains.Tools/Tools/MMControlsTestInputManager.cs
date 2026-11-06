using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000186 RID: 390
	public class MMControlsTestInputManager : MonoBehaviour, MMEventListener<MMSwipeEvent>, MMEventListenerBase
	{
		// Token: 0x06000915 RID: 2325 RVA: 0x0002A768 File Offset: 0x00028968
		protected virtual void Start()
		{
			Application.targetFrameRate = 300;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002A774 File Offset: 0x00028974
		public virtual void LeftJoystickMovement(Vector2 movement)
		{
			MMDebug.DebugOnScreen("left joystick", movement, 25);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002A788 File Offset: 0x00028988
		public virtual void RightJoystickMovement(Vector2 movement)
		{
			MMDebug.DebugOnScreen("right joystick", movement, 25);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002A79C File Offset: 0x0002899C
		public virtual void RepositionableJoystickMovement(Vector2 movement)
		{
			MMDebug.DebugOnScreen("Repositionable joystick", movement, 25);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002A7B0 File Offset: 0x000289B0
		public virtual void FollowerJoystickMovement(Vector2 movement)
		{
			MMDebug.DebugOnScreen("Follower joystick", movement, 25);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002A7C4 File Offset: 0x000289C4
		public virtual void APressed()
		{
			MMDebug.DebugOnScreen("Button A Pressed");
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002A7D0 File Offset: 0x000289D0
		public virtual void BPressed()
		{
			MMDebug.DebugOnScreen("Button B Pressed");
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002A7DC File Offset: 0x000289DC
		public virtual void XPressed()
		{
			MMDebug.DebugOnScreen("Button X Pressed");
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002A7E8 File Offset: 0x000289E8
		public virtual void YPressed()
		{
			MMDebug.DebugOnScreen("Button Y Pressed");
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0002A7F4 File Offset: 0x000289F4
		public virtual void RTPressed()
		{
			MMDebug.DebugOnScreen("Button RT Pressed");
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002A800 File Offset: 0x00028A00
		public virtual void APressedFirstTime()
		{
			Debug.LogFormat("Button A Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002A811 File Offset: 0x00028A11
		public virtual void BPressedFirstTime()
		{
			Debug.LogFormat("Button B Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0002A822 File Offset: 0x00028A22
		public virtual void XPressedFirstTime()
		{
			Debug.LogFormat("Button X Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0002A833 File Offset: 0x00028A33
		public virtual void YPressedFirstTime()
		{
			Debug.LogFormat("Button Y Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002A844 File Offset: 0x00028A44
		public virtual void RTPressedFirstTime()
		{
			Debug.LogFormat("Button RT Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002A855 File Offset: 0x00028A55
		public virtual void AReleased()
		{
			Debug.LogFormat("Button A Released", Array.Empty<object>());
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002A866 File Offset: 0x00028A66
		public virtual void BReleased()
		{
			Debug.LogFormat("Button B Released", Array.Empty<object>());
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002A877 File Offset: 0x00028A77
		public virtual void XReleased()
		{
			Debug.LogFormat("Button X Released", Array.Empty<object>());
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002A888 File Offset: 0x00028A88
		public virtual void YReleased()
		{
			Debug.LogFormat("Button Y Released", Array.Empty<object>());
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002A899 File Offset: 0x00028A99
		public virtual void RTReleased()
		{
			Debug.LogFormat("Button RT Released", Array.Empty<object>());
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0002A8AA File Offset: 0x00028AAA
		public virtual void HorizontalAxisPressed(float value)
		{
			MMDebug.DebugOnScreen("horizontal movement", value, 25);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0002A8BE File Offset: 0x00028ABE
		public virtual void VerticalAxisPressed(float value)
		{
			MMDebug.DebugOnScreen("vertical movement", value, 25);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0002A8D2 File Offset: 0x00028AD2
		public virtual void LeftPressedFirstTime()
		{
			Debug.LogFormat("Button Left Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002A8E3 File Offset: 0x00028AE3
		public virtual void UpPressedFirstTime()
		{
			Debug.LogFormat("Button Up Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002A8F4 File Offset: 0x00028AF4
		public virtual void DownPressedFirstTime()
		{
			Debug.LogFormat("Button Down Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002A905 File Offset: 0x00028B05
		public virtual void RightPressedFirstTime()
		{
			Debug.LogFormat("Button Right Pressed for the first time", Array.Empty<object>());
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002A916 File Offset: 0x00028B16
		public virtual void LeftReleased()
		{
			Debug.LogFormat("Button Left Released", Array.Empty<object>());
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002A927 File Offset: 0x00028B27
		public virtual void UpReleased()
		{
			Debug.LogFormat("Button Up Released", Array.Empty<object>());
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002A938 File Offset: 0x00028B38
		public virtual void DownReleased()
		{
			Debug.LogFormat("Button Down Released", Array.Empty<object>());
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002A949 File Offset: 0x00028B49
		public virtual void RightReleased()
		{
			Debug.LogFormat("Button Right Released", Array.Empty<object>());
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002A95A File Offset: 0x00028B5A
		public virtual void StickDragged()
		{
			Debug.LogFormat("On drag", Array.Empty<object>());
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002A96B File Offset: 0x00028B6B
		public virtual void StickPointerUp()
		{
			Debug.LogFormat("On pointer up", Array.Empty<object>());
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002A97C File Offset: 0x00028B7C
		public virtual void StickPointerDown()
		{
			Debug.LogFormat("On pointer down", Array.Empty<object>());
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002A990 File Offset: 0x00028B90
		public virtual void OnMMEvent(MMSwipeEvent swipeEvent)
		{
			Debug.LogFormat(string.Concat(new string[]
			{
				"Swipe at a ",
				swipeEvent.SwipeAngle.ToString(),
				"° angle, and a length of ",
				swipeEvent.SwipeLength.ToString(),
				" length. Cardinal direction : ",
				swipeEvent.SwipeDirection.ToString()
			}), Array.Empty<object>());
			string text = "Swipe origin : ";
			Vector2 vector = swipeEvent.SwipeOrigin;
			string text2 = vector.ToString();
			string text3 = ", swipe end : ";
			vector = swipeEvent.SwipeDestination;
			Debug.LogFormat(text + text2 + text3 + vector.ToString(), Array.Empty<object>());
			Debug.LogFormat("Swipe duration : " + swipeEvent.SwipeDuration.ToString(), Array.Empty<object>());
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002AA5E File Offset: 0x00028C5E
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMSwipeEvent>();
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002AA66 File Offset: 0x00028C66
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMSwipeEvent>();
		}
	}
}
