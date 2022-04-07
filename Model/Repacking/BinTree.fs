module StorageMachine.Repacking.BinTree

open StorageMachine
open Common

type BinTree =
    | Bin of BinIdentifier * List<BinTree>
    | Product of PartNumber

let rec productCount binTree =
    match binTree with
    | Bin (_, productsOrBins) -> List.sumBy productCount productsOrBins
    | Product _ -> 1
