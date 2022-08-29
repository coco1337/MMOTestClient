using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
  [SerializeField] private bool isLocal;
  [SerializeField] private ulong uid;
  [SerializeField] private string userName;

  public void Init(bool isLocal, ulong uid, string name)
  {
    this.isLocal = isLocal;
    this.uid = uid; 
    this.userName = name;
  }
}
