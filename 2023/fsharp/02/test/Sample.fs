module Day02.Tests

open Expecto

[<Tests>]
let tests =
    testList "Functions" [
        testList "parseGameLine" [
            let testData = [
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                {
                    index = 1u
                    samples = [
                        { blue = 3u; red = 4u; green = 0u }
                        { blue = 6u; red = 1u; green = 2u }
                        { blue = 0u; red = 0u; green = 2u }
                    ]
                }
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                {
                    index = 2u
                    samples = [
                        { blue = 1u; red = 0u; green = 2u }
                        { blue = 4u; red = 1u; green = 3u }
                        { blue = 1u; red = 0u; green = 1u }
                    ]
                }
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                {
                    index = 3u
                    samples = [
                        { blue = 6u; red = 20u; green = 8u }
                        { blue = 5u; red = 4u; green = 13u }
                        { blue = 0u; red = 1u; green = 5u }
                    ]
                }
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                {
                    index = 4u
                    samples = [
                        { blue = 6u; red = 3u; green = 1u }
                        { blue = 0u; red = 6u; green = 3u }
                        { blue = 15u; red = 14u; green = 3u }
                    ]
                }
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
                {
                    index = 5u
                    samples = [
                        { blue = 1u; red = 6u; green = 3u }
                        { blue = 2u; red = 1u; green = 2u }
                    ]
                }
            ]

            for (input, expected) in testData do
                testCase input
                <| fun _ ->
                    let actual = parseGameLine input
                    Expect.equal actual expected "Expected a different result"
        ]
        testList "getSmallestPossibleSet" [
            let testData = [
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                { blue = 6u; red = 4u; green = 2u }
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                { blue = 4u; red = 1u; green = 3u }
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                { blue = 6u; red = 20u; green = 13u }
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                { blue = 15u; red = 14u; green = 3u }
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
                { blue = 2u; red = 6u; green = 3u }
            ]

            for (input, expected) in testData do
                testCase input
                <| fun _ ->
                    let actual = input |> parseGameLine |> getSmallestPossibleSet
                    Expect.equal actual expected "Expected a different result"
        ]
    ]
