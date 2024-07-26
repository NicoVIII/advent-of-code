module Day02

open System.IO

type Input = string array

module Input =
    let read filePath = File.ReadAllLines filePath

type Hand =
    | Rock
    | Paper
    | Scissors

type Round = { enemy: Hand; me: Hand }

let parseInput lines =
    let parseEnemyHand =
        function
        | "A" -> Rock
        | "B" -> Paper
        | "C" -> Scissors
        | _ -> failwith "Invalid hand"

    let parseMyHand =
        function
        | "X" -> Rock
        | "Y" -> Paper
        | "Z" -> Scissors
        | _ -> failwith "Invalid hand"

    lines
    |> Seq.map (fun (line: string) ->
        let parts = line.Split ' '

        {
            enemy = parseEnemyHand parts.[0]
            me = parseMyHand parts.[1]
        })

let getResultScore round =
    match round.me, round.enemy with
    | Paper, Rock
    | Scissors, Paper
    | Rock, Scissors -> 6u // Win
    | Rock, Rock
    | Paper, Paper
    | Scissors, Scissors -> 3u // Draw
    | Rock, Paper
    | Paper, Scissors
    | Scissors, Rock -> 0u // Lose

let getHandScore hand =
    match hand with
    | Rock -> 1u
    | Paper -> 2u
    | Scissors -> 3u

let getScore round =
    let resultScore = getResultScore round
    let handScore = getHandScore round.me
    resultScore + handScore

type Result =
    | Win
    | Lose
    | Draw

type Round2 = { enemy: Hand; result: Result }

let parseInput2 lines =
    let parseEnemyHand =
        function
        | "A" -> Rock
        | "B" -> Paper
        | "C" -> Scissors
        | _ -> failwith "Invalid hand"

    let parseResult =
        function
        | "X" -> Lose
        | "Y" -> Draw
        | "Z" -> Win
        | _ -> failwith "Invalid result"

    lines
    |> Seq.map (fun (line: string) ->
        let parts = line.Split ' '

        {
            enemy = parseEnemyHand parts.[0]
            result = parseResult parts.[1]
        })

let transformRound2ToRound round =
    let myHand =
        match round.enemy, round.result with
        | Rock, Win
        | Paper, Draw
        | Scissors, Lose -> Paper
        | Rock, Draw
        | Paper, Lose
        | Scissors, Win -> Rock
        | Rock, Lose
        | Paper, Win
        | Scissors, Draw -> Scissors

    { enemy = round.enemy; me = myHand }

let part1 = parseInput >> Seq.map getScore >> Seq.sum

let part2 = parseInput2 >> Seq.map (transformRound2ToRound >> getScore) >> Seq.sum

[<EntryPoint>]
let main args =
    if args.Length <> 1 then
        failwith "Please provide the input file path as only argument"

    let input = Input.read args[0]

    // Part 1
    part1 input |> printfn "Score - Part 1: %i"

    // Part 2
    part2 input |> printfn "Score - Part 2: %i"

    0
