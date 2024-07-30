namespace Day08

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()
    let trimChars (chars: char array) (input: string) = input.Trim(chars)
    let toCharArray (input: string) = input.ToCharArray()

[<AutoOpen>]
module Functions =
    let parseInstructions =
        String.trim
        >> String.toCharArray
        >> Array.map Instruction.fromValue
        >> List.ofArray
        >> (fun list ->
            Seq.initInfinite (fun i ->
                let limitedI = i % List.length list
                list[limitedI]))

    let parseNodes: _ -> NodeMap =
        String.trim
        >> String.split "\n"
        >> Array.map (
            String.trim
            >> String.split " = "
            >> (fun [| node; destNodes |] ->
                let [| destNode1; destNode2 |] =
                    String.trimChars [| '('; ')' |] destNodes |> String.split ", "

                node, (destNode1, destNode2))
        )
        >> Map.ofArray

    let parseInput input =
        let parts = String.split "\n\n" input

        parseInstructions parts[0], parseNodes parts[1]
