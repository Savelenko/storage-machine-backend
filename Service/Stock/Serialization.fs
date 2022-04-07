module StorageMachine.Stock.Serialization

open Thoth.Json.Net
open FsToolkit.ErrorHandling
open StorageMachine
open Common
open Bin

let encoderBin : Encoder<Bin> = fun bin ->
    Encode.object [
        "binIdentifier", (let (BinIdentifier identifier) = bin.Identifier in Encode.string identifier)
        "content", (Encode.option (fun (PartNumber partNumber) -> Encode.string partNumber) bin.Content)
    ]

let decoderBinIdentifier : Decoder<BinIdentifier> =
    Decode.string
    |> Decode.andThen (fun s ->
        match BinIdentifier.make s with
        | Ok binIdentifier -> Decode.succeed binIdentifier
        | Error validationMessage -> Decode.fail validationMessage
    )
    
let decoderPartNumber : Decoder<PartNumber> =
    Decode.string
    |> Decode.andThen (fun s ->
        match PartNumber.make s with
        | Ok partNumber -> Decode.succeed partNumber
        | Error validationMessage -> Decode.fail validationMessage
    )