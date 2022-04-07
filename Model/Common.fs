module StorageMachine.Common

open System.Text.RegularExpressions

type BinIdentifier = private | BinIdentifier of string

let (|BinIdentifier|) (BinIdentifier binIdentifier) = binIdentifier

[<RequireQualifiedAccess>]
module BinIdentifier =

    let make rawIdentifier =
        rawIdentifier
        |> Validation.nonEmpty "Bin identifier may not be empty."
        |> Result.bind (Validation.alphaNumeric "Bin identifier may contain only letters or digits.")
        |> Result.map BinIdentifier

type PartNumber = private | PartNumber of string

let (|PartNumber|) (PartNumber partNumber) = partNumber

[<RequireQualifiedAccess>]
module PartNumber =

    let private validPartNumber = Regex "^\d{4}-\d{4}-\d{4}$"

    let make rawPartNumber =
        rawPartNumber
        |> Validation.matches validPartNumber "Part number must have format dddd-dddd-dddd, where d is a digit."
        |> Result.map PartNumber
