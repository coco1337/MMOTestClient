using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using TMPro;

public sealed class MainSceneUIManager : MonoBehaviour
{
  public static MainSceneUIManager Instance;

  [SerializeField] private TMP_InputField idField;
  [SerializeField] private TMP_InputField pwField;

  private void Start()
  {
    Instance ??= this;
  }

  public void OnClickRegister()
  {
    Debug.Log($"{idField.text} :: {pwField}");

    var pkt = new CS_REGISTERREQ();

    pkt.Id = (ulong)MSG_ID.PKT_CS_REGISTERREQ;

    pkt.UserId = this.idField.text;
    pkt.Password = this.pwField.text;

    NetworkManager.Instance.Send(pkt, MSG_ID.PKT_CS_REGISTERREQ);
  }
}
