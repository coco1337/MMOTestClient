using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class MainSceneUIManager : MonoBehaviour
{
  public static MainSceneUIManager Instance;

  [Header("Main")]
  [SerializeField] private GameObject mainPanel;

  [Header("Sign in")]
  [SerializeField] private GameObject signIn;
  [SerializeField] private TMP_InputField inputSignInId;
  [SerializeField] private TMP_InputField inputSignInPw;

  [Header("Sign up")]
  [SerializeField] private GameObject signUp;
  [SerializeField] private TMP_InputField inputSignUpId;
  [SerializeField] private TMP_InputField inputSignUpPw;
  [SerializeField] private TMP_InputField inputSignUpRepeatPw;
  [SerializeField] private Button btnRegister;

  [Header("Loading")]
  [SerializeField] private CommonLoadingPanel loadingPanel;

  private void Start()
  {
    Instance ??= this;
    this.mainPanel.SetActive(true);
    this.signIn.SetActive(false);
    this.signUp.SetActive(false);
    this.btnRegister.interactable = false;
  }

  public void OnClickRegister()
  {
    Debug.Log($"{inputSignUpId.text} :: {inputSignUpPw.text}");

    var pkt = new CS_REGISTER_REQ()
    {
      Id = (ulong)PacketId.PKT_CS_REGISTER_REQ,
      UserId = this.inputSignUpId.text,
      Password = this.inputSignUpPw.text
    };

    NetworkManager.Instance.Send(pkt, PacketId.PKT_CS_REGISTER_REQ);
    this.loadingPanel.SetLoading();
  }

  public void OnClickSignIn()
  {
    Debug.Log($"{inputSignInId.text} :: {inputSignInPw.text}");

    var pkt = new CS_LOGIN_REQ()
    {
      Id = (ulong)PacketId.PKT_CS_LOGIN_REQ,
      UserId = this.inputSignInId.text,
      Password = this.inputSignInPw.text
    };

    NetworkManager.Instance.Send(pkt, PacketId.PKT_CS_LOGIN_REQ);
    this.loadingPanel.SetLoading();
  }

  public void OnChangeRepeatPassword()
  {
    if (this.inputSignUpPw.text.Length == 0 || this.inputSignUpRepeatPw.text.Length == 0)
    {
      this.btnRegister.interactable = false;
      return;
    }
      
    this.btnRegister.interactable = string.Equals(this.inputSignUpRepeatPw.text, this.inputSignUpPw.text);
  }

  public void OnClickSignUpBtn()
  {
    this.mainPanel.SetActive(false);
    this.signUp.SetActive(true);
    this.signIn.SetActive(false);
  }

  public void OnClickSignInBtn()
  {
    this.mainPanel.SetActive(false);
    this.signUp.SetActive(false);
    this.signIn.SetActive(true);
  }

  #region Network
  public void HandleMessage(SC_REGISTER_RES res)
  {
    if (res.PacketResult == PacketErrorType.Success)
    {
      Debug.Log($"REGISTER SUCCESS || ID : {res.Id}, RESULT : {res.PacketResult}");
      this.loadingPanel.CloseLoading();
      this.OnClickSignInBtn();
      this.inputSignInId.text = this.inputSignUpId.text;
      this.inputSignInPw.text = this.inputSignUpPw.text;
    } 
    else
    {
      this.loadingPanel.ErrorOccured("Register Error");
    }
  }

  public void HandleMessage(SC_LOGIN_RES res)
  {
    if (res.PacketResult == PacketErrorType.Success)
    {
      Debug.Log($"LOGIN SUCCESS || ID : {res.Id}, UID : {res.Uid}, RESULT : {res.PacketResult}");
      NetworkManager.Instance.SetUid(res.Uid);
      SceneManager.LoadScene(1);
    }
  }
  #endregion
}
