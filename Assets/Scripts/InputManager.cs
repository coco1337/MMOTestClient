using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public sealed class InputManager : MonoBehaviour
{
  public static InputManager Instance;

  private float horizontalInput;
  private float verticalInput;

  private bool isRegistered;

  [SerializeField] private PlayerController localController;

  private void Start()
  {
    Instance ??= this;
    // TODO : localController, FindByUid
  }

  private void Update()
  {
    if (!this.isRegistered) return;
    this.horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
    this.verticalInput = Input.GetAxis("Vertical") * Time.deltaTime;

    // TODO : 맵 이동 가능한지 확인
    var moveData = this.localController.GetMoveData();

    var movePos = new Vector3(moveData.Position.x + this.horizontalInput, moveData.Position.y, moveData.Position.z + this.verticalInput);
    moveData.Position = movePos;

    this.localController.UpdateMoveData(moveData);
    var req = new CS_MOVE_REQ { Id = (ushort)PacketId.PKT_CS_MOVE_REQ, MoveData = TypeHelper.MoveDataToProtocolMoveData(moveData), Uid = NetworkManager.Uid };

    NetworkManager.Instance.Send(req, PacketId.PKT_CS_MOVE_REQ);
  }

  public void RegisterLocalPlayer(PlayerController player)
  {
    this.localController = player;
    this.isRegistered = true;
  }
}
