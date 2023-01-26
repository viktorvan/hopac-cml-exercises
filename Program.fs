// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics

[<EntryPoint>]
let main argv =
    let sw = Stopwatch.StartNew()
    Eratosthenes.run 1000
    |> printfn "%A"
    printfn "Elapsed: %A ms" sw.ElapsedMilliseconds
    0 // return an integer exit code
