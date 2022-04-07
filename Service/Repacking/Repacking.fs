module StorageMachine.Repacking.Repacking

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Thoth.Json.Giraffe
open StorageMachine
open Common
open Repacking

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

let handlers : HttpHandler =
    choose [
        GET >=> routef "/bin/tree/%s" viewBinTree
        GET >=> routef "/bin/tree/%s/products/count" productCount
    ]