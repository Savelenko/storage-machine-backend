/// Common definitions of domain entities reusable across all components.
module StorageMachine.Common

open System.Text.RegularExpressions

/// Every bin stored in the Storage Machine is identified by a unique identifier.
type BinIdentifier = private | BinIdentifier of string

/// Provides access to raw bin identifier by pattern matching.
let (|BinIdentifier|) (BinIdentifier binIdentifier) = binIdentifier

/// A companion module for 'BinIdentifier'.
[<RequireQualifiedAccess>]
module BinIdentifier =

    /// Construct a valid bin identifier from a raw string or indicate that the string is not a valid bin identifier.
    let make rawIdentifier =
        rawIdentifier
        |> Validation.nonEmpty "Bin identifier may not be empty."
        |> Result.bind (Validation.alphaNumeric "Bin identifier may contain only letters or digits.")
        |> Result.map BinIdentifier

/// All stock (products, components, etc.) is identified by a part number.
type PartNumber = private | PartNumber of string

/// Provides access to raw part number by pattern matching.
let (|PartNumber|) (PartNumber partNumber) = partNumber

/// A companion module for 'PartNumber'.
[<RequireQualifiedAccess>]
module PartNumber =

    /// Part numbers have a specific format.
    let private validPartNumber = Regex "^\d{4}-\d{4}-\d{4}$"

    /// Construct a valid part number from a raw string or indicate that the string is not a valid part number.
    let make rawPartNumber =
        rawPartNumber
        |> Validation.matches validPartNumber "Part number must have format dddd-dddd-dddd, where d is a digit."
        |> Result.map PartNumber
