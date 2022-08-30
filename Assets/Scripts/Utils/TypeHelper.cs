using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Protocol;

public sealed class TypeHelper
{
  public static void Vec3ToVector3(Vec3 vec3, out Vector3 vector3)
  {
    vector3.x = vec3.X;
    vector3.y = vec3.Y;
    vector3.z = vec3.Z;
  }

  public static Vec3 Vector3ToVec3(Vector3 vector3)
  {
    return new Vec3
    {
      X = vector3.x,
      Y = vector3.y,
      Z = vector3.z
    };
  }

  public static Protocol.MoveData MoveDataToProtocolMoveData(MoveData data)
  {
    return new Protocol.MoveData
    {
      MoveDir = Vector3ToVec3(data.MoveDir),
      Position = Vector3ToVec3(data.Position),
      Rotation = Vector3ToVec3(data.Rotation),
    };
  }
}

