#r "bin/Debug/RadioLambda.exe"

open System
open System.Text
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Hamstr.RtlSdrNative
open Hamstr.RtlSdr

let count = DeviceCount()
let deviceDescription = DeviceUsbStrings 0u
