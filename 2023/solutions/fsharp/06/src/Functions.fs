namespace Day06

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()

[<AutoOpen>]
module Functions =
    let parseLineV1 line : Base array =
        // Remove title
        (String.split ":" line)[1]
        |> String.trim
        |> String.split " "
        |> Array.choose (fun x -> if x <> "" then Base.convertTo x |> Some else None)

    let parseInputV1 input =
        let lines = String.split "\n" input
        let times = parseLineV1 lines[0]
        let distances = parseLineV1 lines[1]

        Array.zip times distances
        |> Array.map (fun (time, distance) -> { time = time; distance = distance })
        |> List.ofArray

    let parseLineV2 line : Base =
        // Remove title
        (String.split ":" line)[1]
        |> String.trim
        |> String.filter (fun c -> c <> ' ')
        |> Base.convertTo

    let parseInputV2 input =
        let lines = String.split "\n" input

        {
            time = parseLineV2 lines[0]
            distance = parseLineV2 lines[1]
        }

    let beatRecordCount record : Base =
        // We can express the problem like this
        // distance < (x-y)(x+y) with y < x and x = time / 2
        // count how many ys element of integers are there which solve that
        // (x-y)(x+y) = x² - y²
        // Therefore we can simply find the biggest y for which this is valid and multiply by 2 (for odd x, for even x it is * 2 + 1)
        // distance < x² - y²
        // y² < x² - distance
        // y < sqrt (x² - distance)
        // biggest y = round-down (sqrt (x² - distance) - 0.0001) | we subtract a very small number to ensure, that we are not equal

        // for odd numbers we also have to add 0.5 to get correct results, because halfs of these numbers are not integers
        // (x + 0.5 - y)(x - 0.5 + y) = (x - (y - 0.5))(x + (y - 0.5)) = x² - (y - 0.5)²
        // distance < x² - (y - 0.5)²
        // (y - 0.5)² < x² - distance
        // y - 0.5 < sqrt (x² - distance)
        // y < sqrt (x² - distance) + 0.5
        // biggest y = round-down (sqrt (x² - distance) + 0.5 - 0.0001)
        let timeIsOdd = record.time % 2UL <> 0UL

        let biggestY =
            pown (float record.time / 2.) 2 - float record.distance
            |> sqrt
            |> if timeIsOdd then (+) 0.5 else id
            |> (+) -0.0001
            |> floor
            |> Base.convertTo

        if timeIsOdd then biggestY * 2UL else biggestY * 2UL + 1UL
