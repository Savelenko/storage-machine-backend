/// Data access implementation of the Stock component.
module StorageMachine.Stock.Stock

open StorageMachine
open Bin
open Stock

/// Data access operations of the Stock component implemented using the simulated in-memory DB.
let stockPersistence = { new IStockDataAccess with
    
    member this.RetrieveAllBins () =
        SimulatedDatabase.retrieveBins ()
        |> Set.map (fun binIdentifier ->
            {
                Identifier = binIdentifier
                Content = SimulatedDatabase.retrieveStock () |> Map.tryFind binIdentifier
            }
        )
        |> Set.toList

}