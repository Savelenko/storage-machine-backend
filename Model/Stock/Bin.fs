module StorageMachine.Stock.Bin

open StorageMachine
open Common

type Bin = {
    Identifier : BinIdentifier
    Content : Option<PartNumber>
}
// TODO YURSAV: Explain in documentation that bins can be nested, but this module is specific to stock overview where
// nesting is not relevant and therefore not modeled.

let isEmpty bin =
    match bin with
    | { Content = None } -> true
    | _ -> false

let isNotEmpty = not << isEmpty