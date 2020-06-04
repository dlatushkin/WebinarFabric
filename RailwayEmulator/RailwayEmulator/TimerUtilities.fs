module TimerUtilities

  let createTimer timeInterval eventHandler =
    let timer = new System.Timers.Timer(float timeInterval)
    timer.AutoReset <- true
    timer.Elapsed.Add eventHandler

    async {
      timer.Start()
      //do! Async.Sleep 6000
      //timer.Stop()
    }
