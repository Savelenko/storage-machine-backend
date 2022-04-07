module StorageMachine.Stock.Stock

open StorageMachine
open Bin
open Stock

let stockPersistence = { new IStockPersistence with
    
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