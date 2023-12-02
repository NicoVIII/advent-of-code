module Day02.Program

open System.IO

let input = File.ReadLines "input.txt" |> Seq.map parseGameLine

// Part 1
input
|> Seq.filter (isGamePossible { blue = 14u; red = 12u; green = 13u })
|> Seq.sumBy (fun game -> game.index)
|> printfn "Part 1: %i"

// Part 2
input
|> Seq.sumBy (getSmallestPossibleSet >> getSetPower)
|> printfn "Part 2: %i"
