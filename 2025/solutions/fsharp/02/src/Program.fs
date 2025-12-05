module Day02

open System

// Helpers
module String =
    let split (separator: char) (input: string) = input.Split separator |> Array.toList

// Domain Model
type Id = Id of int64

module Id =
    let value (Id v) = v
    let ofString (s: string) = Id(Int64.Parse s)

// Script
open System.IO

let readInput filePath =
    File.ReadLines filePath
    |> Seq.head
    |> String.split ','
    |> List.map (String.split '-' >> fun [ part1; part2 ] -> Id.ofString part1, Id.ofString part2)

let part1 ranges =
    ranges
    |> List.collect (fun (Id from, Id til) ->
        seq { from..til }
        |> Seq.filter (fun x ->
            string x
            |> function
                | s when s.Length % 2 = 0 -> s.[0 .. (s.Length / 2 - 1)] = s.[(s.Length / 2) ..]
                | _ -> false)
        |> Seq.toList)
    |> List.sum

let part2 ranges =
    let checkForRepetition (s: string) =
        let length = s.Length

        seq { 1 .. length / 2 }
        |> Seq.exists (fun segmentSize ->
            if length % segmentSize <> 0 then
                false
            else
                let segment = s.[0 .. segmentSize - 1]

                seq { 0..segmentSize .. (length - 1) }
                |> Seq.forall (fun startIndex ->
                    s.[startIndex .. startIndex + segmentSize - 1] = segment))

    ranges
    |> List.collect (fun (Id from, Id til) ->
        seq { from..til } |> Seq.filter (string >> checkForRepetition) |> Seq.toList)
    |> List.sum

[<EntryPoint>]
let main args =
    if args.Length <> 1 then
        failwith "Please provide input file as only argument"

    let input = readInput args[0]

    part1 input |> printfn "Part 1: %d"
    part2 input |> printfn "Part 2: %d"
    0
