
#r "bin/Debug/RadioLambda.exe"

open System
open System.Text
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Hamstr.RtlSdrNative
open Hamstr.RtlSdr
open Hamstr.Model

let count = DeviceCount()
let deviceDescription = DeviceUsbStrings 0u

let mutable dev = 0n
let r = rtlsdr_open(&dev, 0u)
printfn "open returned %A" r

let mutable a = 0u
let mutable b = 0u
rtlsdr_get_xtal_freq(dev, &&a, &&b)
b

rtlsdr_close(dev)
