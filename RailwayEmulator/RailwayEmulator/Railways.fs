module Railways

open System

  type Station = {
    Code: string
    Name: string
    FromPoint: float
    ToPoint: float
  }

  type TrainStation = {
    Station: Station
    Moment: Option<DateTime>
  }
  
  type Train = {
    Number: string
    Point: float
    Direction: float
    PrevStation: Option<TrainStation>
    CurrStation: Option<TrainStation>
    NextStation: Option<TrainStation>
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
                { Code = "OSL"; Name = "Oslo"; FromPoint = 10.5; ToPoint = 12.5 }
                { Code = "PAR"; Name = "Paradise"; FromPoint = 20.5; ToPoint = 22.5 }
                { Code = "STV"; Name = "Stavanger"; FromPoint = 90.5; ToPoint = 92.5 }
              |]
            Trains =
              [|
                { Number = "7001"; Point = 0.5; Direction = 1.0; PrevStation = None; CurrStation = None; NextStation = None }
                { Number = "7002"; Point = 75.5; Direction = 1.0; PrevStation = None; CurrStation = None; NextStation = None  }
                { Number = "7003"; Point = 50.5; Direction = -1.0; PrevStation = None; CurrStation = None; NextStation = None  }
              |]
          }
        |]
    }

  let getNextStationIdx (line:Line, station:Station, direction:float) =
    let stationIndex = line.Stations |> Array.tryFindIndex(fun e -> e = station)
    let stationCount = line.Stations.Length
    match stationIndex with
    | Some si ->
        match direction with
        | d when d > 0.0 ->
            match si with
            | i when i = stationCount - 1 -> stationCount - 1
            | i -> i + 1
        | _ ->
            match si with
            | 0 -> 0
            | i -> i - 1
        | _ -> 1
    | None -> failwithf "is out of range"

  let getNextStationByCurrStation (line:Line, station:Station, direction:float) =
      let nextStationIndex = getNextStationIdx (line, station, direction)
      line.Stations.[nextStationIndex]

  let getStationLimits (line:Line) =
    let first = line.Stations |> Array.tryHead
    let last = line.Stations |> Array.tryLast
    match first, last with 
    | (Some f, Some l) -> (f.FromPoint, l.ToPoint) 
    | _, _ -> (0.0, line.Length)

  let getNextStation(line:Line, point:float, direction:float) =
    let fromPoint, toPoint = getStationLimits line
    match point with
    | p when p < fromPoint -> line.Stations |> Array.tryHead
    | p when p > toPoint  -> line.Stations |> Array.tryLast
    | p ->
      match direction with
      | d when d >= 0.0 -> line.Stations |> Array.tryFind(fun e -> e.FromPoint > p)
      | _ -> line.Stations |> Array.rev |> Array.tryFind(fun e -> e.ToPoint < p)

  let getCurrStation(line:Line, point:float, direction:float) =
    let fromPoint, toPoint = getStationLimits line
    match point with
    | p when p < fromPoint -> None
    | p when p > toPoint -> None
    | p ->
      match direction with
      | d when d >= 0.0 -> line.Stations |> Array.tryFind(fun e -> e.FromPoint <= p && p <= e.ToPoint)
      | _ -> line.Stations |> Array.rev |> Array.tryFind(fun e -> e.ToPoint >= p && p >= e.FromPoint )

  let getPrevStation(line:Line, point:float, direction:float) =
    let fromPoint, toPoint = getStationLimits line
    match point with
    | p when p < fromPoint -> line.Stations |> Array.tryHead
    | p when p > toPoint  -> line.Stations |> Array.tryLast
    | p ->
      match direction with
      | d when d >= 0.0 -> line.Stations |> Array.rev |> Array.tryFind(fun e -> e.ToPoint < p)
      | _ -> line.Stations |> Array.tryFind(fun e -> e.FromPoint > p)

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
              | _ -> prevTrain.Direction
          } )
        |> Seq.map(fun train ->
          let currStation = getCurrStation(prevLine, train.Point, train.Direction);
          let currTrainStation = match currStation with | Some ts -> Some { Station = ts; Moment = Some now } | _ -> None;

          let nextStation =
            match currStation with
            | Some s -> 
              let nextStationByCurrStation = getNextStationByCurrStation(prevLine, s, train.Direction)
              Some nextStationByCurrStation
            | None -> getNextStation(prevLine, train.Point, train.Direction)

          let nextTrainStation = match nextStation with | Some ts -> Some { Station = ts; Moment = None } | _ -> None;
          {
            train with
            PrevStation = match train.CurrStation, currTrainStation with
                          | (Some, None) ->  train.CurrStation
                          | _ -> train.PrevStation
            CurrStation = currTrainStation
            NextStation = nextTrainStation
          })
        |> Seq.toArray } )
      |> Seq.toArray }
    nextRailway
