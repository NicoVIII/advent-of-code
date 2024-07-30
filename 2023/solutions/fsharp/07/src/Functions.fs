namespace Day07

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()

[<AutoOpen>]
module Functions =
    let parseHand fromValue (handString: string) : Hand<_> =
        fromValue handString[0],
        fromValue handString[1],
        fromValue handString[2],
        fromValue handString[3],
        fromValue handString[4]

    let parseHandV1: _ -> HandV1 = parseHand CardValueV1.fromValue
    let parseHandV2: _ -> HandV2 = parseHand CardValueV2.fromValue

    let parseLine parseHand line =
        let parts = String.split " " line
        parseHand parts[0], uint parts[1]

    let parseLineV1 = parseLine parseHandV1
    let parseLineV2 = parseLine parseHandV2

    let preParseInput = String.split "\n"
    let parseInput parseLine = Array.map parseLine >> List.ofArray
    let parseInputV1 = parseInput parseLineV1
    let parseInputV2 = parseInput parseLineV2
