using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Protocol;

public sealed class RoomUIManager : MonoBehaviour
{
  public static RoomUIManager Instance { get; private set; }
  [SerializeField] private TMP_Text uitext;
  [SerializeField] private TMP_InputField txtInputField;

  private List<string> msgList = new List<string>();

  private void Start()
  {
    Instance ??= this;
  }

  public void OnSubmit()
  {
    var msg = this.txtInputField.text;

    var req = new CS_SEND_CHAT_REQ
    {
      Id = (ulong)PacketId.PKT_CS_SEND_CHAT_REQ,
      Msg = msg,
    };

    NetworkManager.Instance.Send(req, PacketId.PKT_CS_SEND_CHAT_REQ);
    this.txtInputField.text = "";
  }

  private void AddText(string s)
  {
    this.uitext.text += s + "\n";
  }

  #region Network
  public void HandleMessage(SC_CHAT_NOTI noti)
  {
    var msg = $"{noti.SenderId} :: {noti.Msg}";
    AddText(msg);
  }
  #endregion
}
