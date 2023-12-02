module Day01.Tests

open Expecto

[<Tests>]
let tests =
    testList "Functions" [
        testList "extractFirstAndLastDigit" [
            let testData = [ "1abc2", 12; "pqr3stu8vwx", 38; "a1b2c3d4e5f", 15; "treb7uchet", 77 ]

            for (input, expected) in testData do
                testCase input
                <| fun _ ->
                    let actual = extractFirstAndLastDigit input
                    Expect.equal actual expected (sprintf "Expected %d but got %d" expected actual)
        ]
        testList "replaceStringByDigit" [
            let testData = [
                "two1nine", "219"
                "eightwothree", "823"
                "abcone2threexyz", "123"
                "xtwone3four", "2134"
                "4nineeightseven2", "49872"
                "zoneight234", "18234"
                "7pqrstsixteen", "76"
            ]

            for (input, expected) in testData do
                testCase input
                <| fun _ ->
                    let actual = replaceStringByDigit input
                    Expect.equal actual expected (sprintf "Expected %s but got %s" expected actual)
        ]
    ]
