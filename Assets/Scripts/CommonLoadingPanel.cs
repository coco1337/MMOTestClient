using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public sealed class CommonLoadingPanel : MonoBehaviour
{
  [SerializeField] private GameObject panel;
  [SerializeField] private TMP_Text mainText;
  [SerializeField] private Button btnClose;

  public void SetLoading(string s = "Loading...")
  {
    this.panel.SetActive(true);
    this.mainText.text = s;
    this.btnClose.gameObject.SetActive(false);
  }

  public void CloseLoading()
  {
    this.panel.SetActive(false);
  }

  public void ErrorOccured(string s)
  {
    this.panel.SetActive(true);
    this.mainText.text = s;
    this.btnClose.gameObject.SetActive(true);
  }
}
