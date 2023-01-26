#r "nuget: Hopac"

open System
open System.Threading
open Hopac.Extensions
open Hopac.Infixes
open Hopac

// natural numbers

let counter startCount =
    let ch = Ch ()
    let rec count i = Job.delay <| fun () -> ch *<- i ^=>. count (i + 1)
    start (count startCount)
    ch
let ch = counter 0

Ch.take ch ^-> printfn "%i"
|> Job.forN 10  
|> start