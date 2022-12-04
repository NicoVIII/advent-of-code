open System
open System.IO

[<Measure>]
type cal

type Elf = { calories: uint<cal> list }

let readInput () =
    // Read input file
    File.ReadLines "input.txt"
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

let inline sum_biggest x calories =
    match x with
    | 1 -> Seq.max calories
    | x when x > 1 -> calories |> Seq.sortDescending |> Seq.take x |> Seq.sum
    | _ -> failwith "x must be greater than 0"

// Prepare input
let calories = readInput () |> Seq.map (fun elf -> Seq.sum elf.calories) // Sum all calories per elf

// Part 1
sum_biggest 1 calories |> printfn "Calories - Part 1: %i"

// Part 2
sum_biggest 3 calories |> printfn "Calories - Part 2: %i"
