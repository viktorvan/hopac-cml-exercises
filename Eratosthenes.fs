module Eratosthenes

open Hopac.Infixes
open Hopac

let counter startCount =
//    printfn "creating counter channel"
    let ch = Ch ()
    let rec count i = Job.delay <| fun () -> 
        ch *<- i ^=>. count (i + 1)
    start (count startCount)
    ch

let filter p (inCh: Ch<int>) =
//    printfn "Create new filter channel for %i" p
    let outCh = Ch ()
    let isMultipleOfFilterNum i = i % p <> 0
    let rec loop () =
        Ch.take inCh
        ^=> fun i -> 
            if isMultipleOfFilterNum i then 
                outCh *<- i 
            else
                // do nothing
                Alt.always ()
        ^=>. loop()
    start (loop ())
    outCh

let sieve () =
    let primes = Ch<int> ()
    let rec head (ch: Ch<int>) = 
//        printfn "taking number from input channel"
        Ch.take ch
        ^=> (fun p -> 
//            printfn "got %i from counter" p
            primes *<- p 
            >>=. 
                // printfn "sent %i to primes" p
                head (filter p ch))

    start (head (counter 2))
    primes

let run n =
    let ch = sieve ()
    let rec loop (n, primes) =
        if n = 0 then 
            Job.result primes
        else
            Ch.take ch
            >>= fun p -> loop(n-1, p :: primes)
            
    loop (n, []) |> run

