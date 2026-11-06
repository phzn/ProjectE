using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003F8 RID: 1016
	public class MoneyManager : MonoBehaviour
	{
		// Token: 0x060019BD RID: 6589 RVA: 0x000A32F0 File Offset: 0x000A14F0
		private void Awake()
		{
			this.dir = Application.dataPath + "/CompanionToolkit/Common/Data/JSON";
			this.path = Application.dataPath + "/CompanionToolkit/Common/Data/JSON/Transaction.json";
			if (!Directory.Exists(this.dir))
			{
				Directory.CreateDirectory(this.dir);
			}
			if (!File.Exists(this.path))
			{
				File.Create(this.path);
			}
			Debug.Log(this.path);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000A3364 File Offset: 0x000A1564
		private void Start()
		{
			if (PlayerPrefs.HasKey("BankBalance"))
			{
				this.BankBalance = PlayerPrefs.GetInt("BankBalance");
			}
			else
			{
				PlayerPrefs.SetInt("BankBalance", this.BankBalance);
			}
			this.BankBalanceText.text = this.BankBalance.ToString();
			this.LoadTransactionData();
			this.InstantiateTransactionPrefabs();
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x000A33C4 File Offset: 0x000A15C4
		private void LoadTransactionData()
		{
			if (File.Exists(this.path))
			{
				MoneyManager.TransactionDataList transactionDataList = JsonUtility.FromJson<MoneyManager.TransactionDataList>(File.ReadAllText(this.path));
				this.transactions = ((transactionDataList != null && transactionDataList.transactions != null) ? transactionDataList.transactions : new List<MoneyManager.TransactionData>());
				return;
			}
			Debug.LogError("Transaction data file not found: " + this.path);
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x000A3424 File Offset: 0x000A1624
		private void SaveTransactionData()
		{
			string text = JsonUtility.ToJson(new MoneyManager.TransactionDataList
			{
				transactions = this.transactions
			}, true);
			try
			{
				File.WriteAllText(this.path, text);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error saving updated transaction data: " + ex.Message);
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000A3480 File Offset: 0x000A1680
		private void InstantiateTransactionPrefabs()
		{
			foreach (MoneyManager.TransactionData transactionData in this.transactions)
			{
				GameObject gameObject;
				if (transactionData.type == "Send" && this.sendTransactionPrefab != null)
				{
					gameObject = this.sendTransactionPrefab;
				}
				else
				{
					if (!(transactionData.type == "Receive") || !(this.receiveTransactionPrefab != null))
					{
						Debug.LogWarning("Transaction prefab not assigned for type: " + transactionData.type);
						continue;
					}
					gameObject = this.receiveTransactionPrefab;
				}
				if (gameObject != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, this.transactionParent);
					gameObject2.transform.GetChild(2).GetComponent<TMP_Text>().text = transactionData.date;
					if (transactionData.type == "Send")
					{
						gameObject2.transform.GetChild(1).GetComponent<TMP_Text>().text = "You sent $" + transactionData.amount.ToString() + " to " + transactionData.name;
					}
					else if (transactionData.type == "Receive" && this.receiveTransactionPrefab != null)
					{
						gameObject2.transform.GetChild(1).GetComponent<TMP_Text>().text = transactionData.name + " sent you $" + transactionData.amount.ToString();
					}
				}
			}
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x000A361C File Offset: 0x000A181C
		public void CreateNewTransaction(string name, int amount, string date, string type)
		{
			MoneyManager.TransactionData transactionData = new MoneyManager.TransactionData
			{
				name = name,
				amount = amount,
				date = date,
				type = type
			};
			this.transactions.Insert(0, transactionData);
			if (type == "Send" && this.sendTransactionPrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.sendTransactionPrefab, this.transactionParent);
				gameObject.transform.SetAsFirstSibling();
				gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = "You sent $" + transactionData.amount.ToString() + " to " + transactionData.name;
				this.CashSpent(transactionData.amount);
			}
			else if (type == "Receive" && this.receiveTransactionPrefab != null)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.receiveTransactionPrefab, this.transactionParent);
				gameObject2.transform.SetAsFirstSibling();
				gameObject2.transform.GetChild(1).GetComponent<TMP_Text>().text = transactionData.name + " sent you $" + transactionData.amount.ToString();
				this.CashEarned(transactionData.amount);
			}
			else
			{
				Debug.LogWarning("Transaction prefab not assigned for type: " + type);
			}
			this.SaveTransactionData();
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000A3764 File Offset: 0x000A1964
		public void CashEarned(int cash)
		{
			this.BankBalance += cash;
			this.BankBalanceText.text = "$" + this.BankBalance.ToString();
			PlayerPrefs.SetInt("BankBalance", this.BankBalance);
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x000A37A4 File Offset: 0x000A19A4
		public void CashSpent(int cash)
		{
			this.BankBalance -= cash;
			this.BankBalanceText.text = "$" + this.BankBalance.ToString();
			PlayerPrefs.SetInt("BankBalance", this.BankBalance);
		}

		// Token: 0x04001AF1 RID: 6897
		public List<MoneyManager.TransactionData> transactions;

		// Token: 0x04001AF2 RID: 6898
		public Transform transactionParent;

		// Token: 0x04001AF3 RID: 6899
		public GameObject sendTransactionPrefab;

		// Token: 0x04001AF4 RID: 6900
		public GameObject receiveTransactionPrefab;

		// Token: 0x04001AF5 RID: 6901
		public int BankBalance;

		// Token: 0x04001AF6 RID: 6902
		public TMP_Text BankBalanceText;

		// Token: 0x04001AF7 RID: 6903
		private string path = "";

		// Token: 0x04001AF8 RID: 6904
		private string dir = "";

		// Token: 0x020003F9 RID: 1017
		[Serializable]
		public class TransactionData
		{
			// Token: 0x04001AF9 RID: 6905
			public string name;

			// Token: 0x04001AFA RID: 6906
			public int amount;

			// Token: 0x04001AFB RID: 6907
			public string date;

			// Token: 0x04001AFC RID: 6908
			public string type;
		}

		// Token: 0x020003FA RID: 1018
		[Serializable]
		public class TransactionDataList
		{
			// Token: 0x04001AFD RID: 6909
			public List<MoneyManager.TransactionData> transactions;
		}
	}
}
