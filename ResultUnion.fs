namespace backend

open System
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Mvc

type ResultUnion =
    | OkResult
    | OkObjectResult of obj
    | CreatedResult of location: string * obj
    | NoContentResult
    | BadRequestResult
    | BadRequestObjectResult of obj

    interface IActionResult with
        override this.ExecuteResultAsync(context) =
            match this with
            | OkResult -> Mvc.OkResult().ExecuteResultAsync context
            | OkObjectResult object ->
                Mvc
                    .OkObjectResult(object)
                    .ExecuteResultAsync(context)
            | CreatedResult (location, object) ->
                Mvc
                    .CreatedResult(location, object)
                    .ExecuteResultAsync context
            | NoContentResult -> Mvc.NoContentResult().ExecuteResultAsync context
            | BadRequestResult -> Mvc.BadRequestResult().ExecuteResultAsync context
            | BadRequestObjectResult object ->
                Mvc
                    .BadRequestObjectResult(object)
                    .ExecuteResultAsync context
