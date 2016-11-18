// Learn more about F# at http://fsharp.org

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop


[<DllImport("librtlsdr", CallingConvention=CallingConvention.Cdecl)>]
extern uint32 rtlsdr_get_device_count();

[<EntryPoint>]
let main argv = 
    let count = rtlsdr_get_device_count()
    printfn "We have %d devices" count
    0 // return an integer exit code
