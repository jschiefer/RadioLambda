// Learn more about F# at http://fsharp.org
#nowarn "9"

open System
open FSharp.NativeInterop
open Hamstr.RtlSdr

[<EntryPoint>]
let main argv = 
    let count = rtlsdr_get_device_count()
    match count with
    | 0u -> printfn "No devices found"
    | n -> 
        printfn "%d devices found" n
        [0u..n-1u] 
        |> List.iter (fun i -> 
            let name = rtlsdr_get_device_name(i)
            printfn "%A" name
        )

    let mutable dev:nativeptr<rtlsdr_dev_t> = 0 |> nativeint |> NativePtr.ofNativeInt
    let r = rtlsdr_open(&& dev, 0u)
    printfn "open returned %A" r
    let s = rtlsdr_close(dev)
    printfn "close returned %A" s
    0 // return an integer exit code
