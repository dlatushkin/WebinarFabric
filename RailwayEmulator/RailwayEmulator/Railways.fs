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
          let newRailway = applyMessage railway msg
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
  

