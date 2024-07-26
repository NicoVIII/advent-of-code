namespace Day04

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()

[<AutoOpen>]
module Functions =
    let parseInput (input: string seq) =
        let parseNumbers input =
            input |> String.trim |> String.split " " |> Set.ofArray |> Set.map uint

        let parseLine line =
            let firstSplit = String.split ":" line
            let secondSplit = String.split "|" firstSplit[1]

            {
                winning = parseNumbers secondSplit[0]
                youHave = parseNumbers secondSplit[1]
            }

        input |> Seq.map parseLine

    let getMatches (card: ScratchCard) =
        let { winning = winning; youHave = youHave } = card
        Set.intersect winning youHave |> Set.count

    let calculateWrongScore (card: ScratchCard) =
        let matches = getMatches card
        pown 2u (matches - 1)

    let simulateWins (cards: ScratchCard seq) =
        let indexedCards = cards |> Seq.indexed |> Seq.cache

        let mutable amountMap =
            indexedCards |> Seq.map (fun (index, _) -> index, 1u) |> Map.ofSeq

        let cardMap = indexedCards |> Map.ofSeq

        cardMap
        |> Map.fold
            (fun (cardTotal) index card ->
                let cardAmount = amountMap.Item index
                let matches = getMatches card
                // Increase amount of cards for each match
                for i in 1..matches do
                    let nextIndex = index + i

                    amountMap <- amountMap.Change(nextIndex, Option.map ((+) cardAmount))

                cardTotal + cardAmount)
            0u
