namespace backend.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open backend

[<ApiController>]
[<Route("api/[controller]")>]
type UserController(logger: ILogger<UserController>) =
    inherit ControllerBase()

    static let users = List<User>()

    [<HttpGet>]
    member _.GetById([<FromQuery>] id: string) =
        match Guid.TryParse id with
        | true, guid ->
            match Seq.tryFind (fun user -> user.Id = guid) users with
            | Some user -> OkObjectResult user :> IActionResult
            | None -> BadRequestResult() :> IActionResult
        | false, _ -> BadRequestResult() :> IActionResult

    [<HttpGet("list")>]
    member _.GetAll() = users

    [<HttpPost>]
    member _.Create(request: UserRequest) =
        let id = Guid.NewGuid()

        users.Add
            { Id = id
              Name = request.Name
              Age = request.Age }

        CreatedResult("dummy", {| Id = id |})

    [<HttpDelete>]
    member _.DeleteById(request: {| Id: string |}) =
        match Guid.TryParse request.Id with
        | true, guid ->
            if 0 < users.RemoveAll(fun x -> x.Id = guid) then
                NoContentResult() :> IActionResult
            else
                BadRequestResult() :> IActionResult
        | false, _ -> BadRequestResult() :> IActionResult
