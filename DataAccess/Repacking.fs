/// Data access implementation of the Repacking component.
module StorageMachine.Repacking.Repacking

open StorageMachine
open Common
open BinTree
open SimulatedDatabase
open Repacking

/// A helper for constructing an actual bin tree from the low-level encoding of bin nesting in the simulated DB.
let private binTree outerBin : Option<BinTree> =
    let bins = retrieveBins ()
    let binStructure = retrieveBinNesting ()
    let products = retrieveStock ()
    
    let rec go outerBin =
        // Locate all inner bins of the outer bin
        let innerBins =
            match binStructure |> Map.tryFind outerBin with
            | None -> []
            | Some (NestedBins (oneBin, more)) -> oneBin :: more
        // The outer bin may or may not contain a product
        let product = products |> Map.tryFind outerBin |> Option.map Product
        // Combine the outer bin, its optional product and sub-trees into a tree node
        Bin (outerBin, (Option.toList product) @ (List.map go innerBins))

    if Set.contains outerBin bins then Some (go outerBin) else None

/// Data access operations of the Repacking component implemented using the simulated in-memory DB.
let binTreeDataAccess = { new IBinTreeDataAccess with

    member this.RetrieveBinTree outerBin = binTree outerBin
    
}