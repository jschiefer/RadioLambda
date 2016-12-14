// Learn more about F# at http://fsharp.org
#nowarn "9"

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
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

    let mutable dev = Unchecked.defaultof<_>
    try 
        let r = rtlsdr_open(&&dev, 0u)
        printfn "open returned %A" r
        let t = rtlsdr_get_tuner_type(dev)
        printfn "tuner is %A" t
        let bur = rtlsdr_reset_buffer(dev)
        printfn "rtlsdr_reset_buffer returned %A" bur
        
        // Read with async callback
        let f = new rtlsdr_read_async_cb_t (fun (a, b, c) -> printfn "oink!")
        let u = rtlsdr_read_async(dev, f, 0|> nativeint, 0u, 0u)
        printfn "read returned %A" u

        let s = rtlsdr_close(dev)
        printfn "close returned %A" s
    with 
        | Failure e -> printfn "ouch: %A" e 
    0 // return an integer exit code
