namespace backend

open System

type User = { Id: Guid; Name: string; Age: int }

type UserRequest = { Name: string; Age: int }
