using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Protocol;

public sealed class RoomManager : MonoBehaviour
{
  public static RoomManager Instance;

  [SerializeField] private PlayerController playerPrefab;
  [SerializeField] private List<PlayerController> playerControllers = new List<PlayerController>();

  private void Start()
  {
    Instance ??= this;

    // Finish loading and moved room scene
    var req = new CS_SPAWN_REQ
    {
      Id = (ushort)PacketId.PKT_CS_SPAWN_REQ
    };

    NetworkManager.Instance.Send(req, PacketId.PKT_CS_SPAWN_REQ);
  }

  #region Network
  public void HandleMessage(SC_SPAWN_RES res)
  {
    if (res.PacketResult != PacketErrorType.Success)
    {
      Log.Error("SC_SPAWN_RES error");
      return;
    }

    foreach (var player in res.Players)
    {
      var playerController = Instantiate<PlayerController>(playerPrefab);
      playerController.Init(player.Uid == res.MyId, player.Uid, player.UserName);
      playerController.UpdateMoveData(player.MoveData);
      playerControllers.Add(playerController);
    }
  }

  public void HandleMessage(SC_SPAWN_NOTI noti)
  {
    var playerController = Instantiate<PlayerController>(playerPrefab);
    playerController.Init(false, noti.Player.Uid, noti.Player.UserName);
    this.playerControllers.Add(playerController);
  }

  public void HandleMessage(SC_MOVEDATA_NOTI noti)
  {
    var playerController = this.playerControllers.FirstOrDefault(e => e.Uid == noti.Uid);
    if (playerController == null) return;

    playerController.UpdateMoveData(noti.MoveData);
  }

  public void HandleMessage(SC_DESPAWN_NOTI noti)
  {
    var target = this.playerControllers.FirstOrDefault(e => e.Uid == noti.Uid);
    if (target == null) return;

    this.playerControllers.Remove(target);
    Destroy(target.gameObject);
  }
  #endregion
}
