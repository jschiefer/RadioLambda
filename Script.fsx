#r "bin/Debug/RadioLambda.exe"

open System
open System.Text
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Hamstr.RtlSdrNative
open Hamstr.RtlSdr

let count = DeviceCount()



let manu = new StringBuilder()
let prod = new StringBuilder()
let serial = new StringBuilder()
let s = rtlsdr_get_device_usb_strings(0u, manu, prod, serial)
s
