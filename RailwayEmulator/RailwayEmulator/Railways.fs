module Railways

open System
  
  type Train = {
    Number: string
    Point: float
    Direction: float
  }
  
  type Line = {
    Id: string
    Name: string
    Length: float
    Trains: Train[]
  }
  
  type Railway = {
      Lines: Line[]
    }
  
  type RailwayMessage =
    | Tick of DateTime
    | Get of AsyncReplyChannel<Railway>
  
  let buildRailwayAgent applyMessage initialRailway =
    MailboxProcessor<RailwayMessage>.Start(fun inbox ->
      let rec loop railway = async {
        let! msg = inbox.Receive()
  
        match msg with
        | Tick now ->
          let newRailway = applyMessage railway now
          return! loop newRailway
        | Get channel ->
          channel.Reply railway
          return! loop railway
      }
  
      loop initialRailway
    )
  
  let tick (agent: MailboxProcessor<RailwayMessage>) msg = Tick msg |> agent.Post
  
  let getState (agent: MailboxProcessor<RailwayMessage>) = agent.PostAndReply Get
  
  let getStateAsync (agent: MailboxProcessor<RailwayMessage>) = agent.PostAndAsyncReply Get

  let buildRailway() =
    {
      Lines = 
        [| 
          {
            Id = "L01"
            Name = "1st Line"
            Length = 10.0
            Trains =
              [|
                { Number = "7001"; Point = 0.0; Direction = 1.0 }
                { Number = "7002"; Point = 10.0; Direction = 1.0 }
              |]
          }
        |]
    }

  let calculateRailway (prevRailway:Railway) (now:DateTime) =
    printfn "calculateRailway %A" now
    let nextRailway = { Lines = prevRailway.Lines
      |> Seq.map (fun prevLine -> { prevLine with Trains =
        prevLine.Trains
        |> Seq.map (fun prevTrain ->
          {
            prevTrain with
            Point = 
              match prevTrain.Point with
              | p when p > prevLine.Length -> prevLine.Length
              | p when p < 0.0 -> 0.0
              | p -> p + prevTrain.Direction
            Direction = 
              match prevTrain.Point with
              | p when p > prevLine.Length -> -1.0
              | p when p < 0.0 -> 1.0
              | p -> prevTrain.Direction
          } )
        |> Seq.toArray } )
      |> Seq.toArray }
    nextRailway
