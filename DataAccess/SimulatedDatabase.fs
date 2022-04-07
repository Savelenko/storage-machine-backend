module StorageMachine.SimulatedDatabase

open System.Collections.Generic
open FsToolkit.ErrorHandling
open StorageMachine.Stock
open Common
open Bin
open ResultCE

type ParentBin = BinIdentifier
type NestedBin = BinIdentifier

type private StockData = {
    Bins : Set<BinIdentifier>
    Content : Map<BinIdentifier, PartNumber>
    BinStructure : Map<NestedBin, ParentBin>
    // TODO YURSAV: Is this a convenient representation?
}

let private unsafeResult r =
    match r with
    | Ok a -> a
    | Error _ -> failwith "Attempted to use a Result value which was Error"

let mutable private stockData : StockData =
    let bins =
        [
            BinIdentifier.make "B001"
            BinIdentifier.make "B002"
            BinIdentifier.make "B003"
            BinIdentifier.make "B004"
            BinIdentifier.make "B005"
            BinIdentifier.make "B006"
        ]
        |> List.sequenceResultM
        |> unsafeResult
    let products =
        [
            PartNumber.make "1000-1000-1000"
            PartNumber.make "2000-1000-1000"
        ]
        |> List.sequenceResultM
        |> unsafeResult
    {
        Bins = Set.ofList bins
        Content =
            Map.ofList [
                bins.[1], products.[0]
                bins.[3], products.[1]
            ]
        BinStructure = Map.empty
    }

// Public API

type SimulatedDatabaseError =
    | BinAlreadyStored

let retrieveBins () : Set<BinIdentifier> =
    stockData.Bins

let retrieveStock () : Map<BinIdentifier, PartNumber> =
    stockData.Content

let storeBin (bin : Bin) =
    if Set.contains bin.Identifier stockData.Bins then
        Error BinAlreadyStored
    else
        stockData <-
            { stockData with
                Bins = stockData.Bins |> Set.add bin.Identifier
                Content =
                    match bin.Content with
                    | Some product -> stockData.Content |> Map.add bin.Identifier product
                    | None -> stockData.Content
            }
        Ok ()