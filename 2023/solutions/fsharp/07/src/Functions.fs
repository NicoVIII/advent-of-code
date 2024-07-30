namespace Day07

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()

[<AutoOpen>]
module Functions =
    let parseHand (handString: string) =
        CardValue.fromValue handString[0],
        CardValue.fromValue handString[1],
        CardValue.fromValue handString[2],
        CardValue.fromValue handString[3],
        CardValue.fromValue handString[4]

    let parseLine line : Hand * _ =
        let parts = String.split " " line
        parseHand parts[0], uint parts[1]

    let parseInput = String.split "\n" >> Array.map parseLine >> List.ofArray
