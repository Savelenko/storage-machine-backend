module StorageMachine.PostExample

open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net

let processPost (next: HttpFunc) (ctx: HttpContext) =
    task {
        // Decode an integer number from JSON
        let! inputNumber = ThothSerializer.ReadBody ctx Decode.int
        match inputNumber with
        | Error _ ->
            return! RequestErrors.badRequest (text "POST body expected to consist of a single number") earlyReturn ctx
        | Ok number when number % 2 = 0 ->
            return! RequestErrors.notAcceptable (text "I don't want an even number") earlyReturn ctx
        | Ok number ->
            return! Successful.ok (text (sprintf "That's a nice odd number %d" number)) next ctx
    }