module StorageMachine.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open StorageMachine.Stock

let requestHandlers : HttpHandler =
    choose [
        route "/hello" >=> GET >=> text "Storage machine is running"
        Stock.stockHandlers
    ]