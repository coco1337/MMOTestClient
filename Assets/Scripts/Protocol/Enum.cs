// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Enum.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Protocol {

  /// <summary>Holder for reflection information generated from Enum.proto</summary>
  public static partial class EnumReflection {

    #region Descriptor
    /// <summary>File descriptor for Enum.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EnumReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpFbnVtLnByb3RvEghQcm90b2NvbCp0Cg9QYWNrZXRFcnJvclR5cGUSHAoY",
            "UEFDS0VUX0VSUk9SX1RZUEVfRkFJTEVEEAASHQoZUEFDS0VUX0VSUk9SX1RZ",
            "UEVfU1VDQ0VTUxABEiQKIFBBQ0tFVF9FUlJPUl9UWVBFX1JFR0lTVEVSX0VS",
            "Uk9SEAJiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Protocol.PacketErrorType), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum PacketErrorType {
    [pbr::OriginalName("PACKET_ERROR_TYPE_FAILED")] Failed = 0,
    [pbr::OriginalName("PACKET_ERROR_TYPE_SUCCESS")] Success = 1,
    [pbr::OriginalName("PACKET_ERROR_TYPE_REGISTER_ERROR")] RegisterError = 2,
  }

  #endregion

}

#endregion Designer generated code
