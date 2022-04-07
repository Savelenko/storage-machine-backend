module StorageMachine.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open StorageMachine.Stock

let requestHandlers : HttpHandler =
    choose [
        route "/hello" >=> GET >=> text "Storage machine is running"
        // See the following example on how to process a POST request, decode JSON and return different responses
        route "/number" >=> POST >=> PostExample.processPost
        Stock.stockHandlers
    ]