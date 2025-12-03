module Day01

// Domain Model
type Dial = private Dial of int16

module Dial =
    let create (value: int16) : Dial =
        if value < 0s || value > 99s then
            failwith "Dial value must be between 0 and 99"

        Dial value

    let value (Dial v) = v

    let turnLeft (Dial v) x =
        let newValue = (v + x) % 100s
        Dial newValue

    let turnRight (Dial v) x =
        let newValue = (v - x) % 100s
        Dial newValue

type Instruction =
    | TurnLeft of int16
    | TurnRight of int16

// Script
open System
open System.IO

let readInput filePath =
    let parseLine (line: string) : Instruction =
        let letter = line[0]
        let number = Int16.Parse(line[1..])

        match letter, number with
        | 'L', x -> TurnLeft x
        | 'R', x -> TurnRight x
        | _ -> failwith $"Invalid instruction: {line}"

    File.ReadLines filePath
    |> Seq.filter ((=) String.Empty >> not)
    |> Seq.map parseLine
    |> Seq.toList

let part1 instructions =
    let rec followInstructions zeros instructions dial =
        // Calc zeros
        let newZeros = if Dial.value dial = 0s then zeros + 1 else zeros
        // Partially bind new zeros to pass it consistently
        let followInstructions = followInstructions newZeros

        match instructions with
        | [] -> zeros
        | TurnLeft x :: rest ->
            let newDial = Dial.turnLeft dial x
            followInstructions rest newDial
        | TurnRight x :: rest ->
            let newDial = Dial.turnRight dial x
            followInstructions rest newDial

    followInstructions 0 instructions (Dial.create 50s)

let part2 instructions =
    let rec followInstruction zeros instruction dial =
        // Calc zeros
        let newDial =
            match instruction with
            | TurnLeft _ -> Dial.turnLeft dial 1s
            | TurnRight _ -> Dial.turnRight dial 1s

        let newZeros = if Dial.value newDial = 0s then zeros + 1 else zeros

        match instruction with
        | TurnLeft 1s
        | TurnRight 1s -> newZeros, newDial
        | TurnLeft x -> followInstruction newZeros (TurnLeft(x - 1s)) newDial
        | TurnRight x -> followInstruction newZeros (TurnRight(x - 1s)) newDial

    let rec followInstructions zeros instructions dial =
        match instructions with
        | [] -> zeros
        | inst :: rest ->
            let newZeros, newDial = followInstruction zeros inst dial
            followInstructions newZeros rest newDial

    followInstructions 0 instructions (Dial.create 50s)

[<EntryPoint>]
let main args =
    if args.Length <> 1 then
        failwith "Please provide input file as only argument"

    let input = readInput args[0]

    part1 input |> printfn "Part 1: %d"
    part2 input |> printfn "Part 2: %d"
    0
