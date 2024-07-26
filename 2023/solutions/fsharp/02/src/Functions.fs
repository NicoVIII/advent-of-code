namespace Day02

[<RequireQualifiedAccess>]
module String =
    let contains (value: string) (text: string) = text.Contains(value)
    let split (separator: string) (text: string) = text.Split(separator)
    let substring (startIndex: int) (text: string) = text.Substring(startIndex)
    let trim (text: string) = text.Trim()

[<AutoOpen>]
module Functions =
    let parseGameLine line : Game =
        let parseIndex = String.substring 5 >> String.trim >> uint

        let parseSample text : CubeSet =
            String.split "," text
            |> Array.fold
                (fun set input ->
                    let input = String.trim input
                    let number = String.split " " input |> Array.head |> uint

                    match input with
                    | input when String.contains "blue" input -> { set with blue = number }
                    | input when String.contains "red" input -> { set with red = number }
                    | input when String.contains "green" input -> { set with green = number }
                    | _ -> failwith (sprintf "Invalid input '%s'" input))
                { blue = 0u; red = 0u; green = 0u }

        let parseSamples text =
            String.split ";" text |> Array.map parseSample |> Array.toList

        let parts = String.split ":" line

        {
            index = parseIndex parts[0]
            samples = parseSamples parts[1]
        }

    let isGamePossible set game =
        let isSamplePossible sample =
            sample.blue <= set.blue && sample.red <= set.red && sample.green <= set.green

        game.samples |> List.forall isSamplePossible

    let getSmallestPossibleSet game =
        game.samples
        |> List.fold
            (fun (blueMax, redMax, greenMax) sample ->
                let blueMax = max blueMax sample.blue
                let redMax = max redMax sample.red
                let greenMax = max greenMax sample.green

                (blueMax, redMax, greenMax))
            (0u, 0u, 0u)
        |> fun (blueMax, redMax, greenMax) -> {
            blue = blueMax
            red = redMax
            green = greenMax
        }

    let getSetPower set = set.blue * set.red * set.green
