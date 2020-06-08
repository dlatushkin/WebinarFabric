module FabricClient

open Flurl
open Flurl.Http

 let postTrainPositions obj =
  "http://dxwpc:8971/api/train-positions".PostJsonAsync(obj)
