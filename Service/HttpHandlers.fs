module StorageMachine.HttpHandlers

open Giraffe
open StorageMachine.Stock
open StorageMachine.Repacking

let requestHandlers : HttpHandler =
    choose [
        // A basic example of handling a GET request
        route "/hello" >=> GET >=> text "Storage machine is running"
        // See the following example on how to process a POST request, decode JSON and return different responses
        route "/number" >=> POST >=> PostExample.processPost
        Stock.handlers
        Repacking.handlers
    ]