using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;

public delegate void NetworkCallback<in T>(T packet);

public sealed class NetworkManager : MonoBehaviour
{
  private const string HOST = "127.0.0.1";
  private const int PORT = 7777;

  private Queue<Action> queue = new Queue<Action>();

  private void Start()
  {
    DontDestroyOnLoad(this);
    ClientSocket.StartClient(HOST, PORT);
  }

  private void Update()
  {
    lock (queue)
      while (queue.Count > 0)
        queue.Dequeue().Invoke();
  }
}
