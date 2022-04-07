module StorageMachine.Stock.Stock

open System
open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Stock

let binOverview (next: HttpFunc) (ctx: HttpContext) =
    task {
        let stockPersistence = ctx.GetService<IStockPersistence> ()
        let bins = Stock.binOverview stockPersistence
        return! ThothSerializer.RespondJsonSeq bins Serialization.binEncoder next ctx 
    }

let stockOverview (next: HttpFunc) (ctx: HttpContext) =
    task {
        let stockPersistence = ctx.GetService<IStockPersistence> ()
        let bins = Stock.stockOverview stockPersistence
        return! ThothSerializer.RespondJsonSeq bins Serialization.binEncoder next ctx 
    }

let stockHandlers : HttpHandler =
    choose [
        GET >=> route "/bins" >=> binOverview
        GET >=> route "/stock" >=> stockOverview
    ]