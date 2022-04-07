/// This module simulates the stock database of the Storage Machine using an in-memory data structure.
module StorageMachine.SimulatedDatabase

open FsToolkit.ErrorHandling
open StorageMachine.Stock
open Common
open Bin

type ParentBin = BinIdentifier
type NestedBins = NestedBins of BinIdentifier * List<BinIdentifier>

/// Simulates DB tables which store stock data.
type private StockData = {
    Bins : Set<BinIdentifier>
    Content : Map<BinIdentifier, PartNumber>
    BinStructure : Map<ParentBin, NestedBins>
}

/// A helper function which "unpacks" the 'Result' value assuming it is 'Ok'. Obviously unsafe, therefore private.
let private unsafeResult r =
    match r with
    | Ok a -> a
    | Error _ -> failwith "Attempted to use a Result value which was Error"

/// Predefined in-memory stock data which can be changed.
let mutable private stockData : StockData =
    let bins =
        [
            BinIdentifier.make "B001" // 0
            BinIdentifier.make "B002" // 1
            BinIdentifier.make "B003" // 2
            BinIdentifier.make "B004" // 3
            BinIdentifier.make "B005" // 4
            BinIdentifier.make "B006" // 5
        ]
        |> List.sequenceResultM
        |> unsafeResult
    let products =
        [
            PartNumber.make "1000-1000-1000"
            PartNumber.make "2000-1000-1000"
            PartNumber.make "3000-1000-1000"
        ]
        |> List.sequenceResultM
        |> unsafeResult
    {
        Bins = Set.ofList bins
        Content =
            Map.ofList [
                bins.[1], products.[0]
                bins.[3], products.[1]
                bins.[4], products.[2]
            ]
        BinStructure =
            Map.ofList [
                bins.[0], NestedBins (bins.[1], [bins.[2]])
                bins.[1], NestedBins (bins.[3], [])
                bins.[2], NestedBins (bins.[4], [bins.[5]])
            ]
    }

// Public API

type SimulatedDatabaseError =
    | BinAlreadyStored

/// Retrieves all bins from the simulated Storage Machine DB. These are all bins currently stored.
let retrieveBins () : Set<BinIdentifier> =
    stockData.Bins

/// Retrieves stock for all bins currently stored in the Storage Machine. If the map does not contain a key, then the
/// correspond bin does not contain a product (or is not stored in the Machine at all).
let retrieveStock () : Map<BinIdentifier, PartNumber> =
    stockData.Content

/// Stores a bin in the Storage Machine. It is not allowed to store the same bin "twice".
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

/// Retrieves the encoding of how all the bins currently stored in Storage Machine DB are nested in each other.
let retrieveBinNesting () : Map<ParentBin, NestedBins> =
    stockData.BinStructure