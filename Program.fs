// Learn more about F# at http://fsharp.org

open System
open System.Runtime.InteropServices

[<DllImport("librtlsdr")>]
RTLSDR_API uint32_t rtlsdr_get_device_count()

[<EntryPoint>]
let main argv = 
    let count = rtlsdr_get_device_count()
    printfn "We have %d devices" count
    0 // return an integer exit code
