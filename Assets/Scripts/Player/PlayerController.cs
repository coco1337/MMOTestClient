using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class MoveData
{
  public Vector3 Position;
  public Vector3 Rotation;
  public Vector3 MoveDir;
}

public sealed class PlayerController : MonoBehaviour
{
  [SerializeField] private bool isLocal;
  [SerializeField] private ulong uid;
  [SerializeField] private string userName;

  [SerializeField] private MoveData moveData = new();

  [Header("UI")]
  [SerializeField] private TMP_Text txtUserName;

  public ulong Uid => this.uid;
  public MoveData GetMoveData() => this.moveData;

  public void Init(bool isLocal, ulong uid, string name)
  {
    this.isLocal = isLocal;
    this.uid = uid; 
    this.userName = name;
    this.txtUserName.text = name;

    if (this.isLocal) InputManager.Instance.RegisterLocalPlayer(this);
  }

  public void UpdateMoveData(Protocol.MoveData data)
  {
    TypeHelper.Vec3ToVector3(data.MoveDir, out this.moveData.MoveDir);
    TypeHelper.Vec3ToVector3(data.Position, out this.moveData.Position);
    TypeHelper.Vec3ToVector3(data.Rotation, out this.moveData.Rotation);
    Move();
  }

  public void UpdateMoveData(MoveData data)
  {
    this.moveData = data;
    Move();
  }

  public void Move()
  {
    this.transform.position = this.moveData.Position;
  }
}
