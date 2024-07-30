namespace Tests

open Expecto
open System.IO

type Config<'Input> = {
    day: int
    part1: 'Input -> string
    part2: 'Input -> string
    readInput: string -> 'Input
}

[<RequireQualifiedAccess>]
module Tests =
    let build<'Input> (config: Config<'Input>) =
        sprintf "../../inputs/%02i" config.day
        |> Directory.EnumerateFiles
        |> Seq.filter (fun filePath ->
            let fileName = Path.GetFileName filePath
            fileName.StartsWith "input-" && fileName.EndsWith ".txt")
        |> Seq.collect (fun filePath ->
            // Fetch input identifier
            let fileName = Path.GetFileName filePath
            let pattern = @"input-(.*).txt"

            let id =
                System.Text.RegularExpressions.Regex.Match(fileName, pattern).Groups.[1].Value

            // Fetch expected output
            let outputFilePath = sprintf "../../outputs/%02i/output-%s.txt" config.day id

            if File.Exists outputFilePath then
                let part1Output, part2Output =
                    match File.ReadAllLines outputFilePath |> Array.filter ((<>) "") with
                    | [| part1; part2 |] -> part1, part2
                    | _ ->
                        failwith
                            $"Expected output file {outputFilePath} to contain exactly two lines"

                [
                    if part1Output <> "-" then
                        test $"Input: {fileName} - Part 1" {
                            let input = config.readInput filePath

                            Expect.equal
                                (config.part1 input |> string)
                                part1Output
                                "Part 1 result wrong"
                        }

                    if part2Output <> "-" then
                        test $"Input: {fileName} - Part 2" {
                            let input = config.readInput filePath

                            Expect.equal
                                (config.part2 input |> string)
                                part2Output
                                "Part 2 result wrong"
                        }
                ]
            else
                printfn "Warning: No matching output file for %s" fileName
                [])
        |> Seq.toList
        |> testList $"Day {config.day}"
