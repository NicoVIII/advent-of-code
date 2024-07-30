namespace Day08

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllText >> parseInput

    let part1 (instructions: Instruction seq, nodeMap) =
        let mutable currentNode = "AAA"
        let mutable counter = 0
        let enumerator = instructions.GetEnumerator()

        while currentNode <> "ZZZ" do
            enumerator.MoveNext() |> ignore
            let (left, right) = Map.find currentNode nodeMap

            currentNode <-
                match enumerator.Current with
                | Left -> left
                | Right -> right

            counter <- counter + 1

        counter

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> printfn "Part 1: %i"
        //part2 input |> printfn "Part 2: %i"
        0
