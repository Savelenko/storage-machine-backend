module StorageMachine.Stock.Stock

open StorageMachine
open Common
open Bin

type IStockDataAccess =

    abstract RetrieveAllBins : unit -> List<Bin>

let binOverview (dataAccess : IStockDataAccess) : List<Bin> =
    // Trivially
    dataAccess.RetrieveAllBins ()

let stockOverview (dataAccess : IStockDataAccess) : List<Bin> =
    // Perform I/O
    let allBins = dataAccess.RetrieveAllBins ()
    // Use the model which provides the definition of a bin being (non-)empty
    let actualStock = allBins |> List.filter Bin.isNotEmpty
    actualStock