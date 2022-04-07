module StorageMachine.Stock.Stock

open StorageMachine
open Common
open Bin

type IStockPersistence =

    abstract RetrieveAllBins : unit -> List<Bin>

let binOverview (dataAccess : IStockPersistence) : List<Bin> =
    // Trivially
    dataAccess.RetrieveAllBins ()

let stockOverview (dataAccess : IStockPersistence) : List<Bin> =
    // Perform I/O
    let allBins = dataAccess.RetrieveAllBins ()
    // Use the model which provides the definition of a bin being (non-)empty
    let actualStock = allBins |> List.filter Bin.isNotEmpty
    actualStock