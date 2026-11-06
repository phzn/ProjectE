using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003ED RID: 1005
	public class LoadoutManager : MonoBehaviour
	{
		// Token: 0x0600199D RID: 6557 RVA: 0x000A26B0 File Offset: 0x000A08B0
		private void Start()
		{
			Button[] array = this.EquippedButtonAll;
			for (int i = 0; i < array.Length; i++)
			{
				Button button2 = array[i];
				button2.onClick.AddListener(delegate
				{
					this.OnEquippedButtonAllClick(button2);
				});
			}
			array = this.AvailablePanels[0].GetComponentsInChildren<Button>();
			for (int i = 0; i < array.Length; i++)
			{
				Button button3 = array[i];
				button3.onClick.AddListener(delegate
				{
					this.OnAvailableButtonMainClick(button3);
				});
			}
			array = this.AvailablePanels[1].GetComponentsInChildren<Button>();
			for (int i = 0; i < array.Length; i++)
			{
				Button button4 = array[i];
				button4.onClick.AddListener(delegate
				{
					this.OnAvailableButtonSideClick(button4);
				});
			}
			array = this.AvailablePanels[2].GetComponentsInChildren<Button>();
			for (int i = 0; i < array.Length; i++)
			{
				Button button5 = array[i];
				button5.onClick.AddListener(delegate
				{
					this.OnAvailableButtonKnifeClick(button5);
				});
			}
			array = this.AvailablePanels[3].GetComponentsInChildren<Button>();
			for (int i = 0; i < array.Length; i++)
			{
				Button button = array[i];
				button.onClick.AddListener(delegate
				{
					this.OnAvailableButtonGranadeClick(button);
				});
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x000A284C File Offset: 0x000A0A4C
		private void OnEquippedButtonAllClick(Button clickedButton)
		{
			this.currentEquippedButton = clickedButton.name;
			GameObject[] availablePanels = this.AvailablePanels;
			for (int i = 0; i < availablePanels.Length; i++)
			{
				availablePanels[i].SetActive(false);
			}
			string text = this.currentEquippedButton;
			if (!(text == "Main"))
			{
				if (!(text == "Side"))
				{
					if (!(text == "Knife"))
					{
						if (text == "Granade")
						{
							this.AvailablePanels[3].SetActive(true);
						}
					}
					else
					{
						this.AvailablePanels[2].SetActive(true);
					}
				}
				else
				{
					this.AvailablePanels[1].SetActive(true);
				}
			}
			else
			{
				this.AvailablePanels[0].SetActive(true);
			}
			foreach (Button button in this.EquippedButtonAll)
			{
				if (button == clickedButton)
				{
					button.transform.GetChild(2).gameObject.SetActive(true);
				}
				else
				{
					button.transform.GetChild(2).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x000A2954 File Offset: 0x000A0B54
		private void OnAvailableButtonMainClick(Button clickedButton)
		{
			foreach (Button button in this.AvailablePanels[0].GetComponentsInChildren<Button>())
			{
				if (button.transform.GetChild(0).GetComponent<Image>().sprite == null)
				{
					return;
				}
				if (button == clickedButton)
				{
					button.transform.GetChild(2).gameObject.SetActive(true);
					this.EquippedButtonAll[0].transform.GetChild(0).GetComponent<Image>().sprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
					Debug.Log(button.transform.GetChild(0).GetComponent<Image>().sprite.name + " Selected!");
				}
				else
				{
					button.transform.GetChild(2).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x000A2A3C File Offset: 0x000A0C3C
		private void OnAvailableButtonSideClick(Button clickedButton)
		{
			foreach (Button button in this.AvailablePanels[1].GetComponentsInChildren<Button>())
			{
				if (button.transform.GetChild(0).GetComponent<Image>().sprite == null)
				{
					return;
				}
				if (button == clickedButton)
				{
					button.transform.GetChild(2).gameObject.SetActive(true);
					this.EquippedButtonAll[1].transform.GetChild(0).GetComponent<Image>().sprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
					Debug.Log(button.transform.GetChild(0).GetComponent<Image>().sprite.name + " Selected!");
				}
				else
				{
					button.transform.GetChild(2).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x000A2B24 File Offset: 0x000A0D24
		private void OnAvailableButtonKnifeClick(Button clickedButton)
		{
			foreach (Button button in this.AvailablePanels[2].GetComponentsInChildren<Button>())
			{
				if (button.transform.GetChild(0).GetComponent<Image>().sprite == null)
				{
					return;
				}
				if (button == clickedButton)
				{
					button.transform.GetChild(2).gameObject.SetActive(true);
					this.EquippedButtonAll[2].transform.GetChild(0).GetComponent<Image>().sprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
					Debug.Log(button.transform.GetChild(0).GetComponent<Image>().sprite.name + " Selected!");
				}
				else
				{
					button.transform.GetChild(2).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x000A2C0C File Offset: 0x000A0E0C
		private void OnAvailableButtonGranadeClick(Button clickedButton)
		{
			foreach (Button button in this.AvailablePanels[3].GetComponentsInChildren<Button>())
			{
				if (button.transform.GetChild(0).GetComponent<Image>().sprite == null)
				{
					return;
				}
				if (button == clickedButton)
				{
					button.transform.GetChild(2).gameObject.SetActive(true);
					this.EquippedButtonAll[3].transform.GetChild(0).GetComponent<Image>().sprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
					Debug.Log(button.transform.GetChild(0).GetComponent<Image>().sprite.name + " Selected!");
				}
				else
				{
					button.transform.GetChild(2).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04001AD0 RID: 6864
		public GameObject[] AvailablePanels;

		// Token: 0x04001AD1 RID: 6865
		private string currentEquippedButton = "Main";

		// Token: 0x04001AD2 RID: 6866
		public Button[] EquippedButtonAll;
	}
}
