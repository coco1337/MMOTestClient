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

  [Header("Camera")]
  [SerializeField] private Camera playerCamera;
  [SerializeField] private Transform cameraPos;

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

    if (this.isLocal)
    {
      InputManager.Instance.RegisterLocalPlayer(this);
      this.playerCamera = Camera.main;
      SetCamera();
    }
  }

  public void UpdateMoveData(Protocol.MoveData data)
  {
    TypeHelper.Vec3ToVector3(data.MoveDir, out this.moveData.MoveDir);
    TypeHelper.Vec3ToVector3(data.Position, out this.moveData.Position);
    TypeHelper.Vec3ToVector3(data.Rotation, out this.moveData.Rotation);
    Apply();
  }

  public void UpdateMoveData(MoveData data)
  {
    this.moveData = data;
    Apply();
  }

  private void Apply()
  {
    this.transform.position = this.moveData.Position;

    var rotTemp = this.moveData.Rotation;
    var rot = Quaternion.Euler(0, rotTemp.y, 0);
    this.transform.rotation = rot;
  }

  private void SetCamera()
  {
    this.playerCamera.transform.parent = this.cameraPos;
    this.playerCamera.transform.localPosition = new Vector3(0, 0, 0);
    this.playerCamera.transform.localRotation = new Quaternion(0, 0, 0, 0);
  }
}
