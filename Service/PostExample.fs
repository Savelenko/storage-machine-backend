/// Not related to actual functionality of this back-end. Provides an example of some of the Giraffe functions.
module StorageMachine.PostExample

open Microsoft.AspNetCore.Http
open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net

/// An example of how to:
/// - read JSON from the body of a HTTP request and apply a Thoth decoder to it
/// - return two different "negative" responses, based on the deserialized value
/// - return a "positive" response
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
