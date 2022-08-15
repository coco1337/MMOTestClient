using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using ServerCore;
using Protocol;
using UnityEngine;

public sealed class PacketHandler
{
  public static void CS_TestHandler(PacketSession session, IMessage packet)
  {
    var cs_test = packet as CS_TEST;
    var serverSesion = session as ServerSession;

    Debug.Log(cs_test.Id);
  }

  public static void SC_TestHandler(PacketSession session, IMessage packet)
  {
    var sc_test = packet as SC_TEST;
    var serverSession = session as ServerSession;

    Debug.Log(sc_test.Id);
  }

  public static void CS_LoginHandler(PacketSession session, IMessage packet)
  {
    var cs_login = packet as CS_LOGIN;
    var serverSession = session as ServerSession;

    Debug.Log(cs_login.Id);
  }
}

