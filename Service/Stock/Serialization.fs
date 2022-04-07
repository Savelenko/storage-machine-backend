module StorageMachine.Stock.Serialization

open Thoth.Json.Net
open StorageMachine
open Common
open Bin

let binEncoder : Encoder<Bin> = fun bin ->
    Encode.object [
        "binIdentifier", (let (BinIdentifier identifier) = bin.Identifier in Encode.string identifier)
        "content", (Encode.option (fun (PartNumber partNumber) -> Encode.string partNumber) bin.Content)
    ]