namespace Day01

[<RequireQualifiedAccess>]
module String =
    let contains (text: string) (pattern: string) = text.Contains(pattern)

[<AutoOpen>]
module Functions =
    let isDigit c = c >= '0' && c <= '9'
    let charToInt (c: char) = int c - int '0'

    let extractFirstAndLastDigit line =
        // Extract first and last digit
        let digits = Seq.filter isDigit line |> Seq.toList
        let first = digits |> List.head |> charToInt
        let last = digits |> List.last |> charToInt
        first * 10 + last

    let replaceStringByDigit line =
        let config = [
            ("one", "1")
            ("two", "2")
            ("three", "3")
            ("four", "4")
            ("five", "5")
            ("six", "6")
            ("seven", "7")
            ("eight", "8")
            ("nine", "9")
        ]

        line
        |> Seq.fold
            (fun (digitString, text) char ->
                if isDigit char then
                    // We found a digit, add it to the digitString and reset the search text
                    digitString + string char, ""
                else
                    let text = text + string char
                    // We try to find a pattern in the config that matches the text
                    List.tryFind (fst >> String.contains text) config
                    |> function
                        | Some(pattern, digit) ->
                            // We found a pattern and add the digit to the digitString and shorten the text to no longer include the pattern
                            digitString + digit, text.Substring(text.Length - pattern.Length + 1)
                        | None -> digitString, text)
            ("", "")
        |> fst
