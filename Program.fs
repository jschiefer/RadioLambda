// Learn more about F# at http://fsharp.org
#nowarn "9"

open System
open System.Runtime.InteropServices
open FSharp.NativeInterop
open Hamstr.RtlSdrNative
open Hamstr.RtlSdr

[<EntryPoint>]
let main argv = 
    let count = DeviceCount()
    match count with
    | 0u -> printfn "No devices found"
    | n -> 
        printfn "%d devices found" n
        [0u..n-1u] 
        |> List.iter (fun i -> 
            let name = rtlsdr_get_device_name(i)
            printfn "%A" name
        )

    let mutable dev = 0n
    try 
        let r = rtlsdr_open(&dev, 0u)
        printfn "open returned %A" r
        let t = rtlsdr_get_tuner_type(dev)
        printfn "tuner is %A" t

        let aa = rtlsdr_set_sample_rate(dev, 2560000u)
        let ab = rtlsdr_set_center_freq(dev, 1000000000u)
        let ac = rtlsdr_set_agc_mode(dev, 1)
        let bur = rtlsdr_reset_buffer(dev)
        printfn "rtlsdr_reset_buffer returned %A" bur
        
        // Read with async callback
        let f = rtlsdr_read_async_cb_t (fun a b c -> printfn "oink! %A %A %A" a b c)
        let u = rtlsdr_read_async(dev, f, 4711n, 0u, 16384u)

        let s = rtlsdr_close(dev)
        printfn "close returned %A" s
    with 
        | Failure e -> printfn "ouch: %A" e 
    0 // return an integer exit code
