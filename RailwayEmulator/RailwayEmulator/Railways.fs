module Railways

open System
  
  type Train = {
    Number: string
    Point: float
    Direction: float
  }

  type Station = {
    Code: string
    Name: string
    FromPoint: float
    ToPoint: float
  }
  
  type Line = {
    Id: string
    Name: string
    Length: float
    Trains: Train[]
    Stations: Station[]
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
            Length = 100.0
            Stations =
              [|
                { Code = "OSL"; Name = "Oslo"; FromPoint = 10.0; ToPoint = 12.0 }
              |]
            Trains =
              [|
                { Number = "7001"; Point = 0.5; Direction = 1.5 }
                { Number = "7002"; Point = 75.5; Direction = 1.5 }
                { Number = "7003"; Point = 50.5; Direction = -1.5 }
              |]
          }
        |]
    }

  let getNextStation (line:Line, station:Station, direction:float) =
    let stationIndex = line.Stations |> Array.tryFindIndex(fun e -> e = station)
    let isLastStation = line.Stations.Length - 1
    match stationIndex with
    | si -> si
    | si -> failwithf "%A is out of range" si


  let getStation(line:Line, point:float, direction:float) =
    match point, direction with
    | p, d when p < 0.0 -> line.Stations |> Array.tryHead, None, line.Stations |> Array.tryHead
    | p, d when p > line.Length -> line.Stations |> Array.tryLast, None, line.Stations |> Array.tryLast
    | p, d -> match d with
              | d when d > 0.0 -> None, None, None
              | d -> None, None, None

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
