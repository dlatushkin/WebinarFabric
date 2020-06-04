// Learn more about F# at http://fsharp.org

open System
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.ServerErrors
open Suave.Writers
open Newtonsoft.Json

open AgentUtilities

type NewAnswer =
  {
    Text: string
  }

type Answer =
  {
    Text: string
    AnswerId: int
  }

let getString(rawForm: byte[]) =
  System.Text.Encoding.UTF8.GetString(rawForm)

let fromJson<'a> json =
  JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a

let getAnswerFromDb id =
  { Text = "Sample Text"; AnswerId = id }

let createAnswerDb (newAnswer: NewAnswer) =
  { Text = newAnswer.Text; AnswerId = 10 }

let updateAnswerDb answer =
  answer

let deleteAnswerFromDb id = 
  true

let getAnswer id =
  getAnswerFromDb id
  |> JsonConvert.SerializeObject
  |> OK
  >=> setMimeType "application/json"

let createAnswer =
  request(fun r ->
    r.rawForm
    |> getString
    |> fromJson<NewAnswer>
    |> createAnswerDb
    |> JsonConvert.SerializeObject
    |> CREATED)
  >=> setMimeType "application/json"

let updateAnswer =
  request(fun r ->
    r.rawForm
    |> getString
    |> fromJson<Answer>
    |> updateAnswerDb
    |> JsonConvert.SerializeObject
    |> OK)
  >=> setMimeType "application/json"

let deleteAnswer id =
  let successful = deleteAnswerFromDb id
  if successful then
    NO_CONTENT
  else
    INTERNAL_ERROR "Couldn't delete resource"

let app = 
  choose
    [ GET >=> choose
        [ path "/" >=> OK "Hello World"
          pathScan "/answer/%d" (fun id -> getAnswer id) ]
      POST >=> choose
        [ path "/answer" >=> createAnswer ]
      PUT >=> choose
        [ path "/answer" >=> updateAnswer ]
      DELETE >=> choose
        [ pathScan "/answer/%d" (fun id -> deleteAnswer id) ]
    ]
    
[<EntryPoint>]
let main argv =

  let railway = Railways.buildRailway()
  let railwayAgent = Railways.buildRailwayAgent Railways.calculateRailway railway

  let basicHandler _ =
    let now = DateTime.Now
    printfn "tick %A" now
    railwayAgent.Post(Railways.Tick(now))

  let basicTimer = TimerUtilities.createTimer 1000 basicHandler
  Async.Start basicTimer

  startWebServer defaultConfig app

  printfn "Hello World from F#!"
  0 // return an integer exit code
