module StorageMachine.Repacking.Repacking

open StorageMachine
open Common
open BinTree

type IBinTreeDataAccess =

    abstract RetrieveBinTree : BinIdentifier -> Option<BinTree>

// A trivial use-case for retrieving a tree of bins based on the identifier of the outer bin. Result is 'None' if the
// outer bin does not exist.
let viewTree (dataAccess : IBinTreeDataAccess) (bin : BinIdentifier) : Option<BinTree> =
    dataAccess.RetrieveBinTree bin