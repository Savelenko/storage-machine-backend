/// Provides functionality (use-cases) for nesting of bins and repacking of products within bins.
module StorageMachine.Repacking.Repacking

open StorageMachine
open Common
open BinTree

/// Defines data access operations for repacking functionality.
type IBinTreeDataAccess =

    /// Retrieve the bin tree for the given outer bin (identifier). Result is 'None' when the bin with the given
    /// identifier does not exist.
    abstract RetrieveBinTree : BinIdentifier -> Option<BinTree>

/// A trivial use-case for retrieving a tree of bins based on the identifier of the outer bin. Result is 'None' if the
/// outer bin does not exist.
let viewBinTree (dataAccess : IBinTreeDataAccess) (bin : BinIdentifier) : Option<BinTree> =
    dataAccess.RetrieveBinTree bin

/// Count all products contained in all bins of the identified bin tree. Result is 'None' when there is no bin tree
/// for the provided identifier.
let productCount (dataAccess : IBinTreeDataAccess) (bin : BinIdentifier) : Option<int> =
    let binTree = dataAccess.RetrieveBinTree bin
    binTree |> Option.map BinTree.productCount