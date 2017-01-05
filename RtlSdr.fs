module Hamstr.RtlSdr

open System
open System.Text
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Hamstr.RtlSdrNative

let DeviceCount() = rtlsdr_get_device_count()

let DeviceName index = 
    Marshal.PtrToStringAnsi(rtlsdr_get_device_name(index))

let DeviceUsbStrings (index:uint32) =
    let manu = new StringBuilder(256)
    let prod = new StringBuilder(256)
    let serial = new StringBuilder(256)
    let s = rtlsdr_get_device_usb_strings(0u, manu, prod, serial)
    (manu.ToString(), prod.ToString(), serial.ToString())

