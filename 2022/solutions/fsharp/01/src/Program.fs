module Day01

open System
open System.IO

/// Calories
[<Measure>]
type cal

type Elf = { calories: uint<cal> list }

type Input = uint<cal> seq

module Input =
    let read inputFilePath =
        // Read input file
        File.ReadLines inputFilePath
        // Transform input into a useable form
        |> Seq.fold
            (fun (elfes, calories) line ->
                match line.Trim() with
                | "" ->
                    let elf = { calories = calories }
                    (Seq.append elfes [ elf ], [])
                | line ->
                    let calories_line = (UInt32.Parse line) * 1u<cal>
                    (elfes, calories_line :: calories))
            (Seq.empty, [])
        // Finish collection of last elf
        |> fun (elfes, calories) ->
            match calories with
            | [] -> elfes
            | calories ->
                let elf = { calories = calories }
                Seq.append elfes [ elf ]

    let prepare inputFilePath : Input =
        read inputFilePath |> Seq.map (fun elf -> Seq.sum elf.calories) // Sum all calories per elf

let inline sumBiggest x calories =
    match x with
    | 1 -> Seq.max calories
    | x when x > 1 -> calories |> Seq.sortDescending |> Seq.take x |> Seq.sum
    | _ -> failwith "x must be greater than 0"

let inline part1 input = sumBiggest 1 input
let inline part2 input = sumBiggest 3 input

[<EntryPoint>]
let main args =
    if args.Length <> 1 then
        failwith "Please provide the input file path as only argument"

    // Prepare input
    let calories = Input.prepare args[0]

    part1 calories |> printfn "Calories - Part 1: %i"

    // Part 2
    part2 calories |> printfn "Calories - Part 2: %i"

    0
