/// This module exposes use-cases of the Repacking component as an HTTP Web service using Giraffe.
module StorageMachine.Repacking.Repacking

open Giraffe
open Microsoft.AspNetCore.Http
open Thoth.Json.Giraffe
open StorageMachine
open Common
open Repacking

/// Retrieve a textual representation of a single bin tree stored in the Storage Machine.
let viewBinTree binIdentifier (next: HttpFunc) (ctx: HttpContext) =
    task {
        match BinIdentifier.make binIdentifier with
        | Error message ->
            return! RequestErrors.badRequest (text "Invalid bin identifier") earlyReturn ctx
        | Ok binIdentifier ->
            let dataAccess = ctx.GetService<IBinTreeDataAccess> ()
            let binTree = Repacking.viewBinTree dataAccess binIdentifier
            return! ctx.WriteStringAsync (string binTree)
    }

/// Count all products contained in all bins of a single bin tree currently stored in the Storage Machine.
let productCount binIdentifier (next: HttpFunc) (ctx: HttpContext) =
    task {
        match BinIdentifier.make binIdentifier with
        | Error message ->
            return! RequestErrors.badRequest (text "Invalid bin identifier") earlyReturn ctx
        | Ok binIdentifier ->
            let dataAccess = ctx.GetService<IBinTreeDataAccess> ()
            match Repacking.productCount dataAccess binIdentifier with
            | None ->
                return! RequestErrors.notFound (text "The given bin is not stored in the machine") earlyReturn ctx
            | Some count ->
                return! ctx.WriteStringAsync (sprintf "The bin tree contains %d products" count)
    }

/// Defines URLs for functionality of the Repacking component and dispatches HTTP requests to those URLs.
let handlers : HttpHandler =
    choose [
        GET >=> routef "/bin/tree/%s" viewBinTree
        GET >=> routef "/bin/tree/%s/products/count" productCount
    ]