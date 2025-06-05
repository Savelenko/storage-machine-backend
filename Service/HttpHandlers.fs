/// Dispatching of HTTP requests for all URLs across all components.
module StorageMachine.HttpHandlers

open Giraffe
open StorageMachine.Stock
open StorageMachine.Repacking

/// Composes all dispatching of HTTP requests into a single Giraffe HTTP handler. This handler is then used to "run"
/// Giraffe in the main function of the back-end.
let requestHandlers : HttpHandler =
    choose [
        // A basic example of handling a GET request
        route "/hello" >=> GET >=> text "Storage machine is running"
        // See the following example on how to process a POST request, decode JSON and return different responses
        route "/number" >=> POST >=> PostExample.processPost
        // Dispatching and handling of Stock component requests
        Stock.handlers
        // Dispatching and handling of Repacking component requests
        Repacking.handlers
    ]