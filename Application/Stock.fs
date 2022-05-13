/// Provides functionality (use-cases) related to stock (mostly bins) stored in the Storage Machine.
module StorageMachine.Stock.Stock

open StorageMachine
open Common
open Bin
open Stock

/// Defines data access operations for stock functionality.
type IStockDataAccess =

    /// Retrieve all bins currently stored in the Storage Machine.
    abstract RetrieveAllBins : unit -> List<Bin>

/// An overview of all bins currently stored in the Storage Machine.
let binOverview (dataAccess : IStockDataAccess) : List<Bin> =
    // Trivially
    dataAccess.RetrieveAllBins ()

/// An overview of actual stock currently stored in the Storage Machine. Actual stock is defined as all non-empty bins.
let stockOverview (dataAccess : IStockDataAccess) : List<Bin> =
    // Perform I/O
    let allBins = dataAccess.RetrieveAllBins ()
    // Use the model which provides the definition of a bin being (non-)empty
    let actualStock = allBins |> List.filter Bin.isNotEmpty
    actualStock

/// An overview of all products in stock consists of all unique products and their total quantity stored in the Storage
/// Machine.
type ProductsOverview = Set<Product * Quantity>

/// An overview of all products stored in the Storage Machine, regardless what bins contain them.
let productsInStock ``what parameters are needed here?`` : ProductsOverview =
    // Use the model
    let products = allProducts (failwith "Exercise 0: Fill this in.")
    products
    |> failwith "Exercise 0: Complete this implementation."