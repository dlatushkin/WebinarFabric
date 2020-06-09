// Learn more about F# at http://fsharp.org

open System
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.ServerErrors
open Suave.Writers
open Newtonsoft.Json
open Flurl.Http
open AgentUtilities

let railway = Railways.buildRailway()
let railwayAgent = Railways.buildRailwayAgent Railways.calculateRailway railway


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

let getLines() =
  railwayAgent.PostAndReply(Railways.Get)
  |> JsonConvert.SerializeObject
  |> OK
  >=> setMimeType "application/json"

let getTopology() =
  let railway = railwayAgent.PostAndReply(Railways.Get)
  railway.Lines
  |> Array.map(fun l -> {| Line = {| Id = l.Id; Name = l.Name; Length = l.Length |}; Stations = l.Stations |})
  |> JsonConvert.SerializeObject
  |> OK
  >=> setMimeType "application/json"

let getFlatTrains() = 
  let railway = railwayAgent.PostAndReply(Railways.Get)
  railway.Lines
  |> fun lines -> seq { for l in lines do
                          for t in l.Trains do
                            yield {| Line = l; Train = t |} }

let getTrainPositions() =
  getFlatTrains()
  |> Seq.map(fun t -> {| LineId = t.Line.Id; Number = t.Train.Number; Point = t.Train.Point |})

let getTrainPositionsWebResponse() = 
  getTrainPositions()
  |> JsonConvert.SerializeObject
  |> OK
  >=> setMimeType "application/json"



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

let setCORSHeaders =
  addHeader  "Access-Control-Allow-Origin" "*" 
  >=> setHeader "Access-Control-Allow-Headers" "token" 
  >=> addHeader "Access-Control-Allow-Headers" "content-type" 
  >=> addHeader "Access-Control-Allow-Methods" "GET,POST,PUT" 

let basicHandler _ =
  let now = DateTime.Now
  let response = railwayAgent.Post(Railways.Tick(now))
  let trainPositions = getTrainPositions()
  "http://dxwpc:8971/api/train-positions".PostJsonAsync(trainPositions) |> Async.AwaitIAsyncResult |> ignore
  response

let tick() =
  let now = DateTime.Now
  railwayAgent.Post(Railways.Tick(now))
  let trainPositions = getTrainPositions()
  "http://dxwpc:8971/api/train-positions".PostJsonAsync(trainPositions) |> Async.AwaitIAsyncResult |> ignore
  NO_CONTENT

let app = 
  choose
    [ GET >=> 
        fun context ->
          context |> (
            setCORSHeaders
            >=> choose
              [ path "/" >=> OK "Hello World"
                path "/lines" >=> request (fun r -> getLines())
                path "/topology" >=> request(fun r -> getTopology())
                path "/trains" >=> request(fun r -> getTrainPositionsWebResponse())
                pathScan "/answer/%d" (fun id -> getAnswer id) ]
          )

      POST >=> 
        fun context -> 
          context |> (
            setCORSHeaders
            >=> choose
              [ path "/answer" >=> createAnswer
                path "/tick" >=> request(fun r -> tick()) ]
          )

      PUT >=> choose
        [ path "/answer" >=> updateAnswer ]
      DELETE >=> choose
        [ pathScan "/answer/%d" (fun id -> deleteAnswer id) ]
    ]
    
[<EntryPoint>]
let main argv =

  let basicTimer = TimerUtilities.createTimer 1000 basicHandler
  //Async.Start basicTimer

  startWebServer defaultConfig app

  printfn "Hello World from F#!"
  0 // return an integer exit code
