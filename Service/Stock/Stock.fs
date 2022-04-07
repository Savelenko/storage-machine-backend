/// This module exposes use-cases of the Stock component as an HTTP Web service using Giraffe.
module StorageMachine.Stock.Stock

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Thoth.Json.Giraffe
open Stock

/// An overview of all bins currently stored in the Storage Machine.
let binOverview (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IStockDataAccess> ()
        let bins = Stock.binOverview dataAccess
        return! ThothSerializer.RespondJsonSeq bins Serialization.encoderBin next ctx 
    }

/// An overview of actual stock currently stored in the Storage Machine. Actual stock is defined as all non-empty bins.
let stockOverview (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IStockDataAccess> ()
        let bins = Stock.stockOverview dataAccess
        return! ThothSerializer.RespondJsonSeq bins Serialization.encoderBin next ctx 
    }

/// Defines URLs for functionality of the Stock component and dispatches HTTP requests to those URLs.
let handlers : HttpHandler =
    choose [
        GET >=> route "/bins" >=> binOverview
        GET >=> route "/stock" >=> stockOverview
    ]