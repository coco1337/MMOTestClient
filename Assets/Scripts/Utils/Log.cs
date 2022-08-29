using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Log : MonoBehaviour
{
  [System.Diagnostics.Conditional("DEV")]
  public static void Debug(string format, params object[] paramList) => UnityEngine.Debug.Log(string.Format(format, paramList));

  [System.Diagnostics.Conditional("DEV")]
  public static void Warn(string format, params object[] paramList) => UnityEngine.Debug.LogWarning(string.Format(format, paramList));

  [System.Diagnostics.Conditional("DEV")]
  public static void Error(string format, params object[] paramList) => UnityEngine.Debug.LogError(string.Format(format, paramList));

  [System.Diagnostics.Conditional("DEV")]
  public static void Assert(bool condition) => Assert(condition, string.Empty, false);

  [System.Diagnostics.Conditional("DEV")]
  public static void Assert(bool condition, string assertString)
    => Assert(condition, assertString, false);

  [System.Diagnostics.Conditional("DEV")]
  public static void Assert(bool condition, string assertString, bool pauseOnFail)
  {
    if (!condition)
    {
      UnityEngine.Debug.LogError("Assert failed! " + assertString);

      if (pauseOnFail) UnityEngine.Debug.Break();
    }
  }
}
