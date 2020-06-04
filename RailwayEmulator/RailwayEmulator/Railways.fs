module Railways

open System
  
  type Train = {
    Number: string
  }
  
  type Line = {
    Id: string
    Name: string
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

  let buildRailway() = {
    Lines = [| { Id = "L01"; Name = "1st Line"; Trains = [| { Number = "7001"}; {Number = "7002" } |] } |]
  }

  let calculateRailway (prevRailway:Railway) (now:DateTime) =
    printfn "calculateRailway %A" now
    let nextRailway = { Lines = prevRailway.Lines
      |> Seq.map (fun prevLine -> { prevLine with Trains =
        prevLine.Trains
        |> Seq.map (fun prevTrain -> { prevTrain with Number = prevTrain.Number } )
        |> Seq.toArray } )
      |> Seq.toArray }
    nextRailway
