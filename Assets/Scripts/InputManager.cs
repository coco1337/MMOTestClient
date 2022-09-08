using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public sealed class InputManager : MonoBehaviour
{
  public static InputManager Instance;

  private float horizontalInput;
  private float verticalInput;
  private float mouseXInput;
  private float mouseYInput;

  private bool isRegistered;

  private MoveData moveDataCache;

  [SerializeField] private PlayerController localController;
  [SerializeField] private float axisXSensi = 1f;
  [SerializeField] private float axisYSensi = 1f;

  private void Start()
  {
    Instance ??= this;
    // TODO : localController, FindByUid
  }

  private void Update()
  {
    if (!this.isRegistered) return;
    this.moveDataCache = this.localController.GetMoveData();
    KeyboardMove();
    MouseMove();
    this.localController.UpdateMoveData(this.moveDataCache);

    var req = new CS_MOVE_REQ { Id = (ushort)PacketId.PKT_CS_MOVE_REQ, MoveData = TypeHelper.MoveDataToProtocolMoveData(this.moveDataCache), Uid = NetworkManager.Uid };
    NetworkManager.Instance.Send(req, PacketId.PKT_CS_MOVE_REQ);
  }

  public void RegisterLocalPlayer(PlayerController player)
  {
    this.localController = player;
    this.isRegistered = true;
  }

  private void KeyboardMove()
  {
    this.horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
    this.verticalInput = Input.GetAxis("Vertical") * Time.deltaTime;

    // TODO : 맵 이동 가능한지 확인

    var movePos = new Vector3(this.moveDataCache.Position.x + this.horizontalInput, this.moveDataCache.Position.y, this.moveDataCache.Position.z + this.verticalInput);
    this.moveDataCache.Position = movePos;
  }

  private void MouseMove()
  {
    Log.Debug($"mouse axis : {Input.GetAxis("Mouse X")} - {Input.GetAxis("Mouse Y")}");
    this.mouseXInput = Input.GetAxis("Mouse X") * this.axisXSensi;
    this.mouseYInput = Input.GetAxis("Mouse Y") * this.axisYSensi;

    var rot = new Vector3(Mathf.Clamp(this.moveDataCache.Rotation.y + this.mouseYInput, -85, 85), this.moveDataCache.Rotation.x + this.mouseXInput, this.moveDataCache.Rotation.z);
    this.moveDataCache.Rotation = rot;
  }
}
