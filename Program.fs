// Learn more about F# at http://fsharp.org

open System
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
    0 // return an integer exit code
