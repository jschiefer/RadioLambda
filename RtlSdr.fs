module Hamstr.RtlSdr

open System
open System.Text
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Hamstr.RtlSdrNative

let DeviceCount() = rtlsdr_get_device_count()

let DeviceName index = 
    Marshal.PtrToStringAnsi(rtlsdr_get_device_name(index))

