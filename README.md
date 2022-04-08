# Storage Machine exercises

## Introduction

Storage Machine is a small but realistic and rather complete back-end of a Web-application. It does not include the front-end part so you will use Postman or similar to interact with it.

The goal is to see an example of a complete system made using F# and get acquainted with the Onion architecture. You will do this by example functionalities and (architectural and FP) patterns already implemented in the back-end and extend it with new ones, by applying the same patterns.

## Resources

* As always, [F# language reference](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/).
* Storage Machine uses [Giraffe](https://github.com/giraffe-fsharp/Giraffe/blob/master/DOCUMENTATION.md) -- a library for creating Web-applications using F# as a thin layer on top of ASP.NET Core in FP style, unlike heavily OO-based ASP.NET Core model.
* For JSON serialization the [Thoth.Json](https://thoth-org.github.io/Thoth.Json/documentation/concept/introduction.html) library is used because it provides FP-oriented combinator style of JSON (de)serialization.

## Exercise A

> Extend the `Stock` component with functionality to store a new bin containing zero or one products.

Requirements and hints:

- Adding a bin should be possible using a `POST` request.
- After adding a bin, it should be visible in the bin and/or stock overview already implemented in by the same `Stock` component.
- Extend the data access interface in the Application layer.
- Define the use-case function in the Application layer.
- Is there anything missing in the Model layer or is it complete for this functionality?
- Extend the data access implementation object in the Data Access layer part of the `Stock` component.
- Make a `Decoder` for bins.
- Module `PostExample` in the Service layer contains an example of decoding (simple) JSON values from the body of a `POST` request and returing various responses.
- Existing functionality in the `Stock` component contains examples of serializing JSON responses.
- Make an `HttpHandler` for the handling the `POST` request and combine it with existing handlers of the `Stock` component.

## Exercise B

> Implement "product repacking" functionality in the `Repacking` component.

Requirements and hints:

- Bins can be nested in other bins, this is already modeled in the Model layer of the `Repacking` component.
- Products stored in bins ("bin trees") need to get protective packaging. Each individual product needs to be wrapped in a package, but the overall shape of the bin tree must remain the same. Storage Maching can magically mechanically do this, if you implement this repacking in F#.
- The `Repacking` component contains working functionality for viewing bin trees and counting products in a bin tree. Experiment with these.
- After repacking, the number of products in a bin tree should remain the same. Use this as a sanity check for your repacking implementation.
- Adjust the model to express the packaging requirement.
- Implement repacking as a bin tree transformation function.
- Add the repacking step to an existing use-case function in the Application layer of the `Repacking` component.